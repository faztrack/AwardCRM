using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Configuration;

/// <summary>
/// CopyEventDAO class is the main class which interacts with the database. SQL Server express edition
/// has been used.
/// the event information is stored in a table named 'event' in the database.
///
/// Here is the table format:
/// event(event_id int, title varchar(100), description varchar(200),event_start datetime, event_end datetime)
/// event_id is the primary key
/// </summary>
public class CopyEventDAO
{

    //change the connection string as per your database connection.
    //private static string connectionString = "Data Source=192.168.0.10;Initial Catalog=EventCalender;User ID=sa";

    //this method retrieves all events within range start-end
    public static List<CalendarEvent> getEvents(DateTime start, DateTime end, int nCusId, string strSecName)
    {
        int nTypeId = 0;
        int nOprationExtTypeID = 0;

        int ncid = 0;
        int neid = 0;

        if (System.Web.HttpContext.Current.Session["TypeID"] != null)
        {
            nTypeId = (int)System.Web.HttpContext.Current.Session["TypeID"];
        }
        if (System.Web.HttpContext.Current.Session["cid"] != null)
        {
            ncid = (int)System.Web.HttpContext.Current.Session["cid"];
        }
        if (System.Web.HttpContext.Current.Session["eid"] != null)
        {
            neid = (int)System.Web.HttpContext.Current.Session["eid"];
        }
        bool IsCalendarOnline = true;

        if (System.Web.HttpContext.Current.Session["sIsCalendarOnline"] != null)
            IsCalendarOnline = (bool)System.Web.HttpContext.Current.Session["sIsCalendarOnline"];

        //Opration/Sales Extra Type ID
        if (nTypeId == 1)
            nOprationExtTypeID = 11;
        if (nTypeId == 2)
            nOprationExtTypeID = 22;

        var typelist = new int[] { 0, 5, nTypeId, nOprationExtTypeID };
        // var typelist = new int[] { nTypeId };
        DataClassesDataContext _db = new DataClassesDataContext();
        List<CalendarEvent> events = new List<CalendarEvent>();

        var item = (from sc in _db.ScheduleCalendars
                    where typelist.Contains((int)sc.type_id) && sc.is_complete == false && sc.IsEstimateActive == true && sc.event_start >= start && sc.event_end <= end && sc.customer_id == ncid && sc.estimate_id == neid
                    select new CalendarEvent()
                    {
                        id = (int)sc.event_id,
                        //title = HttpUtility.HtmlDecode(sc.title),
                        title = HttpUtility.HtmlDecode(Regex.Replace(sc.title.Replace(Environment.NewLine, " "), @"\t|\n|\r", " ")),
                        description = HttpUtility.HtmlDecode(Regex.Replace(sc.description.Replace(Environment.NewLine, " "), @"\t|\n|\r", " ")),
                        start = (DateTime)sc.event_start,
                        end = (DateTime)sc.event_end,
                        customer_id = (int)sc.customer_id,
                        estimate_id = (int)sc.estimate_id,
                        employee_id = (int)sc.employee_id,
                        section_name = sc.section_name,
                        location_name = sc.location_name,
                        cssClassName = sc.cssClassName,
                        type_id = (int)sc.type_id,
                        operation_notes = HttpUtility.HtmlDecode(Regex.Replace(sc.operation_notes.Replace(Environment.NewLine, " "), @"\t|\n|\r", " ")),
                        employee_name = sc.employee_name
                    });

        if (!IsCalendarOnline)
        {
            item = (from sc in _db.ScheduleCalendarTemps
                    where typelist.Contains((int)sc.type_id) && sc.is_complete == false && sc.IsEstimateActive == true && sc.event_start >= start && sc.event_end <= end && sc.customer_id == ncid && sc.estimate_id == neid
                    select new CalendarEvent()
                    {
                        id = (int)sc.event_id,
                        //title = HttpUtility.HtmlDecode(sc.title),
                        title = HttpUtility.HtmlDecode(Regex.Replace(sc.title.Replace(Environment.NewLine, " "), @"\t|\n|\r", " ")),
                        description = HttpUtility.HtmlDecode(Regex.Replace(sc.description.Replace(Environment.NewLine, " "), @"\t|\n|\r", " ")),
                        start = (DateTime)sc.event_start,
                        end = (DateTime)sc.event_end,
                        customer_id = (int)sc.customer_id,
                        estimate_id = (int)sc.estimate_id,
                        employee_id = (int)sc.employee_id,
                        section_name = sc.section_name,
                        location_name = sc.location_name,
                        cssClassName = sc.cssClassName,
                        type_id = (int)sc.type_id,
                        operation_notes = HttpUtility.HtmlDecode(Regex.Replace(sc.operation_notes.Replace(Environment.NewLine, " "), @"\t|\n|\r", " ")),
                        employee_name = sc.employee_name
                    });
        }


        if ((nCusId != null && nCusId != 0) && (strSecName != "" && strSecName != null))
            events = item.Where(sc => sc.customer_id == nCusId && sc.section_name == strSecName).ToList();
        else if (strSecName != "")
            events = item.Where(sc => sc.section_name == strSecName).ToList();
        else if (nCusId != 0)
            events = item.Where(sc => sc.customer_id == nCusId).ToList();
        else
            events = item.ToList();

        return events;
        //side note: if you want to show events only related to particular users,
        //if user id of that user is stored in session as Session["userid"]
        //the event table also contains a extra field named 'user_id' to mark the event for that particular user
        //then you can modify the SQL as:
        //SELECT event_id, description, title, event_start, event_end FROM event where user_id=@user_id AND event_start>=@start AND event_end<=@end
        //then add paramter as:cmd.Parameters.AddWithValue("@user_id", HttpContext.Current.Session["userid"]);
    }

