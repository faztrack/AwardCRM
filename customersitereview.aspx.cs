﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class customersitereview : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);

            if (Session["oCustomerUser"] == null)
            {
                Response.Redirect("customerlogin.aspx");
            }
                txtStartDate.Text = DateTime.Now.AddDays(-7).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                GetSiteReviews();
        }
    }

    private void GetSiteReviews()
    {
        try
        {
            customeruserinfo objC = new customeruserinfo();
            if (Session["oCustomerUser"] != null)
            {
                objC = (customeruserinfo)Session["oCustomerUser"];
            }
            DataClassesDataContext _db = new DataClassesDataContext();
            string strCondition = "";
            if (txtStartDate.Text != "" && txtEndDate.Text != "")
            {
                DateTime strStartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
                DateTime strEndDate = Convert.ToDateTime(txtEndDate.Text.Trim());
                strCondition = "WHERE  SiteReviewsDate>='" + strStartDate + "' AND  SiteReviewsDate<'" + strEndDate.AddDays(1).ToString() + "' AND [IsCustomerView]=1 AND [customer_id]=" + objC.customerid;
            }

            string strQ = string.Empty;
            strQ = " select [SiteReviewsId] ,[customer_id],[estimate_id],[SiteReviewsNotes],[SiteReviewsDate],[StateOfMindID], " +
                   " [IsUserView],[IsCustomerView] ,[IsVendorView],[HasAttachments] ,[AttachmentList] ,[CreatedBy] ,[CreateDate] ,[LastUpdatedBy],[LastUpdateDate] " +
                   " from [SiteReviewNotes] " + strCondition + " order by CreateDate DESC";

            IEnumerable<csSiteReview> mList = _db.ExecuteQuery<csSiteReview>(strQ, string.Empty).ToList();
            if (mList.Count() > 0)
                Session.Add("nGSiteReviewList", csCommonUtility.LINQToDataTable(mList));
            else
                Session.Remove("nGSiteReviewList");

              BindSiteReviewDetails(0);
          }
        catch (Exception ex)
        {
            lblResult.Text = ex.Message;
        }
    }



    protected void BindSiteReviewDetails(int nPageNo)
    {
        try
        {
            if (Session["nGSiteReviewList"] != null)
            {
                DataTable dtSiteReview = (DataTable)Session["nGSiteReviewList"];

                grdSiteViewList.DataSource = dtSiteReview;
                grdSiteViewList.PageSize = Convert.ToInt32(ddlItemPerPage.SelectedValue);
                grdSiteViewList.PageIndex = nPageNo;
                grdSiteViewList.DataKeyNames = new string[] { "SiteReviewsId", "customer_id", "estimate_id", "SiteReviewsDate", "IsCustomerView", "SiteReviewsNotes", "StateOfMindID" };
                grdSiteViewList.DataBind();
            }
            else
            {
                grdSiteViewList.DataSource = null;
                grdSiteViewList.DataBind();
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

            if (grdSiteViewList.PageCount == nPageNo + 1)
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
        catch (Exception ex)
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage(ex.Message);
        }

    }
    protected void ddlItemPerPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSiteReviewDetails(0);
    }
    protected void lnkViewAll_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, LinkButton1.ID, LinkButton1.GetType().Name, "Click"); 
        ddlItemPerPage.SelectedIndex = 0;
        txtStartDate.Text = DateTime.Now.AddDays(-7).ToShortDateString();
        txtEndDate.Text = DateTime.Now.ToShortDateString();
        lblResult.Text = "";
        GetSiteReviews();
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnNext.ID, btnNext.GetType().Name, "Click"); 
        int nCurrentPage = 0;
        nCurrentPage = Convert.ToInt32(lblCurrentPageNo.Text);
        BindSiteReviewDetails(nCurrentPage);
    }
    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnPrevious.ID, btnPrevious.GetType().Name, "Click"); 
        int nCurrentPage = 0;
        nCurrentPage = Convert.ToInt32(lblCurrentPageNo.Text);
        BindSiteReviewDetails(nCurrentPage - 2);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, btnSearch.ID, btnSearch.GetType().Name, "Click"); 
        lblResult.Text = "";
        DateTime strStartDate = DateTime.Now;
        DateTime strEndDate = DateTime.Now;
        if (txtStartDate.Text == "")
        {
            lblResult.Text = csCommonUtility.GetSystemRequiredMessage("Start Date is a required field");

            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtStartDate.Text);
            }
            catch (Exception ex)
            {
                lblResult.Text = csCommonUtility.GetSystemErrorMessage("Invalid Start Date");

                return;
            }
            strStartDate = Convert.ToDateTime(txtStartDate.Text);
        }

        if (txtEndDate.Text == "")
        {
            lblResult.Text = csCommonUtility.GetSystemRequiredMessage("End Date is a required field");

            return;
        }
        else
        {
            try
            {
                Convert.ToDateTime(txtEndDate.Text);
            }
            catch (Exception ex)
            {
                lblResult.Text = csCommonUtility.GetSystemErrorMessage("Invalid End Date");

                return;
            }
            strEndDate = Convert.ToDateTime(txtEndDate.Text);
        }
        if (strStartDate > strEndDate)
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage("Invalid Date Range");

            return;
        }

        GetSiteReviews();
    }
   protected void grdSiteViewList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, grdSiteViewList.ID, grdSiteViewList.GetType().Name, "PageIndexChanging"); 
        BindSiteReviewDetails(e.NewPageIndex);
    }
    protected void grdSiteViewList_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        DataClassesDataContext _db = new DataClassesDataContext();
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int SiteReviewsId = Convert.ToInt32(grdSiteViewList.DataKeys[e.Row.RowIndex].Values[0].ToString());
            int customer_id = Convert.ToInt32(grdSiteViewList.DataKeys[e.Row.RowIndex].Values[1].ToString());
            int estimate_id = Convert.ToInt32(grdSiteViewList.DataKeys[e.Row.RowIndex].Values[2].ToString());
            DateTime SiteReviewsDate = Convert.ToDateTime(grdSiteViewList.DataKeys[e.Row.RowIndex].Values[3]);
            Boolean IsCustomer = Convert.ToBoolean(grdSiteViewList.DataKeys[e.Row.RowIndex].Values[4]);
            string SiteReviewNote = grdSiteViewList.DataKeys[e.Row.RowIndex].Values[5].ToString();
            Label lblSiteReviewNote = (Label)e.Row.FindControl("lblSiteReviewNote");

            Label lblNotesLabel = (Label)e.Row.FindControl("lblNotesLabel");
            if (SiteReviewNote != null && SiteReviewNote != "")
                lblNotesLabel.Text = "Notes:";
            else
            {
                lblNotesLabel.Visible = false;
                lblNotesLabel.Text = "";
            }
            lblSiteReviewNote.Text = SiteReviewNote;
        
            if (SiteReviewsId != 0)
            {
                GridView gvUp = e.Row.FindControl("grdUploadedFileList") as GridView;
                GetUploadedFileListData(gvUp, SiteReviewsId);
            }

        }
    }


    private void DeleteTemporaryFiles()
    {
        string DestinationPath = Server.MapPath("~/Uploads//Temp//");
        if (Directory.Exists(DestinationPath))
        {
            string[] fileEntries = Directory.GetFiles(DestinationPath);
            foreach (string file in fileEntries)
            {
                File.Delete(file);
            }

        }
    }



    protected void grdUploadedFileList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            GridView grdUploadedFileList = (GridView)sender;
            try
            {
                Label lblImages = (Label)e.Row.Parent.Parent.Parent.FindControl("lblImages");


                int SiteReview_attach_id = Convert.ToInt32(grdUploadedFileList.DataKeys[e.Row.RowIndex].Values[0].ToString());
                int SiteReviewsId = Convert.ToInt32(grdUploadedFileList.DataKeys[e.Row.RowIndex].Values[1].ToString());
                int customer_id = Convert.ToInt32(grdUploadedFileList.DataKeys[e.Row.RowIndex].Values[2].ToString());
                int estimate_id = Convert.ToInt32(grdUploadedFileList.DataKeys[e.Row.RowIndex].Values[3].ToString());
                string SiteReview_file_name = grdUploadedFileList.DataKeys[e.Row.RowIndex].Values[4].ToString();

                if (SiteReview_file_name != null && SiteReview_file_name != "")
                {
                    lblImages.Text = "Files:";
                }
                else
                    lblImages.Visible = false;

                //string fileName = SiteReview_file_name.Substring(0, 10);
                //Label lblFileName = (Label)e.Row.FindControl("lblFileName");
                //lblFileName.Text = fileName;


                if (SiteReview_file_name.Contains(".jpg") || SiteReview_file_name.Contains(".jpeg") || SiteReview_file_name.Contains(".png") || SiteReview_file_name.Contains(".gif"))
                {
                    HyperLink hypImg = (HyperLink)e.Row.FindControl("hypImg");
                    hypImg.Visible = true;

                    // hypImg.NavigateUrl = "UploadedFiles/" + customer_id + "/IMAGES/" + SiteReview_file_name;
                    //hypImg.Target = "_blank";

                    hypImg.Attributes.Add("onclick", "window.open('generalsitereview_image_gallery.aspx?gsid=" + SiteReviewsId + "&cid=" + customer_id + "', 'MyWindow', 'left=150,top=100,width=900,height=650,status=0,toolbar=0,resizable=0,scrollbars=1');");

                    Image img = (Image)e.Row.FindControl("img");

                    HyperLink hypPDF = (HyperLink)e.Row.FindControl("hypPDF");
                    hypPDF.Visible = false;
                    HyperLink hypExcel = (HyperLink)e.Row.FindControl("hypExcel");
                    hypExcel.Visible = false;
                    HyperLink hypDoc = (HyperLink)e.Row.FindControl("hypDoc");
                    hypDoc.Visible = false;
                    HyperLink hypTXT = (HyperLink)e.Row.FindControl("hypTXT");
                    hypTXT.Visible = false;

                    img.ImageUrl = "UploadedFiles/" + customer_id + "/IMAGES/" + SiteReview_file_name;
                    // img.Attributes.Add("data-zoom-image", "UploadedFiles/" + customer_id + "/IMAGES/" + SiteReview_file_name);
                }


                if (SiteReview_file_name.Contains(".pdf"))
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
                    hypPDF.NavigateUrl = "UploadedFiles/" + customer_id + "/UPLOAD/" + SiteReview_file_name;
                    hypPDF.Target = "_blank";
                }
                if (SiteReview_file_name.Contains(".doc") || SiteReview_file_name.Contains(".docx"))
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
                    hypDoc.NavigateUrl = "UploadedFiles/" + customer_id + "/UPLOAD/" + SiteReview_file_name;
                    hypDoc.Target = "_blank";

                }
                if (SiteReview_file_name.Contains(".xls") || SiteReview_file_name.Contains(".xlsx") || SiteReview_file_name.Contains(".csv"))
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
                    hypExcel.NavigateUrl = "UploadedFiles/" + customer_id + "/UPLOAD/" + SiteReview_file_name;
                    hypExcel.Target = "_blank";
                }
                if (SiteReview_file_name.Contains(".txt") || SiteReview_file_name.Contains(".TXT"))
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
                    hypTXT.NavigateUrl = "UploadedFiles/" + customer_id + "/UPLOAD/" + SiteReview_file_name;
                    hypTXT.Target = "_blank";
                }

            }
            catch (Exception ex)
            {
                lblResult.Text = csCommonUtility.GetSystemErrorMessage(ex.Message);
            }
        }

    }
   private void GetUploadedFileListData(GridView gvUp, int nSiteReviewId)
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        try
        {

            DeleteTemporaryFiles();
            SiteReview_upolad_info objSRU = new SiteReview_upolad_info();
            var attchmentList = from su in _db.SiteReview_upolad_infos
                                where su.SiteReviewsId == nSiteReviewId
                                orderby su.SiteReview_attach_id ascending
                                select su;
            gvUp.DataSource = attchmentList;
            gvUp.DataKeyNames = new string[] { "SiteReview_attach_id", "SiteReviewsId", "customer_id", "estimate_id", "SiteReview_file_name" };
            gvUp.DataBind();




        }
        catch (Exception ex)
        {
            lblResult.Text = csCommonUtility.GetSystemErrorMessage(ex.Message);
        }
    }
}