using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class selectionlog : System.Web.UI.Page
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
           
            if (Request.QueryString.Get("cid") != null && Request.QueryString.Get("eid") != null) 
            {
                DataClassesDataContext _db = new DataClassesDataContext();
                customer objCust = new customer();
                string strCustName = "";

                int nCustomerID = Convert.ToInt32(Request.QueryString.Get("cid"));
                int nEstimateID = Convert.ToInt32(Request.QueryString.Get("eid"));

                hdnCustomerID.Value = nCustomerID.ToString();
                hdnEstimateID.Value = nEstimateID.ToString();

                if (_db.customers.Where(c => c.customer_id == nCustomerID).Count() > 0)
                {
                    objCust = _db.customers.SingleOrDefault(c => c.customer_id == nCustomerID);
                    strCustName = objCust.first_name1 + " " + objCust.last_name1;
                }

                lblHeaderTitle.Text = "Selection Log (" + strCustName + ")";

                GetSectionSelectionListData();
                Session.Add("CustomerId", nCustomerID);
            

                string strJobNumber = csCommonUtility.GetCustomerEstimateInfo(nCustomerID, nEstimateID).job_number ?? "";

                if (strJobNumber.Length > 0)
                {
                    lblTitelJobNumber.Text = " ( Job Number: " + strJobNumber + " )";
                }
            }
        }
    }

    private void GetSectionSelectionListData()
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        int nCustId = Convert.ToInt32(hdnCustomerID.Value);
        int nEstId = Convert.ToInt32(hdnEstimateID.Value);
  
        var sslist = _db.HistorySection_Selections.Where(ss => ss.customer_id == nCustId && ss.estimate_id == nEstId).ToList().OrderByDescending(c => c.DeletedDate);
        grdSelection.DataSource = sslist;
        grdSelection.DataKeyNames = new string[] { "SectionSelectionID", "customer_id", "section_id", "estimate_id", "location_id", "isSelected", "customer_signature", "customer_siignatureDate" };
        grdSelection.DataBind();
    }

    protected void grdSelection_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            userinfo objUser = (userinfo)Session["oUser"];

            int nSectionSelectionID = Convert.ToInt32(grdSelection.DataKeys[e.Row.RowIndex].Values[0].ToString());
            int customer_id = Convert.ToInt32(grdSelection.DataKeys[e.Row.RowIndex].Values[1].ToString());
            int section_id = Convert.ToInt32(grdSelection.DataKeys[e.Row.RowIndex].Values[2].ToString());
            int estimate_id = Convert.ToInt32(grdSelection.DataKeys[e.Row.RowIndex].Values[3].ToString());
            int location_id = Convert.ToInt32(grdSelection.DataKeys[e.Row.RowIndex].Values[4].ToString());
            bool isSelected = Convert.ToBoolean(grdSelection.DataKeys[e.Row.RowIndex].Values[5]);
            string strCustomerSignature = grdSelection.DataKeys[e.Row.RowIndex].Values[6].ToString();
            string strSignatureDate = Convert.ToDateTime(grdSelection.DataKeys[e.Row.RowIndex].Values[7]).ToString("MM/dd/yyyy hh:mm tt");
            FileUpload grdfile_upload = (FileUpload)e.Row.FindControl("grdfile_upload");
            Image imgCustomerSign = e.Row.FindControl("imgCustomerSign") as Image;

            Label lblSectiong = (Label)e.Row.FindControl("lblSectiong");
            Label lblLocation = (Label)e.Row.FindControl("lblLocation");
            LinkButton lnkOpen = (LinkButton)e.Row.FindControl("lnkOpen");
            Label lblTitle = (Label)e.Row.FindControl("lblTitle");
            Label lblNotes = (Label)e.Row.FindControl("lblNotes");
            Label lblPrice = (Label)e.Row.FindControl("lblPrice");
            Label lblDate = (Label)e.Row.FindControl("lblDate");
          
            Label lblVDate = (Label)e.Row.FindControl("lblVDate");
     
            string str = lblNotes.Text.Replace("&nbsp;", "");

           
            if (str != "" && str.Length > 60)
            {
               
                lblNotes.Text = str.Substring(0, 60) + "...";
                lblNotes.ToolTip = str;
                lnkOpen.Visible = true;

            }
            else
            {
                
                lblNotes.Text = str;
                lblNotes.ToolTip = str;
                lnkOpen.Visible = false;

            }
            if (nSectionSelectionID != 0)
            {
                GridView gvUp = e.Row.FindControl("grdUploadedFileList") as GridView;
                GetUploadedFileListData(gvUp, nSectionSelectionID);
            }
        }

    }

    private void GetUploadedFileListData(GridView grd, int nSectionSelectionID)
    {
        DataClassesDataContext _db = new DataClassesDataContext();



        var objfui = from fui in _db.Historyfile_upload_infos
                     where fui.vendor_cost_id == nSectionSelectionID && fui.type == 5
                     orderby fui.upload_fileId ascending
                     select fui;
        if (objfui != null)
        {
            grd.DataSource = objfui;
            grd.DataKeyNames = new string[] { "upload_fileId", "vendor_cost_id", "estimate_id", "ImageName" };
            grd.DataBind();
        }
    }

    protected void grdUploadedFileList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridView grdUploadedFileList = (GridView)sender;
            userinfo objUser = (userinfo)Session["oUser"];
            GridViewRow grdSelectionRow = (GridViewRow)(grdUploadedFileList.NamingContainer);
            int Index = grdSelectionRow.RowIndex;

            GridView grdSelection = (GridView)(grdSelectionRow.Parent.Parent);
            bool isSelected = Convert.ToBoolean(grdSelection.DataKeys[Index].Values[5]);



            int nUploadFileId = Convert.ToInt32(grdUploadedFileList.DataKeys[e.Row.RowIndex].Values[0]);
            string strImageName = grdUploadedFileList.DataKeys[e.Row.RowIndex].Values[1].ToString();
            int customer_id = Convert.ToInt32(hdnCustomerID.Value);


            string file_name = grdUploadedFileList.DataKeys[e.Row.RowIndex].Values[3].ToString();


            string fileName = file_name.Substring(0, 10);
            Label lblFileName = (Label)e.Row.FindControl("lblFileName");
            lblFileName.Text = fileName;
            if (file_name.Contains(".jpg") || file_name.Contains(".jpeg") || file_name.Contains(".png") || file_name.Contains(".gif"))
            {
                HyperLink hypImg = (HyperLink)e.Row.FindControl("hypImg");
                hypImg.Visible = true;

                hypImg.NavigateUrl = "File/" + customer_id + "/DELETEDSELECTIONS/" + file_name;
                hypImg.Target = "_blank";

                Image img = (Image)e.Row.FindControl("img");

                HyperLink hypPDF = (HyperLink)e.Row.FindControl("hypPDF");
                hypPDF.Visible = false;
                HyperLink hypExcel = (HyperLink)e.Row.FindControl("hypExcel");
                hypExcel.Visible = false;
                HyperLink hypDoc = (HyperLink)e.Row.FindControl("hypDoc");
                hypDoc.Visible = false;
                HyperLink hypTXT = (HyperLink)e.Row.FindControl("hypTXT");
                hypTXT.Visible = false;

                img.ImageUrl = "File/" + customer_id + "/DELETEDSELECTIONS/Thumbnail/" + file_name;
                //img.Attributes.Add("data-zoom-image", "Document/" + SiteReviewsId + "/" + SiteReview_file_name);
              
            }


            if (file_name.Contains(".pdf"))
            {
                HyperLink hypImg = (HyperLink)e.Row.FindControl("hypImg");
                hypImg.Visible = false;
                HyperLink hypPDF = (HyperLink)e.Row.FindControl("hypPDF");
                hypPDF.Visible = true;

                HyperLink hypExcel = (HyperLink)e.Row.FindControl("hypExcel");
                hypExcel.Visible = false;
                HyperLink hypDoc = (HyperLink)e.Row.FindControl("hypDoc");
                hypDoc.Visible = false;
                HyperLink hypTXT = (HyperLink)e.Row.FindControl("hypTXT");
                hypTXT.Visible = false;

                Image imgPDF = (Image)e.Row.FindControl("imgPDF");
                imgPDF.ImageUrl = "~/images/icon_pdf.png";
                hypPDF.NavigateUrl = "File/" + customer_id + "/DELETEDSELECTIONS/" + file_name;
                hypPDF.Target = "_blank";
            
            }
            if (file_name.Contains(".doc") || file_name.Contains(".docx"))
            {
                HyperLink hypImg = (HyperLink)e.Row.FindControl("hypImg");
                hypImg.Visible = false;

                HyperLink hypPDF = (HyperLink)e.Row.FindControl("hypPDF");
                hypPDF.Visible = false;
                HyperLink hypExcel = (HyperLink)e.Row.FindControl("hypExcel");
                hypExcel.Visible = false;
                HyperLink hypDoc = (HyperLink)e.Row.FindControl("hypDoc");
                hypDoc.Visible = true;
                HyperLink hypTXT = (HyperLink)e.Row.FindControl("hypTXT");
                hypTXT.Visible = false;

                Image imgDoc = (Image)e.Row.FindControl("imgDoc");
                imgDoc.ImageUrl = "~/images/icon_docs.png";
                hypDoc.NavigateUrl = "File/" + customer_id + "/DELETEDSELECTIONS/" + file_name;
                hypDoc.Target = "_blank";
              

            }
            if (file_name.Contains(".xls") || file_name.Contains(".xlsx") || file_name.Contains(".csv"))
            {
                HyperLink hypImg = (HyperLink)e.Row.FindControl("hypImg");
                hypImg.Visible = false;
                HyperLink hypPDF = (HyperLink)e.Row.FindControl("hypPDF");
                hypPDF.Visible = false;
                HyperLink hypExcel = (HyperLink)e.Row.FindControl("hypExcel");
                hypExcel.Visible = true;
                HyperLink hypDoc = (HyperLink)e.Row.FindControl("hypDoc");
                hypDoc.Visible = false;
                HyperLink hypTXT = (HyperLink)e.Row.FindControl("hypTXT");
                hypTXT.Visible = false;

                Image imgExcel = (Image)e.Row.FindControl("imgExcel");
                imgExcel.ImageUrl = "~/images/icon_excel.png";
                hypExcel.NavigateUrl = "File/" + customer_id + "/DELETEDSELECTIONS/" + file_name;
                hypExcel.Target = "_blank";
              
            }
            if (file_name.Contains(".txt") || file_name.Contains(".TXT"))
            {
                HyperLink hypImg = (HyperLink)e.Row.FindControl("hypImg");
                hypImg.Visible = false;
                HyperLink hypPDF = (HyperLink)e.Row.FindControl("hypPDF");
                hypPDF.Visible = false;
                HyperLink hypExcel = (HyperLink)e.Row.FindControl("hypExcel");
                hypExcel.Visible = false;
                HyperLink hypDoc = (HyperLink)e.Row.FindControl("hypDoc");
                hypDoc.Visible = false;
                HyperLink hypTXT = (HyperLink)e.Row.FindControl("hypTXT");
                hypTXT.Visible = true;

                Image imgTXT = (Image)e.Row.FindControl("imgTXT");
                imgTXT.ImageUrl = "~/images/icon_txt.png";
                hypTXT.NavigateUrl = "File/" + customer_id + "/DELETEDSELECTIONS/" + file_name;
                hypTXT.Target = "_blank";
              
            }
        }
    }
    protected void lnkOpen_Click(object sender, EventArgs e)
    {
        LinkButton btnsubmit = sender as LinkButton;

        GridViewRow gRow = (GridViewRow)btnsubmit.NamingContainer;
        Label lblCallDescriptionG = gRow.Cells[4].Controls[0].FindControl("lblNotes") as Label;
        Label lblCallDescriptionG_r = gRow.Cells[4].Controls[1].FindControl("lblNotes_r") as Label;
        LinkButton lnkOpen = gRow.Cells[4].Controls[3].FindControl("lnkOpen") as LinkButton;

        if (lnkOpen.Text == "More")
        {
            lblCallDescriptionG.Visible = false;
            lblCallDescriptionG_r.Visible = true;
            lnkOpen.Text = " Less";
            lnkOpen.ToolTip = "Click here to view less";
        }
        else
        {
            lblCallDescriptionG.Visible = true;
            lblCallDescriptionG_r.Visible = false;
            lnkOpen.Text = "More";
            lnkOpen.ToolTip = "Click here to view more";
        }
    }

}