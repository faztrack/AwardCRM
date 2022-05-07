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
using System.Drawing;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using Signing;
//using System.IO;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;

public partial class CoDocuSignViewer : System.Web.UI.Page
{
    ReportDocument rptFile = new ReportDocument();
    string sFileName = "test";
    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {
            KPIUtility.PageLoad(this.Page.AppRelativeVirtualPath);
            int nEstimateId = Convert.ToInt32(Request.QueryString.Get("eid"));
            hdnEstimateId.Value = nEstimateId.ToString();
            int nCustomerId = Convert.ToInt32(Request.QueryString.Get("cid"));
            hdnCustomerId.Value = nCustomerId.ToString();
            int nChangeId = Convert.ToInt32(Request.QueryString.Get("coid"));
            hdnChangeId.Value = nChangeId.ToString();
            rptFile = (ReportDocument)Session[SessionInfo.Report_File];
            bool bParam = false;
            foreach (string strKey in Session.Keys)
            {
                if (strKey == SessionInfo.Report_Param)
                {
                    bParam = true;
                    break;
                }
            }
            if (bParam)
            {
                Hashtable htable = (Hashtable)Session[SessionInfo.Report_Param];
                ParameterValues param = new ParameterValues();
                ParameterDiscreteValue Val = new ParameterDiscreteValue();
                foreach (ParameterFieldDefinition obj in rptFile.DataDefinition.ParameterFields)
                {
                    if (htable.ContainsKey(obj.Name))
                    {
                        Val.Value = htable[obj.Name].ToString();
                        param.Add(Val);
                        obj.ApplyCurrentValues(param);
                        if (obj.Name.ToString() == "p_CustomerEstimateId")
                            sFileName = Val.Value.ToString();

                    }
                }
            }
            if (Request.QueryString.Count == 1)
            {
                if (Request.QueryString.Keys[0] == "print")
                    rptFile.PrintToPrinter(1, false, 0, 0);
            }
            else
                CRViewer.ReportSource = rptFile;

            exportReport(rptFile, CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
        }
    }

