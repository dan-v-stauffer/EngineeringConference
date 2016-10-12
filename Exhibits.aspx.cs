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


public partial class Exhibits : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        dl_Exhibits.DataSource = WebDataUtility.Instance.webAppTable("sp_GetConferenceExhibits",
            new GenericCmdParameter[] { new GenericCmdParameter("@conferenceID", Conference.Instance.ID) });
        dl_Exhibits.DataBind();
    }

    protected void dl_Exhibits_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drv = e.Item.DataItem as DataRowView;
            HyperLink hl_Exhibit = (HyperLink)e.Item.FindControl("hl_Exhibit");
            Image img_Thumbnail = (Image)e.Item.FindControl("img_Thumbnail");
            Label lbl_Org = (Label)e.Item.FindControl("lbl_Org");

            hl_Exhibit.NavigateUrl = DBNullable.ToString(drv["orgExhibitLink"]);
            img_Thumbnail.ImageUrl = DBNullable.ToString(drv["orgExhibitThumbnail"]);
            lbl_Org.Text = DBNullable.ToString(drv["orgFullName"]);

        }

    }
}