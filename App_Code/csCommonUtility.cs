using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.IO;

using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Data.SqlClient;
using System.Reflection;
using System.Net.Mail;

/// <summary>
/// Summary description for csCommonUtility
/// </summary>
public class csCommonUtility
{
	public csCommonUtility()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private static string SHORTING = "ASC";

    public static string FormatPhoneNo(string sNumber)
    {


        if (sNumber.Trim().Length == 0)
            return sNumber;

        string sReturn = "";

        try
        {

            sReturn = sNumber.Trim().Replace("-", "");

            sReturn = sReturn.Insert(3, "-");

            sReturn = sReturn.Insert(7, "-");
        }
        catch
        {
            sReturn = sNumber;
        }

        return sReturn;

    }
    public static void GridItemSorting(object sender, GridViewSortEventArgs e, DataTable table)
    {
        GridView grd = (GridView)sender;

        string strShort = e.SortExpression + " " + SHORTING;

        DataView dv = table.DefaultView;
        dv.Sort = strShort;
        grd.DataSource = dv.Table;
        grd.DataBind();
        if (SHORTING == "ASC")
            SHORTING = SHORTING.Replace("ASC", "DESC");
        else
            SHORTING = SHORTING.Replace("DESC", "ASC");
    }

    private static string[] DayArray = { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
    public static string GetSystemMessage(string str)
    {
        return "<span style='color:blue;'><b>System MSG: </b></span><span style='color:green;'>" + str + "</span>";

    }
    public static string GetSystemErrorMessage(string str)
    {
        return "<span style='color:blue;'><b>System MSG: </b></span><span style='color:red;'>" + str + "</span>";
    }

    public static string GetSystemRequiredMessage(string str)
    {
        return "<span style='color:blue;'><b>System MSG: </b></span><br/><span style='color:red;'>" + str + "</span>";
    }
    public static string GetSystemRequiredMessage2(string str)
    {
        return "<span style='color:red;'>" + str + "</span>";
    }

    public static int GetDateDifference(string StartDayName, string EndDayName)
    {

        int nDif = Array.IndexOf(DayArray, EndDayName) - Array.IndexOf(DayArray, StartDayName);
        if (nDif <= 0) nDif += 7;

        return nDif;
    }
    private static int GetDateDifference_Neg(string StartDayName, string EndDayName)
    {

        int nDif = Array.IndexOf(DayArray, EndDayName) - Array.IndexOf(DayArray, StartDayName);


        return nDif;
    }
    public static DateTime GetDateByDayName_Next(string DayName)
    {
        DateTime dToday = DateTime.Today;
        if (dToday.ToString("dddd") != DayName)
        {
            int nDif = GetDateDifference_Neg(dToday.ToString("dddd"), DayName);
            if (nDif <= 0) nDif += 7;
            dToday = dToday.AddDays(nDif);
        }

        return dToday;
    }

    public static DateTime GetDateByDayName_Prev(string DayName)
    {
        DateTime dToday = DateTime.Today;
        if (dToday.ToString("dddd") != DayName)
        {
            dToday = dToday.AddDays(GetDateDifference_Neg(dToday.ToString("dddd"), DayName));
        }

        return dToday;
    }

    public static DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
    {
        DataTable dtReturn = new DataTable();

        // column names
        PropertyInfo[] oProps = null;

        if (varlist == null) return dtReturn;

        foreach (T rec in varlist)
        {
            // Use reflection to get property names, to create table, Only first time, others will follow
            if (oProps == null)
            {
                oProps = ((System.Type)rec.GetType()).GetProperties();
                foreach (PropertyInfo pi in oProps)
                {
                    System.Type colType = pi.PropertyType;

                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                    == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }

                    dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                }
            }

            DataRow dr = dtReturn.NewRow();

            foreach (PropertyInfo pi in oProps)
            {
                dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                (rec, null);
            }

            dtReturn.Rows.Add(dr);
        }
        return dtReturn;
    }

    public static DataTable GetDataTable(string strQ)
    {
        DataTable table = new DataTable();
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["CRMDBConnectionString"].ConnectionString);
        try
        {
            SqlCommand cmd = new SqlCommand(strQ, con);
            con.Open();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds);
            table = ds.Tables[0];
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {

            con.Close();
            con.Dispose();
        }
        return table;
    }

    public static company_profile GetCompanyProfile()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        company_profile objCom = _db.company_profiles.Single(c => c.client_id == 1);

        return objCom;
    }

    public static string GetCompanyOwnerName()
    {
        string CompanyOwnerName = System.Configuration.ConfigurationManager.AppSettings["CompanyOwnerName"].ToString();

        return CompanyOwnerName;
    }

    public static customer_estimate GetCustomerEstimateInfo(int nCusId, int nEstId)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        customer_estimate cus_est = new customer_estimate();
        if (_db.customer_estimates.Any(ce => ce.customer_id == nCusId && ce.client_id == 1 && ce.estimate_id == nEstId))
        {
            cus_est = _db.customer_estimates.Single(ce => ce.customer_id == nCusId && ce.client_id == 1 && ce.estimate_id == nEstId);
        }

        return cus_est;
    }

    public static void SendMail(string From, string To, string Cc, string Bcc, string Subject, string Body)
    {

        MailMessage msg = new MailMessage();
        msg.From = new MailAddress(From);
        if (Cc.Length > 0)
            msg.CC.Add(Cc);
        if (Bcc.Length > 0)
            msg.Bcc.Add(Bcc);
        msg.To.Add(To);
        msg.Subject = Subject;
        msg.IsBodyHtml = true;
        msg.Body = Body;
        msg.Priority = MailPriority.High;


        SendByLocalhost(msg);

    }

    public static void SendByLocalhost(MailMessage msg)
    {

        try
        {
            SmtpClient smtp = new SmtpClient();
            smtp.Host = System.Configuration.ConfigurationManager.AppSettings["smtpserver"];
            string strSendEmail = System.Configuration.ConfigurationManager.AppSettings["SendEmail"];
            if (strSendEmail == "Yes")
            {
                smtp.Send(msg);
            }

        }
        catch
        {

        }

    }

    public static string ExtractNumber(string original) { return new string(original.Where(c => Char.IsDigit(c)).ToArray()); }

    public static string GetPhoneFormat(string str)
    {
        string NoFormat = string.Empty;
        string Phone = string.Empty;

        //NoFormat = str.Replace(" ", "").Replace( "-", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("+", "").Replace(".","");

        if (str.Trim().Length > 0)
        {
            NoFormat = ExtractNumber(str);
            if (NoFormat.Length > 9)
            {
                string countrycode = NoFormat.Substring(0, 3);
                string Areacode = NoFormat.Substring(3, 3);
                string number = NoFormat.Substring(6);
                Phone = "(" + countrycode + ") " + Areacode + "-" + number;
            }
            else
                Phone = NoFormat;
        }

        return Phone;

    }

    public static string GetProjectUrl()
    {
        string ProjectUrl = System.Configuration.ConfigurationManager.AppSettings["ProjectUrl"].ToString();

        return ProjectUrl;
    }

    public class setDMUserData
    {
        public int CustomerId { get; set; }
        public string Role { get; set; }
    }
}