using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PreviewReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["conferenceID"] = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ConferenceID"]);
            ReportParameter Param0 = new ReportParameter("conferenceID", System.Configuration.ConfigurationManager.AppSettings["ConferenceID"]);
            ReportParameter Param1 = new ReportParameter("dateOfConference", String.Empty);
            rpt_Viewer.LocalReport.SetParameters(new ReportParameter[] { Param0 });
            rpt_Viewer.LocalReport.Refresh();
        }
    }
}