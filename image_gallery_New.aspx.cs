using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class image_gallery_New : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);
            lnkClose.Attributes.Add("onClick", "CloseWindow();");
            lnkPrint.Attributes.Add("onClick", "PrintWindow();");

            if (Request.QueryString.Get("jsid") != null)
            {
                int nCustomerId = 0;
                int nEstimateId = 0;
                if (Request.QueryString.Get("cid") != null)
                    nCustomerId = Convert.ToInt32(Request.QueryString.Get("cid"));
                if (Request.QueryString.Get("eid") != null)
                    nEstimateId = Convert.ToInt32(Request.QueryString.Get("eid"));
                int nJobStatusId = Convert.ToInt32(Request.QueryString.Get("jsid"));
                hdnCustomerId.Value = nCustomerId.ToString();
                hdnEstimateId.Value = nEstimateId.ToString();
                DataClassesDataContext _db = new DataClassesDataContext();

                customer objCust = new customer();
                objCust = _db.customers.Single(c => c.customer_id == nCustomerId);

                lblCustomerName.Text = objCust.first_name1 + " " + objCust.last_name1;



                lblJobStatusFor.Text = "Image Gallery";




                GetCustomerImageInfo(Convert.ToInt32(hdnCustomerId.Value));




            }
        }


    }



    protected void grdCustomersImage_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            try
            {

                int upload_ImageId = Convert.ToInt32(grdCustomersImage.DataKeys[e.Row.RowIndex].Values[0].ToString());
                string strImage = grdCustomersImage.DataKeys[e.Row.RowIndex].Values[2].ToString();
                int CustomerId = Convert.ToInt32(grdCustomersImage.DataKeys[e.Row.RowIndex].Values[3].ToString());
                string Desccription = grdCustomersImage.DataKeys[e.Row.RowIndex].Values[4].ToString();

                Label lblDescription = (Label)e.Row.FindControl("lblDescription");
                HyperLink hypImage = (HyperLink)e.Row.FindControl("hypImage");
                Image img = (Image)e.Row.FindControl("img");



                hypImage.NavigateUrl = "UploadedFiles/" + CustomerId + "/" + "UPLOAD/" + strImage;
                hypImage.Attributes.Add("data-ilb2-caption", Desccription);
                hypImage.Attributes.Add("data-ilb3-gellarytitle", "Site Photos for: " + lblCustomerName.Text);

                img.ImageUrl = "UploadedFiles/" + CustomerId + "/" + "UPLOAD/" + strImage;
                img.Attributes.Add("data-zoom-image", "UploadedFiles/" + CustomerId + "/" + "UPLOAD/" + strImage);

                if (Desccription != "" && Desccription.Length > 10)
                {
                    lblDescription.Text = Desccription.Substring(0, 10) + " <u style='color:#992a4f; font-weight:bold;'>...</u>";
                    lblDescription.ToolTip = Desccription;

                }
                else
                {
                    lblDescription.Text = Desccription;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    protected void grdCustomersImage_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataClassesDataContext _db = new DataClassesDataContext();
        int upload_ImageId = Convert.ToInt32(grdCustomersImage.DataKeys[e.RowIndex].Values[0].ToString());
        string strQ = "Delete file_upload_info WHERE upload_fileId=" + Convert.ToInt32(upload_ImageId) + " AND client_id =" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
        _db.ExecuteCommand(strQ, string.Empty);
        GetCustomerImageInfo(Convert.ToInt32(hdnCustomerId.Value));

    }

    protected void grdCustomersImage_RowEditing(object sender, GridViewEditEventArgs e)
    {
        TextBox txtDescription = (TextBox)grdCustomersImage.Rows[e.NewEditIndex].FindControl("txtDescription");
        Label lblDescription = (Label)grdCustomersImage.Rows[e.NewEditIndex].FindControl("lblDescription");
        txtDescription.Visible = true;
        lblDescription.Visible = false;
        ImageButton imgEdit = (ImageButton)grdCustomersImage.Rows[e.NewEditIndex].FindControl("imgEdit");
        ImageButton imgDelete = (ImageButton)grdCustomersImage.Rows[e.NewEditIndex].FindControl("imgDelete");

        imgEdit.ImageUrl = "~/images/icon_save_16x16.png";
        imgEdit.CommandName = "Update";

        imgDelete.Visible = false;
    }

    protected void grdCustomersImage_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DataClassesDataContext _db = new DataClassesDataContext();

        int upload_ImageId = Convert.ToInt32(grdCustomersImage.DataKeys[e.RowIndex].Values[0].ToString());
        TextBox txtDescription = (TextBox)grdCustomersImage.Rows[e.RowIndex].FindControl("txtDescription");
        Label lblDescription = (Label)grdCustomersImage.Rows[e.RowIndex].FindControl("lblDescription");

        ImageButton imgEdit = (ImageButton)grdCustomersImage.Rows[e.RowIndex].FindControl("imgEdit");
        ImageButton imgDelete = (ImageButton)grdCustomersImage.Rows[e.RowIndex].FindControl("imgDelete");

        string StrQ = "UPDATE file_upload_info SET Desccription='" + txtDescription.Text.Replace("'", "''") + "' WHERE upload_fileId=" + Convert.ToInt32(upload_ImageId) + " AND client_id =" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
        _db.ExecuteCommand(StrQ, string.Empty);
        GetCustomerImageInfo(Convert.ToInt32(hdnCustomerId.Value));

        imgEdit.ImageUrl = "~/images/icon_edit_16x16.png";
        imgEdit.CommandName = "Edit";

        imgDelete.Visible = true;
    }

    private void GetCustomerImageInfo(int nCustId)
    {
        try
        {
            DataClassesDataContext _db = new DataClassesDataContext();

            string[] aryImageType = new string[] { "png", "jpg" };

            if (Convert.ToInt32(hdnCustomerId.Value) > 0)
            {
                //var itemImage = from f in _db.file_upload_infos
                //                where f.CustomerId == nCustId                               
                //                && f.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                //                && f.type != 1
                //                && (f.ImageName.ToString().ToLower().Contains("jpg") || f.ImageName.ToString().ToLower().Contains("png") || f.ImageName.ToString().ToLower().Contains("jpeg"))
                //                orderby f.upload_fileId ascending
                //                select f;

                var itemImage = from f in _db.FilesTables
                                where f.CustomerId == nCustId


                                && (f.FileName.ToString().ToLower().Contains("jpg") || f.FileName.ToString().ToLower().Contains("png") || f.FileName.ToString().ToLower().Contains("jpeg"))
                                orderby f.FileId ascending
                                select f;

                //  DataTable dt = csCommonUtility.LINQToDataTable(itemImage);

                //if (itemImage.Count() > 0)
                //{
                //    lblImageGalleryTitle.Visible = true;
                //}

                grdCustomersImage.DataSource = itemImage;
                grdCustomersImage.DataKeyNames = new string[] { "FileId", "FileName", "CustomerId" };
                grdCustomersImage.DataBind();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //protected void imgDelete_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DataClassesDataContext _db = new DataClassesDataContext();

    //        ImageButton btn = (ImageButton)sender;
    //        int upload_ImageId = Convert.ToInt32(btn.CommandArgument);
    //        string strQ = "Delete Image_upload_info WHERE upload_ImageId=" + Convert.ToInt32(upload_ImageId) + " AND client_id =" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
    //        _db.ExecuteCommand(strQ, string.Empty);
    //        GetCustomerImageInfo(Convert.ToInt32(hdnCustomerId.Value));
    //    }
    //    catch (Exception ex)
    //    {
    //        Session.Add("sAddCartError", "AddCartError, " + ex.Message);
    //    }
    //}







}