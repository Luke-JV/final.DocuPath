﻿using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using DocuPath.Models;
using DocuPath.DataLayer;
using DocuPath.Models.DPViewModels;
using System.Collections.Generic;

namespace DocuPath.Controllers
{



    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login

        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (!ModelState.IsValid)
            {
                return View(model);//404 propagate            
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    {

                        int id = UserManager.FindByName(model.Email).Id;
                        #region AUDIT_WRITE
                        AuditModel.WriteTransaction(VERTEBRAE.getCurrentUser().UserID, TxTypes.AddInit, "Account");
                        #endregion
                        //404!!!
                        //ACTIVE_LOGIN login = new ACTIVE_LOGIN();
                        //login.DeviceIPAddress = Request.UserHostAddress;
                        //login.LoginTimestamp = DateTime.Now;
                        //login.LastActionTimestamp = DateTime.Now;
                        //login.UserID = id;

                        //using (DocuPathEntities db = new DocuPathEntities())
                        //{
                        //    db.ACTIVE_LOGIN.Add(login);
                        //    db.SaveChanges();
                        //}
                        if (UserManager.FindById(id).IsDeactivated == true)
                        {
                            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                            VERTEBRAE.sendSMS("+27" + UserManager.FindById(id).CellNum.Substring(1), "DocuPath: You were unable to log in as your User Profile has been deactivated. You may no longer access the DocuPath system. Please contact your Head of Department if you feel that this is incorrect.");
                            return RedirectToAction("Index", "Home");
                        }
                        VERTEBRAE.sendSMS("+27"+UserManager.FindById(id).CellNum.Substring(1),"DocuPath: You logged in to the system at: "+DateTime.Now.ToString()+". If this was not you please contact your system administrator as soon as possible.");
                        return RedirectToLocal(returnUrl);
                    }
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }
        [AllowAnonymous]
        public ActionResult RedeemToken(string id)
        {
            TOKEN_LOG redemptionToken = new TOKEN_LOG();
            using (DocuPathEntities db = new DocuPathEntities())
            {

                PasswordHasher crypto = new PasswordHasher();
                //string hash = crypto.HashPassword(id);
                foreach (var tk in db.TOKEN_LOG.Where(x => x.RedemptionTimestamp == null))
                {
                    if (crypto.VerifyHashedPassword(tk.TokenValue, id) != PasswordVerificationResult.Failed && tk.RedemptionTimestamp == null)
                    {
                        db.TOKEN_LOG.Where(m => m.TokenID == tk.TokenID).FirstOrDefault().RedemptionTimestamp = DateTime.Now;
                        RegSesh session = new RegSesh();
                        session.id = VECTOR.hash(tk.AccessLevelID.ToString());
                        session.alID = tk.AccessLevelID;
                        Session["REG"] = session;
                        return RedirectToAction("RegisterUserProfile","Account");
                    }
                }

            }
            return View("Login");
        }
        struct RegSesh
        {
            public string id;
            public int alID;
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            RegSesh session = new RegSesh();
            session = (RegSesh)Session["REG"];
            Session["REG"] = null;
            if (VECTOR._lock(session.id,session.alID.ToString()))
            {
                RegisterViewModel model = new RegisterViewModel();
                DocuPathEntities db = new DocuPathEntities();

                model.user = new USER();
                model.user.USER_LOGIN = new USER_LOGIN();
                model.user.USER_LOGIN.AccessLevelID = session.alID;
                model.titles = db.TITLE.ToList();

                List<UiPrefKVP> prefslist = new List<UiPrefKVP>();
                UiPrefKVP notset = new UiPrefKVP();
                notset.prefID = null;
                notset.prefPhrase = "Not Set";
                prefslist.Add(notset);
                UiPrefKVP light = new UiPrefKVP();
                light.prefID = 0;
                light.prefPhrase = "Light Theme";
                prefslist.Add(light);
                UiPrefKVP dark = new UiPrefKVP();
                dark.prefID = 1;
                dark.prefPhrase = "Dark Theme";
                prefslist.Add(dark);

                model.uiprefs = prefslist;

                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Index");
            }
        }

