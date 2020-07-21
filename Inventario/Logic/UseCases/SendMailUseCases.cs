using Logic.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Util.Functions;

namespace Logic
{
    public class SendMailUseCases: ISendMailUseCases
    {
        private IConfiguration _configuration;
        public SendMailUseCases(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public Mail.ERetSendMail SendMail(string ToAddress, string Subject, string Content)
        {
            string wMensajeErr = string.Empty;
            Mail wMail = new Mail(
                _configuration["Mail_Server"],
                int.Parse(_configuration["Mail_Port"]),
                _configuration["Mail_Username"],
                _configuration["Mail_Password"],
                _configuration["Mail_Username"],
                bool.Parse(_configuration["Mail_EnableSSL"])
            );
            return wMail.SendMail(ToAddress, Subject, Content, wMensajeErr);
        }
    }
}