    //For Customer Calendar
    public static List<CalendarEvent> getEventsByCusId(DateTime start, DateTime end, int nCusId, string strSecName)
    {
        int nTypeId = 0;
        int nEstimateID = 0;
        if (System.Web.HttpContext.Current.Session["TypeID"] != null)
        {
            nTypeId = (int)System.Web.HttpContext.Current.Session["TypeID"];
        }
        if (System.Web.HttpContext.Current.Session["eid"] != null)
        {
            nEstimateID = (int)System.Web.HttpContext.Current.Session["eid"];
        }
        var typelist = new int[] { 5, nTypeId };
        // var typelist = new int[] { nTypeId };
        DataClassesDataContext _db = new DataClassesDataContext();
        List<CalendarEvent> events = new List<CalendarEvent>();

        string[] strDayOfWeek = new string[] { "Sunday" };

        var item = (from sc in _db.ScheduleCalendarTemps
                    where typelist.Contains((int)sc.type_id) && sc.customer_id == nCusId && sc.estimate_id == nEstimateID
                    && sc.is_complete == false && sc.IsEstimateActive == true
                    && (Convert.ToDateTime(sc.event_start).DayOfWeek != DayOfWeek.Sunday
                    && Convert.ToDateTime(sc.event_start).DayOfWeek != DayOfWeek.Saturday)
                    select new CalendarEvent()
                    {
                        id = (int)sc.event_id,
                        //title = HttpUtility.HtmlDecode(sc.title),
                        title = HttpUtility.HtmlDecode(Regex.Replace(sc.title.Replace(Environment.NewLine, " "), @"\t|\n|\r", " ")),
                        description = HttpUtility.HtmlDecode(Regex.Replace(sc.description.Replace(Environment.NewLine, " "), @"\t|\n|\r", " ")),
                        start = (DateTime)sc.event_start,
                        end = (DateTime)sc.event_end,
                        customer_id = (int)sc.customer_id,
                        estimate_id = (int)sc.estimate_id,
                        employee_id = (int)sc.employee_id,
                        section_name = sc.section_name,
                        location_name = sc.location_name,
                        cssClassName = sc.cssClassName,
                        type_id = (int)sc.type_id,
                        operation_notes = HttpUtility.HtmlDecode(Regex.Replace(sc.operation_notes.Replace(Environment.NewLine, " "), @"\t|\n|\r", " "))
                    }).Union
                   (from sc in _db.ScheduleCalendarTemps
                    where sc.type_id == 11 && sc.customer_id == nCusId && sc.estimate_id == nEstimateID
                    && sc.is_complete == false && sc.IsEstimateActive == true
                    select new CalendarEvent()
                    {
                        id = (int)sc.event_id,
                        //title = HttpUtility.HtmlDecode(sc.title),
                        title = HttpUtility.HtmlDecode(Regex.Replace(sc.title.Replace(Environment.NewLine, " "), @"\t|\n|\r", " ")),
                        description = HttpUtility.HtmlDecode(Regex.Replace(sc.description.Replace(Environment.NewLine, " "), @"\t|\n|\r", " ")),
                        start = (DateTime)sc.event_start,
                        end = (DateTime)sc.event_end,
                        customer_id = (int)sc.customer_id,
                        estimate_id = (int)sc.estimate_id,
                        employee_id = (int)sc.employee_id,
                        section_name = sc.section_name,
                        location_name = sc.location_name,
                        cssClassName = sc.cssClassName,
                        type_id = (int)sc.type_id,
                        operation_notes = HttpUtility.HtmlDecode(Regex.Replace(sc.operation_notes.Replace(Environment.NewLine, " "), @"\t|\n|\r", " "))
                    });
        var tDayOfWeek = "";

        foreach (CalendarEvent ce in item)
        {
            if (strDayOfWeek.Contains(Convert.ToDateTime(ce.start).DayOfWeek.ToString()))
                tDayOfWeek = Convert.ToDateTime(ce.start).DayOfWeek.ToString();
        }

        var result = (from sc in item.AsEnumerable()
                      where sc.type_id != 11
                      select new CalendarEvent()
                      {
                          id = sc.id,
                          //title = HttpUtility.HtmlDecode(sc.title),
                          title = HttpUtility.HtmlDecode(Regex.Replace(sc.title.Replace(Environment.NewLine, " "), @"\t|\n|\r", " ")),
                          description = HttpUtility.HtmlDecode(Regex.Replace(sc.description.Replace(Environment.NewLine, " "), @"\t|\n|\r", " ")),
                          start = (DateTime)sc.start,
                          end = DateTime.Parse(GetDayOfWeekWithOutHoliday(Convert.ToDateTime(sc.end).ToShortDateString()) + " " + Convert.ToDateTime(sc.end).ToShortTimeString()),
                          customer_id = (int)sc.customer_id,
                          estimate_id = (int)sc.estimate_id,
                          employee_id = (int)sc.employee_id,
                          section_name = sc.section_name,
                          location_name = sc.location_name,
                          cssClassName = sc.cssClassName,
                          type_id = (int)sc.type_id,
                          operation_notes = HttpUtility.HtmlDecode(Regex.Replace(sc.operation_notes.Replace(Environment.NewLine, " "), @"\t|\n|\r", " "))
                      }).Union
                     (from sc in item.AsEnumerable()
                      where sc.type_id == 11
                      select new CalendarEvent()
                      {
                          id = sc.id,
                          //title = HttpUtility.HtmlDecode(sc.title),
                          title = HttpUtility.HtmlDecode(Regex.Replace(sc.title.Replace(Environment.NewLine, " "), @"\t|\n|\r", " ")),
                          description = HttpUtility.HtmlDecode(Regex.Replace(sc.description.Replace(Environment.NewLine, " "), @"\t|\n|\r", " ")),
                          start = (DateTime)sc.start,
                          end = (DateTime)sc.end,
                          customer_id = (int)sc.customer_id,
                          estimate_id = (int)sc.estimate_id,
                          employee_id = (int)sc.employee_id,
                          section_name = sc.section_name,
                          location_name = sc.location_name,
                          cssClassName = sc.cssClassName,
                          type_id = (int)sc.type_id,
                          operation_notes = HttpUtility.HtmlDecode(Regex.Replace(sc.operation_notes.Replace(Environment.NewLine, " "), @"\t|\n|\r", " "))
                      });
        events = result.ToList();
        DataTable test = csCommonUtility.LINQToDataTable(item);
        return events;
        //side note: if you want to show events only related to particular users,
        //if user id of that user is stored in session as Session["userid"]
        //the event table also contains a extra field named 'user_id' to mark the event for that particular user
        //then you can modify the SQL as:
        //SELECT event_id, description, title, event_start, event_end FROM event where user_id=@user_id AND event_start>=@start AND event_end<=@end
        //then add paramter as:cmd.Parameters.AddWithValue("@user_id", HttpContext.Current.Session["userid"]);
    }

    //this method updates the event title and description
    public static void updateEvent(int id, String title, String description, DateTime start, DateTime end, int empId, string empName, int child_event_id, int dependencyType, int offsetDays)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        string strUName = (string)System.Web.HttpContext.Current.Session["uname"];
        int nEmployeeID = 0;
        string strEmployeeName = "";
        int nOffsetDays = 0;

        if (offsetDays != null)
        {
            nOffsetDays = offsetDays;
        }

        if (empId != null)
        {
            nEmployeeID = empId;
            strEmployeeName = empName;
        }

        string sql = "UPDATE ScheduleCalendarTemp SET title='" + title.Replace("'", "''") + "', description='" + description.Replace("'", "''") + "', " +
                    " event_start='" + start + "', event_end='" + end + "',  employee_id='" + nEmployeeID + "',  employee_name='" + strEmployeeName + "', " +
                    " last_updated_by='" + strUName + "', last_updated_date='" + DateTime.Now + "' " +
                    " WHERE event_id=" + id;

        _db.ExecuteCommand(sql, string.Empty);


