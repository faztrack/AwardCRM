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
using System.Drawing;
using CrystalDecisions.CrystalReports.Engine;
using System.Web.Services;


public partial class schedulereport : System.Web.UI.Page
{
    [WebMethod]
    public static string[] GetLastName(String prefixText, Int32 count)
    {
        if (HttpContext.Current.Session["spSearch"] != null)
        {
            List<customer> cList = (List<customer>)HttpContext.Current.Session["spSearch"];
            return (from c in cList
                    where c.last_name1.ToLower().StartsWith(prefixText.ToLower())
                    select c.last_name1).Take<String>(count).ToArray();
        }
        else
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            return (from c in _db.customers
                    where c.last_name1.StartsWith(prefixText)
                    select c.last_name1).Take<String>(count).ToArray();
        }
    }

    [WebMethod]
    public static string[] GetFirstName(String prefixText, Int32 count)
    {
        if (HttpContext.Current.Session["spSearch"] != null)
        {
            List<customer> cList = (List<customer>)HttpContext.Current.Session["spSearch"];
            return (from c in cList
                    where c.first_name1.ToLower().StartsWith(prefixText.ToLower())
                    select c.first_name1).Take<String>(count).ToArray();
        }
        else
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            return (from c in _db.customers
                    where c.first_name1.StartsWith(prefixText)
                    select c.first_name1).Take<String>(count).ToArray();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);

            if (Session["oUser"] == null)
            {
                Response.Redirect("login.aspx");
            }

            if (Page.User.IsInRole("rpt007") == false)
            {
                // No Permission Page.
                Response.Redirect("nopermission.aspx");
            }
            // Get Customers
            # region Get Customers

            DataClassesDataContext _db = new DataClassesDataContext();
            List<customer> CustomerList = _db.customers.ToList();
            Session.Add("spSearch", CustomerList);

            # endregion

            GetEmplyee();
        }
    }

    private void GetEmplyee()
    {
        try
        {

            int nclient_id = Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
            DataClassesDataContext _db = new DataClassesDataContext();
            string strQ = "SELECT DISTINCT sp.first_name + ' '+sp.last_name AS sales_person_name,sp.sales_person_id " +
                        " FROM sales_person sp " +
                        " INNER JOIN customer_estimate ce ON ce.sales_person_id = sp.sales_person_id AND ce.client_id = sp.client_id " +
                        " WHERE ce.client_id = " + nclient_id + " AND sp.client_id = " + nclient_id +
                        " ORDER BY sales_person_name ASC";
            List<userinfo> mList = _db.ExecuteQuery<userinfo>(strQ, string.Empty).ToList();
            ddlSalesRep.DataSource = mList;
            ddlSalesRep.DataTextField = "sales_person_name";
            ddlSalesRep.DataValueField = "sales_person_id";
            ddlSalesRep.DataBind();
            ddlSalesRep.Items.Insert(0, "All");
            ddlSalesRep.SelectedIndex = 0;
            ddlSalesRep.DataBind();

            ddlSalesRep.Items.Insert(0, "ALL");
            ddlSalesRep.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnReport_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnReport.ID, btnReport.GetType().Name, "Click");
        lblResult.Text = "";
        DataClassesDataContext _db = new DataClassesDataContext();
        string strSearchDate = "";
        string strSearchBy = "Sales Person: ALL";
        string strCondition = string.Empty;
        if (!chkAll.Checked)
        {




            if (txtEndDate.Text.Length > 1)
            {
                if (txtStartDate.Text.Length > 1)
                {
                    try
                    {
                        Convert.ToDateTime(txtStartDate.Text.Trim());
                    }
                    catch
                    {
                        lblResult.Text = csCommonUtility.GetSystemErrorMessage("Start Date is requierd");
                        return;
                    }

                    try
                    {
                        Convert.ToDateTime(txtEndDate.Text.Trim());
                    }
                    catch
                    {
                        lblResult.Text = csCommonUtility.GetSystemErrorMessage("End Date is requierd");
                        return;
                    }
                }
                else
                {
                    lblResult.Text = csCommonUtility.GetSystemErrorMessage("Start Date is requierd");
                    return;
                }
                DateTime dt1 = Convert.ToDateTime(txtStartDate.Text.Trim());
                DateTime dt2 = Convert.ToDateTime(txtEndDate.Text.Trim());


                if (dt2 >= dt1)
                {
                    strCondition = " WHERE c.customer_id != 0 AND sc.type_id = 1 AND sc. event_start  between '" + dt1 + "' and '" + dt2 + "'";

                    strSearchDate = dt1.ToShortDateString() + "  to  " + dt2.ToShortDateString();
                }
                else
                {
                    lblResult.Text = csCommonUtility.GetSystemErrorMessage("End Date must be greater than Start Date.");
                    return;
                }
            }
        }

        if (txtSearch.Text.Trim() != "")
        {
            string str = txtSearch.Text.Trim();
            if (ddlSearchBy.SelectedValue == "1")
            {
                if (strCondition.Length > 5)
                    strCondition += "AND c.customer_id != 0 AND sc.type_id = 1 AND c.first_name1 LIKE '%" + str + "%'";
                else
                    strCondition += "WHERE c.customer_id != 0 AND sc.type_id = 1 AND c.first_name1 LIKE '%" + str + "%'";
            }
            else
            {
                if (strCondition.Length > 5)
                    strCondition += "AND c.customer_id != 0 AND sc.type_id = 1 AND c.last_name1 LIKE '%" + str + "%'";
                else
                    strCondition += "WHERE c.customer_id != 0 AND sc.type_id = 1 AND c.last_name1 LIKE '%" + str + "%'";
            }

        }

        if (ddlSalesRep.SelectedItem.Text != "ALL")
        {
            if (strCondition.Length > 5)
                strCondition += "AND c.customer_id != 0 AND sc.type_id = 1 AND c.sales_person_id = " + Convert.ToInt32(ddlSalesRep.SelectedValue);
            else
                strCondition += "WHERE c.customer_id != 0 AND sc.type_id = 1 AND c.sales_person_id = " + Convert.ToInt32(ddlSalesRep.SelectedValue);

            strSearchBy = "Sales Person: " + ddlSalesRep.SelectedItem.Text;
        }

       

        try
        {

            string strQ = string.Empty;
            if (strCondition.Length == 0)
            {
                strQ = " SELECT c.customer_id, c.first_name1+' '+c.last_name1 as customer_name, " +
                        "sp.first_name+' '+sp.last_name as employee_name, " +
                        "sc.title, sc.description, sc.type_id, " +
                        "sc.section_name, sc.event_start, sc.event_end " +
                        "FROM customers c " +
                        "INNER JOIN schedule_calendar sc ON c.customer_id = sc.customer_id " +
                        "INNER JOIN sales_person sp ON c.sales_person_id = sp.sales_person_id "+
                        "WHERE c.customer_id != 0 AND sc.type_id = 1 order by sc.event_start";
            }
            else if (ddlSalesRep.SelectedItem.Text == "ALL")
            {
                strQ = " SELECT c.customer_id, c.first_name1+' '+c.last_name1 as customer_name, " +
                        "sp.first_name+' '+sp.last_name as employee_name, " +
                        "sc.title, sc.description, sc.type_id, " +
                        "sc.section_name, sc.event_start, sc.event_end " +
                        "FROM customers c " +
                        "INNER JOIN schedule_calendar sc ON c.customer_id = sc.customer_id " +
                        "INNER JOIN sales_person sp ON c.sales_person_id = sp.sales_person_id " + strCondition + " order by sc.event_start";


            }
            else
            {
                strQ = " SELECT c.customer_id, c.first_name1+' '+c.last_name1 as customer_name, " +
                        "sp.first_name+' '+sp.last_name as employee_name, " +
                        "sc.title, sc.description, sc.type_id, " +
                        "sc.section_name, sc.event_start, sc.event_end " +
                        "FROM customers c " +
                        "INNER JOIN schedule_calendar sc ON c.customer_id = sc.customer_id " +
                        "INNER JOIN sales_person sp ON c.sales_person_id = sp.sales_person_id " + strCondition + " order by sc.event_start";

            }


            IEnumerable<csScheduleReport> list = _db.ExecuteQuery<csScheduleReport>(strQ, string.Empty);
            DataTable dtList = csCommonUtility.LINQToDataTable(list);


            if (dtList.Rows.Count == 0)
            {
                lblResult.Text = csCommonUtility.GetSystemErrorMessage("No Schedule found.");
                return;
            }

            ReportDocument rptFile = new ReportDocument();

            string strReportPath = Server.MapPath(@"Reports\rpt\rptScheduleReport.rpt");
            rptFile.Load(strReportPath);
            rptFile.SetDataSource(dtList);

            Hashtable ht = new Hashtable();
            ht.Add("p_SearchDate", strSearchDate); //date time
            ht.Add("p_SearchBy", strSearchBy); //date time

            Session.Add(SessionInfo.Report_File, rptFile);
            Session.Add(SessionInfo.Report_Param, ht);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Popup", "window.open('Reports/Common/ReportViewer.aspx');", true);
        }
        catch (Exception ex)
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage(ex.Message);
        }
    }



    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, txtStartDate.ID, txtStartDate.GetType().Name, "Change");
        lblResult.Text = "";
        chkAll.Checked = false;

    }
    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, txtEndDate.ID, txtEndDate.GetType().Name, "Change");
        lblResult.Text = "";
        chkAll.Checked = false;
    }
    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, chkAll.ID, chkAll.GetType().Name, "Change");
        if (chkAll.Checked)
        {
            lblResult.Text = "";
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            tblEndDate.Visible = false;
            txtSearch.Text = "";
            ddlSalesRep.SelectedItem.Text = "ALL";
            ddlSearchBy.SelectedValue = "2";
        }
    }

    protected void ddlSearchBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, ddlSearchBy.ID, ddlSearchBy.GetType().Name, "Change");
        txtSearch.Text = "";
        wtmFileNumber.WatermarkText = "Search by " + ddlSearchBy.SelectedItem.Text;
        if (ddlSearchBy.SelectedValue == "2")
        {
            txtSearch_AutoCompleteExtender.ServiceMethod = "GetLastName";
        }
        else
        {
            txtSearch_AutoCompleteExtender.ServiceMethod = "GetFirstName";
        }
    }

}
