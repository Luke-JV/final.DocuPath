using DocuPath.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocuPath.Models
{
    public class AuthorizeByAccessArea : AuthorizeAttribute
    {
        // Custom property
        public string AccessArea { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }
            
            using (DocuPathEntities db = new DocuPathEntities())
            {
                USER check = new USER();
                check = db.USER.Where(x => x.USER_LOGIN.Username == httpContext.User.Identity.Name).FirstOrDefault();
                check.USER_LOGIN = db.USER_LOGIN.Where(x => x.UserLoginID == check.UserLoginID).FirstOrDefault();
                ACCESS_LEVEL level = db.ACCESS_LEVEL.Where(x => x.AccessLevelID == check.USER_LOGIN.AccessLevelID).FirstOrDefault();
                foreach (LEVEL_AREA area in db.LEVEL_AREA)
                {
                    if (area.ACCESS_AREA.AccessAreaDescription == AccessArea && area.AccessLevelID == level.AccessLevelID)
                    {
                        return true;
                    }
                    else return false;
                }
                
            }
            return true;
        }
       
    }
    //public class RestrictAccessToAssignedManagers : AuthorizationAttribute
    //{
    //    protected override AuthorizationResult IsAuthorized(System.Security.Principal.IPrincipal principal, AuthorizationContext authorizationContext)
    //    {
    //        EmployeePayHistory eph = (EmployeePayHistory)authorizationContext.Instance;
    //        Employee selectedEmployee;
    //        Employee authenticatedUser;

    //        using (AdventureWorksEntities context = new AdventureWorksEntities())
    //        {
    //            selectedEmployee = context.Employees.SingleOrDefault(e => e.EmployeeID == eph.EmployeeID);
    //            authenticatedUser = context.Employees.SingleOrDefault(e => e.LoginID == principal.Identity.Name);
    //        }

    //        if (selectedEmployee.ManagerID == authenticatedUser.EmployeeID)
    //        {
    //            return AuthorizationResult.Allowed;
    //        }
    //        else
    //        {
    //            return new AuthorizationResult("Only the authenticated manager for the employee can add a new record.");
    //        }
    //    }
    //}

}