        if (child_event_id != 0)//New Event Link Insert
        {
            ScheduleCalendarLinkTemp objSCLinkTmp = new ScheduleCalendarLinkTemp();

            ScheduleCalendarTemp objParentSCTmp = _db.ScheduleCalendarTemps.SingleOrDefault(sc => sc.event_id == id);
            ScheduleCalendarTemp objChildSCTmp = _db.ScheduleCalendarTemps.SingleOrDefault(sc => sc.event_id == child_event_id);

            int nDays = (Convert.ToDateTime(objChildSCTmp.event_end) - Convert.ToDateTime(objChildSCTmp.event_start)).Days;

            int nLinkId = 1;

            if (_db.ScheduleCalendarLinkTemps.Any())
            {
                nLinkId = Convert.ToInt32(_db.ScheduleCalendarLinkTemps.Max(e => e.link_id)) + 1;
            }
            objSCLinkTmp.link_id = nLinkId;
            objSCLinkTmp.parent_event_id = id;
            objSCLinkTmp.child_event_id = child_event_id;
            objSCLinkTmp.customer_id = objParentSCTmp.customer_id;
            objSCLinkTmp.estimate_id = objParentSCTmp.estimate_id;
            objSCLinkTmp.dependencyType = dependencyType;
            objSCLinkTmp.lag = nOffsetDays;

            _db.ScheduleCalendarLinkTemps.InsertOnSubmit(objSCLinkTmp);
            _db.SubmitChanges();

            if (dependencyType == 1) // Start Same Time
            {
                objChildSCTmp.event_start = objParentSCTmp.event_start;
                objChildSCTmp.event_end = objParentSCTmp.event_end;
                _db.SubmitChanges();
            }

            if (dependencyType == 2) // Start After Finish
            {
                objChildSCTmp.event_start = DateTime.Parse(GetDayOfWeek(Convert.ToDateTime(objParentSCTmp.event_end).AddDays(1).ToShortDateString()) + " " + Convert.ToDateTime(objChildSCTmp.event_start).ToShortTimeString());

                objChildSCTmp.event_end = DateTime.Parse(Convert.ToDateTime(objChildSCTmp.event_start).AddDays(nDays).ToShortDateString() + " " + Convert.ToDateTime(objChildSCTmp.event_end).ToShortTimeString());

                _db.SubmitChanges();
            }

            if (dependencyType == 3) // Offset days
            {
                objChildSCTmp.event_start = DateTime.Parse(GetDayOfWeek(Convert.ToDateTime(objParentSCTmp.event_end).AddDays(nOffsetDays + 1).ToShortDateString()) + " " + Convert.ToDateTime(objChildSCTmp.event_start).ToShortTimeString());

                objChildSCTmp.event_end = DateTime.Parse(Convert.ToDateTime(objChildSCTmp.event_start).AddDays(nDays).ToShortDateString() + " " + Convert.ToDateTime(objChildSCTmp.event_end).ToShortTimeString());

                _db.SubmitChanges();
            }

        }
        else//Update Event Link 
        {
            ScheduleCalendarTemp ocjSCTempUpdate = _db.ScheduleCalendarTemps.SingleOrDefault(sc => sc.event_id == id);
            //If Event is Parent
            List<ScheduleCalendarLinkTemp> objSCLinkTmpP = new List<ScheduleCalendarLinkTemp>();

            if (_db.ScheduleCalendarLinkTemps.Any(sl => sl.parent_event_id == id))
            {
                objSCLinkTmpP = _db.ScheduleCalendarLinkTemps.Where(sl => sl.parent_event_id == id).ToList();

                foreach (ScheduleCalendarLinkTemp slT in objSCLinkTmpP)
                {
                    ScheduleCalendarTemp ocjSCTempC = _db.ScheduleCalendarTemps.FirstOrDefault(s => s.event_id == slT.child_event_id);

                    int nDays = (Convert.ToDateTime(ocjSCTempC.event_end) - Convert.ToDateTime(ocjSCTempC.event_start)).Days;

                    if (slT.dependencyType == 1) // Start Same Time
                    {
                        ocjSCTempC.event_start = ocjSCTempUpdate.event_start;
                        ocjSCTempC.event_end = ocjSCTempUpdate.event_end;
                        _db.SubmitChanges();
                    }

                    if (slT.dependencyType == 2) // Start After Finish
                    {
                        ocjSCTempC.event_start = DateTime.Parse(GetDayOfWeek(Convert.ToDateTime(ocjSCTempUpdate.event_end).AddDays(1).ToShortDateString()) + " " + Convert.ToDateTime(ocjSCTempC.event_start).ToShortTimeString());
                        ocjSCTempC.event_end = DateTime.Parse(Convert.ToDateTime(ocjSCTempC.event_start).AddDays(nDays).ToShortDateString() + " " + Convert.ToDateTime(ocjSCTempC.event_end).ToShortTimeString());
                        _db.SubmitChanges();
                    }

                    if (dependencyType == 3) // Offset days
                    {
                        ocjSCTempC.event_start = DateTime.Parse(GetDayOfWeek(Convert.ToDateTime(ocjSCTempUpdate.event_end).AddDays(nOffsetDays).ToShortDateString()) + " " + Convert.ToDateTime(ocjSCTempC.event_start).ToShortTimeString());
                        ocjSCTempC.event_end = DateTime.Parse(Convert.ToDateTime(ocjSCTempC.event_start).AddDays(nDays).ToShortDateString() + " " + Convert.ToDateTime(ocjSCTempC.event_end).ToShortTimeString());
                        _db.SubmitChanges();
                    }
                }
            }

            //If Event is Child
            ScheduleCalendarLinkTemp objSCLinkTmpC = new ScheduleCalendarLinkTemp();
            if (_db.ScheduleCalendarLinkTemps.Any(sl => sl.child_event_id == id))
            {
                objSCLinkTmpC = _db.ScheduleCalendarLinkTemps.FirstOrDefault(sl => sl.child_event_id == id);

                ScheduleCalendarTemp ocjSCTempP = _db.ScheduleCalendarTemps.FirstOrDefault(s => s.event_id == objSCLinkTmpC.parent_event_id);

                DateTime pStartDate = Convert.ToDateTime(ocjSCTempP.event_start).Date;
                DateTime pEndDate = Convert.ToDateTime(ocjSCTempP.event_end).Date;

                DateTime cStartDate = Convert.ToDateTime(ocjSCTempUpdate.event_start).Date;

                int nDays = Convert.ToDateTime(ocjSCTempUpdate.event_start).Day - Convert.ToDateTime(ocjSCTempP.event_end).Day;

                if (cStartDate == pStartDate) // Start Same Time
                {
                    objSCLinkTmpC.dependencyType = 1;
                    objSCLinkTmpC.lag = 0;
                    _db.SubmitChanges();
                }
                else if (nDays == 1) // Start After Finish
                {
                    objSCLinkTmpC.dependencyType = 2;
                    objSCLinkTmpC.lag = 1;
                    _db.SubmitChanges();
                }
                else // Offset Days
                {
                    objSCLinkTmpC.dependencyType = 3;
                    objSCLinkTmpC.lag = nDays;
                    _db.SubmitChanges();
                }
            }
        }


        //customer cust = new customer();
        //if (_db.customers.Where(c => c.customer_id == _db.ScheduleCalendarTemps.SingleOrDefault(sc => sc.event_id == id).customer_id).Count() > 0)
        //    cust = _db.customers.Single(c => c.customer_id == _db.ScheduleCalendarTemps.SingleOrDefault(sc => sc.event_id == id).customer_id);

        //cust.notes = description;
        //cust.appointment_date = start;
        //_db.SubmitChanges();

