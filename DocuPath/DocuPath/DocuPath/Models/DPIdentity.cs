using DocuPath.DataLayer;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace DocuPath.Models
{


    #region USER
    public class DPUser : IUser<int>
    {

        #region DPUSER
        public DPUser() { }
        public DPUser(string userName) { }
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }


        public int TitleID { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DisplayInitials { get; set; }
        public string QualificationDescription { get; set; }
        public string HPCSARegNumber { get; set; }
        public string NationalID { get; set; }
        public string AcademicID { get; set; }
        public string CellNum { get; set; }
        public string TelNum { get; set; }
        public string WorkNum { get; set; }
        public string PersonalEmail { get; set; }
        public string AcademicEmail { get; set; }
        public string PhysicalAddress { get; set; }
        public string PostalAddress { get; set; }
        public bool IsDeactivated { get; set; }
        public Nullable<bool> DarkUIPref { get; set; }

        public string SecurityStamp { get; set; }
        public string Discriminator { get; set; }
        public bool IsConfirmed { get; set; }
        DocuPathEntities db = new DocuPathEntities();

        public USER buildDPUser()
        {
            USER outUser = new USER();
            //405
            //using (DocuPathEntities db = new DocuPathEntities())
            //{
            //    outUser.UserID = db.USER.Max(u=>u.UserID) + 1; //405
            //}
            outUser.UserID = Id;
            outUser.TitleID = TitleID;
            outUser.FirstName = FirstName;
            outUser.MiddleName = MiddleName;
            outUser.LastName = LastName;
            outUser.DisplayInitials = DisplayInitials;
            outUser.QualificationDescription = QualificationDescription;
            outUser.HPCSARegNumber = HPCSARegNumber;
            outUser.NationalID = NationalID;
            outUser.AcademicID = AcademicID;
            outUser.CellNum = CellNum;
            outUser.TelNum = TelNum;
            outUser.WorkNum = WorkNum;
            outUser.PersonalEmail = PersonalEmail;
            outUser.AcademicEmail = AcademicEmail;
            outUser.PhysicalAddress = PhysicalAddress;
            outUser.PostalAddress = PostalAddress;
            outUser.IsDeactivated = IsDeactivated;
            outUser.DarkUIPref = DarkUIPref;

            outUser.SecurityStamp = SecurityStamp;

            outUser.IsConfirmed = IsConfirmed;

            outUser.UserName = UserName;

            outUser.USER_LOGIN = getLogin();
            //outUser.UserLoginID = outUser.USER_LOGIN.UserLoginID;




            return outUser;
        }
        public USER_LOGIN getLogin()
        {
            USER_LOGIN outLogin = new USER_LOGIN();
            //405

            //outLogin.UserLoginID = db.USER_LOGIN.Max(u => u.UserLoginID) + 1;//405

            outLogin.Username = UserName;
            outLogin.Password = PasswordHash;

            return outLogin;
        }



        public DPUser buildIUserByID(int id)
        {
            DPUser outUser = new DPUser();
            USER inUser = new USER();
            USER_LOGIN inLogin = new USER_LOGIN();
            //405

            inUser = db.USER.Where(x => x.UserID == id).FirstOrDefault();
            inLogin = db.USER_LOGIN.Where(x => x.UserLoginID == inUser.UserLoginID).FirstOrDefault();


            outUser.Id = id;
            outUser.TitleID = inUser.TitleID;
            outUser.FirstName = inUser.FirstName;
            outUser.MiddleName = inUser.MiddleName;
            outUser.LastName = inUser.LastName;
            outUser.DisplayInitials = inUser.DisplayInitials;
            outUser.QualificationDescription = inUser.QualificationDescription;
            outUser.HPCSARegNumber = inUser.HPCSARegNumber;
            outUser.NationalID = inUser.NationalID;
            outUser.AcademicID = inUser.AcademicID;
            outUser.CellNum = inUser.CellNum;
            outUser.TelNum = inUser.TelNum;
            outUser.WorkNum = inUser.WorkNum;
            outUser.PersonalEmail = inUser.PersonalEmail;
            outUser.AcademicEmail = inUser.AcademicEmail;
            outUser.PhysicalAddress = inUser.PhysicalAddress;
            outUser.PostalAddress = inUser.PostalAddress;
            outUser.IsDeactivated = inUser.IsDeactivated;
            outUser.DarkUIPref = inUser.DarkUIPref;

            outUser.SecurityStamp = inUser.SecurityStamp;

            outUser.IsConfirmed = inUser.IsConfirmed;

            outUser.UserName = inLogin.Username;
            outUser.PasswordHash = inLogin.Password;

            return outUser;
        }
        public DPUser buildIUserByName(string name)
        {
            DPUser outUser = new DPUser();
            USER inUser = new USER();
            USER_LOGIN inLogin = new USER_LOGIN();
            //405


            inUser = db.USER.Where(x => x.USER_LOGIN.Username == name).FirstOrDefault();
            if (inUser == null)
            {
                return new DPUser();
            }
            inUser.USER_LOGIN = db.USER.Where(x => x.UserID == inUser.UserID).FirstOrDefault().USER_LOGIN;
            inLogin = inUser.USER_LOGIN;


            outUser.Id = inUser.UserID;
            outUser.TitleID = inUser.TitleID;
            outUser.FirstName = inUser.FirstName;
            outUser.MiddleName = inUser.MiddleName;
            outUser.LastName = inUser.LastName;
            outUser.DisplayInitials = inUser.DisplayInitials;
            outUser.QualificationDescription = inUser.QualificationDescription;
            outUser.HPCSARegNumber = inUser.HPCSARegNumber;
            outUser.NationalID = inUser.NationalID;
            outUser.AcademicID = inUser.AcademicID;
            outUser.CellNum = inUser.CellNum;
            outUser.TelNum = inUser.TelNum;
            outUser.WorkNum = inUser.WorkNum;
            outUser.PersonalEmail = inUser.PersonalEmail;
            outUser.AcademicEmail = inUser.AcademicEmail;
            outUser.PhysicalAddress = inUser.PhysicalAddress;
            outUser.PostalAddress = inUser.PostalAddress;
            outUser.IsDeactivated = inUser.IsDeactivated;
            outUser.DarkUIPref = inUser.DarkUIPref;

            outUser.SecurityStamp = inUser.SecurityStamp;

            outUser.IsConfirmed = inUser.IsConfirmed;

            outUser.UserName = inLogin.Username;
            outUser.PasswordHash = inLogin.Password;

            return outUser;
        }
        // can also define optional properties such as:
        //    PasswordHash
        //    SecurityStamp
        //    Claims
        //    Logins
        //    Roles
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<DPUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        #endregion

    }
    public class DPUserStore : IUserStore<DPUser, int>,
                                    IUserClaimStore<DPUser, int>,
                                    IUserLoginStore<DPUser, int>,
                                    IUserRoleStore<DPUser, int>,
                                    IUserPasswordStore<DPUser, int>,
                                    IUserSecurityStampStore<DPUser, int>,
                                    IUserTwoFactorStore<DPUser, int>,
                                    IUserPhoneNumberStore<DPUser, int>,
                                    IUserEmailStore<DPUser, int>,
                                    IUserLockoutStore<DPUser, int>
    {
        public DPUserStore() { }
        public DPUserStore(ApplicationDbContext x) { }
        DocuPathEntities db = new DocuPathEntities();
        PasswordHasher phash = new PasswordHasher();


        public Task CreateAsync(DPUser user)
        {

            #region DELETE THIS
            using (DocuPathEntities db = new DocuPathEntities())
            {
                try
                {
                    user.Id = db.USER.Max(u => u.UserID) + 1;
                }
                catch (Exception)
                {
                    user.Id = 0;
                }
            }

            #endregion
            USER adduser = user.buildDPUser();
            try
            {
                adduser.UserLoginID = db.USER_LOGIN.Max(u => u.UserLoginID) + 1;
            }
            catch (Exception)
            {
                adduser.UserLoginID = 0;
            }
            adduser.USER_LOGIN.UserLoginID = (int)adduser.UserLoginID;
            db.USER.Add(adduser);
            
            db.SaveChanges();
            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(DPUser user)
        {
            throw new NotImplementedException();
        }

        public Task<DPUser> FindByIdAsync(int userId)
        {
            //if (userId == default(int))
            //{
            //    throw new ArgumentNullException("user");
            //}

            // userTable.Insert(user);
            DPUser user = new DPUser();
            user = user.buildIUserByID(userId);
            if (user != null)
            {
                return Task.FromResult<DPUser>(user);
            }

            return Task.FromResult<DPUser>(null);
        }

        public Task<DPUser> FindByNameAsync(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("user");
            }
            DPUser user = new DPUser();
            user = user.buildIUserByName(userName);
            if (user != null)
            {
                return Task.FromResult<DPUser>(user);
            }
            return Task.FromResult<DPUser>(null);
        }

        public Task UpdateAsync(DPUser user)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls



        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DPUserStore() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        /// <summary>
        /// CLAIMS
        /// </summary>

        public Task<IList<Claim>> GetClaimsAsync(DPUser user)
        {
            if (user == null)
            {
                return Task.FromResult<IList<Claim>>(null);
            }
            List<Claim> claims = new List<Claim>();

            foreach (var claim in db.USER_CLAIM.Where(x => x.UserID == user.Id))
            {
                claims.Add(new Claim(claim.ClaimType, claim.ClaimValue));
            }


            return Task.FromResult<IList<Claim>>(claims);
        }

        public Task AddClaimAsync(DPUser user, Claim claim)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimAsync(DPUser user, Claim claim)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// LOGINS
        /// </summary>

        public Task AddLoginAsync(DPUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(DPUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(DPUser user)
        {
            throw new NotImplementedException();
        }

        public Task<DPUser> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// ROLES
        /// </summary>

        public Task AddToRoleAsync(DPUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(DPUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(DPUser user)
        {
            //405
            if (user == null)
            {
                return Task.FromResult<IList<string>>(null);
            }
            List<string> roles = new List<string>();
            USER roleUser = db.USER.Where(x => x.UserID == user.Id).FirstOrDefault();
            roleUser.USER_LOGIN.ACCESS_LEVEL = db.ACCESS_LEVEL.Where(x => x.AccessLevelID == roleUser.USER_LOGIN.AccessLevelID).FirstOrDefault();
            roles.Add(roleUser.USER_LOGIN.ACCESS_LEVEL.LevelName);

            return Task.FromResult<IList<string>>(roles);
        }

        public Task<bool> IsInRoleAsync(DPUser user, string roleName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// PASSWORD
        /// </summary>

        public Task SetPasswordHashAsync(DPUser user, string passwordHash)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.PasswordHash = passwordHash;
            return Task.FromResult<int>(0);
        }

        public Task<string> GetPasswordHashAsync(DPUser user)
        {
            if (user == null)
            {
                return Task.FromResult<string>(null);
            }

            return Task.FromResult<string>(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(DPUser user)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// SECRITY STAMP
        /// </summary>


        public Task SetSecurityStampAsync(DPUser user, string stamp)
        {
            //405
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            user.SecurityStamp = stamp;
            return Task.FromResult<int>(0);
        }

        public Task<string> GetSecurityStampAsync(DPUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            return Task.FromResult(user.SecurityStamp);
        }
        /// <summary>
        /// TWO FACTOR
        /// </summary>

        public Task SetTwoFactorEnabledAsync(DPUser user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            //405
            return Task.FromResult<int>(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(DPUser user)
        {
            //throw new NotImplementedException();
            //404
            return Task.FromResult<bool>(false);
        }
        /// <summary>
        /// PHONE NUMBER
        /// </summary>

        public Task SetPhoneNumberAsync(DPUser user, string phoneNumber)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPhoneNumberAsync(DPUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(DPUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetPhoneNumberConfirmedAsync(DPUser user, bool confirmed)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// EMAIL
        /// </summary>

        public Task SetEmailAsync(DPUser user, string email)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(DPUser user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetEmailConfirmedAsync(DPUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(DPUser user, bool confirmed)
        {
            throw new NotImplementedException();
        }

        public Task<DPUser> FindByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// LOCKOUT
        /// </summary>

        public Task<DateTimeOffset> GetLockoutEndDateAsync(DPUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(DPUser user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(DPUser user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(DPUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAccessFailedCountAsync(DPUser user)
        {
            //throw new NotImplementedException();
            //404
            return Task.FromResult<int>(0);
        }

        public Task<bool> GetLockoutEnabledAsync(DPUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            //405
            return Task.FromResult<bool>(false);
        }

        public Task SetLockoutEnabledAsync(DPUser user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            //405
            return Task.FromResult<int>(0);
        }
        #endregion
    }

    #endregion
    #region ROLE STORE
    public class DPRole : IRole<int>
    {
        public DPRole() { }
        public DPRole(string roleName) { }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class RoleStore : IRoleStore<DPRole, int>
    {
        public Task CreateAsync(DPRole role)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(DPRole role)
        {
            throw new NotImplementedException();
        }

        public Task<DPRole> FindByIdAsync(int roleId)
        {
            throw new NotImplementedException();
        }

        public Task<DPRole> FindByNameAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(DPRole role)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RoleStore() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }


    #endregion
}
