using ConferenceLibrary;
using DataUtilities;
using DataUtilities.SQLServer;
using DataUtilities.KTActiveDirectory;
using HelperFunctions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    WebDataUtility dataUtil = WebDataUtility.Instance;

    protected void Page_Load(object sender, EventArgs e)
    {
        //repeater_ConfMetaData
        if (!IsPostBack)
        {
            Conference confMetaData = (Conference)Application["Conference"];

            repeater_ConfMetaData.DataSource = confMetaData.ToTable();
            repeater_ConfMetaData.DataBind();
        }
    }
    protected void repeater_ConfMetaData_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            String strRegCloses = Convert.ToString(drv["conferenceRegistrationClosed"]);
            DateTime dateRegCloses;
            Session["registrationCloses"] = DateTime.TryParse(strRegCloses, out dateRegCloses);

            Label confCheckInStart = (Label)e.Item.FindControl("lbl_ConfCheckInStart");
            Label hotelCheckInStart = (Label)e.Item.FindControl("lbl_HotelCheckInStart");
            Label confCheckInStop = (Label)e.Item.FindControl("lbl_ConfCheckInStop");
            Label confStart = (Label)e.Item.FindControl("lbl_ConfStart");
            Label confStop = (Label)e.Item.FindControl("lbl_ConfStop");
            Label receptionStart = (Label)e.Item.FindControl("lbl_WelcomeReceptionStart");
            Label receptionStop = (Label)e.Item.FindControl("lbl_WelcomeReceptionStop");
            Label regCloses = (Label)e.Item.FindControl("lbl_RegistrationCloses");
            HyperLink venueLink = (HyperLink)e.Item.FindControl("hl_VenueLink");
            Label venueAddress = (Label)e.Item.FindControl("lbl_VenueAddress");
            HyperLink venueMapLink = (HyperLink)e.Item.FindControl("hl_VenueMapLink");
            //conferenceWelcomeReception
            confCheckInStart.Text = Convert.ToDateTime(drv["conferenceCheckInStart"].ToString()).ToString("ddd, MMM d, yyyy h:mm tt");
            hotelCheckInStart.Text = confCheckInStart.Text;
            confCheckInStop.Text = Convert.ToDateTime(drv["conferenceCheckInStop"].ToString()).ToString("h:mm tt");
            confStart.Text = Convert.ToDateTime(drv["conferenceStartTime"].ToString()).ToString("ddd, MMM d, yyyy h:mm tt");
            confStop.Text = Convert.ToDateTime(drv["conferenceEndTime"].ToString()).ToString("ddd, MMM d, yyyy h:mm tt");
            regCloses.Text = Convert.ToDateTime(drv["conferenceRegistrationClosed"].ToString()).ToString("ddd, MMM d, yyyy h:mm tt");
            receptionStart.Text = Convert.ToDateTime(drv["receptionStart"].ToString()).ToString("ddd, MMM d, yyyy h:mm");
            receptionStop.Text = Convert.ToDateTime(drv["receptionStop"].ToString()).ToString("h:mm tt");
            venueLink.NavigateUrl = drv["venueWebAddress"].ToString();
            venueLink.Text = drv["venueName"].ToString();
            venueAddress.Text = String.Format("{0}<br>{1}, {2} {3} ",
                new object[] { drv["venueStreetAddress"].ToString(), 
                             drv["venueCity"].ToString(), 
                             drv["venueState"].ToString(), 
                             drv["venueZip"].ToString() });

            venueMapLink.NavigateUrl = drv["venueMapHyperlink"].ToString();

        }
    }

}