        #region Blocked Google Calendar UPDATE (Type ID 2, Sales)------------
        //if (_db.ScheduleCalendarTemps.SingleOrDefault(sc => sc.event_id == id).type_id == 2) // Type ID 2 = Sales
        //{
        //    try
        //    {
        //        customer objcust = new customer();
        //        sales_person objSP = new sales_person();
        //        int nSalesPersonID = 0;
        //        string calendarId = string.Empty;
        //        if (ConfigurationManager.AppSettings["IsProduction"].ToString() == "True")
        //        {
        //            if (_db.customers.Where(sp => sp.customer_id == cust.customer_id).Count() > 0)
        //            {
        //                objcust = _db.customers.Where(c => c.customer_id == cust.customer_id).SingleOrDefault();
        //                nSalesPersonID = (int)objcust.sales_person_id;
        //            }

        //            if (_db.sales_persons.Where(sp => sp.sales_person_id == nSalesPersonID && sp.google_calendar_account != null && sp.google_calendar_account != "").Count() > 0)
        //            {
        //                objSP = _db.sales_persons.Where(sp => sp.sales_person_id == nSalesPersonID && sp.google_calendar_account != null && sp.google_calendar_account != "").SingleOrDefault();
        //                calendarId = objSP.google_calendar_id;
        //            }
        //            if (calendarId != "")
        //            {
        //                var scItem = _db.ScheduleCalendarTemps.FirstOrDefault(sc => sc.customer_id == cust.customer_id && sc.type_id == 2 && sc.event_id == id);
        //                if (scItem.google_event_id != "")
        //                {
        //                    var calendarEvent = new gCalendarEvent()
        //                    {
        //                        CalendarId = calendarId,
        //                        Id = scItem.google_event_id,
        //                        StartDate = Convert.ToDateTime(scItem.event_start),
        //                        EndDate = Convert.ToDateTime(scItem.event_end),
        //                        Description = description
        //                    };

        //                    var authenticator = GetAuthenticator(objSP.sales_person_id);
        //                    var service = new GoogleCalendarServiceProxy(authenticator);

