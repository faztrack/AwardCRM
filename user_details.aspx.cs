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
using Prabhu;

public partial class user_details : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);
            if (Session["oUser"] == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["LoginPage"].ToString());
            }
            if (Page.User.IsInRole("admin007") == false)
            {
                // No Permission Page.
                Response.Redirect("nopermission.aspx");
            }
            userinfo obj = (userinfo)Session["oUser"];
            int nuid = Convert.ToInt32(Request.QueryString.Get("uid"));
            hdnUserId.Value = nuid.ToString();

            BindStates();
            BindRoles();
            BindQuestions();

            lblEmailIntegration.Text = "Outlook/Exchange Email: ";
            lblEmailPassword.Text = "Outlook/Exchange Password: ";
            lblEmailPasswordCon.Text = "Outlook/Exchange Confirm Password: ";

            if (Convert.ToInt32(hdnUserId.Value) > 0)
            {
                lblHeaderTitle.Text = "User Details";

                DataClassesDataContext _db = new DataClassesDataContext();
                user_info uinfo = new user_info();
                uinfo = _db.user_infos.Single(c => c.user_id == Convert.ToInt32(hdnUserId.Value));
                lblChangePassword.Visible = true;
                lblPasswordRequ.Visible = false;
                lblReTypePasswordReq.Visible = false;
                txtFirstName.Text = uinfo.first_name;
                txtLastName.Text = uinfo.last_name;
                txtAddress.Text = uinfo.address;
                txtCity.Text = uinfo.city;
                ddlState.SelectedItem.Text = uinfo.state;
                txtZip.Text = uinfo.zip;
                txtPhone.Text = uinfo.phone;
                txtFax.Text = uinfo.fax;
                txtEmail.Text = uinfo.email;
                txtGoogleCalendarAccount.Text = uinfo.google_calendar_account;
                txtUsername.Text = uinfo.username;
                ddlQuestion.SelectedValue = uinfo.QuestionID.ToString();
                txtAnswer.Text = uinfo.Answer;
                //Company Email 
                //txtCompanyEmail.Text = uinfo.company_email;
                txtEmailIntegration.Text = uinfo.company_email;
                hdnEmailPassword.Value = uinfo.email_password;
                // txtEmailPassword.Text = uinfo.email_password;

                txtEMailSignature.Text = uinfo.EmailSignature.Replace("<br/>", "\n");


                BindLandingPage(Convert.ToInt32(uinfo.role_id));

                if (uinfo.menu_id != 0)
                    ddLandingPage.SelectedValue = uinfo.menu_id.ToString();

                ddlRole.SelectedValue = uinfo.role_id.ToString();
                chkIsActive.Checked = Convert.ToBoolean(uinfo.is_active);
                chkIsSales.Checked = Convert.ToBoolean(uinfo.is_sales);
                chkIsService.Checked = Convert.ToBoolean(uinfo.is_service);
                chkIsInstall.Checked = Convert.ToBoolean(uinfo.is_install);
                hdnSalesPersonId.Value = uinfo.sales_person_id.ToString();

                if (uinfo.IsTimeClock == true)
                {
                    chkIsTimeClock.Checked = true;
                }
                else
                {
                    chkIsTimeClock.Checked = false;
                }
                if (uinfo.IsPriceChange == true)
                {
                    IsPriceChange.Checked = true;
                }
                else
                {
                    IsPriceChange.Checked = false;
                }
                if (Convert.ToInt32(uinfo.EmailIntegrationType) == 1)
                {
                    chkEmailIntegrationType.Checked = true;

                    lblEmailIntegrationRequred.Visible = true;
                    lblEmailPasswordRequred.Visible = true;
                    lblEmailPasswordConRequred.Visible = true;
                }
                else
                {
                    chkEmailIntegrationType.Checked = false;

                    lblEmailIntegrationRequred.Visible = false;
                    lblEmailPasswordRequred.Visible = false;
                    lblEmailPasswordConRequred.Visible = false;
                }




                if (Convert.ToInt32(hdnSalesPersonId.Value) > 0)
                {
                    sales_person sp_info = new sales_person();
                    sp_info = _db.sales_persons.Single(c => c.sales_person_id == Convert.ToInt32(hdnSalesPersonId.Value));
                    if (sp_info.com_per != null)
                        txtCom.Text = Convert.ToDecimal(sp_info.com_per).ToString();
                    if (sp_info.co_com_per != null)
                        txtCOCom.Text = Convert.ToDecimal(sp_info.co_com_per).ToString();



                }
            }
            else
            {
                hdnUserId.Value = "0";
                hdnSalesPersonId.Value = "0";
                BindLandingPage(Convert.ToInt32(ddlRole.SelectedValue));

            }
            this.Validate();
        }
    }
    private void BindStates()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        var states = from st in _db.states
                     orderby st.abbreviation
                     select st;
        ddlState.DataSource = states;
        ddlState.DataTextField = "abbreviation";
        ddlState.DataValueField = "abbreviation";
        ddlState.DataBind();
    }

    private void BindQuestions()
    {
        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            var item = from q in _db.Questions
                       select q;
            ddlQuestion.DataSource = item.ToList();
            ddlQuestion.DataTextField = "QuestionName";
            ddlQuestion.DataValueField = "QuestionID";
            ddlQuestion.DataBind();
            ddlQuestion.SelectedValue = "10";
        }
        catch (Exception ex)
        {

        }
    }
    private void BindRoles()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        var roles = from ro in _db.roles
                    select ro;
        ddlRole.DataSource = roles;
        ddlRole.DataTextField = "role_name";
        ddlRole.DataValueField = "role_id";
        ddlRole.DataBind();
    }

    private void BindLandingPage(int RoleId)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        var query = from mi in _db.menu_items
                    join rr in _db.role_rights on mi.menu_id equals rr.menu_id
                    where rr.role_id == RoleId && mi.parent_id != 0 && mi.menu_url != "#" && mi.menu_id != 67
                    orderby mi.menu_name
                    select new { MenuName = mi.menu_name, MenuId = mi.menu_id };
        ddLandingPage.DataSource = query;
        ddLandingPage.DataTextField = "MenuName";
        ddLandingPage.DataValueField = "MenuId";
        ddLandingPage.DataBind();
        ddLandingPage.Items.Insert(0, "Select landing page");
        ddLandingPage.SelectedIndex = 0;


    }
    public static bool hasSpecialChar(string input)
    {
        string specialChar = @"\|!#$%&/()=?»«@£§€{}.-;'<>_,";
        foreach (var item in specialChar)
        {
            if (input.Contains(item)) return true;
        }

        return false;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSubmit.ID, btnSubmit.GetType().Name, "Click"); 
        lblResult.Text = "";
       
        DataClassesDataContext _db = new DataClassesDataContext();
        user_info uinfo = new user_info();
        sales_person obj = new sales_person();
        string strUserName = txtUsername.Text.Trim();

        if (txtFirstName.Text.Trim() == "")
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage("Missing required field: First Name.");
            return;
        }

        if (txtLastName.Text.Trim() == "")
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage("Missing required field: Last Name.");
            return;
        }

        if (txtEmail.Text.Trim() == "")
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage("Missing required field: " + lblEmail.Text.Replace(":", ".") + "");
            return;
        }

        if (txtUsername.Text.Trim() == "")
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage("Missing required field: User Name.");
            txtUsername.Focus();
            return;
        }

        if (strUserName.IndexOf(" ") != -1)
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage("Invalid format: 'space' is not allowed in User Name.");
            txtUsername.Focus();
            return;
 
        }
        if (hasSpecialChar(strUserName))
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage("Invalid format: any special characters is not allowed in User Name.");
            txtUsername.Focus();
            return;
 
        }

        if (ddlQuestion.SelectedValue == "10")
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage("Please select a Password Verification Question.");
            return;
        }

        if (txtAnswer.Text.Trim() == "")
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage("Missing Answer.");
            return;
        }

        if (txtPassword.Text.Trim() != "")
        {
            if (txtPassword.Text.Trim().Length < 6)
            {
                lblResult.Visible = true;
                lblResult.Text = csCommonUtility.GetSystemErrorMessage("Password length should be minimum 6");
                return;
            }

            if (txtConfirmPass.Text.Trim() == "")
            {
                lblResult.Text = csCommonUtility.GetSystemErrorMessage("Missing required field: Confirm Password.");

                return;
            }
            if (txtPassword.Text.Trim() != txtConfirmPass.Text.Trim())
            {
                lblResult.Text = csCommonUtility.GetSystemErrorMessage("Please confirm password");
                return;
            }
        }

        if (Convert.ToInt32(hdnUserId.Value) > 0)
            uinfo = _db.user_infos.Single(c => c.user_id == Convert.ToInt32(hdnUserId.Value));
        else
        {
            if (_db.user_infos.Where(sp => sp.username == txtUsername.Text).SingleOrDefault() != null)
            {
                lblResult.Text = csCommonUtility.GetSystemErrorMessage("Username already exist. Please try another Username.");
                return;
            }
            if (txtGoogleCalendarAccount.Text != "")
            {
                if (_db.user_infos.Where(sp => sp.google_calendar_account == txtGoogleCalendarAccount.Text).SingleOrDefault() != null)
                {
                    lblResult.Text = csCommonUtility.GetSystemErrorMessage("Calendar Email already exist. Please try another Calendar Email.");
                    return;
                }
            }
        }

        if (txtGoogleCalendarAccount.Text != "")
        {
            if (_db.user_infos.Where(sp => sp.google_calendar_account == txtGoogleCalendarAccount.Text && sp.user_id != Convert.ToInt32(hdnUserId.Value) && sp.is_active == true).SingleOrDefault() != null)
            {
                lblResult.Text = csCommonUtility.GetSystemErrorMessage("Calendar Email already exist. Please try another Calendar Email.");
                return;
            }
        }



        if (chkEmailIntegrationType.Checked)
        {
            if (txtEmailIntegration.Text != "")
            {
                if (_db.user_infos.Where(sp => sp.company_email == txtEmailIntegration.Text && sp.user_id != Convert.ToInt32(hdnUserId.Value)).FirstOrDefault() != null)
                {
                    lblResult.Text = csCommonUtility.GetSystemErrorMessage(lblEmailIntegration.Text.Replace(":", "").Trim() + " Email already exist. Please try another " + lblEmailIntegration.Text.Replace(":", "").Trim() + " Email.");
                    return;
                }

                if (_db.user_infos.Where(sp => sp.company_email == txtEmailIntegration.Text && sp.user_id == Convert.ToInt32(hdnUserId.Value)).SingleOrDefault() != null)
                {
                    if (txtEmailPassword.Text != "")
                    {
                        if (txtEmailPassword.Text != hdnEmailPassword.Value)
                        {
                            if (txtEmailPasswordCon.Text.Trim() == "")
                            {
                                lblResult.Text = csCommonUtility.GetSystemErrorMessage("Missing required field: Outlook/Exchange Confirm Password.");
                                return;
                            }
                            if (txtEmailPassword.Text.Trim() != txtEmailPasswordCon.Text.Trim())
                            {
                                lblResult.Text = csCommonUtility.GetSystemErrorMessage("Please confirm Outlook/Exchange Password");
                                return;
                            }

                            if (EWSAPI.DoesUserExistInOutlookServer(txtEmailIntegration.Text, txtEmailPassword.Text.Trim()) == false)
                            {
                                lblResult.Text = csCommonUtility.GetSystemErrorMessage(lblEmailIntegration.Text.Replace(":", "").Trim() + " or Password does not match in the Outlook / Exchange server.");
                                return;
                            }

                        }
                    }
                }
                else // New outook entry
                {
                    if (txtEmailPassword.Text == "")
                    {
                        lblResult.Text = csCommonUtility.GetSystemErrorMessage("Missing required field: " + lblEmailIntegration.Text.Replace(":", "").Trim() + " Password.");
                        return;
                    }
                    if (txtEmailPasswordCon.Text.Trim() == "")
                    {
                        lblResult.Text = csCommonUtility.GetSystemErrorMessage("Missing required field: Outlook/Exchange Confirm Password.");
                        return;
                    }
                    if (txtEmailPassword.Text.Trim() != txtEmailPasswordCon.Text.Trim())
                    {
                        lblResult.Text = csCommonUtility.GetSystemErrorMessage("Please confirm Outlook/Exchange Password");
                        return;
                    }

                    if (EWSAPI.DoesUserExistInOutlookServer(txtEmailIntegration.Text, txtEmailPassword.Text.Trim()) == false)
                    {
                        lblResult.Text = csCommonUtility.GetSystemErrorMessage(lblEmailIntegration.Text.Replace(":", "").Trim() + " does not exist in the Outlook / Exchange server.");
                        return;
                    }
                }


            }
            else
            {
                lblResult.Text = csCommonUtility.GetSystemErrorMessage("Missing required field: " + lblEmailIntegration.Text.Replace(":", "").Trim() + ".");
                return;
            }
        }

        if (chkIsTimeClock.Checked == true)
        {
            uinfo.IsTimeClock = true;
        }
        else
        {
            uinfo.IsTimeClock = false;
        }
        if (IsPriceChange.Checked)
        {
            uinfo.IsPriceChange = true;
        }
        else
        {
            uinfo.IsPriceChange = false;
        }
        txtPhone.Text = csCommonUtility.GetPhoneFormat(txtPhone.Text.Trim());
        txtFax.Text = csCommonUtility.GetPhoneFormat(txtFax.Text.Trim());

        uinfo.client_id = Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
        uinfo.user_id = Convert.ToInt32(hdnUserId.Value);

        uinfo.first_name = txtFirstName.Text;
        uinfo.last_name = txtLastName.Text;
        uinfo.address = txtAddress.Text;
        uinfo.city = txtCity.Text;
        uinfo.state = ddlState.SelectedItem.Text;
        uinfo.zip = txtZip.Text;
        uinfo.phone = txtPhone.Text;
        uinfo.fax = txtFax.Text;

        uinfo.email = txtEmail.Text;  //txtCompanyEmail.Text;

        if (chkEmailIntegrationType.Checked)
            uinfo.EmailIntegrationType = 1; // Outlook = 1
        else
            uinfo.EmailIntegrationType = 2; // Others = 2

        uinfo.is_active = chkIsActive.Checked;
        uinfo.is_sales = chkIsSales.Checked;
        uinfo.is_service = chkIsService.Checked;
        uinfo.is_install = chkIsInstall.Checked;
        uinfo.username = txtUsername.Text;

        uinfo.company_email = txtEmailIntegration.Text.Trim(); // Intregretion Email
        if (txtEmailPassword.Text != "")
            uinfo.email_password = txtEmailPassword.Text; // Intregretion Email Password

        uinfo.google_calendar_account = txtGoogleCalendarAccount.Text;


        uinfo.role_id = Convert.ToInt32(ddlRole.SelectedValue);

        uinfo.QuestionID = Convert.ToInt32(ddlQuestion.SelectedValue);
        uinfo.Answer = txtAnswer.Text.Trim();

        uinfo.EmailSignature = txtEMailSignature.Text.Replace("\n", "<br/>");

        if (ddLandingPage.SelectedValue == "Select landing page")
        {
            uinfo.menu_id = 0;
        }
        else
        {
            uinfo.menu_id = Convert.ToInt32(ddLandingPage.SelectedValue);
        }

        // Sales person Info
        obj.client_id = Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
        obj.sales_person_id = Convert.ToInt32(hdnSalesPersonId.Value);

        obj.first_name = txtFirstName.Text;
        obj.last_name = txtLastName.Text;
        obj.address = txtAddress.Text;
        obj.city = txtCity.Text;
        obj.state = ddlState.SelectedItem.Text;
        obj.zip = txtZip.Text;
        obj.phone = txtPhone.Text;
        obj.fax = txtFax.Text;
        obj.email = txtEmail.Text;  //txtCompanyEmail.Text;
        obj.google_calendar_account = txtGoogleCalendarAccount.Text;
        obj.is_active = chkIsActive.Checked;
        obj.is_sales = chkIsSales.Checked;
        obj.is_service = chkIsService.Checked;
        obj.is_install = chkIsInstall.Checked;
        obj.role_id = Convert.ToInt32(ddlRole.SelectedValue);
        obj.com_per = Convert.ToDecimal(txtCom.Text.Replace("%", "").Replace("$", ""));
        obj.co_com_per = Convert.ToDecimal(txtCOCom.Text.Replace("%", "").Replace("$", ""));
        string strEmpC1 = string.Empty;
        string strEmpC2 = string.Empty;
        string strEmpC3 = string.Empty;
        try
        {
            strEmpC1 = txtFirstName.Text.Trim().Substring(0, 1) + "" + txtLastName.Text.Trim().Substring(0, 1);
            strEmpC2 = txtFirstName.Text.Trim().Substring(0, 2) + "" + txtLastName.Text.Trim().Substring(0, 1);
            strEmpC3 = txtFirstName.Text.Trim().Substring(0, 2) + "" + txtLastName.Text.Trim().Substring(0, 2);
        }
        catch
        {

        }


        if (Convert.ToInt32(hdnUserId.Value) == 0)
        {


            if (txtPassword.Text.Trim() == "")
            {
                lblResult.Text = csCommonUtility.GetSystemErrorMessage("Missing required field: Password.");
                return;
            }
            else
            {
                if (txtPassword.Text.Trim().Length < 6)
                {
                    lblResult.Visible = true;
                    lblResult.Text = csCommonUtility.GetSystemErrorMessage("Password length should be minimum 6");
                    return;
                }
            }
            if (txtConfirmPass.Text.Trim() == "")
            {
                lblResult.Text = csCommonUtility.GetSystemErrorMessage("Missing required field: Confirm Password.");
                return;
            }
            if (txtPassword.Text.Trim() != txtConfirmPass.Text.Trim())
            {
                lblResult.Text = csCommonUtility.GetSystemErrorMessage("Please confirm password");
                return;
            }
            if (chkIsSales.Checked || ddlRole.SelectedValue == "3")
            {
                if (Convert.ToInt32(hdnSalesPersonId.Value) == 0)
                {
                    string strEmpCode = string.Empty;
                    if (_db.sales_persons.Where(sp => sp.EmpCode == strEmpC1.ToUpper()).SingleOrDefault() == null)
                    {
                        strEmpCode = strEmpC1.ToUpper();

                    }
                    else if (_db.sales_persons.Where(sp => sp.EmpCode == strEmpC2.ToUpper()).SingleOrDefault() == null)
                    {
                        strEmpCode = strEmpC2.ToUpper();

                    }
                    else if (_db.sales_persons.Where(sp => sp.EmpCode == strEmpC3.ToUpper()).SingleOrDefault() == null)
                    {
                        strEmpCode = strEmpC3.ToUpper();

                    }
                    else
                    {
                        strEmpCode = txtFirstName.Text.Trim().ToUpper();
                    }

                    int nSalePersonId = 0;
                    var result = (from sp in _db.sales_persons
                                  where sp.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                                  select sp.sales_person_id);

                    int n = result.Count();
                    if (result != null && n > 0)
                        nSalePersonId = result.Max();
                    nSalePersonId = nSalePersonId + 1;
                    hdnSalesPersonId.Value = nSalePersonId.ToString();
                    obj.sales_person_id = Convert.ToInt32(hdnSalesPersonId.Value);
                    obj.create_date = Convert.ToDateTime(DateTime.Now);
                    obj.last_login_time = Convert.ToDateTime(DateTime.Now);
                    obj.com_per = Convert.ToDecimal(txtCom.Text.Replace("%", "").Replace("$", ""));
                    obj.co_com_per = Convert.ToDecimal(txtCOCom.Text.Replace("%", "").Replace("$", ""));
                    obj.EmpCode = strEmpCode;
                    _db.sales_persons.InsertOnSubmit(obj);
                    _db.SubmitChanges();
                    hdnSalesPersonId.Value = obj.sales_person_id.ToString();
                }
                else
                {
                    string strQ = "UPDATE sales_person SET first_name='" + obj.first_name + "',last_name='" + obj.last_name + "',address='" + obj.address + "',city='" + obj.city + "', state='" + obj.state + "', zip='" + obj.zip + "',phone='" + obj.phone + "',fax='" + obj.fax + "',email='" + obj.email + "', role_id=" + obj.role_id + ", is_active='" + obj.is_active + "',is_sales='" + obj.is_sales + "',is_service='" + obj.is_service + "',is_install='" + obj.is_install + "',client_id=" + obj.client_id + ", com_per =" + Convert.ToDecimal(txtCom.Text.Replace("%", "").Replace("$", "")) + ", co_com_per =" + Convert.ToDecimal(txtCOCom.Text.Replace("%", "").Replace("$", "")) + "  WHERE sales_person_id =" + obj.sales_person_id + " AND client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
                    _db.ExecuteCommand(strQ, string.Empty);

                }
            }


            uinfo.google_calendar_id = "";
            uinfo.is_verify = false;
            uinfo.sales_person_id = Convert.ToInt32(hdnSalesPersonId.Value);
            uinfo.create_date = Convert.ToDateTime(DateTime.Now);
            uinfo.last_login_time = Convert.ToDateTime(DateTime.Now);
            uinfo.password = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPassword.Text.Trim(), "SHA1");
            // uinfo.sales_head_id = 0;
            _db.user_infos.InsertOnSubmit(uinfo);
            lblResult.Text = csCommonUtility.GetSystemMessage("Data saved successfully.");

        }
        else
        {
            if (chkIsSales.Checked || ddlRole.SelectedValue == "3")
            {
                if (Convert.ToInt32(hdnSalesPersonId.Value) == 0)
                {
                    string strEmpCode = string.Empty;
                    if (_db.sales_persons.Where(sp => sp.EmpCode == strEmpC1.ToUpper()).SingleOrDefault() == null)
                    {
                        strEmpCode = strEmpC1.ToUpper();

                    }
                    else if (_db.sales_persons.Where(sp => sp.EmpCode == strEmpC2.ToUpper()).SingleOrDefault() == null)
                    {
                        strEmpCode = strEmpC2.ToUpper();

                    }
                    else if (_db.sales_persons.Where(sp => sp.EmpCode == strEmpC3.ToUpper()).SingleOrDefault() == null)
                    {
                        strEmpCode = strEmpC3.ToUpper();

                    }
                    else
                    {
                        strEmpCode = txtFirstName.Text.Trim().ToUpper();
                    }
                    int nSalePersonId = 0;
                    var result = (from sp in _db.sales_persons
                                  where sp.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                                  select sp.sales_person_id);

                    int n = result.Count();
                    if (result != null && n > 0)
                        nSalePersonId = result.Max();
                    nSalePersonId = nSalePersonId + 1;
                    hdnSalesPersonId.Value = nSalePersonId.ToString();
                    obj.sales_person_id = Convert.ToInt32(hdnSalesPersonId.Value);
                    obj.create_date = Convert.ToDateTime(DateTime.Now);
                    obj.last_login_time = Convert.ToDateTime(DateTime.Now);
                    obj.com_per = Convert.ToDecimal(txtCom.Text.Replace("%", "").Replace("$", ""));
                    obj.co_com_per = Convert.ToDecimal(txtCOCom.Text.Replace("%", "").Replace("$", ""));
                    obj.EmpCode = strEmpCode;
                    _db.sales_persons.InsertOnSubmit(obj);
                    _db.SubmitChanges();
                    hdnSalesPersonId.Value = obj.sales_person_id.ToString();
                }
                else
                {
                    string strQ = "UPDATE sales_person SET first_name='" + obj.first_name + "',last_name='" + obj.last_name + "',address='" + obj.address + "',city='" + obj.city + "', state='" + obj.state + "', zip='" + obj.zip + "',phone='" + obj.phone + "',fax='" + obj.fax + "',email='" + obj.email + "', role_id=" + obj.role_id + ", is_active='" + obj.is_active + "',is_sales='" + obj.is_sales + "',is_service='" + obj.is_service + "',is_install='" + obj.is_install + "',client_id=" + obj.client_id + " , com_per =" + Convert.ToDecimal(txtCom.Text.Replace("%", "").Replace("$", "")) + ",  co_com_per =" + Convert.ToDecimal(txtCOCom.Text.Replace("%", "").Replace("$", "")) + "   WHERE sales_person_id =" + obj.sales_person_id + " AND client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
                    _db.ExecuteCommand(strQ, string.Empty);

                }
            }
            uinfo.sales_person_id = Convert.ToInt32(hdnSalesPersonId.Value);
            if (txtPassword.Text.Trim() != "" && txtConfirmPass.Text.Trim() != "" && txtPassword.Text.Trim() == txtConfirmPass.Text.Trim())
            {
                uinfo.password = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPassword.Text.Trim(), "SHA1");
            }
            if (Convert.ToInt32(hdnSalesPersonId.Value) > 0)
            {
                string strQ = "UPDATE sales_person SET google_calendar_account='" + txtEmail.Text + "', is_active='" + obj.is_active + "' WHERE sales_person_id =" + obj.sales_person_id + " AND client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
                _db.ExecuteCommand(strQ, string.Empty);
            }
            lblResult.Text = csCommonUtility.GetSystemMessage("Data updated successfully.");

        }

        _db.SubmitChanges();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("user_management.aspx");
    }



    protected void chkEmailIntegrationType_CheckedChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, chkEmailIntegrationType.ID, chkEmailIntegrationType.GetType().Name, "CheckedChanged"); 
        if (chkEmailIntegrationType.Checked)
        {
            lblEmailIntegrationRequred.Visible = true;
            lblEmailPasswordRequred.Visible = true;
            lblEmailPasswordConRequred.Visible = true;
        }
        else
        {
            lblEmailIntegrationRequred.Visible = false;
            lblEmailPasswordRequred.Visible = false;
            lblEmailPasswordConRequred.Visible = false;
        }
    }
}
