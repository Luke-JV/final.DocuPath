using DocuPath.DataLayer;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace DocuPath.Models
{
    public static class VERTEBRAE
    {
        public static List<NOTIFICATION> GetUnhandledNeurons()
        {
            using (DocuPathEntities db = new DocuPathEntities())
            {
                List<NOTIFICATION> unhandledNeurons = new List<NOTIFICATION>();

                foreach (var neuron in db.NOTIFICATION)
                {
                    unhandledNeurons.Add(neuron);
                }

                return unhandledNeurons;
            }
        }
        public static USER getCurrentUser()
        {
            
            DocuPathEntities db = new DocuPathEntities();
            //int id = User.Identity.GetUserId<int>();
            int id = HttpContext.Current.User.Identity.GetUserId<int>();
            USER currentUser = db.USER.Where(x => x.UserID == id).FirstOrDefault();
            currentUser.USER_LOGIN = db.USER_LOGIN.Where(x => x.UserLoginID == currentUser.UserLoginID).FirstOrDefault();
            currentUser.TITLE = db.TITLE.Where(x => x.TitleID == currentUser.TitleID).FirstOrDefault();

            return currentUser;
        }
        //public static List<METRIC> FetchVisionMetrics()
        //{
                // TODO: Compile a list of metrics for use in VISION dashboard.
                // Unsure what to do to have METRIC show up as an option when defining method return type... did this too long ago.
        //}
    }
}