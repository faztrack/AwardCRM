using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Exchange.WebServices.Data;
using System.IO;
using System.Configuration;

using System.Net.Mail;
/// <summary>
/// Summary description for EmailAPI
/// </summary>
public class csEmailAPI
{
    public csEmailAPI()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public int CustomerId = 0;
    public int EstimateId = 0;
    public int ProtocolType = 0;
    public string From = "";
    public string To = "";
    public string CC = "";
    public string BCC = "";
    public string Subject = "";
    public string Body = "";

    public string[] fileEntries = null;

    public string UserName = "";
    public string NewAttachmentPath = "";
    public bool IsSaveEmailInDB = true;

    public string FromDisplayName = "";
    public int FollowUpMsgTypeId = 0;
    public string EstimateName = "";
    public string CustomerName = "";

    public bool SendEmail()
    {
        try
        {
            ProtocolType = 0;
            DataClassesDataContext _db = new DataClassesDataContext();

            string sAttListString = "";
            string sMessageUniqueId = "";
            bool IsmailSent = false;

            int nMsId = 0;

            if (ProtocolType == 0)// Outlook
            {
                ExchangeService service = EWSAPI.GetEWSService();
                if (From == "" || service == null)
                {
                    service = EWSAPI.GetEWSService("bryan@awardkb.com", "381fxoBO!");

                    From = "bryan@awardkb.com";
                }
                EmailMessage message = new EmailMessage(service);
                message.Subject = Subject;
                message.Body = Body;

                if (To.Length > 4)
                {
                    string[] strIds = To.Split(',');
                    foreach (string strId in strIds)
                    {
                        message.ToRecipients.Add(strId.Trim());
                    }
                }

                if (CC.Length > 4)
                {
                    string[] strCCIds = CC.Split(',');
                    foreach (string strCCId in strCCIds)
                    {
                        message.CcRecipients.Add(strCCId.Trim());
                    }
                }

                if (BCC.Length > 4)
                {
                    string[] strBCCIds = BCC.Split(',');
                    foreach (string strBCCId in strBCCIds)
                    {
                        message.BccRecipients.Add(strBCCId.Trim());
                    }
                }
                if (ConfigurationManager.AppSettings["DevBCC"] != null)
                {
                    string devBCC = ConfigurationManager.AppSettings["DevBCC"];
                    if (devBCC.Length > 4)
                        message.BccRecipients.Add(devBCC.Trim());
                }

                try
                {
                    if (fileEntries != null)
                    {

                        foreach (string fileName in fileEntries)
                        {
                            string FName = Path.GetFileName(fileName);
                            string extFName = FName.Substring(0, FName.IndexOf('_')).Trim() + Path.GetExtension(fileName);
                            message.Attachments.AddFileAttachment(extFName, fileName);

                            sAttListString += extFName + ", ";
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                // Create a custom extended property and add it to the message.

                Guid PropertySetId = Guid.NewGuid();
                ExtendedPropertyDefinition fazExtendedPropertyDefinition = new ExtendedPropertyDefinition(PropertySetId, "FaztrackPropertyName", MapiPropertyType.String);
                message.SetExtendedProperty(fazExtendedPropertyDefinition, "FaztrackPropertyName");

                message.SendAndSaveCopy();

                System.Threading.Thread.Sleep(1000);

                sMessageUniqueId = EWSAPI.GetEmailId(fazExtendedPropertyDefinition, service);
                IsmailSent = true;

            }

            else // Other
            {
                //From = "faztrackclient@gmail.com";
                MailMessage msg = new MailMessage();

                if (From.Length > 4)
                {
                    if (FromDisplayName.Length > 0)
                    {
                        msg.From = new MailAddress(From, FromDisplayName);
                    }
                    else
                    {
                        msg.From = new MailAddress(From);
                    }
                }

                if (To.Length > 4)
                    msg.To.Add(To);
                if (CC.Length > 4)
                    msg.CC.Add(CC);
                if (BCC.Length > 4)
                    msg.Bcc.Add(BCC);

                msg.Subject = Subject;
                msg.IsBodyHtml = true;
                msg.Body = Body;
                msg.Priority = MailPriority.High;

                try
                {
                    if (fileEntries != null)
                    {

                        foreach (string fileName in fileEntries)
                        {
                            string FName = Path.GetFileName(fileName);
                            string extFName = FName.Substring(0, FName.IndexOf('_')).Trim() + Path.GetExtension(fileName);
                            msg.Attachments.Add(new System.Net.Mail.Attachment(fileName));
                            sAttListString += extFName + ", ";
                        }
                    }
                }
                catch (Exception ex)
                {

                }

                SmtpClient smtp = new SmtpClient();
                smtp.Host = System.Configuration.ConfigurationManager.AppSettings["smtpserver"];
                smtp.Send(msg);
                IsmailSent = true;

                msg.Dispose();
            }

            if (IsSaveEmailInDB)
            {

                // Save Message in DB
                if (CustomerId > 0 && IsmailSent == true) //&& sMessageUniqueId.Length > 0
                {

                    var result = (from cm in _db.customer_messages
                                  where cm.customer_id == CustomerId && cm.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"])
                                  select cm.message_id);

                    int n = result.Count();
                    if (result != null && n > 0)
                        nMsId = result.Max();
                    nMsId = nMsId + 1;

                    //hdnMessageId.Value = nMsId.ToString();


                    string strCC = string.Empty;
                    customer_message cus_ms = new customer_message();
                    cus_ms.client_id = Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
                    cus_ms.customer_id = CustomerId;
                    cus_ms.message_id = nMsId;
                    cus_ms.sent_by = UserName;
                    cus_ms.mess_to = To;
                    cus_ms.mess_from = From;

                    cus_ms.mess_cc = CC;
                    cus_ms.mess_bcc = BCC;
                    cus_ms.mess_description = Body;
                    cus_ms.mess_subject = Subject;
                    cus_ms.last_view = DateTime.Now;
                    cus_ms.create_date = DateTime.Now;


                    try
                    {
                        cus_ms.Outlook_mess_id = sMessageUniqueId;

                    }
                    catch
                    {
                        cus_ms.Outlook_mess_id = "";
                    }
                    cus_ms.Type = "Sent";
                    if (ProtocolType == 1)
                    {
                        cus_ms.Protocol = "Outlook";
                    }
                    else if (ProtocolType == 2)
                    {
                        cus_ms.Protocol = "Gmail";
                    }
                    else
                    {
                        cus_ms.Protocol = "FazTimate";
                    }
                    cus_ms.IsView = false;
                    if (sAttListString.Length > 0)
                    {
                        cus_ms.HasAttachments = true;
                        cus_ms.AttachmentList = sAttListString.Trim().TrimEnd(',');
                    }
                    else
                    {
                        cus_ms.HasAttachments = false;
                        cus_ms.AttachmentList = "";

                    }
                    _db.customer_messages.InsertOnSubmit(cus_ms);

                }
                _db.SubmitChanges();

                // Save Attachment

                if (sAttListString.Length > 0)
                {
                    string NewDir = NewAttachmentPath + nMsId;
                    if (!System.IO.Directory.Exists(NewDir))
                    {
                        System.IO.Directory.CreateDirectory(NewDir);
                    }
                    foreach (string file in fileEntries)
                    {
                        string FileName = Path.GetFileName(file);
                        File.Move(file, Path.Combine(NewDir, FileName));
                        if (CustomerId > 0 && nMsId > 0)
                        {
                            message_upolad_info mui = new message_upolad_info();
                            if (_db.message_upolad_infos.Where(l => l.client_id == Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]) && l.customer_id == CustomerId && l.message_id == nMsId && l.mess_file_name == FileName.ToString()).SingleOrDefault() == null)
                            {
                                mui.client_id = Convert.ToInt32(ConfigurationManager.AppSettings["client_id"]);
                                mui.customer_id = CustomerId;
                                mui.message_id = nMsId;
                                mui.mess_file_name = FileName.ToString();
                                mui.create_date = DateTime.Now;
                                _db.message_upolad_infos.InsertOnSubmit(mui);
                            }

                        }
                    }
                    _db.SubmitChanges();
                }

                if (FollowUpMsgTypeId > 0)
                {
                    followup_message followMsg = new followup_message();
                    followMsg.customer_id = CustomerId;
                    followMsg.message_id = nMsId;
                    followMsg.estimate_id = EstimateId;
                    followMsg.mess_type_id = FollowUpMsgTypeId;
                    followMsg.sent_by = UserName;
                    followMsg.create_date = DateTime.Now;
                    followMsg.EstimateName = EstimateName;
                    followMsg.NextEmailScheduleDate = DateTime.Now;
                    followMsg.mess_to = To;
                    followMsg.CustomerName = CustomerName;

                    _db.followup_messages.InsertOnSubmit(followMsg);
                    _db.SubmitChanges();
                }


            }



            return IsmailSent;
        }
        catch (Exception ex)
        {           
            throw ex;         
        }
    }


    public static bool SendEmail(MailMessage msg)
    {
        try
        {
            string bcc = ConfigurationManager.AppSettings["DevBCC"];

            if (msg.From.Address == null || msg.From.Address.Length == 0)
                msg.From = new MailAddress("faztrackclient@gmail.com");

            msg.Priority = MailPriority.High;
            if (bcc.Length > 1)
                msg.Bcc.Add(bcc);

            SmtpClient smtp = new SmtpClient();
            smtp.Host = System.Configuration.ConfigurationManager.AppSettings["smtpserver"];
            smtp.Send(msg);


            msg.Dispose();

            return true;
        }
        catch
        {


        }

        return false;
    }

    public csEmailAPI GetMessageDetails(string FromEmail, string MessId, int ProtocolType)
    {

        if (ProtocolType == 1)
        {
            ExchangeService service = EWSAPI.GetEWSService(FromEmail);

            EmailMessage message = EWSAPI.GetEmailDetails(MessId, service);

            foreach (EmailAddress add in message.ToRecipients)
            {
                To += add.Address.ToString() + ", ";
            }
            To = To.TrimEnd(',');

            From = message.From.Address;
            Subject = message.Subject;

            foreach (EmailAddress add in message.CcRecipients)
            {
                CC += add.Address.ToString() + ", ";
            }
            CC = CC.Trim().TrimEnd(',');

            foreach (EmailAddress add in message.BccRecipients)
            {
                BCC += add.Address.ToString() + ", ";
            }

            BCC = BCC.Trim().TrimEnd(',');

            Body = message.Body;


        }
        else if (ProtocolType == 2)
        {


        }
        else
        {





        }


        return this;

    }


}