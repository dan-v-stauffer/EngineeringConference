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


public partial class _Default : System.Web.UI.Page
{
    EncryptDecryptQueryString security = EncryptDecryptQueryString.Instance;

    


    protected void Page_Load(object sender, EventArgs e)
    {
        Session["autoRefreshReg"] = true;

        if (!IsPostBack)
        {
            KTConferenceUser user = setUser();

            Session["user"] = user;
            Conference conference = (Conference)Application["Conference"];

            tb_UtilityModalEntry.Attributes["onfocus"] = "javascript:this.select();";
            //CREATE Conference.ToTable() method;

            dl_DayOfConference.DataSource = WebDataUtility.Instance.webAppTable("sp_GetConferenceDateRange",
                new GenericCmdParameter[] { new GenericCmdParameter("@conferenceID", conference.ID) });
            dl_DayOfConference.DataBind();
            
            if (user.IsInvitee)
            {
                Session["rsvpUser"] = Session["user"];
                RSVP rsvp = new RSVP(user, "Guest");
                rsvp.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(rsvp_PropertyChanged);
                bool isRegistered = user.IsRegistered(conference.ID);

                if (bn_masterRegister != null)
                {
                    bn_masterRegister.Click += new EventHandler(bn_Register_Click);
                    if (isRegistered)
                        bn_masterRegister.Text = "Update My Registration";
                }

                if (isRegistered)
                    lb_EditRegistration.Text = "Update My Registration";
                Session["rsvp"] = rsvp;
            }
            tb_UtilityModalEntry.Attributes["onfocus"] = "javascript:this.select();";

            hl_Map.NavigateUrl = ((Conference)Application["Conference"]).Venue.MapHyperlink;

        }
    }

    protected void bn_iConnectEmail_Click(object sender, EventArgs e)
    {

        DataTable guests = WebDataUtility.Instance.webAppTable("sp_GetRegisteredGuests",
                            new GenericCmdParameter[] { new GenericCmdParameter("@conferenceID", Conference.Instance.ID) });

        foreach (DataRow row in guests.Rows)
        {
            KTConferenceUser user = new KTConferenceUser(DBNullable.ToString(row["userEmail"]));

            if (user.IsExec())
                continue;

            RSVP rsvp = new RSVP(user, "Guest");

            try
            {
                Conference.Instance.SendGenericConferenceEmail(this.Page, rsvp, "/EmailTemplates/iConnect.aspx",
                   "Connect-Collaborate-Innovate: iConnect Lunch and Learn", string.Empty, false, false);
            }
            catch (Exception ex)
            {
            }
        }
    }

    protected void bn_SurveyEmail_Click(object sender, EventArgs e)
    {
        DataTable guests = WebDataUtility.Instance.webAppTable("sp_GetRegisteredGuests",
                            new GenericCmdParameter[] { new GenericCmdParameter("@conferenceID", Conference.Instance.ID) });

        foreach (DataRow row in guests.Rows)
        {
            KTConferenceUser user = new KTConferenceUser(DBNullable.ToString(row["userEmail"]));

            if (user.IsExec())
                continue;

            RSVP rsvp = new RSVP(user, "Guest");

            try
            {
                Conference.Instance.SendGenericConferenceEmail(this.Page, rsvp, "/EmailTemplates/Survey.aspx",
                   "Reminder: 2015 Engineering Conference Survey", string.Empty, true, false);
            }
            catch (Exception ex)
            {
            }
        }
    }
    protected void bn_VideoWaiversEmail_Click(object sender, EventArgs e)
    {

        DataTable guests = WebDataUtility.Instance.webAppTable("sp_NeededVideoWaivers",
                            new GenericCmdParameter[] { new GenericCmdParameter("@conferenceID", Conference.Instance.ID) });

        foreach (DataRow row in guests.Rows)
        {
            KTConferenceUser user = new KTConferenceUser(DBNullable.ToString(row["userEmail"]));

            if (user.IsExec())
                continue;


            RSVP rsvp = new RSVP(user, "Guest");

            try
            {
                Conference.Instance.SendGenericConferenceEmail(this.Page, rsvp, "/EmailTemplates/ReleaseWaiver.aspx",
                   "2015 Engineering Conference - Individual Appearance Release", createVideoWaiverDoc(user), false, false);

            }
            catch (Exception ex)
            {
            }
        }
    }

