using DataUtilities;
using DataUtilities.SQLServer;
using DataUtilities.KTActiveDirectory;
using HelperFunctions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;


public partial class Balloon : System.Web.UI.Page
{

    private Dictionary<string, string> images = new Dictionary<string, string>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsCallback)
        {
            //getListData();
            //loadDictionary();

           initPanels(Request.QueryString["type"]);


        }
    }

    private void initPanels(string type)
    {

       if(pnl_TechPanel.Attributes["type"] == type)
       {
           int eventID;
           if(Int32.TryParse(Request.QueryString["id"], out eventID))
           {
               pnl_TechPanel.Visible = true;

               ds_Tech_PanelDetails.DataSource =WebDataUtility.Instance.webAppTable("sp_GetTechPanelDetails", 
                   new GenericCmdParameter[] {new GenericCmdParameter("@eventID", eventID)});
               ds_Tech_PanelDetails.DataBind();
           }
           pnl_Paper.Visible = false;
           pnl_ExecSpeaker.Visible = false;
       }
       else if (pnl_Paper.Attributes["type"] == type)
       {
           int paperID;
           if (Int32.TryParse(Request.QueryString["id"], out paperID))
           {
               pnl_Paper.Visible = true;
               ds_PaperDetails.DataSource =WebDataUtility.Instance.webAppTable("sp_GetPaperDetails",
                   new GenericCmdParameter[] { new GenericCmdParameter("@paperID", paperID) });
               ds_PaperDetails.DataBind();
           }
           pnl_TechPanel.Visible = false;
           pnl_ExecSpeaker.Visible = false;

       }
       else if (pnl_ExecSpeaker.Attributes["type"] == type)
       {
           int speakerID;
           if (Int32.TryParse(Request.QueryString["id"], out speakerID))
           {
               pnl_ExecSpeaker.Visible = true;
               dl_ExecSpeaker.DataSource = WebDataUtility.Instance.webAppTable("sp_GetSpeakerInfo",
                    new GenericCmdParameter[] { new GenericCmdParameter("@speakerID", speakerID) });
               dl_ExecSpeaker.DataBind();

           }
       }
       else
           pnl_TechPanel.Visible = false;

    }


    protected void loadDictionary()
    {
        images["doc"] = "~/Images/doc.png";
        images["docx"] = "~/Images/doc.png";
        images["pdf"] = "~/Images/pdf.png";
        images["ppt"] = "~/Images/ppt.png";
        images["pptx"] = "~/Images/ppt.png";
        images["xls"] = "~/Images/xls.png";
        images["xlsx"] = "~/Images/xls.png";
        images["txt"] = "~/Images/txt.png";
        images["png"] = "~/Images/img.png";
        images["gif"] = "~/Images/img.png";
        images["jpg"] = "~/Images/img.png";
        images["jpeg"] = "~/Images/img.png";
        images["web"] = "~/Images/web.png";

    }

    protected void ds_TechPanelDetails_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item ||
            e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            Label lbl_TechPanelTitle = (Label)e.Item.FindControl("lbl_TechPanelTitle");
            lbl_TechPanelTitle.Text = Common.ProperCase(Convert.ToString(drv["techpanelTitle"]));

            Label lbl_TechPanelTopic = (Label)e.Item.FindControl("lbl_TechPanelTopic");
            lbl_TechPanelTopic.Text = "\"" + Convert.ToString(drv["techPanelTopic"]) + "\"";
            
            Label lbl_TechPanelModerator = (Label)e.Item.FindControl("lbl_TechPanelModerator");
            string techPanelModerator = Convert.ToString(drv["moderatorFirstName"]) + " "
                + Convert.ToString(drv["moderatorLastName"]) + ", " + Convert.ToString(drv["moderatorDivision"]);
            lbl_TechPanelModerator.Text = techPanelModerator;

            Label lbl_TechPanelDescription = (Label)e.Item.FindControl("lbl_TechPanelDescription");
            lbl_TechPanelDescription.Text = Convert.ToString(drv["techpanelDescription"]);

            DataList ds_TechPanelMembers = (DataList)e.Item.FindControl("ds_TechPanelMembers");
            ds_TechPanelMembers.DataSource =WebDataUtility.Instance.webAppTable("sp_GetTechPanelMembers",
                new GenericCmdParameter[] { new GenericCmdParameter("@techPanelID", Convert.ToInt32(drv["eventID"])) });
            ds_TechPanelMembers.DataBind();

        }
    }

    protected void ds_TechPanelMembers_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            Label lbl_TechPanelModerator = (Label)e.Item.FindControl("lbl_TechPanelModerator");

            string techPanelMember = Convert.ToString(drv["techpanelmemberFirstName"]) + " "
                + Convert.ToString(drv["techpanelmemberLastName"]) + ", " + Convert.ToString(drv["techpanelmemberDivision"]);

            lbl_TechPanelModerator.Text = techPanelMember;
        }
    }
    protected void dl_ExecSpeaker_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item ||
                e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Image img_Speaker = (Image)e.Item.FindControl("img_Speaker");
            Label lbl_Speaker = (Label)e.Item.FindControl("lbl_Speaker");
            Label lbl_Title = (Label)e.Item.FindControl("lbl_Title");
            Label lbl_Bio = (Label)e.Item.FindControl("lbl_Bio");

            img_Speaker.ImageUrl = String.Format("~/Images/speakers/{0}{1}.png",
                DBNullable.ToString(drv["userFirstName"]).Substring(0, 1).ToLower(),
                DBNullable.ToString(drv["userLastName"]).ToLower());

            lbl_Speaker.Text = String.Format("{0} {1}",
                DBNullable.ToString(drv["userFirstName"]),
                DBNullable.ToString(drv["userLastName"]));

            lbl_Title.Text = DBNullable.ToString(drv["userTitle"]);
            lbl_Bio.Text = DBNullable.ToString(drv["userHTMLBio"]);

        }
    }

    protected void ds_PaperDetails_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item ||
            e.Item.ItemType == ListItemType.AlternatingItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;
            string summary = drv["paperDescription"].ToString();
            summary = summary.Replace("?", "").Trim();

            if (!drv["paperDescription"].ToString().ToLower().Contains("description to follow"))
            {
                XmlDocument document = new XmlDocument();
                document.Load(Server.MapPath("~/Files/PaperData.xml"));

                XmlNode root = document.DocumentElement;

                XmlNode description = root.SelectSingleNode("//root/item[@paperExternalID='" + drv["paperExternalID"].ToString() + "']/summary");
                if (description != null)
                {
                    XmlCDataSection data = (XmlCDataSection)description.FirstChild;
                    summary = Common.HtmlRemoval.StripTagsRegex(data.InnerText);
                    object retval = null;
                    if(summary.Length>0)
                       WebDataUtility.Instance.webAppCmd("sp_UpdatePaperDescription", new GenericCmdParameter[] {
                            new GenericCmdParameter("@paperExternalID", Convert.ToInt32(drv["paperExternalID"])),
                            new GenericCmdParameter("@paperDescription", summary.Trim())}, ref retval);

                }
            }


            HyperLink lbl_Title = (HyperLink)e.Item.FindControl("lbl_Title");
            lbl_Title.NavigateUrl = "~/Files/EngConfContent/"
                + Convert.ToDateTime(System.Configuration.ConfigurationManager.AppSettings["ConferenceStart"]).ToString("yyyy")
                + "/Papers/Stage/" + Convert.ToInt32(drv["paperExternalID"]).ToString() + ".pdf";

            lbl_Title.Text = Common.ProperCase(Convert.ToString(drv["paperTitle"]));

            Label lbl_Description = (Label)e.Item.FindControl("lbl_Description");
            lbl_Description.Text = (summary == string.Empty? Convert.ToString(drv["paperDescription"]): summary);

            Label lbl_PrimaryAuthor = (Label)e.Item.FindControl("lbl_PrimaryAuthor");
            lbl_PrimaryAuthor.Text = Convert.ToString(drv["authorFullName"]) + ", " + Convert.ToString(drv["userDivision"]);
        
            Image img_PaperIcon = (Image)e.Item.FindControl("img_PaperIcon");
            img_PaperIcon.ImageUrl = "~/Images/" + parseFileType(Convert.ToString(drv["paperHyperlink"])) + ".png";

            HyperLink hl_File = (HyperLink)e.Item.FindControl("hl_File");
            hl_File.NavigateUrl = Convert.ToString(drv["paperHyperlink"]);

            hl_File.NavigateUrl = lbl_Title.NavigateUrl;

            hl_File.Text = Convert.ToString(drv["paperFileName"]);
            
            
            HyperLink hl_FileImage = (HyperLink)e.Item.FindControl("hl_FileImage");
            hl_FileImage.NavigateUrl = hl_File.NavigateUrl;

            Panel pnl_SlideDeck = (Panel)e.Item.FindControl("pnl_SlideDeck");
            if (Convert.ToString(drv["paperSlideDeckLink"]) != string.Empty)
            {

                HyperLink hl_SlideDeckImg = (HyperLink)e.Item.FindControl("hl_SlideDeckImg");
                HyperLink hl_SlideDeck = (HyperLink)e.Item.FindControl("hl_SlideDeck");
                Image img_SlideDeckIcon = (Image)e.Item.FindControl("img_SlideDeckIcon");

                img_SlideDeckIcon.ImageUrl = "~/Images/" + parseFileType(Convert.ToString(drv["paperSlideDeckLink"])) + ".png";
                hl_SlideDeck.NavigateUrl = Convert.ToString(drv["paperSlideDeckLink"]);
                hl_SlideDeckImg.NavigateUrl = Convert.ToString(drv["paperSlideDeckLink"]);
            }


            DataList ds_CoAuthors = (DataList)e.Item.FindControl("ds_CoAuthors");

            ds_CoAuthors.DataSource =WebDataUtility.Instance.webAppTable("sp_GetPaperCoAuthors",
                   new GenericCmdParameter[] { new GenericCmdParameter("@paperID", Convert.ToInt32(drv["paperID"])) });
            ds_CoAuthors.DataBind();
           
        }
    }

    protected void ds_CoAuthors_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item ||
            e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            Label lbl_CoAuthor = (Label)e.Item.FindControl("lbl_CoAuthor");
            lbl_CoAuthor.Text = Convert.ToString(drv["coAuthorFullName"]) + ", " + Convert.ToString(drv["userDivision"]);
        }
    }

    private string parseFileType(string filePath)
    {
        string temp = string.Empty;

        for (int i = filePath.Length - 1; i >= 0; i--)
        {
            if (filePath.Substring(i, 1) == ".")
            {
                temp = filePath.Substring(i+1, filePath.Length - (i+1));
                break;
            }
        }
        temp = temp.Replace("x", String.Empty);
        return temp;
    }


    protected void ds_balloonData_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        //if (e.Item.ItemType == ListItemType.Item ||
        //    e.Item.ItemType == ListItemType.AlternatingItem)
        //{
        //    DataRowView drv = (DataRowView)e.Item.DataItem;
        //    Label lbl = (Label)e.Item.FindControl("title");
        //    lbl.Text = drv["title"].ToString();
        //    Label lbl_description = (Label)e.Item.FindControl("lbl_description");
        //    lbl_description.Text = drv["summary"].ToString();
        //    DataList attachList = (DataList)e.Item.FindControl("ds_Attachments");
        //    attachList.ItemDataBound += new DataListItemEventHandler(ds_Attachments_ItemDataBound);
        //    String[] paramStrings = Convert.ToString(Request.QueryString["id"]).Split('/');
        //    SP_AttachmentList list = new SP_AttachmentList(Convert.ToInt32(paramStrings[0]));

        //    attachList.DataSource = list.Data;
        //    attachList.DataBind();

        //}
    }

    protected void ds_Attachments_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        //if (e.Item.ItemType == ListItemType.Item ||
        //    e.Item.ItemType == ListItemType.AlternatingItem)
        //{
        //    DataRowView drv = (DataRowView)e.Item.DataItem;
        //    Image img = (Image)e.Item.FindControl("attachmentExtension");
        //    HyperLink hl = (HyperLink)e.Item.FindControl("attachmentURL");
        //    HyperLink imgLink = (HyperLink)e.Item.FindControl("imgLink");

        //    loadDictionary();
        //    img.ImageUrl = images[drv["extension"].ToString()];
        //    hl.Text = drv["title"].ToString();
        //    hl.NavigateUrl = drv["url"].ToString();
        //    imgLink.NavigateUrl = drv["url"].ToString();
        //}
    }

}