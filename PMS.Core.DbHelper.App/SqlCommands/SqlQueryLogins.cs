using System;
using System.Collections.Generic;
using System.Text;

namespace PMS.Core.DbHelper.App.SqlCommands
{
   public static class SqlQueryLogins
    {
        //Queries
        //Count captchae to check seleted captchae, is correct?
        public static string GetCorrectCaptcha(string Id, string Name)
        {
           return "SELECT COUNT(*) FROM [Captchae] WHERE Id='" + Id + "' AND Name='" + Name + "'";
        }
        //Get captchae name for selection from list of five captchae picture
        public static string GetCaptchaName(List<string> Id)
        {
            Random rnd = new Random();
            int r = rnd.Next(Id.Count);
            return  "SELECT TOP(1) Name FROM [Captchae] WHERE Id IN ('" + Id[r].ToString() + "')  ORDER BY NEWID()";

        }
        // Get captchae list random 5
        public static string GetCaptchaList()
        {
           return "SELECT TOP(5) * FROM [Captchae] ORDER BY NEWID()";
        }

        //

        public static string UID(string UName, string UPwd)
        {
           return @"Select  UserMail,UserPwd,UserSign,UserEmpID,UserId,UserName From [dbo].[rtUser] where UserActivE= 1 AND USERID='ikram'  AND UserMail ='" + UName + "' and UserPwd='"+ UPwd + "'";
        }

        public static string UID(string UName)
        {
            return @"Select UserPwd From [dbo].[rtUser] Where UserMail='" + UName + "' AND USERID='ikram'";
        }

        public static string GetUID()
        {
             return @"Select UserId,UserName From [dbo].[rtUser] Where UserActive=1";
        }
    }
}
