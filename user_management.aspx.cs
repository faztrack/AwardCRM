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
using System.Collections.Generic;
using System.Web.Services;

public partial class user_management : System.Web.UI.Page
{
    [WebMethod]
    public static string[] GetLastName(String prefixText, Int32 count)
    {
        if (HttpContext.Current.Session["uSearch"] != null)
        {
            List<user_info> cList = (List<user_info>)HttpContext.Current.Session["uSearch"];
            return (from c in cList
                    where c.last_name.ToLower().StartsWith(prefixText.ToLower())
                    select c.last_name).Distinct().Take<String>(count).ToArray();
        }
        else
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            return (from c in _db.user_infos
                    where c.last_name.ToLower().StartsWith(prefixText.ToLower())
                    select c.last_name).Distinct().Take<String>(count).ToArray();
        }
    }
    [WebMethod]
    public static string[] GetFirstName(String prefixText, Int32 count)
    {
        if (HttpContext.Current.Session["uSearch"] != null)
        {
            List<user_info> cList = (List<user_info>)HttpContext.Current.Session["uSearch"];
            return (from c in cList
                    where c.first_name.ToLower().StartsWith(prefixText.ToLower())
                    select c.first_name).Distinct().Take<String>(count).ToArray();
        }
        else
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            return (from c in _db.user_infos
                    where c.first_name.ToLower().StartsWith(prefixText.ToLower())
                    select c.first_name).Distinct().Take<String>(count).ToArray();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);
            if (Session["oUser"] == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["LoginPage"].ToString());
            }
            if (Page.User.IsInRole("admin002") == false)
            {
                // No Permission Page.
                Response.Redirect("nopermission.aspx");
            }
            DataClassesDataContext _db = new DataClassesDataContext();
            List<user_info> UserList = _db.user_infos.ToList();
            Session.Add("uSearch", UserList);
            GetUsers(0);
        }
    }
    protected void GetUsers(int nPageNo)
    {
        string strCondition = string.Empty;
        if (Convert.ToInt32(ddlStatus.SelectedValue) == 1)
        {
            strCondition = "WHERE user_id != 6 AND is_active = 1 ";
        }
        else if (Convert.ToInt32(ddlStatus.SelectedValue) == 2)
        {
            strCondition = "WHERE user_id != 6 AND is_active = 0 ";

        }
        else
        {
            strCondition = "WHERE user_id != 6 ";
        }

        if (txtSearch.Text.Trim() != "")
        {
            string str = txtSearch.Text.Trim();

            if (ddlSearchBy.SelectedValue == "1") // First Name
            {
                if (strCondition.Length > 2)
                    strCondition += "AND first_name LIKE '%" + str + "%'";
                else
                    strCondition = "WHERE first_name LIKE '%" + str + "%'";
            }
            else if (ddlSearchBy.SelectedValue == "2") // Last Name
            {
                if (strCondition.Length > 2)
                    strCondition += "AND last_name LIKE '%" + str + "%'";
                else
                    strCondition = "WHERE last_name LIKE '%" + str + "%'";
            
            }
        
        }

        DataClassesDataContext _db = new DataClassesDataContext();
        grdUserList.PageIndex = nPageNo;
       

        string strQ = "SELECT user_id, first_name, last_name, address, city, state, zip, phone, fax, email,  role_id, is_active,  last_login_time, " +
                      " company_email FROM user_info " + strCondition + " order by last_name asc";

       
        IEnumerable<userinfo> uList = _db.ExecuteQuery<userinfo>(strQ, string.Empty).ToList();
        lblCount.Text = uList.Count().ToString();
        if (ddlItemPerPage.SelectedValue != "4")
        {
            grdUserList.PageSize = Convert.ToInt32(ddlItemPerPage.SelectedValue);
        }
        else
        {
            grdUserList.PageSize = 200;
        }
        grdUserList.DataSource = uList;
        grdUserList.DataKeyNames = new string[] { "user_id", "is_active" };
        grdUserList.DataBind();
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

        if (grdUserList.PageCount == nPageNo + 1)
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
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("user_details.aspx");
    }
    protected void grdUserList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            int nuid = Convert.ToInt32(grdUserList.DataKeys[e.Row.RowIndex].Values[0]);
            bool bAcitve = Convert.ToBoolean(grdUserList.DataKeys[e.Row.RowIndex].Values[1]);
            if (bAcitve)
            {
                e.Row.Cells[5].Text = "Yes";
            }
            else
            {
                e.Row.Cells[5].Text = "No";
            }
            int nRoleId = Convert.ToInt32(e.Row.Cells[4].Text);

            if (nRoleId == 1)
                e.Row.Cells[4].Text = "Admin";
            else if (nRoleId == 2)
                e.Row.Cells[4].Text = "Manager";
            else if (nRoleId == 3)
                e.Row.Cells[4].Text = "Sales";
            else if (nRoleId == 4)
                e.Row.Cells[4].Text = "Superintendent";
            else if (nRoleId == 5)
                e.Row.Cells[4].Text = "Operation";

            e.Row.Cells[3].Text = DateTime.Parse(Convert.ToDateTime(e.Row.Cells[3].Text).ToShortDateString() + " " + Convert.ToDateTime(e.Row.Cells[3].Text).ToLongTimeString()).ToString("g");

            // Customer Address
            //user_info uinfo = new user_info();
            //uinfo = _db.user_infos.Single(c => c.user_id == nuid);
            //string strAddress = uinfo.address + "</br>" + uinfo.city + " " + uinfo.state + " " + uinfo.zip;
            //e.Row.Cells[2].Text = strAddress;
        }
    }
    protected void grdUserList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, grdUserList.ID, grdUserList.GetType().Name, "PageIndexChanging"); 
        GetUsers(e.NewPageIndex);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSearch.ID, btnSearch.GetType().Name, "Click"); 
        GetUsers(0);
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnNext.ID, btnNext.GetType().Name, "Click"); 
        int nCurrentPage = 0;
        nCurrentPage = Convert.ToInt32(lblCurrentPageNo.Text);
        GetUsers(nCurrentPage);
    }
    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnPrevious.ID, btnPrevious.GetType().Name, "Click"); 
        int nCurrentPage = 0;
        nCurrentPage = Convert.ToInt32(lblCurrentPageNo.Text);
        GetUsers(nCurrentPage - 2);
    }
    protected void ddlItemPerPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, ddlItemPerPage.ID, ddlItemPerPage.GetType().Name, "SelectedIndexChanged"); 
        GetUsers(0);
    }
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, ddlStatus.ID, ddlStatus.GetType().Name, "SelectedIndexChanged"); 
        GetUsers(0);
    }
    protected void ddlSearchBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, ddlSearchBy.ID, ddlSearchBy.GetType().Name, "SelectedIndexChanged"); 
        txtSearch.Text = "";
        wtmFileNumber.WatermarkText = "Search by " + ddlSearchBy.SelectedItem.Text;

        if (ddlSearchBy.SelectedValue == "1")
        {
            txtSearch_AutoCompleteExtender.ServiceMethod = "GetFirstName";
        }
        else if (ddlSearchBy.SelectedValue == "2")
        {
            txtSearch_AutoCompleteExtender.ServiceMethod = "GetLastName";
        }

        GetUsers(0);
    }
    protected void lnkViewAll_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, LinkButton1.ID, LinkButton1.GetType().Name, "Click"); 
        txtSearch.Text = "";
        ddlStatus.SelectedValue = "3";
        GetUsers(0);
    }
}
