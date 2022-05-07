using DataStreams.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class changeorder_report : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);
            btnExpList.Visible = false;
            lblStatus.Visible = false;
            ddlStatus.Visible = false;

            string strStartDate = string.Empty;
            string strEndDate = string.Empty;
            string strStatusId = string.Empty;

            if (Session["StartDate"] != null)
            {
                strStartDate = (string)Session["StartDate"];
                txtStartDate.Text = strStartDate;
            }
            if (Session["EndDate"] != null)
            {
                strEndDate = (string)Session["EndDate"];
                txtEndDate.Text = strEndDate;
            }
            if (Session["StatusId"] != null)
            {
                strStatusId = (string)Session["StatusId"];
                ddlStatus.SelectedValue = strStatusId;
            }

            if (strStartDate.Length > 0 && strEndDate.Length > 0)
            {
                LoadReportData(Convert.ToDateTime(strStartDate), Convert.ToDateTime(strEndDate));
            }
           

        }
    }

    private DataTable LoadDataTable()
    {
        DataTable table = new DataTable();
        table.Columns.Add("C/O Date", typeof(string));
        table.Columns.Add("Last Name", typeof(string));
        table.Columns.Add("First Name", typeof(string));
        table.Columns.Add("Estimate", typeof(string));
        table.Columns.Add("C/O Name", typeof(string));
        table.Columns.Add("Sales", typeof(string));
        table.Columns.Add("Status", typeof(string));
        table.Columns.Add("Execution Date", typeof(string));
        table.Columns.Add("Amount", typeof(string));
        table.Columns.Add("chage_order_id", typeof(int));
        table.Columns.Add("estimate_id", typeof(int));
        table.Columns.Add("customer_id", typeof(int));
        return table;
    }
    protected void btnViewReport_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnViewReport.ID, btnViewReport.GetType().Name, "Click");
        
        lblResult.Text = "";
        DateTime strStartDate = DateTime.Now;
        DateTime strEndDate = DateTime.Now;
        if (txtStartDate.Text == "")
        {
            lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Start Date is a required field");
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
                lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Invalid Start Date");
                return;
            }
            strStartDate = Convert.ToDateTime(txtStartDate.Text);
        }

        if (txtEndDate.Text == "")
        {
            lblResult.Text = csCommonUtility.GetSystemRequiredMessage("End Date is a required field");
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
                lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Invalid End Date");
                return;
            }
            strEndDate = Convert.ToDateTime(txtEndDate.Text);
        }
        if (strStartDate > strEndDate)
        {
            lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Invalid Date Range");
            return;
        }

        LoadReportData(strStartDate, strEndDate);
       
        //string strTitle = "Change Order Report ( Date Range: " + txtStartDate.Text + " to " + txtEndDate.Text + ", Status: " + ddlStatus.SelectedItem.Text+" )";

        //Session.Add("Title", strTitle);
       // Response.Redirect("ChangeOrderHtmlReport.aspx");

        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Popup", "window.open('COReport.aspx');", true);
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
    protected void btnExpList_Click(object sender, ImageClickEventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnExpList.ID, btnExpList.GetType().Name, "Click"); 
        DataTable dtTmp = (DataTable)Session["COTable"];
        Response.Clear();
        Response.ClearHeaders();


        dtTmp.Columns.Remove("chage_order_id");
        dtTmp.Columns.Remove("estimate_id");
        dtTmp.Columns.Remove("customer_id");

        using (CsvWriter writer = new CsvWriter(Response.OutputStream, ',', System.Text.Encoding.Default))
        {

            writer.WriteAll(dtTmp, false);
        }
        // Response.ContentType = "application/vnd.ms-excel";
        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        Response.AddHeader("Content-Disposition", "attachment; filename=COReport.csv");
        Response.End();
    }

    public void LoadReportData(DateTime strStartDate, DateTime strEndDate)
    {
        lblResult.Text = "";

        string strCondition = string.Empty;
        if (ddlStatus.SelectedItem.Text == "All")
        {
            strCondition = " AND CONVERT(DATETIME,ce.changeorder_date) BETWEEN '" + strStartDate + "' AND '" + strEndDate + "' ";
        }
        else
        {
            strCondition = " AND ce.change_order_status_id =" + Convert.ToInt32(ddlStatus.SelectedValue) + " AND CONVERT(DATETIME,ce.changeorder_date) BETWEEN '" + strStartDate + "' AND '" + strEndDate + "' ";
        }

        DataClassesDataContext _db = new DataClassesDataContext();
        //CASE change_order_type_id WHEN 1 THEN 'Change Order' WHEN 2 THEN 'Clarification'  ELSE 'Internal Use Only' END AS [Type]
        string strQ = " SELECT ce.changeorder_date ,last_name1  , first_name1,cee.estimate_name, ce.chage_order_id, ce.estimate_id, ce.customer_id,ce.changeorder_name, sp.last_name as Sales, CASE change_order_status_id WHEN 1 THEN 'Draft' WHEN 2 THEN 'Pending' WHEN 3 THEN 'Executed' ELSE 'Declined' END AS [Status], ce.execute_date,ce.tax  FROM changeorder_estimate ce " +
                       " INNER JOIN customers c on ce.customer_id = c.customer_id " +
                       " INNER JOIN   sales_person sp on sp.sales_person_id = c.sales_person_id "+
                       " INNER JOIN customer_estimate cee on ce.estimate_id = cee.estimate_id and ce.customer_id = cee.customer_id " +
                       " WHERE cee.status_id = 3 AND cee.IsEstimateActive = 1 AND c.status_id NOT IN(4,5) AND c.customer_id != 1" + strCondition + " ORDER BY CONVERT(DATETIME,ce.changeorder_date) ASC";

        DataTable dtReport = csCommonUtility.GetDataTable(strQ);
        DataTable tmpTable = LoadDataTable();
        DataTable tmpDataTable = LoadDataTable();
        DataRow drNew = tmpTable.NewRow();
        drNew["C/O Date"] = "Change Order Report";
        tmpTable.Rows.Add(drNew);

        drNew = tmpTable.NewRow();
        drNew["C/O Date"] = "Status:";
        drNew["Last Name"] = ddlStatus.SelectedItem.Text;
        tmpTable.Rows.Add(drNew);

        drNew = tmpTable.NewRow();
        drNew["C/O Date"] = "Date Range:";
        drNew["Last Name"] = txtStartDate.Text + " to " + txtEndDate.Text;
        tmpTable.Rows.Add(drNew);

        drNew = tmpTable.NewRow();
        tmpTable.Rows.Add(drNew);

        drNew = tmpTable.NewRow();
        drNew["C/O Date"] = "C/O Date";
        drNew["Last Name"] = "Last Name";
        drNew["First Name"] = "First Name";
        drNew["Estimate"] = "Estimate";
        drNew["C/O Name"] = "C/O Name";
        drNew["Sales"] = "Sales";
        drNew["Status"] = "Status";
        drNew["Execution Date"] = "Execution Date";
        drNew["Amount"] = "Amount";
        tmpTable.Rows.Add(drNew);


        foreach (DataRow dr in dtReport.Rows)
        {
            int nCustomerId = Convert.ToInt32(dr["customer_id"]);
            int nEstimateId = Convert.ToInt32(dr["estimate_id"]);
            int nChage_order_id = Convert.ToInt32(dr["chage_order_id"]);
            decimal taxRate = Convert.ToDecimal(dr["tax"]);

            decimal dEconCost = 0;
            var Coresult = (from chpl in _db.change_order_pricing_lists
                            where chpl.estimate_id == nEstimateId && chpl.customer_id == nCustomerId && chpl.client_id == 1 && chpl.chage_order_id == nChage_order_id
                            select chpl.EconomicsCost);
            int cn = Coresult.Count();
            if (Coresult != null && cn > 0)
                dEconCost = Coresult.Sum();

            decimal CoTax = dEconCost * (taxRate / 100);

            decimal CoPrice = 0;

            if (CoTax > 0)
            {
                CoPrice = dEconCost + CoTax;
            }
            else
            {
                CoPrice = dEconCost;
            }

            string strExeDate = string.Empty;
            if (dr["execute_date"].ToString() != "")
            {
                try
                {
                    strExeDate = Convert.ToDateTime(dr["execute_date"]).ToShortDateString();
                }
                catch
                {

                }

            }

            DataRow drNew1 = tmpDataTable.NewRow();
            drNew1["C/O Date"] = dr["changeorder_date"];
            drNew1["Last Name"] = dr["last_name1"];
            drNew1["First Name"] = dr["first_name1"];
            drNew1["Estimate"] = dr["estimate_name"];
            drNew1["C/O Name"] = dr["changeorder_name"];
            drNew1["Sales"] = dr["Sales"];
            drNew1["Status"] = dr["Status"];
            drNew1["Execution Date"] = strExeDate;
            drNew1["Amount"] = CoPrice.ToString("c");
            drNew1["chage_order_id"] = nChage_order_id;
            drNew1["estimate_id"] = nEstimateId;
            drNew1["customer_id"] = nCustomerId;

            tmpDataTable.Rows.Add(drNew1);
        }
        Session.Add("CODataTable", tmpDataTable);

        tmpTable.Merge(tmpDataTable);
        Session.Add("COTable", tmpTable);
        BindGrid();


    }

    public void BindGrid()
    {
        string strStartDate = txtStartDate.Text;
        string strEndDate = txtEndDate.Text;
        string strStatusId = ddlStatus.SelectedValue;

        if(strStartDate.Length>0)
            Session.Add("StartDate", strStartDate);
        if (strEndDate.Length > 0)
            Session.Add("EndDate", strEndDate);
        if (strStatusId.Length > 0)
            Session.Add("StatusId", strStatusId);

        DataTable dtTable = new DataTable();

        if (Session["CODataTable"] != null)
        {
            dtTable = (DataTable)Session["CODataTable"];
            btnExpList.Visible = true;
            lblStatus.Visible = true;
            ddlStatus.Visible = true;

           
        }
        grdChangeOrders.DataSource = dtTable;
        grdChangeOrders.DataKeyNames = new string[] { "customer_id", "estimate_id", "chage_order_id" };
        grdChangeOrders.DataBind();


    }


    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, ddlStatus.ID, ddlStatus.GetType().Name, "Change");

        lblResult.Text = "";
        DateTime strStartDate = DateTime.Now;
        DateTime strEndDate = DateTime.Now;
        if (txtStartDate.Text == "")
        {
            lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Start Date is a required field");
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
                lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Invalid Start Date");
                return;
            }
            strStartDate = Convert.ToDateTime(txtStartDate.Text);
        }

        if (txtEndDate.Text == "")
        {
            lblResult.Text = csCommonUtility.GetSystemRequiredMessage("End Date is a required field");
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
                lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Invalid End Date");
                return;
            }
            strEndDate = Convert.ToDateTime(txtEndDate.Text);
        }
        if (strStartDate > strEndDate)
        {
            lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Invalid Date Range");
            return;
        }

        LoadReportData(strStartDate, strEndDate);
    }
    protected void grdChangeOrders_Sorting(object sender, GridViewSortEventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, grdChangeOrders.ID, grdChangeOrders.GetType().Name, "Sorting");
        
        lblResult.Text = "";
        DataTable dt = (DataTable)Session["CODataTable"];

        string strShort = e.SortExpression + " " + hdnOrder.Value;

        DataView dv = dt.DefaultView;
        dv.Sort = strShort;
        Session["CODataTable"] = dv.ToTable();

        if (hdnOrder.Value == "ASC")
            hdnOrder.Value = "DESC";
        else
            hdnOrder.Value = "ASC";
        BindGrid();
    }


    protected void grdChangeOrders_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            grdChangeOrders.DataKeyNames = new string[] { "customer_id", "estimate_id", "chage_order_id" };

            int nCustomerId = Convert.ToInt32(grdChangeOrders.DataKeys[e.Row.RowIndex].Values[0]);
            int nEstimateId = Convert.ToInt32(grdChangeOrders.DataKeys[e.Row.RowIndex].Values[1]);
            int nChage_order_id = Convert.ToInt32(grdChangeOrders.DataKeys[e.Row.RowIndex].Values[2]); 

            HyperLink hypView = (HyperLink)e.Row.FindControl("hypView");
            hypView.Text = "View Details";
            hypView.NavigateUrl = "change_order_worksheet.aspx?ty=1&coestid=" + nChage_order_id + "&eid=" + nEstimateId + "&cid=" + nCustomerId;

        }
    }

   
}