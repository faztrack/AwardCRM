using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class mtimeclock : System.Web.UI.Page
{
    public DataTable dtSection;
    [WebMethod]
    public static string[] GetLastName(String prefixText, Int32 count)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        return (from c in _db.customers
                where c.last_name1.StartsWith(prefixText)
                join ce in _db.customer_estimates on c.customer_id equals ce.customer_id
                where ce.status_id == 3
                select c.first_name1 + " " + c.last_name1 + " (" + ce.job_number + ")").Take<String>(count).ToArray();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);
            DataClassesDataContext _db = new DataClassesDataContext();
        
            if (Session["oUser"] == null && Session["oCrew"] == null)
            {
                Response.Redirect("mobile.aspx");
            }
            else
            {
                btnStartTrip.Attributes.Add("class", "btn btn-success");
                btnEndTrip.Attributes.Add("class", "btn btn-danger endbuttonwidth125");
                CheckEndTime();
            }

            lblDateTime.Text = DateTime.Now.ToShortDateString();
            BindLaborHourTracking();
        }


    }

    private void CheckEndTime()
    {
        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();

            // Crew 
            if (Session["oCrew"] != null)
            {
                    
                     Crew_Detail objC = (Crew_Detail)Session["oCrew"];
                     GPSTracking objCrew = _db.GPSTrackings.SingleOrDefault(gps => gps.UserID == objC.crew_id && gps.IsCrew == true && (Convert.ToDateTime(gps.EndTime).Year == 2000));
                    if (objCrew != null)
                    {
                        if (Convert.ToDateTime(objCrew.EndTime).Year == 2000)
                        {
                            btnStartTrip.Visible = false;  // Start Button
                            pnlEndButton.Visible = true;
                            lblStartTime.Text = Convert.ToDateTime(objCrew.StartTime).ToShortTimeString();
                            lblSearchCustomerName.Text = objCrew.CustomerName;
                            txtSearch.Text = objCrew.CustomerName;
                            if (objCrew.customer_id != 0 && objCrew.Estimate_id != 0)
                            {
                                hdnCustomerId.Value = objCrew.customer_id.ToString();
                                hdnEstimateId.Value = objCrew.Estimate_id.ToString();
                                LoadSection(Convert.ToInt32(objCrew.customer_id), Convert.ToInt32(objCrew.Estimate_id));
                                ddlSection.SelectedValue = objCrew.section_id.ToString();
                            }
                            else
                            {
                                hdnCustomerId.Value = objCrew.customer_id.ToString();
                                hdnEstimateId.Value = objCrew.Estimate_id.ToString();
                                LoadSection(0, 0);
                            }
                        }
                        else
                        {
                            LoadSection(0, 0);
                        }

                    }
                    else
                    {
                        LoadSection(0, 0);
                    }
                
            }

            // User 
            if (Session["oUser"] != null)
            {
           
              
                userinfo objUser = (userinfo)Session["oUser"];
                GPSTracking objCrew = _db.GPSTrackings.SingleOrDefault(gps => gps.UserID == objUser.user_id && gps.IsCrew == false && (Convert.ToDateTime(gps.EndTime).Year == 2000));
                    if (objCrew != null)
                    {
                        if (Convert.ToDateTime(objCrew.EndTime).Year == 2000)
                        {
                            btnStartTrip.Visible = false;  // Start Button
                            pnlEndButton.Visible = true;
                            lblStartTime.Text = Convert.ToDateTime(objCrew.StartTime).ToShortTimeString();
                            lblSearchCustomerName.Text = objCrew.CustomerName;
                            txtSearch.Text = objCrew.CustomerName;
                            if (objCrew.customer_id != 0 && objCrew.Estimate_id != 0)
                            {
                                hdnCustomerId.Value = objCrew.customer_id.ToString();
                                hdnEstimateId.Value = objCrew.Estimate_id.ToString();
                                LoadSection(Convert.ToInt32(objCrew.customer_id), Convert.ToInt32(objCrew.Estimate_id));
                                ddlSection.SelectedValue = objCrew.section_id.ToString();
                            }
                            else
                            {
                                hdnCustomerId.Value = objCrew.customer_id.ToString();
                                hdnEstimateId.Value = objCrew.Estimate_id.ToString();
                                LoadSection(0, 0);
                            }
                        }
                        else
                        {
                            LoadSection(0, 0);
                        }

                    }
                    else
                    {
                        LoadSection(0, 0);
                    }
                }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnStartTrip_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnStartTrip.ID, btnStartTrip.GetType().Name, "Click"); 
        lblResult.Text = "";
        lblEndTime.Text = "";
        DataClassesDataContext _db = new DataClassesDataContext();
        if (btnStartTrip.Text == "START CLOCK")
        {
            if (Session["OCrew"] != null)
            {
                if (Session["OCrew"] == null)
                {
                    btnStartTrip.Text = "START CLOCK";
                    Response.Redirect("mobile.aspx");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "getCurrentLocation", "getCurrentLocation('" + 1 + "');", true);
                    Crew_Detail objC = (Crew_Detail)Session["oCrew"];
                   
                    btnEndTrip.Attributes.Add("class", "btn btn-danger endbuttonwidth135");
                    lblStartTime.Text = DateTime.Now.ToShortTimeString();
                    pnlEndButton.Visible = true;
                    btnEndTrip.Visible = true;
                    btnStartTrip.Visible = false;  // Start Button
                
                    GPSTracking objCrew = new GPSTracking();
                    objCrew.deviceName = hdnDeviceName.Value;
                    objCrew.StartLatitude = hdnLatitude.Value;
                    objCrew.StartLogitude = hdnLongitude.Value;
                    objCrew.CreatedDate = DateTime.Now;
                    objCrew.labor_date = DateTime.Now;
                    objCrew.UserID = objC.crew_id;
                    objCrew.IsCrew = true;
                    objCrew.Distance = "";// hdnDistance.Value;
                    objCrew.CustomerName = lblSearchCustomerName.Text;
                    objCrew.SectionName = ddlSection.SelectedItem.Text;
                    if (ddlSection.SelectedItem.Text == "Select")
                    {
                        objCrew.section_id = 0;
                    }
                    else
                    {
                        objCrew.section_id = Convert.ToInt32(ddlSection.SelectedValue);
                    }
                    objCrew.customer_id = Convert.ToInt32(hdnCustomerId.Value);
                    objCrew.Estimate_id = Convert.ToInt32(hdnEstimateId.Value);
                    objCrew.StartTime = DateTime.Now; //lblStartTime.Text;
                    lblStartTime.Text = DateTime.Now.ToShortTimeString();
                    objCrew.EndTime = Convert.ToDateTime("01/01/2000");
                    objCrew.StartPlace = hdnStartLocation.Value;
                    objCrew.EndPlace = "0";
                    objCrew.Time = "0:0:0:0";
                    _db.GPSTrackings.InsertOnSubmit(objCrew);
                    _db.SubmitChanges();
                    GPSTrackingDetail objGPSTrackingDetail = new GPSTrackingDetail();
                    objGPSTrackingDetail.Latitude = hdnLatitude.Value;
                    objGPSTrackingDetail.Longitude = hdnLongitude.Value;
                    objGPSTrackingDetail.InputType = 0;
                    objGPSTrackingDetail.CreateDate = DateTime.Now;
                    objGPSTrackingDetail.GPSTrackID = objCrew.GPSTrackID;
                    _db.GPSTrackingDetails.InsertOnSubmit(objGPSTrackingDetail);
                    _db.SubmitChanges();
                }
           }

            // User 
            if (Session["oUser"] != null)
            {
                if (Session["oUser"] == null)
                {
                    btnStartTrip.Text = "START CLOCK";
                    Response.Redirect("mobile.aspx");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "getCurrentLocation", "getCurrentLocation('" + 1 + "');", true);
                    userinfo objUser = (userinfo)Session["oUser"];
                    btnEndTrip.Attributes.Add("class", "btn btn-danger endbuttonwidth135");
                    lblStartTime.Text = DateTime.Now.ToShortTimeString();
                    pnlEndButton.Visible = true;
                    btnEndTrip.Visible = true;
                    btnStartTrip.Visible = false;  // Start Button
                
                    GPSTracking objCrew = new GPSTracking();
                    objCrew.deviceName = hdnDeviceName.Value;
                    objCrew.StartLatitude = hdnLatitude.Value;
                    objCrew.StartLogitude = hdnLongitude.Value;
                    objCrew.CreatedDate = DateTime.Now;
                    objCrew.labor_date = DateTime.Now;
                    objCrew.UserID = objUser.user_id;
                    objCrew.IsCrew = false;
                    objCrew.Distance = "";// hdnDistance.Value;
                    objCrew.CustomerName = lblSearchCustomerName.Text;
                    objCrew.SectionName = ddlSection.SelectedItem.Text;
                    if (ddlSection.SelectedItem.Text == "Select")
                    {
                        objCrew.section_id = 0;
                    }
                    else
                    {
                        objCrew.section_id = Convert.ToInt32(ddlSection.SelectedValue);
                    }
                    objCrew.customer_id = Convert.ToInt32(hdnCustomerId.Value);
                    objCrew.Estimate_id = Convert.ToInt32(hdnEstimateId.Value);
                    objCrew.StartTime = DateTime.Now; //lblStartTime.Text;
                    lblStartTime.Text = DateTime.Now.ToShortTimeString();
                    objCrew.EndTime = Convert.ToDateTime("01/01/2000");
                    objCrew.StartPlace = hdnStartLocation.Value;
                    objCrew.EndPlace = "0";
                    objCrew.Time = "0:0:0:0";
                    _db.GPSTrackings.InsertOnSubmit(objCrew);
                    _db.SubmitChanges();
                    GPSTrackingDetail objGPSTrackingDetail = new GPSTrackingDetail();
                    objGPSTrackingDetail.Latitude = hdnLatitude.Value;
                    objGPSTrackingDetail.Longitude = hdnLongitude.Value;
                    objGPSTrackingDetail.InputType = 0;
                    objGPSTrackingDetail.CreateDate = DateTime.Now;
                    objGPSTrackingDetail.GPSTrackID = objCrew.GPSTrackID;
                    _db.GPSTrackingDetails.InsertOnSubmit(objGPSTrackingDetail);
                    _db.SubmitChanges();
                }
            }
        }

    }
    protected void btnEndTrip_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnEndTrip.ID, btnEndTrip.GetType().Name, "Click"); 
        btnStartTrip.Attributes.Add("class", "btn btn-success");
        btnEndTrip.Attributes.Add("class", "btn btn-danger endbuttonwidth125");
        btnStartTrip.Text = "START CLOCK";
        btnStartTrip.Visible = false;
        lblResult.Text = "";
        DataClassesDataContext _db = new DataClassesDataContext();
        if (Session["oCrew"] != null)
        {
            if (Session["oCrew"] == null)
            {
                Response.Redirect("mobile.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "getCurrentLocation", "getCurrentLocation('" + 3 + "');", true);  
                Crew_Detail objC = (Crew_Detail)Session["oCrew"];

                GPSTracking objCrew = _db.GPSTrackings.SingleOrDefault(gps => gps.UserID == objC.crew_id && gps.IsCrew == true && (Convert.ToDateTime(gps.EndTime).Year == 2000));
                if (objCrew != null)
                {
                    objCrew.EndTime = DateTime.Now;
                    lblEndTime.Text = DateTime.Now.ToShortTimeString();
                    objCrew.EndPlace = hdnEndLcation.Value;
                    if (hdnDeviceName.Value == "0")
                        objCrew.deviceName = "Others";
                    else
                        objCrew.deviceName = hdnDeviceName.Value;
                    objCrew.EndLatitude = hdnLatitude.Value;
                    objCrew.EndLogitude = hdnLongitude.Value;
                    objCrew.Distance = "";// hdnDistance.Value;
                    _db.SubmitChanges();
                    btnStartTrip.Text = "START CLOCK";
                    btnStartTrip.Visible = true;
                    btnEndTrip.Visible = false;
                }

            }
        }

        // User 
        if (Session["oUser"] != null)
        {

            if (Session["oUser"] == null)
            {
                Response.Redirect("mobile.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "getCurrentLocation", "getCurrentLocation('" + 3 + "');", true);  
                userinfo objUser = (userinfo)Session["oUser"];
                GPSTracking objCrew = _db.GPSTrackings.SingleOrDefault(gps => gps.UserID == objUser.user_id && gps.IsCrew == false && (Convert.ToDateTime(gps.EndTime).Year == 2000));
                if (objCrew != null)
                {
                     objCrew.EndTime = DateTime.Now;
                     lblEndTime.Text = DateTime.Now.ToShortTimeString();
                     objCrew.EndPlace = hdnEndLcation.Value;
                     if (hdnDeviceName.Value == "0")
                         objCrew.deviceName = "Others";
                     else
                         objCrew.deviceName = hdnDeviceName.Value;
                     objCrew.EndLatitude = hdnLatitude.Value;
                     objCrew.EndLogitude = hdnLongitude.Value;

                     objCrew.Distance = "";// hdnDistance.Value;
                    _db.SubmitChanges();
                     btnStartTrip.Text = "START CLOCK";
                     btnStartTrip.Visible = true;
                     btnEndTrip.Visible = false;
                   
                }
            }
        }
    }

    protected void imgBack_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("mlandingpage.aspx");
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSearch.ID, btnSearch.GetType().Name, "Click"); 
        DataClassesDataContext _db = new DataClassesDataContext();
        string strQ = string.Empty;

        if (txtSearch.Text.Trim() != "")
        {

            string fullName = txtSearch.Text;
            string job = string.Empty;
            var JNum = fullName.Split('(');
            if (fullName.IndexOf("(") != -1)
            {
                job = JNum[1];

            }
            job = job.Replace(")", "");

            int customer_id = 0;
            int Estimate_id = 0;


            if (_db.customer_estimates.Where(ce => ce.job_number == job.Trim() && ce.client_id == 1 && ce.status_id == 3).ToList().Count > 0)
            {
                customer_estimate obj = _db.customer_estimates.FirstOrDefault(ce => ce.job_number == job.Trim() && ce.client_id == 1 && ce.status_id == 3);

                customer_id = Convert.ToInt32(obj.customer_id);
                Estimate_id = Convert.ToInt32(obj.estimate_id);
                hdnEstimateId.Value = Estimate_id.ToString();
                hdnCustomerId.Value = customer_id.ToString();
                // customer objCust = _db.customers.SingleOrDefault(c => c.customer_id == customer_id);
                lblSearchCustomerName.Text = fullName;//objCust.first_name1 + " " + objCust.last_name1;
            }


            LoadSection(customer_id, Convert.ToInt32(hdnEstimateId.Value));
        }
        else
        {
            hdnEstimateId.Value = "0";
            hdnCustomerId.Value = "0";
        }
    }

    private void LoadSection(int custID, int estID)
    {

        try
        {

            DataTable tmpSTable = LoadSectionTable();
            DataRow dr = tmpSTable.NewRow();
            dr["section_id"] = -1;
            dr["section_name"] = "Select";
            tmpSTable.Rows.Add(dr);

            DataClassesDataContext _db = new DataClassesDataContext();


            var item = (from it in _db.customer_sections
                        join si in _db.sectioninfos on it.section_id equals si.section_id
                        where it.customer_id == custID && it.estimate_id == estID && it.client_id == 1
                        orderby si.section_name ascending
                        select new SectionInfo()
                        {
                            section_id = (int)it.section_id,
                            section_name = si.section_name

                        }).ToList();

            foreach (SectionInfo sinfo in item)
            {

                DataRow drNew = tmpSTable.NewRow();
                drNew["section_id"] = sinfo.section_id;
                drNew["section_name"] = sinfo.section_name;
                tmpSTable.Rows.Add(drNew);
            }


            dr = tmpSTable.NewRow();
            dr["section_id"] = 1;
            dr["section_name"] = "Travel";
            tmpSTable.Rows.Add(dr);
            dr = tmpSTable.NewRow();
            dr["section_id"] = 2;
            dr["section_name"] = "Meeting";
            tmpSTable.Rows.Add(dr);

            dr = tmpSTable.NewRow();
            dr["section_id"] = 3;
            dr["section_name"] = "Other";
            tmpSTable.Rows.Add(dr);
            ddlSection.DataSource = tmpSTable;
            ddlSection.DataTextField = "section_name";
            ddlSection.DataValueField = "section_id";
            ddlSection.DataBind();
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    private DataTable LoadSectionTable()
    {
        DataTable table = new DataTable();
        table.Columns.Add("section_id", typeof(int));
        table.Columns.Add("section_name", typeof(string));
        return table;
    }


    private void BindLaborHourTracking()
    {
        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();
            string strCondition = string.Empty;
            if (Session["oCrew"] != null)
            {
                Crew_Detail objC = (Crew_Detail)Session["oCrew"];
                strCondition = " where IsCrew=1 AND UserID = " + objC.crew_id;
            }

            if (Session["oUser"] != null)
            {
                userinfo objUser = (userinfo)Session["oUser"];
                strCondition = " where  IsCrew=0 AND UserID = " + objUser.user_id;
            }


            if (txtStartDate.Text.Trim() != "" && txtEndDate.Text.Trim() != "")
            {

                DateTime dt1 = Convert.ToDateTime(txtStartDate.Text.Trim());
                DateTime dt2 = Convert.ToDateTime(txtEndDate.Text.Trim());

                if (strCondition.Length == 0)
                {

                    strCondition = " WHERE labor_date >= '" + dt1 + "' and labor_date <'" + dt2.AddDays(1) + "'";
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

            GetLaberTracking(0);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void GetLaberTracking(int nPageNo)
    {

        if (Session["nLoaborHour"] != null)
        {
            DataTable dtLaborHour = (DataTable)Session["nLoaborHour"];
            grdLaberTrack.DataSource = dtLaborHour;
            grdLaberTrack.PageIndex = nPageNo;
            grdLaberTrack.PageSize = Convert.ToInt32(ddlItemPerPage.SelectedValue);
            grdLaberTrack.DataKeyNames = new string[] { "GPSTrackID", "SectionName", "EndPlace", "UserID", "StartPlace", "StartTime", "EndTime" };
            grdLaberTrack.DataBind();
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
    float GrandHours = 0;
    int hour = 0;
    string TotalM = "";
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

                if (Convert.ToDateTime(EndTime).Year != 2000)
                {
                    TimeSpan span = EndTime.Subtract(StartTime);
                    float totalHours = (span.Days * 24 * 60 + span.Hours * 60 + span.Minutes);
                   // totalHours = totalHours / 60;
                    GrandHours += totalHours;
                    if (span.Days > 0)
                        lblTotalHours.Text = span.Days + ":" + span.Hours + ":" + span.Minutes;
                    else
                        lblTotalHours.Text = span.ToString(@"hh\:mm");//span.Hours + ":" + span.Minutes;
                }
                else
                {
                    lblTotalHours.Text = "";
                    e.Row.Cells[4].Text = "";
                }
                if (StartPlace.Contains("USA") || StartPlace.Contains("usa"))
                {
                    e.Row.Cells[1].Text = StartPlace.Remove(StartPlace.Length - 5, 5);
                }
                else
                {
                    if (StartPlace == "0" || StartPlace == "" || StartPlace == null)
                    {
                        e.Row.Cells[1].Text = "";
                    }
                    else
                    {
                        e.Row.Cells[1].Text = StartPlace;
                    }

                }




                if (EndPlace == "0")
                {
                    e.Row.Cells[2].Text = "";
                }
                else
                {
                    if (EndPlace.Contains("USA") || EndPlace.Contains("usa"))
                    {
                        e.Row.Cells[2].Text = EndPlace.Remove(EndPlace.Length - 5, 5);
                    }
                    else
                    {
                        e.Row.Cells[2].Text = EndPlace;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

             

        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            if (GrandHours >= 60)
            {
                hour = (int)GrandHours / 60;
                GrandHours = GrandHours % 60;
            }
            else
            {
                hour = 0;
                GrandHours = GrandHours % 60;
            }
            if (GrandHours < 10)
                TotalM = hour.ToString() + ":0" + GrandHours;
            else
                TotalM = hour.ToString() + ":" + GrandHours;

            e.Row.Cells[4].Text = "Total";
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[4].CssClass = "cellColor";
            e.Row.Cells[5].Text = TotalM.ToString();
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[5].CssClass = "cellColor";
        }

    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnView.ID, btnView.GetType().Name, "Click"); 
        string strRequired = string.Empty;
        lblMSG.Text = "";
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
            lblMSG.Text = csCommonUtility.GetSystemRequiredMessage(strRequired);
            return;
        }
        BindLaborHourTracking();
    }


    protected void lnkViewAll_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, LinkButton1.ID, LinkButton1.GetType().Name, "Click"); 
        lblMSG.Text = "";
        txtStartDate.Text = "";
        txtEndDate.Text = "";
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
        GetLaberTracking(0);
    }


}