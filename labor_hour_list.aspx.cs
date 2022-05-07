using DataStreams.Csv;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class labor_hour_list : System.Web.UI.Page
{
    public DataTable dtSection;
    int nPageNo = 0;
    int nPageNoEmp = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);
            if (Session["oUser"] == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["LoginPage"].ToString());
            }
            if (Page.User.IsInRole("timetrack004") == false)
            {
                // No Permission Page.
                Response.Redirect("nopermission.aspx");
            }

            if (Request.QueryString.Get("isCrew") != null)
            {
                int IsCrew = Convert.ToInt32(Request.QueryString.Get("isCrew"));
                if (IsCrew == 1)
                {
                    TabContainer4.ActiveTabIndex = 0;
                }
                else
                {
                    TabContainer4.ActiveTabIndex = 1;
                }
            }
            else
            {
                TabContainer4.ActiveTabIndex = 0;
            }

            BindCrew();
            BindEmployee();
           
            if (Session["CPUFilter"] != null)
            {
                Hashtable ht = (Hashtable)Session["CPUFilter"];
                ddlItemPerPage.SelectedIndex = Convert.ToInt32(ht["ItemPerPage"].ToString());
                ddlInstaller.SelectedValue = ht["Installer"].ToString();
                nPageNo = Convert.ToInt32(ht["PageNo"].ToString());
                txtStartDate.Text = ht["StartDate"].ToString();
                txtEndDate.Text = ht["EndDate"].ToString();

             
            }

            if (Session["CPUFilterEpm"] != null)
            {
                Hashtable htemp = (Hashtable)Session["CPUFilterEpm"];
                ddlItemPerPageEmployee.SelectedIndex = Convert.ToInt32(htemp["ItemPerPageEmp"].ToString());
                ddEmployee.SelectedValue = htemp["Employee"].ToString();
                nPageNoEmp = Convert.ToInt32(htemp["PageNoEmp"].ToString());
                txtEmployeeStartDate.Text = htemp["StartDateEmp"].ToString();
                txtEmployeeEndDate.Text = htemp["EndDateEmp"].ToString();


            }
          BindLaborHourTracking();
          BindEmployeeLaborHourTracking();  
          

        }
    }

   

    private void BindLaborHourTracking()
    {
        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            string strCondition = string.Empty;

            if (ddlInstaller.SelectedItem.Text != "All")
            {
                strCondition = " where IsCrew=1 AND UserID = " + Convert.ToInt32(ddlInstaller.SelectedValue);
            }
            else
            {
                strCondition = " where IsCrew=1 ";
            }
            
            if (txtStartDate.Text.Trim() != "" && txtEndDate.Text.Trim() != "")
            {

                DateTime dt1 = Convert.ToDateTime(txtStartDate.Text.Trim());
                DateTime dt2 = Convert.ToDateTime(txtEndDate.Text.Trim());

                if (strCondition.Length == 0)
                {

                    strCondition = " WHERE IsCrew=1 AND labor_date >= '" + dt1 + "' and labor_date <'" + dt2.AddDays(1) + "'";
                }
                else
                {
                    strCondition += " AND labor_date >= '" + dt1 + "' and labor_date <'" + dt2.AddDays(1) + "'";
                }


            }
           
                string strQ = "select [GPSTrackID],[StartPlace] ,[MakeStopPlace],[EndPlace],[Distance],[Time],[UserID],[IsCrew],[CustomerName],[section_id], " +
                        " [SectionName],[StartTime] ,[EndTime],[customer_id],[Estimate_id],[labor_date],DeviceName from GPSTracking  " + strCondition + " order by labor_date desc ";
                IEnumerable<CrewTrack> clist = _db.ExecuteQuery<CrewTrack>(strQ, string.Empty);
                Session.Add("nLoaborHour", csCommonUtility.LINQToDataTable(clist));

                if (Session["CPUFilter"] != null)
                {
                    Hashtable ht = (Hashtable)Session["CPUFilter"];
                    nPageNo = Convert.ToInt32(ht["PageNo"].ToString());
                    GetLaberTracking(nPageNo);
                }
                else
                {
                    GetLaberTracking(0);
                }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void BindCrew()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        string strQ = "select first_name+' '+last_name AS crew_name,crew_id from Crew_Details WHERE is_active = 1 order by crew_name asc";
        List<CrewDe> mList = _db.ExecuteQuery<CrewDe>(strQ, string.Empty).ToList();
        ddlInstaller.DataSource = mList;
        ddlInstaller.DataTextField = "crew_name";
        ddlInstaller.DataValueField = "crew_id";
        ddlInstaller.DataBind();
        ddlInstaller.Items.Insert(0, "All");
    }


    protected void GetLaberTracking(int nPageNo)
    {

        if (Session["nLoaborHour"] != null)
        {
            DataTable dtLaborHour = (DataTable)Session["nLoaborHour"];
            grdLaberTrack.DataSource = dtLaborHour;
            grdLaberTrack.PageIndex = nPageNo;
            grdLaberTrack.AllowPaging = true;
            grdLaberTrack.PageSize = Convert.ToInt32(ddlItemPerPage.SelectedValue);
            grdLaberTrack.DataKeyNames = new string[] { "GPSTrackID", "SectionName", "EndPlace", "UserID", "StartPlace", "StartTime", "EndTime" };
            grdLaberTrack.DataBind();


            Hashtable ht = new Hashtable();
            ht.Add("ItemPerPage", ddlItemPerPage.SelectedIndex);
           if (ddlInstaller.SelectedItem.Text != "All")
            ht.Add("Installer", Convert.ToInt32(ddlInstaller.SelectedValue));
            else
               ht.Add("Installer", 0);
            ht.Add("PageNo", nPageNo);
            ht.Add("StartDate", txtStartDate.Text);
            ht.Add("EndDate", txtEndDate.Text);

            Session["CPUFilter"] = ht;
        }

        lblCurrentPageNo.Text = Convert.ToString(nPageNo + 1);
        if (nPageNo == 0)
        {
            btnPrevious.Enabled = false;
            btnPrevious0.Enabled = false;
        }
        else
        {
            btnPrevious.Enabled = true;
            btnPrevious0.Enabled = true;
        }

        if (grdLaberTrack.PageCount == nPageNo + 1)
        {
            btnNext.Enabled = false;
            btnNext0.Enabled = false;
        }
        else
        {
            btnNext.Enabled = true;
            btnNext0.Enabled = true;
        }
    }




    protected void grdLaberTrack_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                DataClassesDataContext _db = new DataClassesDataContext();

                int nGPSId = Convert.ToInt32(grdLaberTrack.DataKeys[e.Row.RowIndex].Values[0].ToString());
                string SectionName = grdLaberTrack.DataKeys[e.Row.RowIndex].Values[1].ToString();
                string EndPlace = grdLaberTrack.DataKeys[e.Row.RowIndex].Values[2].ToString();
                int nUserId = Convert.ToInt32(grdLaberTrack.DataKeys[e.Row.RowIndex].Values[3].ToString());
                string StartPlace = grdLaberTrack.DataKeys[e.Row.RowIndex].Values[4].ToString();
                DateTime StartTime = Convert.ToDateTime(grdLaberTrack.DataKeys[e.Row.RowIndex].Values[5]);
                DateTime EndTime = Convert.ToDateTime(grdLaberTrack.DataKeys[e.Row.RowIndex].Values[6]);

                Label lblTotalHours = (Label)e.Row.FindControl("lblTotalHours");
                Label lblLunch = (Label)e.Row.FindControl("lblLunch");
                lblLunch.Text = ".50";

                if (Convert.ToDateTime(EndTime).Year != 2000)
                {
                    TimeSpan span = EndTime.Subtract(StartTime);
                     if (span.Days > 0)
                        lblTotalHours.Text = span.Days + ":" + span.Hours + ":" + span.Minutes;
                    else
                        lblTotalHours.Text = span.ToString(@"hh\:mm");


                   
                }
                else
                {
                    e.Row.Cells[5].Text = "";
                    lblTotalHours.Text = "";
                }
                if (StartPlace.Contains("USA") || StartPlace.Contains("usa"))
                {
                    e.Row.Cells[2].Text = StartPlace.Remove(StartPlace.Length - 5, 5);
                }
                else
                {
                    if (StartPlace == "0" || StartPlace == "" || StartPlace == null)
                    {
                        e.Row.Cells[2].Text = "";
                    }
                    else
                    {
                        e.Row.Cells[2].Text = StartPlace;
                    }

                }
                if (EndPlace == "0")
                {
                    e.Row.Cells[3].Text = "";
                }
                else
                {
                    if (EndPlace.Contains("USA") || EndPlace.Contains("usa"))
                    {
                        e.Row.Cells[3].Text = EndPlace.Remove(EndPlace.Length - 5, 5);
                    }
                    else
                    {
                        e.Row.Cells[3].Text = EndPlace;
                    }
                }
                Crew_Detail objCrew = new Crew_Detail();
                objCrew = _db.Crew_Details.SingleOrDefault(c => c.crew_id == nUserId);
                if (objCrew != null)
                    e.Row.Cells[1].Text = objCrew.full_name;
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                imgDelete.OnClientClick = "return confirm('This will permanently delete this time entry.');";
                imgDelete.CommandArgument = nGPSId.ToString();


            }
            catch (Exception ex)
            { throw ex; }
        }
    }

    protected void DeleteFile(object sender, EventArgs e)
    {
        try
        {
            userinfo obj = (userinfo)Session["oUser"];
            ImageButton imgDelete = (ImageButton)sender;
            int nGPSId = Convert.ToInt32(imgDelete.CommandArgument);
            DataClassesDataContext _db = new DataClassesDataContext();
            string strQ = "";
            lblResult.Text = "";
            lblResultEmp.Text = "";

            strQ = "insert into DeleteGPSTracking " +
                          " SELECT [GPSTrackID] ,[StartPlace] ,[StartLatitude],[StartLogitude] ,[EndLatitude] ,[EndLogitude],[MakeStopPlace], " +
                          " [EndPlace],[Distance],[Time],[CreatedDate] ,[UserID] ,[CustomerName] ,[section_id],[SectionName] ,[StartTime], " +
                          " [EndTime],[customer_id],[Estimate_id],[labor_date],[deviceName],[IsCrew]," + "'" + obj.username + "'" + ",getdate() " +
                          " FROM GPSTracking " +
                          " WHERE GPSTrackID ='" + nGPSId + "'";
            _db.ExecuteCommand(strQ, string.Empty);

             strQ = "delete from GPSTracking where GPSTrackID=" + nGPSId;
            _db.ExecuteCommand(strQ, string.Empty);


            var nList = (from dgps in _db.GPSTrackingDetails where dgps.GPSTrackID == nGPSId select dgps).ToList();

            DeleteGPSTrackingDetail objDGPS = null;

            foreach(var li in nList)
            {
                objDGPS = new DeleteGPSTrackingDetail();
                objDGPS.ID = li.ID;
                objDGPS.GPSTrackID = li.GPSTrackID;
                objDGPS.Latitude = li.Latitude;
                objDGPS.Longitude = li.Longitude;
                objDGPS.InputType = li.InputType;
                objDGPS.CreateDate = li.CreateDate;

                _db.DeleteGPSTrackingDetails.InsertOnSubmit(objDGPS);
                _db.SubmitChanges();
            }
            strQ = "delete from GPSTrackingDetails where GPSTrackID=" + nGPSId;
            _db.ExecuteCommand(strQ, string.Empty);
            lblResult.Text = csCommonUtility.GetSystemMessage("Labor tracking has been deleted successfully.");
            BindLaborHourTracking();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    protected void btnView_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnView.ID, btnView.GetType().Name, "Click"); 
        string strRequired = string.Empty;
        lblResult.Text = "";
        lblResultEmp.Text = "";
        try
        {
            Convert.ToDateTime(txtStartDate.Text.Trim());
        }
        catch
        {
            strRequired += "Start Date is required.<br/>";

        }

        try
        {
            Convert.ToDateTime(txtEndDate.Text.Trim());
            if (Convert.ToDateTime(txtStartDate.Text.Trim()) >= Convert.ToDateTime(txtEndDate.Text.Trim()))
            {
                strRequired += "End Date must be greater than Start Date.<br/>";

            }
        }
        catch
        {
            strRequired += "End Date is required.<br/>";

        }
        if (strRequired.Length > 0)
        {
            lblResult.Text = csCommonUtility.GetSystemRequiredMessage(strRequired);
            return;
        }
      //  GetLaberTracking(0);
        if (chkTotalhours.Checked)
        {
            BindSubTotalHours();
        }
        else
        {
           BindLaborHourTracking();
        }
      
    }

    private void BindSubTotalHours()
    {
        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();
           string strCondition = string.Empty;
           if (ddlInstaller.SelectedItem.Text != "All")
           {
               strCondition = " where IsCrew=1 AND UserID = " + Convert.ToInt32(ddlInstaller.SelectedValue);
           }
           else
           {
               strCondition = " where IsCrew=1 ";
           }

           if (txtStartDate.Text.Trim() != "" && txtEndDate.Text.Trim() != "")
           {

               DateTime dt1 = Convert.ToDateTime(txtStartDate.Text.Trim());
               DateTime dt2 = Convert.ToDateTime(txtEndDate.Text.Trim());

               if (strCondition.Length == 0)
               {

                   strCondition = " WHERE IsCrew=1 AND labor_date >= '" + dt1 + "' and labor_date <'" + dt2.AddDays(1) + "'";
               }
               else
               {
                   strCondition += " AND labor_date >= '" + dt1 + "' and labor_date <'" + dt2.AddDays(1) + "'";
               }


           }
          string strQ = "select [GPSTrackID],[StartPlace] ,[MakeStopPlace],[EndPlace],[Distance],[Time],[UserID],[IsCrew],[CustomerName],[section_id], " +
                         " [SectionName],[StartTime] ,[EndTime],[customer_id],[Estimate_id],[labor_date],DeviceName from GPSTracking  " + strCondition + " order by UserID,labor_date DESC ";
            IEnumerable<CrewTrack> clist = _db.ExecuteQuery<CrewTrack>(strQ, string.Empty);

            DataTable dataTable = csCommonUtility.LINQToDataTable(clist);

            if (dataTable.Rows.Count > 0)
            {
                grdLaberTrack.DataSource = dataTable;
                grdLaberTrack.PageIndex = 0;
                grdLaberTrack.AllowPaging = false;
                grdLaberTrack.DataKeyNames = new string[] { "GPSTrackID", "SectionName", "EndPlace", "UserID", "StartPlace", "StartTime", "EndTime" };
                grdLaberTrack.DataBind();
            }
            else
            {
                grdLaberTrack.DataSource = null;
                grdLaberTrack.PageIndex = 0;
                grdLaberTrack.AllowPaging = false;
                grdLaberTrack.DataKeyNames = new string[] { "GPSTrackID", "SectionName", "EndPlace", "UserID", "StartPlace", "StartTime", "EndTime" };
                grdLaberTrack.DataBind();
            }

            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void grdLaberTrack_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (chkTotalhours.Checked)
            {
                for (int i = subTotalRowIndex; i < grdLaberTrack.Rows.Count; i++)
                {
                    
                    DateTime StartTime = Convert.ToDateTime(grdLaberTrack.Rows[i].Cells[9].Text);
                    string eTime = (grdLaberTrack.Rows[i].Cells[10].Text).ToString();
                    if (eTime != null && eTime != "")
                    {
                        DateTime EndTime = Convert.ToDateTime(grdLaberTrack.Rows[i].Cells[10].Text);
                       
                        if (EndTime.Year != 2000)
                        {
                           TimeSpan span = EndTime.Subtract(StartTime);
                            decimal totalMinutes = (span.Days * 24 * 60 + span.Hours * 60 + span.Minutes);
                            subTotal += totalMinutes;
                        }
                    }
                 }
                if (subTotal >= 60)
                {
                    hour = (int)subTotal / 60;
                    subTotal = subTotal % 60;
                }
                else
                {
                    hour = 0;
                    subTotal = subTotal % 60;
                }
                if (subTotal < 10)
                    TotalM = hour.ToString() + ":0" + subTotal;
                else
                    TotalM = hour.ToString() + ":" + subTotal;
                if (grdLaberTrack.Rows.Count > 0)
                    this.AddTotalRow("Total", TotalM);
                
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }
   
    }

    int currentId = 0;
    decimal subTotal = 0;
    int subTotalRowIndex = 0;
    int hour = 0;
    string TotalM = "";

     protected void grdLaberTrack_RowCreated(object sender, GridViewRowEventArgs e)
      {
    try
    {
        if (chkTotalhours.Checked==true)
        {

            subTotal = 0;
            if (e.Row != null && e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem is DataRowView && (e.Row.DataItem as DataRowView).DataView.Table != null)
                {
                    DataTable dt = (e.Row.DataItem as DataRowView).DataView.ToTable(); //(e.Row.DataItem as DataRowView).DataView.Table;
                    int UserID = Convert.ToInt32(dt.Rows[e.Row.RowIndex]["UserID"]);
                   
                    if (UserID != currentId)
                    {
                        if (e.Row.RowIndex > 0)
                        {
                            for (int i = subTotalRowIndex; i < e.Row.RowIndex; i++)
                            {
                                DateTime StartTime = Convert.ToDateTime(grdLaberTrack.Rows[i].Cells[9].Text);
                                  string eTime = (grdLaberTrack.Rows[i].Cells[10].Text).ToString();
                                  if (eTime != null&&eTime != "")
                                  {
                                      DateTime EndTime = Convert.ToDateTime(grdLaberTrack.Rows[i].Cells[10].Text);
                                      if (EndTime.Year != 2000)
                                      {
                                          TimeSpan span = EndTime.Subtract(StartTime);
                                          decimal totalMinutes = (span.Days * 24 * 60 + span.Hours * 60 + span.Minutes);
                                          subTotal += totalMinutes;
                                      }
                                  }
                            }

                            if (subTotal >= 60)
                            {
                                hour = (int)subTotal / 60;
                                subTotal = subTotal % 60;
                            }
                            else
                            {
                                hour = 0;
                                subTotal = subTotal % 60;
                            }


                            if(subTotal<10)
                              TotalM = hour.ToString()+":0"+subTotal;
                            else
                                TotalM = hour.ToString() + ":" + subTotal;

                            if (grdLaberTrack.Rows.Count > 0)
                                this.AddTotalRow("Total", TotalM);

                            subTotalRowIndex = e.Row.RowIndex;
                        }
                        currentId = UserID;
                    }

                }
            }
        }
    }
    catch( Exception ex)
    {
        throw ex;
    }
}
    private void AddTotalRow(string labelText, string value)
    {
        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
        row.BackColor = ColorTranslator.FromHtml("#F9F9F9");

        row.Cells.AddRange(new TableCell[9] { new TableCell (), new TableCell (),new TableCell (), new TableCell (),new TableCell (),new TableCell (),//Empty Cell
                                        new TableCell { Text = labelText, HorizontalAlign = HorizontalAlign.Right,CssClass="cellColor"},
                                        new TableCell { Text = value, HorizontalAlign = HorizontalAlign.Center,CssClass="cellColor" },new TableCell () });

        grdLaberTrack.Controls[0].Controls.Add(row);
    }
    protected void ddlInstaller_SelectedIndexChanged(object sender, EventArgs e)
    {
       // chkTotalhours.Checked = false;
         lblResult.Text = "";
         lblResultEmp.Text = "";
         if (chkTotalhours.Checked == true)
         {
             BindSubTotalHours();
             chkEmployeeTotalHours.Checked = false;
         }
         else
         {
             chkTotalhours.Checked = false;
             BindLaborHourTracking();
         }
    }
    protected void lnkViewAll_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, lnkViewAll.ID, lnkViewAll.GetType().Name, "Click"); 
        lblResult.Text = "";
        ddlInstaller.SelectedIndex = -1;
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        chkTotalhours.Checked = false;

        if (Session["CPUFilter"] != null)
        {
            Session.Remove("CPUFilter");
          
        }
        ddlItemPerPage.SelectedIndex = 0;
        BindLaborHourTracking();
        
    }

    protected void grdLaberTrack_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, grdLaberTrack.ID, grdLaberTrack.GetType().Name, "PageIndexChanging"); 
        GetLaberTracking(e.NewPageIndex);
    }
    
    protected void btnNext_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnNext.ID, btnNext.GetType().Name, "Click"); 
        int nCurrentPage = 0;
        nCurrentPage = Convert.ToInt32(lblCurrentPageNo.Text);
        GetLaberTracking(nCurrentPage);
    }
    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnPrevious.ID, btnPrevious.GetType().Name, "Click"); 
        int nCurrentPage = 0;
        nCurrentPage = Convert.ToInt32(lblCurrentPageNo.Text);
        GetLaberTracking(nCurrentPage - 2);
    }
    protected void ddlItemPerPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, ddlItemPerPage.ID, ddlItemPerPage.GetType().Name, "SelectedIndexChanged"); 
        chkTotalhours.Checked = false;
        GetLaberTracking(0);
    }
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("laborhourdetails.aspx?isCrew=1");
    }

    protected void btnExpCustList_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnExpCustList.ID, btnExpCustList.GetType().Name, "Click"); 
          DataTable tmpTable = LoadTmpDataTable();
         DataClassesDataContext _db = new DataClassesDataContext();

         string strRequired = string.Empty;
         lblResult.Text = "";
        string strCondition = string.Empty;

        if (ddlInstaller.SelectedItem.Text != "All")
        {
            strCondition = " where IsCrew=1 AND UserID = " + Convert.ToInt32(ddlInstaller.SelectedValue);
        }
        else
        {
            strCondition = " where IsCrew=1 ";
        }
        if (txtStartDate.Text.Trim() != "" && txtEndDate.Text.Trim() != "")
        {
           
            try
            {
                Convert.ToDateTime(txtStartDate.Text.Trim());
            }
            catch
            {
                strRequired += "Start Date is required.<br/>";

            }

            try
            {
                Convert.ToDateTime(txtEndDate.Text.Trim());
                if (Convert.ToDateTime(txtStartDate.Text.Trim()) >= Convert.ToDateTime(txtEndDate.Text.Trim()))
                {
                    strRequired += "End Date must be greater than Start Date.<br/>";

                }
            }
            catch
            {
                strRequired += "End Date is required.<br/>";

            }
            if (strRequired.Length > 0)
            {
                lblResult.Text = csCommonUtility.GetSystemRequiredMessage(strRequired);
                return;
            }


            DateTime dt1 = Convert.ToDateTime(txtStartDate.Text.Trim());
            DateTime dt2 = Convert.ToDateTime(txtEndDate.Text.Trim());

            if (strCondition.Length == 0)
            {

                strCondition = " WHERE IsCrew=1 AND labor_date >= '" + dt1 + "' and labor_date <'" + dt2.AddDays(1) + "'";
                // strCondition = " WHERE labor_date  between '" + dt1 + "' and '" + dt2 + "'";
            }
            else
            {
                strCondition += " AND labor_date >= '" + dt1 + "' and labor_date <'" + dt2.AddDays(1) + "'";
            }

           
        }

       

        string strQ = "select [GPSTrackID],[StartPlace] ,[MakeStopPlace],[EndPlace],[Distance],[Time],[CreatedDate],[UserID],[IsCrew],[CustomerName],[section_id], " +
                     " [SectionName],[StartTime] ,[EndTime],[customer_id],[Estimate_id],[labor_date] from GPSTracking  " + strCondition + " order by labor_date desc ";
          DataTable dtReport = csCommonUtility.GetDataTable(strQ);
        if (dtReport.Rows.Count> 0)
        {
            foreach (DataRow dr in dtReport.Rows)
            {   string StartPlace = "";
                string EndPlace = "";
                DataRow drNew = tmpTable.NewRow();
                StartPlace = dr["StartPlace"].ToString();
                EndPlace = dr["EndPlace"].ToString();
                Crew_Detail objCrew = new Crew_Detail();
                objCrew = _db.Crew_Details.SingleOrDefault(c => c.crew_id == Convert.ToInt32(dr["UserID"]));
                drNew["Labor Date"] = Convert.ToDateTime(dr["labor_date"]).ToString("MM-dd-yyyy");
                if (objCrew != null)
                    drNew["Crew Name"] = objCrew.full_name;
                 drNew["Customer Name"] = dr["CustomerName"].ToString();
                if (dr["SectionName"].ToString() != "Select")
                    drNew["Section"] = dr["SectionName"].ToString();
                if (StartPlace.Contains("USA") || StartPlace.Contains("usa"))
                {
                    drNew["Start Location"] = StartPlace.Remove(StartPlace.Length - 5, 5);
                }
                else
                {
                    if (StartPlace == "0" || StartPlace == "" || StartPlace == null)
                    {
                        drNew["Start Location"] = "";
                    }
                    else
                    {
                        drNew["Start Location"] = StartPlace;
                    }
                }
             if (EndPlace == "0")
                {
                    drNew["End Location"] = "";
                }
                else
                {
                    if (EndPlace.Contains("USA") || EndPlace.Contains("usa"))
                    {
                        drNew["End Location"] = EndPlace.Remove(EndPlace.Length - 5, 5);
                    }
                    else
                    {
                        drNew["End Location"] = EndPlace;
                    }
                }

             drNew["Start Time"] = Convert.ToDateTime(dr["StartTime"]).ToShortTimeString();
             if (Convert.ToDateTime(dr["EndTime"]).Year != 2000)
                 drNew["End Time"] = Convert.ToDateTime(dr["EndTime"]).ToShortTimeString();

                drNew["Lunch Start Time"] = "12 PM";
                drNew["Lunch End Time"] = "12:30 PM";

                drNew["Lunch End Time"] = "12:30 PM";
                drNew["Lunch (Hrs)"] = ".50";

                if (Convert.ToDateTime(dr["EndTime"]).Year!=2000)
                {
                    DateTime  StartTime = Convert.ToDateTime(dr["StartTime"].ToString());
                    DateTime EndTime = Convert.ToDateTime(dr["EndTime"].ToString());
                    TimeSpan span = EndTime.Subtract(StartTime);

                    if (span.Days > 0)
                        drNew["Total (Hr:Min)"] = span.Days + ":" + span.Hours + ":" + span.Minutes;
                    else
                        drNew["Total (Hr:Min)"] = span.ToString(@"hh\:mm"); 
                   
                 }
                else
                {

                    drNew["Total (Hr:Min)"] = "";
                }
            
                tmpTable.Rows.Add(drNew);
            }

            Response.Clear();
            Response.ClearHeaders();

            using (CsvWriter writer = new CsvWriter(Response.OutputStream, ',', System.Text.Encoding.Default))
            {
                writer.WriteAll(tmpTable, true);
            }
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment; filename=LaborTrackingList.csv");
            Response.End();
        }
        else
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage("No records found.");
        }
    }
    private DataTable LoadTmpDataTable()
    {
        DataTable table = new DataTable();
        table.Columns.Add("Labor Date", typeof(string));
        table.Columns.Add("Crew Name", typeof(string));
        table.Columns.Add("Customer Name", typeof(string));
        table.Columns.Add("Section", typeof(string));
        table.Columns.Add("Start Location", typeof(string));
        table.Columns.Add("End Location", typeof(string));
        table.Columns.Add("Start Time", typeof(string));
        table.Columns.Add("End Time", typeof(string));
        table.Columns.Add("Lunch Start Time", typeof(string));
        table.Columns.Add("Lunch End Time", typeof(string));
        table.Columns.Add("Lunch (Hrs)", typeof(string));
        table.Columns.Add("Total (Hr:Min)", typeof(string));
       
       return table;
    }
    protected void chkTotalhours_CheckedChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, chkTotalhours.ID, chkTotalhours.GetType().Name, "Click"); 

        if (chkTotalhours.Checked==true)
        {
            BindSubTotalHours();
            chkEmployeeTotalHours.Checked = false;
        }
        else
        {
            chkTotalhours.Checked = false;
            BindLaborHourTracking();
        }
    }


    // Employee Information

    private void BindEmployee()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        string strQ = "select first_name+' '+last_name AS username,user_id from user_info WHERE is_active = 1 and IsTimeClock=1 order by username asc";
        List<userinfo> mList = _db.ExecuteQuery<userinfo>(strQ, string.Empty).ToList();
        ddEmployee.DataSource = mList;
        ddEmployee.DataTextField = "username";
        ddEmployee.DataValueField = "user_id";
        ddEmployee.DataBind();
        ddEmployee.Items.Insert(0, "All");
    }
    private void BindEmployeeLaborHourTracking()
    {
        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();

            string strCondition = string.Empty;


            if (ddEmployee.SelectedItem.Text != "All")
            {
                strCondition = " where IsCrew=0 AND UserID = " + Convert.ToInt32(ddEmployee.SelectedValue);
            }
            else
            {
                strCondition = " where IsCrew=0 ";
            }

            if (txtEmployeeStartDate.Text.Trim() != "" && txtEmployeeEndDate.Text.Trim() != "")
            {

                DateTime dt1 = Convert.ToDateTime(txtEmployeeStartDate.Text.Trim());
                DateTime dt2 = Convert.ToDateTime(txtEmployeeEndDate.Text.Trim());

                if (strCondition.Length == 0)
                {

                    strCondition = " WHERE IsCrew=0 AND labor_date >= '" + dt1 + "' and labor_date <'" + dt2.AddDays(1) + "'";
                }
                else
                {
                    strCondition += " AND labor_date >= '" + dt1 + "' and labor_date <'" + dt2.AddDays(1) + "'";
                }


            }

            string strQ = "select [GPSTrackID],[StartPlace] ,[MakeStopPlace],[EndPlace],[Distance],[Time],[UserID],[IsCrew],[CustomerName],[section_id], " +
                    " [SectionName],[StartTime] ,[EndTime],[customer_id],[Estimate_id],[labor_date],DeviceName from GPSTracking  " + strCondition + " order by labor_date desc ";
            IEnumerable<CrewTrack> clist = _db.ExecuteQuery<CrewTrack>(strQ, string.Empty);
            Session.Add("nEmpLoaborHour", csCommonUtility.LINQToDataTable(clist));

            if (Session["CPUFilterEpm"] != null)
            {
                Hashtable htEmp = (Hashtable)Session["CPUFilterEpm"];
                nPageNoEmp = Convert.ToInt32(htEmp["PageNoEmp"].ToString());
                GetEmployeeLaberTracking(nPageNoEmp);
            }
            else
            {
                GetEmployeeLaberTracking(0);
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void GetEmployeeLaberTracking(int nPageNoEmp)
    {

        if (Session["nEmpLoaborHour"] != null)
        {
            DataTable dtLaborHour = (DataTable)Session["nEmpLoaborHour"];
            gdrEmployeeTimeTracker.DataSource = dtLaborHour;
            gdrEmployeeTimeTracker.PageIndex = nPageNoEmp;
            gdrEmployeeTimeTracker.AllowPaging = true;
            gdrEmployeeTimeTracker.PageSize = Convert.ToInt32(ddlItemPerPageEmployee.SelectedValue);
            gdrEmployeeTimeTracker.DataKeyNames = new string[] { "GPSTrackID", "SectionName", "EndPlace", "UserID", "StartPlace", "StartTime", "EndTime" };
            gdrEmployeeTimeTracker.DataBind();


            Hashtable htEmp = new Hashtable();
            htEmp.Add("ItemPerPageEmp", ddlItemPerPageEmployee.SelectedIndex);
            if (ddEmployee.SelectedItem.Text != "All")
                htEmp.Add("Employee", Convert.ToInt32(ddEmployee.SelectedValue));
            else
                htEmp.Add("Employee", 0);
            htEmp.Add("PageNoEmp", nPageNoEmp);
            htEmp.Add("StartDateEmp", txtEmployeeStartDate.Text);
            htEmp.Add("EndDateEmp", txtEmployeeEndDate.Text);

            Session["CPUFilterEpm"] = htEmp;
        }

        lblCurrentPageNoEmp.Text = Convert.ToString(nPageNoEmp + 1);
        if (nPageNoEmp == 0)
        {
            btnPreviousEmp.Enabled = false;
            btnPrevious0Emp.Enabled = false;
        }
        else
        {
            btnPreviousEmp.Enabled = true;
            btnPrevious0Emp.Enabled = true;
        }

        if (gdrEmployeeTimeTracker.PageCount == nPageNoEmp + 1)
        {
            btnNextEmp.Enabled = false;
            btnNext0Emp.Enabled = false;
        }
        else
        {
            btnNextEmp.Enabled = true;
            btnNext0Emp.Enabled = true;
        }
    }



    private void BindEmployeeSubTotalHours()
    {
        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            string strCondition = string.Empty;
            if (ddEmployee.SelectedItem.Text != "All")
            {
                strCondition = " where IsCrew=0 AND UserID = " + Convert.ToInt32(ddEmployee.SelectedValue);
            }
            else
            {
                strCondition = " where IsCrew=0 ";
            }

            if (txtEmployeeStartDate.Text.Trim() != "" && txtEmployeeEndDate.Text.Trim() != "")
            {

                DateTime dt1 = Convert.ToDateTime(txtEmployeeStartDate.Text.Trim());
                DateTime dt2 = Convert.ToDateTime(txtEmployeeEndDate.Text.Trim());

                if (strCondition.Length == 0)
                {

                    strCondition = " WHERE IsCrew=0 AND labor_date >= '" + dt1 + "' and labor_date <'" + dt2.AddDays(1) + "'";
                }
                else
                {
                    strCondition += " AND labor_date >= '" + dt1 + "' and labor_date <'" + dt2.AddDays(1) + "'";
                }


            }
            string strQ = "select [GPSTrackID],[StartPlace] ,[MakeStopPlace],[EndPlace],[Distance],[Time],[UserID],[IsCrew],[CustomerName],[section_id], " +
                              " [SectionName],[StartTime] ,[EndTime],[customer_id],[Estimate_id],[labor_date],DeviceName from GPSTracking  " + strCondition + " order by UserID,labor_date DESC ";
            IEnumerable<CrewTrack> clist2 = _db.ExecuteQuery<CrewTrack>(strQ, string.Empty);

            DataTable dt=csCommonUtility.LINQToDataTable(clist2);
            if(dt.Rows.Count>0)
            {
                gdrEmployeeTimeTracker.DataSource = dt;
                gdrEmployeeTimeTracker.PageIndex = 0;
                gdrEmployeeTimeTracker.AllowPaging = false;
                gdrEmployeeTimeTracker.DataKeyNames = new string[] { "GPSTrackID", "SectionName", "EndPlace", "UserID", "StartPlace", "StartTime", "EndTime" };
                gdrEmployeeTimeTracker.DataBind();
            }
            else
            {
                gdrEmployeeTimeTracker.DataSource = null;
                gdrEmployeeTimeTracker.PageIndex = 0;
                gdrEmployeeTimeTracker.AllowPaging = false;
                gdrEmployeeTimeTracker.DataKeyNames = new string[] { "GPSTrackID", "SectionName", "EndPlace", "UserID", "StartPlace", "StartTime", "EndTime" };
                gdrEmployeeTimeTracker.DataBind();
            }


        }
       catch (Exception ex)
        {
            throw ex;
        }
    }

    int currentIdEmp = 0;
    decimal subTotalEmp = 0;
    int subTotalRowIndexEmp = 0;
    string TotalME = "";
    int hourE = 0;
    protected void gdrEmployeeTimeTracker_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (chkEmployeeTotalHours.Checked)
            {
                for (int i = subTotalRowIndexEmp; i < gdrEmployeeTimeTracker.Rows.Count; i++)
                {

                    DateTime StartTime = Convert.ToDateTime(gdrEmployeeTimeTracker.Rows[i].Cells[9].Text);
                    string eTime = (gdrEmployeeTimeTracker.Rows[i].Cells[10].Text).ToString();
                    if (eTime != null && eTime != "")
                    {
                        DateTime EndTime = Convert.ToDateTime(gdrEmployeeTimeTracker.Rows[i].Cells[10].Text);
                      
                        if (EndTime.Year != 2000)
                        {
                            TimeSpan span = EndTime.Subtract(StartTime);
                            decimal totalHours = (span.Days * 24 * 60 + span.Hours * 60 + span.Minutes);
                            subTotalEmp += totalHours;
                        }
                    }
                }

                if (subTotalEmp >= 60)
                {
                    hourE = (int)subTotalEmp / 60;
                    subTotalEmp = subTotalEmp % 60;
                }
                else
                {
                    hourE = 0;
                    subTotalEmp = subTotalEmp % 60;
                }
                if (subTotalEmp < 10)
                    TotalME = hourE.ToString() + ":0" + subTotalEmp.ToString();
                else
                    TotalME = hourE.ToString() + ":" + subTotalEmp.ToString();

                if (gdrEmployeeTimeTracker.Rows.Count > 0)
                    this.AddTotalRowEmp("Total", TotalME);
                
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void gdrEmployeeTimeTracker_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (chkEmployeeTotalHours.Checked == true)
            {

                subTotalEmp = 0;
                if (e.Row != null && e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (e.Row.DataItem is DataRowView && (e.Row.DataItem as DataRowView).DataView.Table != null)
                    {
                        DataTable dt = (e.Row.DataItem as DataRowView).DataView.ToTable(); 
                        int UserID = Convert.ToInt32(dt.Rows[e.Row.RowIndex]["UserID"]);
                      
                        if (UserID != currentIdEmp)
                        {
                            if (e.Row.RowIndex > 0)
                            {
                                for (int i = subTotalRowIndexEmp; i < e.Row.RowIndex; i++)
                                {
                                    DateTime StartTime = Convert.ToDateTime(gdrEmployeeTimeTracker.Rows[i].Cells[9].Text);
                                    string eTime = (gdrEmployeeTimeTracker.Rows[i].Cells[10].Text).ToString();
                                    if (eTime != null && eTime != "")
                                    {
                                        DateTime EndTime = Convert.ToDateTime(gdrEmployeeTimeTracker.Rows[i].Cells[10].Text);
                                        if (EndTime.Year != 2000)
                                        {
                                            TimeSpan span = EndTime.Subtract(StartTime);
                                            decimal totalHours = (span.Days * 24 * 60 + span.Hours * 60 + span.Minutes);
                                            subTotalEmp += totalHours;

                                        }
                                    }

                                }
                                if (subTotalEmp >= 60)
                                {
                                    hourE = (int)subTotalEmp / 60;
                                    subTotalEmp = subTotalEmp % 60;
                                }
                                else
                                {
                                    hourE = 0;
                                    subTotalEmp = subTotalEmp % 60;
                                }
                                if (subTotalEmp < 10)
                                    TotalME = hourE.ToString() + ":0" + Math.Round(subTotalEmp, 2).ToString();
                                else
                                    TotalME = hourE.ToString() + ":" + Math.Round(subTotalEmp, 2).ToString();

                                if (gdrEmployeeTimeTracker.Rows.Count > 0)
                                    this.AddTotalRowEmp("Total", TotalME);

                                subTotalRowIndexEmp = e.Row.RowIndex;
                            }
                            currentIdEmp = UserID;
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void AddTotalRowEmp(string labelText, string value)
    {
        GridViewRow row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
        row.BackColor = ColorTranslator.FromHtml("#F9F9F9");

        row.Cells.AddRange(new TableCell[9] { new TableCell (), new TableCell (),new TableCell (), new TableCell (),new TableCell (),new TableCell (),//Empty Cell
                                        new TableCell { Text = labelText, HorizontalAlign = HorizontalAlign.Right,CssClass="cellColor"},
                                        new TableCell { Text = value, HorizontalAlign = HorizontalAlign.Center,CssClass="cellColor" },new TableCell () });

        gdrEmployeeTimeTracker.Controls[0].Controls.Add(row);
    }
    protected void gdrEmployeeTimeTracker_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {
                DataClassesDataContext _db = new DataClassesDataContext();

                int nGPSId = Convert.ToInt32(gdrEmployeeTimeTracker.DataKeys[e.Row.RowIndex].Values[0].ToString());
                string SectionName = gdrEmployeeTimeTracker.DataKeys[e.Row.RowIndex].Values[1].ToString();
                string EndPlace = gdrEmployeeTimeTracker.DataKeys[e.Row.RowIndex].Values[2].ToString();
                int nUserId = Convert.ToInt32(gdrEmployeeTimeTracker.DataKeys[e.Row.RowIndex].Values[3].ToString());
                string StartPlace = gdrEmployeeTimeTracker.DataKeys[e.Row.RowIndex].Values[4].ToString();
                DateTime StartTime = Convert.ToDateTime(gdrEmployeeTimeTracker.DataKeys[e.Row.RowIndex].Values[5]);
                DateTime EndTime = Convert.ToDateTime(gdrEmployeeTimeTracker.DataKeys[e.Row.RowIndex].Values[6]);

                Label lblTotalHours = (Label)e.Row.FindControl("lblTotalHours");
                Label lblLunch = (Label)e.Row.FindControl("lblLunch");
                lblLunch.Text = ".50";

                if (Convert.ToDateTime(EndTime).Year != 2000)
                {
                   
                    TimeSpan span = EndTime.Subtract(StartTime);
                    int totalHours = (span.Days * 24 * 60 + span.Hours * 60 + span.Minutes);
                    if (span.Days > 0)
                        lblTotalHours.Text = span.Days + ":" + span.Hours + ":" + span.Minutes;
                    else
                        lblTotalHours.Text = span.ToString(@"hh\:mm");

                 
                }
                else
                {
                    e.Row.Cells[5].Text = "";
                    lblTotalHours.Text = "";
                }

                if (StartPlace.Contains("USA") || StartPlace.Contains("usa"))
                {
                    e.Row.Cells[2].Text = StartPlace.Remove(StartPlace.Length - 5, 5);
                }
                else
                {
                    if (StartPlace == "0" || StartPlace == "" || StartPlace == null)
                    {
                        e.Row.Cells[2].Text = "";
                    }
                    else
                    {
                        e.Row.Cells[2].Text = StartPlace;
                    }

                }

                if (EndPlace == "0")
                {
                    e.Row.Cells[3].Text = "";
                }
                else
                {
                    if (EndPlace.Contains("USA") || EndPlace.Contains("usa"))
                    {
                        e.Row.Cells[3].Text = EndPlace.Remove(EndPlace.Length - 5, 5);
                    }
                    else
                    {
                        e.Row.Cells[3].Text = EndPlace;
                    }
                }
                user_info objUser = new user_info();
                objUser = _db.user_infos.SingleOrDefault(c => c.user_id == nUserId && c.IsTimeClock==true);
                if (objUser != null)
                    e.Row.Cells[1].Text = objUser.first_name+" "+objUser.last_name;
                else
                    e.Row.Cells[1].Text = "";
                ImageButton imgDelete = (ImageButton)e.Row.FindControl("imgDelete");
                imgDelete.OnClientClick = "return confirm('This will permanently delete this time entry.');";
                imgDelete.CommandArgument = nGPSId.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    protected void chkEmployeeTotalHours_CheckedChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, chkEmployeeTotalHours.ID, chkEmployeeTotalHours.GetType().Name, "CheckedChanged"); 
        if (chkEmployeeTotalHours.Checked == true)
        {
            chkTotalhours.Checked = false;
            BindEmployeeSubTotalHours();
        }
        else
        {
            chkEmployeeTotalHours.Checked = false;
            BindEmployeeLaborHourTracking();
        }
    }
    protected void ddEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, ddEmployee.ID, ddEmployee.GetType().Name, "SelectedIndexChanged"); 
       // chkEmployeeTotalHours.Checked = false;
        lblResultEmp.Text = "";
        lblResult.Text = "";

        if (chkEmployeeTotalHours.Checked == true)
        {
           
            chkTotalhours.Checked = false;
            BindEmployeeSubTotalHours();
        }
        else
        {
            chkEmployeeTotalHours.Checked = false;
            BindEmployeeLaborHourTracking();
        }
     
    }
    protected void lnkViewAllEmp_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, LinkButton1.ID, LinkButton1.GetType().Name, "Click"); 
        lblResult.Text = "";
        ddEmployee.SelectedIndex = -1;
        txtEmployeeStartDate.Text = "";
        lblResultEmp.Text = "";
        txtEmployeeEndDate.Text = "";
        chkEmployeeTotalHours.Checked = false;

        if (Session["CPUFilterEmp"] != null)
        {
            Session.Remove("CPUFilterEmp");

        }
        ddlItemPerPageEmployee.SelectedIndex = 0;
        BindEmployeeLaborHourTracking();

    }

    protected void gdrEmployeeTimeTracker_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, gdrEmployeeTimeTracker.ID, gdrEmployeeTimeTracker.GetType().Name, "PageIndexChanging"); 
        GetEmployeeLaberTracking(e.NewPageIndex);
    }

    protected void btnNextEmp_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnNextEmp.ID, btnNextEmp.GetType().Name, "Click"); 
        int nCurrentPage = 0;
        nCurrentPage = Convert.ToInt32(lblCurrentPageNoEmp.Text);
        GetEmployeeLaberTracking(nCurrentPage);
    }
    protected void btnPreviousEmp_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnPreviousEmp.ID, btnPreviousEmp.GetType().Name, "Click"); 
        int nCurrentPage = 0;
        nCurrentPage = Convert.ToInt32(lblCurrentPageNoEmp.Text);
        GetEmployeeLaberTracking(nCurrentPage - 2);
    }
    protected void ddlItemPerPageEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, ddlItemPerPageEmployee.ID, ddlItemPerPageEmployee.GetType().Name, "SelectedIndexChanged"); 
        chkEmployeeTotalHours.Checked = false;
        GetEmployeeLaberTracking(0);
    }

    protected void btnViewEmp_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnViewEmp.ID, btnViewEmp.GetType().Name, "Click"); 
        string strRequired = string.Empty;
        lblResult.Text = "";
        lblResultEmp.Text = "";
        try
        {
            Convert.ToDateTime(txtEmployeeStartDate.Text.Trim());
        }
        catch
        {
            strRequired += "Start Date is required.<br/>";

        }

        try
        {
            Convert.ToDateTime(txtEmployeeEndDate.Text.Trim());
            if (Convert.ToDateTime(txtEmployeeStartDate.Text.Trim()) >= Convert.ToDateTime(txtEmployeeEndDate.Text.Trim()))
            {
                strRequired += "End Date must be greater than Start Date.<br/>";

            }
        }
        catch
        {
            strRequired += "End Date is required.<br/>";

        }
        if (strRequired.Length > 0)
        {
            lblResultEmp.Text = csCommonUtility.GetSystemRequiredMessage(strRequired);
            return;
        }

        if (chkEmployeeTotalHours.Checked)
        {
            BindEmployeeSubTotalHours();
        }
        else
        {
            BindEmployeeLaborHourTracking();
        }

    }

    protected void btnAddNewEmp_Click(object sender, EventArgs e)
    {
        Response.Redirect("laborhourdetails.aspx?isCrew=0");
    }

    protected void btnExpCustListEmp_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, ImabtnEmployee.ID, ImabtnEmployee.GetType().Name, "Click"); 
        DataTable tmpTable = LoadTmpDataTableEmp();
        DataClassesDataContext _db = new DataClassesDataContext();

        string strRequired = string.Empty;
        lblResult.Text = "";
        lblResultEmp.Text = "";
        string strCondition = string.Empty;

        if (ddEmployee.SelectedItem.Text != "All")
        {
            strCondition = " where IsCrew=0 AND UserID = " + Convert.ToInt32(ddEmployee.SelectedValue);
        }
        else
        {
            strCondition = " where IsCrew=0 ";
        }
        if (txtEmployeeStartDate.Text.Trim() != "" && txtEmployeeEndDate.Text.Trim() != "")
        {

            try
            {
                Convert.ToDateTime(txtEmployeeStartDate.Text.Trim());
            }
            catch
            {
                strRequired += "Start Date is required.<br/>";

            }

            try
            {
                Convert.ToDateTime(txtEmployeeEndDate.Text.Trim());
                if (Convert.ToDateTime(txtEmployeeStartDate.Text.Trim()) >= Convert.ToDateTime(txtEmployeeEndDate.Text.Trim()))
                {
                    strRequired += "End Date must be greater than Start Date.<br/>";

                }
            }
            catch
            {
                strRequired += "End Date is required.<br/>";

            }
            if (strRequired.Length > 0)
            {
                lblResultEmp.Text = csCommonUtility.GetSystemRequiredMessage(strRequired);
                return;
            }
            DateTime dt1 = Convert.ToDateTime(txtEmployeeStartDate.Text.Trim());
            DateTime dt2 = Convert.ToDateTime(txtEmployeeEndDate.Text.Trim());

            if (strCondition.Length == 0)
            {

                strCondition = " WHERE IsCrew=0 AND labor_date >= '" + dt1 + "' and labor_date <'" + dt2.AddDays(1) + "'";
            }
            else
            {
                strCondition += " AND labor_date >= '" + dt1 + "' and labor_date <'" + dt2.AddDays(1) + "'";
            }
        }
    string strQ = "select [GPSTrackID],[StartPlace] ,[MakeStopPlace],[EndPlace],[Distance],[Time],[CreatedDate],[UserID],[IsCrew],[CustomerName],[section_id], " +
                     " [SectionName],[StartTime] ,[EndTime],[customer_id],[Estimate_id],[labor_date] from GPSTracking  " + strCondition + " order by labor_date desc ";
        DataTable dtReport = csCommonUtility.GetDataTable(strQ);
        if (dtReport.Rows.Count > 0)
        {
            foreach (DataRow dr in dtReport.Rows)
            {
                string StartPlace = "";
                string EndPlace = "";
                DataRow drNew = tmpTable.NewRow();
                StartPlace = dr["StartPlace"].ToString();
                EndPlace = dr["EndPlace"].ToString();
                user_info objUser = new user_info();
                objUser = _db.user_infos.SingleOrDefault(c => c.user_id == Convert.ToInt32(dr["UserID"]) && c.IsTimeClock==true);
                drNew["Labor Date"] = Convert.ToDateTime(dr["labor_date"]).ToString("MM-dd-yyyy");
                if (objUser != null)
                    drNew["Employee Name"] = objUser.first_name + " " + objUser.last_name;
                drNew["Customer Name"] = dr["CustomerName"].ToString();
                if (dr["SectionName"].ToString() != "Select")
                    drNew["Section"] = dr["SectionName"].ToString();
                if (StartPlace.Contains("USA") || StartPlace.Contains("usa"))
                {
                    drNew["Start Location"] = StartPlace.Remove(StartPlace.Length - 5, 5);
                }
                else
                {
                    if (StartPlace == "0" || StartPlace == "" || StartPlace == null)
                    {
                        drNew["Start Location"] = "";
                    }
                    else
                    {
                        drNew["Start Location"] = StartPlace;
                    }
                }
                if (EndPlace == "0")
                {
                    drNew["End Location"] = "";
                }
                else
                {
                    if (EndPlace.Contains("USA") || EndPlace.Contains("usa"))
                    {
                        drNew["End Location"] = EndPlace.Remove(EndPlace.Length - 5, 5);
                    }
                    else
                    {
                        drNew["End Location"] = EndPlace;
                    }
                }

                drNew["Start Time"] = Convert.ToDateTime(dr["StartTime"]).ToShortTimeString();
                if (Convert.ToDateTime(dr["EndTime"]).Year != 2000)
                    drNew["End Time"] = Convert.ToDateTime(dr["EndTime"]).ToShortTimeString();

                drNew["Lunch Start Time"] = "12 PM";
                drNew["Lunch End Time"] = "12:30 PM";

                drNew["Lunch End Time"] = "12:30 PM";
                drNew["Lunch (Hrs)"] = ".50";

                if (Convert.ToDateTime(dr["EndTime"]).Year != 2000)
                {
                    DateTime StartTime = Convert.ToDateTime(dr["StartTime"].ToString());
                    DateTime EndTime = Convert.ToDateTime(dr["EndTime"].ToString());
                 
                    TimeSpan span = EndTime.Subtract(StartTime);

                    int totalHours = (span.Days * 24 * 60 + span.Hours * 60 + span.Minutes);
                    if (span.Days > 0)
                        drNew["Total (Hr:Min)"] = span.Days + ":" + span.Hours + ":" + span.Minutes;
                    else
                        drNew["Total (Hr:Min)"] = span.ToString(@"hh\:mm");//span.Hours + ":" + span.Minutes;
                    
                  }
                else
                {

                    drNew["Total (Hr:Min)"] = "";
                }

                tmpTable.Rows.Add(drNew);
            }

            Response.Clear();
            Response.ClearHeaders();

            using (CsvWriter writer = new CsvWriter(Response.OutputStream, ',', System.Text.Encoding.Default))
            {
                writer.WriteAll(tmpTable, true);
            }
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment; filename=LaborTrackingList.csv");
            Response.End();
        }
        else
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage("No records found.");
        }
    }
    private DataTable LoadTmpDataTableEmp()
    {
        DataTable table = new DataTable();
        table.Columns.Add("Labor Date", typeof(string));
        table.Columns.Add("Employee Name", typeof(string));
        table.Columns.Add("Customer Name", typeof(string));
        table.Columns.Add("Section", typeof(string));
        table.Columns.Add("Start Location", typeof(string));
        table.Columns.Add("End Location", typeof(string));
        table.Columns.Add("Start Time", typeof(string));
        table.Columns.Add("End Time", typeof(string));
        table.Columns.Add("Lunch Start Time", typeof(string));
        table.Columns.Add("Lunch End Time", typeof(string));
        table.Columns.Add("Lunch (Hrs)", typeof(string));
        table.Columns.Add("Total (Hr:Min)", typeof(string));

        return table;
    }

    protected void DeleteFileEmp(object sender, EventArgs e)
    {
        try
        {
            userinfo obj = (userinfo)Session["oUser"];
            ImageButton imgDelete = (ImageButton)sender;
            int nGPSId = Convert.ToInt32(imgDelete.CommandArgument);
            DataClassesDataContext _db = new DataClassesDataContext();
            string strQ = "";
            lblResult.Text = "";
            lblResultEmp.Text = "";
            strQ = "insert into DeleteGPSTracking " +
                          " SELECT [GPSTrackID] ,[StartPlace] ,[StartLatitude],[StartLogitude] ,[EndLatitude] ,[EndLogitude],[MakeStopPlace], " +
                          " [EndPlace],[Distance],[Time],[CreatedDate] ,[UserID] ,[CustomerName] ,[section_id],[SectionName] ,[StartTime], " +
                          " [EndTime],[customer_id],[Estimate_id],[labor_date],[deviceName],[IsCrew], " + "'" + obj.username + "'" + ",getdate() " +
                          " FROM GPSTracking " +
                          " WHERE GPSTrackID ='" + nGPSId + "'";
            _db.ExecuteCommand(strQ, string.Empty);

            strQ = "delete from GPSTracking where GPSTrackID=" + nGPSId;
            _db.ExecuteCommand(strQ, string.Empty);


            var nList = (from dgps in _db.GPSTrackingDetails where dgps.GPSTrackID == nGPSId select dgps).ToList();

            DeleteGPSTrackingDetail objDGPS = null;

            foreach (var li in nList)
            {
                objDGPS = new DeleteGPSTrackingDetail();
                objDGPS.ID = li.ID;
                objDGPS.GPSTrackID = li.GPSTrackID;
                objDGPS.Latitude = li.Latitude;
                objDGPS.Longitude = li.Longitude;
                objDGPS.InputType = li.InputType;
                objDGPS.CreateDate = li.CreateDate;

                _db.DeleteGPSTrackingDetails.InsertOnSubmit(objDGPS);
                _db.SubmitChanges();
            }

             strQ = "delete from GPSTrackingDetails where GPSTrackID=" + nGPSId;
            _db.ExecuteCommand(strQ, string.Empty);
            lblResultEmp.Text = csCommonUtility.GetSystemMessage("Labor tracking has been deleted successfully.");
        
            BindEmployeeLaborHourTracking();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}