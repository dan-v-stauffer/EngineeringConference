using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public enum pageType
{ Start, Auto, Finish }


public delegate void PreviousButtonClickedHandler(object sender, EventArgs e);
public delegate void NextButtonClickedHandler(object sender, EventArgs e);
public delegate void FinishButtonClickedHandler(object sender, EventArgs e);
public delegate void CancelButtonClickedHandler(object sender, EventArgs e);


public partial class CustomControls_WizardActionRow : System.Web.UI.UserControl
{
    public event PreviousButtonClickedHandler PreviousClicked;
    public event NextButtonClickedHandler NextClicked;
    public event FinishButtonClickedHandler FinishClicked;
    public event CancelButtonClickedHandler CancelClicked;

    protected pageType type;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Attributes["PageType"] == null)
            throw new IndexOutOfRangeException("Invalid value for PageType in Wizard Control.");
        
        switch (this.Attributes["PageType"])
        {
            case "Start":
                { type = pageType.Start; break; }
            case "Auto":
                { type = pageType.Auto; break; }
            case "Finish":
                { type = pageType.Finish; break; }
        }


        switch (type)
            {
                case pageType.Start:
                    {
                        Previous.Style["display"] = "none";
                        Finish.Style["display"] = "none";
                        break;
                    }
                case pageType.Auto:
                    {
                        Finish.Style["display"] = "none";
                        break;
                    }
                case pageType.Finish:
                    {
                        Next.Style["display"] = "none";
                        break;
                    }
            }
       
    }
    public virtual void OnPreviousClicked(object sender, EventArgs e)
    {
        PreviousClicked(sender, e);
    }
    public virtual void OnNextClicked(object sender, EventArgs e)
    {
        NextClicked(sender, e);
    }
    public virtual void OnFinishClicked(object sender, EventArgs e)
    {
        FinishClicked(sender, e);
    }
    public virtual void OnCancelClicked(object sender, EventArgs e)
    {
        CancelClicked(sender, e);
    }

}

