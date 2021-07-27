using PMS.Core.DbHelper.App.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMS.Core.DbHelper.App.Generics
{
    public struct Result
    {
        public ResultStatus Status { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
}
