using Microsoft.AspNetCore.Mvc;
using PMS.Core.DbHelper.App.Data;
using PMS.Core.DbHelper.App.Generics;
using PMS.Core.DbHelper.App.SqlCommands;
using PMS.Core.DbHelper.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PMS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly DBHelpers _dBHelpers;

        public LoginController()
        {
            string con = ConnectionStrings.DataConnectionString;
            _dBHelpers = new DBHelpers(con);
        }

        [HttpGet]
        [Route("GetLogins")]
        public Result GetLogins()
        {
            UserViewLogin model = new UserViewLogin();
            model.FasicalYears = _dBHelpers.QueryList<string>("SELECT [YR_CD] FROM [dbo].[rtfyear]").Data;
            model.FasicalYears.Insert(0, "Select");
            model.captcha = CaptchaRefresh();
            model.UserActive = false;
            model.UserName = null;
            model.UserPwd = null;
            return new Result()
            {
                Status = Core.DbHelper.App.Enums.ResultStatus.Success,
                Data = model,
                Message = ""
            };
        }

        [HttpPost]
        [Route("RefreshCapcha")]
        public Result RefreshCapcha()
        {
            UserViewLogin model = new UserViewLogin();
            model.captcha = CaptchaRefresh();
            return new Result()
            {
                Data = model,
                Message = "",
                Status = Core.DbHelper.App.Enums.ResultStatus.Success
            };
        }
        [HttpPost]
        [Route("LoginUsers")]
        public Result LoginUsers(UserLogin model)
        {
            Result result = new Result();
            //  string Msg = "";
            if (!string.IsNullOrEmpty(model.UserName.Trim()) && !string.IsNullOrEmpty(model.UserPwd.Trim()))
            {
                if (model.UserName.Contains("'") || model.UserPwd.Contains("'"))
                {
                    result.Message = "YOU ARE NOT AUTHORIZE TO TYPE (') ...";
                    result.Status = PMS.Core.DbHelper.App.Enums.ResultStatus.NotFound;
                    result.Data = "";
                    return result;
                }
                else
                {
                    if (model.UserName.Contains("@"))
                    {
                        var userquery = SqlQueryLogins.UID(model.UserName);
                        var userinfo = SqlQueryLogins.UID(model.UserName, Encrypt(model.UserPwd, true));
                        var user = _dBHelpers.Query<string>(userquery).Data;
                        if (user != null)
                        {
                            UserSessionView dt = _dBHelpers.Query<UserSessionView>(userinfo).Data;
                            dt.FasicalYear =DateTime.Now.Year;
                            result.Data = dt;
                            result.Message = "";
                            result.Status = PMS.Core.DbHelper.App.Enums.ResultStatus.Success;
                        }
                        else
                        {
                            result.Message = "INVALID USER ID OR PASSWORD...";
                            result.Status = PMS.Core.DbHelper.App.Enums.ResultStatus.Warning;
                            result.Data = "";
                            return result;
                        }
                    }
                    else
                    {
                        result.Message = "VALID USER ID ...";
                        result.Status = PMS.Core.DbHelper.App.Enums.ResultStatus.Warning;
                        result.Data = "";
                    }
                    return result;
                }
            }
            else
            {
                result.Message = "EMAIL AND PASSWORD NOT ENTERED...";
                result.Status = PMS.Core.DbHelper.App.Enums.ResultStatus.Warning;
                result.Data = "";
            }
            return result;
        }
        private string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            System.Configuration.AppSettingsReader settingsReader = new System.Configuration.AppSettingsReader();
            string key = ConnectionStrings.SecurityKey;
            if (useHashing)
            {
                System.Security.Cryptography.MD5CryptoServiceProvider hashmd5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            System.Security.Cryptography.TripleDESCryptoServiceProvider tdes = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = System.Security.Cryptography.CipherMode.ECB;
            tdes.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            System.Security.Cryptography.ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        private string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);
            System.Configuration.AppSettingsReader settingsReader = new System.Configuration.AppSettingsReader();
            string key = ConnectionStrings.SecurityKey;
            if (useHashing)
            {
                System.Security.Cryptography.MD5CryptoServiceProvider hashmd5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }
            System.Security.Cryptography.TripleDESCryptoServiceProvider tdes = new System.Security.Cryptography.TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = System.Security.Cryptography.CipherMode.ECB;
            tdes.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            System.Security.Cryptography.ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        private ViewModelCaptcha CaptchaRefresh()
        {
            var result = _dBHelpers.QueryList<Captcha>(SqlQueryLogins.GetCaptchaList());

            ViewModelCaptcha captcha = new ViewModelCaptcha();
            List<Captcha> CapList = result.Data;
            if (CapList.Count > 0)
            {
                List<string> Item = new List<string>();
                foreach (var item in CapList)
                {
                    Item.Add(item.Id.ToString());
                }

                var CaptchaName = _dBHelpers.Query<CaptchaName>(SqlQueryLogins.GetCaptchaName(Item));

                captcha.captcha = CapList;
                captcha.lblNameC = CaptchaName.Data.Name;
            }
            return captcha;
        }
    }
}