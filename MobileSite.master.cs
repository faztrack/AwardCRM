using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MobileSite : MasterPage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.User.Identity.IsAuthenticated)
        {
             lblEmpName.Text = this.Context.User.Identity.Name;
            // lnkSignOut.Text = "Welcome - " + this.Context.User.Identity.Name + " (Logout)";
            //lnkSignOut.Text = "  Logout                                      ";
            //wlnkSignOut.Text = this.Context.User.Identity.Name + " (Logout)";
        
            //lnkSignOut.Text = "  Logout                                      ";
        }

    }

    protected void lnkSignOut_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, lnkSignOut.ID, lnkSignOut.GetType().Name, "Click"); 
        Session.RemoveAll();

        if (HttpContext.Current.User.Identity.IsAuthenticated == false)
        {
            return;
        }
        FormsAuthentication.SignOut();
        GenericIdentity identity = new GenericIdentity("", "");
        // This principal will flow throughout the request.
        GenericPrincipal principal = new GenericPrincipal(identity, new string[] { });
        // Attach the new principal object to the current HttpContext object
        HttpContext.Current.User = principal;
        Response.Redirect("mobile.aspx");
    }
}