    protected void exportReport(CrystalDecisions.CrystalReports.Engine.ReportDocument selectedReport, CrystalDecisions.Shared.ExportFormatType eft)
    {
        selectedReport.ExportOptions.ExportFormatType = eft;

        string contentType = "";
        // Make sure asp.net has create and delete permissions in the directory
        //string tempDir = System.Configuration.ConfigurationSettings.AppSettings["TempDir"];
        string tempDir = ConfigurationManager.AppSettings["ReportPath"];
        //string tempFileName = Session.SessionID.ToString() + ".";
        string tempFileName = sFileName + ".";

        switch (eft)
        {
            case CrystalDecisions.Shared.ExportFormatType.PortableDocFormat:
                tempFileName += "pdf";
                contentType = "application/pdf";
                break;
            case CrystalDecisions.Shared.ExportFormatType.WordForWindows:
                tempFileName += "doc";
                contentType = "application/msword";
                break;
            case CrystalDecisions.Shared.ExportFormatType.Excel:
                tempFileName += "xls";
                contentType = "application/vnd.ms-excel";
                break;
            case CrystalDecisions.Shared.ExportFormatType.HTML32:
            case CrystalDecisions.Shared.ExportFormatType.HTML40:
                tempFileName += "htm";
                contentType = "text/html";
                CrystalDecisions.Shared.HTMLFormatOptions hop = new CrystalDecisions.Shared.HTMLFormatOptions();
                hop.HTMLBaseFolderName = tempDir;
                hop.HTMLFileName = tempFileName;
                selectedReport.ExportOptions.FormatOptions = hop;
                break;
        }

        CrystalDecisions.Shared.DiskFileDestinationOptions dfo = new CrystalDecisions.Shared.DiskFileDestinationOptions();
        dfo.DiskFileName = tempDir + tempFileName;
        selectedReport.ExportOptions.DestinationOptions = dfo;
        selectedReport.ExportOptions.ExportDestinationType = CrystalDecisions.Shared.ExportDestinationType.DiskFile;
        selectedReport.Export();
        selectedReport.Close();

        string tempFileNameUsed;
        if (eft == CrystalDecisions.Shared.ExportFormatType.HTML32 || eft == CrystalDecisions.Shared.ExportFormatType.HTML40)
        {
            string[] fp = selectedReport.FilePath.Split("\\".ToCharArray());
            string leafDir = fp[fp.Length - 1];
            // strip .rpt extension
            leafDir = leafDir.Substring(0, leafDir.Length - 4);
            tempFileNameUsed = string.Format("{0}{1}\\{2}", tempDir, leafDir, tempFileName);
        }
        else
            tempFileNameUsed = tempDir + tempFileName;

        Response.WriteFile(tempFileNameUsed);
        DataClassesDataContext _db = new DataClassesDataContext();

        //string strQ = " SELECT  pricing_id, pricing_details.client_id, customer_id, estimate_id, pricing_details.location_id, sales_person_id, section_level, item_id, section_name, item_name, measure_unit, item_cost, minimum_qty, quantity, retail_multiplier, labor_rate, labor_id, section_serial, item_cnt, total_direct_price, total_retail_price, is_direct, pricing_type, short_notes,location_name " +
        //            " FROM pricing_details  INNER JOIN location ON pricing_details.location_id=location.location_id AND pricing_details.client_id=location.client_id " +
        //            " WHERE pricing_details.location_id IN (Select location_id from customer_locations WHERE customer_locations.estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND customer_locations.customer_id =" + Convert.ToInt32(hdnCustomerId.Value) + " AND customer_locations.client_id =" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + " ) " +
        //            " AND pricing_details.section_level IN (Select section_id from customer_sections  WHERE customer_sections.estimate_id =" + Convert.ToInt32(hdnEstimateId.Value) + " AND customer_sections.customer_id =" + Convert.ToInt32(hdnCustomerId.Value) + " AND customer_sections.client_id =" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) + " ) " +
        //            " AND estimate_id=" + Convert.ToInt32(hdnEstimateId.Value) + " AND customer_id=" + Convert.ToInt32(hdnCustomerId.Value) + " AND pricing_details.client_id=" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);

        //List<PricingDetailModel> CList = _db.ExecuteQuery<PricingDetailModel>(strQ, string.Empty).ToList();
        //customer_estimate cus_est = new customer_estimate();
        //cus_est = _db.customer_estimates.Single(ce => ce.customer_id == Convert.ToInt32(hdnCustomerId.Value) && ce.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && ce.estimate_id == Convert.ToInt32(hdnEstimateId.Value));
        customer cust = new customer();
        cust = _db.customers.Single(c => c.customer_id == Convert.ToInt32(hdnCustomerId.Value));

        string strCustomerFirstName = cust.first_name1;
        string strCustomerLastName = cust.last_name1;
        string strEmail = (cust.email.Length > 0 ? cust.email : "contact@faztrack.com");
        string strCustomerEstimateId = hdnCustomerId.Value + "_" + hdnEstimateId.Value + "_" + hdnChangeId.Value + "_" + Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
        string strReportPath = ConfigurationManager.AppSettings["ReportPath"];
        WriteSignature(strEmail, strCustomerFirstName, strCustomerLastName, strCustomerEstimateId + ".pdf");

    }


