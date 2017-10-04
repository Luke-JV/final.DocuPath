using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DocuPath.Models
{
    public static class AuditModel
    {
        public static void WriteTransaction(int uID, string type, string context)
        {
            using (DocuPathEntities db = new DocuPathEntities())
            {

                AUDIT_LOG transaction = new AUDIT_LOG();
                transaction.TxDateStamp = DateTime.Now;
                transaction.TxTimeStamp = DateTime.Now;
                transaction.UserID = uID;
                transaction.USER = db.USER.Where(x => x.UserID == uID).FirstOrDefault();
                transaction.AUDIT_TX_TYPE = db.AUDIT_TX_TYPE.Where(x => x.TypeValue == type).FirstOrDefault();
                transaction.AuditLogTxTypeID = transaction.AUDIT_TX_TYPE.AuditLogTxTypeID;
                transaction.TxCriticalDataString = transaction.USER.USER_LOGIN.ACCESS_LEVEL.LevelName+": "+transaction.USER.FirstName +" "+ transaction.USER.LastName+" performed a(n) "+transaction.AUDIT_TX_TYPE.TypeValue+" ("+context+") transaction.";
                try
                {
                    transaction.AuditLogTxID = db.AUDIT_LOG.Max(u => u.AuditLogTxID) + 1;
                }
                catch (Exception)
                {
                    transaction.AuditLogTxID = 0;
                }

                db.AUDIT_LOG.Add(transaction);
                db.SaveChanges();
            }
        }
        public static void WriteTransaction(int uID, string type, string context, object json)
        {
            using (DocuPathEntities db = new DocuPathEntities())
            {

                AUDIT_LOG transaction = new AUDIT_LOG();
                transaction.TxDateStamp = DateTime.Now;
                transaction.TxTimeStamp = DateTime.Now;
                transaction.UserID = uID;
                transaction.USER = db.USER.Where(x => x.UserID == uID).FirstOrDefault();
                transaction.AUDIT_TX_TYPE = db.AUDIT_TX_TYPE.Where(x => x.TypeValue == type).FirstOrDefault();
                transaction.AuditLogTxTypeID = transaction.AUDIT_TX_TYPE.AuditLogTxTypeID;
                transaction.TxCriticalDataString = transaction.USER.USER_LOGIN.ACCESS_LEVEL.LevelName + ": " + transaction.USER.FirstName + " " + transaction.USER.LastName + " performed a(n) " + transaction.AUDIT_TX_TYPE.TypeValue + " (" + context + ") transaction.";
                transaction.TxOldRecord = "-";
                transaction.TxNewRecord = json.ToString();
                try
                {
                    transaction.AuditLogTxID = db.AUDIT_LOG.Max(u => u.AuditLogTxID) + 1;
                }
                catch (Exception)
                {
                    transaction.AuditLogTxID = 0;
                }

                db.AUDIT_LOG.Add(transaction);
                db.SaveChanges();
            }
        }
    }
    public static class TxTypes
    {
        //Uncategorized Operation
        public static string Uncategorized = "Uncategorized Operation";
        //Initiate Add Operation
        public static string AddInit = "Initiate Add Operation";
        //Successful Add Operation
        public static string AddSuccess = "Successful Add Operation";
        //Failed Add Operation
        public static string AddFail = "Failed Add Operation";
        //Initiate Search Operation
        public static string SearchInit = "Initiate Search Operation";
        //Successful Search Operation
        public static string SearchSuccess = "Successful Search Operation";
        //Failed Search Operation
        public static string SearchFail = "Failed Search Operation";
        //Initiate View Operation
        public static string ViewInit = "Initiate View Operation";
        //Successful View Operation
        public static string ViewSuccess = "Successful View Operation";
        //Failed View Operation
        public static string ViewFail = "Failed View Operation";
        //Initiate Update/Edit Operation
        public static string UpdateInit = "Initiate Update/Edit Operation";
        //Successful Update/Edit Operation
        public static string UpdateSuccess = "Successful Update/Edit Operation";
        //Failed Update/Edit Operation
        public static string UpdateFail = "Failed Update/Edit Operation";
        //Initiate Delete Operation
        public static string DeleteInit = "Initiate Delete Operation";
        //Successful Delete Operation
        public static string DeleteSuccess = "Successful Delete Operation";
        //Failed Delete Operation
        public static string DeleteFail = "Failed Delete Operation";
        //Initiate Generic Override Operation
        public static string OverrideInit = "Initiate Generic Override Operation";
        //Successful Generic Override Operation
        public static string OverrideSuccess = "Successful Generic Override Operation";
        //Failed Generic Override Operation
        public static string OverrideFail = "Failed Generic Override Operation";
        //Initiate Override Add Operation
        public static string OverrideAdd = "Initiate Override Add Operation";
        //Successful Override Add Operation
        public static string AddOverrideSuccess = "Successful Override Add Operation";
        //Failed Override Add Operation
        public static string AddOverrideFail = "Failed Override Add Operation";
        //Initiate Override Search Operation
        public static string OverrideSearch = "Initiate Override Search Operation";
        //Successful Override Search Operation
        public static string SearchOverrideSuccess = "Successful Override Search Operation";
        //Failed Override Search Operation
        public static string SearchOverrideFail = "Failed Override Search Operation";
        //Initiate Override Update/Edit Operation
        public static string OverrideUpdate = "Initiate Override Update/Edit Operation";
        //Successful Override Update/Edit Operation
        public static string UpdateOverrideSuccess = "Successful Override Update/Edit Operation";
        //Failed Override Update/Edit Operation
        public static string UpdateOverrideFail = "Failed Override Update/Edit Operation";
        //Initiate Override Delete Operation
        public static string OverrideDelete = "Initiate Override Delete Operation";
        //Successful Override Delete Operation
        public static string DeleteOverrideSuccess = "Successful Override Delete Operation";
        //Failed Override Delete Operation
        public static string DeleteOverrideFail = "Failed Override Delete Operation";
        //No Results Search Operation
        public static string NoResultsSearch = " Results Search Operation";
        //Initiate Reporting Operation
        public static string ReportingInit = "Initiate Reporting Operation";
        //Successful Reporting Operation
        public static string ReportingSuccess = "Successful Reporting Operation";
        //Failed Reporting Operation
        public static string ReportingFail = "Failed Reporting Operation";
        //Initiate Notification Operation
        public static string NotificationInit = "Initiate Notification Operation";
        //Successful Notification Operation
        public static string NotificationSuccess = "Successful Notification Operation";
        //Failed Notification Operation
        public static string NotificationFail = "Failed Notification Operation";
        //Login Operation
        public static string Login = "Login Operation";
        //Logout Operation
        public static string Logout = "Logout Operation";
        //Initiate Upload/Import Operation
        public static string UploadInit = "Initiate Upload/Import Operation";
        //Successful Upload/Import Operation
        public static string UploadSuccess = "Successful Upload/Import Operation";
        //Failed Upload/Import Operation
        public static string UploadFail = "Failed Upload/Import Operation";
        //Initiate Download/Export Operation
        public static string DownloadInit = "Initiate Download/Export Operation";
        //Successful Download/Export Operation
        public static string DownloadSuccess = "Successful Download/Export Operation";
        //Failed Download/Export Operation
        public static string DownloadFail = "Failed Download/Export Operation";
        //Initiate Print/Publish Operation
        public static string PrintInit = "Initiate Print/Publish Operation";
        //Successful Print/Publish Operation
        public static string PrintSuccess = "Successful Print/Publish Operation";
        //Failed Print/Publish Operation
        public static string PrintFail = "Failed Print/Publish Operation";
        //Initiate Tag Maintenance Operation
        public static string TagInit = "Initiate Tag Maintenance Operation";
        //Successful Tag Maintenance Operation
        public static string TagSuccess = "Successful Tag Maintenance Operation";
        //Failed Tag Maintenance Operation
        public static string TagFail = "Failed Tag Maintenance Operation";
        //Initiate SysFlag Operation
        public static string SysFlagInit = "Initiate SysFlag Operation";
        //Successful SysFlag Operation
        public static string SysFlagSuccess = "Successful SysFlag Operation";
        //Failed SysFlag Operation
        public static string SysFlagFail = "Failed SysFlag Operation";
        //Initiate Auto-Archive Operation
        public static string AutoArchiveInit = "Initiate Auto-Archive Operation";
        //Successful Auto-Archive Operation
        public static string AutoArchiveSuccess = "Successful Auto-Archive Operation";
        //Failed Auto-Archive Operation
        public static string AutoArchiveFail = "Failed Auto-Archive Operation";
        //Initiate Lock Operation
        public static string LockInit = "Initiate Lock Operation";
        //Successful Lock Operation
        public static string LockSuccess = "Successful Lock Operation";
        //Failed Lock Operation
        public static string LockFail = "Failed Lock Operation";
        //Initiate Unlock Operation
        public static string UnlockInit = "Initiate Unlock Operation";
        //Pending Unlock Operation
        public static string UnlockPending = "Pending Unlock Operation";
        //Successful Unlock Operation
        public static string UnlockSuccess = "Successful Unlock Operation";
        //Failed Unlock Operation
        public static string UnlockFail = "Failed Unlock Operation";
        //Initiate Migration Operation
        public static string MigrateInit = "Initiate Migration Operation";
        //Successful Migration Operation
        public static string MigrateSuccess = "Successful Migration Operation";
        //Failed Migration Operation
        public static string MigrateFail = "Failed Migration Operation";
        //Initiate Link Operation
        public static string LinkInit = "Initiate Link Operation";
        //Successful Link Operation
        public static string LinkSuccess = "SuccessfulSuccessful Link Operation";
        //Failed Link Operation
        public static string LinkFail = "Failed Link Operation";
        //Initiate Token Generation Operation
        public static string GenerateTokensInit = "Initiate Token Generation Operation";
        //Successful Token Generation Operation
        public static string GenerateTokensSuccess = "Successful Token Generation Operation";
        //Failed Token Generation Operation
        public static string GenerateTokensFail = "Failed Token Generation Operation";
        //Initiate Google Calendar Pull Operation
        public static string CalendarPullInit = "Initiate Google Calendar Pull Operation";
        //Successful Google Calendar Pull Operation
        public static string CalendarPullSuccess = "Successful Google Calendar Pull Operation";
        //Failed Google Calendar Pull Operation
        public static string CalendarPullFail = "Failed Google Calendar Pull Operation";
        //Initiate Google Calendar Push Operation
        public static string CalendarPushInit = "Initiate Google Calendar Push Operation";
        //Successful Google Calendar Push Operation
        public static string CalendarPushSuccess = "Successful Google Calendar Push Operation";
        //Failed Google Calendar Push Operation
        public static string CalendarPushFail = "Failed Google Calendar Push Operation";
        //General Success Operation
        public static string GeneralSuccess = "General Success Operation";
        //General Failed Operation
        public static string GeneralFail = "General Failed Operation";
        //Ignored Warning Operation
        public static string WarningIgnord = "Ignored Warning Operation";
        //Initiate Authorisation Operation
        public static string AuthorisationInit = "Initiate Authorisation Operation";
        //Successful Authorisation Operation
        public static string AuthorisationSuccess = "Successful Authorisation Operation";
        //Failed Authorisation Operation
        public static string AuthorisationFail = "SuccessfulFailed Authorisation Operation";
        //Initiate FTP Transfer Operation
        public static string FTPInit = "Initiate FTP Transfer Operation";
        //Successful FTP Transfer Operation
        public static string FTPSuccess = "Successful FTP Transfer Operation";
        //Failed FTP Transfer Operation
        public static string FTPFail = "Failed FTP Transfer Operation";
        //Cancelled Operation
        public static string Cancellation = "Cancelled Operation";
    }
    
}