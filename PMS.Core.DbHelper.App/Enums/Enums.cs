using System;
using System.Collections.Generic;
using System.Text;

namespace PMS.Core.DbHelper.App.Enums
{
    public enum ResultStatus
    {
        Success = 100,
        Error = 200,
        NotFound = 300,
        Warning = 400,
        InProcess = 500,
        AlreadyExist = 600
    };
}
