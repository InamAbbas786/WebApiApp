using System;
using System.Collections.Generic;
using System.Text;

namespace PMS.Core.DbHelper.App.ViewModels
{
    public class UserView
    {
        public int ID { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public bool? UserActive { get; set; }
        public string UserMail { get; set; }
        public string UserEmpID { get; set; }
        public string UserSign { get; set; }
        public decimal? Mobile { get; set; }
        public decimal? Phone { get; set; }
        public string LocId { get; set; }
        public DateTime? ValidFr { get; set; }
        public DateTime? ValidTo { get; set; }
        public string Path { get; set; }
    }
    public class UserViewLogin
    {
        public UserViewLogin()
        {
            captcha = new ViewModelCaptcha();
            FasicalYears = new List<string>();
        }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public bool UserActive { get; set; }
        public ViewModelCaptcha captcha { get; set; }
        public int FasicalYear { get; set; }
        public List<string> FasicalYears { get; set; }
    }

    public class UserLogin
    {
        public string UserName { get; set; }
        public string UserPwd { get; set; }
    }
    public class Captcha
    {
        public int Id { get; set; }
        public string  CPath { get; set; }
        public string Name { get; set; }

    }

    public class CaptchaName
    {
        public string Name { get; set; }
    }

    public class ViewModelCaptcha
    {

        public ViewModelCaptcha()
        {
            captcha = new List<Captcha>();
        }
        public List<Captcha> captcha { get; set; }
        public string lblNameC { get; set; }
    }

    public class UserSessionView
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public string UserMail { get; set; }
        public string UserEmpID { get; set; }
        public int FasicalYear { get; set; }
    }
}