        [AllowAnonymous]
        public ActionResult RegisterUserProfile()
        {
            RegSesh session = new RegSesh();
            session = (RegSesh)Session["REG"];
            Session["REG"] = null;
            if (VECTOR._lock(session.id, session.alID.ToString()))
            {
                RegisterViewModel model = new RegisterViewModel();
                DocuPathEntities db = new DocuPathEntities();

                model.user = new USER();
                model.user.USER_LOGIN = new USER_LOGIN();
                model.user.USER_LOGIN.AccessLevelID = session.alID;
                model.titles = db.TITLE.ToList();

                List<UiPrefKVP> prefslist = new List<UiPrefKVP>();
                UiPrefKVP notset = new UiPrefKVP();
                notset.prefID = null;
                notset.prefPhrase = "Not Set";
                prefslist.Add(notset);
                UiPrefKVP light = new UiPrefKVP();
                light.prefID = 0;
                light.prefPhrase = "Light Theme";
                prefslist.Add(light);
                UiPrefKVP dark = new UiPrefKVP();
                dark.prefID = 1;
                dark.prefPhrase = "Dark Theme";
                prefslist.Add(dark);

                model.uiprefs = prefslist;

                return View(model);
            }
            else
            {
                return RedirectToAction( "Index","Home");
            }
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new DPUser { UserName = model.Email };
                user.FirstName = model.user.FirstName;
                user.AcademicID = model.user.AcademicID;
                user.NationalID = model.user.NationalID;
                user.MiddleName = model.user.MiddleName;
                user.LastName = model.user.LastName;
                user.PhysicalAddress = model.user.PhysicalAddress;
                user.TitleID = model.user.TitleID;
                user.TelNum = model.user.TelNum;
                user.PostalAddress = model.user.PostalAddress;
                user.PersonalEmail = model.Email;
                user.AcademicEmail = model.user.AcademicEmail;
                user.QualificationDescription = model.user.QualificationDescription;
                user.AccessLevelID = model.user.USER_LOGIN.AccessLevelID;

                user.Discriminator = model.user.FirstName;
                user.DisplayInitials = model.user.DisplayInitials;
                user.CellNum = model.user.CellNum;
                user.DarkUIPref = model.user.DarkUIPref;
                user.WorkNum = model.user.WorkNum;
                user.HPCSARegNumber = model.user.HPCSARegNumber;
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return

                View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterUserProfile(RegisterViewModel model)
        {
            
                var user = new DPUser { UserName = model.user.AcademicEmail };
                user.FirstName = model.user.FirstName;
                user.AcademicID = model.user.AcademicID;
                user.NationalID = model.user.NationalID;
                user.MiddleName = model.user.MiddleName;
                user.LastName = model.user.LastName;
                user.PhysicalAddress = model.user.PhysicalAddress;
                user.TitleID = model.user.TitleID;
                user.TelNum = model.user.TelNum;
                user.PostalAddress = model.user.PostalAddress;
                user.PersonalEmail = model.Email;
                user.AcademicEmail = model.user.AcademicEmail;
                user.QualificationDescription = model.user.QualificationDescription;
                user.AccessLevelID = model.user.USER_LOGIN.AccessLevelID;

                user.Discriminator = model.user.FirstName;
                user.DisplayInitials = model.user.DisplayInitials;
                user.CellNum = model.user.CellNum;
                user.DarkUIPref = model.user.DarkUIPref;
                user.WorkNum = model.user.WorkNum;
                user.HPCSARegNumber = model.user.HPCSARegNumber;
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            return null;
        }
        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (userId == default(int) || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToAction("Home", "Index");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                VERTEBRAE.sendMail(model.Email, "Please reset your password by clicking: " + callbackUrl, "RESET PASSWORD");
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        [ValidateInput(false)]
        public ActionResult ResetPassword(int userId, string code)
        {

            //return code == null ? View("Error") : View();
            ResetPasswordViewModel model = new ResetPasswordViewModel();
            var user = UserManager.FindById(userId);
            model.Email = user.AcademicEmail;
            model.Code = model.Code;
            return View(model);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == default(int))
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new DPUser { UserName = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}