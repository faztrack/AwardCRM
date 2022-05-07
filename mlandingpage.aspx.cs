using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mlandingpage : System.Web.UI.Page
{
    [WebMethod]
    public static string[] GetCustomerName(String prefixText, Int32 count)
    {

      
            List<csCustomer> cList = (List<csCustomer>)HttpContext.Current.Session["eSearch"];
            return (from c in cList
                    where  c.customer_name.ToLower().Contains(prefixText.ToLower())
                    select c.customer_name).Take<String>(count).ToArray();
       
        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);
            if (Session["oUser"] == null && Session["oCrew"] == null)
            {
                Response.Redirect("mobile.aspx");
            }
       
           if (Session["oCrew"] != null)
            {
               hyp_Leads.Visible = false;
               pnlTimeClock.Visible = true;
            }
            if (Session["oUser"] != null)
            {
                hyp_Leads.Visible = true;
                userinfo obj = (userinfo)Session["oUser"];
                if(obj.IsTimeClock==true)
                   pnlTimeClock.Visible = true;
                else
                    pnlTimeClock.Visible = false;
                if (obj.role_id == 1 || obj.role_id == 4)
                    hypVendorList.Visible = true;
                else
                    hypVendorList.Visible = false;
            }
            BindCustomer();
            GetCustomersNew(0);
            GetSearchCustomer(0);
          }
    }

    private void BindCustomer()
    {

        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            string StrCondition = string.Empty;
            if (Session["oUser"] != null)
            {
                userinfo obj = (userinfo)Session["oUser"];
                if (obj.role_id == 1)
                {
                    StrCondition = " where  status_id NOT IN(4,5) ";
                }
                else if (obj.role_id == 4)
                {
                    StrCondition = " where status_id NOT IN(4,5) AND SuperintendentId="+obj.user_id;
                }
                else
                {
                    StrCondition = " where status_id NOT IN(4,5) AND sales_person_id=" + obj.sales_person_id;
                }

                string strQ = "select last_name1+', '+first_name1 AS customer_name,customer_id,last_name1 from customers  " + StrCondition + " order by customer_name ";
                    List<csCustomer> mList = _db.ExecuteQuery<csCustomer>(strQ, string.Empty).ToList();
                    Session.Add("eSearch", mList);
                    ddCustomer.DataSource = mList;
                    ddCustomer.DataTextField = "customer_name";
                    ddCustomer.DataValueField = "customer_id";
                    ddCustomer.DataBind();
                    ddCustomer.Items.Insert(0, "Select Customer");
                  
            }
            if (Session["oCrew"] != null)
            {
                Crew_Detail objC = (Crew_Detail)Session["oCrew"];
                string CrewName =objC.first_name.Trim() +" "+objC.last_name.Trim();
                StrCondition = " where customers.IsCustomer=1 AND customers.status_id NOT IN(4,5) AND ScheduleCalendar.IsEstimateActive = 1 AND ScheduleCalendar.employee_name  LIKE '%" + CrewName + "%' ";
                string strQ = " select distinct last_name1+', '+first_name1 AS customer_name,customers.customer_id,last_name1 from customers " +
                              " Right join ScheduleCalendar on  customers.customer_id=ScheduleCalendar.customer_id " +
                               " " + StrCondition + " ";
                              
                List<csCustomer> mList = _db.ExecuteQuery<csCustomer>(strQ, string.Empty).ToList();
                Session.Add("eSearch", mList);
                ddCustomer.DataSource = mList;
                ddCustomer.DataTextField = "customer_name";
                ddCustomer.DataValueField = "customer_id";
                ddCustomer.DataBind();
                ddCustomer.Items.Insert(0, "Select Customer");
           
            }

           
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void GetSearchCustomer(int nPageNo)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
       grdCustomerList.PageIndex = nPageNo;
        string strQ = string.Empty;
        // Initial Load Data

        if (Session["oUser"] != null)
        {
            userinfo obj = (userinfo)Session["oUser"];

            strQ = " SELECT searchCustomerId,client_id, customer_id, first_name1, last_name1, first_name2, last_name2, address, cross_street, city, state, zip_code, fax, email, phone, is_active, registration_date, " +
                                 " sales_person_id, update_date, status_id, notes, appointment_date, lead_source_id, status_note, company, email2, SuperintendentId " +
                                 " FROM searchcustomers " +
                                 " WHERE userId =" + obj.user_id + " AND IsCrew=0 order by searchDate DESC";
        }
        if (Session["oCrew"] != null)
        {
            Crew_Detail objC = (Crew_Detail)Session["oCrew"];
            strQ = " SELECT searchCustomerId,client_id, customer_id, first_name1, last_name1, first_name2, last_name2, address, cross_street, city, state, zip_code, fax, email, phone, is_active, registration_date, " +
                                 " sales_person_id, update_date, status_id, notes, appointment_date, lead_source_id, status_note, company, email2, SuperintendentId " +
                                 " FROM searchcustomers " +
                                 " WHERE userId =" + objC.crew_id + " AND IsCrew=1 order by searchDate DESC";
        }

        IEnumerable<searchcustomer> mList = _db.ExecuteQuery<searchcustomer>(strQ, string.Empty).ToList();
        if (mList.Count() > 0)
        {
            lblInitial.Visible = false;
            pnlSearchCustomer.Visible = true;
            grdCustomerList.DataSource = mList;
            grdCustomerList.DataKeyNames = new string[] { "customer_id", "searchCustomerId" };
            grdCustomerList.DataBind();
        }
        else
        {
            lblInitial.Visible = true;
            pnlSearchCustomer.Visible = false;
        }

        hdnCurrentPageNo.Value = Convert.ToString(nPageNo + 1);
        if (nPageNo == 0)
        {
            btnPrevious.Enabled = false;

        }
        else
        {
            btnPrevious.Enabled = true;

        }

        if (grdCustomerList.PageCount == nPageNo + 1)
        {
            btnNext.Enabled = false;

        }
        else
        {
            btnNext.Enabled = true;

        }
    }
  protected void GetCustomersNew(int nPageNo)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        grdIcon.PageIndex = nPageNo;
        string strQ = string.Empty;
        if (Session["oUser"] != null)
            {
                userinfo obj = (userinfo)Session["oUser"];
                strQ = " SELECT top 1 searchCustomerId,client_id, customer_id, first_name1, last_name1, first_name2, last_name2, address, cross_street, city, state, zip_code, fax, email, phone, is_active, registration_date,mobile, " +
                            " sales_person_id, update_date, status_id, notes, appointment_date, lead_source_id, status_note, company, email2, SuperintendentId " +
                            " FROM searchcustomers " +
                            " WHERE userId =" + obj.user_id + " AND IsCrew=0 order by searchDate DESC";
            }

            if (Session["oCrew"] != null)
            {
                Crew_Detail objC = (Crew_Detail)Session["oCrew"];
                strQ = " SELECT top 1 searchCustomerId,client_id, customer_id, first_name1, last_name1, first_name2, last_name2, address, cross_street, city, state, zip_code, fax, email, phone, is_active, registration_date,mobile, " +
                           " sales_person_id, update_date, status_id, notes, appointment_date, lead_source_id, status_note, company, email2, SuperintendentId " +
                           " FROM searchcustomers " +
                           " WHERE userId =" + objC.crew_id + " AND IsCrew=1 order by searchDate DESC";
            }
            IEnumerable<searchcustomer> mList = _db.ExecuteQuery<searchcustomer>(strQ, string.Empty).ToList();
        if (mList.Count() > 0)
            {
                foreach (var li in mList)
                {

                    if (li.notes.Length > 70)
                    {
                        pnlLeadNotes.Visible = true;
                        lblCustomerLeadNotes.Text = li.notes.Substring(0, 70) + "...";
                    }
                    else
                    {
                        if (li.notes != "" && li.notes != null)
                        {
                            lblCustomerLeadNotes.Text = li.notes;
                            inkLeadnoteView.Visible = false;
                        }
                        else
                            pnlLeadNotes.Visible = false;
                        
                    }
                    hdnCustomerId.Value = li.customer_id.ToString();
                    lblSearchCustomerName.Text = li.first_name1 + " " + li.last_name1;

                    if (li.mobile != "" && li.mobile!=null)
                    {
                        lblCustomerMobile.Visible = true;
                        lblCustomerPhone.Text = li.phone + ", ";
                        hrfCustomerPhone.HRef = "tel:" + li.phone;
                        lblCustomerMobile.Text = li.mobile + " m";
                        hrfCustomerMobile.HRef="tel:"+li.mobile;
                    }
                    else
                    {
                        lblCustomerMobile.Visible = false;
                        lblCustomerPhone.Text = li.phone;
                        hrfCustomerPhone.HRef = "tel:" + li.phone;
                    }

                    lblAddress.Text = li.address + " " + li.city;
                    string address = li.address + ",+" + li.city + ",+" + li.state + ",+" + li.zip_code;
                    hypGoogleMap.NavigateUrl = "http://maps.google.com/maps?f=q&source=s_q&hl=en&geocode=&q=" + address;
                 if(li.SuperintendentId!=0)
                    {
                      
                        user_info objUser = _db.user_infos.SingleOrDefault(u => u.user_id == li.SuperintendentId);
                        if (objUser != null)
                        {
                            lblSuperintendent.Text = objUser.first_name + " " + objUser.last_name;
                            lblPhone.Text = objUser.phone;
                            hyPhone.HRef = "tel:" + objUser.phone;
                        }
                    }
                    else
                    {
                        lblSuperintendent.Text="";
                        lblPhone.Text = "";
                     
                    }
                    
                }
                lblSearhText.Visible = false;
                pnlCustomerFullName.Visible = true;
                DataTable dt = csCommonUtility.LINQToDataTable(mList);
                Session.Add("customerList", dt);
                grdIcon.DataSource = mList;
                grdIcon.DataKeyNames = new string[] { "customer_id", "sales_person_id" };
                grdIcon.DataBind();
            }
            else
            {
                lblSearhText.Visible = true;
                pnlCustomerFullName.Visible = false;
            }
    }

    protected void grdCustomerList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataClassesDataContext _db = new DataClassesDataContext();
           LinkButton InkCustomerName = (LinkButton)e.Row.FindControl("InkCustomerName");
            if (e.Row.RowIndex == 0)
            {
                InkCustomerName.CssClass = "custName";
            }
             int ncid = Convert.ToInt32(grdCustomerList.DataKeys[e.Row.RowIndex].Values[0]);
            // Customer Address
            customer cust = new customer();
            cust = _db.customers.Single(c => c.customer_id == ncid);
          
            InkCustomerName.Text = cust.last_name1;
            InkCustomerName.CommandArgument = ncid.ToString();
        }
    }

    protected void GetCustomer(object sender, EventArgs e)
    {
        try
        {
            
            LinkButton InkCustomerName = (LinkButton)sender;
            KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, InkCustomerName.ID, InkCustomerName.GetType().Name, "Gridview click Select Customer Landing Page");

            int nCustomerId = Convert.ToInt32(InkCustomerName.CommandArgument);
            hdnCustomerId.Value = nCustomerId.ToString();
            GetCustomerByCustomerID(nCustomerId);
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }

    private void GetCustomerByCustomerID( int nCustomerId)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        customer objCust = _db.customers.SingleOrDefault(c => c.customer_id == nCustomerId);
        if (objCust != null)
        {
            if (objCust.notes.Length > 70)
            {
                pnlLeadNotes.Visible = true;
                lblCustomerLeadNotes.Text = objCust.notes.Substring(0, 70) + "...";
            }
            else
            {
                if (objCust.notes != "" && objCust.notes != null)
                {
                    lblCustomerLeadNotes.Text = objCust.notes;
                    inkLeadnoteView.Visible = false;
                }
                else
                    pnlLeadNotes.Visible = false;
            }

            if (objCust.mobile != "" && objCust.mobile != null)
            {
                lblCustomerMobile.Visible = true;
                lblCustomerPhone.Text = objCust.phone + ", ";
                hrfCustomerPhone.HRef = "tel:" + objCust.phone;
                lblCustomerMobile.Text = objCust.mobile + " m";
                hrfCustomerMobile.HRef = "tel:" + objCust.mobile;
            }
            else
            {
                lblCustomerMobile.Visible = false;
                lblCustomerPhone.Text = objCust.phone;
                hrfCustomerPhone.HRef = "tel:" + objCust.phone;
            }


            lblSearchCustomerName.Text = objCust.first_name1 + " " + objCust.last_name1;
            string address = objCust.address + ",+" + objCust.city + ",+" + objCust.state + ",+" + objCust.zip_code;
            hypGoogleMap.NavigateUrl = "http://maps.google.com/maps?f=q&source=s_q&hl=en&geocode=&q=" + address;
            lblAddress.Text = objCust.address + " " + objCust.city;

            if (objCust.SuperintendentId != 0)
            {
                user_info objUser = _db.user_infos.SingleOrDefault(u => u.user_id == objCust.SuperintendentId);
                if (objUser != null)
                {
                    lblSuperintendent.Text = objUser.first_name + " " + objUser.last_name;
                    lblPhone.Text = objUser.phone;
                    hyPhone.HRef = "tel:" + objUser.phone;
                }

            }
            else
            {
                lblSuperintendent.Text = "";
                lblPhone.Text = "";
            }
        }
        string strQ = string.Empty;

        if (Session["oUser"] != null)
        {
            userinfo obj = (userinfo)Session["oUser"];

            strQ = "delete from searchcustomers WHERE customer_id ='" + objCust.customer_id + "' AND userId=" + obj.user_id + "  AND IsCrew=0  ";
            _db.ExecuteCommand(strQ, string.Empty);

            strQ = "insert into searchcustomers " +
                         " SELECT  [client_id],[customer_id],[first_name1],[last_name1] ,[first_name2],[last_name2],[address] ,[cross_street], " +
                         " [city] ,[state] ,[zip_code] ,[fax] ,[email] ,[phone] ,[is_active],[registration_date],[sales_person_id],[update_date], " +
                         " [status_id],[notes] ,[appointment_date],[lead_source_id] ,[status_note],[company] ,[email2],[SuperintendentId], " +
                         " [mobile],[lead_status_id],[islead],[isCustomer],[website],[isCalendarOnline], " + obj.user_id + ",getdate(),0 " +
                         " FROM customers " +
                         " WHERE status_id NOT IN(4,5) AND  customer_id ='" + objCust.customer_id + "' ";

            _db.ExecuteCommand(strQ, string.Empty);
        }

        if (Session["oCrew"] != null)
        {
            Crew_Detail objC = (Crew_Detail)Session["oCrew"];

            strQ = "delete from searchcustomers WHERE customer_id ='" + objCust.customer_id + "' AND userId=" + objC.crew_id + "  AND IsCrew=1  ";
            _db.ExecuteCommand(strQ, string.Empty);

            strQ = "insert into searchcustomers " +
                         " SELECT  [client_id],[customer_id],[first_name1],[last_name1] ,[first_name2],[last_name2],[address] ,[cross_street], " +
                         " [city] ,[state] ,[zip_code] ,[fax] ,[email] ,[phone] ,[is_active],[registration_date],[sales_person_id],[update_date], " +
                         " [status_id],[notes] ,[appointment_date],[lead_source_id] ,[status_note],[company] ,[email2],[SuperintendentId], " +
                         " [mobile],[lead_status_id],[islead],[isCustomer],[website],[isCalendarOnline], " + objC.crew_id + ",getdate(),1 " +
                         " FROM customers " +
                         " WHERE status_id NOT IN(4,5) AND  customer_id ='" + objCust.customer_id + "' ";

            _db.ExecuteCommand(strQ, string.Empty);
        }
        strQ = " SELECT DISTINCT  client_id,customers.islead,customers.isCustomer, customers.customer_id, first_name1+' '+ last_name1 AS customer_name, first_name1, last_name1, first_name2, last_name2, address, cross_street, city, state, zip_code, fax, email, phone, is_active, registration_date, mobile," +
                                  " sales_person_id, update_date, status_id, notes, appointment_date, lead_source_id, status_note, company, email2, SuperintendentId " +
                                  " FROM customers where status_id NOT IN(4,5) AND customer_id=" + nCustomerId;

        IEnumerable<csCustomer> mList = _db.ExecuteQuery<csCustomer>(strQ, string.Empty).ToList();
        if (mList.Count() > 0)
        {
            lblSearhText.Visible = false;
            pnlCustomerFullName.Visible = true;
            DataTable dt = csCommonUtility.LINQToDataTable(mList);
            Session.Add("customerList", dt);
            grdIcon.DataSource = mList;
            grdIcon.DataKeyNames = new string[] { "customer_id", "sales_person_id" };
            grdIcon.DataBind();
        }
        else
        {
            lblSearhText.Visible = true;
            pnlCustomerFullName.Visible = false;
        }

        GetSearchCustomer(0);
    }
    protected void grdCustomerList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, grdCustomerList.ID, grdCustomerList.GetType().Name, "PageIndexChanging");
        GetSearchCustomer(e.NewPageIndex);
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnNext.ID, btnNext.GetType().Name, "Click"); 
        int nCurrentPage = 0;
        nCurrentPage = Convert.ToInt32(hdnCurrentPageNo.Value);
        GetSearchCustomer(nCurrentPage);
    }
    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnPrevious.ID, btnPrevious.GetType().Name, "Click"); 
        int nCurrentPage = 0;
        nCurrentPage = Convert.ToInt32(hdnCurrentPageNo.Value);
        GetSearchCustomer(nCurrentPage - 2);
    }

    protected void grdIcon_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                int ncid = Convert.ToInt32(grdIcon.DataKeys[e.Row.RowIndex].Values[0]);
                int nEstId = 0;

                if (_db.customer_estimates.Where(ce => ce.customer_id == ncid && ce.client_id == 1 && ce.status_id == 3 && ce.IsEstimateActive == true).ToList().Count > 0)
                {

                    var result = (from ce in _db.customer_estimates
                                  where ce.customer_id == ncid && ce.client_id == 1 && ce.status_id == 3 && ce.IsEstimateActive == true
                                  select ce.estimate_id);

                    int n = result.Count();
                    if (result != null && n > 0)
                        nEstId = result.Max();
                }
                else
                {

                    var result = (from ce in _db.customer_estimates
                                  where ce.customer_id == ncid && ce.client_id == 1 && ce.IsEstimateActive == true
                                  select ce.estimate_id);

                    int n = result.Count();
                    if (result != null && n > 0)
                        nEstId = result.Max();
                    else
                        nEstId = 1;
                }

                //Panel pnlLead = (Panel)e.Row.FindControl("pnlLead");
                Panel pnlCompoSiteCrew = (Panel)e.Row.FindControl("pnlCompoSiteCrew");
                //Panel pnlCompositUser = (Panel)e.Row.FindControl("pnlCompositUser");
                //if (Session["oCrew"] != null)
                //{
                //    pnlLead.Visible = false;
                //    pnlCompositUser.Visible = false;
                //    pnlCompoSiteCrew.Visible = true;
                 
                //}
                //else
                //{
                //    pnlCompoSiteCrew.Visible = false;
                //    pnlLead.Visible = true;
                //    pnlCompositUser.Visible = true;
                //}

                Image imgSiteReview = (Image)e.Row.FindControl("imgSiteReview");
                imgSiteReview.ImageUrl = "~/assets/mobileicons/12-Site-review-List.png";

                HyperLink hypSiteReview = (HyperLink)e.Row.FindControl("hypSiteReview");
                hypSiteReview.NavigateUrl = "msiteviewlist.aspx?cid=" + ncid + "&nestid=" + nEstId;
                //Crew

                HyperLink hyp_DocumentManagement = (HyperLink)e.Row.FindControl("hyp_DocumentManagement");
                hyp_DocumentManagement.NavigateUrl = "mDocumentManagement.aspx?cid=" + ncid;

                HyperLink hypCompositrSowCrew = (HyperLink)e.Row.FindControl("hypCompositrSowCrew");
                hypCompositrSowCrew.NavigateUrl = "mcomposite_sow.aspx?cid=" + ncid + "&nestid=" + nEstId;
                HyperLink hypProjectNotes = (HyperLink)e.Row.FindControl("hypProjectNotes");
                hypProjectNotes.NavigateUrl = "mProjectNotes.aspx?cid=" + ncid + "&nestid=" + nEstId;
                HyperLink hypSelection = (HyperLink)e.Row.FindControl("hypSelection");
                hypSelection.NavigateUrl = "mselectionlist.aspx?cid=" + ncid + "&nestid=" + nEstId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    protected void lnkTimeClock_Click(object sender, EventArgs e)
    {
        Response.Redirect("mtimeclock.aspx");
    }
    protected void ddCustomer_SelectedIndexChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, ddCustomer.ID, ddCustomer.GetType().Name, "SelectedIndexChanged"); 
        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            string strQ = string.Empty;
            KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, ddCustomer.ID, ddCustomer.GetType().Name, "Select Customer Landing Page");
           
            if (Session["oUser"] != null)
            {
                userinfo obj = (userinfo)Session["oUser"];

                if (ddCustomer.SelectedValue != "Select Customer")
                {
                    hdnCustomerId.Value = ddCustomer.SelectedValue;
                    customer objCust = _db.customers.SingleOrDefault(c => c.customer_id == Convert.ToInt32(ddCustomer.SelectedValue));
                    if (objCust.notes.Length > 0)
                    {
                        pnlLeadNotes.Visible = true;
                    }
                    else
                    {
                        pnlLeadNotes.Visible = false;
                    }
                    strQ = " delete from searchcustomers WHERE customer_id ='" + ddCustomer.SelectedValue + "' AND userId=" + obj.user_id + "  AND IsCrew=0 ";
                    _db.ExecuteCommand(strQ, string.Empty);

                    strQ = "insert into searchcustomers " +
                                 " SELECT  [client_id],[customer_id],[first_name1],[last_name1] ,[first_name2],[last_name2],[address] ,[cross_street], " +
                                 " [city] ,[state] ,[zip_code] ,[fax] ,[email] ,[phone] ,[is_active],[registration_date],[sales_person_id],[update_date], " +
                                 " [status_id],[notes] ,[appointment_date],[lead_source_id] ,[status_note],[company] ,[email2],[SuperintendentId], " +
                                 " [mobile],[lead_status_id],[islead],[isCustomer],[website],[isCalendarOnline], " + obj.user_id + ",getdate(),0 " +
                                 " FROM customers " +
                                 " WHERE status_id NOT IN(4,5) AND customer_id ='" + ddCustomer.SelectedValue + "'";
                                _db.ExecuteCommand(strQ, string.Empty);
                }
            }

            if (Session["oCrew"] != null)
            {
                Crew_Detail objC = (Crew_Detail)Session["oCrew"];
                if (ddCustomer.SelectedValue != "Select Customer")
                {
                    customer objCust = _db.customers.SingleOrDefault(c => c.customer_id == Convert.ToInt32(ddCustomer.SelectedValue));
                    if (objCust.notes.Length > 0)
                    {
                        pnlLeadNotes.Visible = true;
                    }
                    else
                    {
                        pnlLeadNotes.Visible = false;
                    }
                    hdnCustomerId.Value = ddCustomer.SelectedValue;
                    strQ = " delete from searchcustomers WHERE customer_id ='" + ddCustomer.SelectedValue + "' AND userId=" + objC.crew_id + "  AND IsCrew=1 "; 
                   _db.ExecuteCommand(strQ, string.Empty);
                    strQ = "insert into searchcustomers " +
                                 " SELECT  [client_id],[customer_id],[first_name1],[last_name1] ,[first_name2],[last_name2],[address] ,[cross_street], " +
                                 " [city] ,[state] ,[zip_code] ,[fax] ,[email] ,[phone] ,[is_active],[registration_date],[sales_person_id],[update_date], " +
                                 " [status_id],[notes] ,[appointment_date],[lead_source_id] ,[status_note],[company] ,[email2],[SuperintendentId], " +
                                 " [mobile],[lead_status_id],[islead],[isCustomer],[website],[isCalendarOnline], " + objC.crew_id + ",getdate(),1 " +
                                 " FROM customers " +
                                 " WHERE status_id NOT IN(4,5) AND customer_id ='" + ddCustomer.SelectedValue + "'";
                                _db.ExecuteCommand(strQ, string.Empty);
                }
          }
            GetSearchCustomer(0);
            GetCustomersNew(0);
         }
       catch( Exception ex)
        {
            throw ex;
        }
    }
    protected void InkSchedule_Click(object sender, EventArgs e)
    {
        Response.Redirect("mcrewschedulecalendar.aspx");
    }
    protected void inkMyJobs_Click(object sender, EventArgs e)
    {
         Response.Redirect("mmyjobs.aspx");
 
    }


  protected void inkLeadnoteView_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, inkLeadnoteView.ID, inkLeadnoteView.GetType().Name, "Click"); 
        DataClassesDataContext _db = new DataClassesDataContext();

        try
        {
            customer cust = _db.customers.SingleOrDefault(c => c.customer_id == Convert.ToInt32(hdnCustomerId.Value));
            if (cust != null)
            {
                lblCustomerName.Text = "(" + cust.last_name1 + ")";
                lblLeadsNote.Text = cust.notes;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

  protected void btnSearch_Click(object sender, EventArgs e)
  {
      KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSearch.ID, btnSearch.GetType().Name, "Click"); 
          string StrSearch = txtSearch.Text.Trim().Replace("'","");
          lblResultMSG.Text = "";
          if (StrSearch.Contains(","))
          {
              try
              {
                  DataClassesDataContext _db = new DataClassesDataContext();
                  string StrCondition = string.Empty;

                  string[] fullName = StrSearch.Split(',');

                  string lastName = fullName[0].Trim();
                  string firstName = fullName[1].Trim();
                  

                  if (Session["oUser"] != null)
                  {
                      userinfo obj = (userinfo)Session["oUser"];
                      if (obj.role_id == 1)
                      {
                          StrCondition = " where  status_id NOT IN(4,5) AND last_name1 ='"+lastName+ "' AND first_name1='"+firstName+"'";
                      }
                      else if (obj.role_id == 4)
                      {
                          StrCondition = " where status_id NOT IN(4,5) AND   last_name1 ='" + lastName + "' AND first_name1='" + firstName + "' AND SuperintendentId=" + obj.user_id;
                      }
                      else
                      {
                          StrCondition = " where status_id NOT IN(4,5) AND  last_name1 ='" + lastName + "' AND first_name1='" + firstName + "' AND sales_person_id=" + obj.sales_person_id;
                      }

                      string strQ = "select last_name1+', '+first_name1 AS customer_name,customer_id,last_name1,first_name1 from customers  " + StrCondition + " order by customer_name ";
                      List<csCustomer> mList = _db.ExecuteQuery<csCustomer>(strQ, string.Empty).ToList();
                      if (mList.Count==1)
                      {
                          var custObj = mList.SingleOrDefault(c => c.first_name1 == firstName || c.last_name1 == lastName);
                          ddCustomer.DataSource = mList;
                          ddCustomer.DataTextField = "customer_name";
                          ddCustomer.DataValueField = "customer_id";
                          ddCustomer.DataBind();
                          ddCustomer.SelectedValue = custObj.customer_id.ToString();
                          GetCustomerByCustomerID(custObj.customer_id);
                      }
                      else
                      {
                          ddCustomer.DataSource = mList;
                          ddCustomer.DataTextField = "customer_name";
                          ddCustomer.DataValueField = "customer_id";
                          ddCustomer.DataBind();
                          ddCustomer.Items.Insert(0, "Select Customer");
                      }

                     

                  }
                  if (Session["oCrew"] != null)
                  {
                      Crew_Detail objC = (Crew_Detail)Session["oCrew"];
                      string CrewName = objC.first_name.Trim() + " " + objC.last_name.Trim();
                      StrCondition = " where customers.IsCustomer=1 AND customers.status_id NOT IN(4,5) AND ScheduleCalendar.IsEstimateActive = 1 AND ScheduleCalendar.employee_name  LIKE '%" + CrewName + "%' and last_name1 ='" + lastName + "' AND first_name1='" + firstName + "'";
                      string strQ = " select distinct last_name1+', '+first_name1 AS customer_name,customers.customer_id,last_name1,first_name1 from customers " +
                                    " Right join ScheduleCalendar on  customers.customer_id=ScheduleCalendar.customer_id " +
                                     " " + StrCondition + " ";

                      List<csCustomer> mList = _db.ExecuteQuery<csCustomer>(strQ, string.Empty).ToList();

                      if (mList.Count==1)
                      {
                          var custObj = mList.SingleOrDefault(c => c.first_name1 == firstName || c.last_name1 == lastName);
                          ddCustomer.DataSource = mList;
                          ddCustomer.DataTextField = "customer_name";
                          ddCustomer.DataValueField = "customer_id";
                          ddCustomer.DataBind();
                          ddCustomer.SelectedValue = custObj.customer_id.ToString();
                          GetCustomerByCustomerID(custObj.customer_id);
                      }
                      else
                      {
                          ddCustomer.DataSource = mList;
                          ddCustomer.DataTextField = "customer_name";
                          ddCustomer.DataValueField = "customer_id";
                          ddCustomer.DataBind();
                          ddCustomer.Items.Insert(0, "Select Customer");
                      }

                  }
              }
              catch (Exception ex)
              {
                  throw ex;
              }
          }
          else
          {
              if (StrSearch == "")
                  BindCustomer();
              else
                lblResultMSG.Text = csCommonUtility.GetSystemErrorMessage("No record found.");
          }
         
  }
}