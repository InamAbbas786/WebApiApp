using PMS.Core.DbHelper.App.Data;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace PMS.Core.DbHelper.App.SqlCommands
{
    public static class SqlAEFCommands
    {
        public static string SqlQueryAllAEF()
        {
            return string.Format("EXEC [SP_PMS_GETAFEH]");
        }
        public static string SqlQueryAllPO()
        {
            return string.Format("EXEC [dbo].[SP_PMS_GETPOH]");
        }
        public static string SqlQueryDepartment()
        {
            return "SELECT [ID],[COMP_NAME] AS Name FROM [dbo].[RTCOMPANY] WHERE [Status] = 1";
        }

        public static string SqlQueryExchange()
        {
            return "SELECT Cur_Id, Cur_Id+'-'+CONVERT(varchar(20), Rate) AS Name from[dbo].[Exchange]";
        }
        public static string SqlQueryPay()
        {
            return "SELECT PAY_CD, PAY_DESC from[dbo].[INPAYTERMS]";
        }
        public static string SqlQueryShipment()
        {
            return "SELECT Ship_Id, Cust_Id from[dbo].[ShipmentAddress]";
        }
        public static string SqlQueryMatMaster(string MAPCD, string Type)
        {
            if (Type.ToUpper() == "FSD")
                return string.Format("SELECT MAPCD FROM [dbo].[FSDMatMaster] WHERE MAPCD LIKE '%{0}%'", MAPCD);
            else
                return string.Format("SELECT MAPCD FROM [dbo].[MatMaster] WHERE MAPCD LIKE '%{0}%'", MAPCD);
        }

        public static string SqlQueryActiveSupplier(string Supplier)
        {
            return string.Format("Select SId,SId+'_'+Cname Cname from [dbo].[SupHeader] WHERE Active = 1 AND Cname LIKE '%"+ Supplier + "%'");
        }
        public static string SqlQueryDescription(string MAPCD, string Type)
        {
            if (Type.ToUpper() == "FSD")
                return string.Format("SELECT MAPCD, MAPRCD, MAPDS, MABINNO, MAUOM, MA_DEF_UOM FROM[dbo].[FsdMatMaster] WHERE MAPCD = '{0}'", MAPCD);
            else
                return string.Format("SELECT MAPCD, MAPRCD, MAPDS, MABINNO, MAUOM, MA_DEF_UOM FROM[dbo].[MatMaster] WHERE MAPCD = '{0}'", MAPCD);
        }

        public static string SqlQueryGetAFEId(string id)
        {
            return string.Format("Select COUNT(*) from [AprvlForExpenditure] WHere ReqNo='{0}'", id);
        }
        public static string SqlQuerySaveLog(string USER, string FILE, string ACTION, string MACHINE, string KEY, string REMARKS)
        {
            return @"INSERT INTO [RTLOG]([LOG_USER],[LOG_DATE],[LOG_FILE],[LOG_ACTION],[LOG_MACHINE],[LOG_KEY],[LOG_REMARKS]) VALUES ('" + USER + "','" + DateTime.Now + "','" + FILE + "','" + ACTION + "','" + MACHINE + "','" + KEY + "','" + REMARKS + "') Select @@RowCount";            
        }
        public static string SaveUpdateAFEH(int Id, string ReqNo, string Reference, string Crncy, string RqDate, string RequesterId, string FlowCode, string PayTID, string ShipmentId, string ContactPrsn, string Email, string ContactNo, string Remark,
                                            string tAmnt, string ReqType, string Disc, bool Perc, string WONo, bool VPerc, string VValue, string MatClear, string Total, string Freight, string Packing, string MicsCharge, string SubTotal, string NetTotal)
        {
            if (Id <= 0)
            {
                 return @"INSERT INTO [AprvlForExpenditure]([ReqNo],[RtDate],[Reference],[Currency],[RqDate],[RequesterId],[FlowCode],[PayTID],[ShipmentId],[ContactPrsn],[Email],[ContactNo],[Remark],[TotalAmount],[Status], [ReqType],[CanAStatus],[HDisc],
            [HPerc],[WONo],VPerc,VValue,MatClear,Total,Freight,Packing,MicsCharge,SubTotal,NetTotal) VALUES ('" + ReqNo + "','" + DateTime.Now.ToShortDateString() + "','" + Reference + "','" + Crncy + "','" + RqDate + "','" + RequesterId + @"',
            '" + FlowCode + "','" + PayTID + "','" + ShipmentId + "','" + ContactPrsn + "','" + Email + "','" + ContactNo + "','" + Remark + "'," + tAmnt + ",'*','" + ReqType + "','false','" + Disc + "','" + Perc + "','" + WONo + "','" + VPerc + @"',
            '" + VValue + "','" + MatClear + "','" + Total + "','" + Freight + "','" + Packing + "','" + MicsCharge + "','" + SubTotal + "','" + NetTotal + "') Select REQNO FROM [AprvlForExpenditure] WHERE RID = @@Identity ";
            }
            else
            {
              return @"UPDATE [AprvlForExpenditure] SET [Reference] = '" + Reference + "',[Currency] = '" + Crncy + "',[RqDate] = '" + RqDate + "',[FlowCode] = '" + FlowCode + "',[PayTID] = '" + PayTID + "',[ShipmentId] = '" + ShipmentId + @"',
            [ContactPrsn] = '" + ContactPrsn + "',[Email]='" + Email + "',[ContactNo] = '" + ContactNo + "',[Remark] = '" + Remark + "',[ReqType]='" + ReqType + "',[TotalAmount] = '" + tAmnt + @"', [Status]='*',[CanAStatus]='false',
            [HDisc]='" + Disc + "',[HPerc]='" + Perc + "',[WONo]='" + WONo + "',VPerc='" + VPerc + "',VValue='" + VValue + "',MatClear='" + MatClear + "',Total='" + Total + "',Freight='" + Freight + "',Packing='" + Packing + "',MicsCharge='" + MicsCharge + @"',
            SubTotal='" + SubTotal + "',NetTotal='" + NetTotal + "' WHERE [ReqNo]='" + ReqNo + "' Select '"+ ReqNo+"'";
            }
        }
        public static string SupIsRegistered(string SNAME)
        {
            return "Select COUNT(*) FROM  [dbo].[SupHeader] Where Flag='$' AND Active=1 AND [Cname]='" + SNAME + "'";
        }
        public static string GetSuplierExistInAFE(string ReqNo, string Splr)
        {
             return @"Select Count(*) From [AprvlForExpenditureD] Where [ReqNo]='" + ReqNo + "' AND [Suplr]='" + Splr + "'"; 
        }

        public static int TempSupCount(string SNAME,string conection)
        {
            DBHelpers _dBHelpers = new DBHelpers(conection);
            var a = _dBHelpers.Query<int>("Select Count(*) From [dbo].TempSupCounter Where [SID] IN (Select [SID] FROM [dbo].[SupHeader] Where [Cname]='" + SNAME + "')").Data;
            if (a > 0)
            {
                var  b =_dBHelpers.Query<int>("Select [Count] From [dbo].TempSupCounter Where [SID] IN (Select top 1 [SID] FROM [dbo].[SupHeader] Where [Cname]='" + SNAME + "' ORDER BY ID ASC )").Data;
                b = b + 1;
                if (b <= 2)
                {
                    return _dBHelpers.Query<int>("Update [dbo].TempSupCounter Set [Count]=" + b + " Where [SID] IN (Select  top 1 [SID] FROM [dbo].[SupHeader] Where [Cname]='" + SNAME + "' ORDER BY ID ASC)  Select @@RowCount").Data;
                }
                else
                    return 0;
            }
            else
            {
                return _dBHelpers.Query<int>("Insert Into [dbo].TempSupCounter Values (1,(Select top 1 [SID] FROM [dbo].[SupHeader] Where [Cname]='" + SNAME + "' ORDER BY ID ASC))  Select @@IDENTITY").Data;
            }
        }
        public static string AFEIdS()
        {
            return "SELECT Max(REPLACE(ReqNo, 'AFE', '')) From [AprvlForExpenditure]";
        }
        public static string POIdS()
        {
            return "SELECT Max(REPLACE([PONo], 'PO', '')) From [PurchaseOrder] Where [PONo] Not Like '%.R%'";
        }
        
        public static string SaveUpdateAFED(int id, string ReqNo, string CostCd, string Item, string Qunty, string URate, string Disc, string Amnt, string Dates, string Suplr, string UOMsr, string detl, bool Perc)
        {
            if (id > 0)
            {
                return @"UPDATE [AprvlForExpenditureD] SET [CostCode] = " + CostCd + ",[ItemId] = '" + Item + "',[Quantity] = " + Qunty + ",[UnitRate] = " + URate + ",[Disc] = '" + Disc + "',[Amount] =" + Amnt + ",[DelDate] = '" + Dates + @"',
                    [Suplr]='" + Suplr + "',[Descsr]='" + detl + "',[UOMsr]='" + UOMsr + "',[CanStatus]='true',[DPerc]='" + Perc + "' WHERE [ReqNo]='" + ReqNo + "' AND RDID=" + id + "" +
                    "SELECT RDID,ITEMID Item,DESCSR Description,UOMSR UOM,QUANTITY Qty,UNITRATE URate,Disc,DPERC Perc,Amount,SUPLR Supplier,DELDATE SinDate,COSTCODE C_Center FROM [dbo].[APRVLFOREXPENDITURED] " +
                    "WHERE [RDID] = "+ id;
            }
            else
            {
                return @"INSERT INTO [AprvlForExpenditureD]([ReqNo],[CostCode],[ItemId],[Quantity],[UnitRate],[Disc],[Amount],[DelDate],[Suplr],[Descsr],[UOMsr],[CanStatus],[DPerc])
                        VALUES('" + ReqNo + "'," + CostCd + ",'" + Item + "'," + Qunty + "," + URate + "," + Disc + "," + Amnt + ",'" + Dates + "','" + Suplr + "','" + detl + "','" + UOMsr + "','true','" + Perc + "') " +
                        "SELECT RDID,ITEMID Item,DESCSR Description,UOMSR UOM,QUANTITY Qty,UNITRATE URate,Disc,DPERC Perc,Amount,SUPLR Supplier,DELDATE SinDate,COSTCODE C_Center FROM [dbo].[APRVLFOREXPENDITURED] WHERE [RDID] = SCOPE_IDENTITY()";
            }
        }
        public static string  AddNewID(string connection)
        {
            DBHelpers _dBHelpers = new DBHelpers(connection);
            string a = "1";
            string y = DateTime.Now.Year.ToString();
            y = y.Substring(2, y.Length - 2);

            if (_dBHelpers.Query<string>(AFEIdS()).Data != "")
            {
                string yr = _dBHelpers.Query<string>(AFEIdS()).Data;
                string yrL = yr.Substring(0, 2);
                string yrR = _dBHelpers.Query<string>(AFEIdS()).Data.Substring(2, _dBHelpers.Query<string>(AFEIdS()).Data.Length - 2);
                if (Convert.ToInt32(y) == Convert.ToInt32(yrL))
                    a = "" + (Convert.ToInt32(yrR) + 1);
            }

            return y + "AFE" + a.PadLeft(4, '0');
        }

        public static string GetRequireById(string DocId)
        {
             return @"SELECT [RId],[ReqNo],[RtDate],[Reference],e.Cur_Id+'-'+Cast(e.Rate as VARCHAR(10)) 'Currency',[RqDate],[RequesterId],u.UserName,[FlowCode],[PayTID],p.PAY_DESC,[ShipmentId],Cust_Id 'SHTO_DESC',[ContactPrsn],[ContactNo],afe.[Remark],ReqType,
        [Status], Case When ReqType='Stock' Then 'ST' else 'GR' END 'RTYPE',afe.[WONo],afe.Email,[TotalAmount],HPerc,HDisc,Total,Freight,Packing,MicsCharge,SubTotal,VPerc,VValue,NetTotal,MatClear FROM [AprvlForExpenditure] afe Left Join [dbo].rtUser u ON 
        afe.[RequesterId]=u.UserId Left Join [dbo].INPAYTERMS p ON afe.PayTID=p.PAY_CD Left Join [dbo].ShipmentAddress s On afe.ShipmentId=s.Ship_Id LEFT JOIN DBO.Exchange e ON LEFT([Currency],3)=e.Cur_Id AND e.LOC='DXB' Where ReqNo='"+DocId+"'";
           
        }
        public static string GetRequirDetailById(string DocId)
        {
            return @"Select [RDID], ItemId 'Item',Descsr 'Description', UOMsr  'UOM',[CostCode] 'C_Center',[CostCode] 'C_C',[Quantity] 'Qty',[UnitRate] 'URate',Disc,DPerc 'Perc',[Amount],[DelDate] 'SinDate', Suplr 'Supplier' 
            From [AprvlForExpenditureD] afe Where [ReqNo] ='" + DocId + "' AND afe.CanStatus!='False'";
        }

        public static string SqlQueryGetLog(string LogKey)
        {
            return string.Format("SELECT * FROM [dbo].[RTLOG] WHERE  LOG_FILE ='AFE' AND LOG_KEY='{0}'", LogKey);
        }

        public static string SqlQueryGetLogPO(string LogKey)
        {
            return string.Format("SELECT * FROM [dbo].[RTLOG] WHERE  LOG_FILE ='PO' AND LOG_KEY='{0}'", LogKey);
        }
        public static string SqlQueryGetFiles(string DOCNO)
        {
            return string.Format("SELECT DOCNO,FNAME,FPATH,FILETYPE,FILESIZE FROM [dbo].[FILESMANAGEMENT] WHERE DOCNO ='{0}'", DOCNO);
        }
        private static string POIds()
        {
            return "SELECT Max(REPLACE(PONO, 'PO', '')) From [PURCHASEORDER]";
        }
        public static string AddPONewID(string connection)
        {
            DBHelpers _dBHelpers = new DBHelpers(connection);
            string a = "1";
            string y = DateTime.Now.Year.ToString();
            y = y.Substring(2, y.Length - 2);
            var qury = _dBHelpers.Query<string>(POIdS()).Data;
            if (qury != "")
            {
                string yr = qury;
                string yrL = yr.Substring(0, 2);
               
                string yrR = qury.Substring(2, qury.Length - 2);
                if (Convert.ToInt32(y) == Convert.ToInt32(yrL))
                    a = "" + (Convert.ToInt32(yrR) + 1);
            }

            return y + "PO" + a.PadLeft(4, '0');
        }
        public static string  POSaveUpdateD(int id, string LNo, string PONO, string CostCd, string Item, string UOM, string Qty, string URate, string Amnt, string Dates, string Desc, string Disc, string Perc, string NUR, string EXNUR)
        {
            int save = 0;
            //if (id != "" && id != null)
            //{
            //    if (PONO.Contains(".R"))
            //    {
            //       var isExist= _dBHelpers.Query<int>("Select Count(*) From PurchaseOrderD  WHERE PLNO='" + LNo + "' AND [PONo] = '" + PONO + "'").Data;
            //        if (isExist > 0)
            //        {

            //            save= _dBHelpers.Query<int>(@"UPDATE [PurchaseOrderD] SET [PONo] = '" + PONO + "',[C_C] = '" + CostCd + "',[Item] = '" + Item + "',[UOM] = '" + UOM + "',PLNO='" + LNo + "',[QTY] = '" + Qty + "',[Rate] = '" + URate + @"',
            //        [Amount] = '" + Amnt + "',[ETA] = '" + Dates + "',[Descr]='" + Desc + "',DPDisc='" + Disc + "',DPPerc='" + Perc + "',NUR='" + NUR + "',EXNUR='" + EXNUR + "' WHERE PLNO='" + LNo + "' AND [PONo] = '" + PONO + "' Select @@RowCount").Data;
                      
            //        }
            //        else
            //        {
            //            save = _dBHelpers.Query<int>(@"INSERT INTO [PurchaseOrderD]([PONo],[C_C],[Item],[UOM],[QTY],[Rate],[Amount],[ETA],[Descr],DPDisc,DPPerc,[PLNO],[NUR],EXNUR) VALUES  ('" + PONO + "','" + CostCd + "','" + Item + "','" + UOM + @"',
            //        '" + Qty + "','" + URate + "','" + Amnt + "','" + Dates + "','" + Desc + "','" + Disc + "','" + Perc + "','" + LNo + "','" + NUR + "','" + EXNUR + "') Select @@RowCount").Data;
            //        }
            //    }
            //    else
            //    {
            //        save = _dBHelpers.Query<int>(@"UPDATE [PurchaseOrderD] SET [PONo] = '" + PONO + "',[C_C] = '" + CostCd + "',[Item] = '" + Item + "',[UOM] = '" + UOM + "',PLNO='" + LNo + "',EXNUR='" + EXNUR + "',[QTY] = '" + Qty + @"',
            //    [Rate] = '" + URate + "',[Amount] = '" + Amnt + "',[ETA] = '" + Dates + "',[Descr]='" + Desc + "',DPDisc='" + Disc + "',DPPerc='" + Perc + "',NUR='" + NUR + "' WHERE Id='" + id + "' Select @@RowCount").Data;
                  
            //    }
            //}
            //else
            //{
            //    var isExist = _dBHelpers.Query<int>("Select Count(*) From PurchaseOrderD  WHERE PLNO='" + LNo + "' AND [PONo] = '" + PONO + "'").Data;
                if (id > 0)
                {
                   return @"UPDATE [PurchaseOrderD] SET [PONo] = '" + PONO + "',[C_C] = '" + CostCd + "',[Item] = '" + Item + "',[UOM] = '" + UOM + "',PLNO='" + LNo + "',EXNUR='" + EXNUR + "',[QTY] = '" + Qty + @"',
                [Rate] = '" + URate + "',[Amount] = '" + Amnt + "',[ETA] = '" + Dates + "',[Descr]='" + Desc + "',DPDisc='" + Disc + "',DPPerc='" + Perc + "',NUR='" + NUR + "' WHERE PLNO='" + LNo + "' AND [PONo] = '" + PONO + "' Select @@RowCount";
                }
                else
                {
                   return @"INSERT INTO [PurchaseOrderD]([PONo],[C_C],[Item],[UOM],[QTY],[Rate],[Amount],[ETA],[Descr],DPDisc,DPPerc,[PLNO],[NUR],EXNUR) VALUES('" + PONO + "','" + CostCd + "','" + Item + "','" + UOM + "','" + Qty + @"',
                '" + URate + "','" + Amnt + "','" + Dates + "','" + Desc + "','" + Disc + "','" + Perc + "','" + LNo + "','" + NUR + "','" + EXNUR + "') Select @@RowCount";
                }
           // }
        }


        public static string POSaveNdUpdate(int Id, string PONo, string Date, string ShipTerm, string DelMode, string ReqNo, string SupName, string Remark, string DelDate, string SupMail, string SupCPrsn,
            string shipto, string pyterm, string Flg,
             string PDisc, bool PPerc, bool POVAT, string POVATVal, string GTotal, string DTotal, string NTotal, string Freight, string Packing, string MicsCharge, string SubTotal, string Cur, string Exch,string wono)
        {
            if (Id > 0)
            {
              return @"UPDATE [PurchaseOrder] SET [Date] = '" + Date + "',[ShipTerm] = '" + ShipTerm + "',[DelMode] = '" + DelMode + "',[ReqNo] = '" + ReqNo + "',[SupName] = '" + SupName + "',[PDisc]='" + PDisc + @"',
            [PPerc]='" + PPerc + "',[Remark] = '" + Remark + "',DelDate='" + DelDate + "',SupMail='" + SupMail + "',SupCPrsn='" + SupCPrsn + "',[POStatus]='true',[CanPStatus]='false',ShipTo='" + shipto + "',PyTerm='" + pyterm + @"',
            Revise='0',Flag='" + Flg + "',[POVAT]='" + POVAT + "',[POVATVal]='" + POVATVal + "',[GTotal]='" + GTotal + "',[DTotal]='" + DTotal + "',[NTotal]='" + NTotal + "',Freight='" + Freight + "',Packing='" + Packing + @"',
            MicsCharge='" + MicsCharge + "',SubTotal='" + SubTotal + "',Cur='" + Cur + "',Exch='" + Exch + "'  WHERE [PONo]='" + PONo + "' Select @@RowCount";
            }
            else
            {
                return @"INSERT INTO [dbo].[PURCHASEORDER]([PONO],[DATE],[SHIPTERM],[DELMODE],[REQNO],[SUPNAME],[REMARK],[POSTATUS],[DELDATE],[CANPSTATUS],[SUPMAIL],[SUPCPRSN],[SHIPTO],[PYTERM],[REVISE],[FLAG],[PDISC],[PPERC],[WONO],[COMENT],[POVAT],[POVATVAL],[GTOTAL],[DTOTAL],[NTOTAL],[FREIGHT],[PACKING],[MICSCHARGE],[SUBTOTAL],[EXCH],[CUR])
                         VALUES('" + PONo + "','" + Date + "','" + ShipTerm + "','" + DelMode + "','" + ReqNo + "','" + SupName + "','" + Remark + "',0,'" + DelDate + "',0,'" + SupMail + "','" + SupCPrsn + "','" + shipto + "','" + pyterm + "',0,'" + Flg + "'," + PDisc + ",0,'" + wono + "','" + Remark + "','" + POVAT + "'," + POVATVal + "," + GTotal + "," + DTotal + "," + NTotal + "," + Freight + "," + Packing + "," + MicsCharge + "," + SubTotal + "," + Exch + ",'" + Cur + "') Select @@Identity";
            }
        }

        public static string GetAppDMSByDiv(string Div, string Page, string Lvl)
        {
            return "Select [Division],[Page],[User],[Email],[Level],[Limit],[Remark],[Flag] From [AppFlowDMS] Where [Division]='" + Div + "' AND [Page] ='" + Page + "' AND [Level] = '" + Lvl + "'";          
        }

        public static string SubmitAFEPost(string RID, string sts)
        {
          return "update [AprvlForExpenditure] Set [Status]='" + sts + "' Where [ReqNo]='" + RID + "' Select @@RowCount";
        }

        public static string GetUserInfoByUId(string UserId)
        {
             return "Select UserMail From rtUser Where UserId='" + UserId + "'";
        }

        public static string SendMailForReEvaluation(string MailTo, string Sub, string Msg)
        {
            string from = "noreplysigsme@gmail.com", pwd = "7&c7DBr@Wc", server = "smtp.gmail.com", noreply = "no-reply@sigsme.com";
            int port = 587;
            SmtpClient smtpClient = new SmtpClient(server, port);
            smtpClient.Credentials = new System.Net.NetworkCredential(from, pwd);
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(from, noreply);
            mailMessage.To.Add(MailTo);
            mailMessage.Subject = Sub;
            mailMessage.Body = Msg;
            mailMessage.Priority = MailPriority.High;
            string a = "";
            try
            {
                smtpClient.Send(mailMessage);
                a = "Succefull";
            }
            catch (Exception ex)
            {
                a = "Unsucce" + ex;
            }
            finally
            {
                a = "Succefull";
            }
            return a;
        }
        public static int MsgInboxReadedByUserAFE(string TTCD, string User,string connection)
        {
            DBHelpers _dBHelpers = new DBHelpers(connection);
            var Level = _dBHelpers.Query<int>("Select TTD_Level  from [UserInbox] Where TransId='" + TTCD + "' AND [UserId]='" + User + "'").Data;

            if (Level != 0)
            {
                var update=_dBHelpers.Query<int>("Update [UserInbox] Set Flag='False' Where TransId='" + TTCD + "' AND [TTD_Level]='" + Level + "' Select @@Rowcount").Data;
                return update;
            }
            else
                return 0;
        }

        public static string SaveUserInbox(string TTD_CD, string PageId, string Level, string UserId, string Work, string Trans, string Url, bool flg)
        {
            return @"INSERT INTO [dbo].[UserInbox]([TTD_CD],[PageId],[TTD_Level],[UserId],[Work],[DateTime],[TransId],[Adres],[Flag])
            VALUES ('" + TTD_CD + "','" + PageId + "','" + Level + "','" + UserId + "','" + Work + "','" + DateTime.Now + "','" + Trans + "','" + Url + "','" + flg + "') Select @@Identity";
        }

        public static string SqlDelModelQuery()
        {
            return "SELECT DEL_CD,DEL_DESC FROM [dbo].[INDELMOD]";
        }

        public static string GetMailInbox(string Usr)
        {
            return @"Select Count(*) from UserInbox ui Left Join TransactionAndPg tp ON ui.PageId=tp.Code Where Tab='prc' AND Flag=1 AND UserId='" + Usr + "'";
        }
        public static string GetAppDMSByUser(string User, string Page, string div)
        {
            return "Select [Division],[Page],[User],[Email],[Level],[Limit],[Remark],[Flag] From [AppFlowDMS] Where [User]='" + User + "' AND [Page] ='" + Page + "' AND [Division] Like '" + div + "'";

        }

        public static string ShipmentCleanace(string DivId)
        {
            return @"SELECT UserMail,UserId,UserEmpID FROM [dbo].RTUSER WHERE UserActive=1 AND UserEmpID IN (SELECT EmpId FROM [SIGS].[dbo].Configure_AddEmployeeDetails WHERE Desig =139 AND DivId='" + DivId + "' AND DivId IN (1,2,3,12,14))";
        }

        //public static string SaveUserInbox(string TTD_CD, string PageId, string Level, string UserId, string Work, string Trans, string Url, bool flg)
        //{
        //   return @"INSERT INTO [SIGS].[dbo].[UserInbox]([TTD_CD],[PageId],[TTD_Level],[UserId],[Work],[DateTime],[TransId],[Adres],[Flag])
        //    VALUES ('" + TTD_CD + "','" + PageId + "','" + Level + "','" + UserId + "','" + Work + "','" + DateTime.Now + "','" + Trans + "','" + Url + "','" + flg + "') Select @@Identity";
        //}

        public static string PODeactive(string POno)
        {
            return @"Update PurchaseOrder Set Revise='1' Where PONo='" + POno + "' Select @@ROWCOUNT";
        }

        public static string GetUserSubmit(string DocNo)
        {
            return  "Select Log_User from RTLog Where LOG_ACTION='Document submited' AND LOG_KEY='" + DocNo + "'";
        }
        public static string SubmitPOPost(string POID, string sts)
        {
           return "Update [PurchaseOrder] Set [Flag]='" + sts + "' Where [PONo]='" + POID + "' Select @@RowCount";
        }
        public static string GetAFEEMailByRNO(string RNO)
        {
           return "Select Email From AprvlForExpenditure Where ReqNo='" + RNO + "'";
        }

        public static string GetSupplier(string reqno)
        {
            return "SELECT distinct SId,Cname  FROM [dbo].[APRVLFOREXPENDITURED] d left join SupHeader s on " +
                "(SELECT TOP 1 * FROM dbo.[fnSplitString](D.SUPLR,'_')) = s.SId where REQNO = '"+ reqno + "'";
            
        }
        public static string GetPOById(string Id)
        {
             return @"SELECT TOP 1 p.*,s.Cust_Id ShipToN,ip.PAY_DESC PyTermN,af.FlowCode,PDisc,PPerc,af.CONTACTNO,af.EMAIL,af.SHIPMENTID FROM [PurchaseOrder] p Left Join dbo.ShipmentAddress s ON p.ShipTo=s.Ship_Id
        Left Join dbo.INPAYTERMS ip ON p.PyTerm=ip.PAY_CD Left Join AprvlForExpenditure af ON p.ReqNo=af.ReqNo WHERE PONo='" + Id + "' AND Revise=0";
           
        }
        public static string GetPODByPONo(string PONO)
        {
         return @"Select ID,PLNO 'LNo',[PONo],[C_C] Analysis_Code,0 C_CName,[Item],[UOM],[QTY],'' CQty,[Rate] 'URATE',[Amount],[ETA] 'ETA_SIGS',Descr 'Description',DPDisc 'Disc',DPPerc '%'
        From [PurchaseOrderD] d   WHERE [PONo] = '" + PONO + "' Order By PLNO ASC";

        }
        public static string SaveComment(string ReqNo,string Comment,string connection)
        {
            DBHelpers _dBHelpers = new DBHelpers(connection);
            var result = _dBHelpers.Query<int>("SELECT COUNT(*) from ComentOnEnqFromQC  WHERE EnqNo='" + ReqNo + "'").Data;
            if (result > 0)
                return "UPDATE ComentOnEnqFromQC SET Coment ='"+ Comment + "' WHERE EnqNo = '"+ ReqNo + "'";
            else
                return "INSERT INTO ComentOnEnqFromQC ([PCode],[EnqNo],[Coment]) VALUES ('0','" + ReqNo + "','" + Comment + "')";
        }

        //public static string  GetRequireById(string Id)
        //{
        //    return = @"SELECT [RId],[ReqNo],[RtDate],[Reference],e.Cur_Id+'-'+Cast(e.Rate as VARCHAR(10)) 'Currency',[RqDate],[RequesterId],u.UserName,[FlowCode],[PayTID],p.PAY_DESC,[ShipmentId],Cust_Id 'SHTO_DESC',[ContactPrsn],[ContactNo],afe.[Remark],ReqType,
        //[Status], Case When ReqType='Stock' Then 'ST' else 'GR' END 'RTYPE',afe.[WONo],afe.Email,[TotalAmount],HPerc,HDisc,Total,Freight,Packing,MicsCharge,SubTotal,VPerc,VValue,NetTotal,MatClear FROM [AprvlForExpenditure] afe Left Join rtUser u ON 
        //afe.[RequesterId]=u.UserId Left Join [dbo].INPAYTERMS p ON afe.PayTID=p.PAY_CD Left Join [dbo].ShipmentAddress s On afe.ShipmentId=s.Ship_Id LEFT JOIN DBO.Exchange e ON LEFT([Currency],3)=e.Cur_Id AND e.LOC='DXB' Where ReqNo='" + Id + "'";

        //}

    }
}