    #region For DocuSign
    void WriteSignature(string email, string first_name, string last_name, string file_name)
    {
        Signing.DocuSignWeb.Recipient signer = new Signing.DocuSignWeb.Recipient();

        Signing.LinkedDocument doc = new LinkedDocument();
        Signing.LinkedTab[] tabs = new LinkedTab[0];
        AccountCredentials creds = GetAPICredentials();
        signer = MakeRecipient(email, first_name, last_name, Session.SessionID);
        // NOTE: for a complicated solution please pick hidden text that is not likely to 
        // appear anywhere in the text.  We have picked E-mail: and Name: here because
        // the solution is very small.  In a complex form a simple anchor tag might result
        // in unintentional tabs being placed.
        tabs = new LinkedTab[]
                            {   //LinkedTab.CreateTab(signer, "E-mail", Request.Form["Email"], "E-mail:", 200, -2),
                                //LinkedTab.CreateTab(signer, "Phone", Request.Form["Phone"], "Phone:", 200, -2),
                                //LinkedTab.CreateTab(Signing.DocuSignWeb.TabTypeCode.FullName, signer, "Name:", 200, -2),
                                LinkedTab.CreateTab(Signing.DocuSignWeb.TabTypeCode.SignHere,signer, "Customer Signature", -10, -30),
                                //LinkedTab.CreateTab(Signing.DocuSignWeb.TabTypeCode.SignHere,signer, "Client:", 50, 0),
                                //LinkedTab.CreateTab(Signing.DocuSignWeb.TabTypeCode.InitialHere,signer, "I have received a copy", -30, 40),
                                //LinkedTab.CreateTab(Signing.DocuSignWeb.TabTypeCode.InitialHereOptional,signer, "Declined - I certify that", -30, 40),
                                //LinkedTab.CreateTab(Signing.DocuSignWeb.TabTypeCode.InitialHereOptional,signer, "Unavailable for signature", -30, 40)
                            };

        doc = MakeDocument("CODoc", Server.MapPath(@"Reports\Common\" + file_name));
        doc.Tabs = tabs;
        // add our signer to an array
        Signing.DocuSignWeb.Recipient[] signers = new Signing.DocuSignWeb.Recipient[] { signer };
        // add our doc to an array
        Signing.LinkedDocument[] docs = new LinkedDocument[] { doc };
        // now send the envelope
        Envelope envelope = null;
        try
        {
            envelope = Envelope.CreateAndSendEnvelope(creds, signers, docs, "DocuSign Contract Sample Docs", "Hello!  This was submitted from a sample application");
        }
        catch (Exception excp)
        {
            GoToErrorPage(excp.Message);
        }

        // Now that the envelope has been sent, we want to open it up so the applicant can sign it. 
        // We will get something called a RecipientToken, which is an URL that will open the envelope up for the specified recipient

        // here we are using the Ticks from the clock as our assertion ID. Of course, you wouldn't do this in real code, you 
        // would use an identifier that made sense, like the applicants ID from their database record. 
        // This value is informational for you - it allows you to use your own id to refer to the recipient.
        Signing.DocuSignWeb.RequestRecipientTokenAuthenticationAssertion assert = MakeRecipientTokenAuthAssert(System.DateTime.Now.Ticks.ToString());
        // Now we need to setup a page that will be loaded when the signing is concluded. 
        // when the signer is done signing (or cancels) DocuSign will redirect the browser to this URL. 
        // Since we are hosting the signing in an iframe, we will have docusign redirect the frame to a frame-pop html page
        // which will then redirect to the final handler (SigningReturn.aspx)
        // first, get the absolute URL of our frame-pop page
        string retUrl = Request.Url.AbsoluteUri.Replace("CoDocuSignViewer.aspx", "pop.html");
        // then create a URI from the URL
        Uri retUri = new Uri(retUrl, UriKind.Absolute);
        // the build the set of redirect URLS. There is a set because you could specify a different redirect target for each
        // status event - signing completed, cancelled, error, etc. We just use one page with different Querystring params to 
        // indicate the event.
        Signing.DocuSignWeb.RequestRecipientTokenClientURLs clientURLs = Envelope.StandardUrls(retUri, signers[0].CaptiveInfo.ClientUserId);
        // finally, pass all this into docusign and get the token back in return.
        string token = null;
        try
        {
            token = envelope.RequestRecipientToken(signer, assert, clientURLs);
        }
        catch (Exception excp)
        {
            GoToErrorPage(excp.Message);
        }
        // now we store the envelope & token in session and redirect to the signing host page
        Session["EmbeddedToken"] = token;
        Session["Envelope"] = envelope;
        Response.Redirect("EmbeddedHost.aspx", false);
    }
    AccountCredentials GetAPICredentials()
    {
        AccountCredentials credentials = new AccountCredentials();

        if (SettingIsSet("IntegratorsKey") && SettingIsSet("APIAccountId") && SettingIsSet("APIUrl") && SettingIsSet("APIUserEmail") && SettingIsSet("Password"))
        {
            credentials.UserName = "[" + ConfigurationManager.AppSettings["IntegratorsKey"] + "]";
            credentials.UserName += ConfigurationManager.AppSettings["APIUserEmail"];
            credentials.Password = ConfigurationManager.AppSettings["Password"];
            credentials.AccountId = ConfigurationManager.AppSettings["APIAccountId"];
            credentials.ApiUrl = ConfigurationManager.AppSettings["APIUrl"];
        }
        else
        {
            GoToErrorPage("ENTERCREDS");
        }

        return credentials;
    }
    public bool SettingIsSet(string settingName)
    {
        // check if a value is specified in config file
        return (ConfigurationManager.AppSettings[settingName] != null && ConfigurationManager.AppSettings[settingName].Length > 0);
    }

    void GoToErrorPage(string errorMessage)
    {
        Session["errorMessage"] = errorMessage;
        Response.Redirect("error.aspx", true);
    }
    Signing.DocuSignWeb.Recipient MakeRecipient(string email, string firstName, string lastName, string clientUserId)
    {
        Signing.DocuSignWeb.Recipient r = new Signing.DocuSignWeb.Recipient();
        r.Email = email;
        r.UserName = firstName + " " + lastName;
        r.Type = Signing.DocuSignWeb.RecipientTypeCode.Signer;
        r.RequireIDLookup = false;

        r.CaptiveInfo = new Signing.DocuSignWeb.RecipientCaptiveInfo();
        r.CaptiveInfo.ClientUserId = clientUserId;
        r.SignatureInfo = new Signing.DocuSignWeb.RecipientSignatureInfo();
        r.SignatureInfo.SignatureInitials =
            (firstName.Length > 0 ? firstName.Substring(0, 1) : "")
            + (lastName.Length > 0 ? lastName.Substring(0, 1) : "");
        r.SignatureInfo.FontStyle = Signing.DocuSignWeb.FontStyleCode.BradleyHandITC;
        r.SignatureInfo.SignatureName = firstName + " " + lastName;

        return r;
    }
    Signing.LinkedDocument MakeDocument(string documentName, string fqFileName)
    {
        LinkedDocument doc = new LinkedDocument();
        doc.Document.Name = documentName;
        using (System.IO.FileStream streamReader = new System.IO.FileStream(fqFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
        {
            byte[] pdfBytes = new byte[streamReader.Length];
            streamReader.Read(pdfBytes, 0, (int)streamReader.Length);
            doc.Document.PDFBytes = pdfBytes;
        }
        return doc;
    }
    public static Signing.DocuSignWeb.RequestRecipientTokenAuthenticationAssertion MakeRecipientTokenAuthAssert(string assertionId)
    {
        Signing.DocuSignWeb.RequestRecipientTokenAuthenticationAssertion assert = new Signing.DocuSignWeb.RequestRecipientTokenAuthenticationAssertion();
        assert.AssertionID = assertionId;
        assert.AuthenticationInstant = DateTime.Now;
        assert.AuthenticationMethod = Signing.DocuSignWeb.RequestRecipientTokenAuthenticationAssertionAuthenticationMethod.Password;
        assert.SecurityDomain = "LoanCo Demo";

        return assert;
    }
    #endregion
}

