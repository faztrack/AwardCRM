using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class EmbeddedHost : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["EmbeddedToken"] != null)
        {
            this.hostframe.Attributes["src"] = Session["EmbeddedToken"].ToString();
        }
        else
        {
            Session["errorMessage"] = "No Embedded Token to start embedded session with.";
            Response.Redirect("error.aspx");
        }
    }
}
