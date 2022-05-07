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
using Prabhu;
using System.Net.Mail;
using System.Drawing;

public partial class customermessagecenteroutlook : System.Web.UI.Page
{
    protected int sCustId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);
            if (Session["oUser"] == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["LoginPage"].ToString());
            }
            if (Page.User.IsInRole("admin035") == false)
            {
                // No Permission Page.
                Response.Redirect("nopermission.aspx");
            }
            int ncid = Convert.ToInt32(Request.QueryString.Get("cid"));
            sCustId = Convert.ToInt32(Request.QueryString.Get("cid"));
            hdnCustomerId.Value = ncid.ToString();
            HyperLink1.Attributes.Add("onClick", "DisplayWindowMessage();");
            int nEstid = Convert.ToInt32(Request.QueryString.Get("eid"));
            hdnEstimateId.Value = nEstid.ToString();
           

            DataClassesDataContext _db = new DataClassesDataContext();
            if (Convert.ToInt32(hdnCustomerId.Value) > 0)
            {
                Session.Add("CustomerId", hdnCustomerId.Value);

                customer cust = new customer();
                cust = _db.customers.Single(c => c.customer_id == Convert.ToInt32(hdnCustomerId.Value));

                lblCustomerName.Text = cust.first_name1 + " " + cust.last_name1;

                string strAddress = "";
                strAddress = cust.address + " </br>" + cust.city + ", " + cust.state + " " + cust.zip_code;
                lblAddress.Text = strAddress;
                lblEmail.Text = cust.email;
                lblPhone.Text = cust.phone;

                hypGoogleMap.NavigateUrl = "GoogleMap.aspx?strAdd=" + strAddress.Replace("</br>", "");
                string address = cust.address + ",+" + cust.city + ",+" + cust.state + ",+" + cust.zip_code;
                hypGoogleMap.NavigateUrl = "http://maps.google.com/maps?f=q&source=s_q&hl=en&geocode=&q=" + address;

                company_profile comp = csCommonUtility.GetCompanyProfile();

                hdnCompanyEmail.Value = comp.email.ToString();
                hdnCompanyName.Value = comp.company_name.ToString();

                GetCustomerMessageInfo(Convert.ToInt32(hdnCustomerId.Value));
                var resultCount = (from ce in _db.customer_estimates
                                   where ce.customer_id == Convert.ToInt32(hdnCustomerId.Value) && ce.client_id == 1 && ce.status_id == 3
                                   select ce.estimate_id);
                int nEstCount = resultCount.Count();
                if (nEstCount == 0)
                {
                    trWelcome.Visible = false;

                    GetEstimate(Convert.ToInt32(hdnCustomerId.Value));
                    //GetEmail(0);
                    if (ddlEstimate.Items.Count > 0)
                    {
                        followup_message followMsg = new followup_message();
                        if (_db.followup_messages.Where(ep => ep.estimate_id == Convert.ToInt32(ddlEstimate.SelectedValue) && ep.mess_type_id == 1 && ep.customer_id == Convert.ToInt32(hdnCustomerId.Value)).SingleOrDefault() != null)
                        {
                            followMsg = _db.followup_messages.Single(ep => ep.estimate_id == Convert.ToInt32(ddlEstimate.SelectedValue) && ep.mess_type_id == 1 && ep.customer_id == Convert.ToInt32(hdnCustomerId.Value));
                            lblInitial.Text = "Initial eMail was sent on " + Convert.ToDateTime(followMsg.create_date).ToShortDateString();
                            lblInitial.CssClass = "imgBtnTxtGreen";
                            btnSendInitailMail.Visible = false;
                            lblfollow2.Text = "2nd eMail with follow-up scheduled on " + Convert.ToDateTime(followMsg.create_date).AddDays(3).ToShortDateString();
                            trfollow2.Visible = true;
                            if (_db.followup_messages.Where(ep => ep.estimate_id == Convert.ToInt32(ddlEstimate.SelectedValue) && ep.mess_type_id == 2 && ep.customer_id == Convert.ToInt32(hdnCustomerId.Value)).SingleOrDefault() != null)
                            {
                                followMsg = _db.followup_messages.Single(ep => ep.estimate_id == Convert.ToInt32(ddlEstimate.SelectedValue) && ep.mess_type_id == 2 && ep.customer_id == Convert.ToInt32(hdnCustomerId.Value));
                                lblfollow2.Text = "2nd eMail with follow-up was sent on " + Convert.ToDateTime(followMsg.create_date).ToShortDateString();
                                lblfollow2.CssClass = "imgBtnTxtGreen";
                                btnSend2FollowUp.Visible = false;
                                trfollow3.Visible = true;
                            }
                            else
                            {
                                trfollow3.Visible = false;

                            }
                            if (_db.followup_messages.Where(ep => ep.estimate_id == Convert.ToInt32(ddlEstimate.SelectedValue) && ep.mess_type_id == 3 && ep.customer_id == Convert.ToInt32(hdnCustomerId.Value)).SingleOrDefault() != null)
                            {
                                followMsg = _db.followup_messages.Single(ep => ep.estimate_id == Convert.ToInt32(ddlEstimate.SelectedValue) && ep.mess_type_id == 3 && ep.customer_id == Convert.ToInt32(hdnCustomerId.Value));
                                lblfollow3.Text = "3rd eMail with Coupon was sent on " + Convert.ToDateTime(followMsg.create_date).ToShortDateString();
                                lblfollow3.CssClass = "imgBtnTxtGreen";
                                btnSend3FollowUp.Visible = false;
                                trfollow3.Visible = true;
                            }

                        }
                        else
                        {
                            trfollow2.Visible = false;
                            trfollow3.Visible = false;

                        }
                    }
                }
                else
                {
                    followup_message followMsg = new followup_message();
                    trWelcome.Visible = true;
                    trfollow1.Visible = false;
                    trfollow2.Visible = false;
                    trfollow3.Visible = false;
                    lblPendingEst.Visible = false;
                    ddlEstimate.Visible = false;
                    if (_db.followup_messages.Where(ep => ep.mess_type_id == 4 && ep.customer_id == Convert.ToInt32(hdnCustomerId.Value)).SingleOrDefault() != null)
                    {
                        followMsg = _db.followup_messages.Single(ep => ep.mess_type_id == 4 && ep.customer_id == Convert.ToInt32(hdnCustomerId.Value));
                        lblWelcome.Text = "Welcome Email sent on " + Convert.ToDateTime(followMsg.create_date).ToShortDateString();
                        lblWelcome.CssClass = "imgBtnTxtGreen";
                        btnSendWelcome.Visible = false;
                    }
                }

            }
           
        }
        else
        {
            if (Session["FromEmailPage"] != null)
            {
                Session.Remove("FromEmailPage");
                GetCustomerMessageInfo(Convert.ToInt32(hdnCustomerId.Value));

            }
        }


    }

    

    private void GetEstimate(int nCustId)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        string strQ = "select * from customer_estimate where customer_id=" + nCustId + " and client_id= 1 and status_id != 3 order by estimate_id desc ";
        IEnumerable<customer_estimate_model> clist = _db.ExecuteQuery<customer_estimate_model>(strQ, string.Empty);
        ddlEstimate.DataSource = clist;
        ddlEstimate.DataTextField = "estimate_name";
        ddlEstimate.DataValueField = "estimate_id";
        ddlEstimate.DataBind();


    }
    private void GetCustomerMessageInfo(int nCustId)
    {
        if (nCustId > 0)
        {

            try
            {


                DSMessage dsMessageSent = new DSMessage();

                DataClassesDataContext _db = new DataClassesDataContext();
                var messList = (from mess_info in _db.customer_messages
                                where mess_info.customer_id == nCustId && mess_info.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                                orderby mess_info.cust_message_id descending
                                select mess_info).ToList();

                foreach (customer_message msg in messList)
                {
                    DSMessage.MessageRow mes = dsMessageSent.Message.NewMessageRow();



                    if (msg.HasAttachments == null)
                    {
                        string strQ = "select * from message_upolad_info where customer_id=" + nCustId + " and message_id=" + msg.message_id + " and client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
                        IEnumerable<message_upolad_info> list = _db.ExecuteQuery<message_upolad_info>(strQ, string.Empty);

                        string mess_file = "";
                        foreach (message_upolad_info message_upolad in list)
                        {
                            mess_file += message_upolad.mess_file_name.Replace("amp;", "").Trim() + ", "; ;
                        }
                        mess_file = mess_file.Trim().TrimEnd(',');

                        if (mess_file.Length > 0)
                        {
                            mes.HasAttachments = true;
                            mes.AttachmentList = mess_file.Trim().TrimEnd(',');


                        }
                        else
                        {
                            mes.AttachmentList = "";
                            mes.HasAttachments = false;// msg.HasAttachments;
                        }

                        msg.HasAttachments = mes.HasAttachments;
                        msg.AttachmentList = mes.AttachmentList;

                    }
                    else if (Convert.ToBoolean(msg.HasAttachments))
                    {

                        mes.HasAttachments = true;
                        mes.AttachmentList = msg.AttachmentList;


                    }
                    else
                    {
                        mes.HasAttachments = false;
                        mes.AttachmentList = "";
                    }

                    mes.From = msg.mess_from;
                    mes.To = msg.mess_to;
                    mes.IsRead = (bool)(msg.IsView ?? false);
                    mes.customer_id = nCustId.ToString();
                    mes.message_id = msg.message_id.ToString();
                    mes.create_date = (DateTime)msg.create_date;
                    if (msg.mess_subject != null)
                        mes.mess_subject = msg.mess_subject.ToString();
                    else
                        mes.mess_subject = "";
                    mes.last_view = (DateTime)msg.last_view;
                    mes.Protocol = msg.Protocol;
                    mes.Type = msg.Type;
                    mes.sent_by = msg.sent_by;
                    dsMessageSent.Message.AddMessageRow(mes);

                }

                _db.SubmitChanges();

                dsMessageSent.AcceptChanges();

                DataView dv = dsMessageSent.Tables[0].DefaultView;
                dv.Sort = "create_date DESC";
                grdCustomersMessage.DataSource = dv;
                grdCustomersMessage.DataKeyNames = new string[] { "customer_id", "message_id", "AttachmentList", "From", "To", "Type" };
                grdCustomersMessage.DataBind();




            }
            catch (Exception ex)
            {
                lblResult.Text = csCommonUtility.GetSystemErrorMessage(ex.Message);

            }

        }

    }


    protected void grdCustomersMessage_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                string script = "";

                string Attacheent = grdCustomersMessage.DataKeys[e.Row.RowIndex].Values[2].ToString();
                string MessId = grdCustomersMessage.DataKeys[e.Row.RowIndex].Values[1].ToString();
                int CustId = Convert.ToInt32(grdCustomersMessage.DataKeys[e.Row.RowIndex].Values[0].ToString());

                string From = grdCustomersMessage.DataKeys[e.Row.RowIndex].Values[3].ToString();
                string To = grdCustomersMessage.DataKeys[e.Row.RowIndex].Values[4].ToString();
                string Type = grdCustomersMessage.DataKeys[e.Row.RowIndex].Values[5].ToString();




                //if (e.Row.Cells[7].Text.Equals("True"))
                //    e.Row.Cells[7].Text = "Yes";
                //else
                //    e.Row.Cells[7].Text = "";


                HyperLink hypMessageDetails = (HyperLink)e.Row.FindControl("hypMessageDetails");
                hypMessageDetails.ToolTip = "Click on Message Details to view specific Message Details .";
                hypMessageDetails.Target = "MyWindow";


                script = String.Format("GetdatakeyValue1Old('{0}')", MessId.ToString());

                hypMessageDetails.Attributes.Add("onclick", script);
                if (Attacheent.Length > 0)
                {
                    HyperLink hypAttachment = (HyperLink)e.Row.FindControl("hypAttachment");
                    hypAttachment.Text = Attacheent;
                    hypAttachment.ToolTip = "Click on Message Details to view specific Message Details .";
                    hypAttachment.Target = "MyWindow";

                    hypAttachment.Attributes.Add("onclick", script);
                }





            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
    }

    private void SendInitialeMailwithestimateToCustomer(string strEstimate)
    {
        try
        {
            int nCustomerId = Convert.ToInt32(hdnCustomerId.Value);
            string strTable = "<table align='center' width='704px' border='0'>" + Environment.NewLine +
                    "<tr><td align='left'>Hello, </td></tr>" + Environment.NewLine +
                    "<tr><td align='left'></td></tr><tr><td align='left'></td></tr>" + Environment.NewLine +
                    "<tr><td align='left'>From all of us in " + csCommonUtility.GetCompanyProfile().company_name + ", we would like to thank you for giving us the opportunity to present you with an estimate. We look forward to providing you with our award winning craftsmanship and exceptional customer service. If you have any questions concerning your estimate, please do not hesitate to call us. </td></tr>" + Environment.NewLine +
                    "<tr><td align='left'></td></tr><tr><td align='left'></td></tr>" + Environment.NewLine +
                    "<tr><td align='left'>" + csCommonUtility.GetCompanyProfile().company_name + " is a family owned and operated, five star rated, licensed, bonded and insured contracting company. We specialize in whole house redesign, bathroom remodeling and kitchen remodeling. With an “A” rating with the BBB, we've been in business in the Valley for more than 14 years with a talented team of tradespeople, designers and project managers - voted year after year as the Valley's Best! </td></tr>" + Environment.NewLine +
                     "<tr><td align='left'></td></tr><tr><td align='left'></td></tr>" + Environment.NewLine +
                    "<tr><td align='left'>As a family business, we take pride in building lasting relationships with our clients and helping them build or redesign the homes they’ve always dreamed of. And now we look forward to adding you to our family of customers (more than 5,000 strong) we are so proud to serve. </td></tr>" + Environment.NewLine +
                    "<tr><td align='left'></td></tr><tr><td align='left'></td></tr>" + Environment.NewLine +
                    "<tr><td align='left'>Sincerely,</td></tr>" + Environment.NewLine +
                    "<tr><td align='left'>" + csCommonUtility.GetCompanyOwnerName() + "</td></tr>" + Environment.NewLine +
                    "<tr><td align='left'>" + csCommonUtility.GetCompanyProfile().company_name + "</td></tr>" + Environment.NewLine +
                    "<tr><td align='left'>Phone 1: " + csCommonUtility.GetCompanyProfile().phone + "</td></tr>" + Environment.NewLine +

                    "<tr><td align='left'></td></tr></table>";

            string strToEmail = lblEmail.Text;
            string strCCEmail = string.Empty;
            string strBCCEmail = ConfigurationManager.AppSettings["CustomerFinalizePaymentBCCEmail"].ToString();
            if (strToEmail.Length > 4)
            {
                DataClassesDataContext _db = new DataClassesDataContext();

                userinfo obj = new userinfo();
                int ProtocolType = 0;
                string strUser = "";
                string strFrom = hdnCompanyEmail.Value;

                if ((userinfo)Session["oUser"] != null)
                {
                    obj = (userinfo)Session["oUser"];
                    ProtocolType = obj.EmailIntegrationType;
                    strUser = obj.first_name + " " + obj.last_name;
                }

                csEmailAPI email = new csEmailAPI();

                email.From = strFrom;
                email.To = strToEmail.Trim();
                email.CC = strCCEmail.Trim();
                email.BCC = strBCCEmail.Trim();

                email.Subject = "Thank you from " + hdnCompanyName.Value;
                email.Body = strTable;

                email.UserName = "Info";
                email.IsSaveEmailInDB = true;
                email.ProtocolType = ProtocolType;
                email.CustomerId = nCustomerId;
                email.FollowUpMsgTypeId = 1;
                email.EstimateName = ddlEstimate.SelectedItem.Text;
                email.EstimateId = Convert.ToInt32(ddlEstimate.SelectedValue);
                email.CustomerName = lblCustomerName.Text;

                email.SendEmail();

                string strQ = "UPDATE customers SET status_id = 2 WHERE customer_id =" + nCustomerId; //FollowUp
                _db.ExecuteCommand(strQ, string.Empty);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }


    }



    private void Send2ndFollowupeMailToCustomer(string strEstimate)
    {
        try
        {
            int nCustomerId = Convert.ToInt32(hdnCustomerId.Value);
            string strTable = "<table align='center' width='704px' border='0'>" + Environment.NewLine +
                    "<tr><td align='left'>Hello from " + csCommonUtility.GetCompanyProfile().company_name + "! </td></tr>" + Environment.NewLine +
                    "<tr><td align='left'></td></tr><tr><td align='left'></td></tr>" + Environment.NewLine +
                    "<tr><td align='left'>We are just following-up to see if you are closer to making a decision on the estimate we sent you. We understand that it can be a challenge to find a competent partner to fulfill your home improvement needs. Your concerns are very important to us and we will do whatever it takes to make your project a success.</td></tr>" + Environment.NewLine +
                    "<tr><td align='left'></td></tr><tr><td align='left'></td></tr>" + Environment.NewLine +
                    "<tr><td align='left'>Please let us know if you have any questions. We look forward to hearing from you soon. </td></tr>" + Environment.NewLine +
                    "<tr><td align='left'></td></tr><tr><td align='left'></td></tr>" + Environment.NewLine +
                    "<tr><td align='left'>Sincerely,</td></tr>" + Environment.NewLine +
                    "<tr><td align='left'>" + csCommonUtility.GetCompanyOwnerName() + "</td></tr>" + Environment.NewLine +
                    "<tr><td align='left'>" + csCommonUtility.GetCompanyProfile().company_name + "</td></tr>" + Environment.NewLine +
                    "<tr><td align='left'>Phone 1: " + csCommonUtility.GetCompanyProfile().phone + "</td></tr>" + Environment.NewLine +

                    "<tr><td align='left'></td></tr></table>";

            string strToEmail = lblEmail.Text;
            string strCCEmail = string.Empty;
            string strBCCEmail = ConfigurationManager.AppSettings["CustomerFinalizePaymentBCCEmail"].ToString();
            if (strToEmail.Length > 4)
            {
                DataClassesDataContext _db = new DataClassesDataContext();

                userinfo obj = new userinfo();
                int ProtocolType = 0;
                string strUser = "";
                string strFrom = hdnCompanyEmail.Value;
                if ((userinfo)Session["oUser"] != null)
                {
                    obj = (userinfo)Session["oUser"];
                    ProtocolType = obj.EmailIntegrationType;
                    strUser = obj.first_name + " " + obj.last_name;
                }


                csEmailAPI email = new csEmailAPI();


                email.From = strFrom;
                email.To = strToEmail.Trim();
                email.CC = strCCEmail.Trim();
                email.BCC = strBCCEmail.Trim();
                email.Subject = "Follow-up from " + hdnCompanyName.Value;
                email.Body = strTable;

                email.UserName = "Info";
                email.IsSaveEmailInDB = true;
                email.ProtocolType = ProtocolType;
                email.CustomerId = nCustomerId;
                email.FollowUpMsgTypeId = 2;
                email.EstimateName = ddlEstimate.SelectedItem.Text;
                email.EstimateId = Convert.ToInt32(ddlEstimate.SelectedValue);
                email.CustomerName = lblCustomerName.Text;

                email.SendEmail();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void Send3rdFollowupWithCuponToCustomer(string strEstimate)
    {
        try
        {
            int nCustomerId = Convert.ToInt32(hdnCustomerId.Value);
            string strTable = "<table align='center' width='704px' border='0'>" + Environment.NewLine +
                "<tr><td align='left'>Hello from " + csCommonUtility.GetCompanyProfile().company_name + "!</td></tr>" + Environment.NewLine +
                "<tr><td align='left'></td></tr><tr><td align='left'></td></tr>" + Environment.NewLine +
                "<tr><td align='left'>Just checking in to see if you're one step closer to working with us. We understand that budget can be a concern in making a decision on which contractor to work with. In order to help ease your concern, we are including a coupon to put towards your estimate. This is a limited time offer valid for 30 days. </td></tr>" + Environment.NewLine +
                "<tr><td align='left'></td></tr><tr><td align='left'></td></tr>" + Environment.NewLine +
                "<tr><td align='left'>Please let us know if you'd like to take advantage of this offer and join the 5,000 satisfied customers that have worked with us. We look forward to hearing from you soon.</td></tr>" + Environment.NewLine +
                "<tr><td align='left'></td></tr><tr><td align='left'></td></tr>" + Environment.NewLine +
                "<tr><td align='left'>Sincerely,</td></tr>" + Environment.NewLine +
                "<tr><td align='left'>" + csCommonUtility.GetCompanyOwnerName() + "</td></tr>" + Environment.NewLine +
                "<tr><td align='left'>" + csCommonUtility.GetCompanyProfile().company_name + "</td></tr>" + Environment.NewLine +
                "<tr><td align='left'>Phone 1: " + csCommonUtility.GetCompanyProfile().phone + "</td></tr>" + Environment.NewLine +

                "<tr><td align='left'></td></tr></table>";

            string strToEmail = lblEmail.Text;
            string strCCEmail = string.Empty;
            string strBCCEmail = ConfigurationManager.AppSettings["CustomerFinalizePaymentBCCEmail"].ToString();
            if (strToEmail.Length > 4)
            {
                DataClassesDataContext _db = new DataClassesDataContext();

                userinfo obj = new userinfo();
                int ProtocolType = 0;
                string strUser = "";
                string strFrom = hdnCompanyEmail.Value;
                if ((userinfo)Session["oUser"] != null)
                {
                    obj = (userinfo)Session["oUser"];
                    ProtocolType = obj.EmailIntegrationType;
                    strUser = obj.first_name + " " + obj.last_name;
                }

                csEmailAPI email = new csEmailAPI();


                email.From = strFrom;
                email.To = strToEmail.Trim();
                email.CC = strCCEmail.Trim();
                email.BCC = strBCCEmail.Trim();
                email.Subject = "Follow-up from " + hdnCompanyName.Value;

                email.Body = strTable;

                email.UserName = "Info";
                email.IsSaveEmailInDB = true;
                email.ProtocolType = ProtocolType;
                email.CustomerId = nCustomerId;
                email.FollowUpMsgTypeId = 3;
                email.EstimateName = ddlEstimate.SelectedItem.Text;
                email.EstimateId = Convert.ToInt32(ddlEstimate.SelectedValue);
                email.CustomerName = lblCustomerName.Text;

                email.SendEmail();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private void SendWelcomeToCustomer()
    {
        try
        {

            int nCustomerId = Convert.ToInt32(hdnCustomerId.Value);
            string strTable = "<table align='center' width='704px' border='0'>" + Environment.NewLine +
                "<tr><td align='left'>Hello from " + csCommonUtility.GetCompanyProfile().company_name + "!</td></tr>" + Environment.NewLine +
                "<tr><td align='left'></td></tr><tr><td align='left'></td></tr>" + Environment.NewLine +
                "<tr><td align='left'>We're delighted that you've selected to work with us. We understand the gravity of trust you've imparted in us to help build your dream project and look forward to providing nothing short of exceptional customer service and fantastic craftsmanship.</td></tr>" + Environment.NewLine +
                "<tr><td align='left'></td></tr><tr><td align='left'></td></tr>" + Environment.NewLine +
                "<tr><td align='left'>Should you have any additional questions about your estimate, please reach out. In the meantime, please stay tuned for a phone call from one of our top notch project coordinators to go over next steps.</td></tr>" + Environment.NewLine +
                "<tr><td align='left'></td></tr><tr><td align='left'></td></tr>" + Environment.NewLine +
                 "<tr><td align='left'>We're honored to be a part of your project's process and can't wait to work with you!</td></tr>" + Environment.NewLine +
                "<tr><td align='left'></td></tr><tr><td align='left'></td></tr>" + Environment.NewLine +
                "<tr><td align='left'>With gratitude,</td></tr>" + Environment.NewLine +
                "<tr><td align='left'><img  src='" + csCommonUtility.GetProjectUrl() + "/" + "assets/HHI-Signature-VS.jpg" + "'></td></tr>" + Environment.NewLine +
                "<tr><td align='left'>" + csCommonUtility.GetCompanyOwnerName() + "</td></tr>" + Environment.NewLine +
                "<tr><td align='left'>" + csCommonUtility.GetCompanyProfile().company_name + "</td></tr>" + Environment.NewLine +
                "<tr><td align='left'>Phone 1: " + csCommonUtility.GetCompanyProfile().phone + "</td></tr>" + Environment.NewLine +
               "<tr><td align='left'><img  src='" + csCommonUtility.GetProjectUrl() + "/" + "assets/login_logo.png" + "'></td></tr>" + Environment.NewLine +
                "<tr><td align='left'></td></tr></table>";


            string strToEmail = lblEmail.Text;
            string strCCEmail = string.Empty;
            string strBCCEmail = ConfigurationManager.AppSettings["CustomerFinalizePaymentBCCEmail"].ToString();
            if (strToEmail.Length > 4)
            {
                DataClassesDataContext _db = new DataClassesDataContext();

                userinfo obj = new userinfo();
                int ProtocolType = 0;
                string strUser = "";
                string strFrom = hdnCompanyEmail.Value;
                if ((userinfo)Session["oUser"] != null)
                {
                    obj = (userinfo)Session["oUser"];
                    ProtocolType = obj.EmailIntegrationType;
                    strUser = obj.first_name + " " + obj.last_name;
                }

                csEmailAPI email = new csEmailAPI();

                email.From = strFrom;
                email.To = strToEmail.Trim();
                email.CC = strCCEmail.Trim();
                email.BCC = strBCCEmail.Trim();
                email.Subject = "Welcome from " + hdnCompanyName.Value;
                email.Body = strTable;

                email.UserName = "Info";
                email.IsSaveEmailInDB = true;
                email.ProtocolType = ProtocolType;
                email.CustomerId = nCustomerId;
                email.FollowUpMsgTypeId = 4;
                email.EstimateName = "";
                email.EstimateId = 0;
                email.CustomerName = lblCustomerName.Text;

                email.SendEmail();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void ddlEstimate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, ddlEstimate.ID, ddlEstimate.GetType().Name, "Change");

            DataClassesDataContext _db = new DataClassesDataContext();
            if (ddlEstimate.Items.Count > 0)
            {
                followup_message followMsg = new followup_message();
                if (_db.followup_messages.Where(ep => ep.estimate_id == Convert.ToInt32(ddlEstimate.SelectedValue) && ep.mess_type_id == 1 && ep.customer_id == Convert.ToInt32(hdnCustomerId.Value)).SingleOrDefault() != null)
                {
                    followMsg = _db.followup_messages.Single(ep => ep.estimate_id == Convert.ToInt32(ddlEstimate.SelectedValue) && ep.mess_type_id == 1 && ep.customer_id == Convert.ToInt32(hdnCustomerId.Value));
                    lblInitial.Text = "Initial eMail was sent on " + Convert.ToDateTime(followMsg.create_date).ToShortDateString();
                    lblInitial.CssClass = "imgBtnTxtGreen";
                    btnSendInitailMail.Visible = false;
                    lblfollow2.Text = "2nd eMail with follow-up scheduled on " + Convert.ToDateTime(followMsg.create_date).AddDays(3).ToShortDateString();
                    trfollow2.Visible = true;
                    if (_db.followup_messages.Where(ep => ep.estimate_id == Convert.ToInt32(ddlEstimate.SelectedValue) && ep.mess_type_id == 2 && ep.customer_id == Convert.ToInt32(hdnCustomerId.Value)).SingleOrDefault() != null)
                    {
                        followMsg = _db.followup_messages.Single(ep => ep.estimate_id == Convert.ToInt32(ddlEstimate.SelectedValue) && ep.mess_type_id == 2 && ep.customer_id == Convert.ToInt32(hdnCustomerId.Value));
                        lblfollow2.Text = "2nd eMail with follow-up was sent on " + Convert.ToDateTime(followMsg.create_date).ToShortDateString();
                        lblfollow2.CssClass = "imgBtnTxtGreen";
                        btnSend2FollowUp.Visible = false;
                        trfollow3.Visible = true;
                    }
                    else
                    {
                        trfollow3.Visible = false;

                    }
                    if (_db.followup_messages.Where(ep => ep.estimate_id == Convert.ToInt32(ddlEstimate.SelectedValue) && ep.mess_type_id == 3 && ep.customer_id == Convert.ToInt32(hdnCustomerId.Value)).SingleOrDefault() != null)
                    {
                        followMsg = _db.followup_messages.Single(ep => ep.estimate_id == Convert.ToInt32(ddlEstimate.SelectedValue) && ep.mess_type_id == 3 && ep.customer_id == Convert.ToInt32(hdnCustomerId.Value));
                        lblfollow3.Text = "3rd eMail with Coupon was sent on " + Convert.ToDateTime(followMsg.create_date).ToShortDateString();
                        lblfollow3.CssClass = "imgBtnTxtGreen";
                        btnSend3FollowUp.Visible = false;
                        trfollow3.Visible = true;
                    }

                }
                else
                {
                    trfollow2.Visible = false;
                    trfollow3.Visible = false;

                }
            }
        }
        catch (Exception ex)
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage(ex.Message);
        }

    }
    protected void btnSendInitailMail_Click(object sender, EventArgs e)
    {
        try
        {
            KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSendInitailMail.ID, btnSendInitailMail.GetType().Name, "Click");
            SendInitialeMailwithestimateToCustomer(ddlEstimate.SelectedItem.Text);
            lblResult.Text = csCommonUtility.GetSystemMessage("Initial eMail sent successfully");
            lblResult1.Text = csCommonUtility.GetSystemMessage("Initial eMail sent successfully");
            lblInitial.Text = "Initial eMail was sent on " + DateTime.Now.ToShortDateString();
            lblInitial.CssClass = "imgBtnTxtGreen";
            btnSendInitailMail.Visible = false;
            trfollow2.Visible = true;
            lblfollow2.Text = "2nd eMail with follow-up scheduled on " + DateTime.Now.AddDays(3).ToShortDateString();
            GetCustomerMessageInfo(Convert.ToInt32(hdnCustomerId.Value));
        }
        catch (Exception ex)
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage(ex.Message);
        }
    }
    protected void btnSend2FollowUp_Click(object sender, EventArgs e)
    {
        try
        {
            KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSend2FollowUp.ID, btnSend2FollowUp.GetType().Name, "Click");
            Send2ndFollowupeMailToCustomer(ddlEstimate.SelectedItem.Text);
            lblResult.Text = csCommonUtility.GetSystemMessage("2nd eMail with follow-up sent successfully");
            trfollow3.Visible = true;
            lblfollow2.Text = "2nd eMail with follow-up sent on " + DateTime.Now.ToShortDateString();
            lblfollow2.CssClass = "imgBtnTxtGreen";
            btnSend2FollowUp.Visible = false;
            GetCustomerMessageInfo(Convert.ToInt32(hdnCustomerId.Value));
        }
        catch (Exception ex)
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage(ex.Message);
        }
    }
    protected void btnSend3FollowUp_Click(object sender, EventArgs e)
    {
        try
        {
            KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSend3FollowUp.ID, btnSend3FollowUp.GetType().Name, "Click");
            Send3rdFollowupWithCuponToCustomer(ddlEstimate.SelectedItem.Text);
            lblResult.Text = csCommonUtility.GetSystemMessage("3rd eMail with Coupon sent successfully");
            lblResult2.Text = csCommonUtility.GetSystemMessage("3rd eMail with Coupon sent successfully");
            lblfollow3.Text = "3rd eMail with coupon sent on " + DateTime.Now.ToShortDateString();
            lblfollow3.CssClass = "imgBtnTxtGreen";
            btnSend3FollowUp.Visible = false;
            GetCustomerMessageInfo(Convert.ToInt32(hdnCustomerId.Value));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnSendWelcome_Click(object sender, EventArgs e)
    {
        try
        {
            KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSendWelcome.ID, btnSendWelcome.GetType().Name, "Click");
            SendWelcomeToCustomer();
            lblResult.Text = csCommonUtility.GetSystemMessage("Welcome Email sent successfully");
            lblWelcomeResult.Text = csCommonUtility.GetSystemMessage("Welcome Email sent successfully");
            lblWelcome.Text = "Welcome Email sent on " + DateTime.Now.ToShortDateString();
            lblWelcome.CssClass = "imgBtnTxtGreen";
            btnSendWelcome.Visible = false;
            GetCustomerMessageInfo(Convert.ToInt32(hdnCustomerId.Value));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
