using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class crewdetails : System.Web.UI.Page
{
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
            if (Page.User.IsInRole("timetrack002") == false)
            {
                // No Permission Page.
                Response.Redirect("nopermission.aspx");
            }

            int ncrid = Convert.ToInt32(Request.QueryString.Get("crid"));
            hdnCrewId.Value = ncrid.ToString();

            BindSections();
           
            if (Convert.ToInt32(hdnCrewId.Value) > 0)
            {
                lblHeaderTitle.Text = "Crew Details";
                DataClassesDataContext _db = new DataClassesDataContext();
                Crew_Detail objCrew = new Crew_Detail();
                objCrew = _db.Crew_Details.Single(c => c.crew_id == Convert.ToInt32(hdnCrewId.Value));

                txtFirstName.Text = objCrew.first_name;
                txtLastName.Text = objCrew.last_name;
               // txtAddress.Text = objCrew.Address;
               // txtCity.Text = objCrew.city;
               // ddlState.SelectedItem.Text = objCrew.state;
               // txtZip.Text = objCrew.zip_code;
                txtPhone.Text = objCrew.phone;
                //txtFax.Text = objCrew.fax;
               // txtEmailAddress.Text = objCrew.email;
                txtUsername.Text = objCrew.username.Trim();
                if (objCrew.is_active == true)
                {
                    chkIsActive.Checked = true;
                }
                else
                {
                    chkIsActive.Checked = false;
                }
                //txtPassword.Text = objCrew.password;
                txtPassword.Attributes.Add("value", objCrew.password);

                if (_db.crew_sections.Any(c => c.crew_Id == Convert.ToInt32(hdnCrewId.Value)))
                {
                    var item = _db.crew_sections.Where(c => c.crew_Id == Convert.ToInt32(hdnCrewId.Value)).ToList();
                    foreach (ListItem li in chkSections.Items)
                    {
                        foreach (crew_section vSec in item)
                        {
                            if (vSec.section_id == Convert.ToInt32(li.Value.ToString()))
                                li.Selected = true;
                        }
                    }
                }

            }
            else
            {
                lblHeaderTitle.Text = "Add New Crew";
             
            }

            this.Validate();
        }

    }

    private void BindSections()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        chkSections.DataSource = _db.sectioninfos.Where(si => si.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && si.parent_id == 0).OrderBy(s => s.section_name).ToList();
        chkSections.DataTextField = "section_name";
        chkSections.DataValueField = "section_id";
        chkSections.DataBind();
    }
  
   
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSubmit.ID, btnSubmit.GetType().Name, "Click"); 
        lblResult.Text = "";
       

        DataClassesDataContext _db = new DataClassesDataContext();
        Crew_Detail objCrew = new Crew_Detail();
        user_info objUser = new user_info();

        if (Convert.ToInt32(hdnCrewId.Value) > 0)
        {
            objCrew = _db.Crew_Details.Single(c => c.crew_id == Convert.ToInt32(hdnCrewId.Value));

            if (txtUsername.Text.Trim().ToLower() != objCrew.username.Trim().ToLower())
            {
                if (_db.Crew_Details.Where(v => v.username == txtUsername.Text.Trim()).SingleOrDefault() != null)
                {
                    lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Crew Username already exist. Please try another Crew Username.");
                    return;
                }
            }
            if (txtFirstName.Text.Trim() == "")
            {
                lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Missing required field: First Name.");
                return;
            }
            if (txtLastName.Text.Trim() == "")
            {
                lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Missing required field: Last Name.");
                return;
            }
            if (txtUsername.Text.Trim() == "")
            {
                lblResult.Text = csCommonUtility.GetSystemErrorMessage("Missing required field: User Name.");
                return;
            }
            else
            {
                if (txtUsername.Text.Trim().Contains(" "))
                {
                    lblResult.Text = csCommonUtility.GetSystemErrorMessage("Missing required field: User Name space is not valid.");
                    return;
                }

            }
            if (txtPassword.Text.Trim() != "")
            {
                if (txtPassword.Text.Trim().Length < 6)
                {
                    lblResult.Visible = true;
                    lblResult.Text = csCommonUtility.GetSystemErrorMessage("Password length should be minimum 6");
                    return;
                }

              
            }
        }
       


      
     decimal nLabor = 0;

      

        txtPhone.Text = csCommonUtility.GetPhoneFormat(txtPhone.Text.Trim());
       // txtFax.Text = csCommonUtility.GetPhoneFormat(txtFax.Text.Trim());

        objCrew.client_id = Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
        objCrew.crew_id = Convert.ToInt32(hdnCrewId.Value);
        objCrew.first_name = txtFirstName.Text;
        objCrew.last_name = txtLastName.Text;
       // objCrew.Address = txtAddress.Text;
        //objCrew.city = txtCity.Text;
       // objCrew.state = ddlState.SelectedItem.Text;
       // objCrew.zip_code = txtZip.Text;
        objCrew.phone = txtPhone.Text;
       // objCrew.fax = txtFax.Text;
       // objCrew.email = txtEmailAddress.Text;
        if (chkIsActive.Checked)
        {
            objCrew.is_active = true;
        }
        else
        {
            objCrew.is_active = false;
        }
      
        objCrew.full_name = txtFirstName.Text + " " + txtLastName.Text;
        objCrew.hourly_rate = nLabor;
        
        if (Convert.ToInt32(hdnCrewId.Value) == 0)
            {
                if (_db.Crew_Details.Where(v => v.username.Trim().ToLower() == txtUsername.Text.Trim().ToLower()).SingleOrDefault() != null)
                {
                    lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Crew username  exists already. Please use a different crew username..");
                    return;
                }
                if (_db.user_infos.Where(v => v.username.Trim().ToLower() == txtUsername.Text.Trim().ToLower()).SingleOrDefault() != null)
                {
                    lblResult.Text = csCommonUtility.GetSystemRequiredMessage("This username exists already.  Please use a different username..");
                    return;
                }

                if (txtFirstName.Text.Trim() == "")
                {
                    lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Missing required field: First Name.");
                    return;
                }
                if (txtLastName.Text.Trim() == "")
                {
                    lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Missing required field: Last Name.");
                    return;
                }
                if (txtUsername.Text.Trim() == "")
                {
                    lblResult.Text = csCommonUtility.GetSystemErrorMessage("Missing required field: User Name.");
                    return;
                }
                else
                {
                    if (txtUsername.Text.Trim().Contains(" "))
                    {
                        lblResult.Text = csCommonUtility.GetSystemErrorMessage("Missing required field: User Name space is not valid.");
                        return;
                    }

                }

                if (txtPassword.Text.Trim() == "")
                {
                    lblResult.Visible = true;
                    lblResult.Text = csCommonUtility.GetSystemErrorMessage("Missing Password");
                    return;
                }
                else
                {
                    if (txtPassword.Text.Trim() != "")
                    {
                        if (txtPassword.Text.Trim().Length < 6)
                        {
                            lblResult.Visible = true;
                            lblResult.Text = csCommonUtility.GetSystemErrorMessage("Password length should be minimum 6");
                            return;
                        }

                       
                    }
                }




             objCrew.username = txtUsername.Text.Trim();
             //objCrew.password = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPassword.Text.Trim(), "SHA1");
             objCrew.password = txtPassword.Text.Trim();
             objCrew.CreatedDate = DateTime.Now;
            _db.Crew_Details.InsertOnSubmit(objCrew);
            _db.SubmitChanges();
             lblResult.Text = csCommonUtility.GetSystemMessage("Data saved successfully.");


             //Section Details;

             int selectedCount = chkSections.Items.Cast<ListItem>().Count(li => li.Selected);
             if (selectedCount > 0)
             {
                 foreach (ListItem li in chkSections.Items)
                 {
                     crew_section objCrewSec = new crew_section();
                     if (li.Selected)
                     {
                         objCrewSec.section_id = Convert.ToInt32(li.Value);
                         objCrewSec.SectionName = li.Text;
                         objCrewSec.client_id = Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
                         objCrewSec.crew_Id = objCrew.crew_id; ;
                         objCrewSec.LastUpdateDate = DateTime.Now;
                         objCrewSec.UpdateBy = User.Identity.Name;

                         _db.crew_sections.InsertOnSubmit(objCrewSec);
                         _db.SubmitChanges();
                     }
                 }
             }
            




        }  
        else
        {
            // Existing Crew Update
            if (txtUsername.Text.Trim().ToLower() != objCrew.username.Trim().ToLower())
                objCrew.username = txtUsername.Text.Trim().ToLower();
            if (txtPassword.Text.Trim() != "")
                objCrew.password = txtPassword.Text.Trim();
               // objCrew.password = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPassword.Text.Trim(), "SHA1");
              lblResult.Text = csCommonUtility.GetSystemMessage("Data updated successfully.");



            //Section Details;

            // 
            string strQ = "DELETE crew_section WHERE crew_Id =" + Convert.ToInt32(hdnCrewId.Value) + " AND client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
            _db.ExecuteCommand(strQ, string.Empty);
             _db.SubmitChanges();

            int selectedCount = chkSections.Items.Cast<ListItem>().Count(li => li.Selected);
            if (selectedCount > 0)
            {
                foreach (ListItem li in chkSections.Items)
                {
                    crew_section objCrewSec = new crew_section();
                    if (li.Selected)
                    {
                        objCrewSec.section_id = Convert.ToInt32(li.Value);
                        objCrewSec.SectionName = li.Text;
                        objCrewSec.client_id = Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
                        objCrewSec.crew_Id = Convert.ToInt32(hdnCrewId.Value);
                        objCrewSec.LastUpdateDate = DateTime.Now;
                        objCrewSec.UpdateBy = User.Identity.Name;

                        _db.crew_sections.InsertOnSubmit(objCrewSec);
                        _db.SubmitChanges();
                    }
                }
            }
           

        }


       


      
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("crewlist.aspx");

    }
    protected void imgPasswordShow_Click(object sender, ImageClickEventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, imgPasswordShow.ID, imgPasswordShow.GetType().Name, "Click"); 
        DataClassesDataContext _db = new DataClassesDataContext();
        Crew_Detail objCrew = new Crew_Detail();
        imgPasswordShow.Visible = false;
        imgPasswordHide.Visible = true;

        if (Convert.ToInt32(hdnCrewId.Value) > 0)
        {
            objCrew = _db.Crew_Details.Single(c => c.crew_id == Convert.ToInt32(hdnCrewId.Value));
            txtPassword.Text = objCrew.password;
            txtPassword.TextMode = TextBoxMode.SingleLine;
        }
        else
        {
            txtPassword.TextMode = TextBoxMode.SingleLine;
        }
      
       
    }
    protected void imgPasswordHide_Click(object sender, ImageClickEventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, imgPasswordHide.ID, imgPasswordHide.GetType().Name, "Click"); 
        DataClassesDataContext _db = new DataClassesDataContext();
        Crew_Detail objCrew = new Crew_Detail();
        imgPasswordShow.Visible = true;
        imgPasswordHide.Visible = false;
        if (Convert.ToInt32(hdnCrewId.Value) > 0)
        {
            objCrew = _db.Crew_Details.Single(c => c.crew_id == Convert.ToInt32(hdnCrewId.Value));
            txtPassword.Text = objCrew.password;
            txtPassword.TextMode = TextBoxMode.Password;
        }
        else
        {
            txtPassword.Attributes.Add("value", txtPassword.Text);
            txtPassword.TextMode = TextBoxMode.Password;
        }

      
    }
}