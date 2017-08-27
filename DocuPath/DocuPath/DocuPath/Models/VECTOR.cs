using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.Models
{
    public static class VECTOR
    {
        public static void GenerateToken(string email)
        {
            using (DocuPathEntities db = new DocuPathEntities())
            {
                TOKEN_LOG token = new TOKEN_LOG();
                token.TokenID = 0;
                token.TokenValue = "qwer1234";
                token.IssueTimestamp = DateTime.Now;
                token.DestinationEmail = email;
                token.AccessLevelID = 0;

            }
        }

        public static bool ValidateAccess(int userID)
        {
            if (VERTEBRAE.getCurrentUser().UserID == userID)
            {
                return true;
            }
            else return false;
        }
    }
}