using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);
            LoadMenuList();
        }


    }

    public void LoadMenuList()
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        var menuList = _db.menu_items.OrderBy(m => m.menu_id).ToList();

        grdMenu.DataSource = menuList;
        grdMenu.DataBind();

        ddlParent.DataSource = menuList.Where(m => m.parent_id == 0).ToList();
        ddlParent.DataTextField = "menu_name";
        ddlParent.DataValueField = "menu_id";
        ddlParent.DataBind();
    }

    protected void btnUpdateCustomerPhoneNumber_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnUpdateCustomerPhoneNumber.ID, btnUpdateCustomerPhoneNumber.GetType().Name, "Click"); 
        lblMessage.Text = "";
        lblMessage2.Text = "";
        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            List<customer> CustomerList = _db.customers.ToList();


            foreach (customer c in CustomerList)
            {
                c.phone = csCommonUtility.GetPhoneFormat(c.phone ?? "");
                c.mobile = csCommonUtility.GetPhoneFormat(c.mobile ?? "");
                c.fax = csCommonUtility.GetPhoneFormat(c.fax ?? "");
            }
            _db.SubmitChanges();

            lblMessage2.Text = csCommonUtility.GetSystemMessage("Customer Data updated successfully.");
        }
        catch (Exception ex)
        {
            lblMessage2.Text = csCommonUtility.GetSystemErrorMessage("Customer: " + ex.Message);
        }
    }

    protected void btnUpdateUserPhoneNumber_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnUpdateUserPhoneNumber.ID, btnUpdateUserPhoneNumber.GetType().Name, "Click"); 
        lblMessage.Text = "";
        lblMessage2.Text = "";
        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            List<user_info> userList = _db.user_infos.ToList();


            foreach (user_info u in userList)
            {
                u.phone = csCommonUtility.GetPhoneFormat(u.phone ?? "");
                u.fax = csCommonUtility.GetPhoneFormat(u.fax ?? "");
            }
            _db.SubmitChanges();

            lblMessage2.Text = csCommonUtility.GetSystemMessage("User Data updated successfully.");
        }
        catch (Exception ex)
        {
            lblMessage2.Text = csCommonUtility.GetSystemErrorMessage("User: " + ex.Message);
        }
    }

    protected void btnUpdateSalesPersonPhoneNumber_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnUpdateSalesPersonPhoneNumber.ID, btnUpdateSalesPersonPhoneNumber.GetType().Name, "Click"); 
        lblMessage.Text = "";
        lblMessage2.Text = "";
        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            List<sales_person> spList = _db.sales_persons.ToList();


            foreach (sales_person s in spList)
            {
                s.phone = csCommonUtility.GetPhoneFormat(s.phone ?? "");
                s.fax = csCommonUtility.GetPhoneFormat(s.fax ?? "");
            }
            _db.SubmitChanges();

            lblMessage2.Text = csCommonUtility.GetSystemMessage("Sales Person Data updated successfully.");
        }
        catch (Exception ex)
        {
            lblMessage2.Text = csCommonUtility.GetSystemErrorMessage("Sales Person: " + ex.Message);
        }
    }


    protected void btnUpdateCustomerAppointment_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnUpdateCustomerAppointment.ID, btnUpdateCustomerAppointment.GetType().Name, "Click"); 
        lblMessage.Text = "";
        lblMessage2.Text = "";
        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            List<ScheduleCalendar> scList = _db.ScheduleCalendars.Where(s => s.type_id == 2).ToList();


            foreach (ScheduleCalendar sc in scList)
            {
                CustomerCallLog custCall = new CustomerCallLog();

                int empID = Convert.ToInt32(_db.customers.SingleOrDefault(c => c.customer_id == sc.customer_id).sales_person_id);

                custCall.CallSubject = sc.title;
                custCall.Description = sc.description;
                custCall.AppointmentDateTime = Convert.ToDateTime(sc.event_start);

                custCall.customer_id = sc.customer_id;
                custCall.CallDate = Convert.ToDateTime(sc.event_start).ToShortDateString();
                custCall.CallHour = Convert.ToDateTime(sc.event_start).ToString("hh", CultureInfo.InvariantCulture);
                custCall.CallMinutes = Convert.ToDateTime(sc.event_start).ToString("mm", CultureInfo.InvariantCulture);
                custCall.CallAMPM = Convert.ToDateTime(sc.event_start).ToString("tt", CultureInfo.InvariantCulture);

                custCall.CallDuration = "0";
                custCall.DurationHour = "0";

                string strCallDateTime = custCall.CallDate + " " + custCall.CallHour + ":" + custCall.CallMinutes + " " + custCall.CallAMPM;

                custCall.DurationMinutes = "0";

                custCall.CreatedByUser = sc.last_updated_by;
                custCall.CreateDate = sc.create_date;
                custCall.CallDateTime = Convert.ToDateTime(strCallDateTime);

                custCall.CallTypeId = 3;
                custCall.IsFollowUp = false;
                custCall.FollowDate = Convert.ToDateTime(sc.event_start).ToShortDateString(); ;
                custCall.FollowHour = Convert.ToDateTime(sc.event_start).ToString("hh", CultureInfo.InvariantCulture);
                custCall.FollowMinutes = Convert.ToDateTime(sc.event_start).ToString("mm", CultureInfo.InvariantCulture);
                custCall.FollowAMPM = Convert.ToDateTime(sc.event_start).ToString("tt", CultureInfo.InvariantCulture);

                string strFollowupDate = custCall.FollowDate + " " + custCall.FollowHour + ":" + custCall.FollowMinutes + " " + custCall.FollowAMPM;

                custCall.FollowDateTime = Convert.ToDateTime(strFollowupDate);
                custCall.IsDoNotCall = false;
                custCall.sales_person_id = empID;


                _db.CustomerCallLogs.InsertOnSubmit(custCall);
            }
            _db.SubmitChanges();

            lblMessage3.Text = csCommonUtility.GetSystemMessage("Data updated successfully.");
        }
        catch (Exception ex)
        {
            lblMessage3.Text = csCommonUtility.GetSystemErrorMessage("Appointment: " + ex.Message);
        }
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSend.ID, btnSend.GetType().Name, "Click"); 
        lblMessage.Text = "";
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/calendar/v3/calendars/faztrackclient@gmail.com/events/watch");
            request.Method = "POST";


            request.ContentType = "application/json";
            string postData = "type=web_hook&id=01234567-89ab-cdef-0123456789ab&address=https://awardkb.faztrack.com/schedulecalendar.aspx";
            byte[] bytes = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = bytes.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);

            var result = reader.ReadToEnd();
            stream.Dispose();
            reader.Dispose();
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = Color.Red;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnAdd.ID, btnAdd.GetType().Name, "Click"); 
        lblResultMenu.Text = "";

        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            menu_item objmList = new menu_item();

            objmList.menu_code = txtMenuCode.Text.Trim();
            objmList.menu_name = txtMenuName.Text.Trim();  
            objmList.parent_id = Convert.ToInt32(ddlParent.SelectedValue); 
            objmList.client_id = Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
            objmList.menu_url = txtMenuUrl.Text.Trim();   
            objmList.isShow = Convert.ToInt32(ddlisShow.SelectedValue);
            objmList.serial = Convert.ToInt32(txtSerial.Text);

            _db.menu_items.InsertOnSubmit(objmList);
            _db.SubmitChanges();
            lblResultMenu.Text = csCommonUtility.GetSystemMessage("Menu Item Added Successfully");

            LoadMenuList();
        }
        catch (Exception ex)
        {
            lblResultMenu.Text = csCommonUtility.GetSystemErrorMessage(ex.Message);
        }
    }
}