using System;
using System.Collections.Generic;
using System.Text;

namespace Util.Auth
{
    public interface IAuthService
    {
        string GetCurrentUserId();
    }
}
