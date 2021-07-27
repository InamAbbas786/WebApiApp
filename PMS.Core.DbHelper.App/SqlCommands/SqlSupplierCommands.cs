using System;
using System.Collections.Generic;
using System.Text;

namespace PMS.Core.DbHelper.App.SqlCommands
{
    public static class SqlSupplierCommands
    {
        public static string SqlQueryHistory(string SId)
        {
            return string.Format("SELECT R.LOG_USER,R.LOG_DATE,R.LOG_ACTION,LOG_MACHINE,UI.UserId,UI.Work,UI.Adres,UI.DateTime AS UIDate,UI.TTD_Level from [dbo].[RTLOG] R LEFT JOIN UserInbox UI ON R.LOG_KEY = UI.TransId  WHERE LOG_KEY ='{0}' AND LOG_ACTION NOT IN ('Save','Update')", SId);
        }

        public static string SqlQuerySaveSupplier()
        {
            return string.Format(@"EXEC [dbo].[SaveSupplier] @Id,@SId ,@Cname,@SubDate,@Address,@City,@Country,@Phone,@Mobile,@Fax ,@Web,@Email,@ContactBy,@Position,@Password,@Active,@TDate,@CDate,@REDate,
                                                             @SupType,@Remark,@Cat1,@Cat2,@Cat3,@Cat4,@Cat5,@Cat6,@Email2,@LicenseNo,@PlaceLns,@Local,@Activity 
                                                             ,@PCode,@POBox,@Comp1,@Comp2,@Comp3,@Comp4,@Loc1,@Loc2,@Loc3,@Loc4,@OtherOffices,@Flag,@PAY_TERM
                                                             ,@QName,@QEMail,@QMobile,@QMSIMP,@QMSCERT,@QMSSTD1,@HSESIMP,@HSECERT,@HSESTD1,@HSESTD2,@HSESTD3,@QMSSTD2,@QMSSTD3,@TRN");
        }

        public static string SqlQueryGetTemFiles()
        {
            return string.Format(@"EXEC GetTempFiles");
        }

        public static string Savefiles()
        {
            return string.Format("EXEC [dbo].[SaveFiles] @DOCNO,@FNAME,@FPATH,@FILETYPE,@FILESIZE,@Supplier");
        }    
        public static string SaveAddress()
        {
            return string.Format("EXEC SaveSupplierAddress @SupId ,@FName,@LName ,@Position,@POBox,@Mobile,@Email");
        }

        public static string SqlQueryUpdateSupplier()
        {
            return string.Format(@"EXEC [dbo].[SaveSupplier] @Id,@SId,@Cname,@SubDate,@Address,@City,@Country,@Phone,@Mobile,@Fax ,@Web,@Email,@ContactBy,@Position,@Password,@Active,@TDate,@CDate,@REDate,
                                                             @SupType,@Remark,@Cat1,@Cat2,@Cat3,@Cat4,@Cat5,@Cat6,@Email2,@LicenseNo,@PlaceLns,@Local,@Activity 
                                                             ,@PCode,@POBox,@Comp1,@Comp2,@Comp3,@Comp4,@Loc1,@Loc2,@Loc3,@Loc4,@OtherOffices,@Flag,@PAY_TERM
                                                             ,@QName,@QEMail,@QMobile,@QMSIMP,@QMSCERT,@QMSSTD1,@HSESIMP,@HSECERT,@HSESTD1,@HSESTD2,@HSESTD3,@QMSSTD2,@QMSSTD3,@TRN");
        }


    }                                                         
}                                                          
                                                              
                                                             
                                                              