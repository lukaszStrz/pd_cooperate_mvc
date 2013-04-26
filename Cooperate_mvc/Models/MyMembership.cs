using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Projekt_1
{
    public class MyMembership : MembershipProvider
    {
        string appname = "MyMembershipProvider";

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

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            using (db_BigbrainDataContext db = new db_BigbrainDataContext())
            {
                bool valid;

                List<P1_User> users = (from u in db.P1_Users
                                 where u.User_login.Equals(username)
                                 select u).ToList();

                if (users.Count == 0)
                    return false;
                if (users.Count > 1)
                    throw new System.Exception("Błąd bazy danych.");

                if (users[0].User_IsLocked)
                    valid=false;
                else
                {
                    valid = Hash.Hash.CheckHash(password, users[0].User_password, Hash.HashType.SHA512); // sprawdzanie zgodności hasła
                    if(!valid)
                    {
                        List<P1_LoginAttempt> attempts = users[0].P1_LoginAttempts.ToList();
                        if(attempts.Count>=2 && (!attempts[attempts.Count-1].LoginAttempt_success && !attempts[attempts.Count-2].LoginAttempt_success)) // blokowanie usera w przypadku trzeciego błędnego logowania
                            users[0].User_IsLocked=true;
                    }
                }

                P1_LoginAttempt attempt = new P1_LoginAttempt();
                attempt.LoginAttempt_date = DateTime.Now;
                attempt.LoginAttempt_success=valid;
                //attempt.User_login=username;
                users[0].P1_LoginAttempts.Add(attempt);
                db.SubmitChanges();
                return valid;
            }
        }

        public static bool IsLocked(string username)
        {
            using (db_BigbrainDataContext db = new db_BigbrainDataContext())
            {
                List<P1_User> users = (from u in db.P1_Users
                                       where u.User_login.Equals(username)
                                       select u).ToList();

                if (users.Count == 0)
                {
                    throw new ArgumentException("Brak użytkownika o podanej nazwie.");
                }
                else if (users.Count > 1)
                    throw new System.Exception("Błąd bazy danych.");

                return users[0].User_IsLocked;
            }
        }
    }
}