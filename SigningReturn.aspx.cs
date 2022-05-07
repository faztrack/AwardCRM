using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.IO;

public partial class SigningReturn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);
        }
        ProcessEvent();
        //DownloadPdfEvent();

    }
    void ProcessEvent()
    {
        downloadPdf.Enabled = false;
        string uname = Request["uname"];
        // string signingEvent = Request["event"];
        string signingEvent = Request.RawUrl;

        if (signingEvent.IndexOf("SignComplete") > 0)
        {
            downloadPdf.Enabled = true;
            statusLabel.Text = "The user has completed the signing.  The legally binding document with signatures is stored on the DocuSign, Inc. server.";
        }
        else if (signingEvent.IndexOf("ViewComplete") > 0)
            statusLabel.Text = "The user has viewed the document without signing it.";
        else if (signingEvent.IndexOf("Cancel") > 0)
            statusLabel.Text = "The user has cancelled out of the signign experience";
        else if (signingEvent.IndexOf("Decline") > 0)
            statusLabel.Text = "The user has declined to sign the document.";
        else if (signingEvent.IndexOf("Timeout") > 0)
            statusLabel.Text = "The user did not sign the document in time.  The timeout is set to 20 minutes.";
        else if (signingEvent.IndexOf("TTLExpired") > 0)
            statusLabel.Text = "Trusted connection has expired.  The server communication might be a problem.";
        else if (signingEvent.IndexOf("IDCheck") > 0)
            statusLabel.Text = "The ID Check has failed.  The user was denied an opportunity to view or sign the document.";
        else if (signingEvent.IndexOf("AccessCode") > 0)

            statusLabel.Text = "The access code verification has failed.  The user was denied an opportunity to view or sign the document.";

        else if (signingEvent.IndexOf("Exception") > 0)

            statusLabel.Text = "An exception has occurred on the server.  Please check the parameters passed to the Web Service Methods.";
        else
        {
            Debug.Assert(false,
                "Got an unexpected code back: " + signingEvent);
            // by default assign the return even to the label 
            // to debug the unexpected signing event strings 
            statusLabel.Text = signingEvent;
        }
    }
    void DownloadPdfEvent()
    {
        Debug.WriteLine("Session ID:" + Session.SessionID);
        Signing.Envelope envelope = (Signing.Envelope)Session["Envelope"];
        byte[] bytes = envelope.RetrievePdfBytes();

        Response.Clear();
        Response.AddHeader("Content-Type", "application/pdf");
        Response.AddHeader("Content-Length", bytes.Length.ToString());
        Response.AddHeader("Content-Disposition", "attachment; filename=Contract.pdf");
        Response.BinaryWrite(bytes);
        //Response.WriteFile(tempFileNameUsed);
        Response.End();
    }


    protected void downloadPdf_Click(object sender, EventArgs e)
    {
        KPIUtility.SaveEvent(this.Page.AppRelativeVirtualPath, downloadPdf.ID, downloadPdf.GetType().Name, "Click");
        DownloadPdfEvent();
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("customerlist.aspx");
    }
}
