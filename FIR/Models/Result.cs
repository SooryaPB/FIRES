using System;
using System.Collections.Generic;

using System.Web;


namespace ENVHR_CSHARP.Models
{
    public class Result
    {
        public string result { set; get; }
        public int retcode { set; get; }
        public string ServerName { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public string SqlText { set; get; }
        public Result(string result, int retcode)
        {
            this.result = result;
            this.retcode = retcode;
        }
        public Result(string ServerName, string UserName, string Password, string result, int retcode)
        {
            this.ServerName = ServerName;
            this.UserName = UserName;
            this.Password = Password;
            this.result = result;
            this.retcode = retcode;
        }
        public Result(string SqlText)
        {
            this.SqlText = SqlText;           
        }
    }
}