    string createVideoWaiverDoc(KTConferenceUser user)
    {
        Warning[] warnings;
        string[] streamIds;
        string mimeType = string.Empty;
        string encoding = string.Empty;
        string extension = string.Empty;

           try
            {
                string reportsFolder = Server.MapPath("Temp\\releases");
                string fileName = reportsFolder + "\\" + String.Format("{0}.{1}", user.FirstName, user.LastName) + ".release.pdf";
                
                if (File.Exists(fileName))
                    File.Delete(fileName);
                FileInfo file = new FileInfo(fileName);

                if (!file.Exists)
                {
                    DataTable rptData = WebDataUtility.Instance.webAppTable("sp_GetKTUser",
                      new GenericCmdParameter[] { new GenericCmdParameter("@userKTEmail", user.Email) });

                    ReportDataSource rds = new ReportDataSource("DataSet1", rptData);
                    ReportViewer viewer = new ReportViewer();
                    viewer.LocalReport.DataSources.Add(rds);
                    viewer.ProcessingMode = ProcessingMode.Local;
                    viewer.LocalReport.ReportPath = "Reports/VideoWaiver.rdlc";

                    ReportParameter Param0 = new ReportParameter("userKTEmail", user.Email);
                    viewer.LocalReport.SetParameters(new ReportParameter[] { Param0 });
                    byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                    //File.Delete(fileName);

                    FileStream fs = new FileStream(fileName, FileMode.Create);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                   
                } 
               return fileName;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
    }


    protected void bn_TestEmail_Click(object sender, EventArgs e)
    {

        Warning[] warnings;
        string[] streamIds;
        string mimeType = string.Empty;
        string encoding = string.Empty;
        string extension = string.Empty;
        //setup report datasource
        EngConferenceDataSetTableAdapters.sp_GetPersonalScheduleTableAdapter ds = new EngConferenceDataSetTableAdapters.sp_GetPersonalScheduleTableAdapter();


        DataTable guests = WebDataUtility.Instance.webAppTable("sp_GetRegisteredGuests",
                            new GenericCmdParameter[] { new GenericCmdParameter("@conferenceID", Conference.Instance.ID) });

        foreach (DataRow row in guests.Rows)
        {
                 KTConferenceUser user = new KTConferenceUser(DBNullable.ToString(row["userEmail"]));
           try
            {
                RSVP rsvp = new RSVP(user, "Guest");

                int userID = user.UserID;
                string reportsFolder = Server.MapPath("Temp");
                string fileName = reportsFolder + "\\" + String.Format("{0}.{1}", user.FirstName, user.LastName) + ".pdf";

                if (File.Exists(fileName))
                    File.Delete(fileName);
                FileInfo file = new FileInfo(fileName);

                if (!file.Exists)
                {
                    DataTable rptData = WebDataUtility.Instance.webAppTable("sp_GetPersonalSchedule",
                        new GenericCmdParameter[] { new GenericCmdParameter("@conferenceID", Conference.Instance.ID),
                                                new GenericCmdParameter("@userID", userID),
                                                new GenericCmdParameter("@invitationType", rsvp.InvitationType)        });
                    ReportDataSource rds = new ReportDataSource("DataSet1", rptData);

                    // Setup the report viewer object and get the array of bytes
                    ReportViewer viewer = new ReportViewer();
                    viewer.LocalReport.DataSources.Add(rds);
                    viewer.ProcessingMode = ProcessingMode.Local;
                    viewer.LocalReport.ReportPath = "Reports/PersonalAgenda.rdlc";

                    ReportParameter Param0 = new ReportParameter("conferenceID", DBNullable.ToString(Conference.Instance.ID));
                    ReportParameter Param1 = new ReportParameter("userID", userID.ToString());
                    ReportParameter Param2 = new ReportParameter("userName", String.Format("{0} {1}",
                                                    user.FirstName, user.LastName));

                    viewer.LocalReport.SetParameters(new ReportParameter[] { Param0, Param1, Param2 });

                    byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                    //File.Delete(fileName);

                    FileStream fs = new FileStream(fileName, FileMode.Create);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                }

                string cc = (rsvp.Admin == null ? string.Empty : rsvp.Admin.Email);

              // Conference.Instance.SendConfirmationEmail(this.Page, user, cc, fileName);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(new Exception("FAILED: Invite sent to " +
                    user.FullName.ToUpper()), "bn_TestEmail_Click(object sender, EventArgs e)");
                ExceptionUtility.LogException(ex, "bn_TestEmail_Click(object sender, EventArgs e)");
            }
        }
    }


    protected void lb_PostConferenceEmail_Click(Object sender, EventArgs e)
    {
        DataTable guests = WebDataUtility.Instance.webAppTable("sp_GetAllRSVPs",
                          new GenericCmdParameter[] { new GenericCmdParameter("@conferenceID", Conference.Instance.ID) });

        foreach (DataRow row in guests.Rows)
        {
            KTConferenceUser user = new KTConferenceUser(DBNullable.ToString(row["userEmail"]));
            RSVP rsvp = new RSVP(user, DBNullable.ToString(row["rsvpInvitationType"]));

            try //""
            {
                Conference.Instance.SendGenericConferenceEmail(this.Page, rsvp, "EmailTemplates/PostConferenceEmail.aspx",
                    "Congratulations on the First KT Engineering Conference. What's Next?",String.Empty, false, false);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(new Exception("FAILED: Mobile Email sent to " +
                    user.FullName.ToUpper()), "bn_TestEmail_Click(object sender, EventArgs e)");
                ExceptionUtility.LogException(ex, "bn_TestEmail_Click(object sender, EventArgs e)");
            }
        }
    }

    protected void bn_MobileAppEmail_Click(object sender, EventArgs e)
    {
        string mimeType = string.Empty;
        string encoding = string.Empty;
        string extension = string.Empty;

        //setup report datasource
        
        DataTable guests = WebDataUtility.Instance.webAppTable("sp_GetAllRSVPs",
                            new GenericCmdParameter[] { new GenericCmdParameter("@conferenceID", Conference.Instance.ID) });
        foreach (DataRow row in guests.Rows)
        {
            KTConferenceUser user = new KTConferenceUser(DBNullable.ToString(row["userEmail"]));
            RSVP rsvp = new RSVP(user, DBNullable.ToString(row["rsvpInvitationType"]));

            try
            {
                Conference.Instance.SendMobileAppEmail(this.Page, rsvp);
            }
            catch (Exception ex)
            {
                ExceptionUtility.LogException(new Exception("FAILED: Mobile Email sent to " +
                    user.FullName.ToUpper()), "bn_TestEmail_Click(object sender, EventArgs e)");
                ExceptionUtility.LogException(ex, "bn_TestEmail_Click(object sender, EventArgs e)");
            }
        }
    }

    
    
    
    private void rsvp_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        
        Session["rsvp"] = (RSVP)sender;
    }

    protected void lb_DeleteRegistration_Click(object sender, EventArgs e)
    {
        KTConferenceUser user = setUser();
        Conference confMetaData = (Conference)Application["Conference"];

        string msg = "Are you sure you want to delete your reservation?<p>Your selections will be cleared from the registration system " + 
            "and you'll need re-enter all information if you need to register again.</p>For our planning purposes for the next conference, " +
            "can you let us know why you're cancelling?";
        showUtilityModalPopup("delete", "Delete Registration", msg, String.Empty, "OK", true);

    }


    protected void bn_Register_Click(object sender, EventArgs e)
    {
        KTConferenceUser user = setUser();
        Conference confMetaData = (Conference)Application["Conference"];

        if ((DateTime.Now > confMetaData.PrimaryRegistrationClosed && !user.IsInvitee && user.InviteClass == 1))
        {
            string msg = "Sorry, registration is currently closed for primary invitees.";
            showUtilityModalPopup("nonvite", "Primary Registration Closed", msg, String.Empty, "OK", false);
            return;
        }
        else if (DateTime.Now > confMetaData.LateRegistrationClosed && !user.IsInvitee && user.InviteClass == 2)
        {
            string msg = "Sorry, registration is currently closed for alternate invitees.";
            showUtilityModalPopup("nonvite", "Alternate Registration Closed", msg, String.Empty, "OK", false);
            return;
        }
        else if (DateTime.Now > confMetaData.NoChangeDate && user.IsInvitee)
        {
            string msg = "Sorry, changes to registration options are not available at this time. <br> Please contact the conference points of contact for urgent matters.";
            showUtilityModalPopup("nonvite", "Changes Not Allowed", msg, String.Empty, "OK", false);
            return;
        }
        else if (!user.IsInvitee)
        {
            string msg = "Oops! We cannot find your name in the guest list. Please contact your divisional Head of Engineering " +
                 "to determine if the mistake is ours!<br><br>Are you registering for someone else? If so, please enter their KLA-Tencor email address." +
                 "<br><br> (Note: By cancelling you will exit this page and be directed to the 2015 Engineering Conference Website.)";
            showUtilityModalPopup("adminvite", "Registering for Someone Else?", msg, String.Empty, "OK", true);
        }
        else
        {
            Session["rsvpUser"] = user;
            redirectToRegistration(user, user);
        }

    }

    protected void bn_DeclineRegister_Click(object sender, EventArgs e)
    {
        KTConferenceUser user = setUser();
        Conference confMetaData = (Conference)Application["Conference"];

        string msg = "Are you sure you want to decline? You won't receive any reminders to register.<p>If you're already registered, " + 
            "this will cancel your existing reservation. If you wish to register in the future, you'll need to complete the registration form again.<br>" + 
            "For planning purposes, would you mind letting let us know why you are declining/cancelling?</p>";

        showUtilityModalPopup("decline", "Decline Invitation Confirmation", msg, String.Empty, "OK", true);

    }
    private KTConferenceUser setUser()
    {
        KTConferenceUser user = null;

        if (Session["user"] == null)
        {
            user = new KTConferenceUser(new KTActiveDirectoryUser(new KTLogin(HttpContext.Current.User.Identity.Name)));
            
            Session["user"] = user;
            
            return user;
        }
        else
        {
            user = (KTConferenceUser)Session["user"];
        }
            
        return user;
    }

    protected void bn_RegOther_Click(object sender, EventArgs e)
    {

        Session["rsvpUser"] = null;
        KTConferenceUser user = setUser();
        string msg = "Registering for someone else? If so, please enter their KLA-Tencor email address.";
        showUtilityModalPopup("adminvite", "Registering for Someone Else?", msg, String.Empty, "OK", true);

    }

    protected void lb_SendBatchConfirmations_Click(object sender, EventArgs e)
    {
        Control context = (Control)sender;
        Conference.Instance.SendConfirmationEmailBatch(context, false);
    }

    protected void dl_DayOfConference_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Conference conference = (Conference)Application["Conference"];
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Label lbl_DayOfConference = (Label)e.Item.FindControl("lbl_DayOfConference");

            DateTime dayOfConference = (DateTime)drv["conferenceDay"];

            lbl_DayOfConference.Text = dayOfConference.ToLongDateString();

            DataList dl_DailyAgenda = (DataList)e.Item.FindControl("dl_DailyAgenda");
            dl_DailyAgenda.DataSource = WebDataUtility.Instance.webAppTable("sp_GetDailyAgendaItems", 
                new GenericCmdParameter[] { 
                    new GenericCmdParameter("@conferenceID", conference.ID),
                    new GenericCmdParameter("@dateOfConference", dayOfConference)
                });
            dl_DailyAgenda.DataBind();
        }
    }

    protected void dl_DailyAgenda_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            Label lbl_Time = (Label)e.Item.FindControl("lbl_Time");
            DateTime eventStart = (DateTime)drv["eventStart"];
            String eventDuration = String.Empty;

            if (drv["eventStop"].Equals(DBNull.Value))
            {
                eventDuration = eventStart.ToString("h:mm tt");
            }
            else
            {
              DateTime eventStop = (DateTime)drv["eventStop"];
              eventDuration = eventStart.ToString("h:mm") +
                  (eventStart.ToString("tt").Equals(eventStop.ToString("tt")) ? String.Empty : eventStart.ToString(" tt")) + " - " + eventStop.ToString("h:mm tt");
            }

            lbl_Time.Text = eventDuration;

            Label lbl_EventText = (Label)e.Item.FindControl("lbl_EventText");

            lbl_EventText.Text= Convert.ToString(drv["eventText"]);

            Label lbl_EventSpeaker = (Label)e.Item.FindControl("lbl_EventSpeaker");
            
            string speakerFirstName = Convert.ToString(drv["userFirstName"]);
            string speakerLastName = Convert.ToString(drv["userLastName"]);
            string speakerPrefix = Convert.ToString(drv["userPrefix"]);
            string speakerTitle = Convert.ToString(drv["userTitle"]);

            if((speakerFirstName + speakerLastName).Length>0)
            {
                string fullText = (speakerPrefix.Length>0)?speakerPrefix + " ":String.Empty;
                fullText = fullText + (speakerFirstName.Length>0?speakerFirstName + " " :String.Empty);
                fullText = fullText + (speakerLastName.Length>0?speakerLastName:String.Empty);
                fullText = fullText + (speakerTitle.Length>0?", " + speakerTitle:string.Empty);
                lbl_EventSpeaker.Text = "- " + fullText;
            }


        }
    }

    private void showUtilityModalPopup(string purpose, string heading, string msg, string initialInputText, string OKButtonText, bool needTextBox)
    {
        hdn_UtilityModalPurpose.Value = purpose;
        lbl_UtilityModalHeader.Text = heading;
        lbl_UtilityModalMessage.Text = msg;

        tb_UtilityModalEntry.Visible = needTextBox;
        if (needTextBox)
        {
            tb_UtilityModalEntry.Text = initialInputText;
            tb_UtilityModalEntry.Focus();
        }
        bn_UtilityModalSave.Text = OKButtonText;

        bn_UtilityModaClose.CssClass = (purpose == "validation" ? "hidden" : String.Empty);
        lbl_UtilityModalMessage.CssClass = (purpose == "validation" ? "validationMsg" : String.Empty);

        up_UtilityModal.Update();
        mdl_UtilityModal.Show();

    }

    protected void tb_UtilityModalEntry_Validate()
    {
        if (hdn_UtilityModalPurpose.Value == "nonvite" || hdn_UtilityModalPurpose.Value == "adminvite")
        {
            try
            {
                KTConferenceUser user = new KTConferenceUser(tb_UtilityModalEntry.Text);

                if (!user.IsInvitee)
                    Session["rsvpUser"] = null;
                else
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
                                       string.Format("admin={0}", user.Login), rsvpUser.Email.Substring(0,8)));
    }

    protected void lb_SendInvitations_Click(object sender, EventArgs e)
    {
        int inviteClass = (DateTime.Now <= Conference.Instance.PrimaryRegistrationClosed ? 1 : (DateTime.Now <= Conference.Instance.LateRegistrationClosed ? 2 : 3));
        int emailsSent = Conference.Instance.SendInvitations(sender, inviteClass, false);
    }

    protected void bn_SendReminder_Click(object sender, EventArgs e)
    {
        int inviteClass = (DateTime.Now <= Conference.Instance.PrimaryRegistrationClosed ? 1 : (DateTime.Now <= Conference.Instance.LateRegistrationClosed ? 2 : 3));
        int emailsSent = Conference.Instance.SendReminderEmails(sender, inviteClass, false);

    }


    protected void bn_UtilityModalSave_Click(object sender, EventArgs e)
    {
        KTConferenceUser user = setUser();
        
        if (hdn_UtilityModalPurpose.Value == "adminvite")
        {
            tb_UtilityModalEntry_Validate();
            if (!val_TextEntry.IsValid)
                lbl_Error.Text = "*Sorry, that email address is not on the current invite list.<br> Please double-check the spelling or contact the employee's Head of Engineering.";
            else
            {
                KTConferenceUser rsvpUser = new KTConferenceUser(tb_UtilityModalEntry.Text);
                Session["rsvp"] = null;
                redirectToRegistration(user, rsvpUser);
            }
        }
        
        if (hdn_UtilityModalPurpose.Value == "reminder")
        {
            //lbl_UtilityModalMessage.Text = "Emails sent: " + emailsSent;
            hdn_UtilityModalPurpose.Value = "info";
            return;
        }

        if (hdn_UtilityModalPurpose.Value == "delete")
        {
            RSVP rsvp = new RSVP(user, "Guest");
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
                Session["rsvp"] = rsvp;
                Response.Redirect("Confirmation.aspx");
            }
        }

        if (hdn_UtilityModalPurpose.Value == "decline")
        {
            string declineType = "1";

            RSVP rsvp = new RSVP(user, "Guest");
            if (rsvp.IsNew)
                declineType = "1";

            Session["rsvpUser"] = user;
            object retval = null;
            string reason = tb_UtilityModalEntry.Text;
            WebDataUtility.Instance.webAppCmd("sp_InviteeDecline", new GenericCmdParameter[] {
                new GenericCmdParameter("@conferenceID", Conference.Instance.ID), 
                new GenericCmdParameter("@userID", user.UserID), new GenericCmdParameter("@cancelReason", reason) }, ref retval);

            Response.Redirect("Confirmation.aspx?decline=" + declineType);
        }

        if (val_TextEntry.IsValid)
        {
            hdn_UtilityModalPurpose.Value = string.Empty;
            lbl_UtilityModalMessage.Text = string.Empty;
            tb_UtilityModalEntry.Text = string.Empty;
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
        Session["rsvpUser"] = null;

        hdn_UtilityModalPurpose.Value = string.Empty;
        lbl_UtilityModalMessage.Text = string.Empty;
        tb_UtilityModalEntry.Text = string.Empty;
        lbl_Error.Text = string.Empty;
        lbl_Error.Visible = false;
        mdl_UtilityModal.Hide();
    }


    protected void lb_AllocateTechPanels_Click(object sender, EventArgs e)
    {

        Dictionary<int, DataTable> techPanelAlloc = new Dictionary<int, DataTable>();

        DataTable techPanels = WebDataUtility.Instance.webAppTable("tbl_TechPanels");

        foreach (DataRow row in techPanels.Rows)
        {
            DataTable alloc = new DataTable("alloc" + DBNullable.ToString(row["eventID"]));

            alloc.Columns.Add(new DataColumn("eventID", typeof(int)));
            alloc.Columns.Add(new DataColumn("userID", typeof(int)));
            alloc.Columns.Add(new DataColumn("choice", typeof(int)));

            techPanelAlloc[DBNullable.ToInt(row["eventID"])] = alloc;
        }
        // Step 1. Assign first 40 (by reg date) into each tech panel
        //

        int i = 0;

        foreach (KeyValuePair<int, DataTable> pair in techPanelAlloc)
        {
            DataTable requests = WebDataUtility.Instance.webAppTable("sp_GetTechPanelRequestByEventID",
                new GenericCmdParameter[] { new GenericCmdParameter("@eventID", pair.Key), new GenericCmdParameter("@choice", 1) });

            foreach (DataRow row in requests.Rows)
            {
                object retval = null;

                DataRow newRow = pair.Value.NewRow();
                newRow["userID"] = DBNullable.ToInt(row["userID"]);
                newRow["eventID"] = pair.Key;
                newRow["choice"] = 1;
                pair.Value.Rows.Add(newRow);

                WebDataUtility.Instance.webAppCmd("sp_admin_AllocateTechPanel",
                    new GenericCmdParameter[] { new GenericCmdParameter("@userID", DBNullable.ToInt(row["userID"])),
                                                new GenericCmdParameter("@eventID", pair.Key),
                                                new GenericCmdParameter("@choice", 1) }, ref retval);
            }
        }
        i = 1;
        // Step 2. For Tech Panels with <= 10 audience members, assign those with panel as 2nd or 3rd choice
        foreach (KeyValuePair<int, DataTable> pair in techPanelAlloc)
        {
            if (pair.Value.Rows.Count > 10)
                continue;

            DataTable requests = WebDataUtility.Instance.webAppTable("sp_GetTechPanelRequestByEventID",
                new GenericCmdParameter[] { new GenericCmdParameter("@eventID", pair.Key), new GenericCmdParameter("@choice", 2) });

            foreach (DataRow row in requests.Rows)
            {
                object retval = null;
                DataRow newRow = pair.Value.NewRow();
                newRow["userID"] = DBNullable.ToInt(row["userID"]);
                newRow["eventID"] = pair.Key;
                newRow["choice"] = 2;
                pair.Value.Rows.Add(newRow);
                WebDataUtility.Instance.webAppCmd("sp_admin_AllocateTechPanel",
                    new GenericCmdParameter[] { new GenericCmdParameter("@userID", DBNullable.ToInt(row["userID"])),
                                                new GenericCmdParameter("@eventID", pair.Key),
                                                new GenericCmdParameter("@choice", 2) }, ref retval);
            }

            requests = null;

            requests = WebDataUtility.Instance.webAppTable("sp_GetTechPanelRequestByEventID",
                new GenericCmdParameter[] { new GenericCmdParameter("@eventID", pair.Key), new GenericCmdParameter("@choice", 3) });
            foreach (DataRow row in requests.Rows)
            {
                object retval = null;
                DataRow newRow = pair.Value.NewRow();
                newRow["userID"] = DBNullable.ToInt(row["userID"]);
                newRow["eventID"] = pair.Key;
                newRow["choice"] = 3;
                pair.Value.Rows.Add(newRow);

                WebDataUtility.Instance.webAppCmd("sp_admin_AllocateTechPanel",
                    new GenericCmdParameter[] { new GenericCmdParameter("@userID", DBNullable.ToInt(row["userID"])),
                                                new GenericCmdParameter("@eventID", pair.Key),
                                                new GenericCmdParameter("@choice", 3) }, ref retval);
            }


        }
        i = 2;
        //Step 3. For Panels < 40 members, assign second choice if available (has < 40)

        foreach (KeyValuePair<int, DataTable> pair in techPanelAlloc)
        {
            if (pair.Value.Rows.Count >= 40)
                continue;

            DataTable requests = WebDataUtility.Instance.webAppTable("sp_GetTechPanelRequestByEventID",
                new GenericCmdParameter[] { new GenericCmdParameter("@eventID", pair.Key), new GenericCmdParameter("@choice", 2) });

            foreach (DataRow row in requests.Rows)
            {
                object retval = null;
                DataRow newRow = pair.Value.NewRow();
                newRow["userID"] = DBNullable.ToInt(row["userID"]);
                newRow["eventID"] = pair.Key;
                newRow["choice"] = 2;
                pair.Value.Rows.Add(newRow);

                WebDataUtility.Instance.webAppCmd("sp_admin_AllocateTechPanel",
                    new GenericCmdParameter[] { new GenericCmdParameter("@userID", DBNullable.ToInt(row["userID"])),
                                                new GenericCmdParameter("@eventID", pair.Key),
                                                new GenericCmdParameter("@choice", 2) }, ref retval);
            }
        }
        i = 3;
        //Step 4. For Panels < 40 members, assign third choice if available (has < 40)

        foreach (KeyValuePair<int, DataTable> pair in techPanelAlloc)
        {
            if (pair.Value.Rows.Count >= 40)
                continue;

            DataTable requests = WebDataUtility.Instance.webAppTable("sp_GetTechPanelRequestByEventID",
                new GenericCmdParameter[] { new GenericCmdParameter("@eventID", pair.Key), new GenericCmdParameter("@choice", 3) });

            foreach (DataRow row in requests.Rows)
            {
                object retval = null;
                DataRow newRow = pair.Value.NewRow();
                newRow["userID"] = DBNullable.ToInt(row["userID"]);
                newRow["eventID"] = pair.Key;
                newRow["choice"] = 3;
                pair.Value.Rows.Add(newRow);

                WebDataUtility.Instance.webAppCmd("sp_admin_AllocateTechPanel",
                    new GenericCmdParameter[] { new GenericCmdParameter("@userID", DBNullable.ToInt(row["userID"])),
                                                new GenericCmdParameter("@eventID", pair.Key),
                                                new GenericCmdParameter("@choice", 3) }, ref retval);
            }
        }


        //Step 5. Get list of unallocated users = these users registered to late for the panels that they selected......

        i = 4;


    }


}

