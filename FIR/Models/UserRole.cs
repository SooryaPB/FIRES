using System;
using System.Collections.Generic;

using System.Web;

namespace ENVHR_CSHARP.Models
{
    public class UserRole
    {
        public string UserId { set; get; }
        public string UserName { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Admin { set; get; }
        public string User { set; get; }
        public string Tester { set; get; }



        public UserRole(string UserId, string UserName, string FirstName, string LastName, string Admin, string User, string Tester)
        {
            this.UserId = UserId;
            this.UserName = UserName;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Admin = Admin;
            this.User = User;
            this.Tester = Tester;
        }
    }
}