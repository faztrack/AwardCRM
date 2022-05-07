using System;
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
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Exchange.WebServices.Data;

public partial class sendemailoutlook : System.Web.UI.Page
{
    private string COName = "";
    private string strCustName = "";
    private string strEstName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);
            customer cust = new customer();
            //wtmFileNumber.WatermarkText = "";
            //wtmBcc.WatermarkText = "";
            string strEmailSignature = "";
            imgCencel.Attributes.Add("onClick", "CloseWindow();");
            DataClassesDataContext _db = new DataClassesDataContext();
            if (Request.QueryString.Get("custId") != null)
            {
                int ncid = Convert.ToInt32(Request.QueryString.Get("custId"));
                if (ncid > 0)
                {
                    hdnCustomerId.Value = ncid.ToString();

                    string DestinationPath = Server.MapPath("~/Uploads//" + hdnCustomerId.Value + "//Temp//");
                    if (Directory.Exists(DestinationPath))
                    {
                        string[] fileEntries = Directory.GetFiles(DestinationPath);
                        foreach (string file in fileEntries)
                        {
                            File.Delete(file);
                        }

                    }
                }
                else // New Code ZAM 13 10 2018
                {
                    csCommonUtility.setDMUserData objDMU = (csCommonUtility.setDMUserData)Session["sDMUserData"];
                    if (objDMU != null)
                    {
                        hdnCustomerId.Value = objDMU.CustomerId.ToString();

                        System.Threading.Thread.Sleep(1000);

                    }

                }

            }

            userinfo obj = new userinfo();
            if ((userinfo)Session["oUser"] != null)
            {
                obj = (userinfo)Session["oUser"];
                txtFrom.Text = obj.company_email;
                if (txtFrom.Text == "")
                    txtFrom.Text = obj.email;
                hdnEmailType.Value = obj.EmailIntegrationType.ToString();
            }

            if (Convert.ToInt32(hdnCustomerId.Value) > 0)
            {

                cust = _db.customers.Single(c => c.customer_id == Convert.ToInt32(hdnCustomerId.Value));
                hdnSalesID.Value = cust.sales_person_id.ToString();
                txtTo.Text = cust.email;
                if (cust.email2 != null)
                {
                    if (cust.email2.ToString().Length > 4)
                    {
                        txtCc2.Text = cust.email2;
                        Email2.Visible = true;
                    }
                    else
                    {
                        Email2.Visible = false;
                        txtCc2.Text = "";
                    }
                }
                else
                {
                    Email2.Visible = false;
                    txtCc2.Text = "";
                }
                company_profile com = new company_profile();
                if (_db.company_profiles.Where(cp => cp.client_id == 1).SingleOrDefault() != null)
                {
                    com = _db.company_profiles.Single(cp => cp.client_id == 1);

                    txtCc.Text = "";// com.email;


                }


                if (obj.user_id > 0)
                {
                    user_info objuser = _db.user_infos.Where(u => u.user_id == obj.user_id).SingleOrDefault();

                    if (objuser != null)
                    {
                        strEmailSignature = objuser.EmailSignature ?? "";
                    }
                }
            }

            if (Request.QueryString.Get("cfn") != null)
            {
                try
                {
                    string sFileName = Request.QueryString.Get("cfn") + ".pdf";
                    string sourceFile = Server.MapPath("tmp\\Contract") + @"\" + sFileName;

                    string DestinationPath = Server.MapPath("~/Uploads//" + hdnCustomerId.Value + "//Temp//");
                    if (!System.IO.Directory.Exists(DestinationPath))
                    {
                        System.IO.Directory.CreateDirectory(DestinationPath);
                    }
                    File.Move(sourceFile, DestinationPath + sFileName);

                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }
            else if (Request.QueryString.Get("sfn") != null)
            {
                try
                {
                    txtTo.Attributes.Add("placeholder", "type in email address - use comma separator for multiple emails");
                    txtBcc.Attributes.Add("placeholder", "type in vendor email address - use comma separator for multiple vendors");
                    //wtmFileNumber.WatermarkText = "type in email address - use comma separator for multiple emails";
                    //wtmBcc.WatermarkText = "type in vendor email address - use comma separator for multiple vendors";
                    string sFileName = Request.QueryString.Get("sfn") + ".pdf";
                    string sourceFile = Server.MapPath("tmp\\Contract") + @"\" + sFileName;
                    if (Session["sSubject"] != null)
                    {
                        txtSubject.Text = Session["sSubject"].ToString();
                    }

                    if (Session["sBody"] != null)
                    {
                        txtBody.Content = Session["sBody"].ToString();
                    }
                    txtTo.Text = "";

                    int nCustomerId = Convert.ToInt32(Session["CustomerId"]);
                    string DestinationPath = Server.MapPath("~/Uploads//" + hdnCustomerId.Value + "//Temp//");
                    if (!System.IO.Directory.Exists(DestinationPath))
                    {
                        System.IO.Directory.CreateDirectory(DestinationPath);
                    }
                    File.Move(sourceFile, DestinationPath + sFileName);

                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }
            else if (Request.QueryString.Get("cofn") != null)
            {
                try
                {
                    string sFileName = Request.QueryString.Get("cofn") + ".pdf";
                    string sourceFile = Server.MapPath("tmp\\ChangeOrder") + @"\" + sFileName;

                    string DestinationPath = Server.MapPath("~/Uploads//" + hdnCustomerId.Value + "//Temp//");
                    if (!System.IO.Directory.Exists(DestinationPath))
                    {
                        System.IO.Directory.CreateDirectory(DestinationPath);
                    }
                    File.Move(sourceFile, DestinationPath + sFileName);

                    if (Request.QueryString.Get("coid") != null)
                    {
                        int ncid = Convert.ToInt32(Request.QueryString.Get("coid"));
                        hdnChEstId.Value = ncid.ToString();



                    }
                    if (Request.QueryString.Get("eid") != null)
                    {
                        int ncid = Convert.ToInt32(Request.QueryString.Get("eid"));
                        hdnEstimateId.Value = ncid.ToString();
                    }
                    changeorder_estimate cho = new changeorder_estimate();
                    cho = _db.changeorder_estimates.Single(ce => ce.estimate_id == Convert.ToInt32(hdnEstimateId.Value) && ce.customer_id == Convert.ToInt32(hdnCustomerId.Value) && ce.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && ce.chage_order_id == Convert.ToInt32(hdnChEstId.Value));
                    COName = cho.changeorder_name;
                    if (Request.QueryString.Get("afn") != null)
                    {

                        strCustName = cust.first_name1 + " " + cust.last_name1;
                        string strcfn = Request.QueryString.Get("afn");
                        if (strcfn == "a")
                        {
                            txtTo.Text = "";
                            txtSubject.Text = "Approval request for Change Order (" + strCustName + ", " + COName + ")";
                            Email2.Visible = false;
                            txtCc2.Text = "";
                        }

                    }
                    else if (Request.QueryString.Get("scfn") != null)
                    {

                        strCustName = cust.first_name1 + " " + cust.last_name1;
                        string strcfn = Request.QueryString.Get("scfn");
                        if (strcfn == "s")
                        {
                            sales_person sp = new sales_person();
                            sp = _db.sales_persons.Single(s => s.sales_person_id == Convert.ToInt32(hdnSalesID.Value));
                            txtTo.Text = sp.email;
                            txtSubject.Text = "Request for Review - Change Order (" + strCustName + ", " + COName + ")";
                            Email2.Visible = false;
                            txtCc2.Text = "";
                        }

                    }
                    else
                    {


                        string strLibk = " </br> </br> </br></br></br></br></br></br> ";

                        //strLibk += "https://awardkb.faztrack.com/customerlogin.aspx?coid=" + Convert.ToInt32(hdnChEstId.Value) + "&eid=" + Convert.ToInt32(hdnEstimateId.Value) +
                        //    "&cid=" + Convert.ToInt32(hdnCustomerId.Value) + "&coestid=" + cho.change_order_estimate_id + "";

                        strLibk += "Please click <a target='_blank' href='https://awardkb.faztrack.com/customerlogin.aspx?coid=" + Convert.ToInt32(hdnChEstId.Value) + "&eid=" + Convert.ToInt32(hdnEstimateId.Value) +
                            "&cid=" + Convert.ToInt32(hdnCustomerId.Value) + "&coestid=" + cho.change_order_estimate_id + "'> here </a> to review your change order.";

                        txtTo.Text = cust.email;
                        txtBody.Content = strLibk;
                    }

                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }
            else if (Request.QueryString.Get("ssfn") != null)
            {
                try
                {
                    if (Request.QueryString.Get("eid") != null)
                    {
                        int ncid = Convert.ToInt32(Request.QueryString.Get("eid"));
                        hdnEstimateId.Value = ncid.ToString();
                    }
                    lblTitle.Text = "Selection Email";
                    customer_estimate cust_est = new customer_estimate();
                    cust_est = _db.customer_estimates.Single(c => c.estimate_id == Convert.ToInt32(hdnEstimateId.Value) && c.customer_id == Convert.ToInt32(hdnCustomerId.Value));

                    txtSubject.Text = "Selection(" + cust.last_name1 + ", " + cust_est.estimate_name + ") ";

                    string strLibk = " </br> </br>";

                    strLibk += "Please click <a target='_blank' href='https://awardkb.faztrack.com/customerlogin.aspx?eid=" + Convert.ToInt32(hdnEstimateId.Value) +
                        "&cid=" + Convert.ToInt32(hdnCustomerId.Value) + "&ssfn=a'> here </a> to review following selection.";

                    txtTo.Text = cust.email;
                    txtBody.Content = strLibk;
                    if (Session["MessBody"] != null)
                    {
                        string str = Convert.ToString(Session["MessBody"]);
                        txtBody.Content += str;

                    }

                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }
            else if (Request.QueryString.Get("precon") != null)
            {
                try
                {
                    if (Request.QueryString.Get("eid") != null)
                    {
                        int ncid = Convert.ToInt32(Request.QueryString.Get("eid"));
                        hdnEstimateId.Value = ncid.ToString();
                    }


                    if ((userinfo)Session["oUser"] != null)
                    {
                        obj = (userinfo)Session["oUser"];
                        txtFrom.Text = obj.email;
                        if (txtFrom.Text == "")
                            txtFrom.Text = obj.email;
                        hdnEmailType.Value = obj.EmailIntegrationType.ToString();
                    }
                    lblTitle.Text = "Pre-Construction Checklist Email";
                    customer_estimate cust_est = new customer_estimate();
                    cust_est = _db.customer_estimates.Single(c => c.estimate_id == Convert.ToInt32(hdnEstimateId.Value) && c.customer_id == Convert.ToInt32(hdnCustomerId.Value));

                    string strLibk = "</br> </br></br></br>";
                    strLibk = "Pre-Con Flagged Item(s) (" + cust.last_name1 + ": " + cust_est.estimate_name + ") ";
                    txtSubject.Text = "Pre-Con Flagged Item(s) (" + cust.last_name1 + ": " + cust_est.estimate_name + ") ";

                    txtTo.Text = cust.email;
                    txtBody.Content = strLibk;
                    if (Session["MessBody"] != null)
                    {
                        string str = Convert.ToString(Session["MessBody"]);
                        txtBody.Content += str;

                    }

                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                }
            }

            else if (Request.QueryString.Get("directId") != null)
            {
                lblTitle.Text = "Section Email";
                int nEstId = Convert.ToInt32(Request.QueryString.Get("eid"));
                hdnEstimateId.Value = nEstId.ToString();
                int nsid = Convert.ToInt32(Request.QueryString.Get("sid"));
                int ndirectId = Convert.ToInt32(Request.QueryString.Get("directId"));

                GetSectionEmailBody(nEstId, nsid, ndirectId);
            }
            else if (Request.QueryString.Get("eid") != null)
            {
                lblTitle.Text = "Statement Email";
                int nEid = Convert.ToInt32(Request.QueryString.Get("eid"));
                hdnEstimateId.Value = nEid.ToString();
                customer_estimate cust_est = new customer_estimate();
                cust_est = _db.customer_estimates.Single(c => c.estimate_id == Convert.ToInt32(hdnEstimateId.Value) && c.customer_id == Convert.ToInt32(hdnCustomerId.Value));

                txtSubject.Text = "Statement(" + cust.last_name1 + ", " + cust_est.estimate_name + ") ";

                if (Session["MessBody"] != null)
                {
                    string str = Convert.ToString(Session["MessBody"]);
                    txtBody.Content = str;

                    //    Session.Remove("MessBody");

                }
            }
            else if (Request.QueryString.Get("calnotifyCustID") != null)
            {

                lblTitle.Text = "Calendar Notification Email";

                int ncid = Convert.ToInt32(Request.QueryString.Get("calnotifyCustID"));
                int neid = Convert.ToInt32(Request.QueryString.Get("calnotifyEstID"));

                hdnCustomerId.Value = ncid.ToString();
                hdnEstimateId.Value = neid.ToString();

                string strUserName = "";
                string strCompanyName = "";
                string strCustomerPortalURL = "";

                company_profile objCom = new company_profile();
                if (_db.company_profiles.Where(cp => cp.client_id == 1).SingleOrDefault() != null)
                {
                    objCom = _db.company_profiles.Single(cp => cp.client_id == 1);
                    strCompanyName = objCom.company_name;
                    strCustomerPortalURL = objCom.CustomerPortalURL;
                }

                userinfo objUser = new userinfo();
                if (Session["oUser"] != null)
                {
                    objUser = (userinfo)Session["oUser"];
                    strUserName = objUser.first_name + " " + objUser.last_name;

                    txtFrom.Text = obj.company_email;
                    if (txtFrom.Text == "")
                        txtFrom.Text = obj.email;
                    hdnEmailType.Value = obj.EmailIntegrationType.ToString();
                }

                customer objCust = _db.customers.SingleOrDefault(c => c.customer_id == ncid);
                if (objCust != null)
                {
                    txtTo.Text = objCust.email.ToString();
                    strCustName = objCust.first_name1 + " " + objCust.last_name1;
                }

                txtSubject.Text = "Calendar update alert";

                txtBody.Content += "Dear Customer,<br/><br/>" +
                    "Your calendar has been updated. Please go to your customer portal to review the changes.<br/><br/>" +
                            "" + strCustomerPortalURL + "" +
                           "<br/><br/><br/>" +
                           "Sincerely,<br/>" +
                           "" + strUserName + "<br/>" +
                           "" + strCompanyName;
            }

            txtBody.Content += strEmailSignature;
        }
        //else if (file_upload.PostedFile.FileName.Length > 0)
        //{

        //    btnUpload_Click(sender, new EventArgs());
        //}

        LoadAttachment();
    }


    private void GetSectionEmailBody(int nEstId, int nsid, int ndirectId)
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        var price_detail = from p in _db.co_pricing_masters
                           join lc in _db.locations on p.location_id equals lc.location_id
                           where (from clc in _db.changeorder_locations
                                  where clc.estimate_id == Convert.ToInt32(hdnEstimateId.Value) && clc.customer_id == Convert.ToInt32(hdnCustomerId.Value) && clc.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                                  select clc.location_id).Contains(p.location_id) &&
                                  (from cs in _db.changeorder_sections
                                   where cs.estimate_id == Convert.ToInt32(hdnEstimateId.Value) && cs.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cs.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                                   select cs.section_id).Contains(p.section_level)
                                  && p.item_status_id != 3 && p.section_level == nsid && p.is_direct == ndirectId && p.estimate_id == Convert.ToInt32(hdnEstimateId.Value) && p.customer_id == Convert.ToInt32(hdnCustomerId.Value) && p.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                           orderby p.week_id, p.section_level, p.execution_unit, lc.location_name ascending

                           select new CO_PricingDeatilModel()
                           {
                               co_pricing_list_id = (int)p.co_pricing_list_id,
                               item_id = (int)p.item_id,
                               labor_id = (int)p.labor_id,
                               section_serial = (decimal)p.section_serial,
                               location_name = lc.location_name,
                               section_name = p.section_name,
                               item_name = p.item_name,
                               measure_unit = p.measure_unit,
                               item_cost = (decimal)p.item_cost,
                               total_retail_price = (decimal)p.total_retail_price,
                               total_direct_price = (decimal)p.total_direct_price,
                               minimum_qty = (decimal)p.minimum_qty,
                               quantity = (decimal)p.quantity,
                               retail_multiplier = (decimal)p.retail_multiplier,
                               labor_rate = (decimal)p.labor_rate,
                               short_notes = p.short_notes,
                               item_status_id = (int)p.item_status_id,
                               tmpCol = string.Empty,
                               week_id = (int)p.week_id,
                               execution_unit = (decimal)p.execution_unit,
                               is_complete = (bool)p.is_complete,
                               schedule_note = p.schedule_note
                           };
        if (nsid < 1000)
        {
            price_detail = from p in _db.co_pricing_masters
                           join lc in _db.locations on p.location_id equals lc.location_id
                           where (from clc in _db.changeorder_locations
                                  where clc.estimate_id == Convert.ToInt32(hdnEstimateId.Value) && clc.customer_id == Convert.ToInt32(hdnCustomerId.Value) && clc.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                                  select clc.location_id).Contains(p.location_id) &&
                                  (from cs in _db.changeorder_sections
                                   where cs.estimate_id == Convert.ToInt32(hdnEstimateId.Value) && cs.customer_id == Convert.ToInt32(hdnCustomerId.Value) && cs.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                                   select cs.section_id).Contains(p.section_level)
                                  && p.item_status_id != 3 && p.week_id == nsid && p.is_direct == ndirectId && p.estimate_id == Convert.ToInt32(hdnEstimateId.Value) && p.customer_id == Convert.ToInt32(hdnCustomerId.Value) && p.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                           orderby p.week_id, p.section_level, p.execution_unit, lc.location_name ascending

                           select new CO_PricingDeatilModel()
                           {
                               co_pricing_list_id = (int)p.co_pricing_list_id,
                               item_id = (int)p.item_id,
                               labor_id = (int)p.labor_id,
                               section_serial = (decimal)p.section_serial,
                               location_name = lc.location_name,
                               section_name = p.section_name,
                               item_name = p.item_name,
                               measure_unit = p.measure_unit,
                               item_cost = (decimal)p.item_cost,
                               total_retail_price = (decimal)p.total_retail_price,
                               total_direct_price = (decimal)p.total_direct_price,
                               minimum_qty = (decimal)p.minimum_qty,
                               quantity = (decimal)p.quantity,
                               retail_multiplier = (decimal)p.retail_multiplier,
                               labor_rate = (decimal)p.labor_rate,
                               short_notes = p.short_notes,
                               item_status_id = (int)p.item_status_id,
                               tmpCol = string.Empty,
                               week_id = (int)p.week_id,
                               execution_unit = (decimal)p.execution_unit,
                               is_complete = (bool)p.is_complete,
                               schedule_note = p.schedule_note
                           };

        }
        DataTable dt = SessionInfo.LINQToDataTable(price_detail);

        string Body = CreateHtml(dt, Convert.ToInt32(hdnCustomerId.Value));
        txtBody.Content = Body;






    }


    string CreateHtml(DataTable dt, int ncid)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        // Customer Address
        customer cust = new customer();
        cust = _db.customers.Single(c => c.customer_id == ncid);
        string strCustomer = cust.first_name1 + " " + cust.last_name1;
        string strAddress = cust.address + " </br>" + cust.city + ", " + cust.state + " " + cust.zip_code;
        string address = cust.address + ",+" + cust.city + ",+" + cust.state + ",+" + cust.zip_code;
        string strPO = "";

        if (Convert.ToInt32(hdnEstimateId.Value) > 0)
        {
            customer_estimate cus_est = new customer_estimate();
            cus_est = _db.customer_estimates.Single(ce => ce.customer_id == Convert.ToInt32(hdnCustomerId.Value) && ce.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && ce.estimate_id == Convert.ToInt32(hdnEstimateId.Value));
            strPO = cus_est.job_number;
        }

        string url = "http://maps.google.com/maps?f=q&source=s_q&hl=en&geocode=&q=" + address;

        DataView dvFinal = dt.DefaultView;
        dvFinal.Sort = "location_name,week_id";
        string strHTML = "<table width='680' border='0' cellspacing='0'cellpadding='0' align='center'> <tr><td align='left' valign='top'><p style='color:#000; font-size:16px; font-weight:bold; font-family:Georgia, 'Times New Roman', Times, serif; font-style:italic; padding:5px 0 0; margin:0 auto;'>Customer Name: " + strCustomer + "</p> </td></tr>";
        strHTML += "<tr><td align='left' valign='top'><p style='color:#000; font-size:16px; font-weight:bold; font-family:Georgia, 'Times New Roman', Times, serif; font-style:italic; padding:5px 0 0; margin:0 auto;'><a style='color:#000;' target='_blank' href='" + url + "'>Address: " + strAddress + "</a></p> </td></tr>";
        strHTML += "<tr><td align='left' valign='top'><p style='color:#000; font-size:16px; font-weight:bold; font-family:Georgia, 'Times New Roman', Times, serif; font-style:italic; padding:5px 0 0; margin:0 auto;'>Job Number: " + strPO + "</p> </td></tr>";
        strHTML += " <tr style='background-color:#330f02; color:#FFFFFF; font-weight:bold; font-size:12px; font-family:Arial, Helvetica, sans-serif;'><td align='left' valign='top'><table width='100%' border='0' cellspacing='1' cellpadding='5' > <tr style='background-color:#330f02; color:#FFFFFF; font-weight:bold; font-size:12px; font-family:Arial, Helvetica, sans-serif;'> <td width='6%'>SL</td><td  width='12%' >Section</td><td width='12%'>Location</td><td width='40%'>Item Name</td><td width='5%'>UoM</td><td align='right'width='5%'>Code</td><td width='20%'>Short Notes</td></tr>";

        for (int i = 0; i < dvFinal.Count; i++)
        {
            DataRowView dr = dvFinal[i];

            string strColor = "";

            if (i % 2 == 0)
                strColor = "background-color:#f0eae8; color:#333333; font-weight:normal; font-size:12px; font-family:Arial, Helvetica, sans-serif;";
            else
                strColor = "background-color:#faf8f6; color:#333333; font-weight:normal; font-size:12px; font-family:Arial, Helvetica, sans-serif;";

            strHTML += "<tr style='" + strColor + "'><td>" + dr["section_serial"].ToString() + "</td><td>" + dr["section_name"].ToString() + "</td><td>" + dr["location_name"].ToString() + "</td><td>" + dr["item_name"].ToString() + "</td><td>" + dr["measure_unit"].ToString() + "</td><td align='right'>" + Convert.ToInt32(dr["quantity"]).ToString() + "</td><td>" + dr["short_notes"].ToString() + "</td></tr>";

        }
        strHTML += "</table> </td></tr>";
        strHTML += "</table>";

        return strHTML;
    }
    protected void imgSend_Click(object sender, ImageClickEventArgs e)
    {

        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, imgSend.ID, imgSend.GetType().Name, "Click");


        DataClassesDataContext _db = new DataClassesDataContext();

        string strFromEmail = txtFrom.Text;
        string strToEmail = txtTo.Text;
        string strCCEmail = txtCc.Text;
        string strCC2Email = txtCc2.Text;
        string strBCCEmail = txtBcc.Text;
        string strFrom = txtFrom.Text;

        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

        if (strFromEmail.Length > 4)
        {

            Match match1 = regex.Match(strFromEmail.Trim());
            if (!match1.Success)
            {
                lblMessage.Text = csCommonUtility.GetSystemRequiredMessage("From email address " + strFromEmail + " is not in correct format, Please enter valid email address (Ex: john@domain.com)");
                return;

            }
            else
            {




            }
        }
        else
        {
            lblMessage.Text = csCommonUtility.GetSystemRequiredMessage("From email address is a required field");
            return;

        }

        if (strToEmail.Length > 4)
        {
            string[] strIds = strToEmail.Split(',');
            foreach (string strId in strIds)
            {
                Match match1 = regex.Match(strId.Trim());
                if (!match1.Success)
                {
                    lblMessage.Text = csCommonUtility.GetSystemRequiredMessage("Recipient email address " + strId + " is not in correct format, Please enter valid email address (Ex: john@domain.com)");
                    return;

                }
            }
        }
        else
        {
            lblMessage.Text = csCommonUtility.GetSystemRequiredMessage("Recipient email address is a required field");
            return;

        }

        if (strCCEmail.Length > 4)
        {
            string[] strCCIds = strCCEmail.Split(',');
            foreach (string strCCId in strCCIds)
            {
                Match match1 = regex.Match(strCCId.Trim());
                if (!match1.Success)
                {
                    lblMessage.Text = csCommonUtility.GetSystemRequiredMessage("CC email address " + strCCId + " is not in correct format, Please enter valid email address (Ex: john@domain.com)");
                    return;

                }
            }
        }
        if (strCC2Email.Length > 4)
        {
            string[] strCC2Ids = strCC2Email.Split(',');
            foreach (string strCC2Id in strCC2Ids)
            {
                Match match1 = regex.Match(strCC2Id.Trim());
                if (!match1.Success)
                {
                    lblMessage.Text = csCommonUtility.GetSystemRequiredMessage("CC (Email 2) email address " + strCC2Id + " is not in correct format, Please enter valid email address (Ex: john@domain.com)");
                    return;

                }
            }
        }
        if (strBCCEmail.Length > 4)
        {
            string[] strBCCIds = strBCCEmail.Split(',');
            foreach (string strBCCId in strBCCIds)
            {
                Match match1 = regex.Match(strBCCId.Trim());
                if (!match1.Success)
                {
                    lblMessage.Text = csCommonUtility.GetSystemRequiredMessage("BCC email address " + strBCCId + " is not in correct format, Please enter valid email address (Ex: john@domain.com)");
                    return;

                }
            }
        }

        // if (txtSubject.Text.Length > 0)
        //{
        //    lblMessage.Text = csCommonUtility.GetSystemRequiredMessage("Subject is a required field");
        //    return;

        //}


        string strpath = Request.PhysicalApplicationPath + "Uploads\\";
        strpath = strpath + hdnCustomerId.Value + "\\Temp\\";
        string[] fileEntries = null;

        if (Directory.Exists(strpath))
        {
            fileEntries = Directory.GetFiles(strpath);
        }

        string strBody = txtBody.Content;

        try
        {
            string strUser = "";
            csEmailAPI email = new csEmailAPI();

            userinfo obj = new userinfo();

            int ProtocolType = 0;

            if ((userinfo)Session["oUser"] != null)
            {
                obj = (userinfo)Session["oUser"];
                hdnEmailType.Value = obj.EmailIntegrationType.ToString();
                ProtocolType = obj.EmailIntegrationType;
                strUser = obj.first_name + " " + obj.last_name;
            }

            email.CustomerId = Convert.ToInt32(hdnCustomerId.Value);
            email.ProtocolType = ProtocolType;
            email.From = strFrom;
            email.To = strToEmail;

            if (strCCEmail.Length > 0)
            {
                email.CC = strCCEmail;

                if (strCC2Email.Length > 0)
                {
                    email.CC += "," + strCC2Email;
                }
            }
            else if (strCC2Email.Length > 0)
            {
                email.CC = strCC2Email;
            }

            if (strBCCEmail.Length > 0)
            {
                email.BCC = strBCCEmail;
            }
            email.Subject = txtSubject.Text.ToString();
            email.Body = strBody;

            email.fileEntries = fileEntries;
            if (Request.QueryString.Get("pnote") != null)
                email.IsSaveEmailInDB = false;
            else
                email.IsSaveEmailInDB = true;
            email.UserName = strUser;
            email.NewAttachmentPath = Server.MapPath("~/Uploads//" + hdnCustomerId.Value + "//");

            email.SendEmail();



            if (Request.QueryString.Get("cofn") != null)
            {


                if (Request.QueryString.Get("afn") == null && Request.QueryString.Get("scfn") == null) //Babu
                {
                    string strQUP = "UPDATE changeorder_estimate SET is_cutomer_viewable=1 WHERE chage_order_id=" + hdnChEstId.Value + " AND estimate_id=" + hdnEstimateId.Value + " AND customer_id=" + hdnCustomerId.Value;
                    _db.ExecuteCommand(strQUP, string.Empty);
                }
                if (Request.QueryString.Get("scfn") != null) //Babu
                {
                    string strQUP = "UPDATE changeorder_estimate SET is_cutomer_viewable = 2,change_order_status_id = 1 WHERE chage_order_id=" + hdnChEstId.Value + " AND estimate_id=" + hdnEstimateId.Value + " AND customer_id=" + hdnCustomerId.Value;
                    _db.ExecuteCommand(strQUP, string.Empty);
                }

                string url = "change_order_worksheet.aspx?coestid=" + hdnChEstId.Value + "&eid=" + hdnEstimateId.Value + "&cid=" + hdnCustomerId.Value;
                string Script = @"<script language=JavaScript>window.close('" + url + "'); opener.document.forms[0].submit(); </script>";
                if (!IsClientScriptBlockRegistered("OpenFile"))
                    this.RegisterClientScriptBlock("OpenFile", Script);

            }
            else if (Request.QueryString.Get("calnotifyCustID") != null)
            {
                string url = "schedulecalendar.aspx?eid=" + hdnEstimateId.Value + "&cid=" + hdnCustomerId.Value + "&TypeID=1";
                string Script = @"<script language=JavaScript>window.close('" + url + "'); opener.document.forms[0].submit(); </script>";
                if (!IsClientScriptBlockRegistered("OpenFile"))
                    this.RegisterClientScriptBlock("OpenFile", Script);
            }
            else
            {

                Session.Add("FromEmailPage", "Yes");
                string url = "customer_details.aspx?cid=" + hdnCustomerId.Value;
                string Script = @"<script language=JavaScript>window.close('" + url + "'); opener.document.forms[0].submit(); </script>";
                if (!IsClientScriptBlockRegistered("OpenFile"))
                    this.RegisterClientScriptBlock("OpenFile", Script);

            }

        }
        catch (Exception ex)
        {
            lblMessage.Text = csCommonUtility.GetSystemErrorMessage(ex.Message);

        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnUpload.ID, btnUpload.GetType().Name, "Click");
        HttpFileCollection fileCollection = Request.Files;

        string DestinationPath = Server.MapPath("~/Uploads//" + hdnCustomerId.Value + "//Temp//");
        if (!System.IO.Directory.Exists(DestinationPath))
        {
            System.IO.Directory.CreateDirectory(DestinationPath);
        }

        for (int i = 0; i < fileCollection.Count; i++)
        {

            HttpPostedFile uploadfile = fileCollection[i];
            // string fileName = Path.GetFileNameWithoutExtension(uploadfile.FileName);
            string fileName = "";
            string fileExt = Path.GetExtension(uploadfile.FileName);
            if (uploadfile.ContentLength > 0)
            {
                fileName = uploadfile.FileName.Replace(fileExt, "") + "_" + DateTime.Now.Ticks.ToString() + fileExt;
                uploadfile.SaveAs(DestinationPath + fileName);

            }


        }
        LoadAttachment();
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "HideProgress", "HideProgress();", true);

    }

    private void LoadAttachment()
    {
        string DestinationPath = Server.MapPath("~/Uploads//" + hdnCustomerId.Value + "//Temp//");

        tdLink.Rows.Clear();

        if (System.IO.Directory.Exists(DestinationPath))
        {
            string[] fileEntries = Directory.GetFiles(DestinationPath);

            foreach (string file in fileEntries)
            {

                FileInfo fi = new FileInfo(file);
                fi.IsReadOnly = false;

                string FileName = Path.GetFileName(file);
                //File.Delete(Path.Combine(NewDir, FileName));
                TableRow row = new TableRow();
                TableCell cell = new TableCell();
                TableCell cellRemove = new TableCell();
                HyperLink hyp = new HyperLink();
                ImageButton btnRemove = new ImageButton();
                btnRemove.ImageUrl = "~/_scripts/remove_att.png";
                btnRemove.CssClass = "blindInput AttachmentsImg";
                btnRemove.Click += new System.Web.UI.ImageClickEventHandler(removeAttachment);

                btnRemove.ID = FileName;

                cell.BorderWidth = 0;
                try
                {
                    hyp.Text = FileName.Substring(0, FileName.IndexOf('_')).Trim() + Path.GetExtension(file);
                }
                catch
                {
                    hyp.Text = FileName;
                }
                hyp.NavigateUrl = "Uploads/" + hdnCustomerId.Value + "/" + "Temp" + "/" + FileName;
                hyp.Target = "_blank";
                cell.Controls.Add(hyp);
                cell.HorizontalAlign = HorizontalAlign.Left;
                row.Cells.Add(cell);
                cellRemove.Controls.Add(btnRemove);
                row.Cells.Add(cellRemove);
                tdLink.Rows.Add(row);

            }
        }

    }

    protected void removeAttachment(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        ImageButton btn = (ImageButton)sender;
        string FileName = btn.ID;

        string DestinationPath = Server.MapPath("~/Uploads//" + hdnCustomerId.Value + "//Temp//");
        if (Directory.Exists(DestinationPath))
        {
            string[] fileEntries = Directory.GetFiles(DestinationPath);
            foreach (string file in fileEntries)
            {
                if (Path.GetFileName(file).Equals(FileName))
                {
                    File.Delete(file);
                    break;
                }
            }

        }

        LoadAttachment();

    }

}