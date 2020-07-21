using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Util.Functions
{
    public class Mail
    {
        public enum ERetSendMail
        {
            Ok = 0,
            SMTPServerNotSet = 1,
            SMTPPortNotSet = 2,
            SMTPUserNameNotSet = 3,
            SMTPPasswordNotSet = 4,
            FromAddressNotSet = 5,
            KO = 99
        }

        private string mServer = "";
        private int mPort = 25;
        private string mUserName = string.Empty;
        private string mPassword = string.Empty;
        private string mFromAddress = string.Empty;
        private bool mIsBodyHtml = false;
        private List<Attachment> mAttachments = new List<Attachment>();
        private string mToAddress = string.Empty;

        private string mOggetto = string.Empty;
        private string mContent = string.Empty;
        private bool mEnableSsl = true;
        private bool mAsync = false;


        #region "Property"
        public string Server { get => mServer; set => mServer = value; }
        public int Port { get => mPort; set => mPort = value; }
        public string UserName { get => mUserName; set => mUserName = value; }
        public string Password { get => mPassword; set => mPassword = value; }
        public string FromAddress { get => mFromAddress; set => mFromAddress = value; }
        public bool IsBodyHtml { get => mIsBodyHtml; set => mIsBodyHtml = value; }
        public List<Attachment> Attachments { get => mAttachments; set => mAttachments = value; }
        public string ToAddress { get => mToAddress; set => mToAddress = value; }
        public string Oggetto { get => mOggetto; set => mOggetto = value; }
        public string Content { get => mContent; set => mContent = value; }
        #endregion

        public Mail() { }

        public Mail(string Server, int Port, string FromAddress)
        {
            mServer = Server;
            mPort = Port;
            mFromAddress = FromAddress;
        }

        public Mail(string Server, int Port, string UserName, string Password, string FromAddress)
        {
            mServer = Server;
            mPort = Port;
            mUserName = UserName;
            mPassword = Password;
            mFromAddress = FromAddress;
        }

        public Mail(string Server, int Port, string UserName, string Password, string FromAddress, bool EnableSsl)
        {
            mServer = Server;
            mPort = Port;
            mUserName = UserName;
            mPassword = Password;
            mFromAddress = FromAddress;
            mEnableSsl = EnableSsl;
        }

        public Mail(string Server, int Port, string UserName, string Password, string FromAddress, bool EnableSsl, bool Async)
        {
            mServer = Server;
            mPort = Port;
            mUserName = UserName;
            mPassword = Password;
            mFromAddress = FromAddress;
            mEnableSsl = EnableSsl;
            mAsync = Async;
        }

        public ERetSendMail SendMail()
        {
            return SendMail(ToAddress, Oggetto, Content);
        }

        public ERetSendMail SendMail(string MessaggeErr)
        {
            return SendMail(ToAddress, Oggetto, Content, MessaggeErr);
        }

        public ERetSendMail SendMail(string ToAddress, string Subject, string Content)
        {
            return SendMail(ToAddress, Subject, Content, "");
        }

        public ERetSendMail SendMail(string ToAddress, string Subject, string Content, string MessageErr)
        {
            if (string.IsNullOrEmpty(mServer))
            {
                return ERetSendMail.SMTPServerNotSet;
            }

            if (string.IsNullOrEmpty(mFromAddress))
            {
                mFromAddress = mUserName;
            }

            if (string.IsNullOrEmpty(ToAddress))
            {
                throw new ArgumentNullException("ToAddress", "Smouse.Language.My.Resources.ResourceLanguage.Param_Addres_NoValue");
            }

            if (string.IsNullOrEmpty(Subject))
            {
                throw new ArgumentNullException("Subject", "Smouse.Language.My.Resources.ResourceLanguage.Param_Subject_NoValue");
            }

            if (string.IsNullOrEmpty(Content))
            {
                throw new ArgumentNullException("Content", "Smouse.Language.My.Resources.ResourceLanguage.Param_Content_NoValue");
            }

            System.Net.NetworkCredential wAuthInfo;
            System.Net.Mail.MailAddress wFrom;

            try
            {
                wFrom = new System.Net.Mail.MailAddress(mFromAddress);
            }

            catch (Exception e)
            {
                MessageErr = "Smouse.Language.My.Resources.ResourceLanguage.Address_Error" + mFromAddress + Environment.NewLine + e.Message;
                return ERetSendMail.KO;
            }


            using (var wMail = new System.Net.Mail.MailMessage())
            {
                wMail.From = wFrom;
                wMail.Subject = Subject;
                wMail.Body = Content;
                wMail.IsBodyHtml = IsBodyHtml;
                wMail.BodyEncoding = Encoding.UTF8;
                wMail.HeadersEncoding = Encoding.UTF8;
                wMail.SubjectEncoding = Encoding.UTF8;
                foreach (var att in Attachments)
                {
                    wMail.Attachments.Add(att);
                }

                foreach (var address in ToAddress.Split(";"))
                {
                    if (string.IsNullOrEmpty(address)) continue;
                    wMail.To.Add(address.Trim());
                }

                System.Net.Mail.SmtpClient wSmtp = null;
                try
                {
                    wSmtp = new System.Net.Mail.SmtpClient(mServer, mPort);

                    if (!string.IsNullOrEmpty(mUserName))
                    {
                        wAuthInfo = new System.Net.NetworkCredential(mUserName, mPassword);
                        wSmtp.UseDefaultCredentials = false;
                        wSmtp.Credentials = wAuthInfo;
                        wSmtp.EnableSsl = mEnableSsl;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(mPassword))
                        {

                            wSmtp.UseDefaultCredentials = false;
                            wSmtp.EnableSsl = mEnableSsl;
                        }
                        else
                        {
                            wSmtp.UseDefaultCredentials = true;
                            wSmtp.EnableSsl = mEnableSsl;
                        }

                    }

                    if (mAsync)
                    {
                        wSmtp.SendAsync(wMail, null);
                    }
                    else
                    {
                        wSmtp.Send(wMail);
                    }
                }
                catch (Exception e)
                {
                    MessageErr = e.Message;//Smouse.Common.Traccia.ExToString(e);
                    return ERetSendMail.KO;
                }
                finally
                {
                    //if (VS = 2010)
                    //{
                    //    wSmtp.Dispose();
                    //}
                }

                return ERetSendMail.Ok;
            }
        }        
    }
}
