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

public partial class schedule_calendar_new : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string startdate = "";
        if (Request.QueryString.GetValues("startdate") != null)
        {
            startdate = Convert.ToDateTime(Request.QueryString.Get("startdate")).ToShortDateString();
        }
        else
        {
            startdate = System.DateTime.Now.AddDays(-System.DateTime.Now.Day + 1).ToShortDateString();//beginning of the month
        }
        lnkPrev1.NavigateUrl = "schedule_calendar_new.aspx?startdate=" + Convert.ToDateTime(startdate).AddMonths(-1).ToShortDateString();
        lnkPrev1.Text = Convert.ToDateTime(startdate).AddMonths(-1).ToString("MMMM");
        lnkPrev2.NavigateUrl = "schedule_calendar_new.aspx?startdate=" + Convert.ToDateTime(startdate).AddMonths(-2).ToShortDateString();
        lnkPrev2.Text = Convert.ToDateTime(startdate).AddMonths(-2).ToString("MMMM");
        lnkPrev3.NavigateUrl = "schedule_calendar_new.aspx?startdate=" + Convert.ToDateTime(startdate).AddMonths(-3).ToShortDateString();
        lnkPrev3.Text = Convert.ToDateTime(startdate).AddMonths(-3).ToString("MMMM");

        lnkPrevD1.NavigateUrl = "schedule_calendar_new.aspx?startdate=" + Convert.ToDateTime(startdate).AddMonths(-1).ToShortDateString();
        lnkPrevD1.Text = Convert.ToDateTime(startdate).AddMonths(-1).ToString("MMMM");
        lnkPrevD2.NavigateUrl = "schedule_calendar_new.aspx?startdate=" + Convert.ToDateTime(startdate).AddMonths(-2).ToShortDateString();
        lnkPrevD2.Text = Convert.ToDateTime(startdate).AddMonths(-2).ToString("MMMM");
        lnkPrevD3.NavigateUrl = "schedule_calendar_new.aspx?startdate=" + Convert.ToDateTime(startdate).AddMonths(-3).ToShortDateString();
        lnkPrevD3.Text = Convert.ToDateTime(startdate).AddMonths(-3).ToString("MMMM");


        lnkNext1.NavigateUrl = "schedule_calendar_new.aspx?startdate=" + Convert.ToDateTime(startdate).AddMonths(1).ToShortDateString();
        lnkNext1.Text = Convert.ToDateTime(startdate).AddMonths(1).ToString("MMMM");
        lnkNext2.NavigateUrl = "schedule_calendar_new.aspx?startdate=" + Convert.ToDateTime(startdate).AddMonths(2).ToShortDateString();
        lnkNext2.Text = Convert.ToDateTime(startdate).AddMonths(2).ToString("MMMM");

        lnkNextD1.NavigateUrl = "schedule_calendar_new.aspx?startdate=" + Convert.ToDateTime(startdate).AddMonths(1).ToShortDateString();
        lnkNextD1.Text = Convert.ToDateTime(startdate).AddMonths(1).ToString("MMMM");
        lnkNextD2.NavigateUrl = "schedule_calendar_new.aspx?startdate=" + Convert.ToDateTime(startdate).AddMonths(2).ToShortDateString();
        lnkNextD2.Text = Convert.ToDateTime(startdate).AddMonths(2).ToString("MMMM");


        //PopulateCalendar(startdate);

    }
    //private void PopulateCalendar(string date)
    //{

    //    System.Web.UI.WebControls.Label lblReport = new Label();
    //    lblReport.Height = System.Web.UI.WebControls.Unit.Pixel(800);
    //    lblReport.Width = System.Web.UI.WebControls.Unit.Pixel(1250);

    //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
    //    string dtStart = "";
    //    string dtEnd = "";
    //    dtStart = date;
    //    dtEnd = Convert.ToDateTime(dtStart.ToString()).AddDays(35).ToShortDateString();
    //    DataGrid dgSchedule = new DataGrid();
    //    dgSchedule.ID = "dgSchedule";
    //    //int dayInterval = 0;
    //    DateTime dt = Convert.ToDateTime(dtStart);
    //    dtInit = dt;
    //    sb.Append("<table border=0>");
    //    var item = from sc in _db.schedule_calendars
    //               join sp in _db.sales_persons on sc.sales_person_id equals sp.sales_person_id
    //               join cus in _db.customers on sc.customer_id equals cus.customer_id
    //               //where sc.customer_id == 1 && sc.sales_person_id == 8
    //               select new ScheduleCalendar()
    //               {
    //                   SalesPerson = sp.first_name + " " + sp.last_name,
    //                   Customer = cus.first_name1 + " " + cus.last_name1,
    //                   Description = sc.Description,
    //                   Dates = sc.Dates,
    //                   location_id = sc.location_id
    //               };

    //    for (int dayCount = 0; dayCount < 34; dayCount++)
    //    {
    //        sb.Append("<table valign='top' Table width=1200 style='BORDER-RIGHT: #6fa53a 1px solid; BORDER-TOP: #6fa53a 1px solid; BORDER-LEFT: #6fa53a 1px solid; BORDER-BOTTOM: #6fa53a 1px solid' cellSpacing='0' cellPadding='0'><tr><td align=left valign='top'>");
    //        for (int dayPerRow = 0; dayPerRow < 7; dayPerRow++)
    //        {
    //            dtStart = dtInit.ToShortDateString();

    //            //string startdate = "";
    //            string enddate = "";

    //            try
    //            {
    //                dtInit = DateTime.Parse(dtStart);
    //                enddate = dtEnd;//DateTime.Parse(startdate).AddDays(7).ToShortDateString();
    //            }
    //            catch
    //            {
    //            }

    //            string txtGrid = "";

    //            sb.Append("<td valign='top'><Table valign='top' width='200px' style='BORDER-RIGHT: #6fa53a 1px solid; BORDER-TOP: #6fa53a 1px solid; BORDER-LEFT: #6fa53a 1px solid; BORDER-BOTTOM: #6fa53a 1px solid' cellSpacing='0' cellPadding='0'>");
    //            //edited by Md. Showkot Ali,MCT
    //            sb.Append("<tr><td width='200px' valign='top' class='SmallTitleBlackbold8'><a href='InstallationCalendarEntry.aspx?startdate=" + dtInit.ToShortDateString() + "'>" + dtInit.ToLongDateString() + "</a></td></tr>");

    //            dt = dtInit;
    //            int apptId = 0;
    //            decimal numCrewTotal = 0;

    //            //for(int i = 0;i<19;i++)
    //            {
    //                //dt = dt.AddMinutes(30);

    //                foreach (ScheduleCalendar sc in item)
    //                {
    //                //loop thru all appts.
                        
    //                    DateTime dtAppt = DateTime.Parse(sc.Dates);

    //                    if (dt.DayOfYear == DateTime.Parse(appt.StartTime).DayOfYear)
    //                    {


    //                        if (appt.AppointmentTypeId == 5)// Temporary Used For Kectchen And Bath
    //                        {
    //                            if (appt.AppointmentId != apptId)
    //                            {
    //                                apptId = appt.AppointmentId;
    //                                txtGrid += "<div style='background-color:Silver;width:200px;'><span class=SmallTitleBlack><a href='InstallationCalendarEntry.aspx?jid=0&" +
    //                                           "apptId=" + apptId + "'>[KITCHEN AND BATH]</a></span>";
    //                                txtGrid += "<br><span>#Crew(" + appt.NumCrew + ")</span>";
    //                                txtGrid += "<br><span>" + appt.Notes + "</span></div>";
    //                            }
    //                        }
    //                    }

    //                }//foreach appt

    //                if (txtGrid.Length == 0)
    //                {
    //                    txtGrid = "<div style='background-color:white;'><hr></div>";
    //                }
    //                else
    //                {
    //                    txtGrid += "<br><br>Total Crew: " + numCrewTotal.ToString();
    //                }
    //                sb.Append("<tr valign='top'><td class='SmallTitleBlack' valign='top' style='background-color:white;' height='600px'>" + txtGrid + "</td></tr>");

    //                txtGrid = "";
    //            }//foreach starttime

    //            dtInit = dtInit.AddDays(1);
    //            sb.Append("</table></td>");
    //            dayCount++;
    //        }//foreach day
    //    }//foreach daycount
    //    sb.Append("</tr></table>");
    //    lblReport.Text = sb.ToString();
    //    plcGrid.Controls.Add(lblReport);

    //}

}
