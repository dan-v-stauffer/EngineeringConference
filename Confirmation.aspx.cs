using AppSecurity;
using ConferenceLibrary;
using DataUtilities;
using DataUtilities.SQLServer;
using DataUtilities.KTActiveDirectory;
using HelperFunctions;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;


public partial class Confirmation : System.Web.UI.Page
{
    EncryptDecryptQueryString security = EncryptDecryptQueryString.Instance;

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["autoRefreshReg"] = true;

        //USE CASES
        //1. User registering himself gets here from Registration.aspx 
        //      --> conditions: Session["rsvp"], Session["rsvpUser"], and Session["user"] exist; Session["rsvpUser"] = Session["user"]; encrypted qs exists
        //      --> action:verify qs user.login==rsvpUser.login and build new rsvp using Session["rsvpUser"] and load form
        //      --> action: query if user is the admin for other rsvp's if so, populate modal combobox with rsvp names, unhide "View Other Registrations"

        //2. User registering another user gets here from Registration.aspx
        //      --> conditions: Session["rsvp"], Session["rsvpUser"], and Session["user"] exist; Session["rsvpUser"] != Session["user"]; encrypted qs exists
        //      --> action: verify qs adminName==user.Login and user.email = rsvpUser.admin email and then load form

        //3. User comes here first with/without an existing registration. (i.e. bookmarked this page)
        //      --> conditions: Session["rsvp"], Session["rsvpUser"] == null, and Session["user"]  is not null; no encrypted qs exists
        //      --> however, can find query for existing rsvp
        //      --> action: see if rsvp exists for user.email. if exists, load form, else error message and redirect to Default.aspx
        //4. User comes here first - has registered oneo or more other users, and maybe themself
        // --> conditions: Session["rsvp"], Session["rsvpUser"], and Session["user"] all == null; no encrypted qs exists
        // --> however, can query for registrations in which they are admin
        // --> 
        // --> action: query database for any reservations 
        //5. User comes here first - has registered other users
        // --> conditions: Session["rsvp"], Session["rsvpUser"], and Session["user"] all == null; no encrypted qs exists
        // --> however, can query for registrations in which they are admin

