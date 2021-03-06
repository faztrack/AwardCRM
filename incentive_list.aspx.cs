using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;
using System.Web.Services;
using System.Collections.Generic;

public partial class incentive_list : System.Web.UI.Page
{
    [WebMethod]
    public static string[] GetIncentiveName(String prefixText, Int32 count)
    {
        if (HttpContext.Current.Session["iSearch"] != null)
        {
            List<incentive> cList = (List<incentive>)HttpContext.Current.Session["iSearch"];
            return (from c in cList
                    where c.incentive_name.ToLower().StartsWith(prefixText.ToLower())
                    select c.incentive_name).Distinct().Take<String>(count).ToArray();
        }
        else
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            return (from c in _db.incentives
                    where c.incentive_name.StartsWith(prefixText)
                    select c.incentive_name).Distinct().Take<String>(count).ToArray();
        }
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        DateTime end = (DateTime)Session["loadstarttime"];
        TimeSpan loadtime = DateTime.Now - end;
        lblLoadTime.Text = (Math.Round(Convert.ToDecimal(loadtime.TotalSeconds), 3).ToString()) + " Sec";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Add("loadstarttime", DateTime.Now);
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);
            if (Session["oUser"] == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["LoginPage"].ToString());
            }
            if (Page.User.IsInRole("admin005") == false)
            {
                // No Permission Page.
                Response.Redirect("nopermission.aspx");
            }
            DataClassesDataContext _db = new DataClassesDataContext();
            List<incentive> iList = _db.incentives.ToList();
            Session.Add("iSearch", iList);

            GetIncentives(0);
        }
    }

    protected void GetIncentives(int nPageNo)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        lblResult.Text = "";
        company_profile objComp = new company_profile();
        objComp = _db.company_profiles.Single(com => com.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]));
        // Is Incentive Active/Inactive
        chkIsActive.Checked = Convert.ToBoolean(objComp.IsIncentiveActive);

        grdIncentive.PageIndex = nPageNo;

        var item = from inc in _db.incentives
                   where inc.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                   orderby inc.incentive_name
                   select inc;

        if (txtSearch.Text.Trim() != "")
        {
            string str = txtSearch.Text.Trim();

            item = from inc in _db.incentives
                   where inc.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && inc.incentive_name.Contains(str)
                   orderby inc.incentive_name
                   select inc;
        }

        if (ddlItemPerPage.SelectedValue != "4")
        {
            grdIncentive.PageSize = Convert.ToInt32(ddlItemPerPage.SelectedValue);
        }
        else
        {
            grdIncentive.PageSize = 200;
        }
        grdIncentive.DataSource = item;
        grdIncentive.DataKeyNames = new string[] { "incentive_type", "discount", "amount" };
        grdIncentive.DataBind();

        lblCurrentPageNo.Text = Convert.ToString(nPageNo + 1);
        if (nPageNo == 0)
        {
            btnPrevious.Enabled = false;
            btnPrevious0.Enabled = false;
        }
        else
        {
            btnPrevious.Enabled = true;
            btnPrevious0.Enabled = true;
        }

        if (grdIncentive.PageCount == nPageNo + 1)
        {
            btnNext.Enabled = false;
            btnNext0.Enabled = false;
        }
        else
        {
            btnNext.Enabled = true;
            btnNext0.Enabled = true;
        }
    }
    protected void grdIncentive_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, grdIncentive.ID, grdIncentive.GetType().Name, "PageIndexChanging"); 
        GetIncentives(e.NewPageIndex);
    }
    protected void grdIncentive_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int incentiveType = Convert.ToInt32(grdIncentive.DataKeys[e.Row.RowIndex].Values[0]);
            string discount = grdIncentive.DataKeys[e.Row.RowIndex].Values[1].ToString();
            string amount = grdIncentive.DataKeys[e.Row.RowIndex].Values[2].ToString();
            Label lblDiscount = (Label)e.Row.FindControl("lblDiscount");
            if (incentiveType == 1)
            {
                lblDiscount.Text = discount + "%";
            }
            else
            {
                lblDiscount.Text = "$" + amount;
            }

            if (Convert.ToBoolean(e.Row.Cells[3].Text) == true)
                e.Row.Cells[3].Text = "Yes";
            else
                e.Row.Cells[3].Text = "No";
        }
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("incentivedetails.aspx");
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSearch.ID, btnSearch.GetType().Name, "Click"); 
        lblResult.Text = "";
        GetIncentives(0);
    }
    protected void chkIsActive_click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, chkIsActive.ID, chkIsActive.GetType().Name, "Click"); 
        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            company_profile objComp = new company_profile();

            objComp = _db.company_profiles.SingleOrDefault(c => c.client_id == 1);

            objComp.IsIncentiveActive = chkIsActive.Checked;
            _db.SubmitChanges();
            lblResult.Text = csCommonUtility.GetSystemMessage("Data updated successfully.");
        }
        catch (Exception ex)
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage(ex.Message);
        }
    }

    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnPrevious.ID, btnPrevious.GetType().Name, "Click"); 
        int nCurrentPage = 0;
        nCurrentPage = Convert.ToInt32(lblCurrentPageNo.Text);
        GetIncentives(nCurrentPage - 2);
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnNext.ID, btnNext.GetType().Name, "Click"); 
        int nCurrentPage = 0;
        nCurrentPage = Convert.ToInt32(lblCurrentPageNo.Text);
        GetIncentives(nCurrentPage);
    }

    protected void ddlItemPerPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, ddlItemPerPage.ID, ddlItemPerPage.GetType().Name, "SelectedIndexChanged"); 
        GetIncentives(0);
    }

    protected void lnkViewAll_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, LinkButton1.ID, LinkButton1.GetType().Name, "Click"); 
        txtSearch.Text = "";
        GetIncentives(0);
    }
}
