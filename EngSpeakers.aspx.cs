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


public partial class EngSpeakers : System.Web.UI.Page
{

    private bool isTopCategory = true;
    private bool isBottomCategory = false;
    private int j = 1;

    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            getListData();
            String userName = User.Identity.Name;
            KTLogin login = new KTLogin(userName);
            KTConferenceUser user = new KTConferenceUser(new KTActiveDirectoryUser(new KTLogin(userName)));
            Session["user"] = user;
            existingGroupBy = "abstractCategory";

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "_BindEvents_Page_Load" + this.ClientID,
                "BindEvents();", true);

        }
        else {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "_BindEvents_Page_Load" + this.ClientID,
                "BindEvents();", true);
        }

        existingGroupBy = ddl_GroupBy.SelectedValue;
        dl_StageCategories.DataSource = getCategories();
        dl_StageCategories.DataBind();

    }


    private void getListData()
    {
        //            Session["stageTable"] = null;

        int presentationType = (Request.QueryString["presType"] != null? Convert.ToInt32(Request.QueryString["presType"]):1);


        DataTable allData = WebDataUtility.Instance.webAppTable("sp_SelectedWhitePapersForWebsite",
            new GenericCmdParameter[] { new GenericCmdParameter("@conferenceID", Conference.Instance.ID),
                                        new GenericCmdParameter("@paperSelectedType", presentationType)});
        Session["stageTable"] = allData;

    }

    public DataTable getCategories()
    {
        DataTable table = new DataTable("categories");
        table.Columns.Add(new DataColumn("category", typeof(String)));
        table.Columns.Add(new DataColumn("count", typeof(int)));

        String[] categories;

        if (existingGroupBy == "abstractCategory")
            categories = Common.categories;
        else if (existingGroupBy == "authorDivision")
        {
            DataTable divs = WebDataUtility.Instance.webAppTable("sp_GetDivsions", null);
            List<string> list = new List<string>();
            foreach (DataRow row in divs.Rows)
            {
                list.Add(DBNullable.ToString(row["divisionText"]));
            }
            categories = list.ToArray();
        }

        else
            categories = Common.alphabet;

        DataTable masterTable = (DataTable)Session["stageTable"];

        String keyWordFilter = string.Empty;

        foreach (String str in categories)
        {
            String filter = existingGroupBy + "= '" + str.ToLower() + "'" + (keyWordFilter != String.Empty ? " AND " + keyWordFilter : String.Empty);
            int numberOfRecords = masterTable.Select(filter).Length;
            object[] values = { str, numberOfRecords };
            if (numberOfRecords > 0)
                table.Rows.Add(values);

        }
        if (existingGroupBy == "abstractCategory")
            table.DefaultView.Sort = "count DESC, category ASC";
        else
            table.DefaultView.Sort = "category ASC";

        DataTable dtOut = table.DefaultView.ToTable();
        return dtOut;
    }

    string existingGroupBy = "";


    protected void dl_Categories_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drv = e.Item.DataItem as DataRowView;
            DataList dl_CategoryItem = e.Item.FindControl("dl_CategoryItem") as DataList;

            string type = dl_CategoryItem.Attributes["PresentationType"];

            String filterExpression = string.Empty;
            DataTable table = (DataTable)Session["stageTable"];

            switch(existingGroupBy)
            {
                case "abstractCategory":
                    {
                        filterExpression = existingGroupBy + " = '" + drv["category"].ToString() + "'";
                        table = SortTable(table, "authorLastName", "ASC");
                        break;
                    }
                case "authorLastNameStart":
                    {
                        filterExpression = existingGroupBy + " = '" + drv["category"].ToString() + "'";
                        table = SortTable(table, "authorLastName", "ASC");
                        break;
                    }
                case "authorDivision":
                    {
                        filterExpression = existingGroupBy + " = '" + drv["category"].ToString() + "'";
                        table = SortTable(table, "authorLastName", "ASC");
                        break;
                    }
                case "paperTitleStart":
                    {
                        filterExpression = existingGroupBy + " = '" + drv["category"].ToString() + "'";
                        table = SortTable(table, "paperTitle", "ASC");
                        break;
                    }
                
            }
            

            String keywordFilter = tb_Filter.Text.Length>0? getKeyWordFilterString(tb_Filter.Text):string.Empty;

            keywordFilter = keywordFilter.Length > 0 ? " AND " + keywordFilter : String.Empty;

            DataView dvFitlterdView = new DataView(table);
            dl_CategoryItem.ItemDataBound += new DataListItemEventHandler(dl_CategoryItem_ItemDataBound);
            dvFitlterdView.RowFilter = filterExpression + keywordFilter;
            dl_CategoryItem.DataSource = dvFitlterdView;

            dl_CategoryItem.DataBind();
            j++;
            isTopCategory = false;
            isBottomCategory = (j == drv.DataView.Count);
        }
    }
    int itemCounter = 0;

    protected void dl_CategoryItem_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView drv = e.Item.DataItem as DataRowView;
            DataList dlist = (DataList)sender;
            int presType = Request.QueryString["presType"] != null ? Convert.ToInt32(Request.QueryString["presType"]) : 1;

            string type = dlist.Attributes["PresentationType"];

            int rowCount = drv.DataView.Count;
            if (rowCount % 2 == 1) rowCount++;

            Label lblTitle = (Label)e.Item.FindControl("lblTitle");
            Label lblPresenter = (Label)e.Item.FindControl("lbl_Presenter");
            itemCounter++;

            Label lblLeadAuthor = (Label)e.Item.FindControl("lblLeadAuthor");
            Label lblCoAuthors = (Label)e.Item.FindControl("lblCoAuthors");
            HtmlGenericControl divCoAuthors = (HtmlGenericControl)e.Item.FindControl("divCoAuthors");
            HtmlGenericControl divPresenter = (HtmlGenericControl)e.Item.FindControl("divPresenter");

            lblTitle.Text = drv["paperTitle"].ToString();
            lblLeadAuthor.Text = drv["authorName"].ToString();

            string coAuthors = getCoAuthors(Convert.ToInt32(drv["ID"]));

            if (coAuthors != "None")
            {
                lblCoAuthors.Text = coAuthors;
                divCoAuthors.Attributes["style"] = "display:block";
            }
            else
            {
                divCoAuthors.Attributes["style"] = "display:none";
            }

            if (!(DBNullable.ToString(drv["authorName"]).Equals(DBNullable.ToString(drv["presenterName"]))))
            {
                lblPresenter.Text = DBNullable.ToString(drv["presenterName"]);
                divPresenter.Attributes["style"] = "display:block";
            }
       

            HyperLink hl_PaperPDF = (HyperLink)e.Item.FindControl("hl_PaperPDF");
            HyperLink hl_Paper = (HyperLink)e.Item.FindControl("hl_Paper");
            Image img_PaperPDF = (Image)e.Item.FindControl("img_PaperPDF");

            hl_PaperPDF.NavigateUrl = DBNullable.ToString(drv["paperFileLink"]);
            hl_Paper.NavigateUrl = DBNullable.ToString(drv["paperFileLink"]);

            HyperLink hl_SlidesPDF = (HyperLink)e.Item.FindControl("hl_SlidesPDF");
            HyperLink hl_Slides = (HyperLink)e.Item.FindControl("hl_Slides");
            Image img_SlidesPDF = (Image)e.Item.FindControl("img_SlidesPDF");

            hl_SlidesPDF.NavigateUrl =  DBNullable.ToString(drv["paperSlidesLink"]);
            hl_Slides.NavigateUrl = DBNullable.ToString(drv["paperSlidesLink"]);
            hl_Slides.Text = (presType == 1 ? "Presentation" : "Poster");
            HyperLink hl_imgSummary = (HyperLink)e.Item.FindControl("hl_imgSummary");
            HyperLink hl_Summary = (HyperLink)e.Item.FindControl("hl_Summary");

            hl_imgSummary.Attributes.Add("rel", "Balloon.aspx?type=paper&id=" + DBNullable.ToInt(drv["ID"]));
            hl_Summary.Attributes.Add("rel", "Balloon.aspx?type=paper&id=" + DBNullable.ToInt(drv["ID"]));

            HyperLink hl_imgVideo = (HyperLink)e.Item.FindControl("hl_imgVideo");
            HyperLink hl_Video = (HyperLink)e.Item.FindControl("hl_Video");
            
            hl_imgVideo.Visible = (presType == 1 ? true : false);
            hl_Video.Visible = (presType == 1 ? true : false);

            hl_Video.Text = (DBNullable.ToString(drv["videoLink"]).Equals(String.Empty)?"Video Coming Soon":"Video");
            hl_Video.NavigateUrl = DBNullable.ToString(drv["videoLink"]);


        }
    }

    private string getCoAuthors(int paperID)
    {
        DataTable tbl = WebDataUtility.Instance.webAppTable("sp_GetCoAuthors", 
            new GenericCmdParameter[] { new GenericCmdParameter("@paperID", paperID) });
        StringBuilder b = new StringBuilder();
        if(tbl.Rows.Count == 0)
            return "None";
        else{
            int i = 1;
            foreach (DataRow row in tbl.Rows)
            {
                b.Append(row["coAuthorName"]);
                if (++i <= tbl.Rows.Count)
                    b.Append(", ");
            }   
        }
        return b.ToString();
    }

    protected void dl_Categories_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    
    protected void ddl_GroupBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "_BindEvents_Page_Load" + this.ClientID,
    "BindEvents();", true);
    }

    protected void bn_Find_Click(object sender, EventArgs e)
    {
      
        existingGroupBy = ddl_GroupBy.SelectedValue;
        dl_StageCategories.DataSource = getCategories();
        dl_StageCategories.DataBind();
        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "_BindEvents_bnFind" + this.ClientID,
        //    "BindEvents();", true);


    }

    protected void bn_Clear_Click(object sender, EventArgs e)
    {

        tb_Filter.Text = string.Empty;
        existingGroupBy = ddl_GroupBy.SelectedValue;
        dl_StageCategories.DataSource = getCategories();
        dl_StageCategories.DataBind();
        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "_BindEvents_bnClear" + this.ClientID,
        //    "BindEvents();", true);

    }

    private DataTable SortTable(DataTable dt, string colName, string direction)
    {
        dt.DefaultView.Sort = colName + " " + direction;
        dt = dt.DefaultView.ToTable();
        return dt;
    }

    private string getKeyWordFilterString(string keyWord)
    {
        StringBuilder builder = new StringBuilder();

        DataTable table = (DataTable)Session["stageTable"];

        bool isFirst = true;
        
        builder.Append(" (");

        foreach (DataColumn col in table.Columns)
        {
            if (col.DataType == typeof(string))
            {
                if (!isFirst)
                {
                    builder.Append(" OR (" + col.ColumnName + " LIKE '%" + keyWord + "%') ");

                }
                else
                {
                    builder.Append(" (" + col.ColumnName + " LIKE '%" + keyWord + "%') ");
                    isFirst = false;
                }
            }

        }

        builder.Append(") ");
        return builder.ToString();
    }
}