        //                    service.UpdateEvent(calendarEvent);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion
    }

    //this method updates the event start and end time
    //this is called when a event is dragged or resized in the calendar
    public static void updateEventTime(int id, DateTime start, DateTime end, int customer_id, int estimate_id)
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        List<ScheduleCalendarTemp> objSchClndlist = new List<ScheduleCalendarTemp>();

        customer_estimate objCusEst = new customer_estimate();
        sales_person objSP = new sales_person();
        string operationCalendarId = string.Empty;
        operationCalendarId = ConfigurationManager.AppSettings["GoogleCalendarID"];
        //objCusEst = _db.customer_estimates.Single(ce => ce.customer_id == customer_id && ce.estimate_id == estimate_id);
        //if (_db.sales_persons.Where(sp => sp.sales_person_id == Convert.ToInt32(objCusEst.sales_person_id) && sp.google_calendar_account != null && sp.google_calendar_account != "").Count() > 0)
        //{
        //    objSP = _db.sales_persons.Where(sp => sp.sales_person_id == Convert.ToInt32(objCusEst.sales_person_id) && sp.google_calendar_account != null && sp.google_calendar_account != "").SingleOrDefault();
        //    calendarId = objSP.google_calendar_id;
        //}

        if (_db.ScheduleCalendarTemps.SingleOrDefault(sc => sc.event_id == id).type_id == 1) // Type ID 1 = Operation
        {
            try
            {
                var job_Start = _db.ScheduleCalendarTemps.FirstOrDefault(sc => sc.customer_id == customer_id && sc.estimate_id == estimate_id && sc.event_id == id).job_start_date;
                int nWk = GetWeek(Convert.ToDateTime(start), Convert.ToDateTime(job_Start));

                //string sql = "UPDATE ScheduleCalendarTemp SET  event_start='" + start + "', event_end='" + end + "', week_id='" + nWk + "' WHERE event_id=" + id;
                //_db.ExecuteCommand(sql, string.Empty);

                ScheduleCalendarTemp ocjSCTempUpdate = _db.ScheduleCalendarTemps.Where(sc => sc.event_id == id).FirstOrDefault();
                ocjSCTempUpdate.event_start = start;
                ocjSCTempUpdate.event_end = end;
                _db.SubmitChanges();

                //If Event is Parent
                List<ScheduleCalendarLinkTemp> objSCLinkTmpP = new List<ScheduleCalendarLinkTemp>();

                if (_db.ScheduleCalendarLinkTemps.Any(sl => sl.parent_event_id == id))
                {
                    objSCLinkTmpP = _db.ScheduleCalendarLinkTemps.Where(sl => sl.parent_event_id == id).ToList();

                    foreach (ScheduleCalendarLinkTemp slT in objSCLinkTmpP)
                    {
                        ScheduleCalendarTemp ocjSCTempC = _db.ScheduleCalendarTemps.FirstOrDefault(s => s.event_id == slT.child_event_id);


                        int nDays = Convert.ToDateTime(ocjSCTempC.event_end).Day - Convert.ToDateTime(ocjSCTempC.event_start).Day;

                        int nLagDays = (int)slT.lag;// Convert.ToDateTime(ocjSCTempC.event_start).Day - Convert.ToDateTime(ocjSCTempUpdate.event_end).Day;

                        if (slT.dependencyType == 1) // Start Same Time
                        {
                            ocjSCTempC.event_start = ocjSCTempUpdate.event_start;
                            ocjSCTempC.event_end = ocjSCTempUpdate.event_end;
                            _db.SubmitChanges();
                        }

                        if (slT.dependencyType == 2) // Start After Finish
                        {
                            ocjSCTempC.event_start = DateTime.Parse(GetDayOfWeek(Convert.ToDateTime(ocjSCTempUpdate.event_end).AddDays(1).ToShortDateString()) + " " + Convert.ToDateTime(ocjSCTempC.event_start).ToShortTimeString());
                            ocjSCTempC.event_end = DateTime.Parse(Convert.ToDateTime(ocjSCTempC.event_start).AddDays(nDays).ToShortDateString() + " " + Convert.ToDateTime(ocjSCTempC.event_end).ToShortTimeString());
                            _db.SubmitChanges();
                        }
                        if (slT.dependencyType == 3) // Offset Days
                        {
                            ocjSCTempC.event_start = DateTime.Parse(GetDayOfWeek(Convert.ToDateTime(ocjSCTempUpdate.event_end).AddDays(nLagDays+1).ToShortDateString()) + " " + Convert.ToDateTime(ocjSCTempC.event_start).ToShortTimeString());
                            ocjSCTempC.event_end = DateTime.Parse(Convert.ToDateTime(ocjSCTempC.event_start).AddDays(nDays).ToShortDateString() + " " + Convert.ToDateTime(ocjSCTempC.event_end).ToShortTimeString());
                            _db.SubmitChanges();
                        }
                    }
                }

                //If Event is Child
                ScheduleCalendarLinkTemp objSCLinkTmpC = new ScheduleCalendarLinkTemp();
                if (_db.ScheduleCalendarLinkTemps.Any(sl => sl.child_event_id == id))
                {
                    objSCLinkTmpC = _db.ScheduleCalendarLinkTemps.FirstOrDefault(sl => sl.child_event_id == id);

                    ScheduleCalendarTemp ocjSCTempP = _db.ScheduleCalendarTemps.FirstOrDefault(s => s.event_id == objSCLinkTmpC.parent_event_id);
                                      
                    DateTime pStartDate = Convert.ToDateTime(ocjSCTempP.event_start).Date;
                    DateTime pEndDate = Convert.ToDateTime(ocjSCTempP.event_end).Date;
                    
                    DateTime cStartDate = Convert.ToDateTime(ocjSCTempUpdate.event_start).Date;

                    int nDays = Convert.ToDateTime(ocjSCTempUpdate.event_start).Day - Convert.ToDateTime(ocjSCTempP.event_end).Day;

                    if (cStartDate == pStartDate) // Start Same Time
                    {
                        objSCLinkTmpC.dependencyType = 1;
                        objSCLinkTmpC.lag = 0;
                        _db.SubmitChanges();
                    }
                    else if (nDays == 1) // Start After Finish
                    {
                        objSCLinkTmpC.dependencyType = 2;
                        objSCLinkTmpC.lag = 1;
                        _db.SubmitChanges();
                    }
                    else // Offset Days
                    {
                        objSCLinkTmpC.dependencyType = 3;
                        objSCLinkTmpC.lag = nDays-1;
                        _db.SubmitChanges();
                    }
                }



                #region Blocked  Google Calendar UPDATE (Type ID 1, Operation)------------
                //try
                //{
                //    if (ConfigurationManager.AppSettings["IsProductionOpeartion"].ToString() == "True")
                //    {
                //        var scItem = _db.ScheduleCalendarTemps.Single(sc => sc.customer_id == customer_id && sc.estimate_id == estimate_id && sc.event_id == id);

                //        var calendarEvent = new gCalendarEvent()
                //            {
                //                CalendarId = operationCalendarId,
                //                Title = scItem.title.Trim(),
                //                Id = scItem.google_event_id,
                //                StartDate = Convert.ToDateTime(start),
                //                EndDate = Convert.ToDateTime(end),
                //                Description = scItem.description.Trim()
                //            };

                //        // var authenticator = GetAuthenticator(Convert.ToInt32(objCusEst.sales_person_id));
                //        var authenticator = GetAuthenticator(6);
                //        var service = new GoogleCalendarServiceProxy(authenticator);

                //        if (scItem.google_event_id != "")
                //        {
                //            service.UpdateEvent(calendarEvent);
                //        }
                //        else
                //        {

                //            string strGoogleEventId = service.CreateEvent(calendarEvent);

                //            string sql2 = "UPDATE ScheduleCalendarTemp SET  google_event_id ='" + strGoogleEventId + "' WHERE event_id=" + scItem.event_id;
                //            _db.ExecuteCommand(sql2, string.Empty);
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}
                #endregion

                // Update "week_id" in co_pricing_master ------------
                //List<co_pricing_master> objcpmList = new List<co_pricing_master>();

                //List<ScheduleCalendarTemp> objSCLList = new List<ScheduleCalendarTemp>();
                //objSCLList = _db.ScheduleCalendarTemps.Where(sc => sc.customer_id == customer_id && sc.estimate_id == estimate_id && sc.event_id == id).ToList();

                //foreach (ScheduleCalendarTemp objSC in objSCLList)
                //{
                //    objcpmList = _db.co_pricing_masters.Where(c => c.customer_id == objSC.customer_id && c.estimate_id == objSC.estimate_id
                //        && c.section_name == objSC.section_name && c.CalEventId == objSC.event_id).ToList();

                //    foreach (co_pricing_master objcpm in objcpmList)
                //    {
                //        objcpm.week_id = objSC.week_id;
                //        _db.SubmitChanges();
                //    }
                //}
                //-------------------------------------------
            }
            catch (Exception ex)
            {
                string msg = ex.StackTrace;
            }
        }
        else if (_db.ScheduleCalendarTemps.SingleOrDefault(sc => sc.event_id == id).type_id == 2) // Type ID 2 = Sales
        {
            string sql = "UPDATE ScheduleCalendarTemp SET  event_start='" + start + "', event_end='" + end + "' WHERE event_id=" + id;
            _db.ExecuteCommand(sql, string.Empty);
            string calendarId = string.Empty;

            #region Blocked  Google Calendar UPDATE (Type ID 2, Sales)------------
            //try
            //{
            //    customer objcust = new customer();
            //    sales_person nObjSP = new sales_person();
            //    int nSalesPersonID = 0;
            //    //string calendarId = string.Empty;
            //    if (ConfigurationManager.AppSettings["IsProduction"].ToString() == "True")
            //    {
            //        if (_db.customers.Where(sp => sp.customer_id == customer_id).Count() > 0)
            //        {
            //            objcust = _db.customers.Where(c => c.customer_id == customer_id).SingleOrDefault();
            //            nSalesPersonID = (int)objcust.sales_person_id;
            //        }

            //        if (_db.sales_persons.Where(sp => sp.sales_person_id == nSalesPersonID && sp.google_calendar_account != null && sp.google_calendar_account != "").Count() > 0)
            //        {
            //            nObjSP = _db.sales_persons.Where(sp => sp.sales_person_id == nSalesPersonID && sp.google_calendar_account != null && sp.google_calendar_account != "").SingleOrDefault();
            //            calendarId = nObjSP.google_calendar_id;
            //        }

            //        if (calendarId != "")
            //        {
            //            var scItem = _db.ScheduleCalendarTemps.FirstOrDefault(sc => sc.customer_id == customer_id && sc.type_id == 2 && sc.event_id == id);
            //            if (scItem.google_event_id != "")
            //            {
            //                var calendarEvent = new gCalendarEvent()
            //                {
            //                    CalendarId = calendarId,
            //                    Title = scItem.title.Trim(),
            //                    Id = scItem.google_event_id,
            //                    StartDate = Convert.ToDateTime(start),
            //                    EndDate = Convert.ToDateTime(end),
            //                    Description = scItem.description
            //                };

            //                var authenticator = GetAuthenticator(nObjSP.sales_person_id);
            //                var service = new GoogleCalendarServiceProxy(authenticator);

            //                service.UpdateEvent(calendarEvent);
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            #endregion

            customer cust = new customer();
            if (_db.customers.Where(c => c.customer_id == customer_id).Count() > 0)
                cust = _db.customers.Single(c => c.customer_id == customer_id);

            cust.appointment_date = start;
            _db.SubmitChanges();
        }
        else
        {
            string sql = "UPDATE ScheduleCalendarTemp SET  event_start='" + start + "', event_end='" + end + "' WHERE event_id=" + id;
            _db.ExecuteCommand(sql, string.Empty);
        }
    }

    //this method updates the All event start and end time
    //this is called when a event is dragged or resized in the calendar
    public static void updateEventTimeAll(int id, DateTime start, DateTime end, int customer_id, int estimate_id)
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        List<ScheduleCalendarTemp> objSchClndlist = new List<ScheduleCalendarTemp>();

        if (_db.ScheduleCalendarTemps.SingleOrDefault(sc => sc.event_id == id).type_id != 1)
        {
            string sql = "UPDATE ScheduleCalendarTemp SET  event_start='" + start + "', event_end='" + end + "' WHERE event_id=" + id;
            _db.ExecuteCommand(sql, string.Empty);
        }
        else // Type ID 1 = Operation
        {
            try
            {
                var selected_event_Start = _db.ScheduleCalendarTemps.FirstOrDefault(sc => sc.customer_id == customer_id && sc.estimate_id == estimate_id && sc.event_id == id).event_start;

                objSchClndlist = _db.ScheduleCalendarTemps.Where(sc => sc.customer_id == customer_id && sc.estimate_id == estimate_id && sc.event_start >= selected_event_Start).OrderBy(sc => sc.event_start).ToList();

                string dtStart = Convert.ToDateTime(start).ToShortDateString();

                // int nDays = start.Day - Convert.ToDateTime(selected_event_Start).Day;
                int nDays = (start.Date - Convert.ToDateTime(selected_event_Start)).Days;

                customer_estimate objCusEst = new customer_estimate();
                sales_person objSP = new sales_person();
                //string calendarId = string.Empty;
                string operationCalendarId = string.Empty;
                operationCalendarId = ConfigurationManager.AppSettings["GoogleCalendarID"];
                //objCusEst = _db.customer_estimates.Single(ce => ce.customer_id == customer_id && ce.estimate_id == estimate_id);
                //if (_db.sales_persons.Where(sp => sp.sales_person_id == Convert.ToInt32(objCusEst.sales_person_id) && sp.google_calendar_account != null && sp.google_calendar_account != "").Count() > 0)
                //{
                //    objSP = _db.sales_persons.Where(sp => sp.sales_person_id == Convert.ToInt32(objCusEst.sales_person_id) && sp.google_calendar_account != null && sp.google_calendar_account != "").SingleOrDefault();
                //    calendarId = objSP.google_calendar_id;
                //}
                int nt = 0;
                foreach (ScheduleCalendarTemp objsc in objSchClndlist)
                {
                    //if (nDays < 0)
                    //    nt = nDays;
                    //----Old
                    //objsc.event_start = DateTime.Parse(GetDayOfWeek(Convert.ToDateTime(objsc.event_start).AddDays(nDays).ToShortDateString()) + " " + "12:00:00 AM");
                    //objsc.event_end = Convert.ToDateTime(objsc.event_start);
                    //-------
                    objsc.event_start = DateTime.Parse(GetDayOfWeek(Convert.ToDateTime(objsc.event_start).AddDays(nDays).ToShortDateString()) + " " + Convert.ToDateTime(objsc.event_start).ToShortTimeString());
                    int nDateDifference = (Convert.ToDateTime(objsc.event_end) - Convert.ToDateTime(objsc.event_start)).Days;
                    DateTime dtEndDate = Convert.ToDateTime(objsc.event_start).AddDays(nDateDifference).AddDays(nDays);
                    // objsc.event_end = DateTime.Parse(GetDayOfWeek(Convert.ToDateTime(dtEndDate).ToShortDateString()) + " " + Convert.ToDateTime(dtEndDate).ToShortTimeString());
                    objsc.event_end = dtEndDate;
                    objsc.last_updated_date = DateTime.Now;
                    //int nWk = GetWeek(Convert.ToDateTime(objsc.event_start), Convert.ToDateTime(objsc.job_start_date));
                    //objsc.week_id = nWk;

                    //objsc.event_start = DateTime.Parse(GetDayOfWeek(dtStart) + " " + "06:00:00.000");
                    //dtStart = Convert.ToDateTime(dtStart).AddDays(7).ToShortDateString();
                    //objsc.event_end = Convert.ToDateTime(objsc.event_start).AddHours(1);
                    //objsc.last_updated_date = DateTime.Now;
                    //int nWk = GetWeek(Convert.ToDateTime(objsc.event_start), Convert.ToDateTime(objsc.job_start_date));
                    //objsc.week_id = nWk;

                    #region Blocked  Google Calendar UPDATE (Type ID 1, Operation)------------
                    //try
                    //{
                    //    if (ConfigurationManager.AppSettings["IsProductionOpeartion"].ToString() == "True")
                    //    {
                    //        var calendarEvent = new gCalendarEvent()
                    //        {
                    //            CalendarId = operationCalendarId,
                    //            Id = objsc.google_event_id,
                    //            Title = objsc.title.Trim(),
                    //            StartDate = Convert.ToDateTime(objsc.event_start),
                    //            EndDate = Convert.ToDateTime(objsc.event_end),
                    //            Description = objsc.description.Trim()
                    //        };

                    //        // var authenticator = GetAuthenticator(Convert.ToInt32(objCusEst.sales_person_id));
                    //        var authenticator = GetAuthenticator(6);
                    //        var service = new GoogleCalendarServiceProxy(authenticator);

                    //        if (objsc.google_event_id != "")
                    //        {
                    //            service.UpdateEvent(calendarEvent);
                    //        }
                    //        else
                    //        {
                    //            objsc.google_event_id = service.CreateEvent(calendarEvent);
                    //            //string sql2 = "UPDATE ScheduleCalendarTemp SET  google_event_id ='" + service.CreateEvent(calendarEvent) + "' WHERE event_id=" + objsc.event_id;
                    //            //_db.ExecuteCommand(sql2, string.Empty);
                    //        }
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw ex;
                    //}
                    #endregion

                    _db.SubmitChanges();

                }

                // Update "week_id" in co_pricing_master ------------
                //List<co_pricing_master> objcpmList = new List<co_pricing_master>();

                //List<ScheduleCalendarTemp> objSCLList = new List<ScheduleCalendarTemp>();
                //objSCLList = _db.ScheduleCalendarTemps.Where(sc => sc.customer_id == customer_id && sc.estimate_id == estimate_id).ToList();

                //foreach (ScheduleCalendarTemp objSC in objSCLList)
                //{
                //    objcpmList = _db.co_pricing_masters.Where(c => c.customer_id == objSC.customer_id && c.estimate_id == objSC.estimate_id
                //        && c.section_name == objSC.section_name && c.CalEventId == objSC.event_id).ToList();

                //    foreach (co_pricing_master objcpm in objcpmList)
                //    {
                //        objcpm.week_id = objSC.week_id;
                //        _db.SubmitChanges();
                //    }
                //}
                //-------------------------------------------
            }
            catch (Exception ex)
            {
                string msg = ex.StackTrace;
            }
        }
    }

    public static void UpdateEventNotes(int id, DateTime start, DateTime end, string operation_notes)
    {
        DataClassesDataContext _db = new DataClassesDataContext();


        string sql = "UPDATE ScheduleCalendarTemp SET event_start='" + start + "', event_end='" + end + "',  operation_notes='" + operation_notes + "' WHERE event_id=" + id;
        _db.ExecuteCommand(sql, string.Empty);

    }

    public static int GetWeek(DateTime Event_Start, DateTime Job_Start)
    {
        TimeSpan t = Event_Start - Job_Start;
        var dt1 = t.Days + 1;
        int wk = 1;
        int nwk = 0;
        for (int i = 7; i < dt1; i += 7)
        {
            if (dt1 <= i)
            {
                break;
            }
            wk++;
        }
        nwk = wk;
        return nwk;
    }

    //this mehtod deletes event with the id passed in.
    public static void deleteEvent(int id)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        try
        {
            ScheduleCalendarTemp objSchClnd = _db.ScheduleCalendarTemps.SingleOrDefault(c => c.event_id == id);
            int nTypeID = (int)objSchClnd.type_id;
            int nCustomerID = (int)objSchClnd.customer_id;
            int nSalesPersonID = 0;
            customer objcust = new customer();
            sales_person objSP = new sales_person();
            DateTime dt = Convert.ToDateTime("1900-01-01");

            //Google Calendar
            string calendarId = string.Empty;
            // calendarId = ConfigurationManager.AppSettings["GoogleCalendarID"];

            #region Blocked  Google Calendar DELETE
            //if (ConfigurationManager.AppSettings["IsProduction"].ToString() == "True")
            //{
            //    if (_db.customers.Where(sp => sp.customer_id == nCustomerID).Count() > 0)
            //    {
            //        objcust = _db.customers.Where(c => c.customer_id == nCustomerID).SingleOrDefault();
            //        nSalesPersonID = (int)objcust.sales_person_id;
            //    }

            //    if (_db.sales_persons.Where(sp => sp.sales_person_id == nSalesPersonID && sp.google_calendar_account != null && sp.google_calendar_account != "").Count() > 0)
            //    {
            //        objSP = _db.sales_persons.Where(sp => sp.sales_person_id == nSalesPersonID && sp.google_calendar_account != null && sp.google_calendar_account != "").SingleOrDefault();
            //        calendarId = objSP.google_calendar_id;
            //    }

            //    if (calendarId != "")
            //    {
            //        List<ScheduleCalendarTemp> sclist = _db.ScheduleCalendarTemps.Where(sc => sc.event_id == id && sc.type_id == 2).ToList();
            //        foreach (ScheduleCalendarTemp sc in sclist)
            //        {
            //            if (sc.google_event_id != "")
            //            {
            //                var authenticator = GetAuthenticator(objSP.sales_person_id);
            //                var service = new GoogleCalendarServiceProxy(authenticator);
            //                service.DeleteEvent(calendarId, sc.google_event_id); // Delete
            //            }
            //        }
            //    }
            //}
            #endregion


            string sql1 = "DELETE ScheduleCalendarTemp WHERE event_id=" + id;
            _db.ExecuteCommand(sql1, string.Empty);

            string sqlLinkp = "DELETE ScheduleCalendarLinkTemp WHERE parent_event_id=" + id;
            _db.ExecuteCommand(sqlLinkp, string.Empty);

            string sqlLinkc = "DELETE ScheduleCalendarLinkTemp WHERE child_event_id=" + id;
            _db.ExecuteCommand(sqlLinkc, string.Empty);
           
            //string sql2 = "UPDATE customers SET appointment_date='" + dt + "' WHERE customer_id=" + nCustomerID;
            //_db.ExecuteCommand(sql2, string.Empty);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //this mehtod deletes event with the id passed in.
    public static void cancelEvent()
    {
        //DataClassesDataContext _db = new DataClassesDataContext();

        //ScheduleCalendarTemp objSC = new ScheduleCalendarTemp();
        //Customer objCust = new Customer();
        //ServiceCall objServiceCall = new ServiceCall();

        //string strSerTktNumber = "0";
        //int nCustomerID = 0;
        //int nServiceCallId = 0;

        //string sql2 = "";

        //strSerTktNumber = (string)System.Web.HttpContext.Current.Session["STktNo"];
        //nCustomerID = (int)System.Web.HttpContext.Current.Session["cid"];
        //nServiceCallId = (int)System.Web.HttpContext.Current.Session["scallid"];

        //if (_db.ServiceTickets.Where(s => s.CustomerID == nCustomerID && s.SerTktNumber == strSerTktNumber && s.ServiceCallId == nServiceCallId).Count() == 0)
        //{
        //    sql2 = "DELETE ServiceCall WHERE CustomerID=" + nCustomerID + " AND ServiceCallId=" + nServiceCallId + " AND CallNumber ='" + strSerTktNumber + "'";
        //    _db.ExecuteCommand(sql2, string.Empty);
        //}
    }

    //this method adds events to the database
    public static int addEvent(CalendarEvent cevent)
    {
        try
        {
            //add event to the database and return the primary key of the added event row
            int key = 0;
            DataClassesDataContext _db = new DataClassesDataContext();
            ScheduleCalendarTemp objSC = new ScheduleCalendarTemp();
            co_pricing_master objCOPM = new co_pricing_master();
            customer_estimate cus_est = new customer_estimate();
            location objLocation = new location();

            int nCustomerID = 0;
            int nEstimateID = 0;
            int nEmployeeID = 0;
            if (cevent.employee_id != null)
                nEmployeeID = cevent.employee_id;

            string strEmployeeName = "";
            if (cevent.employee_name != null)
                strEmployeeName = cevent.employee_name;

            int nTypeId = 0;
            int nPricingId = 0;
            string strSectionName = "";
            string StrLocationName = "";
            DateTime dtJobStartDate = cevent.start;
            DateTime dtStartDate = cevent.start;
            DateTime dtEndDate = cevent.end;
            string strUName = "";
            string strTitle = cevent.title;
            string strCssClassName = "fc-default";
            string GoogleEventID = "";

            // For Operation Calendar
            if (System.Web.HttpContext.Current.Session["TypeID"] != null)
            {
                nTypeId = (int)System.Web.HttpContext.Current.Session["TypeID"];
                if (nTypeId == 1) // Operation Calendar
                {
                    if (System.Web.HttpContext.Current.Session["CusId"] != null)
                    {
                        nCustomerID = (int)System.Web.HttpContext.Current.Session["CusId"];

                        if (System.Web.HttpContext.Current.Session["sEstSelectedByCustSearch"] != null)
                            nEstimateID = (int)System.Web.HttpContext.Current.Session["sEstSelectedByCustSearch"];

                        nTypeId = 11; // Opration Extra event TypeID for Holidays Date event (Operation Calendar) with Customer ID
                    }
                }
            }


            if (System.Web.HttpContext.Current.Session["uname"] != null)
            {
                strUName = (string)System.Web.HttpContext.Current.Session["uname"];
            }
            if (cevent.cssClassName != null)
            {
                strCssClassName = cevent.cssClassName;
            }

            if (System.Web.HttpContext.Current.Session["cid"] != null)
            {
                nCustomerID = (int)System.Web.HttpContext.Current.Session["cid"];
                dtStartDate = DateTime.Parse(dtStartDate.ToShortDateString() + " " + dtStartDate.ToShortTimeString());
                dtEndDate = DateTime.Parse(dtEndDate.ToShortDateString() + " " + dtEndDate.ToShortTimeString());
                if (System.Web.HttpContext.Current.Session["TypeID"] != null)
                {
                    nTypeId = (int)System.Web.HttpContext.Current.Session["TypeID"];
                }
            }
            if (System.Web.HttpContext.Current.Session["eid"] != null)
            {
                nEstimateID = (int)System.Web.HttpContext.Current.Session["eid"];
            }
            //if (System.Web.HttpContext.Current.Session["empid"] != null)
            //{
            //    nEmployeeID = (int)System.Web.HttpContext.Current.Session["empid"];
            //}

            customer objcust = new customer();
            sales_person objSP = new sales_person();
            string strCustInfo = "";
            if (nTypeId == 2 && nCustomerID != 0)// Type ID 2 = Sales
            {
                #region Blocked Google Calendar
                //int nSalesPersonID = 0;
                //string calendarId = string.Empty;

                //// calendarId = ConfigurationManager.AppSettings["GoogleCalendarID"];

                //if (ConfigurationManager.AppSettings["IsProduction"].ToString() == "True")
                //{
                //    if (_db.customers.Where(sp => sp.customer_id == nCustomerID).Count() > 0)
                //    {
                //        objcust = _db.customers.Where(c => c.customer_id == nCustomerID).SingleOrDefault();
                //        nSalesPersonID = (int)objcust.sales_person_id;
                //        strCustInfo = "\n\n" + objcust.first_name1.Trim() + " " + objcust.last_name1.Trim() + "\n\n" + objcust.phone.Trim() + "\n\n" + objcust.email.Trim();
                //    }

                //    if (_db.sales_persons.Where(sp => sp.sales_person_id == nSalesPersonID && sp.google_calendar_account != null && sp.google_calendar_account != "").Count() > 0)
                //    {
                //        objSP = _db.sales_persons.Where(sp => sp.sales_person_id == nSalesPersonID && sp.google_calendar_account != null && sp.google_calendar_account != "").SingleOrDefault();
                //        calendarId = objSP.google_calendar_id;
                //    }

                //    if (calendarId != "")
                //    {
                //        //// Google Calendar DELETE------------
                //        List<ScheduleCalendarTemp> sclist = _db.ScheduleCalendarTemps.Where(sc => sc.customer_id == nCustomerID && sc.type_id == 2).ToList();
                //        foreach (ScheduleCalendarTemp sc in sclist)
                //        {
                //            if (sc.google_event_id != "")
                //            {
                //                var authenticator = GetAuthenticator(objSP.sales_person_id);
                //                var service = new GoogleCalendarServiceProxy(authenticator);
                //                service.DeleteEvent(calendarId, sc.google_event_id); // Delete
                //            }
                //        }
                //        //// Calendar DELETE------------


                //        //Google Calendar Insert----------------------------------------------------------

                //        var calendarEvent = new gCalendarEvent()
                //        {
                //            CalendarId = calendarId,
                //            Title = (objcust.first_name1.Trim() + " " + objcust.last_name1.Trim() + " " + objcust.phone.Trim()).Trim(),
                //            Location = objcust.address + ", " + objcust.city + ", " + objcust.state + " " + objcust.zip_code,
                //            StartDate = dtStartDate,
                //            EndDate = dtEndDate,
                //            Description = cevent.description + strCustInfo,
                //            ColorId = 1
                //        };

                //        var authenticatorr = GetAuthenticator(objSP.sales_person_id);
                //        var servicee = new GoogleCalendarServiceProxy(authenticatorr);
                //        GoogleEventID = servicee.CreateEvent(calendarEvent);

                //        //Google Calendar Insert End Code----------------------------------------------------------
                //    }
                //}
                #endregion

                //Customer Appointment Insert/Update
                customer cust = new customer();
                cust = _db.customers.Single(c => c.customer_id == nCustomerID);
                cust.appointment_date = dtStartDate;
                cust.notes = cevent.description;
                _db.SubmitChanges();
            }
            else if (nTypeId == 2)// Type ID 2 = Sales
            {
                nTypeId = 22;// Sales Extra event TypeID  (Sales Calendar) without Customer ID
            }

            int nEventId = 1;

            if (_db.ScheduleCalendarTemps.Any())
            {
                nEventId = Convert.ToInt32(_db.ScheduleCalendarTemps.Max(e => e.event_id)) + 1;
            }

            objSC.event_id = nEventId;
            objSC.title = strTitle;
            objSC.description = cevent.description + strCustInfo;
            objSC.event_start = dtStartDate;
            objSC.event_end = dtEndDate;
            objSC.customer_id = nCustomerID;
            objSC.estimate_id = nEstimateID;
            objSC.employee_id = nEmployeeID;
            objSC.employee_name = strEmployeeName;
            objSC.section_name = strSectionName;
            objSC.location_name = StrLocationName;
            objSC.create_date = DateTime.Now;
            objSC.type_id = nTypeId;
            objSC.last_updated_by = strUName;
            objSC.last_updated_date = DateTime.Now;
            //   objSC.week_id = 0;
            objSC.job_start_date = dtJobStartDate;
            objSC.co_pricing_list_id = nPricingId;
            objSC.cssClassName = strCssClassName;
            objSC.google_event_id = GoogleEventID;
            objSC.operation_notes = "";
            objSC.is_complete = false;
            objSC.IsEstimateActive = true;
            _db.ScheduleCalendarTemps.InsertOnSubmit(objSC);
            _db.SubmitChanges();



            key = Convert.ToInt32(objSC.event_id);

            //System.Web.HttpContext.Current.Session["cid"] = null;
            //System.Web.HttpContext.Current.Session["eid"] = null;
            //System.Web.HttpContext.Current.Session["empid"] = null;
            // System.Web.HttpContext.Current.Session["TypeID"] = null;

            return key;
        }
        catch (Exception ex)
        {
            return 0;
        }

    }

    private static string GetDayOfWeekWithOutHoliday(string strdt)
    {
        int cnt = 0;
        DateTime dt = Convert.ToDateTime(strdt);

        if (dt.DayOfWeek == DayOfWeek.Saturday)
            cnt--;
        else if (dt.DayOfWeek == DayOfWeek.Sunday)
            cnt = -2;

        return dt.AddDays(cnt).ToShortDateString();
    }

    private static string GetDayOfWeek(string strdt)
    {
        int cnt = 0;
        DateTime dt = Convert.ToDateTime(strdt);

        if (dt.DayOfWeek == DayOfWeek.Saturday)
            cnt = +2;
        else if (dt.DayOfWeek == DayOfWeek.Sunday)
            cnt++;
        else if (IsHoliday(dt))
        {
            DateTime hdt = dt.AddDays(1);

            if (hdt.DayOfWeek == DayOfWeek.Saturday)
                cnt = +3;
            else if (hdt.DayOfWeek == DayOfWeek.Sunday)
                cnt = +2;
            else
                cnt++;
        }

        return dt.AddDays(cnt).ToShortDateString();
    }

    private static bool IsHoliday(DateTime dt)
    {
        bool IsHoliday = false;
        DateTime date = DateTime.Parse(dt.ToShortDateString());
        HolidayCalculator hc = new HolidayCalculator(date, "Holidays.xml");
        foreach (HolidayCalculator.Holiday h in hc.OrderedHolidays)
        {
            if (h.Date.ToShortDateString() == date.ToShortDateString())
            {
                IsHoliday = true;
            }
        }
        return IsHoliday;
    }

    private static GoogleAuthenticator GetAuthenticator(int salespersonid)
    {
        //var authenticator = (GoogleAuthenticator)System.Web.HttpContext.Current.Session["authenticator"];

        //if (authenticator == null || !authenticator.IsValid)
        //{
        DataClassesDataContext _db = new DataClassesDataContext();
        // Get a new Authenticator using the Refresh Token
        int nUserID = salespersonid;
        var refreshToken = _db.GoogleRefreshTokens.FirstOrDefault(c => c.UserID == salespersonid).RefreshToken;
        var authenticator = GoogleAuthorizationHelper.RefreshAuthenticator(refreshToken);
        //    System.Web.HttpContext.Current.Session["authenticator"] = authenticator;
        //}

        return authenticator;
    }

}
