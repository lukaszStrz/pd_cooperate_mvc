using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Projekt_1
{
    public class MyRole : RoleProvider
    {
        string appname = "MyRole";

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                return appname;
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            using (db_BigbrainDataContext db = new db_BigbrainDataContext())
            {
                string[] userRoles = (from u in db.P1_Users
                                      where u.User_login.Equals(username)
                                      join uir in db.P1_UserInRoles on u.User_login equals uir.User_login
                                      join ur in db.P1_UserRoles on uir.UserRole_id equals ur.UserRole_id
                                      select ur.UserRole_name).ToArray();
                return userRoles;
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (db_BigbrainDataContext db = new db_BigbrainDataContext())
            {
                if (!RoleExists(roleName))
                    throw new Exception("Nie istnieje rola o podanej nazwie.");
                if (String.IsNullOrEmpty(username))
                    throw new ArgumentException("User name empty!");
                bool isInRole = Convert.ToBoolean((from uir in db.P1_UserInRoles
                                                   where uir.User_login.Equals(username) && uir.P1_UserRole.UserRole_name.Equals(roleName)
                                                   select uir).Count());
                return isInRole;
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            using (db_BigbrainDataContext db = new db_BigbrainDataContext())
            {
                if (String.IsNullOrEmpty(roleName))
                    throw new ArgumentException("Rolename empty!");
                bool exists = Convert.ToBoolean((from r in db.P1_UserRoles
                                                 where r.UserRole_name.Equals(roleName)
                                                 select r.UserRole_name).Count());
                return exists;
            }
        }
    }
}