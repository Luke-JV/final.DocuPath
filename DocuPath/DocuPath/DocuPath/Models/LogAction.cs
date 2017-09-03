using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Models
{
    public class LogAction : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //return null;
            //updateLastAction();
            
        }
        public void updateLastAction()
        {
            using (DocuPathEntities db = new DocuPathEntities())
            {
                try
                {
                    int id = VERTEBRAE.getCurrentUser().UserID;
                    db.ACTIVE_LOGIN.Where(x => x.UserID == id).FirstOrDefault().LastActionTimestamp = DateTime.Now;
                }
                catch (Exception)
                {

                }
            }
        }
    }
}