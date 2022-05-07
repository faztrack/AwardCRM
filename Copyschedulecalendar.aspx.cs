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
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Drawing;

public partial class Copyschedulecalendar : System.Web.UI.Page
{
    //this method only updates title and description //this is called when a event is clicked on the calendar
    [System.Web.Services.WebMethod(true)]
    public static string UpdateEvent(ImproperCalendarEvent cevent)
    {
        var date = cevent.start.ToString();
        List<int> idList = (List<int>)System.Web.HttpContext.Current.Session["idList"];
        if (idList != null && idList.Contains(cevent.id))
        {
            if (CheckAlphaNumeric(cevent.title) && CheckAlphaNumeric(cevent.description))
            {
                CopyEventDAO.updateEvent(cevent.id, cevent.title, cevent.description,
                    DateTime.ParseExact(cevent.start.ToString(), "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                    DateTime.ParseExact(cevent.end.ToString(), "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                    cevent.employee_id, cevent.employee_name, cevent.child_event_id, cevent.dependencyType, cevent.offsetDays);

                //GridView gv = new GridView();
                //System.IO.StringWriter stringWriter = new System.IO.StringWriter();
                //HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
                //DataClassesDataContext _db = new DataClassesDataContext();
                //var item = from scTemp in _db.ScheduleCalendarTemps
                //           join linktemp in _db.ScheduleCalendarLinkTemps on scTemp.event_id equals linktemp.child_event_id
                //           where linktemp.customer_id == cevent.customer_id && linktemp.estimate_id == cevent.estimate_id && linktemp.parent_event_id == cevent.id
                //           select new
                //           {
                //               link_id = linktemp.link_id,
                //               title = scTemp.title,
                //               start = scTemp.event_start,
                //               end = scTemp.event_end
                //           };

                //gv.DataSource = item.ToList();
                //gv.DataKeyNames = new string[] { "link_id" };
                //gv.DataBind();

                //gv.RenderControl(htmlWriter);
                //return stringWriter.ToString();

                return cevent.id.ToString();

                //return "updated event with id:" + cevent.id + " update title to: " + cevent.title +
                //" update description to: " + cevent.description;
            }

        }

        return "unable to update event with id:" + cevent.id + " title : " + cevent.title +
            " description : " + cevent.description;
    }

    [System.Web.Services.WebMethod(true)]
    public static string CancelUpdateEventTime(ImproperCalendarEvent improperEvent)
    {
        try
        {

            String strdt = (String)System.Web.HttpContext.Current.Session["strdt"];
            DateTime dt = Convert.ToDateTime(strdt);
            string ndt = dt.ToString("dd-MM-yyyy hh:mm:ss tt");
            List<int> idList = (List<int>)System.Web.HttpContext.Current.Session["idList"];
            if (idList != null && idList.Contains(improperEvent.id))
            {
                CopyEventDAO.updateEventTime(improperEvent.id,
                    DateTime.ParseExact(ndt.ToString(), "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                    DateTime.ParseExact(ndt.ToString(), "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                      improperEvent.customer_id, improperEvent.estimate_id);

                return "updated event with id:" + improperEvent.id + "update start to: " + improperEvent.start +
                    " update end to: " + improperEvent.end;
            }
        }
        catch (Exception ex)
        {
        }
        return "unable to update event with id: " + improperEvent.id;
    }

    //this method only updates start and end time //this is called when a event is dragged or resized in the calendar
    [System.Web.Services.WebMethod(true)]
    public static string UpdateEventTime(ImproperCalendarEvent improperEvent)
    {
        List<int> idList = (List<int>)System.Web.HttpContext.Current.Session["idList"];
        if (idList != null && idList.Contains(improperEvent.id))
        {
            CopyEventDAO.updateEventTime(improperEvent.id,
                DateTime.ParseExact(improperEvent.start.ToString(), "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                DateTime.ParseExact(improperEvent.end.ToString(), "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                  improperEvent.customer_id, improperEvent.estimate_id);

            return "updated event with id:" + improperEvent.id + "update start to: " + improperEvent.start +
                " update end to: " + improperEvent.end;
        }

        return "unable to update event with id: " + improperEvent.id;
    }

    //this method only updates All events start and end time //this is called when a event is dragged or resized in the calendar
    [System.Web.Services.WebMethod(true)]
    public static string UpdateEventTimeAll(ImproperCalendarEvent improperEvent)
    {
        List<int> idList = (List<int>)System.Web.HttpContext.Current.Session["idList"];
        if (idList != null && idList.Contains(improperEvent.id))
        {
            //DataClassesDataContext _db = new DataClassesDataContext();
            //DateTime nStart = DateTime.ParseExact(improperEvent.start.ToString(), "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            //ScheduleCalendar objsc = _db.ScheduleCalendars.SingleOrDefault(sc => sc.event_id == improperEvent.id);
            //string strdt = objsc.event_start.ToString();
            //HttpContext.Current.Session.Add("strdt", strdt);

            CopyEventDAO.updateEventTimeAll(improperEvent.id,
                DateTime.ParseExact(improperEvent.start.ToString(), "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                DateTime.ParseExact(improperEvent.end.ToString(), "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                  improperEvent.customer_id, improperEvent.estimate_id);

            return "updated event with id:" + improperEvent.id + "update start to: " + improperEvent.start +
                " update end to: " + improperEvent.end;
        }

        return "unable to update event with id: " + improperEvent.id;
    }

    [System.Web.Services.WebMethod(true)]
    public static string UpdateEventNotes(ImproperCalendarEvent improperEvent)
    {
        List<int> idList = (List<int>)System.Web.HttpContext.Current.Session["idList"];
        if (idList != null && idList.Contains(improperEvent.id))
        {
            CopyEventDAO.UpdateEventNotes(improperEvent.id,
                 DateTime.ParseExact(improperEvent.start.ToString(), "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                DateTime.ParseExact(improperEvent.end.ToString(), "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                improperEvent.operation_notes);

            return "updated event with id:" + improperEvent.id;
        }

        return "unable to update event with id: " + improperEvent.id;
    }
    //called when delete button is pressed
    [System.Web.Services.WebMethod(true)]
    public static String deleteEvent(int id)
    {
        //idList is stored in Session by JsonResponse.ashx for security reasons
        //whenever any event is update or deleted, the event id is checked
        //whether it is present in the idList, if it is not present in the idList
        //then it may be a malicious user trying to delete someone elses events
        //thus this checking prevents misuse
        List<int> idList = (List<int>)System.Web.HttpContext.Current.Session["idList"];
        if (idList != null && idList.Contains(id))
        {
            CopyEventDAO.deleteEvent(id);
            return "deleted event with id:" + id;
        }

        return "unable to delete event with id: " + id;

    }

    //called when delete button is pressed
    [System.Web.Services.WebMethod(true)]
    public static String cancelEvent()
    {
        CopyEventDAO.cancelEvent();
        return "cancel event";
    }

    //called when Add button is clicked //this is called when a mouse is clicked on open space of any day or dragged  //over mutliple days
    [System.Web.Services.WebMethod]
    public static int addEvent(ImproperCalendarEvent improperEvent)
    {

        CalendarEvent cevent = new CalendarEvent()
        {
            title = improperEvent.title,
            description = improperEvent.description,
            start = DateTime.ParseExact(improperEvent.start, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
            end = DateTime.ParseExact(improperEvent.end, "dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
            customer_id = improperEvent.customer_id,
            estimate_id = improperEvent.estimate_id,
            employee_id = improperEvent.employee_id,
            employee_name = improperEvent.employee_name
        };

        if (CheckAlphaNumeric(cevent.title) && CheckAlphaNumeric(cevent.description))
        {
            int key = CopyEventDAO.addEvent(cevent);

            List<int> idList = (List<int>)System.Web.HttpContext.Current.Session["idList"];

            if (idList != null)
            {
                idList.Add(key);
            }

            return key;//return the primary key of the added cevent object

        }

        return -1;//return a negative number just to signify nothing has been added

    }
    [System.Web.Services.WebMethod]
    public static string GetEvent(int nCustId, string SectionName)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        HttpContext.Current.Session.Add("CusId", nCustId);
        HttpContext.Current.Session.Add("sSecName", SectionName);

        if (nCustId != 0 && SectionName != "")
        {
            var objSC = _db.ScheduleCalendars.Where(c => c.customer_id == nCustId && c.section_name == SectionName);
            var nEstimateID = objSC.Max(x => x.estimate_id);
            var date = objSC.Where(c => c.event_start >= DateTime.Today).Min(x => x.event_start);

            HttpContext.Current.Session.Add("sEstSelectedByCustSearch", nEstimateID);

            return date.ToString();
        }
        else if (nCustId != 0)
        {
            var objSC = _db.ScheduleCalendars.Where(c => c.customer_id == nCustId);
            var nEstimateID = objSC.Max(x => x.estimate_id);
            var date = objSC.Where(c => c.event_start >= DateTime.Today).Min(x => x.event_start);

            if (date == null)
                date = objSC.Max(x => x.event_start);

            HttpContext.Current.Session.Add("sEstSelectedByCustSearch", nEstimateID);

            return date.ToString();
        }
        else if (SectionName != "")
        {
            var date = _db.ScheduleCalendars.Where(c => c.section_name == SectionName && c.event_start >= DateTime.Now).Min(x => x.event_start);

            HttpContext.Current.Session.Add("sEstSelectedByCustSearch", null);

            return date.ToString();
        }
        else
        {
            HttpContext.Current.Session.Add("sEstSelectedByCustSearch", null);
            return "";
        }
    }

    private static bool CheckAlphaNumeric(string str)
    {
        return Regex.IsMatch(str, @"^[a-zA-Z0-9_\-.?""',;:!@#$%&\[\]()/* ]*$");
    }

    [System.Web.Services.WebMethod]
    public static List<csCustomer> GetCustomer(string keyword)
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        var item = (from c in _db.customers
                    join sc in _db.ScheduleCalendars on c.customer_id equals sc.customer_id
                    where c.last_name1.ToUpper().StartsWith(keyword.Trim().ToUpper()) && sc.estimate_id != 0
                    select new csCustomer
                    {
                        customer_id = c.customer_id,
                        customer_name = c.first_name1.Trim() + " " + c.last_name1.Trim()
                    }).Distinct().ToList();
        return item.ToList();
    }

    [System.Web.Services.WebMethod]
    public static string GetEmployeeById(int empId)
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        var item = (from sp in _db.sales_persons
                    where sp.sales_person_id == empId
                    select sp).SingleOrDefault();

        return item.first_name.ToString() + " " + item.last_name.ToString();
    }

    [System.Web.Services.WebMethod]
    public static List<SectionInfo> GetSection(string keyword)
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        //var item = from c in _db.ScheduleCalendars
        //           where c.section_name.ToUpper().StartsWith(keyword.Trim().ToUpper()) && c.customer_id == keyword2
        //           select new SectionInfo
        //           {
        //               section_name = c.section_name.Trim()
        //           };

        var item = from c in _db.sectioninfos
                   where c.section_name.ToUpper().StartsWith(keyword.Trim().ToUpper()) && c.parent_id == 0
                   orderby c.section_name
                   select new SectionInfo
                   {
                       section_name = c.section_name.Trim()
                   };

        return item.ToList();
    }

    [System.Web.Services.WebMethod]
    public static List<CO_PricingDeatilModel> GetSectionByCustomerId(string keyword, int nCustId, int nEstId)
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        var item = from cpm in _db.co_pricing_masters
                   where cpm.section_name.ToUpper().StartsWith(keyword.Trim().ToUpper()) && cpm.customer_id == nCustId && cpm.estimate_id == nEstId
                   orderby cpm.section_name
                   select new CO_PricingDeatilModel
                   {
                       section_name = cpm.section_name.Trim()
                   };

        if (item.ToList().Count() == 0 && keyword.ToLower().Contains("*"))
        {
            item = from cpm in _db.co_pricing_masters
                   where cpm.customer_id == nCustId && cpm.estimate_id == nEstId
                   orderby cpm.section_name
                   select new CO_PricingDeatilModel
                   {
                       section_name = cpm.section_name.Trim()
                   };
        }

        return item.Distinct().ToList();
    }

    [System.Web.Services.WebMethod]
    public static List<userinfo> GetSalesPerson(string keyword)
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        var item = from sp in _db.sales_persons
                   where sp.first_name.ToUpper().StartsWith(keyword.Trim().ToUpper()) && sp.is_active == true
                   orderby sp.first_name
                   select new userinfo
                   {
                       sales_person_id = sp.sales_person_id,
                       sales_person_name = sp.first_name.Trim() + " " + sp.last_name.Trim()
                   };

        if (item.ToList().Count() == 0 && keyword.ToLower().Contains("*"))
        {
            item = from sp in _db.sales_persons
                   where sp.is_active == true
                   orderby sp.first_name
                   select new userinfo
                   {
                       sales_person_id = sp.sales_person_id,
                       sales_person_name = sp.first_name.Trim() + " " + sp.last_name.Trim()
                   };
        }

        return item.Distinct().ToList();
    }

    [System.Web.Services.WebMethod]
    public static List<csScheduleCalendar> GetSubsequentSection(string keyword, int nCustId, int nEstId, string ParentEventName)
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        List<csScheduleCalendar> listSC = new List<csScheduleCalendar>();
        string sSql = "";
        string strCondition = "";

        if (!keyword.ToLower().Contains("*"))
        {
            strCondition = " WHERE sc.[customer_id] = " + nCustId + " AND sc.[estimate_id] = " + nEstId + " AND sc.[title] <> '" + ParentEventName + "' ";
        }
        {
            strCondition = " WHERE sc.[title] LIKE '" + keyword.Trim() + "%' AND sc.[customer_id] = " + nCustId + " AND sc.[estimate_id] = " + nEstId + " AND sc.[title] <> '" + ParentEventName + "' ";
        }

        sSql = "SELECT sc.[event_id], sc.[title] + '...' + CONVERT(varchar, sc.[event_start], 101) + ' To ' +  CONVERT(varchar, sc.[event_end], 101) AS [section_name] " +
             " FROM [dbo].[ScheduleCalendarTemp] AS sc " +
             " " + strCondition + " " +
             " ORDER BY sc.[title]";


        listSC = _db.ExecuteQuery<csScheduleCalendar>(sSql).ToList();

        //var item = from sc in _db.ScheduleCalendarTemps
        //           where sc.title.ToUpper().StartsWith(keyword.Trim().ToUpper()) && sc.customer_id == nCustId && sc.estimate_id == nEstId && sc.title != ParentEventName
        //           orderby sc.title
        //           select new csScheduleCalendar
        //           {
        //               event_id = sc.event_id,
        //               section_name = (sc.title.Trim().ToString() + "..." + ((DateTime)sc.event_start).ToShortDateString().ToString() + " To " + ((DateTime)sc.event_end).ToShortDateString().ToString()).ToString()
        //           };

        //if (item.ToList().Count() == 0 && keyword.ToLower().Contains("*"))
        //{
        //    item = from sc in _db.ScheduleCalendarTemps
        //           where sc.customer_id == nCustId && sc.estimate_id == nEstId && sc.title != ParentEventName
        //           orderby sc.title
        //           select new csScheduleCalendar
        //           {
        //               event_id = sc.event_id,
        //               section_name = sc.title.Trim() + "..." + sc.event_start + " To " + sc.event_end
        //           };
        //}

        return listSC.Distinct().ToList();
    }

