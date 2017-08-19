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
        public static void WriteTransaction(int id, string type)
        {
            using (DocuPathEntities db = new DocuPathEntities()) 
            {
                
                AUDIT_LOG transaction = new AUDIT_LOG();
                transaction.TxDateStamp = DateTime.Now;
                transaction.TxTimeStamp = DateTime.Now;
                transaction.UserID = id;
                transaction.USER = db.USER.Where(x => x.UserID == id).FirstOrDefault();
                transaction.AUDIT_TX_TYPE = db.AUDIT_TX_TYPE.Where(x => x.TypeValue == type).FirstOrDefault();
                transaction.AuditLogTxTypeID = transaction.AUDIT_TX_TYPE.AuditLogTxTypeID;
                transaction.TxCriticalDataString = "404";
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
}