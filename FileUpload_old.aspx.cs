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
using System.Collections.Generic;
using System.Drawing;

public partial class FileUpload_old : System.Web.UI.Page
{
    protected string UploadFolderPath = ConfigurationManager.AppSettings["Document_path"].ToString() + "//";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);

            if (Session["FileName"] == null)
            {
                //int nCustomerId = Convert.ToInt32(Session["CustomerId"]);
                div1.Visible = true;
                btnUpload.Visible = true;
                lblOutput.Text = "";
            }
            else
            {
                lblOutput.Text = Session["FileName"].ToString();
                div1.Visible = false;

            }



        }

    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        // Initialize variables
        string sSavePath;
        // Set constant values
        sSavePath = UploadFolderPath;
        // If file field isn’t empty
        if (FileUpload1.PostedFile != null)
        {
            // Check file size (mustn’t be 0)
            HttpPostedFile myFile = FileUpload1.PostedFile;
            int nFileLen = myFile.ContentLength;
            if (nFileLen == 0)
            {
                return;
            }
            // Check file extension
            //if (System.IO.Path.GetExtension(myFile.FileName).ToLower() != ".pdf")
            //{
            //    lblOutput.Text = "The file must have an extension of PDF";
            //    return;
            //}
            string extension = System.IO.Path.GetExtension(myFile.FileName).ToLower();
            // Read file into a data stream
            byte[] myData = new Byte[nFileLen];
            myFile.InputStream.Read(myData, 0, nFileLen);
            // Make sure a duplicate file doesn’t exist.  If it does, keep on appending an incremental numeric until it is unique
            string sFilename = System.IO.Path.GetFileName(myFile.FileName);
            // sFilename = Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]).ToString() + "_" + hdnCustomerId.Value.ToString() + "_" + hdnEstimateId.Value.ToString() + sFilename;

            int file_append = 0;
            while (System.IO.File.Exists(sSavePath + sFilename))
            {
                file_append++;
                sFilename = System.IO.Path.GetFileNameWithoutExtension(myFile.FileName) + file_append.ToString() + "" + extension;
            }
            // Save the stream to disk
            System.IO.FileStream newFile = new System.IO.FileStream(sSavePath + sFilename, System.IO.FileMode.Create);

            newFile.Write(myData, 0, myData.Length);
            newFile.Close();
            btnUpload.Visible = false;
            div1.Visible = false;
            lblOutput.Text = sFilename;
            Session.Add("FileName", sFilename);

        }
    }
}