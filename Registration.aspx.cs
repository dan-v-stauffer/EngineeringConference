using AppSecurity;
using ConferenceLibrary;
using DataUtilities;
using DataUtilities.SQLServer;
using DataUtilities.KTActiveDirectory;
using HelperFunctions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.DirectoryServices;
public partial class _Default : System.Web.UI.Page
{
    EncryptDecryptQueryString security = EncryptDecryptQueryString.Instance;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["page"] = this;

       

        if (!IsPostBack)
        {
            bn_Dummy.Focus();

            if (Session["user"] == null || Session["rsvpUser"] == null)
                HttpContext.Current.Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["RedirectToHome"]);

            KTConferenceUser user = (KTConferenceUser)Session["user"];
            KTConferenceUser rsvpUser = (KTConferenceUser)Session["rsvpUser"];

            if (user.Email != rsvpUser.Email)
                pnl_ExecAdmin.CssClass = "execAdmin";

            tb_UtilityModalEntry.Attributes["onfocus"] = "javascript:this.select();";

            string strReq = "";
            strReq = Request.RawUrl;
            strReq = strReq.Substring(strReq.IndexOf('?') + 1);
            string[] strUserName = null;

            try
            {
                string qs = security.Decrypt(strReq, rsvpUser.Email.Substring(0, 8));

                if (qs.Contains('='))
                    strUserName = qs.Split('=');

                if (strUserName.Length < 2)
                    throw new Exception("Invalid admin querystring.");
                if (strUserName[1].Length == 0)
                    throw new Exception("Invalid admin querystring.");
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["RedirectToHome"]);
            }

            if (!user.IsInvitee && user.Login != strUserName[1])
            {
                string msg = "Oops! We cannot find your name in the guest list. Please contact your divisional Head of Engineering " +
                     "to determine if the mistake is ours!<br><br>Are you registering for someone else? If so, please enter their KLA-Tencor email address." +
                     "<br><br> (Note: By cancelling you will exit this page and be directed to the 2015 Engineering Conference Website.)";
                showUtilityModalPopup("adminvite", "Registering for Someone Else?", msg, String.Empty, "OK", true);
            }
            else
            {
                loadForm();
            }

            //if(Convert.ToBoolean(Session["autoRefreshReg"]))             
            //         ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "refresh", "window.setTimeout('var url = window.location.href;window.location.href = url',1);", true);

            Session["autoRefreshReg"] = false;
            //up_TechPanelChoice.Update();
        }
        else
        {
            var ctrlName = Request.Params[Page.postEventSourceID];
            var args = Request.Params[Page.postEventArgumentID];

            HandleCustomPostbackEvent(ctrlName, args);
        }

        if (ScriptManager.GetCurrent(Page).IsInAsyncPostBack)
        {
            rbl_TransporationOptions_SelectedIndexChanged(rbl_MonteryInbound, new EventArgs());
            rbl_TransporationOptions_SelectedIndexChanged(rbl_MonteryOutbound, new EventArgs());
        }
    }

    private void HandleCustomPostbackEvent(string ctrlName, string args)
    {
        //Since this will get called for every postback, we only
        // want to handle a specific combination of control
        // and argument.
        RSVP rsvp = (RSVP)Session["rsvp"];
        if (args == "OnBlur")
        {
            switch (ctrlName)
            {
                case ("tb_FirstName"):
                    {
                        if (rsvp.User.FirstName != tb_FirstName.Text)
                        {
                            rsvp.User.FirstName = tb_FirstName.Text;
                            //tb_LastName.Focus();
                        }
                        break;
                    }
                case ("tb_LastName"):
                    {
                        if (rsvp.User.LastName != tb_LastName.Text)
                        {
                            rsvp.User.LastName = tb_LastName.Text;
                            //cbo_Division.Focus();
                        }
                        break;
                    }
                case ("tb_MobilePhone"):
                    {
                        rsvp.User.MobilePhone = tb_MobilePhone.Text;
                        //rb_ShirtType.Focus();
                        break;
                    }
                case ("tb_SpecialNeeds"):
                    {
                        rsvp.User.SpecialNeeds = tb_SpecialNeeds.Text;
                        //dl_TechPanelsList.Focus();
                        break;
                    }
                case ("tb_FoodRestrictions"):
                    {
                        rsvp.User.FoodAllergies = tb_FoodRestrictions.Text;
                        //tb_SpecialNeeds.Focus();
                        break;
                    }
            }
        }

    }

    protected void Page_PreRenderComplete(object sender, EventArgs e)
    {
        Label lbl = (Label)progress_Modal.FindControl("lbl_ModalProgress");

        if (hdn_UtilityModalPurpose.Value == "nonvite" && !val_TextEntry.IsValid)
            lbl.Text = "Redirecting you to conference website...";
        else
            lbl.Text = "Working...";
    }

    protected void up_Transportation_Load(object sender, EventArgs e)
    {

    }

    protected void upModal_PreRenderComplete(object sender, EventArgs e)
    {
        Label lbl = (Label)progress_Modal.FindControl("lbl_ModalProgress");

        if (hdn_UtilityModalPurpose.Value == "nonvite" && !val_TextEntry.IsValid)
            lbl.Text = "Redirecting you to conference website...";
        else
            lbl.Text = "Working...";
    }


    protected void dl_PaperSymposiaList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Conference conference = (Conference)Application["Conference"];
            DataRowView drv = (DataRowView)e.Item.DataItem;
            DateTime startTime = Convert.ToDateTime(drv["eventStart"]);
            DateTime stopTime = Convert.ToDateTime(drv["eventStop"]);

            Label lbl_SymposiaGroupTitle = (Label)e.Item.FindControl("lbl_SymposiaGroupTitle");
            lbl_SymposiaGroupTitle.Text = drv["eventType"].ToString() + " " + drv["roman"].ToString();

            Label lbl_SymposiaGroupTimes = (Label)e.Item.FindControl("lbl_SymposiaGroupTimes");

            lbl_SymposiaGroupTimes.Text = startTime.ToString("dddd, MMMM d, yyyy h:mm") + " - " + stopTime.ToString("h:mm tt");

            GenericCmdParameter[] parameters = {new GenericCmdParameter("@conferenceID", conference.ID),
                                                   new GenericCmdParameter("@paperStart", Convert.ToDateTime(drv["eventStart"]))};


            DataList dl_PaperTimeSlots = (DataList)e.Item.FindControl("dl_PaperTimeSlots");


            dl_PaperTimeSlots.DataSource = WebDataUtility.Instance.webAppTable("sp_GetPapersByRoomAndDate_Crosstab", parameters);


            dl_PaperTimeSlots.DataBind();


        }
    }

    private string[] getPaperTitleItems(string input)
    {
        List<string> retval = new List<string>();

        string[] temp = input.Split('|');

        foreach (string s in temp)
            retval.Add(Common.ProperCase(s));

        if (retval.Count == 1)
        {
            retval.Add("-1");
            retval.Add("-1");
        }
        return retval.ToArray();
    }


    private void assignCheckBoxAttributes(ref CheckBox cb, String paperID, string eventID, String type, DateTime paperTime, string parentContainer)
    {
        cb.Attributes.Add("type", type);
        cb.Attributes.Add("paperID", paperID);
        cb.Attributes.Add("eventID", eventID);
        cb.Attributes.Add("paperTime", paperTime.ToString("MM/dd/yyyy h:mm tt"));
        cb.Attributes.Add("parentContainer", parentContainer);
    }

    private void assignPaperTitleLabelAttributes(ref Label lbl, String paperID, DateTime paperTime, String room)
    {
        lbl.Attributes.Add("paperID", paperID);
        lbl.Attributes.Add("paperTime", paperTime.ToString("MM/dd/yyyy h:mm tt"));
        lbl.Attributes.Add("room", room);
    }


    protected void dl_PaperTimeSlots_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataList me = (DataList)sender;

            AjaxControlToolkit.ToolkitScriptManager ajaxScriptManager = (AjaxControlToolkit.ToolkitScriptManager)Master.FindControl("ajaxScriptManager");

            DataRowView drv = (DataRowView)e.Item.DataItem;
            DateTime timeSlot = Convert.ToDateTime(drv["paperStart"]);
            DataTable paperRankOrderTable = null;
            DataRow row = null;
            int firstChoice = 0;
            int secondChoice = 0;


            me.Attributes.Add("timeSlotEventID", Convert.ToString(drv["timeSlotEventID"]));

            int timeSlotID = Convert.ToInt32(drv["timeSlotEventID"]);

            RSVP rsvp = (RSVP)Session["rsvp"];

            bool enableCheckBoxes = (rsvp.Events.Select("parentEventID=" + timeSlotID + " AND eventAssigned=true").Length == 0);

            if (Session["paperRankOrder"] == null)
                paperRankOrderTable = createPaperRankOrderTable();
            else
            {
                paperRankOrderTable = (DataTable)Session["paperRankOrder"];
                row = paperRankOrderTable.Rows.Find(timeSlot);

                if (row != null)
                {
                    firstChoice = Convert.ToInt32(row["firstChoice"]);
                    secondChoice = Convert.ToInt32(row["secondChoice"]);
                }
            }

            if (row == null)
            {
                DataRow newRow = paperRankOrderTable.NewRow();
                newRow["timeSlot"] = timeSlot;
                newRow["timeSlotID"] = timeSlotID;
                newRow["firstChoice"] = firstChoice;
                newRow["secondChoice"] = secondChoice;
                newRow["locked"] = false;


                if (rsvp.Events.Rows.Count > 0)
                {
                    DataRow[] rows = rsvp.Events.Select("parentEventID = " + timeSlotID);
                    if (rows.Length > 0)
                    {
                        foreach (DataRow paper in rows)
                        {
                            //if(
                            //firstChoice = Convert.ToInt32(row["eventID"]);
                            //secondChoice = Convert.ToInt32(row["eventID"]);
                            if (Convert.ToInt32(paper["eventRequestOrder"]) == 1)
                            {
                                firstChoice = Convert.ToInt32(paper["eventID"]);
                                newRow["firstChoice"] = firstChoice;
                            }
                            else if (Convert.ToInt32(paper["eventRequestOrder"]) == 2)
                            {
                                secondChoice = Convert.ToInt32(paper["eventID"]);
                                newRow["secondChoice"] = secondChoice;
                            }
                            newRow["locked"] = (Convert.ToBoolean(paper["eventAssigned"]) == true);
                        }
                    }
                }
                //enableCheckBoxes = !Convert.ToBoolean(newRow["locked"]);

                paperRankOrderTable.Rows.Add(newRow);
                Session["paperRankOrder"] = paperRankOrderTable;
            }

            Label lbl_PaperTimeStart = (Label)e.Item.FindControl("lbl_PaperTimeStart");
            lbl_PaperTimeStart.Text = timeSlot.ToString("h:mm tt");
            lbl_PaperTimeStart.Attributes.Add("paperTimeSlot", timeSlot.ToString("MM/dd/yyyy h:mm tt"));


            HtmlTableCell td1 = (HtmlTableCell)e.Item.FindControl("td_paper1");
            HtmlTableCell td2 = (HtmlTableCell)e.Item.FindControl("td_paper2");
            HtmlTableCell td3 = (HtmlTableCell)e.Item.FindControl("td_paper3");
            HtmlTableCell td4 = (HtmlTableCell)e.Item.FindControl("td_paper4");
            HtmlTableCell td5 = (HtmlTableCell)e.Item.FindControl("td_paper5");


            HtmlGenericControl divBox1 = (HtmlGenericControl)e.Item.FindControl("divPaperOptionBox1");
            HtmlGenericControl divBox2 = (HtmlGenericControl)e.Item.FindControl("divPaperOptionBox2");
            HtmlGenericControl divBox3 = (HtmlGenericControl)e.Item.FindControl("divPaperOptionBox3");
            HtmlGenericControl divBox4 = (HtmlGenericControl)e.Item.FindControl("divPaperOptionBox4");
            HtmlGenericControl divBox5 = (HtmlGenericControl)e.Item.FindControl("divPaperOptionBox5");

            Label lbl_PaperTitleRm1 = (Label)e.Item.FindControl("lbl_PaperTitleRm1");
            string[] paper1 = getPaperTitleItems(Convert.ToString(drv[1]));
            if (paper1[0].Trim() == String.Empty)
                td1.Attributes.CssStyle.Add("background-image", "url(Images/nodata.png);");
            lbl_PaperTitleRm1.Text = paper1[0].Trim();
            Label lbl_PaperCatRm1 = (Label)e.Item.FindControl("lbl_PaperCatRm1");
            lbl_PaperCatRm1.Text = paper1[3].Trim();

            if (paper1.Length > 1)
            {
                assignPaperTitleLabelAttributes(ref lbl_PaperTitleRm1, paper1[1].Trim(),
                    timeSlot, drv.DataView.Table.Columns[1].ColumnName);
            }


            CheckBox cb_FirstChoicePaperRm1 = (CheckBox)e.Item.FindControl("cb_FirstChoicePaperRm1");
            cb_FirstChoicePaperRm1.Checked = (Convert.ToInt32(paper1[2].Trim()) == firstChoice);
            if (cb_FirstChoicePaperRm1.Checked)
                td1.Attributes.CssStyle.Add("background-color", "#98FB98");
            assignCheckBoxAttributes(ref cb_FirstChoicePaperRm1, paper1[1].Trim(), paper1[2].Trim(), "firstChoice", timeSlot, td1.ClientID);
            divBox1.Attributes.CssStyle.Add("visibility", paper1[0].Trim() == String.Empty ? "hidden" : "visible");
            cb_FirstChoicePaperRm1.Enabled = enableCheckBoxes;
            Panel pnl_ModOrMember1 = (Panel)e.Item.FindControl("pnl_ModOrMember1");
            pnl_ModOrMember1.Visible = (rsvp.Events.Select("eventID = " + paper1[2].Trim() + " AND userRole='speaker' AND eventAssigned=true").Length > 0);


            CheckBox cb_SecondChoicePaperRm1 = (CheckBox)e.Item.FindControl("cb_SecondChoicePaperRm1");
            cb_SecondChoicePaperRm1.Checked = (Convert.ToInt32(paper1[2].Trim()) == secondChoice);
            if (cb_SecondChoicePaperRm1.Checked)
                td1.Attributes.CssStyle.Add("background-color", "#D5D8F5");
            assignCheckBoxAttributes(ref cb_SecondChoicePaperRm1, paper1[1].Trim(), paper1[2].Trim(), "secondChoice", timeSlot, td1.ClientID);
            cb_SecondChoicePaperRm1.Enabled = enableCheckBoxes;

            Label lbl_PaperTitleRm2 = (Label)e.Item.FindControl("lbl_PaperTitleRm2");
            string[] paper2 = getPaperTitleItems(Convert.ToString(drv[2]));
            if (paper2[0].Trim() == String.Empty)
                td2.Attributes.CssStyle.Add("background-image", "url(Images/nodata.png);");
            lbl_PaperTitleRm2.Text = paper2[0].Trim();
            assignPaperTitleLabelAttributes(ref lbl_PaperTitleRm2, paper2[1].Trim(),
                    timeSlot, drv.DataView.Table.Columns[2].ColumnName);
            divBox2.Attributes.CssStyle.Add("visibility", paper2[0].Trim() == String.Empty ? "hidden" : "visible");
            Panel pnl_ModOrMember2 = (Panel)e.Item.FindControl("pnl_ModOrMember2");
            pnl_ModOrMember2.Visible = (rsvp.Events.Select("eventID = " + paper2[2].Trim() + " AND userRole='speaker' AND eventAssigned=true").Length > 0);
            Label lbl_PaperCatRm2 = (Label)e.Item.FindControl("lbl_PaperCatRm2");
            lbl_PaperCatRm2.Text = paper2[3].Trim();

            CheckBox cb_FirstChoicePaperRm2 = (CheckBox)e.Item.FindControl("cb_FirstChoicePaperRm2");
            cb_FirstChoicePaperRm2.Checked = (Convert.ToInt32(paper2[2].Trim()) == firstChoice);
            if (cb_FirstChoicePaperRm2.Checked)
                td2.Attributes.CssStyle.Add("background-color", "#98FB98");
            assignCheckBoxAttributes(ref cb_FirstChoicePaperRm2, paper2[1].Trim(), paper2[2].Trim(), "firstChoice", timeSlot, td2.ClientID);
            cb_FirstChoicePaperRm2.Enabled = enableCheckBoxes;

            CheckBox cb_SecondChoicePaperRm2 = (CheckBox)e.Item.FindControl("cb_SecondChoicePaperRm2");
            cb_SecondChoicePaperRm2.Checked = (Convert.ToInt32(paper2[2].Trim()) == secondChoice);
            if (cb_SecondChoicePaperRm2.Checked)
                td2.Attributes.CssStyle.Add("background-color", "#D5D8F5");
            assignCheckBoxAttributes(ref cb_SecondChoicePaperRm2, paper2[1].Trim(), paper2[2].Trim(), "secondChoice", timeSlot, td2.ClientID);
            cb_SecondChoicePaperRm2.Enabled = enableCheckBoxes;

            Label lbl_PaperTitleRm3 = (Label)e.Item.FindControl("lbl_PaperTitleRm3");
            string[] paper3 = getPaperTitleItems(Convert.ToString(drv[3]));
            if (paper3[0].Trim() == String.Empty)
                td3.Attributes.CssStyle.Add("background-image", "url(Images/nodata.png);");
            lbl_PaperTitleRm3.Text = paper3[0].Trim();
            assignPaperTitleLabelAttributes(ref lbl_PaperTitleRm3, paper3[1].Trim(),
                    timeSlot, drv.DataView.Table.Columns[3].ColumnName);
            divBox3.Attributes.CssStyle.Add("visibility", paper3[0].Trim() == String.Empty ? "hidden" : "visible");
            Panel pnl_ModOrMember3 = (Panel)e.Item.FindControl("pnl_ModOrMember3");
            pnl_ModOrMember3.Visible = (rsvp.Events.Select("eventID = " + paper3[2].Trim() + " AND userRole='speaker' AND eventAssigned=true").Length > 0);

            CheckBox cb_FirstChoicePaperRm3 = (CheckBox)e.Item.FindControl("cb_FirstChoicePaperRm3");
            cb_FirstChoicePaperRm3.Checked = (Convert.ToInt32(paper3[2].Trim()) == firstChoice);
            if (cb_FirstChoicePaperRm3.Checked)
                td3.Attributes.CssStyle.Add("background-color", "#98FB98");
            assignCheckBoxAttributes(ref cb_FirstChoicePaperRm3, paper3[1].Trim(), paper3[2].Trim(), "firstChoice", timeSlot, td3.ClientID);
            cb_FirstChoicePaperRm3.Enabled = enableCheckBoxes;

            CheckBox cb_SecondChoicePaperRm3 = (CheckBox)e.Item.FindControl("cb_SecondChoicePaperRm3");
            cb_SecondChoicePaperRm3.Checked = (Convert.ToInt32(paper3[2].Trim()) == secondChoice);
            if (cb_SecondChoicePaperRm3.Checked)
                td3.Attributes.CssStyle.Add("background-color", "#D5D8F5");
            assignCheckBoxAttributes(ref cb_SecondChoicePaperRm3, paper3[1].Trim(), paper3[2].Trim(), "secondChoice", timeSlot, td3.ClientID);
            cb_SecondChoicePaperRm3.Enabled = enableCheckBoxes;
            Label lbl_PaperCatRm3 = (Label)e.Item.FindControl("lbl_PaperCatRm3");
            lbl_PaperCatRm3.Text = paper3[3].Trim();

            Label lbl_PaperTitleRm4 = (Label)e.Item.FindControl("lbl_PaperTitleRm4");
            string[] paper4 = getPaperTitleItems(Convert.ToString(drv[4]));
            if (paper4[0].Trim() == String.Empty)
                td4.Attributes.CssStyle.Add("background-image", "url(Images/nodata.png);");

            lbl_PaperTitleRm4.Text = paper4[0].Trim();
            assignPaperTitleLabelAttributes(ref lbl_PaperTitleRm4, paper4[1].Trim(),
                    timeSlot, drv.DataView.Table.Columns[4].ColumnName);
            divBox4.Attributes.CssStyle.Add("visibility", paper4[0].Trim() == String.Empty ? "hidden" : "visible");
            Panel pnl_ModOrMember4 = (Panel)e.Item.FindControl("pnl_ModOrMember4");
            pnl_ModOrMember4.Visible = (rsvp.Events.Select("eventID = " + paper4[2].Trim() + " AND userRole='speaker' AND eventAssigned=true").Length > 0);

            CheckBox cb_FirstChoicePaperRm4 = (CheckBox)e.Item.FindControl("cb_FirstChoicePaperRm4");
            cb_FirstChoicePaperRm4.Checked = (Convert.ToInt32(paper4[2].Trim()) == firstChoice);
            if (cb_FirstChoicePaperRm4.Checked)
                td4.Attributes.CssStyle.Add("background-color", "#98FB98");
            assignCheckBoxAttributes(ref cb_FirstChoicePaperRm4, paper4[1].Trim(), paper4[2].Trim(), "firstChoice", timeSlot, td4.ClientID);
            cb_FirstChoicePaperRm4.Enabled = enableCheckBoxes;

            CheckBox cb_SecondChoicePaperRm4 = (CheckBox)e.Item.FindControl("cb_SecondChoicePaperRm4");
            cb_SecondChoicePaperRm4.Checked = (Convert.ToInt32(paper4[2].Trim()) == secondChoice);
            if (cb_SecondChoicePaperRm4.Checked)
                td4.Attributes.CssStyle.Add("background-color", "#D5D8F5");
            assignCheckBoxAttributes(ref cb_SecondChoicePaperRm4, paper4[1].Trim(), paper4[2].Trim(), "secondChoice", timeSlot, td4.ClientID);
            cb_SecondChoicePaperRm4.Enabled = enableCheckBoxes;
            Label lbl_PaperCatRm4 = (Label)e.Item.FindControl("lbl_PaperCatRm4");
            lbl_PaperCatRm4.Text = paper4[3].Trim();

            Label lbl_PaperTitleRm5 = (Label)e.Item.FindControl("lbl_PaperTitleRm5");
            string[] paper5 = getPaperTitleItems(Convert.ToString(drv[5]));

            if (paper5[0].Trim() == String.Empty)
                td5.Attributes.CssStyle.Add("background-image", "url(Images/nodata.png);");

            lbl_PaperTitleRm5.Text = paper5[0].Trim();
            assignPaperTitleLabelAttributes(ref lbl_PaperTitleRm5, paper5[1].Trim(),
                    timeSlot, drv.DataView.Table.Columns[5].ColumnName);
            divBox5.Attributes.CssStyle.Add("visibility", paper5[0].Trim() == String.Empty ? "hidden" : "visible");
            Panel pnl_ModOrMember5 = (Panel)e.Item.FindControl("pnl_ModOrMember5");
            pnl_ModOrMember5.Visible = (rsvp.Events.Select("eventID = " + paper5[2].Trim() + " AND userRole='Speaker' AND eventAssigned=true").Length > 0);

            CheckBox cb_FirstChoicePaperRm5 = (CheckBox)e.Item.FindControl("cb_FirstChoicePaperRm5");
            cb_FirstChoicePaperRm5.Checked = (Convert.ToInt32(paper5[2].Trim()) == firstChoice);
            if (cb_FirstChoicePaperRm5.Checked)
                td5.Attributes.CssStyle.Add("background-color", "#98FB98");
            assignCheckBoxAttributes(ref cb_FirstChoicePaperRm5, paper5[1].Trim(), paper5[2].Trim(), "firstChoice", timeSlot, td5.ClientID);
            cb_FirstChoicePaperRm5.Enabled = enableCheckBoxes;

            CheckBox cb_SecondChoicePaperRm5 = (CheckBox)e.Item.FindControl("cb_SecondChoicePaperRm5");
            cb_SecondChoicePaperRm5.Checked = (Convert.ToInt32(paper5[2].Trim()) == secondChoice);
            if (cb_SecondChoicePaperRm5.Checked)
                td5.Attributes.CssStyle.Add("background-color", "#D5D8F5");
            assignCheckBoxAttributes(ref cb_SecondChoicePaperRm5, paper5[1].Trim(), paper5[2].Trim(), "secondChoice", timeSlot, td5.ClientID);
            cb_SecondChoicePaperRm5.Enabled = enableCheckBoxes;
            Label lbl_PaperCatRm5 = (Label)e.Item.FindControl("lbl_PaperCatRm5");
            lbl_PaperCatRm5.Text = paper5[3].Trim();


            HtmlAnchor hl_PaperDetail_1 = (HtmlAnchor)e.Item.FindControl("hl_PaperDetail_1");
            if (paper1[0].Trim() == String.Empty)
                hl_PaperDetail_1.Attributes["class"] = "hidden";
            else
            {
                hl_PaperDetail_1.Attributes.Add("rel", "Balloon.aspx?type=paper&id=" + paper1[1].Trim());
                hl_PaperDetail_1.Attributes["class"] += " cluetip-clicked";
            }

            HtmlAnchor hl_PaperDetail_2 = (HtmlAnchor)e.Item.FindControl("hl_PaperDetail_2");
            if (paper2[0].Trim() == String.Empty)
                hl_PaperDetail_2.Attributes["class"] = "hidden";
            else
            {
                hl_PaperDetail_2.Attributes.Add("rel", "Balloon.aspx?type=paper&id=" + paper2[1].Trim());
                hl_PaperDetail_2.Attributes["class"] += " cluetip-clicked";
            }

            HtmlAnchor hl_PaperDetail_3 = (HtmlAnchor)e.Item.FindControl("hl_PaperDetail_3");
            if (paper3[0].Trim() == String.Empty)
                hl_PaperDetail_3.Attributes["class"] = "hidden";
            else
            {
                hl_PaperDetail_3.Attributes.Add("rel", "Balloon.aspx?type=paper&id=" + paper3[1].Trim());
                hl_PaperDetail_3.Attributes["class"] += " cluetip-clicked";
            }

            HtmlAnchor hl_PaperDetail_4 = (HtmlAnchor)e.Item.FindControl("hl_PaperDetail_4");
            if (paper4[0].Trim() == String.Empty)
                hl_PaperDetail_4.Attributes["class"] = "hidden";
            else
            {
                hl_PaperDetail_4.Attributes.Add("rel", "Balloon.aspx?type=paper&id=" + paper4[1].Trim());
                hl_PaperDetail_4.Attributes["class"] += " cluetip-clicked";
            }

            HtmlAnchor hl_PaperDetail_5 = (HtmlAnchor)e.Item.FindControl("hl_PaperDetail_5");
            if (paper5[0].Trim() == String.Empty)
                hl_PaperDetail_5.Attributes["class"] = "hidden";
            else
            {
                hl_PaperDetail_5.Attributes.Add("rel", "Balloon.aspx?type=paper&id=" + paper5[1].Trim());
                hl_PaperDetail_5.Attributes["class"] += " cluetip-clicked";
            }
        }
    }

    protected void dl_Papers_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataList parent = (DataList)sender;
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Label lbl_PaperTitle = (Label)e.Item.FindControl("lbl_PaperTitle");
            CheckBox first = (CheckBox)e.Item.FindControl("cb_FirstChoicePaper");
            CheckBox second = (CheckBox)e.Item.FindControl("cb_SecondChoicePaper");

            if (drv["paperID"].GetType() == typeof(DBNull))
            {
                lbl_PaperTitle.Text = "To be assigned";
                first.Visible = false;
                second.Visible = false;
            }
            else
            {

                lbl_PaperTitle.Text = Convert.ToString(drv["paperTitle"]);
                lbl_PaperTitle.Attributes.Add("paperID", Convert.ToString(drv["paperID"]));

                first.Attributes.Add("paperID", Convert.ToString(drv["paperID"]));
                first.Attributes.Add("cbType", "firstChoice");
                first.Attributes.Add("timeSlot", parent.Attributes["timeSlot"]);
                second.Attributes.Add("paperID", Convert.ToString(drv["paperID"]));
                second.Attributes.Add("cbType", "secondChoice");
                second.Attributes.Add("timeSlot", parent.Attributes["timeSlot"]);

                String timeSlot = ((DataList)sender).Attributes["timeSlot"];

                DataTable paperRankOrderTable = null;
                if (Session["paperRankOrder"] == null)
                    paperRankOrderTable = createPaperRankOrderTable();
                else
                    paperRankOrderTable = (DataTable)Session["paperRankOrder"];

                DataRow row = paperRankOrderTable.Rows.Find(Convert.ToInt32(drv["paperID"]));
                if (row == null)
                {
                    row = paperRankOrderTable.NewRow();

                    row["paperID"] = Convert.ToInt32(drv["paperID"]);
                    row["timeSlot"] = timeSlot;
                    row["firstChoice"] = false;
                    row["secondChoice"] = false;

                    paperRankOrderTable.Rows.Add(row);
                    paperRankOrderTable.AcceptChanges();
                    Session["paperRankOrder"] = paperRankOrderTable;

                }
                else
                {

                    first.Checked = Convert.ToBoolean(row["firstChoice"]);
                    second.Checked = Convert.ToBoolean(row["secondChoice"]);
                }
            }
        }
    }

    protected void dl_PaperSymposiaChoices_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Label lbl_SymposiumName = (Label)e.Item.FindControl("lbl_SymposiumName");
            int symposiumID = Convert.ToInt32(drv["symposiumID"]);
            lbl_SymposiumName.Text = drv["paperSymposiumTitle"].ToString();
            lbl_SymposiumName.ToolTip = drv["paperSymposiumDescription"].ToString();
            string attributeValue = ((DataList)sender).Attributes["symposiumID"];

            Dictionary<int, Dictionary<string, RadioButton>> dictionary = (Dictionary<int, Dictionary<string, RadioButton>>)Session[attributeValue];
            Dictionary<string, RadioButton> rowButtons = new Dictionary<string, RadioButton>();

            RadioButton rbSymposium1st = (RadioButton)e.Item.FindControl("rbSymposium1st");
            RadioButton rbSymposium2nd = (RadioButton)e.Item.FindControl("rbSymposium2nd");

            rbSymposium1st.Attributes.Add("symposiumID", attributeValue);
            rbSymposium2nd.Attributes.Add("symposiumID", attributeValue);

            rbSymposium1st.Attributes.Add("value", "1");
            rbSymposium2nd.Attributes.Add("value", "2");

            rowButtons[rbSymposium1st.ClientID] = rbSymposium1st;
            rowButtons[rbSymposium2nd.ClientID] = rbSymposium2nd;

            if (!dictionary.ContainsKey(symposiumID))
                dictionary[symposiumID] = rowButtons;
            else
            {
                rbSymposium1st.Checked = dictionary[symposiumID][rbSymposium1st.ClientID].Checked;
                rbSymposium2nd.Checked = dictionary[symposiumID][rbSymposium2nd.ClientID].Checked;
            }
        }
    }

    protected void dl_TechPanelsList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataList me = (DataList)sender;
            Conference conference = (Conference)Application["Conference"];
            DataRowView drv = (DataRowView)e.Item.DataItem;
            DateTime startTime = Convert.ToDateTime(drv["eventStart"]);
            int parentEventID = (drv["eventID"] as int?) ?? 0;

            Label lbl_TechPanelGroupTitle = (Label)e.Item.FindControl("lbl_TechPanelGroupTitle");
            lbl_TechPanelGroupTitle.Text = drv["eventText"].ToString() + " - " + startTime.ToShortDateString();

            DataList dl_TechPanelChoices = (DataList)e.Item.FindControl("dl_TechPanelChoices");

            GenericCmdParameter[] parameters = {new GenericCmdParameter("@conferenceID", conference.ID),
            new GenericCmdParameter("@startTime", startTime)};

            string techPanelID = lbl_TechPanelGroupTitle.Text + Convert.ToString(drv["roman"]);
            dl_TechPanelChoices.Attributes.Add("TechPanelID", techPanelID);

            if (Session[techPanelID] == null)
                Session[techPanelID] = new Dictionary<int, Dictionary<string, RadioButton>>();

            dl_TechPanelChoices.DataSource = WebDataUtility.Instance.webAppTable("sp_GetTechPanelsByStartTime", parameters);
            dl_TechPanelChoices.Attributes.Add("parentEventID", Convert.ToString(parentEventID));
            dl_TechPanelChoices.DataBind();
        }
    }

    protected void dl_TechPanelChoices_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataList me = (DataList)sender;
            RSVP rsvp = (RSVP)Session["rsvp"];

            DataRowView drv = (DataRowView)e.Item.DataItem;

            int parentEventID = Convert.ToInt32(me.Attributes["parentEventID"]);

            int techPanelID = Convert.ToInt32(drv["techpanelID"]);
            Label lbl_TechPanelTitle = (Label)e.Item.FindControl("lbl_TechPanelTitle");
            lbl_TechPanelTitle.Text = (drv["techpanelTitle"].ToString());
            //         lbl_TechPanelTitle.ToolTip = drv["techpanelDescription"].ToString();

            Label lbl_TechPanelTopic = (Label)e.Item.FindControl("lbl_TechPanelTopic");
            lbl_TechPanelTopic.Text = drv["techpanelTopic"].ToString();

            string attributeValue = ((DataList)sender).Attributes["techpanelID"];

            DataTable techPanelRankOrderTable = null;
            DataRow row = null;

            bool firstChoice = false;
            bool secondChoice = false;
            bool thirdChoice = false;
            bool locked = false;



            if (Session["techPanelRankOrder"] == null)
                techPanelRankOrderTable = createTechPanelRankOrderTable();
            else
            {
                techPanelRankOrderTable = (DataTable)Session["techPanelRankOrder"];
            }



            row = techPanelRankOrderTable.Rows.Find(techPanelID);

            if (row != null)
            {
                firstChoice = Convert.ToBoolean(row["firstChoice"]);
                secondChoice = Convert.ToBoolean(row["secondChoice"]);
                thirdChoice = Convert.ToBoolean(row["thirdChoice"]);
            }
            else
            {
                DataRow newRow = techPanelRankOrderTable.NewRow();
                newRow["techPanelID"] = techPanelID;
                newRow["parentEventID"] = parentEventID;
                newRow["firstChoice"] = firstChoice;
                newRow["secondChoice"] = secondChoice;
                newRow["thirdChoice"] = thirdChoice;
                techPanelRankOrderTable.Rows.Add(newRow);
                Session["techPanelRankOrder"] = techPanelRankOrderTable;
            }

            DataRow[] selections = rsvp.Events.Select("eventID = " + techPanelID);

            locked = rsvp.Events.Select("parentEventID= " + parentEventID + " AND eventAssigned=true").Length > 0;

            if (selections.Length > 0)
            {
                string role = Convert.ToString(selections[0]["userRole"]);

                if (role.ToLower().Contains("moderator") || role.ToLower().Contains("member"))
                {
                    Panel pnlModorMember = (Panel)e.Item.FindControl("pnl_ModOrMember");
                    Label lblModOrMmeber = (Label)e.Item.FindControl("lbl_ModOrMember");
                    if (pnlModorMember != null)
                    {
                        pnlModorMember.CssClass = "floaterL";
                        lblModOrMmeber.Text = role.ToLower().Contains("moderator") ? "You're moderating this panel!" : "You're a member on this panel!";
                    }
                }

                //locked = (Convert.ToBoolean(selections[0]["eventAssigned"]));
                int eventID = Convert.ToInt32(selections[0]["eventID"]);
                row = techPanelRankOrderTable.Rows.Find(eventID);
                if (row != null)
                {
                    if (techPanelID == eventID)
                    {
                        row["firstChoice"] = (Convert.ToInt32(selections[0]["eventRequestOrder"]) == 1);
                        row["secondChoice"] = (Convert.ToInt32(selections[0]["eventRequestOrder"]) == 2);
                        row["thirdChoice"] = (Convert.ToInt32(selections[0]["eventRequestOrder"]) == 3);

                        firstChoice = Convert.ToBoolean(row["firstChoice"]);
                        secondChoice = Convert.ToBoolean(row["secondChoice"]);
                        thirdChoice = Convert.ToBoolean(row["thirdChoice"]);
                    }
                }
            }


            RadioButton rbTechPanel1st = (RadioButton)e.Item.FindControl("rbTechPanel1st");
            RadioButton rbTechPanel2nd = (RadioButton)e.Item.FindControl("rbTechPanel2nd");
            RadioButton rbTechPanel3rd = (RadioButton)e.Item.FindControl("rbTechPanel3rd");

            HtmlGenericControl container = (HtmlGenericControl)e.Item.FindControl("div_TechPanelItem");

            HtmlAnchor hl_TechPanelDetail = (HtmlAnchor)e.Item.FindControl("hl_TechPanelDetail");
            hl_TechPanelDetail.Attributes.Add("rel", "Balloon.aspx?type=techpanel&id=" + Convert.ToString(techPanelID));
            hl_TechPanelDetail.Attributes["class"] += " cluetip-clicked";

            rbTechPanel1st.Attributes.Add("techPanelID", Convert.ToString(techPanelID));
            rbTechPanel2nd.Attributes.Add("techPanelID", Convert.ToString(techPanelID));
            rbTechPanel3rd.Attributes.Add("techPanelID", Convert.ToString(techPanelID));

            rbTechPanel1st.Attributes.Add("type", "1");
            rbTechPanel2nd.Attributes.Add("type", "2");
            rbTechPanel3rd.Attributes.Add("type", "3");

            rbTechPanel1st.Checked = firstChoice;
            rbTechPanel2nd.Checked = secondChoice;
            rbTechPanel3rd.Checked = thirdChoice;

            rbTechPanel1st.Enabled = !locked;
            rbTechPanel2nd.Enabled = !locked;
            rbTechPanel3rd.Enabled = !locked;

            if (container != null)
            {
                if (rbTechPanel1st.Checked)
                    container.Attributes.CssStyle.Add("background-color", "#98FB98");
                else if (rbTechPanel2nd.Checked)
                    container.Attributes.CssStyle.Add("background-color", "#D5D8F5");
                else if (rbTechPanel3rd.Checked)
                    container.Attributes.CssStyle.Add("background-color", "#FFE4B5");

            }

        }

    }

    protected void verifyDeepLearning(RadioButtonList list)
    {
        if (!rbl_DeepLearning.Items[0].Selected)
            return;

        RSVP rsvp = (RSVP)Session["rsvp"];
        if (Session["techPanelRankOrder"] == null)
            return;

        DataTable data = (DataTable)Session["techPanelRankOrder"];
        foreach (DataRow row in data.Rows)
        {
            if (Convert.ToInt32(row[0]) != 274)
                continue;
            else
                if (Convert.ToBoolean(row[2]) || Convert.ToBoolean(row[3]) || Convert.ToBoolean(row[4]))
                {
                    StringBuilder builder = new StringBuilder();
                    builder.Append("You have already signed up for the optional 'Deep Learning' tutorial on Sunday, October 25 from 1-4PM.");
                    builder.Append(Environment.NewLine);
                    builder.Append("Much of the content will be similar.");
                    builder.Append(Environment.NewLine);
                    builder.Append("You may want to select one or the other.");

                    showUtilityModalPopup("deeplearning", "Deep Learning Tutorial", builder.ToString().Replace(Environment.NewLine, "<br />"), string.Empty, "OK", false);
                }
        }
    }

    protected void verifyDeepLearning(RadioButton rb)
    {
        if (!rbl_DeepLearning.Items[0].Selected)
            return;

        String techPanelID = rb.Attributes["techPanelID"];
        if (techPanelID == "274")
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("You have already signed up for the optional 'Deep Learning' tutorial on Sunday, October 25 from 1-4PM.");
            builder.Append(Environment.NewLine);
            builder.Append("Much of the content will be similar.");
            builder.Append(Environment.NewLine);
            builder.Append("You may want to select one or the other.");

            showUtilityModalPopup("deeplearning", "Deep Learning Tutorial", builder.ToString().Replace(Environment.NewLine, "<br />"), string.Empty, "OK", false);
        }
    }

    protected void rbTechPanel_CheckedChanged(object sender, EventArgs e)
    {
        RadioButton me = (RadioButton)sender;
        String attributeValue = me.Attributes["techPanelID"];

        if (me.Checked)
            verifyDeepLearning(me);

        int eventID = Convert.ToInt32(attributeValue);
        int parentEventID = 0;
        int requestOrder = 0;
        DataTable data = (DataTable)Session["techPanelRankOrder"];
        DataRow row = data.Rows.Find(Convert.ToInt32(attributeValue));
        string type = me.Attributes["type"];

        List<EventItem> events = new List<EventItem>();

        if (row != null)
        {
            parentEventID = Convert.ToInt32(row["parentEventID"]);
            if (me.Checked)
                requestOrder = Convert.ToInt32(type);

            row.BeginEdit();

            if (type == "1")
            {
                row["firstChoice"] = me.Checked;
                row["secondChoice"] = !me.Checked;
                row["thirdChoice"] = !me.Checked;

            }
            else if (type == "2")
            {
                row["firstChoice"] = !me.Checked;
                row["secondChoice"] = me.Checked;
                row["thirdChoice"] = !me.Checked;
            }
            else
            {
                row["firstChoice"] = !me.Checked;
                row["secondChoice"] = !me.Checked;
                row["thirdChoice"] = me.Checked;
            }
            row.EndEdit();

        }

        foreach (DataRow sibling in data.Rows)
        {
            if (Convert.ToInt32(sibling["techPanelID"]) != Convert.ToInt32(attributeValue))
            {
                sibling.BeginEdit();
                switch (type)
                {
                    case "1":
                        {
                            sibling["firstChoice"] = false;
                            break;
                        }
                    case "2":
                        {
                            sibling["secondChoice"] = false;
                            break;
                        }
                    case "3":
                        {
                            sibling["thirdChoice"] = false;
                            break;
                        }
                }
                sibling.EndEdit();
            }
        }

        events.Add(new EventItem(eventID, parentEventID, "Guest", requestOrder, false));

        RSVP rsvp = (RSVP)Session["rsvp"];
        rsvp.SetEventItems(events);

        data.AcceptChanges();
        Session["techPanelRankOrder"] = data;
        val_TechPanels_Validate();
        loadTechPanels();
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "_BindEvents_rbTechPanels" + me.ClientID,
             "BindEvents();", true);

    }


    protected void dl_MealDates_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Conference conference = (Conference)Application["Conference"];

            DataRowView drv = (DataRowView)e.Item.DataItem;
            DateTime date = (DateTime)drv["mealDate"];
            Label dateLabel = (Label)e.Item.FindControl("lbl_MealDate");
            dateLabel.Text = date.ToString("dddd, MMMM d, yyyy");

            DataList dl_MealsOnDate = (DataList)e.Item.FindControl("dl_MealsOnDate");

            dl_MealsOnDate.DataSource = WebDataUtility.Instance.webAppTable("sp_GetNumberMealOptions",
                new GenericCmdParameter[] { new GenericCmdParameter("@conferenceID", conference.ID),
                    new GenericCmdParameter("@dateOfMeal", date) });
            dl_MealsOnDate.ItemDataBound += new DataListItemEventHandler(dl_MealsOnDate_ItemDataBound);

            dl_MealsOnDate.DataBind();

        }
    }

    protected void dl_MealsOnDate_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            RSVP rsvp = (RSVP)Session["rsvp"];

            DataList me = (DataList)sender;

            DataRowView drv = (DataRowView)e.Item.DataItem;
            int mealID = Convert.ToInt32(drv["mealID"]);

            Label mealType = (Label)e.Item.FindControl("lbl_MealType");
            mealType.Text = Convert.ToString(drv["mealType"]) + ":";
            int numberOfOptions = (int)drv["numberOfOptions"];

            AjaxControlToolkit.ComboBox list = (AjaxControlToolkit.ComboBox)e.Item.FindControl("cbo_MultipleMealChoices");

            RadioButtonList rbl = (RadioButtonList)e.Item.FindControl("rbl_SingleMealChoice");
            Panel pnlSingle = (Panel)e.Item.FindControl("pnl_SingleMealChoice");
            Panel pnlMultiple = (Panel)e.Item.FindControl("pnl_MultipleMealChoices");

            list.Attributes.Add("mealID", Convert.ToString(mealID));
            rbl.Attributes.Add("mealID", Convert.ToString(mealID));

            rbl.SelectedIndexChanged += new EventHandler(rbl_SingleMealChoice_SelectedIndexChanged);

            if (numberOfOptions == 1)
            {
                pnlSingle.Visible = true;

                pnlMultiple.Visible = false;
                list.Visible = false;

                object retval = null;
                WebDataUtility.Instance.webAppScalar("sp_GetMealOptionsByMealID",
                     new GenericCmdParameter[] { new GenericCmdParameter("@mealID", mealID) }, ref retval);
                rbl.Attributes.Add("mealOptionID", Convert.ToString(retval));

                rbl.Visible = true;

                if (rsvp.Meals.Rows.Count > 0)
                {
                    DataRow[] selected = rsvp.Meals.Select("mealID = " + mealID);
                    rbl.Items[0].Selected = selected.Length > 0;
                    rbl.Items[1].Selected = selected.Length == 0;
                }
                //else
                //{
                //    List<MealItem> meal = new List<MealItem>();
                //    meal.Add(new MealItem(mealID, Convert.ToInt32(retval)));
                //    rsvp.SetMealItems(meal);
                //}
            }
            else
            {
                pnlMultiple.Visible = true;

                pnlSingle.Visible = false;
                rbl.Visible = false;

                list.Visible = true;

                list.DataSource = WebDataUtility.Instance.webAppTable("sp_GetMealOptionsByMealID",
                    new GenericCmdParameter[] { new GenericCmdParameter("@mealID", mealID) });
                list.DataValueField = "mealOptionID";
                list.DataTextField = "mealOptionName";
                list.DataBind();
                list.Items[0].Selected = true;

                DataRow[] selection = rsvp.Meals.Select("mealID = " + mealID);

                foreach (ListItem item in list.Items)
                {
                    foreach (DataRow row in ((DataTable)list.DataSource).Rows)
                    {
                        if (item.Value == Convert.ToString(row["mealOptionID"]))
                        {
                            item.Attributes.Add("onmouseover", "this.title='" + row["mealOptionDescription"].ToString() + "'");
                            item.Attributes.Add("mealID", Convert.ToString(mealID));
                            item.Attributes.Add("mealOptionID", Convert.ToString(row["mealOptionID"]));
                            break;
                        }
                    }

                }
                if (selection.Length > 0)
                    list.SelectedIndex = findListIndex(list.Items, Convert.ToString(selection[0]["mealOptionID"]));
                //else
                //{
                //    List<MealItem> meal = new List<MealItem>();
                //    meal.Add(new MealItem(mealID, Convert.ToInt32(list.SelectedValue)));
                //    rsvp.SetMealItems(meal);
                //}
            }
        }
    }

    protected void rbl_SundayReception_SelectedIndexChanged(object sender, EventArgs e)
    {
        RSVP rsvp = (RSVP)Session["rsvp"];
        rsvp.WelcomeReception = (rbl_SundayReception.SelectedValue == "True");

    }

    protected void rbl_SingleMealChoice_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList me = (RadioButtonList)sender;

        int mealID = Convert.ToInt32(me.Attributes["mealID"]);
        int mealOptionID = Convert.ToInt32(me.Attributes["mealOptionID"]);
        RSVP rsvp = (RSVP)Session["rsvp"];
        if (me.SelectedValue == "True")
        {
            List<MealItem> meal = new List<MealItem>();
            meal.Add(new MealItem(mealID, mealOptionID));
            rsvp.SetMealItems(meal);
        }
        else
        {
            rsvp.ClearMealChoice(mealID);
        }
    }

    protected void cbo_Division_SelectedIndexChanged(object sender, EventArgs e)
    {
        AjaxControlToolkit.ComboBox me = (AjaxControlToolkit.ComboBox)sender;

        if (me.SelectedValue == "Other") //other is selected - put out AJAX Modal popup window to have user enter division.
        {
            showUtilityModalPopup("division", "Add Your Division", "Your division is not listed? Please type it in.", cbo_Division.SelectedItem.Text, "Save", true);

        }
        else
        {
            RSVP rsvp = (RSVP)Session["rsvp"];
            ((KTConferenceUser)rsvp.User).Division = me.SelectedValue;
            me.Focus();
        }

    }

    private void showUtilityModalPopup(string purpose, string heading, string msg, string initialInputText, string OKButtonText, bool needTextBox)
    {
        hdn_UtilityModalPurpose.Value = purpose;
        lbl_UtilityModalHeader.Text = heading;
        lbl_UtilityModalMessage.Text = msg;

        if (OKButtonText == "Yes")
            bn_UtilityModaClose.Text = "No";

        tb_UtilityModalEntry.Visible = needTextBox;
        if (needTextBox)
        {
            tb_UtilityModalEntry.Text = initialInputText;
            tb_UtilityModalEntry.Focus();
        }
        bn_UtilityModalSave.Text = OKButtonText;

        bn_UtilityModaClose.CssClass = (purpose == "validation" ? "hidden" : bn_UtilityModaClose.CssClass);
        lbl_UtilityModalMessage.CssClass = (purpose == "validation" ? "validationMsg" : String.Empty);

        // up_UtilityModal.Update();
        mdl_UtilityModal.Show();

    }

    private string getDivisionID(string divisionText)
    {
        object objDivID = null;

        WebDataUtility.Instance.webAppScalar("sp_LoadNewDivision", new GenericCmdParameter[] { new GenericCmdParameter("divisionText", tb_UtilityModalEntry.Text) }, ref objDivID);

        string divText = Convert.ToString(objDivID);
        cbo_Division.DataSource = getDivisions();
        cbo_Division.DataBind();
        return divText;
    }

    private string getLocation(string location)
    {
        foreach (ListItem item in cbo_WorkLocation.Items)
        {
            if (item.Text == location)
                return location;
        }
        DataTable locations = getWorkLocations();

        DataRow newRow = locations.NewRow();
        newRow["countryName"] = location;
        locations.Rows.Add(newRow);

        cbo_WorkLocation.DataSource = locations;
        cbo_WorkLocation.DataValueField = "countryName";
        cbo_WorkLocation.DataTextField = "countryName";
        cbo_WorkLocation.DataBind();

        return location;

    }


    private string getEngineerType(string discipline)
    {
        foreach (ListItem item in cbo_EngineerType.Items)
        {
            if (item.Text == discipline)
                return discipline;
        }
        object retval = null;
        WebDataUtility.Instance.webAppScalar("sp_LoadNewEngineerType", new GenericCmdParameter[] { new GenericCmdParameter("@engineerType", discipline) }, ref retval);

        DataTable table = WebDataUtility.Instance.webAppTable("tbl_EngineerTypes");

        DataRow newRow = table.NewRow();
        newRow["engineerType"] = "Other";
        table.Rows.Add(newRow);

        cbo_EngineerType.DataSource = table;
        cbo_EngineerType.DataValueField = "engineerType";
        cbo_EngineerType.DataTextField = "engineerType";
        cbo_EngineerType.DataBind();

        return discipline;
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

    protected void bn_UtilityModalSave_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(2000);
        switch (hdn_UtilityModalPurpose.Value)
        {
            case ("division"):
                {
                    cbo_Division.Items.FindByValue(Convert.ToString(getDivisionID(tb_UtilityModalEntry.Text))).Selected = true;
                    RSVP rsvp = (RSVP)Session["rsvp"];
                    ((KTConferenceUser)rsvp.User).Division = tb_UtilityModalEntry.Text;

                    break;
                }
            case ("location"):
                {
                    cbo_WorkLocation.Items.FindByValue(getLocation(tb_UtilityModalEntry.Text)).Selected = true;
                    RSVP rsvp = (RSVP)Session["rsvp"];
                    ((KTConferenceUser)rsvp.User).HomeOffice = tb_UtilityModalEntry.Text;
                    break;
                }
            case ("discipline"):
                {
                    cbo_EngineerType.Items.FindByValue(getEngineerType(tb_UtilityModalEntry.Text)).Selected = true;
                    RSVP rsvp = (RSVP)Session["rsvp"];
                    ((KTConferenceUser)rsvp.User).JobRole = tb_UtilityModalEntry.Text;
                    break;
                }
            case ("nonvite"): //this is use case where non-invited user is registering for an invited user. ie. - exec admin registering for boss.
                {
                    tb_UtilityModalEntry_Validate();
                    if (!val_TextEntry.IsValid)
                        lbl_Error.Text = "*Sorry, that email address is not on the current invite list.<br> Please double-check the spelling or contact the employee's Head of Engineering.";
                    else
                    {
                        loadForm();
                        pnl_ExecAdmin.CssClass = "execAdmin";
                        //up_Body.Update();
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
                        loadForm();
                        pnl_ExecAdmin.CssClass = "execAdmin";
                        //up_Body.Update();
                    }
                    break;
                }
            case ("validation"):
                {
                    val_TextEntry.IsValid = true;
                    validatePage();
                    break;
                }
            case ("submit"):
                {
                    RSVP rsvp = (RSVP)Session["rsvp"];
                    rsvp.setPhotoWaiver();
                    Session["rsvp"] = rsvp;
                    writeRSVP();
                    //sendConfirmationEmail(true);
                    redirectToConfirmation();

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
        RSVP rsvp = (RSVP)Session["rsvp"];
        if (hdn_UtilityModalPurpose.Value == "division")
        {
            cbo_Division.SelectedIndex = findListIndex(cbo_Division.Items, "Other");
            ((KTConferenceUser)rsvp.User).Division = cbo_Division.SelectedValue;
        }

        if (hdn_UtilityModalPurpose.Value == "location")
        {
            cbo_WorkLocation.SelectedIndex = findListIndex(cbo_WorkLocation.Items, "Other");
            ((KTConferenceUser)rsvp.User).Division = cbo_WorkLocation.SelectedValue;

        }

        if (hdn_UtilityModalPurpose.Value == "discipline")
        {
            cbo_EngineerType.SelectedIndex = findListIndex(cbo_EngineerType.Items, "Other");
            ((KTConferenceUser)rsvp.User).Division = cbo_EngineerType.SelectedValue;
        }

        if (hdn_UtilityModalPurpose.Value == "nonvite")
        {
            //Session.Clear();
            Session["rsvpUser"] = null;
            Session["rsvp"] = null;
            System.Threading.Thread.Sleep(2000);
            Response.Redirect(ConfigurationManager.AppSettings["RedirectToHome"]);
        }
        if (hdn_UtilityModalPurpose.Value == "adminvite")
        {
            Session["rsvpUser"] = null;
            Session["rsvp"] = null;
            System.Threading.Thread.Sleep(2000);
            Response.Redirect(ConfigurationManager.AppSettings["RedirectToHome"]);
        }
        if (hdn_UtilityModalPurpose.Value == "deeplearning")
        {
        }
        if (hdn_UtilityModalPurpose.Value == "submit")
        {

        }

        hdn_UtilityModalPurpose.Value = string.Empty;
        lbl_UtilityModalMessage.Text = string.Empty;
        tb_UtilityModalEntry.Text = string.Empty;
        lbl_Error.Text = string.Empty;
        lbl_Error.Visible = false;
        mdl_UtilityModal.Hide();
    }

    protected void lb_AdminSubmit_Click(object sender, EventArgs e)
    {
        string msg = "Registering for someone else? If so, please enter their KLA-Tencor email address.";
        showUtilityModalPopup("adminvite", "Registering for Someone Else?", msg, String.Empty, "OK", true);
    }

    protected void cbo_MultipleMealChoices_SelectedIndexChanged(object sender, EventArgs e)
    {
        string index = ((AjaxControlToolkit.ComboBox)sender).SelectedItem.Text;

        AjaxControlToolkit.ComboBox me = (AjaxControlToolkit.ComboBox)sender;

        int mealID = Convert.ToInt32(me.Attributes["mealID"]);

        RSVP rsvp = (RSVP)Session["rsvp"];
        List<MealItem> meal = new List<MealItem>();
        meal.Add(new MealItem(mealID, Convert.ToInt32(me.SelectedValue)));
        rsvp.SetMealItems(meal);
        Session["rsvp"] = rsvp;
    }
    protected void rbl_PresenterInfo_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList self = (RadioButtonList)sender;
        int eventID = 0;

        if (!Int32.TryParse(self.Attributes["eventID"], out eventID))
            return;
        RSVP rsvp = (RSVP)Session["rsvp"];

        if (Convert.ToBoolean(self.SelectedValue))
        {
            ConferenceEvent presenterInfo = new ConferenceEvent(eventID);
            EventItem presenterInfoItem = new EventItem(presenterInfo.ID, presenterInfo.ParentID, "Guest", 1, false);
            List<EventItem> list = new List<EventItem>();
            list.Add(presenterInfoItem);

            rsvp.SetEventItems(list);
        }
        else
        {
            rsvp.ClearEvent(eventID);
        }
    }

    protected void rbl_DeepLearning_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList self = (RadioButtonList)sender;
        int eventID = 0;

        if (!Int32.TryParse(self.Attributes["eventID"], out eventID))
            return;

        if (self.Items[0].Selected)
            verifyDeepLearning(self);

        RSVP rsvp = (RSVP)Session["rsvp"];

        if (Convert.ToBoolean(self.SelectedValue))
        {
            ConferenceEvent deepLearning = new ConferenceEvent(eventID);
            EventItem deepLearningItem = new EventItem(deepLearning.ID, deepLearning.ParentID, "Guest", 1, false);
            List<EventItem> list = new List<EventItem>();
            list.Add(deepLearningItem);

            rsvp.SetEventItems(list);
        }
        else
        {
            rsvp.ClearEvent(eventID);
        }
    }

    protected void rb_ShirtType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbo_ShirtSize.SelectedIndex > 0)
        {
            RSVP rsvp = (RSVP)Session["rsvp"];
            string shirtSize = rb_ShirtType.SelectedValue + "-" + cbo_ShirtSize.SelectedValue;
            ((KTConferenceUser)rsvp.User).ShirtSize = shirtSize;
        }
    }


    protected void cbo_ShirtSize_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbo_ShirtSize.SelectedIndex > 0)
        {

            RSVP rsvp = (RSVP)Session["rsvp"];
            string shirtSize = rb_ShirtType.SelectedValue + "-" + cbo_ShirtSize.SelectedValue;
            ((KTConferenceUser)rsvp.User).ShirtSize = shirtSize;
        }
    }
    protected void cbl_HotelReservations_SelectedIndexChanged(object sender, EventArgs e)
    {
        RSVP rsvp = (RSVP)Session["rsvp"];

        DateTime checkIn = new DateTime(9999, 1, 1);
        DateTime checkOut = new DateTime(1, 1, 1);
        foreach (ListItem day in cbl_HotelReservations.Items)
        {
            DateTime t = Convert.ToDateTime(day.Value);
            if (day.Selected)
            {
                if (t < checkIn)
                    checkIn = t;
                if (t > checkOut)
                    checkOut = t;
            }
        }

        rsvp.CheckInDate = checkIn;
        rsvp.CheckOutDate = checkOut.AddDays(1);
    }
    protected void rbl_TransporationOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList me = (RadioButtonList)sender;
        if (me == null)
            return;

        String direction = me.Attributes["Direction"];
        RSVP rsvp = null;

        if (Session["rsvp"] == null)
        {
            rsvp = new RSVP(setUser(), "Guest");
        }
        else
            rsvp = (RSVP)Session["rsvp"];

        List<TransportationItem> list = new List<TransportationItem>();

        list.Add(new TransportationItem(Convert.ToInt32(me.SelectedValue), direction));
        rsvp.SetTransporationItem(list);
        Session["rsvp"] = rsvp;
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

    protected void cb_PaperChoiceCheckedChanged(object sender, EventArgs e)
    {
        CheckBox me = (CheckBox)sender;
        DataTable data = (DataTable)Session["paperRankOrder"];

        DataRow row = data.Rows.Find(Convert.ToDateTime(me.Attributes["paperTime"]));

        int eventID = Convert.ToInt32(me.Attributes["eventID"]);
        int parentEventID;
        List<EventItem> events = new List<EventItem>();
        RSVP rsvp = (RSVP)Session["rsvp"];

        if (row != null)
        {
            row.BeginEdit();

            string cbType = me.Attributes["type"];
            parentEventID = Convert.ToInt32(row["timeSlotID"]);

            if (cbType == "firstChoice")
            {
                if (me.Checked)
                {

                    row["firstChoice"] = eventID;

                    events.Add(new EventItem(eventID, parentEventID, "Guest", 1, false));

                    if (Convert.ToInt32(row["secondChoice"]) == eventID)
                        row["secondChoice"] = 0;
                }
                else
                {
                    if (Convert.ToInt32(row["firstChoice"]) == eventID)
                    {
                        row["firstChoice"] = 0;
                        rsvp.ClearEvent(eventID);
                    }

                }
            }
            else
            {
                if (me.Checked)
                {
                    row["secondChoice"] = eventID;
                    events.Add(new EventItem(eventID, parentEventID, "Guest", 2, false));

                    if (Convert.ToInt32(row["firstChoice"]) == eventID)
                        row["firstChoice"] = 0;
                }
                else
                {

                    if (Convert.ToInt32(row["secondChoice"]) == eventID)
                    {
                        row["secondChoice"] = 0;
                        rsvp.ClearEvent(eventID);
                    }
                }
            }

        }
        row.EndEdit();
        data.AcceptChanges();
        Session["paperRankOrder"] = data;
        val_Papers_Validate(true);
        loadSymposia();

        rsvp.SetEventItems(events);

        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "_BindEvents_cbPapers" + me.ClientID,
                     "BindEvents();", true);
        //DataRow[] siblingPapers = data.Select("timeSlot = '" + Convert.ToString(me.Attributes["timeSlot"]) + "' AND paperID <> " + Convert.ToInt32(me.Attributes["paperID"]));

        //if(row != null)
        //{

        //    row.BeginEdit();

        //    String cbType = me.Attributes["cbType"];


        //    row[cbType] = me.Checked;

        //    switch(cbType)
        //    {
        //        case "firstChoice":
        //            {
        //                if (me.Checked)
        //                {
        //                    row["secondChoice"] = false;

        //                    foreach(DataRow sibling in siblingPapers)
        //                    {
        //                        if(Convert.ToBoolean(sibling["firstChoice"]))
        //                        {
        //                            sibling.BeginEdit();
        //                            sibling["firstChoice"] = false;
        //                            sibling.EndEdit();
        //                        }
        //                    }

        //                }
        //                break;
        //            }
        //        case "secondChoice":
        //            {

        //                if(me.Checked)
        //                {
        //                    row["firstChoice"] = false;

        //                    foreach(DataRow sibling in siblingPapers)
        //                    {
        //                        if(Convert.ToBoolean(sibling["secondChoice"]))
        //                        {
        //                            sibling.BeginEdit();
        //                            sibling["secondChoice"] = false;
        //                            sibling.EndEdit();
        //                        }
        //                    }
        //                }
        //                break;
        //            }
        //    }

        //    row.EndEdit();
        //    data.AcceptChanges();
        //    loadSymposia();
        //}
    }

    private EventItem createEventItem(int eventID, int parentEventID, int requestOrder, string userRole, bool eventAssigned)
    {
        return new EventItem(eventID, parentEventID, userRole, requestOrder, eventAssigned);
    }


    protected void cbo_EngineerType_SelectedIndexChanged(object sender, EventArgs e)
    {
        AjaxControlToolkit.ComboBox me = (AjaxControlToolkit.ComboBox)sender;

        if (me.SelectedValue == "Other")
        {
            showUtilityModalPopup("discipline", "Add Your Job Role", "Your job role isn't listed? Please type it in.",
                   cbo_EngineerType.SelectedItem.Text, "Save", true);

        }
        else
        {
            RSVP rsvp = (RSVP)Session["rsvp"];
            ((KTConferenceUser)rsvp.User).JobRole = me.SelectedValue;
            me.Focus();
        }
    }

    protected void cbo_WorkLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        AjaxControlToolkit.ComboBox me = (AjaxControlToolkit.ComboBox)sender;

        if (me.SelectedValue == "Other")
        {
            showUtilityModalPopup("location", "Add Your Home Office Location", "Your home office isn't listed? Please type it in.",
                    cbo_WorkLocation.SelectedItem.Text, "Save", true);

        }
        else
        {
            RSVP rsvp = (RSVP)Session["rsvp"];
            ((KTConferenceUser)rsvp.User).HomeOffice = me.SelectedValue;
            me.Focus();
        }
    }

    protected void cbo_SymposiumSelection_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList me = (DropDownList)sender;
        string attrributeValue = me.Attributes["symposiumID"];
        Dictionary<string, DropDownList> cboGroup = (Dictionary<string, DropDownList>)Session[attrributeValue];

        if (me.SelectedValue != "0")
        {
            foreach (KeyValuePair<string, DropDownList> pair in cboGroup)
            {

                Control ctl = FindControlRecursiveByClientID(this, pair.Value.ClientID);

                if (me.ClientID != pair.Key)
                {
                    if (pair.Value.Items.FindByValue("2").Selected && me.SelectedValue == "2")
                    {
                        pair.Value.Items.FindByValue("2").Selected = false;
                        pair.Value.Items.FindByValue("1").Selected = false;
                        pair.Value.Items.FindByValue("0").Selected = true;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), pair.Value.ClientID + "_UpdateTo0",
                              "var elem = document.getElementById('" + pair.Value.ClientID + "'); elem.selectedIndex =0; elem.onchange();", true);
                    }
                    else if (pair.Value.Items.FindByValue("1").Selected && me.SelectedValue == "1")
                    {
                        pair.Value.Items.FindByValue("2").Selected = true;
                        pair.Value.Items.FindByValue("1").Selected = false;
                        pair.Value.Items.FindByValue("0").Selected = false;

                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), pair.Value.ClientID + "_UpdateTo1",
                            "var elem = document.getElementById('" + pair.Value.ClientID + "'); elem.selectedIndex =2; elem.onchange();", true);
                    }
                }
                else
                {
                    foreach (ListItem item in pair.Value.Items)
                    {
                        if (item.Value == me.SelectedValue)
                            item.Selected = true;
                        else
                            item.Selected = false;
                    }
                }
            }
        }
    }

    protected void rbl_SundayGolf_SelectedIndexChanged(object sender, EventArgs e)
    {

        RSVP rsvp = (RSVP)Session["rsvp"];
        rsvp.Golfing = (rbl_SundayGolf.SelectedValue == "1");
    }

    private void val_Papers_Validate(bool ignoreUpdgrade)
    {
        bool invalid = false;
        if (Session["paperRankOrder"] == null)
        {
            tb_PaperDummy.Text = "0";
            invalid = true;
        }
        else
        {


            DataTable paperRankOrderTable = (DataTable)Session["paperRankOrder"];
            foreach (DataRow row in paperRankOrderTable.Rows)
            {
                if (Convert.ToInt32(row["firstChoice"]) == 0 &&
                    Convert.ToInt32(row["secondChoice"]) == 0)
                {
                    invalid = true;
                }
                else if (Convert.ToInt32(row["firstChoice"]) == 0 &&
                    Convert.ToInt32(row["secondChoice"]) > 0 && !ignoreUpdgrade)
                {
                    row["firstChoice"] = row["secondChoice"];
                    row["secondChoice"] = 0;
                }
            }
            tb_PaperDummy.Text = (invalid ? "0" : "1");

        }

        val_Papers.IsValid = !invalid;
    }


    private void val_TechPanels_Validate()
    {
        if (Session["techPanelRankOrder"] == null)
        { tb_TechPanelDummy.Text = "0";
        }
        else
        {
            bool invalid = false;
            DataTable data = (DataTable)Session["techPanelRankOrder"];
            foreach (DataRow row in data.Rows)
            {
                invalid = (!Convert.ToBoolean(row["firstChoice"])) && (!Convert.ToBoolean(row["secondChoice"])) && (!Convert.ToBoolean(row["thirdChoice"]));
                if (!invalid)
                    break;
            }
            tb_TechPanelDummy.Text = (invalid ? "0" : "1");
            val_TechPanels.IsValid = !invalid;
        }
    }

    private bool validatePage()
    {
        if (tb_FirstName.Text.Trim() == string.Empty || tb_LastName.Text.Trim() == string.Empty)
            val_Name.IsValid = false;
        if (cbo_Division.SelectedItem.Text == "Please select:")
            val_Division.IsValid = false;
        if (cbo_ShirtSize.SelectedItem.Text == "Please select:")
            val_ShirtSize.IsValid = false;
        if (cbo_EngineerType.SelectedItem.Text == "Please select:")
            val_EngDiscipline.IsValid = false;
        val_TechPanels_Validate();
        val_Papers_Validate(false);

        return (val_Division.IsValid && val_EngDiscipline.IsValid && val_ShirtSize.IsValid && val_TechPanels.IsValid && val_Papers.IsValid);

    }

    //protected void bn_NextClicked(object sender, EventArgs e)
    //{

    //    mv_RSVPForm.ActiveViewIndex = ++mv_RSVPForm.ActiveViewIndex;


    //}
    //protected void bn_PreviousClicked(object sender, EventArgs e)
    //{
    //    mv_RSVPForm.ActiveViewIndex = --mv_RSVPForm.ActiveViewIndex;
    //}
    //private void updateWizardPageShown(Panel pnl)
    //{
    //    foreach(View page in mv_RSVPForm.Views)
    //    {
    //        if (page.ID.Contains(pnl.ID))
    //            page.Visible =  true;
    //        else
    //            page.Visible = false;
    //    }
    //}

    protected void bn_SubmitRSVP_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(2000);
        if (!validatePage())
        {
            StringBuilder builder = new StringBuilder("Errors! There's some important information missing.\nPlease check the following fields and enter the following information:" + Environment.NewLine);
            builder.AppendLine(Environment.NewLine);
            foreach (IValidator val in Page.Validators)
            {
                if (!val.IsValid)
                {
                    builder.AppendLine(val.ErrorMessage + Environment.NewLine);
                }
            }
            showUtilityModalPopup("validation", "Missing Information", builder.ToString().Replace(Environment.NewLine, "<br />"), string.Empty, "OK", false);
        }
        else
        {
            RSVP rsvp = (RSVP)Session["rsvp"];

            if (rsvp.IsNew || !rsvp.PhotoWaiver)
            {
                string msg = "By submitting your registration, you agree to the terms of the <a href='Files/KT_photo_release_form.pdf' target='_blank'>KLA-Tencor Photograph and Video Waiver</a>.";
                showUtilityModalPopup("submit", "Confirm Registration Submission", msg, string.Empty, "OK", false);
                //sendConfirmationEmail(true);
            }
            else
            {
                bool sendChangeEmail = (!rsvp.IsNew); //if is a new registration or a re-register of cancelled rsvp - don't send update email.

                writeRSVP();
                
                //sendConfirmationEmail(false);

                //if (sendChangeEmail)
                //    rsvp.SendChangeEmail();
                
                redirectToConfirmation();
            }
        }



    }

    private bool writeRSVP()
    {
        //update database
        //if successful update, give user success message
        //send confirmation email and redirect to finalized page
        RSVP rsvp = (RSVP)Session["rsvp"];

        rsvp.User.FirstName = tb_FirstName.Text;
        rsvp.User.LastName = tb_LastName.Text;
        rsvp.User.MobilePhone = tb_MobilePhone.Text;
        rsvp.User.FoodAllergies = tb_FoodRestrictions.Text;
        rsvp.User.SpecialNeeds = tb_SpecialNeeds.Text;

        rsvp.Golfing = (rbl_SundayGolf.SelectedValue == "1");

        string shirtSize = rb_ShirtType.SelectedValue + "-" + cbo_ShirtSize.SelectedValue;
        ((KTConferenceUser)rsvp.User).ShirtSize = shirtSize;
        
        ((KTConferenceUser)rsvp.User).HomeOffice = cbo_WorkLocation.SelectedValue;

        rsvp.WelcomeReception = Convert.ToBoolean(rbl_SundayReception.SelectedValue);

        DateTime checkIn = new DateTime(2100, 1, 1);
        DateTime checkOut = new DateTime(1900, 1, 1);

        
        foreach (ListItem day in cbl_HotelReservations.Items)
        {
            DateTime t = Convert.ToDateTime(day.Value);
            if (day.Selected)
            {
                if (t < checkIn)
                    checkIn = t;
                if (t > checkOut)
                    checkOut = t;
            }
        }
        //if year = 2100, no dates selected.
        if (checkIn.Year == 2100)
        {
            rsvp.CheckInDate = new DateTime(1900, 1, 1);
            rsvp.CheckOutDate = new DateTime(1900, 1, 1);
        }
        else
        {
            rsvp.CheckInDate = checkIn;
            rsvp.CheckOutDate = checkOut.AddDays(1);
        }

        if (rsvp.Admin != null)
        {
            rsvp.Admin.FirstName = (tb_ExecAdminFirstName.Text.Length > 0 ? tb_ExecAdminFirstName.Text : rsvp.Admin.FirstName);
            rsvp.Admin.LastName = (tb_ExecAdminLastName.Text.Length > 0 ? tb_ExecAdminLastName.Text : rsvp.Admin.LastName);
            rsvp.Admin.WorkPhone = (tb_ExecAdminPhone.Text.Length > 0 ? tb_ExecAdminPhone.Text : rsvp.Admin.WorkPhone);

        }

        return rsvp.Update();
    }

    private void sendConfirmationEmail(bool isNewRegistration)
    {
        if (Session["rsvp"] != null)
        {
            RSVP rsvp = (RSVP)Session["rsvp"];
            AlternateView view = null;
            if (rsvp.IsValid)
            {
                string html = string.Empty;
                //string url = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath + "/EmailBody.aspx?user=" + rsvp.User.Email;
                string url = ConfigurationManager.AppSettings["EmailTemplatePath"] + "EmailBody.aspx?user=" + rsvp.User.Email;
                url = new Uri(Page.Request.Url, ResolveClientUrl(url)).AbsoluteUri;
                try
                {
                    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                    req.Credentials = CredentialCache.DefaultCredentials;
                    HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                    StreamReader sr = new StreamReader(res.GetResponseStream());
                    html = sr.ReadToEnd();

                    List<LinkedResource> imgList = new List<LinkedResource>();

                    Bitmap header = new Bitmap(Resources.EmailResources.imgheader);
                    ImageConverter icHeader = new ImageConverter();
                    Byte[] headerByte = (Byte[])icHeader.ConvertTo(header, typeof(Byte[]));
                    MemoryStream msHeader = new MemoryStream(headerByte);
                    LinkedResource lrHeader = new LinkedResource(msHeader, "image/jpeg");
                    lrHeader.ContentId = "imgheader";
                    html = html.Replace("{imgheader}", "<img src=cid:imgheader />");

                    Bitmap footer = new Bitmap(Resources.EmailResources.imgfooter);
                    ImageConverter icfooter = new ImageConverter();
                    Byte[] footerByte = (Byte[])icfooter.ConvertTo(footer, typeof(Byte[]));
                    MemoryStream msfooter = new MemoryStream(footerByte);
                    LinkedResource lrfooter = new LinkedResource(msfooter, "image/jpeg");
                    lrfooter.ContentId = "imgfooter";
                    html = html.Replace("{imgfooter}", "<img src=cid:imgfooter />");
                    view = AlternateView.CreateAlternateViewFromString(html, null, "text/html");
                    
                    Bitmap security = new Bitmap(Resources.EmailResources.imgsecuritymarking);
                    ImageConverter icsecurity = new ImageConverter();
                    Byte[] securityByte = (Byte[])icsecurity.ConvertTo(security, typeof(Byte[]));
                    MemoryStream mssecurity = new MemoryStream(securityByte);
                    LinkedResource lrsecurity = new LinkedResource(mssecurity, "image/jpeg");
                    lrsecurity.ContentId = "imgsecurity";
                    html = html.Replace("{imgsecuritymarking}", "<img src=cid:imgsecurity />");

                    view = AlternateView.CreateAlternateViewFromString(html, null, "text/html");

                    view.LinkedResources.Add(lrHeader);
                    view.LinkedResources.Add(lrfooter);
                    view.LinkedResources.Add(lrsecurity);
                }
                catch (Exception ex)
                {
                    ExceptionUtility.LogException(ex, "ConferenceObjects.ConferenceLibrary.Conference.SendConfirmationEmail(" + rsvp.User.Email + ")");
                }
                rsvp.SendEmailConfirmation(view, html, isNewRegistration);
            }
        }
    }


    protected void bn_Cancel_Click(object sender, EventArgs e)
    {
        Session.Clear();
        redirectToDefault();
    }

    private void redirectToDefault()
    {
        Session["rsvp"] = null;
        Session["rsvpUser"] = null;

        HttpContext.Current.Response.Redirect("Default.aspx");
    }

    private void redirectToConfirmation()
    {
        KTConferenceUser rsvpUser = null;
        KTConferenceUser user = null;
        if (Session["rsvpUser"] != null)
        {
            rsvpUser = (KTConferenceUser)Session["rsvpUser"];
        }
        if (Session["user"] != null)
        {
            user = (KTConferenceUser)Session["user"];
        }

        Session["rsvp"] = null;
        Session["rsvpUser"] = null;

        if (user != null && rsvpUser != null)
        {
            if(!user.Email.Equals(rsvpUser.Email))
                HttpContext.Current.Response.Redirect("Confirmation.aspx?" + security.Encrypt(
                                       string.Format("rsvpUser={0}", rsvpUser.Email), user.Email.Substring(0, 8)));
            else
                HttpContext.Current.Response.Redirect("Confirmation.aspx");
        }

        
        HttpContext.Current.Response.Redirect("Confirmation.aspx");
    }


    private void loadWorkLocations(RSVP rsvp)
    {
        KTConferenceUser user = (KTConferenceUser)rsvp.User;

        string location = string.Empty;

        //cbo_WorkLocation
        if (!user.HomeOffice.Equals(string.Empty))
            location = user.HomeOffice;
        else if (user.City != "Milpitas")
        {
            location = user.Country;
        }
        else
            location = "US - " + user.City;

        cbo_WorkLocation.DataSource = getWorkLocations();
        cbo_WorkLocation.DataValueField = "countryName";
        cbo_WorkLocation.DataTextField = "countryName";
        cbo_WorkLocation.DataBind();
        cbo_WorkLocation.Items.Insert(0, new ListItem("Please select:", "0"));
        if (location.Length > 0)
            cbo_WorkLocation.SelectedIndex = findListIndex(cbo_WorkLocation.Items, location);
        else
            cbo_WorkLocation.SelectedIndex = findListIndex(cbo_WorkLocation.Items, cbo_WorkLocation.Items[0].Value);
    }

    private void loadEngineerTypes(RSVP rsvp)
    {
        //cbo_EngineerType
        DataTable table = WebDataUtility.Instance.webAppTable("tbl_EngineerTypes");
        KTConferenceUser user = (KTConferenceUser)rsvp.User;

        string jobRole = string.Empty;


        DataRow newRow = table.NewRow();
        newRow["engineerType"] = "Other";
        table.Rows.Add(newRow);

        cbo_EngineerType.DataSource = table;
        cbo_EngineerType.DataValueField = "engineerType";
        cbo_EngineerType.DataTextField = "engineerType";
        cbo_EngineerType.DataBind();
        cbo_EngineerType.Items.Insert(0,new ListItem("Please select:", "0"));

        if (!user.JobRole.Equals(string.Empty))
        {
            jobRole = user.JobRole;
            ListItem selected = cbo_EngineerType.Items.FindByValue(jobRole);
            if (selected != null)
                selected.Selected = true;
            else
                cbo_EngineerType.SelectedIndex = findListIndex(cbo_EngineerType.Items, cbo_EngineerType.Items[0].Value);
        }
        else
            cbo_EngineerType.SelectedIndex = findListIndex(cbo_EngineerType.Items, cbo_EngineerType.Items[0].Value);
    }

    private void loadDivisions(RSVP rsvp)
    {
        //ddlDivision 
        KTConferenceUser user = (KTConferenceUser)rsvp.User;

        string division = user.Division;

        cbo_Division.DataSource = getDivisions();
        cbo_Division.DataValueField = "divisionText";
        cbo_Division.DataTextField = "divisionText";
        cbo_Division.DataBind();

        cbo_Division.Items.Insert(0, new ListItem("Please select:", "0"));

        if (!division.Equals(string.Empty))
            cbo_Division.SelectedIndex = findListIndex(cbo_Division.Items, Convert.ToString(division));
        else
            cbo_Division.SelectedIndex = findListIndex(cbo_Division.Items, cbo_Division.Items[0].Value);
    }

    private DataTable getDivisions()
    {
        DataTable tbl_Divisions = WebDataUtility.Instance.webAppTable("sp_GetDivsions", null);

        //DataRow rowOther = tbl_Divisions.NewRow();
        //rowOther["divisionText"] = "Other";

        //tbl_Divisions.Rows.Add(rowOther);

        return tbl_Divisions;
    }

    private DataTable getWorkLocations()
    {
        DataTable table = WebDataUtility.Instance.webAppTable("sp_GetCountryList", null);
        DataRow newRow = table.NewRow();
        newRow["countryName"] = "US - Milpitas";
        table.Rows.Add(newRow);
        
        newRow = table.NewRow();
        newRow["countryName"] = "Other";
        table.Rows.Add(newRow);

        return table;
    }

    private void loadForm()
    {
        KTConferenceUser rsvpUser = (KTConferenceUser)Session["rsvpUser"];
        KTConferenceUser user = (KTConferenceUser)Session["user"];

        RSVP rsvp = null;

        if (Session["rsvp"] == null)
            rsvp = new RSVP(rsvpUser, "Guest");
        else
            rsvp = (RSVP)Session["rsvp"];

        rsvp.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(rsvp_PropertyChanged);


        if (!rsvpUser.Email.Equals(user.Email))
        {
            rsvp.Admin = user;
        }

        Session["rsvp"] = rsvp;

        lblName.Text = ((KTConferenceUser)Session["rsvpUser"]).FullName;

        loadDivisions(rsvp);
        loadEngineerTypes(rsvp);
        loadWorkLocations(rsvp);

        //cbl_HotelReservation
        Conference confMetaData = (Conference)Application["Conference"];
        Dictionary<string, Type> parameters = new Dictionary<string, Type>();
        parameters["dateValue"] = typeof(DateTime);
        parameters["dateText"] = typeof(String);
        cbl_HotelReservations.DataSource = Common.GetDateTableFromDateRange(
           Convert.ToDateTime(confMetaData.CheckInStart),
           Convert.ToDateTime(confMetaData.Stop),
           parameters, "dddd, MMMM d, yyyy", true);
        cbl_HotelReservations.DataValueField = "dateValue";
        cbl_HotelReservations.DataTextField = "dateText";
        cbl_HotelReservations.DataBind();
        foreach (ListItem item in cbl_HotelReservations.Items)
        {
            item.Selected = true;
        }
        cbl_HotelReservations.SelectedIndexChanged += new EventHandler(cbl_HotelReservations_SelectedIndexChanged);
        //dl_MealDates
        dl_MealDates.DataSource = WebDataUtility.Instance.webAppTable("sp_MealCountByDate", 
                        new GenericCmdParameter[] {new GenericCmdParameter("@conferenceID", Conference.Instance.ID)});
        dl_MealDates.ItemDataBound += new DataListItemEventHandler(dl_MealDates_ItemDataBound);
        dl_MealDates.DataBind();

        //if (!rsvp.IsNew)
        {
            //bn_SubmitRSVP.Text = (rsvp.IsNew?"Register":"Update RSVP");
            tb_FirstName.Text = rsvpUser.FirstName;
            tb_LastName.Text = rsvpUser.LastName;
            tb_MobilePhone.Text = rsvpUser.MobilePhone;
            if (rsvpUser.ShirtSize.Length > 0)
            {
                rb_ShirtType.SelectedIndex = (rsvpUser.ShirtSize.Substring(0, 1) == "M" ? 0 : 1);

                string size = rsvpUser.ShirtSize.Substring(2, rsvpUser.ShirtSize.Length - 2);
                ListItem shirtSize = cbo_ShirtSize.Items.FindByValue(size);
                cbo_ShirtSize.SelectedIndex = findListIndex(cbo_ShirtSize.Items, size);
            }

            rbl_SundayReception.SelectedIndex = (rsvp.WelcomeReception ? 0 : 1);
            rbl_SundayGolf.SelectedIndex = (rsvp.Golfing ? 1 : 0);

            tb_FoodRestrictions.Text = rsvpUser.FoodAllergies;
            tb_SpecialNeeds.Text = rsvpUser.SpecialNeeds;

            if (rsvp.Admin != null)
            {
                tb_ExecAdminFirstName.Text = rsvp.Admin.FirstName;
                tb_ExecAdminLastName.Text = rsvp.Admin.LastName;
                tb_ExecAdminEmail.Text = rsvp.Admin.Email;
                tb_ExecAdminPhone.Text = rsvp.Admin.WorkPhone;
            }

            foreach (ListItem item in cbl_HotelReservations.Items)
            {
                if (rsvp.CheckInDate == null || rsvp.CheckOutDate == null)
                    item.Selected = false;
                DateTime date = Convert.ToDateTime(item.Value);
                if (date.DayOfYear >= ((DateTime)rsvp.CheckInDate).DayOfYear && date.DayOfYear < ((DateTime)rsvp.CheckOutDate).DayOfYear)
                    item.Selected = true;
                else
                    item.Selected = false;
            }
        }
        //else
        //    bn_SubmitRSVP.Text = "Register";

        //optional sunday deep learning....
        rbl_DeepLearning.SelectedIndex = rsvp.ContainsEvent(313) ? 0 : 1;

        if (!rsvp.User.isSpeaker)
        {
            lbl_PresenterInfo.Style["display"] = "none";
            lbl_PresenterInfoDescription.Style["display"] = "none";
            rbl_PresenterInfo.Style["display"] = "none";
        }
        else
        {
            rbl_PresenterInfo.SelectedIndex = rsvp.ContainsEvent(310) ? 1 : 0;
        }


        loadSymposia();
        loadTechPanels();
        loadTransportation(rsvp);

    }

    private void rsvp_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        Session["rsvp"] = (RSVP)sender;
    }

    private void loadSymposia()
    {
        if (Session["symposiaMasterList"] == null)
        {
            //sp_GetSimultaneousEventsByEventType
            Conference conference = (Conference)Application["Conference"];

            GenericCmdParameter[] parameters = {new GenericCmdParameter("@conferenceID", conference.ID),
            new GenericCmdParameter("@eventType", "Paper Symposium")};
            DataTable table = WebDataUtility.Instance.webAppTable("sp_GetSimultaneousEventsByEventType", parameters);

            table.Columns.Add(new DataColumn("roman", typeof(String)));

            DateTime temp = Convert.ToDateTime("1/1/1900");
            int i = 1;
            foreach (DataRow row in table.Rows)
                row["roman"] = Common.ToRoman(i++);

            Session["symposiaMasterList"] = table;

            dl_PaperSymposiaList.DataSource = table;
        }
        else
        {
            dl_PaperSymposiaList.DataSource = (DataTable)Session["symposiaMasterList"];
        }
        dl_PaperSymposiaList.DataBind();
    }

    private void loadTechPanels()
    {
        if (Session["techPanelMasterList"] == null)
        {
            Conference conference = (Conference)Application["Conference"];

            GenericCmdParameter[] parameters = {new GenericCmdParameter("@conferenceID", conference.ID),
            new GenericCmdParameter("@eventType", "Technical Panels")};
            DataTable table = WebDataUtility.Instance.webAppTable("sp_GetSimultaneousEventsByEventType", parameters);
            table.Columns.Add(new DataColumn("roman", typeof(String)));

            DateTime temp = Convert.ToDateTime("1/1/1900");
            int i = 1;
            foreach (DataRow row in table.Rows)
                row["roman"] = Common.ToRoman(i++);

            Session["techPanelMasterList"] = table;



            dl_TechPanelsList.DataSource = table;

        }
        else
        {
            DataTable table = (DataTable)Session["techPanelMasterList"];
            dl_TechPanelsList.DataSource = (DataTable)Session["techPanelMasterList"];
        }
        
        
        dl_TechPanelsList.DataBind();
    }

    private void loadTransportation(RSVP rsvp)
    {
        List<TransportationItem> transItem = new List<TransportationItem>();
        Conference conference = (Conference)Application["Conference"];

        string direction = "Inbound";

        GenericCmdParameter[] parameters = {new GenericCmdParameter("@conferenceID", conference.ID),
                new GenericCmdParameter("@direction", direction)};

        if(!IsPostBack)
        {
            DataTable tbl_Inbound = WebDataUtility.Instance.webAppTable("sp_GetConferenceTransportationOptions", parameters);
            tbl_Inbound.Columns.Add(new DataColumn("dataTextField", typeof(String)));
            lbl_DepartLocationInbound.Text = String.Empty;

            foreach (DataRow row in tbl_Inbound.Rows)
            {
                row["dataTextField"] = (Convert.ToString(row["transportationModeText"]) == "Self" ? "Self" : 
                          Convert.ToString(row["transportationModeText"]) + "* - " 
                          + Convert.ToDateTime(row["transportationDepartTime"]).ToString("MMMM d, yyyy, h:mm tt"));
                lbl_DepartLocationInbound.Text = Convert.ToString(row["transportationModeText"]) == "Self" && lbl_DepartLocationInbound.Text != String.Empty ?
                   lbl_DepartLocationInbound.Text : "*" + Convert.ToString(row["transportationDepartLocation"]);
            }
            rbl_MonteryInbound.DataSource = tbl_Inbound;
            rbl_MonteryInbound.DataValueField = "conferenceTransportationOptionID";
            rbl_MonteryInbound.DataTextField = "dataTextField";

            rbl_MonteryInbound.DataBind();
            rbl_MonteryInbound.AutoPostBack = true;
            rbl_MonteryInbound.SelectedIndexChanged += new EventHandler(rbl_TransporationOptions_SelectedIndexChanged);
        }
        //if (!rsvp.IsNew)
        {
            DataRow[] transOptions = rsvp.Transportation.Select("transportationDirection = 'Inbound'");
            if (transOptions.Length > 0)
            {
                foreach (ListItem item in rbl_MonteryInbound.Items)
                {
                    if (item.Value == Convert.ToString(transOptions[0]["userTransportationOptionID"]))
                        item.Selected = true;
                }
            }
            else
            {
                rbl_MonteryInbound.Items[0].Selected = true;
                transItem.Add(new TransportationItem(Convert.ToInt32(rbl_MonteryInbound.SelectedValue), direction));

            }
        }
        //else
        //    rbl_MonteryInbound.Items[0].Selected = true;
        if(!IsPostBack)
        {
            direction = "Outbound";
            parameters[1].ParamValue = direction;
            DataTable tbl_Outbound = WebDataUtility.Instance.webAppTable("sp_GetConferenceTransportationOptions", parameters);
            tbl_Outbound.Columns.Add(new DataColumn("dataTextField", typeof(String)));

            lbl_DepartLocationOutbound.Text = String.Empty;

            foreach (DataRow row in tbl_Outbound.Rows)
            {
                row["dataTextField"] = (Convert.ToString(row["transportationModeText"]) == "Self" ? "Self" : 
                          Convert.ToString(row["transportationModeText"]) + "* - " 
                          + Convert.ToDateTime(row["transportationDepartTime"]).ToString("MMMM d, yyyy, h:mm tt"));

                lbl_DepartLocationOutbound.Text = Convert.ToString(row["transportationModeText"]) == "Self" && lbl_DepartLocationOutbound.Text != String.Empty ?
                    lbl_DepartLocationOutbound.Text : "*" + Convert.ToString(row["transportationDepartLocation"]);
            }

            rbl_MonteryOutbound.DataSource = tbl_Outbound;
            rbl_MonteryOutbound.DataValueField = "conferenceTransportationOptionID";
            rbl_MonteryOutbound.DataTextField = "dataTextField";
            rbl_MonteryOutbound.AutoPostBack = true;
            rbl_MonteryOutbound.SelectedIndexChanged += new EventHandler(rbl_TransporationOptions_SelectedIndexChanged);
            rbl_MonteryOutbound.DataBind();
        }
        //if (!rsvp.IsNew)
        {
            DataRow[] transOptions = rsvp.Transportation.Select("transportationDirection = 'Outbound'");
            if (transOptions.Length > 0)
            {
                foreach (ListItem item in rbl_MonteryOutbound.Items)
                {
                    if (item.Value == Convert.ToString(transOptions[0]["userTransportationOptionID"]))
                        item.Selected = true;
                }
            }
            else
            {
                rbl_MonteryOutbound.Items[0].Selected = true;
                transItem.Add(new TransportationItem(Convert.ToInt32(rbl_MonteryOutbound.SelectedValue), direction));
            }
        }
        //else
        //{
        //    rbl_MonteryOutbound.Items[0].Selected = true;
        //    transItem.Add(new TransportationItem(Convert.ToInt32(rbl_MonteryOutbound.SelectedValue), direction));
        //    rsvp.SetTransporationItem(transItem);
        //}
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


    private DataTable createTechPanelRankOrderTable()
    {
        DataTable table = new DataTable("techPanelRankOrder");

        DataColumn techPanelID = new DataColumn("techPanelID", typeof(int));
        DataColumn parentEventID = new DataColumn("parentEventID", typeof(int));
        DataColumn firstChoice = new DataColumn("firstChoice", typeof(bool));
        DataColumn secondChoice = new DataColumn("secondChoice", typeof(bool));
        DataColumn thirdChoice = new DataColumn("thirdChoice", typeof(bool));
        //DataColumn locked = new DataColumn("locked", typeof(bool));

        DataColumn[] keys = new DataColumn[1];
        table.Columns.Add(techPanelID);
        table.Columns.Add(parentEventID);
        table.Columns.Add(firstChoice);
        table.Columns.Add(secondChoice);
        table.Columns.Add(thirdChoice);
       // table.Columns.Add(locked);
        keys[0] = techPanelID;
        table.PrimaryKey = keys;
        Session["techPanelRankOrder"] = table;

        return table;
    }


    private DataTable createPaperRankOrderTable()
    {
        DataTable table = new DataTable("paperRankOrder");

        DataColumn timeSlot = new DataColumn("timeSlot", typeof(DateTime));
        DataColumn timeSlotID = new DataColumn("timeSlotID", typeof(int));
        DataColumn firstChoice = new DataColumn("firstChoice", typeof(int));
        DataColumn secondChoice = new DataColumn("secondChoice", typeof(int));
        DataColumn locked = new DataColumn("locked", typeof(bool));
        DataColumn[] keys = new DataColumn[1];
        table.Columns.Add(timeSlot);
        table.Columns.Add(timeSlotID);
        table.Columns.Add(firstChoice);
        table.Columns.Add(secondChoice);
        table.Columns.Add(locked);

        keys[0] = timeSlot;
        table.PrimaryKey = keys;
        
        return table;
    }

    private Control FindControlRecursiveByClientID(Control rootControl, string controlClientID)
    {
        if (rootControl.ClientID == controlClientID) return rootControl;

        foreach (Control controlToSearch in rootControl.Controls)
        {
            Control controlToReturn =
                FindControlRecursiveByClientID(controlToSearch, controlClientID);
            if (controlToReturn != null) return controlToReturn;
        }
        return null;
    }

    private Control FindControlRecursiveByID(Control rootControl, string controlID)
    {
        if (rootControl.ID == controlID) return rootControl;

        foreach (Control controlToSearch in rootControl.Controls)
        {
            Control controlToReturn =
                FindControlRecursiveByID(controlToSearch, controlID);
            if (controlToReturn != null) return controlToReturn;
        }
        return null;
    }

}
