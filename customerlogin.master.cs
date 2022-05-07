using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class customerlogin : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);
            //if (Request.Url.Scheme == "http")
            //{
            //  //  Response.Redirect("https://awardkb.faztrack.com/customerlogin.aspx");
            //}

            string strUseHTTPS = System.Configuration.ConfigurationManager.AppSettings["UseHTTPS"];
            if (strUseHTTPS == "Yes")
            {
                if (Request.Url.AbsoluteUri.Contains("awardkb.faztrack.com"))
                {
                    Response.Redirect("https://awardkb.faztrack.com");
                }

                if (Request.Url.Scheme == "http")
                {
                    string strQuery = Request.Url.AbsoluteUri;
                    Response.Redirect(strQuery.Replace("http:", "https:"));
                }
            }
            else if (strUseHTTPS == "No")
            {
                if (Request.Url.Scheme == "https")
                {
                    string strQuery = Request.Url.AbsoluteUri;
                    Response.Redirect(strQuery.Replace("https:", "http:"));
                }
            }
        }
    }
}
