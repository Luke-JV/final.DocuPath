using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DocuPath.DBLayer
{
    public class IDUser : IUser<int>
    {
        
        public IDUser()
        {
        }
        public IDUser(string userName)
        {
        }
        public int Id { get; set; }
        public string UserName { get; set; }
        // can also define optional properties such as:
        //    PasswordHash
        //    SecurityStamp
        //    Claims
        //    Logins
        //    Roles
    }

    public class UserStore : IUserStore<USER, int>
    {
        public UserStore() {  }
        public UserStore(/*ExampleStorage database*/int x) {  }
        public Task CreateAsync(USER user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            using (DocuPathEntities db = new DocuPathEntities())
            {
                USER addUser = new USER();
                addUser.FirstName = "Luke";
                addUser.Id = 1;
                addUser.USER_LOGIN.Username = "Luke";
                db.USER.Add(addUser);
            }
            //userTable.Insert(user);

            return Task.FromResult<object>(null);
        }
        public Task DeleteAsync(USER user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            //userTable.Insert(user);

            return Task.FromResult<object>(null);
        }
        public Task<USER> FindByIdAsync(int userId)
        {
            if (userId == default(int))
            {
                throw new ArgumentNullException("userId");
            }

            //userTable.Insert(user);

            return Task.FromResult<USER>(null);
        }
        public Task<USER> FindByNameAsync(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("user");
            }

            //userTable.Insert(user);

            return Task.FromResult<USER>(null);
        }
        public Task UpdateAsync(USER user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            //userTable.Insert(user);

            return Task.FromResult<object>(null);
        }
        public void Dispose() { }
    }

    public class IdentityRole : IRole<int>
    {
        public IdentityRole() { }
        public IdentityRole(string roleName) {  }
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class RoleStore : IRoleStore<IdentityRole, int>
    {
        public RoleStore() {  }
        public RoleStore(/*ExampleStorage database*/int x) {  }
        public Task CreateAsync(IdentityRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            //userTable.Insert(user);

            return Task.FromResult<object>(null);
        }
        public Task DeleteAsync(IdentityRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            //userTable.Insert(user);

            return Task.FromResult<object>(null);
        }
        public Task<IdentityRole> FindByIdAsync(int roleId)
        {
            if (roleId == default(int))
            {
                throw new ArgumentNullException("roleId");
            }

            //userTable.Insert(user);

            return Task.FromResult<IdentityRole>(null);
        }
        public Task<IdentityRole> FindByNameAsync(string roleName)
        {
            if (roleName == null)
            {
                throw new ArgumentNullException("role");
            }

            //userTable.Insert(user);

            return Task.FromResult<IdentityRole>(null);
        }
        public Task UpdateAsync(IdentityRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            //userTable.Insert(user);

            return Task.FromResult<object>(null);
        }
        public void Dispose() { }
    }
}