    [System.Web.Services.WebMethod]
    public static String GetSomeData()
    {
        GridView gv = new GridView();
        System.IO.StringWriter stringWriter = new System.IO.StringWriter();
        HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
        DataClassesDataContext _db = new DataClassesDataContext();
        var item = from scTemp in _db.ScheduleCalendarTemps
                   join linktemp in _db.ScheduleCalendarLinkTemps on scTemp.event_id equals linktemp.child_event_id
                   select new
                   {
                       link_id = linktemp.link_id,
                       title = scTemp.title,
                       start = scTemp.event_start,
                       end = scTemp.event_end
                   };

        gv.DataSource = item.ToList();
        gv.DataKeyNames = new string[] { "link_id" };
        gv.DataBind();

        gv.RenderControl(htmlWriter);
        return stringWriter.ToString();
    }

    [System.Web.Services.WebMethod]
    public static String DeleteEventLink(string eventId)
    {
        string strEventIds = eventId.Trim().TrimEnd(',');
        string result = "";
        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();

            string sqlDELETE = "DELETE ScheduleCalendarLinkTemp WHERE [link_id] in(" + strEventIds + ")";
            _db.ExecuteCommand(sqlDELETE, string.Empty);

            result = "Ok";
        }
        catch (Exception ex)
        {
            result = ex.Message;
        }

