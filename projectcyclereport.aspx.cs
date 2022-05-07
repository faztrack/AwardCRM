using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class projectcyclereport : System.Web.UI.Page
{
    [WebMethod]
    public static string[] GetLastName(String prefixText, Int32 count)
    {
        if (HttpContext.Current.Session["clSearch"] != null)
        {
            List<customer> cList = (List<customer>)HttpContext.Current.Session["clSearch"];
            return (from c in cList
                    where c.last_name1.ToLower().StartsWith(prefixText.ToLower())
                    select c.last_name1).Take<String>(count).ToArray();
        }
        else
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            return (from c in _db.customers
                    where c.isCustomer == 1 && c.is_active == true && c.last_name1.StartsWith(prefixText)
                    select c.last_name1).Take<String>(count).ToArray();
        }
    }

    [WebMethod]
    public static string[] GetFirstName(String prefixText, Int32 count)
    {
        if (HttpContext.Current.Session["clSearch"] != null)
        {
            List<customer> cList = (List<customer>)HttpContext.Current.Session["clSearch"];
            return (from c in cList
                    where c.first_name1.ToLower().StartsWith(prefixText.ToLower())
                    select c.first_name1).Take<String>(count).ToArray();
        }
        else
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            return (from c in _db.customers
                    where c.isCustomer == 1 && c.is_active == true && c.first_name1.StartsWith(prefixText)
                    select c.first_name1).Take<String>(count).ToArray();
        }
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
            if (Page.User.IsInRole("projectcycle001") == false)
            {
                // No Permission Page.
                Response.Redirect("nopermission.aspx");
            }

            // Get Leads
            # region Get Customer

            DataClassesDataContext _db = new DataClassesDataContext();
            List<customer> CustomerList = _db.customers.Where(c => c.isCustomer == 1 && c.is_active == true).ToList();
            Session.Add("clSearch", CustomerList);

            # endregion
            int nPage = 0;
            txtSearch.Text = "";
            GetCustomerProjectList(nPage);

        }
    }

  protected void GetCustomerProjectList(int nPageNo)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        grdProjectRecycle.PageIndex = nPageNo;
        string strCondition = "";

        if (txtStartDate.Text != "" && txtEndDate.Text != "")
        {
            DateTime strStartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
            DateTime strEndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
            strCondition += " CONVERT(DATETIME,ce.sale_date)>='" + strStartDate + "' AND  CONVERT(DATETIME,ce.sale_date) <'" + strEndDate.AddDays(1).ToString() + "' ";
        }

        if (txtSearch.Text.Trim() != "")
        {
  

            string str = txtSearch.Text.Trim();
            if (ddlSearchBy.SelectedValue == "1")
            {
                strCondition = " customers.first_name1 LIKE '%" + str + "%'";
            }
            else if (ddlSearchBy.SelectedValue == "2")
            {
                strCondition = "  customers.last_name1 LIKE '%" + str + "%'";
            }

        }


        if (strCondition.Length > 0)
        {

            strCondition = " where customers.is_active=1 and customers.status_id=2 and ce.status_id=3 and s.IsEstimateActive=1 and s.employee_name != ''  and s.employee_name != 'TBD TBD' and " + strCondition;
        }
        else
        {
            strCondition = " where customers.is_active=1 and customers.status_id=2 and ce.status_id=3 and s.IsEstimateActive=1 and s.employee_name != ''  and s.employee_name != 'TBD TBD'";
        }

      

        string strQ=" select customers.first_name1 + ' ' + customers.last_name1 as customername,s.customer_id,s.estimate_id,u.first_name + ' ' + u.last_name as suername,sp.first_name + ' ' + sp.last_name as salesperson, MIN(event_start) as EventFisrtDay, getdate() as currentdate, MAX(event_end) as EventLastDay ,"+
                    " DATEDIFF(DAY, MAX(event_end),getdate()) AS LastAcitivityDate, DATEDIFF(DAY, MIN(event_start),getdate()) AS StartActivityDate, ce.sale_date,ce.job_number " +
                    " from[ScheduleCalendar]  as s " +
                    " inner join customers on customers.customer_id = s.customer_id "+
                    " inner join customer_estimate as ce on ce.customer_id = s.customer_id and ce.estimate_id = s.estimate_id "+ 
                    " inner join user_info as u on u.user_id = customers.SuperintendentId "+ 
                    " inner join sales_person as sp on sp.sales_person_id = customers.sales_person_id "+strCondition +
                   // " where customers.is_active = 1 and customers.status_id = 2 and ce.status_id = 3 and s.employee_name != ''  and s.employee_name != 'TBD TBD' " +
                    " group by s.customer_id,s.estimate_id,customers.last_name1,customers.first_name1,ce.sale_date,ce.job_number,u.first_name,u.last_name,sp.first_name,sp.last_name order by ce.sale_date";


      DataTable dt = csCommonUtility.GetDataTable(strQ);
        Session.Add("sProjectList", dt);

        grdProjectRecycle.DataSource = dt;
        grdProjectRecycle.DataKeyNames = new string[] { "customer_id", "estimate_id" };
        grdProjectRecycle.DataBind();
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

        if (grdProjectRecycle.PageCount == nPageNo + 1)
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



    protected void grdProjectRecycle_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int nCustomer_id = Convert.ToInt32(grdProjectRecycle.DataKeys[e.Row.RowIndex].Values[0]);
            int nEstimate_id = Convert.ToInt32(grdProjectRecycle.DataKeys[e.Row.RowIndex].Values[1]);
       }
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSearch.ID, btnSearch.GetType().Name, "Search Click");
        GetCustomerProjectList(0);
    }

    protected void ddlItemPerPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        GetCustomerProjectList(0);
    }

    protected void lnkViewAll_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, LinkButton2.ID, LinkButton2.GetType().Name, "View All Click");
        txtStartDate.Text ="";
        txtEndDate.Text = "";
        txtSearch.Text = "";
        GetCustomerProjectList(0);

    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnView.ID, btnView.GetType().Name, "View Click");

        lblResult.Text = "";
        DateTime strStartDate = DateTime.Now;
        DateTime strEndDate = DateTime.Now;
        if (txtStartDate.Text == "")
        {
            lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Sold Start Date is a required field");

            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtStartDate.Text);
            }
            catch (Exception ex)
            {
                lblResult.Text = csCommonUtility.GetSystemErrorMessage("Invalid Sold Start Date");

                return;
            }
            strStartDate = Convert.ToDateTime(txtStartDate.Text);
        }

        if (txtEndDate.Text == "")
        {
            lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Sold End Date is a required field");

            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtEndDate.Text);
            }
            catch (Exception ex)
            {
                lblResult.Text = csCommonUtility.GetSystemErrorMessage("Invalid Sold End Date");

                return;
            }
            strEndDate = Convert.ToDateTime(txtEndDate.Text);
        }
        if (strStartDate > strEndDate)
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage("Invalid Date Range");

            return;
        }

        GetCustomerProjectList(0);
    }

    protected void ddlSearchBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, ddlSearchBy.ID, ddlSearchBy.GetType().Name, "Search By Changed");
        txtSearch.Text = "";
        wtmFileNumber.WatermarkText = "Search by " + ddlSearchBy.SelectedItem.Text;
        if (ddlSearchBy.SelectedValue == "2")
        {
            txtSearch_AutoCompleteExtender.ServiceMethod = "GetLastName";
        }
        else if (ddlSearchBy.SelectedValue == "1")
        {
            txtSearch_AutoCompleteExtender.ServiceMethod = "GetFirstName";
        }


        GetCustomerProjectList(0);
    }


    protected void grdProjectRecycle_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, grdProjectRecycle.ID, grdProjectRecycle.GetType().Name, "Project Recycle Changed");
        Session.Add("sPage", e.NewPageIndex.ToString());
        GetCustomerProjectList(e.NewPageIndex);
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnNext.ID, btnNext.GetType().Name, "Click"); 
        int nCurrentPage = 0;
        nCurrentPage = Convert.ToInt32(lblCurrentPageNo.Text);
        Session.Add("sPage", nCurrentPage.ToString());
        GetCustomerProjectList(nCurrentPage);
    }
    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnPrevious.ID, btnPrevious.GetType().Name, "Click"); 
        int nCurrentPage = 0;
        nCurrentPage = Convert.ToInt32(lblCurrentPageNo.Text);
        Session.Add("sPage", (nCurrentPage - 2).ToString());
        GetCustomerProjectList(nCurrentPage - 2);
    }


  

    protected void grdProjectRecycle_Sorting(object sender, GridViewSortEventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, grdProjectRecycle.ID, grdProjectRecycle.GetType().Name, "Project Recycle Sorting");
        int nPageNo = 0;
        DataTable dtCallList = (DataTable)Session["sProjectList"];
        if (hdnOrder.Value == "ASC")
            hdnOrder.Value = "DESC";
        else
            hdnOrder.Value = "ASC";

        string strShort = e.SortExpression + " " + hdnOrder.Value;
        DataView dv = dtCallList.DefaultView;
        dv.Sort = strShort;
        Session["sProjectList"] = dv.ToTable();

        dtCallList = (DataTable)Session["sProjectList"];
        grdProjectRecycle.DataSource = dtCallList;
        grdProjectRecycle.DataKeyNames = new string[] { "customer_id", "estimate_id" };
        grdProjectRecycle.DataBind();
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

        if (grdProjectRecycle.PageCount == nPageNo + 1)
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

}