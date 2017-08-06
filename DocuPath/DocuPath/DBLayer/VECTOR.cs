using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocuPath.DBLayer
{
    public static class VECTOR
    {
        
        #region CAPSULE
        //public static string CAPSULE(string key, string value)
        //{
        //    switch (key)
        //    {
        //        case "PASS":
        //            {
        //                if (VPass(value))
        //                {
        //                    return "TRUE";
        //                }                    
        //            }
        //            break;
        //        default:
        //            {
        //                return "999";
        //            }
                    
        //    }
        //    return "998";
        //}
        #endregion

        #region ENCRYPTION

        #endregion

        #region AUTHENTICATION
        public static bool VPass(string user,string pass)
        {
            DocuPathEntities db = new DocuPathEntities();
            foreach (var UL in db.USER_LOGIN)
            {
                if (UL.Username == user && UL.Password == pass)
                {
                    return true;
                }
                else
                {
                    return false;
                }                
            }
            return false;
        }
        #endregion

        #region RAT GENERATION

        #endregion

        #region TIMEOUTS

        #endregion

        #region C/S INTEGRITY

        #endregion
    }
}