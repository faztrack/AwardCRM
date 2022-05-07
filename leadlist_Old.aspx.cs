using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using DataStreams.Csv;
using System.Data.OleDb;
using System.IO;

public partial class leadlist_Old : System.Web.UI.Page
{
    [WebMethod]
    public static string[] GetJobNumber(String prefixText, Int32 count)
    {
        
            DataClassesDataContext _db = new DataClassesDataContext();
            return (from c in _db.customer_estimates
                    where c.status_id == 3 && c.job_number != null && c.job_number.StartsWith(prefixText)
                    select c.job_number).Take<String>(count).ToArray();
    }
    [WebMethod]
    public static string[] GetCompany(String prefixText, Int32 count)
    {
        if (HttpContext.Current.Session["lSearch"] != null)
        {
            List<customer> cList = (List<customer>)HttpContext.Current.Session["lSearch"];
            return (from c in cList
                    where c.company.ToLower().StartsWith(prefixText.ToLower())
                    select c.company).Take<String>(count).ToArray();
        }
        else
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            return (from c in _db.customers
                    where c.company.StartsWith(prefixText)
                    select c.company).Take<String>(count).ToArray();
        }
    }

    [WebMethod]
    public static string[] GetLastName(String prefixText, Int32 count)
    {
        if (HttpContext.Current.Session["lSearch"] != null)
        {
            List<customer> cList = (List<customer>)HttpContext.Current.Session["lSearch"];
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
        if (HttpContext.Current.Session["lSearch"] != null)
        {
            List<customer> cList = (List<customer>)HttpContext.Current.Session["lSearch"];
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

    [WebMethod]
    public static string[] GetAddress(String prefixText, Int32 count)
    {
        if (HttpContext.Current.Session["lSearch"] != null)
        {
            List<customer> cList = (List<customer>)HttpContext.Current.Session["lSearch"];
            return (from c in cList
                    where c.address.ToLower().StartsWith(prefixText.ToLower())
                    select c.address).Take<String>(count).ToArray();
        }
        else
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            return (from c in _db.customers
                    where c.address.StartsWith(prefixText)
                    select c.address).Take<String>(count).ToArray();
        }
    }

    [WebMethod]
    public static string[] GetEmail(String prefixText, Int32 count)
    {
        if (HttpContext.Current.Session["lSearch"] != null)
        {
            List<customer> cList = (List<customer>)HttpContext.Current.Session["lSearch"];
            return (from c in cList
                    where c.email.ToLower().StartsWith(prefixText.ToLower())
                    select c.email).Take<String>(count).ToArray();
        }
        else
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            return (from c in _db.customers
                    where c.email.StartsWith(prefixText)
                    select c.email).Take<String>(count).ToArray();
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
            else
            {
                userinfo oUser = (userinfo)Session["oUser"];
                

            }

            if (Page.User.IsInRole("lead001") == false)
            {
                // No Permission Page.
                Response.Redirect("nopermission.aspx");
            }
            Session.Remove("CustomerId");
            if (Session["LeadId"] != null)
            {
                int nCustomerId = Convert.ToInt32(Session["LeadId"]);
                hdnLeadId.Value = nCustomerId.ToString();
            }
            if (Session["LeadSreach"] != null)
            {
                string strSearch = Session["LeadSreach"].ToString();
                txtSearch.Text = strSearch;
               

            }
            Session.Remove("LeadSreach");
            Session.Remove("CustSreach");
            

            // Get Leads
            # region Get Leads

            DataClassesDataContext _db = new DataClassesDataContext();
            List<customer> LeadList = _db.customers.ToList();
            Session.Add("lSearch", LeadList);

            # endregion
            BindSalesPerson();
            BindSuperintendent();
            BindLeadSource();
            BindLeadStatus();
            ddlStatus.SelectedValue = "2";
            GetCustomersNew(0);
        }        
    }

    private void BindSalesPerson()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        string strQ = "select first_name+' '+last_name AS sales_person_name,sales_person_id from sales_person WHERE is_active = 1 AND sales_person.client_id =" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + " order by first_name asc";
        List<userinfo> mList = _db.ExecuteQuery<userinfo>(strQ, string.Empty).ToList();
        ddlSalesRep.DataSource = mList;
        ddlSalesRep.DataTextField = "sales_person_name";
        ddlSalesRep.DataValueField = "sales_person_id";
        ddlSalesRep.DataBind();
        ddlSalesRep.Items.Insert(0, "All");

      

    }

    private void BindLeadStatus()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        var LeadStatus = from st in _db.lead_status
                         orderby st.lead_status_id
                         select st;
        ddlStatus.DataSource = LeadStatus;
        ddlStatus.DataTextField = "lead_status_name";
        ddlStatus.DataValueField = "lead_status_id";
        ddlStatus.DataBind();

      
    }

    private void BindLeadSource()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        var item = from l in _db.lead_sources
                   where l.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && l.is_active == Convert.ToBoolean(1)
                   orderby l.lead_name
                   select l;

        ddlLeadSource.DataSource = item;
        ddlLeadSource.DataTextField = "lead_name";
        ddlLeadSource.DataValueField = "lead_source_id";
        ddlLeadSource.DataBind();
        ddlLeadSource.Items.Insert(0, "All");
        ddlLeadSource.SelectedIndex = 0;

       
    }

    private void BindSuperintendent()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        string strQ = "select first_name+' '+last_name AS Superintendent_name,user_id from user_info WHERE role_id = 4";
        List<userinfo> mList = _db.ExecuteQuery<userinfo>(strQ, string.Empty).ToList();
        ddlSuperintendent.DataSource = mList;
        ddlSuperintendent.DataTextField = "Superintendent_name";
        ddlSuperintendent.DataValueField = "user_id";
        ddlSuperintendent.DataBind();
        ddlSuperintendent.Items.Insert(0, "All");
    }
    protected void GetCustomersNew(int nPageNo)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        userinfo obj = (userinfo)Session["oUser"];
        int nSalePersonId = obj.sales_person_id;
        int nRoleId = obj.role_id;
        if (nRoleId == 4)
        {
            //ddlSuperintendent.SelectedValue = obj.user_id.ToString();
            //ddlSuperintendent.Enabled = false;
        }
        else if(nRoleId == 3)
        {
            ddlSalesRep.SelectedValue = nSalePersonId.ToString();
            ddlSalesRep.Enabled = false;
        }
        grdLeadList.PageIndex = nPageNo;

     
        string strCondition = "";

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
            else if (ddlSearchBy.SelectedValue == "3")
            {

                strCondition = "  customers.email LIKE '%" + str + "%'";
            }
            else if (ddlSearchBy.SelectedValue == "4")
            {
                strCondition = "  customers.address LIKE '%" + str + "%'";
            }
            else if (ddlSearchBy.SelectedValue == "6")
            {
                strCondition = "  customers.company LIKE '%" + str + "%'";
            }
        }
        else
        {

            if (Convert.ToInt32(ddlStatus.SelectedValue) != 1)
            {
                if (Convert.ToInt32(ddlStatus.SelectedValue) == 2)
                {
                    if (strCondition.Length > 0)
                    {
                        strCondition += " AND customers.status_id NOT IN(4,5) ";
                    }
                    else
                    {
                        strCondition += " customers.status_id NOT IN(4,5) ";
                    }

                }
                else if (Convert.ToInt32(ddlStatus.SelectedValue) > 7)
                {
                    if (strCondition.Length > 0)
                    {
                        strCondition += " AND customers.lead_status_id = " + Convert.ToInt32(ddlStatus.SelectedValue);
                    }
                    else
                    {
                        strCondition += " customers.lead_status_id = " + Convert.ToInt32(ddlStatus.SelectedValue);
                    }

                }
                else if (Convert.ToInt32(ddlStatus.SelectedValue) == 5)
                {
                    if (strCondition.Length > 0)
                    {
                        strCondition += " AND customers.status_id = 5 ";
                    }
                    else
                    {
                        strCondition += " customers.status_id = 5 ";
                    }

                }

            }


            if (ddlSalesRep.SelectedItem.Text != "All")
            {
                if (strCondition.Length > 0)
                {
                    strCondition += " AND customers.sales_person_id =" + Convert.ToInt32(ddlSalesRep.SelectedValue);
                }
                else
                {
                    strCondition += " customers.sales_person_id =" + Convert.ToInt32(ddlSalesRep.SelectedValue);
                }

            }
            if (ddlLeadSource.SelectedItem.Text != "All")
            {
                if (strCondition.Length > 0)
                {
                    strCondition += " AND customers.lead_source_id =" + Convert.ToInt32(ddlLeadSource.SelectedValue);
                }
                else
                {
                    strCondition += " customers.lead_source_id =" + Convert.ToInt32(ddlLeadSource.SelectedValue);
                }

            }
            if (ddlSuperintendent.SelectedItem.Text != "All")
            {
                if (strCondition.Length > 0)
                {
                    strCondition += " AND  customers.SuperintendentId = " + Convert.ToInt32(ddlSuperintendent.SelectedValue);
                }
                else
                {
                    strCondition += "  customers.SuperintendentId = " + Convert.ToInt32(ddlSuperintendent.SelectedValue);
                }

            }
        }
        if (strCondition.Length > 0)
        {
            strCondition = "Where " + strCondition;
        }
        //if (strCondition.Length > 0)
        //{
        //    strCondition = "Where customers.islead = 1 and " + strCondition;
        //}
        //else
        //{
        //    strCondition = "Where customers.islead = 1";
        //}
        string strQ = string.Empty;
        if (Convert.ToInt32(hdnLeadId.Value) > 0)
        {
            int nCustomerId = Convert.ToInt32(hdnLeadId.Value);
            strQ = " SELECT client_id,customers.islead,customers.isCustomer, customers.customer_id, first_name1+' '+ last_name1 AS customer_name, first_name1, last_name1, first_name2, last_name2, address, cross_street, city, state, zip_code, fax, email, phone, is_active, registration_date, " +
                          " sales_person_id, update_date, status_id, notes, appointment_date, lead_source_id, status_note, company, email2, SuperintendentId,Convert(VARCHAR,t1.SaleDate,101) AS Sold_date, company,CAST(1 AS BIT) AS IsEstimateActive " +
                          " FROM customers " +
                          " LEFT OUTER JOIN (SELECT  customer_estimate.customer_id,MIN(CONVERT(DATETIME,sale_date)) AS SaleDate FROM customer_estimate WHERE customer_estimate.status_id=3 AND customer_estimate.customer_id = " + nCustomerId + " GROUP BY customer_estimate.customer_id) AS t1 ON t1.customer_id = customers.customer_id " +
                          " WHERE customers.islead = 1 and customers.customer_id =" + nCustomerId + " order by t1.SaleDate asc";
            hdnLeadId.Value = "0";
        }
        else
        {
            if (nRoleId == 4)
            {
                if (ddlSearchBy.SelectedValue == "5")
                {
                    strQ = " SELECT client_id,customers.status_id,customers.islead,customers.isCustomer, customers.customer_id, first_name1+' '+ last_name1 AS customer_name, first_name1, last_name1, first_name2, last_name2, address, cross_street, city, state, zip_code, fax, email, phone, is_active, registration_date, " +
                                   " sales_person_id, update_date, status_id, notes, appointment_date, lead_source_id, status_note, company, email2, SuperintendentId,Convert(VARCHAR,t1.SaleDate,101) AS Sold_date,company,CAST(1 AS BIT) AS IsEstimateActive " +
                                   " FROM customers " +
                                   " INNER JOIN (SELECT  customer_estimate.customer_id,MIN(CONVERT(DATETIME,sale_date)) AS SaleDate FROM customer_estimate WHERE customers.islead = 1 and customer_estimate.status_id = 3 AND job_number LIKE '%" + txtSearch.Text + "%' GROUP BY customer_estimate.customer_id) AS t1 ON t1.customer_id = customers.customer_id " +
                                   "  order by t1.SaleDate asc";
                }
                else
                {
                    strQ = " SELECT client_id,customers.status_id,customers.islead,customers.isCustomer, customers.customer_id, first_name1+' '+ last_name1 AS customer_name, first_name1, last_name1, first_name2, last_name2, address, cross_street, city, state, zip_code, fax, email, phone, is_active, registration_date, " +
                                  " sales_person_id, update_date, status_id, notes, appointment_date, lead_source_id, status_note, company, email2, SuperintendentId,Convert(VARCHAR,t1.SaleDate,101) AS Sold_date,company,CAST(1 AS BIT) AS IsEstimateActive " +
                                  " FROM customers " +
                                  " LEFT OUTER JOIN  (SELECT  customer_estimate.customer_id,MIN(CONVERT(DATETIME,sale_date)) AS SaleDate FROM customer_estimate WHERE customer_estimate.status_id=3 GROUP BY customer_estimate.customer_id) AS t1 ON t1.customer_id = customers.customer_id " +
                                  " " + strCondition + " order by t1.SaleDate asc";
 
                }
            }
            else
            {
                if (ddlSearchBy.SelectedValue == "5")
                {
                    strQ = " SELECT client_id,customers.status_id, customers.islead,customers.isCustomer,customers.customer_id, first_name1+' '+ last_name1 AS customer_name, first_name1, last_name1, first_name2, last_name2, address, cross_street, city, state, zip_code, fax, email, phone, is_active, registration_date, " +
                                  " sales_person_id, update_date, status_id, notes, appointment_date, lead_source_id, status_note, company, email2, SuperintendentId,Convert(VARCHAR,t1.SaleDate,101) AS Sold_date, company,CAST(1 AS BIT) AS IsEstimateActive " +
                                  " FROM customers " +
                                  " INNER JOIN  (SELECT  customer_estimate.customer_id,MIN(CONVERT(DATETIME,sale_date)) AS SaleDate FROM customer_estimate WHERE customers.islead = 1 and customer_estimate.status_id=3 AND job_number LIKE '%" + txtSearch.Text + "%' GROUP BY customer_estimate.customer_id) AS t1 ON t1.customer_id = customers.customer_id " +
                                  "  order by customers.registration_date desc, customers.last_name1 asc";
                }
                else
                {
                    strQ = " SELECT client_id,customers.status_id,customers.islead,customers.isCustomer, customers.customer_id, first_name1+' '+ last_name1 AS customer_name, first_name1, last_name1, first_name2, last_name2, address, cross_street, city, state, zip_code, fax, email, phone, is_active, registration_date, " +
                                  " sales_person_id, update_date, status_id, notes, appointment_date, lead_source_id, status_note, company, email2, SuperintendentId,Convert(VARCHAR,t1.SaleDate,101) AS Sold_date,company, CAST(1 AS BIT) AS IsEstimateActive " +
                                  " FROM customers " +
                                  " LEFT OUTER JOIN   (SELECT  customer_estimate.customer_id,MIN(CONVERT(DATETIME,sale_date)) AS SaleDate FROM customer_estimate WHERE customer_estimate.status_id=3 GROUP BY customer_estimate.customer_id) AS t1 ON t1.customer_id = customers.customer_id " +
                                  " " + strCondition + " order by customers.registration_date desc, customers.last_name1 asc";
 
                }
            }
        }

        IEnumerable<csCustomer> mList = _db.ExecuteQuery<csCustomer>(strQ, string.Empty).ToList();
        DataTable dt = csCommonUtility.LINQToDataTable(mList);
        if (dt.Rows.Count > 0)
        {
            DataView dv = dt.DefaultView;
            dv.RowFilter = "islead = 1";
            if (dv.Count > 0)
            {
                Session.Add("sCustList", dv.ToTable());
                if (ddlItemPerPage.SelectedValue != "4")
                {
                    grdLeadList.PageSize = Convert.ToInt32(ddlItemPerPage.SelectedValue);
                }
                else
                {
                    grdLeadList.PageSize = 200;
                }
                grdLeadList.DataSource = dv.ToTable();
                grdLeadList.DataBind();
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

                if (grdLeadList.PageCount == nPageNo + 1)
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
            else
            {
                if (txtSearch.Text.Length > 0)
                {
                    dv.RowFilter = "isCustomer = 1";
                    if (dv.Count > 0)
                    {
                        Session.Add("CustSreach", txtSearch.Text);
                        Response.Redirect("customerlist.aspx");
                    }
                }
                else
                {
                    if (ddlItemPerPage.SelectedValue != "4")
                    {
                        grdLeadList.PageSize = Convert.ToInt32(ddlItemPerPage.SelectedValue);
                    }
                    else
                    {
                        grdLeadList.PageSize = 200;
                    }
                    grdLeadList.DataSource = null;
                    grdLeadList.DataBind();
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

                    if (grdLeadList.PageCount == nPageNo + 1)
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
        }
        else
        {
            if (ddlItemPerPage.SelectedValue != "4")
            {
                grdLeadList.PageSize = Convert.ToInt32(ddlItemPerPage.SelectedValue);
            }
            else
            {
                grdLeadList.PageSize = 200;
            }
            grdLeadList.DataSource = mList;
            grdLeadList.DataBind();
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

            if (grdLeadList.PageCount == nPageNo + 1)
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

    
   
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnAddNew.ID, btnAddNew.GetType().Name, "Click"); 
        Session.Remove("LeadId");
        Response.Redirect("lead_details.aspx");
    }
    protected void grdLeadList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int ncid = Convert.ToInt32(grdLeadList.DataKeys[e.Row.RowIndex].Value.ToString());

            DataClassesDataContext _db = new DataClassesDataContext();

            int nid = Convert.ToInt32(e.Row.Cells[2].Text);
            string strLatestActivity = "";
            string strCallQ = "SELECT Description as CallActivity FROM CustomerCallLog WHERE customer_id = " + ncid + " and " +
             " CallLogID=(SELECT max(CallLogID) FROM CustomerCallLog where  customer_id =  " + ncid + "  ) ";
            IEnumerable<customer_CallNotes> Calllist = _db.ExecuteQuery<customer_CallNotes>(strCallQ, string.Empty);
            foreach (customer_CallNotes cus_Notet in Calllist)
            {
                strLatestActivity = cus_Notet.CallActivity;
            }
            Label lblActivity = (Label)e.Row.FindControl("lblActivity");
            Label lblActivity_r = (Label)e.Row.FindControl("lblActivity_r");
            LinkButton lnkOpen = (LinkButton)e.Row.FindControl("lnkOpen");
            lblActivity.Text = strLatestActivity;

            if (strLatestActivity != "" && strLatestActivity.Length > 90)
            {
                lblActivity.Text = strLatestActivity.Substring(0, 90) + " ...";
                lblActivity.ToolTip = strLatestActivity;
                lblActivity_r.Text = strLatestActivity;
                lnkOpen.Visible = true;
            }
            else
            {
                lblActivity.Text = strLatestActivity;
                lblActivity_r.Text = strLatestActivity;
                lnkOpen.Visible = false;
            }
            

            //int nSuperintendentId = Convert.ToInt32(e.Row.Cells[8].Text);
            //if (nSuperintendentId > 0)
            //{
            //    user_info uinfo = _db.user_infos.Single(u => u.user_id == nSuperintendentId);
            //    e.Row.Cells[8].Text = uinfo.first_name + " " + uinfo.last_name;
            //}
            //else
            //{
            //    e.Row.Cells[8].Text = "";
            //}

         

            // Customer Address
            customer cust = new customer();
            cust = _db.customers.Single(c => c.customer_id == ncid);
            string strAddress = cust.address + " </br>" + cust.city + ", " + cust.state + " " + cust.zip_code;
            //e.Row.Cells[2].Text = strAddress;

            sales_person sp = new sales_person();
            sp = _db.sales_persons.Single(s => s.sales_person_id == nid);
            e.Row.Cells[2].Text = sp.first_name + " " + sp.last_name + "<br/>" + Convert.ToDateTime(cust.registration_date).ToShortDateString();

            Label lblPhone = (Label)e.Row.FindControl("lblPhone");
            lblPhone.Text = cust.phone;
            lblPhone.Attributes.CssStyle.Add("padding", "5px 0 5px 0");

            // Customer Email
            HyperLink hypEmail = (HyperLink)e.Row.FindControl("hypEmail");
            hypEmail.Text = cust.email;
            //hypEmail.NavigateUrl = "mailto:" + cust.email + "?subject=Contact";
            hypEmail.NavigateUrl = "sendemailoutlook.aspx?custId=" + ncid;
            //hypEmail.Target = "MyWindow";

            // Customer Address in Google Map
            HyperLink hypAddress = (HyperLink)e.Row.FindControl("hypAddress");
            hypAddress.Text = strAddress;
            //hypAddress.NavigateUrl = "GoogleMap.aspx?strAdd=" + strAddress.Replace("</br>", "");
            string address = cust.address + ",+" + cust.city + ",+" + cust.state + ",+" + cust.zip_code;
            hypAddress.NavigateUrl = "http://maps.google.com/maps?f=q&source=s_q&hl=en&geocode=&q=" + address;
            hypAddress.ToolTip = "Google Map";


            // Customer Messsage Center
            HyperLink hypMessage = (HyperLink)e.Row.FindControl("hypMessage");
            hypMessage.NavigateUrl = "customermessagecenteroutlook.aspx?cid=" + ncid;
           

            HyperLink hyp_ProjectNotes = (HyperLink)e.Row.FindControl("hyp_ProjectNotes");
            hyp_ProjectNotes.NavigateUrl = "ProjectNotes.aspx?cid=" + ncid + "&TypeId=3";

            HyperLink hyp_CallLog = (HyperLink)e.Row.FindControl("hyp_CallLog");

            hyp_CallLog.NavigateUrl = "CallLogInfo.aspx?cid=" + ncid + "&TypeId=3";

            HyperLink hyp_DocumentManagement = (HyperLink)e.Row.FindControl("hyp_DocumentManagement");
            hyp_DocumentManagement.NavigateUrl = "DocumentManagement.aspx?cid=" + ncid;

            // Customer Estimates
          
            Table tdLink = (Table)e.Row.FindControl("tdLink");
            

            string strQ = "select * from customer_estimate where customer_id=" + ncid + " and client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
            IEnumerable<customer_estimate_model> list = _db.ExecuteQuery<customer_estimate_model>(strQ, string.Empty);
            var resultCount = (from ce in _db.customer_estimates
                               where ce.customer_id == ncid && ce.client_id == 1 && ce.status_id == 3
                               select ce.estimate_id);
            int nEstCount = resultCount.Count();

            foreach (customer_estimate_model cus_est in list)
            {
                string strJobNum = cus_est.job_number;
                string strestimateName = cus_est.estimate_name;
                int nestid = Convert.ToInt32(cus_est.estimate_id);
                int nest_status_id = Convert.ToInt32(cus_est.status_id);



                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                HyperLink hyp = new HyperLink();
                //HyperLink hypCostLoc = new HyperLink();
                //HyperLink hypCostSec = new HyperLink();
                HyperLink hypAllow = new HyperLink();
                HyperLink hypSelection = new HyperLink();

                cell.BorderWidth = 0;
               

                    hyp.Text = strestimateName;
               
                
                //hypCostLoc.Text = " [C by Loc]";
                //hypCostLoc.NavigateUrl = "CostPerEstimateReport.aspx?TypeId=1&eid=" + nestid + "&cid=" + ncid;
                //hypCostLoc.Target = "_blank";
                //hypCostSec.Text = " [C by Sec]";
                //hypCostSec.NavigateUrl = "CostPerEstimateReport.aspx?TypeId=2&eid=" + nestid + "&cid=" + ncid;
                //hypCostSec.Target = "_blank";
                hyp.NavigateUrl = "customer_locations.aspx?eid=" + nestid + "&cid=" + ncid;
                hypAllow.Text = " [Allow]";
                hypAllow.NavigateUrl = "AllowanceReport.aspx?eid=" + nestid + "&cid=" + ncid;
                hypSelection.Text = " [Selection]";
                hypSelection.NavigateUrl = "SelectionSheetNew.aspx?eid=" + nestid + "&cid=" + ncid;

                cell.Controls.Add(hyp);
                //cell.Controls.Add(hypCostLoc);
                //cell.Controls.Add(hypCostSec);
                cell.Controls.Add(hypAllow);
                cell.Controls.Add(hypSelection);
                cell.HorizontalAlign = HorizontalAlign.Left;
                row.Cells.Add(cell);
                tdLink.Rows.Add(row);
            }
            TableRow commonrow = new TableRow();
            TableCell commoncell = new TableCell();
            HyperLink hypCommon = new HyperLink();
            hypCommon.Text = "New Estimate";
            commoncell.HorizontalAlign = HorizontalAlign.Left;
            hypCommon.NavigateUrl = "customer_locations.aspx?cid=" + ncid;
            commoncell.Controls.Add(hypCommon);
            commonrow.Cells.Add(commoncell);
            tdLink.Rows.Add(commonrow);
            commoncell.BorderWidth = 0;

            //if (cust.status_id == 1)
            //{
            //    e.Row.Attributes.CssStyle.Add("background-color", "#ebffff");
            //    //e.Row.Attributes.CssStyle.Add("font-weight", "bold");
            //}
            //else if (cust.status_id == 2)
            //{
            //    e.Row.Attributes.CssStyle.Add("background-color", "#fff4e7");
            //}
            //else if (cust.status_id == 3)
            //{
            //    e.Row.Attributes.CssStyle.Add("background-color", "#f3fbea");
 
            //}
        }
    }
    protected void GotoPricing(object sender, EventArgs e)
    {
        LinkButton lnk = (LinkButton)sender;
        int ncid = Convert.ToInt32(lnk.CommandArgument);
        
        Response.Redirect("customer_locations.aspx?cid=" + ncid);
    }
    protected void grdLeadList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GetCustomersNew(e.NewPageIndex);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSearch.ID, btnSearch.GetType().Name, "Click"); 
        Session.Remove("LeadId");
        GetCustomersNew(0);
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnNext.ID, btnNext.GetType().Name, "Click"); 
        int nCurrentPage = 0;
        nCurrentPage = Convert.ToInt32(lblCurrentPageNo.Text);
        GetCustomersNew(nCurrentPage);
    }
    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnPrevious.ID, btnPrevious.GetType().Name, "Click"); 
        int nCurrentPage = 0;
        nCurrentPage = Convert.ToInt32(lblCurrentPageNo.Text);
        GetCustomersNew(nCurrentPage - 2);
    }
    protected void ddlItemPerPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session.Remove("LeadId");
        GetCustomersNew(0);
    }
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session.Remove("LeadId");
        txtSearch.Text = string.Empty;
        GetCustomersNew(0);
    }
    protected void lnkViewAll_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, LinkButton1.ID, LinkButton1.GetType().Name, "Click"); 
        txtSearch.Text = "";
        ddlStatus.SelectedValue = "2";
       // ddlStatus.SelectedIndex = -1;
        ddlSalesRep.SelectedIndex = -1;
        ddlLeadSource.SelectedIndex = -1;
        ddlSuperintendent.SelectedIndex = -1;
        Session.Remove("LeadId");
        GetCustomersNew(0);

    }
    protected void btnExpCustList_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnExpCustList.ID, btnExpCustList.GetType().Name, "Click"); 
        //DataClassesDataContext _db = new DataClassesDataContext();
        //var custList = from cus in _db.customers
        //               where cus.client_id == 1 && cus.islead == 1 
        //               orderby cus.registration_date descending, cus.last_name1 ascending
        //               select new
        //               {
        //                   FirstName = cus.first_name1,
        //                   LastName = cus.last_name1,
        //                   Phone = cus.phone,
        //                   Email = cus.email,
        //                   Address = cus.address + ' ' + cus.city + ',' + ' ' + cus.state + ' ' + cus.zip_code
        //               };

        //DataTable tblCustList = SessionInfo.LINQToDataTable(custList);
        DataTable tmpTable = LoadTmpDataTable();
        DataClassesDataContext _db = new DataClassesDataContext();

        DataTable tblCustList = (DataTable)Session["sCustList"];
        foreach (DataRow dr in tblCustList.Rows)
        {
            int nLeadSourceId = Convert.ToInt32(dr["lead_source_id"]);
            lead_source ls = new lead_source();
            ls = _db.lead_sources.Single(s => s.lead_source_id == nLeadSourceId);
            DataRow drNew = tmpTable.NewRow();
           // drNew["Company"] = dr["company"];
            drNew["First Name"] = dr["first_name1"];
            drNew["Last Name"] = dr["last_name1"];
            drNew["Phone"] = dr["phone"];
            drNew["Email"] = dr["email"];
            drNew["Address"] = dr["address"].ToString() + ' ' + dr["city"] + ',' + ' ' +  dr["state"] + ' ' +  dr["zip_code"];
            drNew["Lead Source"] = ls.lead_name.ToString();
            tmpTable.Rows.Add(drNew);
        }

        Response.Clear();
        Response.ClearHeaders();

        using (CsvWriter writer = new CsvWriter(Response.OutputStream, ',', System.Text.Encoding.Default))
        {
            writer.WriteAll(tmpTable, true);
        }
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment; filename=CustomerList.csv");
        Response.End();
    }
    private DataTable LoadTmpDataTable()
    {
        DataTable table = new DataTable();

       // table.Columns.Add("Company", typeof(string));
        table.Columns.Add("First Name", typeof(string));
        table.Columns.Add("Last Name", typeof(string));
        table.Columns.Add("Phone", typeof(string));
        table.Columns.Add("Email", typeof(string));
        table.Columns.Add("Address", typeof(string));
        table.Columns.Add("Lead Source", typeof(string));
        return table;
    }
    protected void ddlSalesRep_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session.Remove("LeadId");
        txtSearch.Text = string.Empty;
        GetCustomersNew(0);
    }
    protected void ddlSuperintendent_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session.Remove("LeadId");
        txtSearch.Text = string.Empty;
        GetCustomersNew(0);
    }
    protected void ddlLeadSource_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session.Remove("LeadId");
        txtSearch.Text = string.Empty;
        GetCustomersNew(0);
    }
    protected void ddlSearchBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        wtmFileNumber.WatermarkText = "Search by " + ddlSearchBy.SelectedItem.Text;
         if (ddlSearchBy.SelectedValue == "2")
        {
            txtSearch_AutoCompleteExtender.ServiceMethod = "GetLastName";
        }
         else if (ddlSearchBy.SelectedValue == "5")
         {
             txtSearch_AutoCompleteExtender.ServiceMethod = "GetJobNumber";
         }
        else if (ddlSearchBy.SelectedValue == "1")
        {
            txtSearch_AutoCompleteExtender.ServiceMethod = "GetFirstName";
        }
        else if (ddlSearchBy.SelectedValue == "4")
        {
            txtSearch_AutoCompleteExtender.ServiceMethod = "GetAddress";
        }
        else if (ddlSearchBy.SelectedValue == "3")
        {
            txtSearch_AutoCompleteExtender.ServiceMethod = "GetEmail";
        }
         else if (ddlSearchBy.SelectedValue == "6")
        {
            txtSearch_AutoCompleteExtender.ServiceMethod = "GetCompany";
        }
        
        GetCustomersNew(0);
    }
    protected void btnSalesCalendar_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("schedulecalendar.aspx?TypeID=2");
    }
    protected void btnLeadReport_Click(object sender, EventArgs e)
    {
        Response.Redirect("lead_report.aspx");
    }
    protected void lnkOpen_Click(object sender, EventArgs e)
    {
        LinkButton btnsubmit = sender as LinkButton;

        GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
        Label lblActivity = gRow.Cells[4].Controls[0].FindControl("lblActivity") as Label;
        Label lblActivity_r = gRow.Cells[4].Controls[1].FindControl("lblActivity_r") as Label;
        LinkButton lnkOpen = gRow.Cells[4].Controls[2].FindControl("lnkOpen") as LinkButton;

        if (lnkOpen.Text == "More")
        {
            lblActivity.Visible = false;
            lblActivity_r.Visible = true;
            lnkOpen.Text = " Less";
            lnkOpen.ToolTip = "Click here to view less";
        }
        else
        {
            lblActivity.Visible = true;
            lblActivity_r.Visible = false;
            lnkOpen.Text = "More";
            lnkOpen.ToolTip = "Click here to view more";
        }
    }

   
}