        return result;
    }

    [System.Web.Services.WebMethod]
    public static ImproperCalendarLinkEvent AddEventLink(int parent_event_id, int child_event_id)
    {

        ImproperCalendarLinkEvent objlnkEvent = new ImproperCalendarLinkEvent();
        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();


            ScheduleCalendarTemp objSCTmp = _db.ScheduleCalendarTemps.SingleOrDefault(sc => sc.event_id == parent_event_id);

            ScheduleCalendarLinkTemp objSCLinkTmp = new ScheduleCalendarLinkTemp();

            objSCLinkTmp.parent_event_id = parent_event_id;
            objSCLinkTmp.child_event_id = child_event_id;
            objSCLinkTmp.customer_id = objSCTmp.customer_id;
            objSCLinkTmp.estimate_id = objSCTmp.estimate_id;

            _db.ScheduleCalendarLinkTemps.InsertOnSubmit(objSCLinkTmp);
            _db.SubmitChanges();

            objlnkEvent = (from scTemp in _db.ScheduleCalendarTemps
                           join linktemp in _db.ScheduleCalendarLinkTemps on scTemp.event_id equals linktemp.child_event_id
                           where linktemp.customer_id == objSCTmp.customer_id && linktemp.estimate_id == objSCTmp.estimate_id && linktemp.link_id == objSCLinkTmp.link_id
                           select new ImproperCalendarLinkEvent
                           {
                               link_id = (int)linktemp.link_id,
                               title = scTemp.title,
                               start = scTemp.event_start.ToString(),
                               end = scTemp.event_end.ToString(),
                               parent_event_id = (int)linktemp.parent_event_id,
                               IsSuccess = "Ok"
                           }).FirstOrDefault();

        }
        catch (Exception ex)
        {
            objlnkEvent.IsSuccess = ex.Message;
        }

        return objlnkEvent;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);

            if (Session["oUser"] == null)
            {
                Response.Redirect("AwardCRMLogin.aspx");
            }
            //if (Page.User.IsInRole("Call003") == false)
            //{
            //    // No Permission Page.
            //    Response.Redirect("nopermission.aspx");
            //}



            //Clear Search
            HttpContext.Current.Session.Add("CusId", 0);
            HttpContext.Current.Session.Add("sSecName", "");

            userinfo objUName = (userinfo)Session["oUser"];
            string strUName = objUName.first_name;

            int nCustomerID = 0;
            int nEstimateID = 0;
            int nEmployeeID = 0;
            int nTypeId = 0;

            DataClassesDataContext _db = new DataClassesDataContext();
            customer objCust = new customer();
            customer_estimate cus_est = new customer_estimate();
            ScheduleCalendar objSC = new ScheduleCalendar();
            co_pricing_master objCOPM = new co_pricing_master();
            location objLocation = new location();

            string strEventName = "";
            string serviceColor = "fc-default";
            string strEstimateName = "";
            string strCustName = "";
            string strEmpInitial = "";
            string strNotes = "";
            nTypeId = Convert.ToInt32(Request.QueryString.Get("TypeID"));
            HttpContext.Current.Session.Add("TypeID", nTypeId);
            if (nTypeId == 1)
            {
                lbltopHead.Text = "Operation Calendar";
                txtSearch.Visible = true;
                txtSection.Visible = true;
                btnSearch.Visible = true;
                lnkViewAll.Visible = true;
            }
            if (nTypeId == 2)
            {
                lbltopHead.Text = "Sales Calendar";
                txtSearch.Visible = false;
                txtSection.Visible = false;
                btnSearch.Visible = false;
                lnkViewAll.Visible = false;

            }
            if (Request.QueryString.Get("cid") != null && Request.QueryString.Get("eid") != null) // Customer Schedule
            {

                nCustomerID = Convert.ToInt32(Request.QueryString.Get("cid"));
                nEstimateID = Convert.ToInt32(Request.QueryString.Get("eid"));
                nEmployeeID = Convert.ToInt32(Request.QueryString.Get("empid"));
                nTypeId = Convert.ToInt32(Request.QueryString.Get("TypeID"));

                hdnEstIDSelected.Value = nEstimateID.ToString();
                hdnCustIDSelected.Value = nCustomerID.ToString();

                BindDragSectionList(nCustomerID, nEstimateID);

                HttpContext.Current.Session.Add("uname", strUName);
                HttpContext.Current.Session.Add("CustSelected", nCustomerID);
                HttpContext.Current.Session.Add("EstSelected", nEstimateID);

                string strQ = "SELECT * FROM customer_estimate WHERE customer_id=" + nCustomerID + " AND status_id = 3 AND client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
                IEnumerable<customer_estimate_model> list = _db.ExecuteQuery<customer_estimate_model>(strQ, string.Empty);

                ddlEst.DataSource = list;
                ddlEst.DataTextField = "estimate_name";
                ddlEst.DataValueField = "estimate_id";
                ddlEst.DataBind();

                Session.Add("CustomerId", nCustomerID);

               

                btnBack.Text = "Return to Customer List";// Schedule (" + strCustName + " - " + strEstimateName + ")";
                if (_db.customer_estimates.Where(ce => ce.customer_id == nCustomerID && ce.estimate_id == nEstimateID).Count() > 0)
                {
                    cus_est = _db.customer_estimates.Single(ce => ce.customer_id == nCustomerID && ce.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && ce.estimate_id == nEstimateID);
                    strEstimateName = cus_est.estimate_name;

                    ddlEst.SelectedValue = nEstimateID.ToString();
                }


                if (_db.customers.Where(c => c.customer_id == nCustomerID).Count() > 0)
                {
                    objCust = _db.customers.SingleOrDefault(c => c.customer_id == nCustomerID);
                    strCustName = objCust.first_name1 + " " + objCust.last_name1;

                    lbltopHead.Text = "Operation Calendar (" + strCustName + ")";

                    hdnCalStateAction.Value = objCust.isCalendarOnline.ToString().ToLower();
                    Session.Add("sIsCalendarOnline", objCust.isCalendarOnline);

                    if ((bool)objCust.isCalendarOnline)
                    {
                        lblCalState.Text = "Calendar is Online";
                        lblCalState.BackColor = Color.Green;
                        lblCalState.ForeColor = Color.White;

                        btnCalStateAction.Text = "Go Offline";

                        //btnCalStateAction.BorderColor = Color.Red;

                        if (_db.ScheduleCalendars.Where(sc => sc.customer_id == nCustomerID && sc.estimate_id == nEstimateID).Count() == 0)
                        {
                            HttpContext.Current.Session.Add("cid", nCustomerID);
                            HttpContext.Current.Session.Add("eid", nEstimateID);
                            HttpContext.Current.Session.Add("empid", nEmployeeID);
                            HttpContext.Current.Session.Add("TypeID", nTypeId);

                            //  hdnAddEventName.Value = (strEmpInitial + " " + strEstimateName + " - " + strCustName).Trim();
                            hdnAddEventName.Value = "";
                            hdnEventDesc.Value = "";// objCOPM.short_notes;
                            hdnEstimateID.Value = nEstimateID.ToString();
                            hdnCustomerID.Value = nCustomerID.ToString();
                            hdnEmployeeID.Value = nEmployeeID.ToString();
                            hdnTypeID.Value = nTypeId.ToString();

                            if (nTypeId == 1)
                                serviceColor = "fc-contract";
                            else if (nTypeId == 2)
                                serviceColor = "fc-ticket";
                            else if (nTypeId == 3)
                                serviceColor = "fc-sales";
                            else
                                serviceColor = "fc-default";

                            hdnServiceCssClass.Value = serviceColor;
                        }
                        else
                        {
                            var date = _db.ScheduleCalendars.Where(c => c.customer_id == nCustomerID && c.estimate_id == nEstimateID && c.event_start >= DateTime.Now).Min(x => x.event_start);
                            HttpContext.Current.Session.Add("cid", null);
                            HttpContext.Current.Session.Add("eid", null);
                            HttpContext.Current.Session.Add("empid", null);
                            //HttpContext.Current.Session.Add("TypeID", null);
                            hdnAddEventName.Value = "";
                            hdnEventDesc.Value = "";
                            hdnEstimateID.Value = "";
                            hdnCustomerID.Value = "";
                            hdnEmployeeID.Value = "";
                            hdnTypeID.Value = "";
                            hdnEventStartDate.Value = date.ToString();
                            btnBack.Visible = true;
                            btnBack.Text = "Return to Customer List";// Schedule (" + strCustName + " - " + strEstimateName + ")";
                        }
                    }
                    else
                    {
                        lblCalState.Text = "Calendar is Offline";
                        lblCalState.BackColor = Color.Red;
                        lblCalState.ForeColor = Color.White;

                        btnCalStateAction.Text = "Go Online";

                        //btnCalStateAction.BorderColor = Color.Green;

                        string date = "";



                        if (_db.ScheduleCalendarTemps.Where(sc => sc.customer_id == nCustomerID && sc.estimate_id == nEstimateID).Count() != 0)
                        {
                            date = (_db.ScheduleCalendarTemps.Where(c => c.customer_id == nCustomerID && c.estimate_id == nEstimateID && c.event_start >= DateTime.Now).Min(x => x.event_start)).ToString();


                        }


                        var item = from scTemp in _db.ScheduleCalendarTemps
                                   join linktemp in _db.ScheduleCalendarLinkTemps on scTemp.event_id equals linktemp.child_event_id
                                   where linktemp.customer_id == nCustomerID && linktemp.estimate_id == nEstimateID
                                   orderby scTemp.event_start
                                   select new
                                   {
                                       link_id = linktemp.link_id,
                                       title = scTemp.title,
                                       start = scTemp.event_start,
                                       end = scTemp.event_end,
                                       parent_event_id = linktemp.parent_event_id,
                                       dependencyType = linktemp.dependencyType,
                                       lag = linktemp.lag
                                   };
                        hdnEventLinkCount.Value = item.Count().ToString();
                        grdCalLinkInfo.DataSource = item.ToList();
                        grdCalLinkInfo.DataKeyNames = new string[] { "link_id", "dependencyType" };
                        grdCalLinkInfo.DataBind();

                        HttpContext.Current.Session.Add("cid", nCustomerID);
                        HttpContext.Current.Session.Add("eid", nEstimateID);
                        HttpContext.Current.Session.Add("empid", nEmployeeID);
                        HttpContext.Current.Session.Add("TypeID", nTypeId);

                        //  hdnAddEventName.Value = (strEmpInitial + " " + strEstimateName + " - " + strCustName).Trim();
                        hdnAddEventName.Value = "";
                        hdnEventDesc.Value = "";// objCOPM.short_notes;
                        hdnEstimateID.Value = nEstimateID.ToString();
                        hdnCustomerID.Value = nCustomerID.ToString();
                        hdnEmployeeID.Value = nEmployeeID.ToString();
                        hdnTypeID.Value = nTypeId.ToString();

                        if (nTypeId == 1)
                            serviceColor = "fc-contract";
                        else if (nTypeId == 2)
                            serviceColor = "fc-ticket";
                        else if (nTypeId == 3)
                            serviceColor = "fc-sales";
                        else
                            serviceColor = "fc-default";

                        hdnServiceCssClass.Value = serviceColor;



                        hdnEventStartDate.Value = date.ToString();
                        btnBack.Visible = true;
                        btnBack.Text = "Return to Customer List";// Schedule (" + strCustName + " - " + strEstimateName + ")";


                        //if (_db.ScheduleCalendarTemps.Where(sc => sc.customer_id == nCustomerID && sc.estimate_id == nEstimateID).Count() == 0)
                        //{
                        //    HttpContext.Current.Session.Add("cid", nCustomerID);
                        //    HttpContext.Current.Session.Add("eid", nEstimateID);
                        //    HttpContext.Current.Session.Add("empid", nEmployeeID);
                        //    HttpContext.Current.Session.Add("TypeID", nTypeId);

                        //    hdnAddEventName.Value = (strEmpInitial + " " + strEstimateName + " - " + strCustName).Trim();
                        //    hdnEventDesc.Value = "";// objCOPM.short_notes;
                        //    hdnEstimateID.Value = nEstimateID.ToString();
                        //    hdnCustomerID.Value = nCustomerID.ToString();
                        //    hdnEmployeeID.Value = nEmployeeID.ToString();
                        //    hdnTypeID.Value = nTypeId.ToString();

                        //    if (nTypeId == 1)
                        //        serviceColor = "fc-contract";
                        //    else if (nTypeId == 2)
                        //        serviceColor = "fc-ticket";
                        //    else if (nTypeId == 3)
                        //        serviceColor = "fc-sales";
                        //    else
                        //        serviceColor = "fc-default";

                        //    hdnServiceCssClass.Value = serviceColor;
                        //}
                        //else
                        //{
                        //    var date = _db.ScheduleCalendarTemps.Where(c => c.customer_id == nCustomerID && c.estimate_id == nEstimateID && c.event_start >= DateTime.Now).Min(x => x.event_start);
                        //    HttpContext.Current.Session.Add("cid", null);
                        //    HttpContext.Current.Session.Add("eid", null);
                        //    HttpContext.Current.Session.Add("empid", null);
                        //    //HttpContext.Current.Session.Add("TypeID", null);
                        //    hdnAddEventName.Value = "";
                        //    hdnEventDesc.Value = "";
                        //    hdnEstimateID.Value = "";
                        //    hdnCustomerID.Value = "";
                        //    hdnEmployeeID.Value = "";
                        //    hdnTypeID.Value = "";
                        //    hdnEventStartDate.Value = date.ToString();
                        //    btnBack.Visible = true;
                        //    btnBack.Text = "Return to Schedule (" + strCustName + " - " + strEstimateName + ")";
                        //}
                    }


                }


            }
            else if (Request.QueryString.Get("cid") != null) // Customer sales
            {
                nCustomerID = Convert.ToInt32(Request.QueryString.Get("cid"));
                nEmployeeID = Convert.ToInt32(Request.QueryString.Get("empid"));
                nTypeId = Convert.ToInt32(Request.QueryString.Get("TypeID"));

                hdnEstIDSelected.Value = nEstimateID.ToString();
                hdnCustIDSelected.Value = nCustomerID.ToString();


                HttpContext.Current.Session.Add("uname", strUName);
                HttpContext.Current.Session.Add("CustSelected", nCustomerID);
                HttpContext.Current.Session.Add("EstSelected", null);

                if (_db.customers.Where(c => c.customer_id == nCustomerID).Count() > 0)
                {
                    objCust = _db.customers.FirstOrDefault(c => c.customer_id == nCustomerID);
                    var emp = _db.sales_persons.FirstOrDefault(em => em.sales_person_id == objCust.sales_person_id);

                    strEventName = (objCust.last_name1 + " (" + emp.first_name + " " + emp.last_name + ")").Trim();
                    strCustName = objCust.last_name1;
                    strNotes = objCust.notes ?? "";
                }

                if (_db.ScheduleCalendars.Where(sc => sc.customer_id == nCustomerID && sc.type_id == nTypeId).Count() == 0)
                {
                    HttpContext.Current.Session.Add("cid", nCustomerID);
                    HttpContext.Current.Session.Add("eid", null);
                    HttpContext.Current.Session.Add("empid", nEmployeeID);
                    HttpContext.Current.Session.Add("TypeID", nTypeId);

                    hdnAddEventName.Value = strEventName;
                    hdnEventDesc.Value = strNotes;
                    hdnEstimateID.Value = nEstimateID.ToString();
                    hdnCustomerID.Value = nCustomerID.ToString();
                    hdnEmployeeID.Value = nEmployeeID.ToString();
                    hdnTypeID.Value = nTypeId.ToString();

                    if (nTypeId == 1)
                        serviceColor = "fc-contract";
                    else if (nTypeId == 2)
                        serviceColor = "fc-ticket";
                    else if (nTypeId == 3)
                        serviceColor = "fc-sales";
                    else
                        serviceColor = "fc-default";

                    hdnServiceCssClass.Value = serviceColor;
                    btnBack.Visible = true;
                    btnBack.Text = "Return to  Customer List";//  (" + strCustName + ")";
                }
                else
                {
                    var date = _db.ScheduleCalendars.Where(c => c.customer_id == nCustomerID && c.type_id == 2).FirstOrDefault().event_start;
                    HttpContext.Current.Session.Add("cid", null);
                    HttpContext.Current.Session.Add("eid", null);
                    HttpContext.Current.Session.Add("empid", null);
                    //HttpContext.Current.Session.Add("TypeID", null);
                    hdnAddEventName.Value = "";
                    hdnEventDesc.Value = "";
                    hdnEstimateID.Value = "";
                    hdnCustomerID.Value = "";
                    hdnEmployeeID.Value = "";
                    hdnTypeID.Value = "";
                    hdnEventStartDate.Value = date.ToString();
                    btnBack.Visible = true;
                    btnBack.Text = "Return to Customer List";//  (" + strCustName + ")";
                }
            }
            else
            {
                HttpContext.Current.Session.Add("cid", null);
                HttpContext.Current.Session.Add("eid", null);
                HttpContext.Current.Session.Add("empid", null);
                //HttpContext.Current.Session.Add("TypeID", null);
                HttpContext.Current.Session.Add("CustSelected", null);
                HttpContext.Current.Session.Add("EstSelected", null);
                hdnAddEventName.Value = "";
                hdnEventDesc.Value = "";
                hdnEstimateID.Value = "";
                hdnCustomerID.Value = "";
                hdnEmployeeID.Value = "";
                hdnTypeID.Value = "";
            }

        }
    }

    protected void grdCalLinkInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int ndependencyType = Convert.ToInt32(grdCalLinkInfo.DataKeys[e.Row.RowIndex].Values[1].ToString());

            TextBox grdtxtOffsetdays = (TextBox)e.Row.FindControl("grdtxtOffsetdays");
            Label lblStart = (Label)e.Row.FindControl("lblStart");
            Label lblEnd = (Label)e.Row.FindControl("lblEnd");

            DropDownList grdddldependencyType = (DropDownList)e.Row.FindControl("grdddldependencyType");

            grdddldependencyType.SelectedValue = ndependencyType.ToString();

            grdddldependencyType.Attributes.Add("onchange", "javascript:dependencyTypeChange(" + grdddldependencyType.ClientID + "," + lblStart.ClientID + "," + lblEnd.ClientID + "," + grdtxtOffsetdays.ClientID + ");");
            grdtxtOffsetdays.Attributes.Add("onblur", "javascript:txtOffsetChange(" + grdddldependencyType.ClientID + "," + lblStart.ClientID + "," + lblEnd.ClientID + "," + grdtxtOffsetdays.ClientID + ");");

            if (ndependencyType == 3)
            {
                grdtxtOffsetdays.CssClass = "csstxtOffsetdays displayshow";
            }
            else
            {
                grdtxtOffsetdays.CssClass = "csstxtOffsetdays displayhide";
                // grdtxtOffsetdays.Visible = false;
            }

        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnBack.ID, btnBack.GetType().Name, "Click");
        //if (hdnEstIDSelected.Value != null && hdnEstIDSelected.Value != "0")
        //{
        //    Response.Redirect("cust_schedule.aspx?cid=" + Convert.ToInt32(hdnCustIDSelected.Value) + "&eid=" + Convert.ToInt32(hdnEstIDSelected.Value));
        //}
        //else
        //{
        //    Response.Redirect("customer_details.aspx?cid=" + Convert.ToInt32(hdnCustIDSelected.Value));
        //}
        Response.Redirect("customerlist.aspx");
    }

    protected void btnSalesCalendar_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("schedulecalendar.aspx?TypeID=2");
    }

    protected void btnOperationCalendar_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("schedulecalendar.aspx?TypeID=1");
    }

    protected void btnCalStateAction_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnCalStateAction.ID, btnCalStateAction.GetType().Name, "Click"); 
        DataClassesDataContext _db = new DataClassesDataContext();

        int nCustId = Convert.ToInt32(hdnCustIDSelected.Value);
        int nEstid = Convert.ToInt32(hdnEstIDSelected.Value);

        customer objcpmList = _db.customers.SingleOrDefault(c => c.customer_id == nCustId);




        if (btnCalStateAction.Text == "Go Offline") // Offline
        {
            if (_db.ScheduleCalendars.Any(sc => sc.customer_id == nCustId && sc.estimate_id == nEstid))
            {
                //Insert
                string sSqlINSERT = "INSERT INTO [ScheduleCalendarTemp] " +
                                " SELECT [title],[description],[event_start],[event_end],[customer_id],[estimate_id],[employee_id],[section_name], " +
                                " [location_name],[create_date],[last_updated_date],[last_updated_by],[type_id],[parent_id],[job_start_date], " +
                                " [co_pricing_list_id],[cssClassName],[google_event_id],[operation_notes],[is_complete],[IsEstimateActive],[employee_name],[event_id] " +
                                " FROM [ScheduleCalendar] " +
                                " WHERE [customer_id] = " + nCustId + " AND [estimate_id] = " + nEstid + "";

                _db.ExecuteCommand(sSqlINSERT, string.Empty);

                //Insert
                string sSqlLinkINSERT = "INSERT INTO [ScheduleCalendarLinkTemp] " +
                                       " SELECT [parent_event_id], " +
                                       " [child_event_id], " +
                                       " [customer_id], " +
                                       " [estimate_id], " +
                                       " [dependencyType], " +
                                       " [lag],[link_id] " +
                                       " FROM [ScheduleCalendarLink] " +
                                       " WHERE [customer_id] = " + nCustId + " AND [estimate_id] = " + nEstid + "";

                _db.ExecuteCommand(sSqlLinkINSERT, string.Empty);
            }

            objcpmList.isCalendarOnline = false;

            lblCalState.Text = "Calendar is Offline";
            lblCalState.BackColor = Color.Red;
            lblCalState.ForeColor = Color.White;
            btnCalStateAction.Text = "Go Online";

            //btnCalStateAction.BorderColor = Color.Green;

            hdnCalStateAction.Value = "false";
            Session.Add("sIsCalendarOnline", false);

        }
        else // Online
        {
            if (_db.ScheduleCalendarTemps.Any(sc => sc.customer_id == nCustId && sc.estimate_id == nEstid))
            {
                //Delete  table
                string sqlDELETE = "DELETE ScheduleCalendar WHERE [customer_id] = " + nCustId + " AND [estimate_id] = " + nEstid + "";
                _db.ExecuteCommand(sqlDELETE, string.Empty);

                string sqlDELETELink = "DELETE ScheduleCalendarLink WHERE [customer_id] = " + nCustId + " AND [estimate_id] = " + nEstid + "";
                _db.ExecuteCommand(sqlDELETELink, string.Empty);

                //Insert
                string sSQLINSERT = "INSERT INTO [ScheduleCalendar] " +
                                " SELECT [title],[description],[event_start],[event_end],[customer_id],[estimate_id],[employee_id],[section_name], " +
                                " [location_name],[create_date],[last_updated_date],[last_updated_by],[type_id],[parent_id],[job_start_date], " +
                                " [co_pricing_list_id],[cssClassName],[google_event_id],[operation_notes],[is_complete],[IsEstimateActive],[employee_name],[event_id] " +
                                " FROM [ScheduleCalendarTemp] " +
                                " WHERE [customer_id] = " + nCustId + " AND [estimate_id] = " + nEstid + "";

                _db.ExecuteCommand(sSQLINSERT, string.Empty);

                //Insert
                string sSqlLinkINSERT = "INSERT INTO [ScheduleCalendarLink] " +
                                       " SELECT [parent_event_id], " +
                                       " [child_event_id], " +
                                       " [customer_id], " +
                                       " [estimate_id], " +
                                       " [dependencyType], " +
                                       " [lag],[link_id] " +
                                       " FROM [ScheduleCalendarLinkTemp] " +
                                       " WHERE [customer_id] = " + nCustId + " AND [estimate_id] = " + nEstid + "";

                _db.ExecuteCommand(sSqlLinkINSERT, string.Empty);

                //Delete Temp table
                string sqlDELETETemp = "DELETE ScheduleCalendarTemp WHERE [customer_id] = " + nCustId + " AND [estimate_id] = " + nEstid + "";
                _db.ExecuteCommand(sqlDELETETemp, string.Empty);

                //Delete Temp table
                string sqlDELETELinkTemp = "DELETE ScheduleCalendarLinkTemp WHERE [customer_id] = " + nCustId + " AND [estimate_id] = " + nEstid + "";
                _db.ExecuteCommand(sqlDELETELinkTemp, string.Empty);
            }

            objcpmList.isCalendarOnline = true;

            lblCalState.Text = "Calendar is Online";
            lblCalState.BackColor = Color.Green;
            lblCalState.ForeColor = Color.White;

            btnCalStateAction.Text = "Go Offline";

            //btnCalStateAction.BorderColor = Color.Red;

            hdnCalStateAction.Value = "true";
            Session.Add("sIsCalendarOnline", true);
        }
        _db.SubmitChanges();




        hdnCalStateAction.Value = objcpmList.isCalendarOnline.ToString().ToLower();
    }

    protected void btnHdn_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnHdn.ID, btnHdn.GetType().Name, "Click"); 
        DataClassesDataContext _db = new DataClassesDataContext();

        int nCustId = Convert.ToInt32(hdnCustIDSelected.Value);
        int nEstid = Convert.ToInt32(hdnEstIDSelected.Value);
        int nEventId = Convert.ToInt32(hdnEventId.Value);

        BindDragSectionList(nCustId, nEstid);

        //foreach (GridViewRow gr in grdCalLinkInfo.Rows)
        //{
        //    Label lblStart = (Label)gr.FindControl("lblStart");
        //    Label lblEnd = (Label)gr.FindControl("lblEnd");

        //    DropDownList grdddldependencyType = (DropDownList)gr.FindControl("grdddldependencyType");
        //    TextBox grdtxtOffsetdays = (TextBox)gr.FindControl("grdtxtOffsetdays");

        //    int nLinkId = Convert.ToInt32(grdCalLinkInfo.DataKeys[gr.RowIndex].Values[0]);
        //    int nDependencyType = Convert.ToInt32(grdddldependencyType.SelectedItem.Value);

        //    int nOffsetDays = 0;
        //    if (nDependencyType == 1)
        //    {
        //        nOffsetDays = 0;
        //    }
        //    else if (nDependencyType == 2)
        //    {
        //        nOffsetDays = 1;
        //    }
        //    else if (nDependencyType == 3)
        //    {
        //        nOffsetDays = Convert.ToInt32(grdtxtOffsetdays.Text);
        //    }


        //    ScheduleCalendarLinkTemp objSCLinkTmp = _db.ScheduleCalendarLinkTemps.FirstOrDefault(lnk => lnk.link_id == nLinkId);


        //    if (objSCLinkTmp != null)
        //    {
        //        objSCLinkTmp.dependencyType = nDependencyType;
        //        objSCLinkTmp.lag = nOffsetDays;

        //        ScheduleCalendarTemp ocjSCTemp = _db.ScheduleCalendarTemps.FirstOrDefault(sc => sc.event_id == objSCLinkTmp.child_event_id);

        //        if (ocjSCTemp != null)
        //        {
        //            ocjSCTemp.event_start = Convert.ToDateTime(lblStart.Text);
        //            ocjSCTemp.event_end = Convert.ToDateTime(lblEnd.Text);

        //        }
        //    }
        //    _db.SubmitChanges();
        //}


        var item = from scTemp in _db.ScheduleCalendarTemps
                   join linktemp in _db.ScheduleCalendarLinkTemps on scTemp.event_id equals linktemp.child_event_id
                   where linktemp.customer_id == nCustId && linktemp.estimate_id == nEstid
                   orderby scTemp.event_start
                   select new
                   {
                       link_id = linktemp.link_id,
                       title = scTemp.title,
                       start = scTemp.event_start,
                       end = scTemp.event_end,
                       parent_event_id = linktemp.parent_event_id,
                       dependencyType = linktemp.dependencyType,
                       lag = linktemp.lag
                   };
        hdnEventLinkCount.Value = item.Count().ToString();
        grdCalLinkInfo.DataSource = item.ToList();
        grdCalLinkInfo.DataKeyNames = new string[] { "link_id", "dependencyType" };
        grdCalLinkInfo.DataBind();
    }

    protected void btnUpdateLink_Click(object sender, EventArgs e)
    {
        

        DataClassesDataContext _db = new DataClassesDataContext();

        int nCustId = Convert.ToInt32(hdnCustIDSelected.Value);
        int nEstid = Convert.ToInt32(hdnEstIDSelected.Value);
        int nEventId = Convert.ToInt32(hdnEventId.Value);

        foreach (GridViewRow gr in grdCalLinkInfo.Rows)
        {
            Label lblStart = (Label)gr.FindControl("lblStart");
            Label lblEnd = (Label)gr.FindControl("lblEnd");

            DropDownList grdddldependencyType = (DropDownList)gr.FindControl("grdddldependencyType");
            TextBox grdtxtOffsetdays = (TextBox)gr.FindControl("grdtxtOffsetdays");

            int nLinkId = Convert.ToInt32(grdCalLinkInfo.DataKeys[gr.RowIndex].Values[0]);
            int nDependencyType = Convert.ToInt32(grdddldependencyType.SelectedItem.Value);

            int nOffsetDays = 0;
            if (nDependencyType == 1)
            {
                nOffsetDays = 0;
            }
            else if (nDependencyType == 2)
            {
                nOffsetDays = 1;
            }
            else if (nDependencyType == 3)
            {
                nOffsetDays = Convert.ToInt32(grdtxtOffsetdays.Text);
            }


            ScheduleCalendarLinkTemp objSCLinkTmp = _db.ScheduleCalendarLinkTemps.FirstOrDefault(lnk => lnk.link_id == nLinkId);


            if (objSCLinkTmp != null)
            {
                objSCLinkTmp.dependencyType = nDependencyType;
                objSCLinkTmp.lag = nOffsetDays;

                ScheduleCalendarTemp ocjSCTemp = _db.ScheduleCalendarTemps.FirstOrDefault(sc => sc.event_id == objSCLinkTmp.child_event_id);

                if (ocjSCTemp != null)
                {
                    ocjSCTemp.event_start = Convert.ToDateTime(lblStart.Text);
                    ocjSCTemp.event_end = Convert.ToDateTime(lblEnd.Text);

                }
            }
            _db.SubmitChanges();
        }


        //var item = from scTemp in _db.ScheduleCalendarTemps
        //           join linktemp in _db.ScheduleCalendarLinkTemps on scTemp.event_id equals linktemp.child_event_id
        //           where linktemp.customer_id == nCustId && linktemp.estimate_id == nEstid
        //           orderby scTemp.event_start
        //           select new
        //           {
        //               link_id = linktemp.link_id,
        //               title = scTemp.title,
        //               start = scTemp.event_start,
        //               end = scTemp.event_end,
        //               parent_event_id = linktemp.parent_event_id,
        //               dependencyType = linktemp.dependencyType,
        //               lag = linktemp.lag
        //           };
        //hdnEventLinkCount.Value = item.Count().ToString();
        //grdCalLinkInfo.DataSource = item.ToList();
        //grdCalLinkInfo.DataKeyNames = new string[] { "link_id", "dependencyType" };
        //grdCalLinkInfo.DataBind();
    }

    protected void ddlEst_SelectedIndexChanged(object sender, EventArgs e)
    {
        int nCustId = Convert.ToInt32(hdnCustomerID.Value);
        int nEstid = Convert.ToInt32(ddlEst.SelectedItem.Value);

        Response.Redirect("Copyschedulecalendar.aspx?eid=" + nEstid + "&cid=" + nCustId);
    }


    [System.Web.Services.WebMethod]
    public static String UpdateEventLink(string datascEventLinks)
    {
        System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
        List<scEventLink> scEventLinks = json.Deserialize<List<scEventLink>>(datascEventLinks);

        string result = "";
        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();


            foreach (scEventLink sclink in scEventLinks)
            {
                int nLinkId = sclink.link_id;
                int nDependencyType = sclink.dependencyType;

                int nOffsetDays = 0;
                if (nDependencyType == 1)
                {
                    nOffsetDays = 0;
                }
                else if (nDependencyType == 2)
                {
                    nOffsetDays = 1;
                }
                else if (nDependencyType == 3)
                {
                    nOffsetDays = sclink.offsetdays;
                }


                ScheduleCalendarLinkTemp objSCLinkTmp = _db.ScheduleCalendarLinkTemps.FirstOrDefault(lnk => lnk.link_id == nLinkId);


                if (objSCLinkTmp != null)
                {
                    objSCLinkTmp.dependencyType = nDependencyType;
                    objSCLinkTmp.lag = nOffsetDays;

                    ScheduleCalendarTemp ocjSCTemp = _db.ScheduleCalendarTemps.FirstOrDefault(sc => sc.event_id == objSCLinkTmp.child_event_id);

                    if (ocjSCTemp != null)
                    {
                        ocjSCTemp.event_start = Convert.ToDateTime(sclink.event_start);
                        ocjSCTemp.event_end = Convert.ToDateTime(sclink.event_end);

                    }
                }
                _db.SubmitChanges();
            }
            result = "Ok";
        }
        catch (Exception ex)
        {
            result = ex.Message;
        }

        return result;
    }

    public class scEventLink
    {
        public int link_id { get; set; }
        public int dependencyType { get; set; }
        public DateTime event_start { get; set; }
        public DateTime event_end { get; set; }
        public int offsetdays { get; set; }

    }

    protected void BindDragSectionList(int nCustId, int nEstId)
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        var item = from cpm in _db.co_pricing_masters
                   where cpm.customer_id == nCustId && cpm.estimate_id == nEstId && !(from sc in _db.ScheduleCalendarTemps where sc.customer_id == nCustId && sc.estimate_id == nEstId select sc.title).Contains(cpm.section_name)
                   orderby cpm.section_name
                   select new CO_PricingDeatilModel
                   {
                       section_name = cpm.section_name.Trim()
                   };
        grdDragSectionList.DataSource = item.Distinct().ToList();
        grdDragSectionList.DataBind();
    }

    protected void grdDragSectionList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {


        }
    }

    //public void GetScheduleEventColor()
    //{
    //    DataClassesDataContext _db = new DataClassesDataContext();
    //    if (_db.ScheduleCalendars.Where(sc => sc.customer_id == Convert.ToInt32(hdnCustomerID.Value) && sc.estimate_id == Convert.ToInt32(hdnEstimateID.Value)).Count() > 0)
    //    {
    //        var strcolor = _db.ScheduleCalendars.Where(sc => sc.customer_id == Convert.ToInt32(hdnCustomerID.Value) && sc.estimate_id == Convert.ToInt32(hdnEstimateID.Value)).FirstOrDefault().cssClassName;

    //        if (btnSoftBlue.ClientID.Replace("head_btn", "fc-") == strcolor)
    //        {
    //            btnSoftBlue.Text = HttpUtility.HtmlDecode("&#10004;");
    //        }
    //        else if (btnDarkBlue.ClientID.Replace("head_btn", "fc-") == strcolor)
    //        {
    //            btnDarkBlue.Text = HttpUtility.HtmlDecode("&#10004;");
    //        }
    //        else if (btnDarkCyan.ClientID.Replace("head_btn", "fc-") == strcolor)
    //        {
    //            btnDarkCyan.Text = HttpUtility.HtmlDecode("&#10004;");
    //        }
    //        else if (btnSlightlyBlue.ClientID.Replace("head_btn", "fc-") == strcolor)
    //        {
    //            btnSlightlyBlue.Text = HttpUtility.HtmlDecode("&#10004;");
    //        }
    //        else if (btnLimeGreen.ClientID.Replace("head_btn", "fc-") == strcolor)
    //        {
    //            btnLimeGreen.Text = HttpUtility.HtmlDecode("&#10004;");
    //        }
    //        else if (btnDarkGreen.ClientID.Replace("head_btn", "fc-") == strcolor)
    //        {
    //            btnDarkGreen.Text = HttpUtility.HtmlDecode("&#10004;");
    //        }
    //        else if (btnStrongYellow.ClientID.Replace("head_btn", "fc-") == strcolor)
    //        {
    //            btnStrongYellow.Text = HttpUtility.HtmlDecode("&#10004;");
    //        }
    //        else if (btnModeOrange.ClientID.Replace("head_btn", "fc-") == strcolor)
    //        {
    //            btnModeOrange.Text = HttpUtility.HtmlDecode("&#10004;");
    //        }
    //        else if (btnDarkRed.ClientID.Replace("head_btn", "fc-") == strcolor)
    //        {
    //            btnDarkRed.Text = HttpUtility.HtmlDecode("&#10004;");
    //        }
    //        else if (btnSlightlyViolet.ClientID.Replace("head_btn", "fc-") == strcolor)
    //        {
    //            btnSlightlyViolet.Text = HttpUtility.HtmlDecode("&#10004;");
    //        }
    //        else if (btnDarkGray.ClientID.Replace("head_btn", "fc-") == strcolor)
    //        {
    //            btnDarkGray.Text = HttpUtility.HtmlDecode("&#10004;");
    //        }
    //    }
    //    else
    //    {
    //        btnSoftBlue.Text = HttpUtility.HtmlDecode("&#10004;");
    //    }
    //}

    //public static string UpdateEventLink(List<scEventLink> scEventLinks, int nCustId, int nEstId)
}
