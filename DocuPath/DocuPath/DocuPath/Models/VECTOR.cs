using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.Models
{
    public static class VECTOR
    {
        public static TOKEN_LOG GenerateToken(string email)
        {
            TOKEN_LOG token = new TOKEN_LOG();
            using (DocuPathEntities db = new DocuPathEntities())
            {
                token.TokenID = 0;
                token.TokenValue = Randomiser();
                token.IssueTimestamp = DateTime.Now;
                token.DestinationEmail = email;
                //token.AccessLevelID = 0;

            }
            return token;
        }

        public static bool ValidateAccess(int userID)
        {
            if (VERTEBRAE.getCurrentUser().UserID == userID)
            {
                return true;
            }
            else return false;
        }

        public static string Randomiser()
        {
            string output = "";
            int count = 0;
            Random gen = new Random();
            char adder = 'a';
            while(output.Length<15)
            {
                adder = 'a';
                if (gen.Next(0,2) > 0)
                {
                    adder = (char)(adder + gen.Next(0, 26));
                    if (gen.Next(0, 1) > 0)
                    {
                        output += adder;
                    }
                    else
                    {
                        output += adder.ToString().ToUpper();
                    }
                }
                else if (gen.Next(0,100) > 25)
                {
                    output += gen.Next(0, 9);
                }
                else
                {
                    adder = (char)(adder + gen.Next(0, 26));
                    output += adder;
                }
            }

            return output;
        }
    }
}