        // get user
        if (!IsPostBack)
        {
            KTConferenceUser user = null;
            KTConferenceUser rsvpUser = null;

            if (Session["user"] == null)
                user = new KTConferenceUser(
                           new KTActiveDirectoryUser(
                               new KTLogin(System.Web.HttpContext.Current.User.Identity.Name)));
            else
                user = (KTConferenceUser)Session["user"];

            Conference conference = (Conference)Application["Conference"];
            string strReq = "";
            strReq = Request.RawUrl;
            strReq = strReq.Substring(strReq.IndexOf('?') + 1);
            string[] strUserName = null;

            try
            {
                string qs = security.Decrypt(strReq, user.Email.Substring(0, 8));

                if (qs.Contains('='))
                {
                    strUserName = qs.Split('=');
                    rsvpUser = new KTConferenceUser(strUserName[1]);
                }
            }
            catch (Exception ex)
            {
            }

            if (rsvpUser == null)
                rsvpUser = user;
                      
            //does user have a rsvp submitted
            //if yes, load form


            RSVP rsvp = new RSVP(rsvpUser, "Guest");
            rsvp.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(rsvp_PropertyChanged);
            Session["verifyRSVP"] = rsvp;
            //if no, error msg and redirect to Default.aspx
            DataTable adminRSVP = WebDataUtility.Instance.webAppTable("sp_GetRsvpsForAdmin", new GenericCmdParameter[] { 
            new GenericCmdParameter("@conferenceID", conference.ID),
            new GenericCmdParameter("@adminEmail", user.Email) });

            int adminCount = adminRSVP.Rows.Count;

            adminCount = 1;

            HtmlTableRow row = tr_adminView;

            row.Attributes["class"] = (adminCount > 0) ? "" : "hidden";

            Panel pnl_POC = (Panel)Page.Master.FindControl("pnl_POC");
            if (pnl_POC != null)
            {
                pnl_POC.CssClass = "hidden";
            }

            if (Request.QueryString["decline"] != null)
            {
                string declineType = Request.QueryString["decline"];
                if (declineType == "1")
                    loadForDecline();
                else
                    loadForCancellation();
            }
            else if (!rsvp.IsValid)
            {
                pnl_Header.CssClass = "errorHeader";
                pnl_HdrMessage.CssClass = "errorMsg";
                pnl_Icon.CssClass = "errorIcon";
                lbl_HeaderMsgTitle.Text = "Error. No registration found";
                lbl_HeaderMsg.Text = "There is no registration found for " + user.Email + ". Redirecting you to the  <a href='Default.aspx'> Registration Home Page</a>.";
                pnl_ConfirmationCode.Visible = false;
                if (adminCount == 0)
                    timer_Redirect.Interval = 5000;
            }
            else if(!rsvp.isCurrent)
            {
                loadForCancellation();
            }
            else
            {
                if (strUserName == null) // valid rsvp but not new
                {
                    lbl_HeaderMsgTitle.Text = "Your Registration is Complete";
                    lbl_HeaderMsg.Text = "You are currently registered. To change or have another email sent, please use the links below.";
                }
                else if (strUserName.Length > 1) //if true, then new RSVP
                {
                    lbl_HeaderMsgTitle.Text = "Your registration is complete.";
                    lbl_HeaderMsg.Text = "An email has been sent to " + rsvp.User.Email + ".<br>Your confirmation code is: " + rsvp.ConfirmationCode; ;
                }
                else
                {
                    lbl_HeaderMsgTitle.Text = "Your Registration";
                    lbl_HeaderMsg.Text = "You are currently registered. To change or have another email sent, please use the links below.";
                }
                loadForm(rsvp);
            }

            //is user an 'admin' for other rsvp's?
            //if yes, populate combo box

            //if no then hide listbutton (lb_ShowAdminRsvps)


            if (Session["rsvpUser"] == null)
            {


                //lbl_HeaderMsgTitle.Text = "

            }
        }
    }

    private void rsvp_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        Session["verifyRSVP"] = (RSVP)sender;
    }

    private void loadForm(RSVP rsvp)
    {
        KTConferenceUser ktUser = (KTConferenceUser)rsvp.User;
        lbl_Cancel.Text = string.Empty;
        cancel_form.CssClass = "hidden";
        lbl_ConfirmationCode.Text = "Confirmation Code: " + rsvp.ConfirmationCode;
        lbl_FullName.Text = ktUser.FullName;
        lbl_Division.Text = ktUser.Division;
        lbl_WorkLocation.Text = ktUser.HomeOffice;
        lbl_MobilePhone.Text = ktUser.MobilePhone;
        lbl_JobRole.Text = ktUser.JobRole;
        lbl_SpecialNeeds.Text = ktUser.SpecialNeeds;
        lbl_FoodRestrictions.Text = ktUser.FoodAllergies;
        lbl_HotelCheckin.Text = ((DateTime)rsvp.CheckInDate).Year==1900? "N/A": ((DateTime)rsvp.CheckInDate).ToString("dddd, MMMM d, yyyy");
        lbl_HotelCheckout.Text = ((DateTime)rsvp.CheckOutDate).Year == 1900 ? "N/A" : ((DateTime)rsvp.CheckOutDate).ToString("dddd, MMMM d, yyyy");
        lbl_HotelDuration.Text = Math.Floor((((DateTime)rsvp.CheckOutDate) - ((DateTime)rsvp.CheckInDate)).TotalDays).ToString("G") + " nights";
        lbl_ShirtSize.Text = ktUser.ShirtSize.Replace("M-", "Mens-").Replace("W-", "Womens-");

        if (rsvp.ContainsEvent(313))
        {
            pnl_SundayDeeopLearning.CssClass = "CheckBoxChecked";
            pnl_SundayDeeopLearning.ToolTip = "Selected";
        }
        else
        {
            pnl_SundayDeeopLearning.CssClass = "CheckBoxClear";
            hl_DeepLearningCalendar.Style["display"] = "none";
        }

        if (rsvp.Golfing)
        {
            pnl_Golfing.CssClass = "CheckBoxChecked";
            pnl_Golfing.ToolTip = "Selected";
        }
        else
        {
            pnl_Golfing.CssClass = "CheckBoxClear";
            pnl_Golfing.ToolTip = "Not Selected";
        }
        if (rsvp.WelcomeReception)
        {
            pnl_SundayReception.CssClass = "CheckBoxChecked";
            pnl_SundayReception.ToolTip = "Selected";
        }
        else
        {
            pnl_SundayReception.CssClass = "CheckBoxClear";
            pnl_SundayReception.ToolTip = "Not Selected";
        }
        //dl_MealDates=rsvp.Meals.s

        DataView view = new DataView(rsvp.GetMealsDetails());
        DataTable distinctDates = view.ToTable(true, "simpleDate");
        dl_MealDates.DataSource = distinctDates;
        dl_MealDates.DataBind();

        foreach (DataRow row in rsvp.GetTransporationDetails().Rows)
        {
            string direction = Convert.ToString(row["transportationDirection"]);
            string modeText = Convert.ToString(row["transportationmodeText"]);
            DateTime depart = Convert.ToDateTime(row["transportationDepartTime"]);
            bool hideTime = modeText.Equals("Self");
            if (direction == "Inbound")
                lbl_FromKT.Text = modeText + (hideTime ? String.Empty : " (" + depart.ToString("MM/dd h:mm tt") + ")");
            if (direction == "Outbound")
                lbl_ToKT.Text = modeText + (hideTime ? String.Empty : " (" + depart.ToString("MM/dd h:mm tt") + ")");

        }

        //lbl_FromKT.Text = rsvp.Transportation

        dl_TechPanelsList.DataSource = rsvp.GetTechPanelsDetails();
        dl_TechPanelsList.DataBind();


        DataView symposiumView = new DataView(rsvp.GetPaperDetails());
        DataTable sypmosiums = symposiumView.ToTable(true, new string[] { "symposiumText", "symposiumStart", "symposiumStop" });

        Session["rsvpPaperDetails"] = symposiumView;

        dl_PaperSymposiaList.DataSource = sypmosiums;
        dl_PaperSymposiaList.DataBind();


        hl_Map.NavigateUrl = ((Conference)Application["Conference"]).Venue.MapHyperlink;

    }
    private void loadForDecline()
    {
        RSVP rsvp = (RSVP)Session["verifyRSVP"];
        lbl_HeaderMsgTitle.Text = "You've Declined Your Invitation .";
        pnl_Icon.CssClass = "declineIcon";
        pnl_Header.CssClass = "warningHeader";
        pnl_HdrMessage.CssClass = "warningMsg";

        lbl_HeaderMsg.Text = "You've declined your invitation. If you decide register, you'll need to complete the registration by the deadline.";
        KTConferenceUser user = setUser();
        lbl_Cancel.Text = "Invitation for '" + user.FullName + "' declined on " + DateTime.Now.ToString("dddd, MMM d, yyyy h:mm tt") + ".";
        rsvp_form.CssClass = "hidden";
        cancel_form.CssClass = "floaterL";
        pnl_ConfirmationCode.Visible = false;
        Session["rsvp"] = null;
    }
    private void loadForCancellation()
    {
        RSVP rsvp = (RSVP)Session["verifyRSVP"];
        lbl_HeaderMsgTitle.Text = "Your Registration is Cancelled.";
        pnl_Icon.CssClass = "warningIcon";
        pnl_Header.CssClass = "warningHeader";
        pnl_HdrMessage.CssClass = "warningMsg";

        lbl_HeaderMsg.Text = "Your registration is cancelled. If you decide to re-register, you'll need to complete the registration form again.";
        KTConferenceUser user = setUser();
        lbl_Cancel.Text = "Registration for '" + user.FullName + "' cancelled on " + ((DateTime)rsvp.CancelDate).ToString("dddd, MMM d, yyyy h:mm tt") + ".";
        rsvp_form.CssClass = "hidden";
        cancel_form.CssClass = "floaterL";
        pnl_ConfirmationCode.Visible = false;
        Session["rsvp"] = null;

    }

    protected void dl_MealDates_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            RSVP rsvp = (RSVP)Session["verifyRSVP"];

            DataList me = (DataList)sender;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            DateTime day = Convert.ToDateTime(drv["simpleDate"]);

            Label lbl_MealDate = (Label)e.Item.FindControl("lbl_MealDate");
            lbl_MealDate.Text = Convert.ToDateTime(drv["simpleDate"]).ToString("dddd, MMMM dd, yyyy");

            DataList dl_MealsOnDate = (DataList)e.Item.FindControl("dl_MealsOnDate");
            DataView view = new DataView(rsvp.GetMealsDetails());
            view.RowFilter = "simpleDate = '" + day + "'";
            dl_MealsOnDate.DataSource = view;
            dl_MealsOnDate.DataBind();

        }

    }

    protected void dl_MealsOnDate_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataList me = (DataList)sender;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            Label lbl_MealType = (Label)e.Item.FindControl("lbl_MealType");
            Label lbl_MealChoice = (Label)e.Item.FindControl("lbl_MealChoice");
            Panel pnl_MealChoice = (Panel)e.Item.FindControl("pnl_MealChoice");

            lbl_MealType.Text = Convert.ToString(drv["mealType"]) + ": ";

            if (Convert.ToInt32(drv["optionCount"]) == 1)
            {
                pnl_MealChoice.CssClass = "CheckBoxChecked";
                lbl_MealChoice.CssClass = "hidden";
            }
            else
                lbl_MealChoice.Text = Convert.ToString(drv["mealOptionName"]);


        }
    }

    protected void cbo_AdminRsvps_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void dl_PaperSymposiaList_ItemDataBound(object sender, DataListItemEventArgs e)
    {

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            RSVP rsvp = (RSVP)Session["verifyRSVP"];

            DataList me = (DataList)sender;
            DataRowView drv = (DataRowView)e.Item.DataItem;
            DataList dl_PaperTimeSlots = (DataList)e.Item.FindControl("dl_PaperTimeSlots");
            Label lbl_SymposiaGroupTitle = (Label)e.Item.FindControl("lbl_SymposiaGroupTitle");
            Label lbl_SymposiaGroupTimes = (Label)e.Item.FindControl("lbl_SymposiaGroupTimes");

            lbl_SymposiaGroupTitle.Text = Convert.ToString(drv["symposiumText"]);
            lbl_SymposiaGroupTimes.Text = Convert.ToDateTime(drv["symposiumStart"]).ToString("dddd, MMMM dd yyyy hh:mm -") +
                Convert.ToDateTime(drv["symposiumStop"]).ToString(" hh:mm tt");

            DataView allData = (DataView)Session["rsvpPaperDetails"];
            allData.RowFilter = "symposiumText = '" + Convert.ToString(drv["symposiumText"]) + "'";
            DataTable timeSlots = allData.ToTable(true, new string[] { "slotID", "slotStart", "slotStop" });

            dl_PaperTimeSlots.DataSource = timeSlots;
            dl_PaperTimeSlots.DataBind();
        }

    }

    protected void dl_PaperTimeSlots_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            RSVP rsvp = (RSVP)Session["verifyRSVP"];

            DataList me = (DataList)sender;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            DataList dl_PaperTimeSlotChoices = (DataList)e.Item.FindControl("dl_PaperTimeSlotChoices");
            Label lbl_PaperTimeStart = (Label)e.Item.FindControl("lbl_PaperTimeStart");


            lbl_PaperTimeStart.Text = Convert.ToDateTime(drv["slotStart"]).ToString("hh:mm tt");

            DataView allData = (DataView)Session["rsvpPaperDetails"];
            allData.RowFilter = "slotID = " + Convert.ToString(drv["slotID"]);
            DataTable papers = allData.ToTable(true, new string[] { "eventRequestOrder", "paperID", "paperTitle", "paperHyperLink", "authorID", "authorFirstName", "authorLastName", "authorDivision" });
            dl_PaperTimeSlotChoices.DataSource = papers;
            dl_PaperTimeSlotChoices.DataBind();
        }
    }

    protected void dl_PaperTimeSlotChoices_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            RSVP rsvp = (RSVP)Session["verifyRSVP"];

            DataList me = (DataList)sender;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            Label lbl_TimeSlotChoice = (Label)e.Item.FindControl("lbl_TimeSlotChoice");
            HyperLink hl_PaperTitle = (HyperLink)e.Item.FindControl("hl_PaperTitle");
            Panel pnl_Presenter = (Panel)e.Item.FindControl("pnl_Presenter");
            Label lbl_Presenter = (Label)e.Item.FindControl("lbl_Presenter");
            int order = Convert.ToInt32(drv["eventRequestOrder"]);
            string sup = "<sup>" + (order == 1 ? "st" : "nd") + "</sup>";

            lbl_TimeSlotChoice.Text = order.ToString() + sup;
            hl_PaperTitle.Text = Convert.ToString(drv["paperTitle"]);
            hl_PaperTitle.NavigateUrl = Convert.ToString(drv["paperHyperLink"]);

            if (Convert.ToInt32(drv["authorID"]) == rsvp.User.UserID)
            {
                pnl_Presenter.Visible = true;
                lbl_Presenter.Text = "You're presenting this paper!";
            }

        }
    }

    protected void dl_TechPanelsList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            RSVP rsvp = (RSVP)Session["verifyRSVP"];

            DataList me = (DataList)sender;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            Label lbl_TechPanelChoice = (Label)e.Item.FindControl("lbl_TechPanelChoice");
            Label lbl_TechPanelTitle = (Label)e.Item.FindControl("lbl_TechPanelTitle");
            Label lbl_Moderator = (Label)e.Item.FindControl("lbl_Moderator");
            Label lbl_ModOrMember = (Label)e.Item.FindControl("lbl_ModOrMember");

            Panel pnl_ModOrMember = (Panel)e.Item.FindControl("pnl_ModOrMember");
            int choice = Convert.ToInt32(drv["eventRequestOrder"]);
            string super = (choice == 1 ? "st" : choice == 2 ? "nd" : "rd");
            lbl_TechPanelChoice.Text = Convert.ToString(choice + "<sup>" + super + "</sup>:");
            lbl_TechPanelTitle.Text = Convert.ToString(drv["techpanelTitle"]);
            lbl_Moderator.Text = "Moderated by: " + Convert.ToString(drv["modFirstName"]) + " " + Convert.ToString(drv["modLastName"]) + ", " + Convert.ToString(drv["modDivision"]);
            
            if (Convert.ToString(drv["userRole"]).Contains("Panel M"))
                pnl_ModOrMember.Visible = true;
            if (Convert.ToString(drv["userRole"]).Contains("Moderator"))
                lbl_ModOrMember.Text = "You're moderating this panel!";
            else
                lbl_ModOrMember.Text = "You're a member on this panel!";
        }

    }
  
    protected void timer_Redirect_Tick(object sender, EventArgs e)
    {
        Timer me = (Timer)sender;
        bool redirect = false;
        if (Session["verifyRSVP"] == null)
            redirect = true;
        else
        {
            RSVP rsvp = (RSVP)Session["verifyRSVP"];
            redirect = !rsvp.IsValid;
        }
        me.Enabled = redirect;
        if (redirect)
            redirectToDefault();
    }

    protected void bn_Close_Click(object sender, EventArgs e)
    {
        redirectToDefault();
    }

    protected void bn_FixRSVP_Click(object sender, EventArgs e)
    {
        HttpContext.Current.Response.Redirect("Registration.aspx"); //need to update for encrypted qs; also need to update registrationt to ensure only attendee and/or user allowed to update.
        //should also allow only user to update if admin_user is null
    }

    protected void bn_RegAnother_Click(object sender, EventArgs e)
    {
        HttpContext.Current.Response.Redirect("Registration.aspx"); // modal popup, verify email, blah blah...with encrypted qs

    }

    private void showUtilityModalPopup(string purpose, string heading, string msg, string initialInputText, string OKButtonText, bool needCboBox, bool needTextBox)
    {
        hdn_UtilityModalPurpose.Value = purpose;
        lbl_UtilityModalHeader.Text = heading;
        lbl_UtilityModalMessage.Text = msg;
        pnl_CboContainer.CssClass = (needCboBox ? "comboBoxInsideModalPopup" : "hidden");
        //if (needCboBox)
        //{
        //    KTConferenceUser user = setUser();
        //    DataTable adminForUsers = user.getAdminForUsersList(Convert.ToInt32(Session["conferenceID"]), "Guest");

        //    cbo_AdminRsvps.DataValueField = "rsvpEmail";
        //    cbo_AdminRsvps.DataTextField = "rsvpUser";
        //    cbo_AdminRsvps.DataSource = adminForUsers;
        //    cbo_AdminRsvps.DataBind();
        //    cbo_AdminRsvps.Items.Insert(0, new ListItem("Select:", "0"));
        //    cbo_AdminRsvps.SelectedIndex = findListIndex(cbo_AdminRsvps.Items, "0");

        //}


        if (!needTextBox)
            tb_UtilityModalEntry.CssClass = "hidden";
        else
            tb_UtilityModalEntry.CssClass = "input";


        bn_UtilityModalSave.Text = OKButtonText;

        bn_UtilityModaClose.CssClass = (purpose == "validation" ? "hidden" : String.Empty);
        lbl_UtilityModalMessage.CssClass = (purpose == "validation" ? "validationMsg" : String.Empty);

        up_UtilityModal.Update();
        mdl_UtilityModal.Show();

    }

    protected void bn_UtilityModalSave_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(2000);
        switch (hdn_UtilityModalPurpose.Value)
        {
            case ("info"):
                {
                    val_TextEntry.IsValid = true;
                    break;
                }
            case ("adminview"):
                {
                    if (cbo_AdminRsvps.SelectedIndex == 0)
                    {
                        val_TextEntry.IsValid = false;
                        lbl_UtilityModalMessage.Text = "Please select a user or click 'Cancel'.";
                    }
                    else
                    {

                        RSVP rsvp = new RSVP(new KTConferenceUser(cbo_AdminRsvps.SelectedValue), "Guest");
                        rsvp.PropertyChanged += new PropertyChangedEventHandler(rsvp_PropertyChanged);
                        if (rsvp.IsValid)
                        {
                            val_TextEntry.IsValid = true;
                            Session["verifyRSVP"] = rsvp;
                            loadForm(rsvp);
                        }
                        else
                        {
                            val_TextEntry.IsValid = false;
                            lbl_UtilityModalMessage.Text = "The registration for user '" + cbo_AdminRsvps.SelectedValue 
                                + "' is invalid. Please contact confernence adnimistrators.";
                        }

                    }
                    break;
                }
            case ("adminvite"): //this is use case where invited user is registering for someone else. big difference if user cancels then we just reload invited users's info rather than redirect off of page. Also need to tweak the wording of modal popup.
                {
                    tb_UtilityModalEntry_Validate();
                    if (!val_TextEntry.IsValid)
                        lbl_Error.Text = "*Sorry, that email address is not on the current invite list.<br> Please double-check the spelling or contact the employee's Head of Engineering.";
                    else
                    {
                        KTConferenceUser user = setUser();
                        if(user.isAdminForKTUser(tb_UtilityModalEntry.Text, "Guest"))
                        {
                            Session["rsvp"] = null;
                            redirectToRegistration(user, new KTConferenceUser(tb_UtilityModalEntry.Text));
                        }
                        else 
                        {
                            val_TextEntry.IsValid = false;
                            lbl_Error.Text = "*Sorry, you are not the administrative user for '" + tb_UtilityModalEntry.Text + "'.<br> Please check with this employee.";
                        }
                    }
                    break;
                }
            case ("submit"):
                {
                    RSVP rsvp = (RSVP)Session["rsvp"];
                    rsvp.setPhotoWaiver();
                    Session["rsvp"] = rsvp;

                    //if (writeRSVP())
                    //    HttpContext.Current.Response.Redirect("Confirmation.aspx");
                    //else
                    //    HttpContext.Current.Response.Redirect("ConfirmationError.aspx");

                    break;
                }
            case ("delete"):
                {
                    KTConferenceUser user = setUser();
                    RSVP rsvp = (RSVP)Session["verifyRSVP"];

                    Session["rsvpUser"] = user;

                    if (!rsvp.isCurrent)
                    {
                        lbl_Error.Text = "This registration is already cancelled.";
                    }
                    else if (!rsvp.IsValid)
                    {
                        lbl_Error.Text = "No registration exists for current user.";
                    }
                    else
                    {
                        rsvp.CancelRSVP(tb_UtilityModalEntry.Text.Length==0?"No reason given" : tb_UtilityModalEntry.Text);
                        Session["verifyRSVP"] = rsvp;
                        Session["rsvp"] = rsvp;

                        loadForCancellation();
                    }
                    break;
                }

            case ("badRSVP"):
                {
                    redirectToDefault();
                    break;
                }
            default:
                {
                    break;
                }
        }
        if (val_TextEntry.IsValid)
        {
            hdn_UtilityModalPurpose.Value = string.Empty;
            lbl_UtilityModalMessage.Text = string.Empty;
            //tb_UtilityModalEntry.Text = string.Empty;
            lbl_Error.Text = string.Empty;
            lbl_Error.Visible = false;
            mdl_UtilityModal.Hide();
        }
        else
        {
            lbl_Error.Visible = true;
            up_UtilityModal.Update();
            mdl_UtilityModal.Show();
        }

    }

    protected void bn_UtilityModalCancel_Click(object sender, EventArgs e)
    {
        RSVP rsvp = (RSVP)Session["rsvp"];
        if (hdn_UtilityModalPurpose.Value == "adminview")
        {
            cbo_AdminRsvps.Items.Clear();
        }
        if (hdn_UtilityModalPurpose.Value == "nonvite")
        {
            //Session.Clear();
            Session["rsvpUser"] = null;
            Session["rsvp"] = null;
            System.Threading.Thread.Sleep(2000);
            redirectToDefault();
        }
        if (hdn_UtilityModalPurpose.Value == "adminvite")
        {
            Session["rsvpUser"] = null;
            Session["rsvp"] = null;
            System.Threading.Thread.Sleep(2000);
        }
        if (hdn_UtilityModalPurpose.Value == "submit")
        {

        }
        if (hdn_UtilityModalPurpose.Value == "badRSVP")
        {
            System.Threading.Thread.Sleep(2000);
            redirectToDefault();
        }

        hdn_UtilityModalPurpose.Value = string.Empty;
        lbl_UtilityModalMessage.Text = string.Empty;
        tb_UtilityModalEntry.Text = string.Empty;
        lbl_Error.Text = string.Empty;
        lbl_Error.Visible = false;
        mdl_UtilityModal.Hide();
    }

    protected void tb_UtilityModalEntry_Validate()
    {
        if (hdn_UtilityModalPurpose.Value == "nonvite" || hdn_UtilityModalPurpose.Value == "adminvite")
        {
            try
            {
                KTConferenceUser user = new KTConferenceUser(tb_UtilityModalEntry.Text);
                Session["rsvpUser"] = null;
                if (user.IsInvitee)
                    Session["rsvpUser"] = user;

                val_TextEntry.IsValid = (user.IsInvitee);
            }
            catch (Exception e)
            {
                Session["rsvpUser"] = null;
                val_TextEntry.IsValid = false;
            }
        }
        else
        {
            val_TextEntry.IsValid = true;
        }
    }

    private void redirectToRegistration(KTConferenceUser user, KTConferenceUser rsvpUser)
    {
        Session["rsvpUser"] = rsvpUser;
        Session["user"] = user;

        HttpContext.Current.Response.Redirect("Registration.aspx?" + security.Encrypt(
                                       string.Format("admin={0}", user.Login), rsvpUser.Email.Substring(0, 8)));
    }

    private KTConferenceUser setUser()
    {
        KTConferenceUser user = null;

        if (Session["user"] == null)
        {
            user = new KTConferenceUser(new KTActiveDirectoryUser(new KTLogin(HttpContext.Current.User.Identity.Name)));
#if DEBUG
                user = new KTConferenceUser(new KTActiveDirectoryUser(new KTLogin("KLASJ\\arangle")));
#endif

            Session["user"] = user;
            return user;
        }
        else
        {
            user = (KTConferenceUser)Session["user"];
        }

        return user;
    }

    private void redirectToDefault()
    {
        HttpContext.Current.Response.Redirect("Default.aspx");
    }
    protected void lb_Exit_Click(object sender, EventArgs e)
    {
        WebDataUtility.Instance.Dispose();
        Session.Abandon();
        HttpContext.Current.Response.Redirect("http://ktintranet.kla-tencor.com/home/");
    }

    protected void lb_EditRegistration_Click(object sender, EventArgs e)
    {

        if (Session["verifyRSVP"] != null)
        {
            RSVP rsvp = (RSVP)Session["verifyRSVP"];

            Session["rsvp"] = Session["verifyRSVP"];
            KTConferenceUser user = (KTConferenceUser)rsvp.User;

            redirectToRegistration(user, user);
        }
        else
        {
            KTConferenceUser user = (KTConferenceUser)Session["user"];
            if (!user.IsInvitee)
            {
                string msg = "No registration found for " + user.FullName + ". Redirecting you to the <a href='Default.aspx'>Registration Home Page.</a>";
                showUtilityModalPopup("badRSVP", "No Registration Found", msg, string.Empty, "OK", false, false);
            }
            else
            {
                redirectToRegistration(user, user);
            }
        }
    }

    protected void lb_RegisterAnother_Click(object sender, EventArgs e)
    {
        Session["rsvpUser"] = null;
        KTConferenceUser user = setUser();

        string msg = "Registering for someone else? If so, please enter their KLA-Tencor email address.";
        showUtilityModalPopup("adminvite", "Registering for Someone Else?", msg, String.Empty, "OK", false, true);

    }

    protected void lb_ViewAdmin_Click(object sender, EventArgs e)
    {
        KTConferenceUser user = setUser();
        Conference conference = (Conference)Application["Conference"];
        DataTable adminForUsers = user.getAdminForUsersList(conference.ID, "Guest");

        if (adminForUsers.Rows.Count == 0)
        {
            string msg = "You're not an Admin User for any conference attendees.";
            showUtilityModalPopup("info", "Not an Admin", msg, String.Empty, "OK", false, false);
        }
        else
        {
            cbo_AdminRsvps.DataValueField = "rsvpEmail";
            cbo_AdminRsvps.DataTextField = "rsvpUser";
            cbo_AdminRsvps.DataSource = adminForUsers;
            cbo_AdminRsvps.DataBind();

            if(user.IsInvitee)
                cbo_AdminRsvps.Items.Insert(0, new ListItem(user.FullName, user.Email));
            
            cbo_AdminRsvps.Items.Insert(0, new ListItem("Select:", "0"));
            cbo_AdminRsvps.SelectedIndex = findListIndex(cbo_AdminRsvps.Items, "0");

            string msg = "Please select the Conference Attendee's registration you wouldl like to review.";
            showUtilityModalPopup("adminview", "View My Admin Registrations", msg, String.Empty, "OK", true, false);
        }

    }

    protected void lb_CurrentAgenda_Click(object sender, EventArgs e)
    {
        Warning[] warnings;
        string[] streamIds;
        string mimeType = string.Empty;
        string encoding = string.Empty;
        string extension = string.Empty;
        //setup report datasource
        EngConferenceDataSetTableAdapters.sp_GetPersonalScheduleTableAdapter ds = new EngConferenceDataSetTableAdapters.sp_GetPersonalScheduleTableAdapter();
        int userID = ((ConferenceUser)Session["user"]).UserID;
        //ObjectDataSource obj = new ObjectDataSource();
        //obj.SelectMethod = "GetData";
        //obj.TypeName = "EngConferenceDataSetTableAdapters.sp_GetDailyAgendaItemsTableAdapter";
        //obj.OldValuesParameterFormatString = "original_{0}";
        //obj.SelectParameters.Add(new Parameter("conferenceID", DbType.Int32, System.Configuration.ConfigurationManager.AppSettings["ConferenceID"]));
        //obj.SelectParameters["conferenceID"].Direction = ParameterDirection.Input;
        //obj.DataBind();
        //DataTable table = (DataTable)obj.Select();
        string reportsFolder = Server.MapPath("Temp");
        string fileName = reportsFolder + "\\baseAgenda.pdf";
        FileInfo file = new FileInfo(fileName);

        if(!file.Exists)
        {
        ReportDataSource rds = new ReportDataSource("DataSet1", (DataTable)ds.GetData(Conference.Instance.ID, userID));

        // Setup the report viewer object and get the array of bytes
        ReportViewer viewer = new ReportViewer();
        viewer.LocalReport.DataSources.Add(rds);
        viewer.ProcessingMode = ProcessingMode.Local;
        viewer.LocalReport.ReportPath = "Reports/BaseAgenda.rdlc";

        ReportParameter Param0 = new ReportParameter("conferenceID", System.Configuration.ConfigurationManager.AppSettings["ConferenceID"]);
        viewer.LocalReport.SetParameters(new ReportParameter[] { Param0 });

        byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

        KTConferenceUser user = (KTConferenceUser)Session["user"];
        //File.Delete(fileName);
         FileStream fs = new FileStream(fileName, FileMode.Create);
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();
       }
        
        //Response.Redirect("~/Temp/" + user.Login + "_baseAgenda.pdf");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), null, "window.open('Temp/baseAgenda.pdf', '_newtab')", true);
    }

    protected void lb_EmailDetails_Click(object sender, EventArgs e)
    {
        bool notValid = false;

        if (Session["verifyRSVP"] != null)
        {
            RSVP rsvp = (RSVP)Session["verifyRSVP"];
            notValid = !rsvp.IsValid;

            if (!notValid)
            {
                string html = string.Empty;
                string url = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + "/EmailBody.aspx?user=" + rsvp.User.Email;

                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                    req.Credentials = CredentialCache.DefaultCredentials;
                    HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                    StreamReader sr = new StreamReader(res.GetResponseStream());
                    html = sr.ReadToEnd();
                }
                catch (Exception ex)
                {
                    html = ex.Message;
                }
                //html = url;
                rsvp.SendEmailConfirmation(html, false);
                string msg = "Email confirmation sent to '" + rsvp.User.Email + "'.";
                if (rsvp.Admin != null)
                    msg += "<br>Cc'd to '" + rsvp.Admin.Email + "'.";

                showUtilityModalPopup("info", "Email Confirmation Sent", msg, String.Empty, "OK", false, false);
            }
        }
        else
            notValid = true;

        if (notValid)
        {
            string msg = "There is not a valid registration to email.";
            showUtilityModalPopup("info", "No Registration to Email", msg, String.Empty, "OK", false, false);
        }
    }

    protected void lb_Calendar_Click(object sender, EventArgs e)
    {
        if (Session["verifyRSVP"] != null)
        {
            RSVP rsvp = (RSVP)Session["verifyRSVP"];

            if (rsvp.IsValid)
            {
                string fileName = rsvp.CreateCalendarObjectForDownload();
                string fullPath = Request.PhysicalApplicationPath + @"\Temp\" + fileName;
                string strURL=  "Temp/" + fileName;
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), null, "window.open('" + strURL + "', '_newtab')", true);
                Response.Redirect(strURL);
                return;
            }

        }
        string msg = "There is not a valid registration to create a calendar file.";
        showUtilityModalPopup("info", "No Registration for Calendar", msg, String.Empty, "OK", false, false);


    }

    protected void lb_DeleteRegistration_Click(object sender, EventArgs e)
    {
        KTConferenceUser user = setUser();
        Conference confMetaData = (Conference)Application["Conference"];

        string msg = "Are you sure you want to delete your reservation?<p>Your selections will be cleared from the registration system " +
            "and you'll need re-enter all information if you need to register again.</p>For our planning purposes for the next conference, " +
            "can you let us know why you're cancelling?";
        showUtilityModalPopup("delete", "Delete Registration", msg, String.Empty, "OK", false, true);

    }


    private int findListIndex(ListItemCollection list, string value)
    {
        int i = 0;
        bool selected = false;
        foreach (ListItem item in list)
        {
            if (item.Value == value)
            {
                selected = true;
                break;
            }
            else
                i++;
        }
        if (!selected)
            i = 0;
        return i;
    }

}