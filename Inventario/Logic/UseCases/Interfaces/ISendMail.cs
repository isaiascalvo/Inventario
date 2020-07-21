using System;
using System.Collections.Generic;
using System.Text;
using Util.Functions;

namespace Logic.Interfaces
{
    public interface ISendMailUseCases
    {
        Mail.ERetSendMail SendMail(string ToAddress, string Subject, string Content);
    }
}
