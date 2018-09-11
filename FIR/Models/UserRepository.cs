using ENVHR_CSHARP.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Web;

namespace ENVHR_CSHARP.Models
{

    public class User
    {    
        public string UserName { set; get; }
        public string Password { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string UserId { set; get; }
        public string Token { set; get; }       
        public string DivCode { set; get; }


        public User(string UserName, string Password)
        {
            this.UserName = UserName;
            this.Password = Password;
        }
        public User(string UserId, string UserName, string FirstName, string LastName)
        {
            this.UserId = UserId;
            this.UserName = UserName;
            this.FirstName = FirstName;
            this.LastName = LastName;          
        }
     
        public User(string Token,string UserId, string UserName, string FirstName, string LastName,string DivCode)
        {
            this.Token = Token;
            this.UserId = UserId;
            this.UserName = UserName;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.DivCode = DivCode;
        }
        public User(string Token, string UserName, string FirstName, string LastName, string DivCode)
        {
            this.Token = Token;          
            this.UserName = UserName;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.DivCode = DivCode;
        }
        public User(string UserName, string FirstName, string LastName)
        {
            this.UserName = UserName;
            this.FirstName = FirstName;
            this.LastName = LastName;
        }
        public User(string Token)
        {
            this.Token = Token;
        }         
    }
   
    public class UserProfile
    {
        public string RoleToken { set; get; }
        public string Token { set; get; }
        public string UserName { set; get; }
        public string DivCode { set; get; }
        public string UserId { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public Result aResult;   
      
        public UserProfile(string UserName, string RoleToken, string Token,string DivCode)
        {
            this.UserName = UserName;
            this.Token = Token;
            this.RoleToken = RoleToken;
            this.DivCode = DivCode;
        }

        public UserProfile(string UserId, string UserName, string FirstName, string LastName, string DivCode)
        {
            this.UserId = UserId;
            this.UserName = UserName;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.DivCode = DivCode;
        }

    }

    public class UserProfileList
    {
        public List<UserProfile> rows { get; set; } 
        public UserProfileList()
        {
            rows = new List<UserProfile>(); 
        } 
    }

    public class UsernameList
    {
        public List<string> rows { get; set; }
        public UsernameList(string Term)
        {
            rows = new List<string>();
            SqlConnection aSqlConnection = new Utility().getDbConnection();
            try
            {
                SqlCommand aSqlCommand = new SqlCommand();
                aSqlConnection.Open();
                aSqlCommand.Connection = aSqlConnection;
                aSqlCommand.CommandType = CommandType.StoredProcedure;
                aSqlCommand.CommandText = "GET_USERNAME_AUTO";
                aSqlCommand.Parameters.AddWithValue("TERM", Term); 
                SqlDataReader aSqlDataReader = aSqlCommand.ExecuteReader();
                while(aSqlDataReader.Read())
                {
                    rows.Add(Convert.ToString(aSqlDataReader["USERNAME"]));
                }

            }
            catch (Exception ex)
            { 
            }
            finally
            {
                aSqlConnection.Close();
            }
        }

    }

    public class UserRepository
    {
        public User aUser;
        public UserProfile aUserProfile;   
        public Result aResult;
        public string SessionId;
        public string DelToken;
        public string DelUserId;      
        public string AccessDivisionNameForDdl { set; get; }
        public UserProfileList aUserProfileList;
        public Utility Util = Utility.Instance;
        public UserRepository() { }
        public UserRepository(string UserName, string Password, string SessionId)
        {
            aUser = new User(UserName, Password);
            this.SessionId = SessionId;
        }
        public UserRepository(string Token)
        {
            aUser = new User(Token);           
        }
        public UserRepository(string Token, string UserId)
        {
            aUser = new User(Token,UserId);
        }
        public UserRepository(string Token, string UserName, string FirstName, string LastName,string DivCode)
        {
            aUser = new User( Token, UserName,FirstName,LastName, DivCode);
        }
        public UserRepository(string Token,string UserId, string UserName, string FirstName, string LastName, string DivCode)
        {
            aUser = new User(Token,UserId, UserName, FirstName, LastName, DivCode);
        }

        public void GetFullName()
        { 
            SqlConnection aSqlConnection = Util.getDbConnection();
            try
            {
                SqlCommand aSqlCommand = new SqlCommand();
                aSqlConnection.Open();
                aSqlCommand.Connection = aSqlConnection;
                aSqlCommand.CommandType = CommandType.StoredProcedure;
                aSqlCommand.CommandText = "GET_FULL_NAME";
                aSqlCommand.Parameters.AddWithValue("TOKEN", aUser.Token);
                var resultParameter = aSqlCommand.Parameters.Add("@RESULT", SqlDbType.VarChar);
                resultParameter.Size = 2000;
                resultParameter.Direction = ParameterDirection.Output;
                var retcodeParameter = aSqlCommand.Parameters.Add("@RETCODE", SqlDbType.Int);
                retcodeParameter.Size = 5;
                retcodeParameter.Direction = ParameterDirection.Output;

                SqlDataReader aSqlDataReader = aSqlCommand.ExecuteReader();
                aSqlDataReader.Read();

                if (Convert.ToInt16(aSqlCommand.Parameters["@RETCODE"].Value) == 0)
                {
                    aResult = new Result(Convert.ToString(resultParameter), 0);
                }
                else
                {
                    aResult = new Result(Convert.ToString(resultParameter), -1);
                }

            }
            catch (Exception ex)
            {
                aResult = new Result(ex.ToString(), -1);
            }
            finally
            {
                aSqlConnection.Close();
            }
             
        }
        public void Login()
        {
            SqlConnection aSqlConnection = Util.getDbConnection();
            try
            {
                SqlCommand aSqlCommand = new SqlCommand();
                aSqlConnection.Open();
                aSqlCommand.Connection = aSqlConnection;
                aSqlCommand.CommandType = CommandType.StoredProcedure;
                aSqlCommand.CommandText = "LOGIN_PROC";
                aSqlCommand.Parameters.AddWithValue("SESSION_ID", this.SessionId);
                aSqlCommand.Parameters.AddWithValue("USERNAME", aUser.UserName);
                aSqlCommand.Parameters.AddWithValue("PASSWORD", null);
                var resultParameter = aSqlCommand.Parameters.Add("@RESULT", SqlDbType.VarChar);
                resultParameter.Size = 2000;
                resultParameter.Direction = ParameterDirection.Output;
                var retcodeParameter = aSqlCommand.Parameters.Add("@RETCODE", SqlDbType.Int);
                retcodeParameter.Size = 5;
                retcodeParameter.Direction = ParameterDirection.Output;

                SqlDataReader aSqlDataReader = aSqlCommand.ExecuteReader();
                aSqlDataReader.Read();

                if (Convert.ToInt16(aSqlCommand.Parameters["@RETCODE"].Value) == 0)
                {
                    string[] Result = Convert.ToString(aSqlCommand.Parameters["@RESULT"].Value).Split('/');
                    aUserProfile = new UserProfile(aUser.UserName, Result[1], Result[0],Result[2]);
                    aResult = new Result("Login Success !", 0);
                }
                else
                {
                    aResult = new Result(Convert.ToString(aSqlCommand.Parameters["@RESULT"].Value) , -1);
                }

            }
            catch (Exception ex)
            {
                aResult = new Result(ex.ToString(), -1);
            }
            finally
            {
                aSqlConnection.Close();
            }

        }
        
        public void GetUserList()
        {
            SqlConnection aSqlConnection = Util.getDbConnection();
            try
            {
                SqlCommand aSqlCommand = new SqlCommand();
                aSqlConnection.Open();
                aSqlCommand.Connection = aSqlConnection;
                aSqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                aSqlCommand.CommandText = "GET_USER_ROLENAMES";
                aSqlCommand.Parameters.AddWithValue("Token", aUser.Token); 
                SqlDataReader aSqlDataReader = aSqlCommand.ExecuteReader();
               
                aUserProfileList = new UserProfileList();
                aUserProfileList.rows = new List<UserProfile>();
            
                while (aSqlDataReader.Read())
                {
                    UserProfile aUser_listRecord = new UserProfile(
                                                      Convert.ToString(aSqlDataReader["UserId"]),
                                                      Convert.ToString(aSqlDataReader["UserName"]),
                                                      Convert.ToString(aSqlDataReader["FirstName"]),
                                                      Convert.ToString(aSqlDataReader["LastName"]),
                                                       Convert.ToString(aSqlDataReader["Div_Code"]));
                    aUserProfileList.rows.Add(aUser_listRecord);
                }
            }
            catch (Exception ex)
            {
                aResult = new Result(ex.ToString(), -1);
            }
            finally
            {
                aSqlConnection.Close();
            }
        }

        public void Insert_User()
        {
            SqlConnection aSqlConnection = Util.getDbConnection();
            try
            {
                SqlCommand aSqlCommand = new SqlCommand();
                aSqlConnection.Open();
                aSqlCommand.Connection = aSqlConnection;
                aSqlCommand.CommandType = CommandType.StoredProcedure;
                aSqlCommand.CommandText = "SAVE_USER_ROLES";
                aSqlCommand.Parameters.AddWithValue("@TOKEN", aUser.Token);
                aSqlCommand.Parameters.AddWithValue("@USER_NAME", aUser.UserName);
                aSqlCommand.Parameters.AddWithValue("@FIRST_NAME", aUser.FirstName);
                aSqlCommand.Parameters.AddWithValue("@LAST_NAME", aUser.LastName);
                aSqlCommand.Parameters.AddWithValue("@DIV_CODE", aUser.DivCode);
                var resultParameter = aSqlCommand.Parameters.Add("@RESULT", SqlDbType.VarChar);
                resultParameter.Size = 2000;
                resultParameter.Direction = ParameterDirection.Output;
                var retcodeParameter = aSqlCommand.Parameters.Add("@RETCODE", SqlDbType.Int);
                retcodeParameter.Size = 5;
                retcodeParameter.Direction = ParameterDirection.Output;
                SqlDataReader aSqlDataReader = aSqlCommand.ExecuteReader();
                aSqlDataReader.Read();
                aResult = new Result(Convert.ToString(aSqlCommand.Parameters["@RESULT"].Value), Convert.ToInt16(aSqlCommand.Parameters["@RETCODE"].Value));
            }
            catch (Exception ex)
            {
                aResult = new Result(ex.ToString(), -1);
            }
            finally
            {
                aSqlConnection.Close();
            }
        }

        public void Update_User()
        {
            SqlConnection aSqlConnection = Util.getDbConnection();
            try
            {
                SqlCommand aSqlCommand = new SqlCommand();
                aSqlConnection.Open();
                aSqlCommand.Connection = aSqlConnection;
                aSqlCommand.CommandType = CommandType.StoredProcedure;
                aSqlCommand.CommandText = "UPDATE_USER_MANAGE_ROLE";
                aSqlCommand.Parameters.AddWithValue("@TOKEN", aUser.Token);
                aSqlCommand.Parameters.AddWithValue("@ID", aUser.UserId);
                aSqlCommand.Parameters.AddWithValue("@USER_NAME", aUser.UserName);
                aSqlCommand.Parameters.AddWithValue("@FIRST_NAME", aUser.FirstName);
                aSqlCommand.Parameters.AddWithValue("@LAST_NAME", aUser.LastName);
                aSqlCommand.Parameters.AddWithValue("@DIV_CODE", aUser.DivCode);
                var resultParameter = aSqlCommand.Parameters.Add("@RESULT", SqlDbType.VarChar);
                resultParameter.Size = 2000;
                resultParameter.Direction = ParameterDirection.Output;
                var retcodeParameter = aSqlCommand.Parameters.Add("@RETCODE", SqlDbType.Int);
                retcodeParameter.Size = 5;
                retcodeParameter.Direction = ParameterDirection.Output;
                SqlDataReader aSqlDataReader = aSqlCommand.ExecuteReader();
                aSqlDataReader.Read();
                aResult = new Result(Convert.ToString(aSqlCommand.Parameters["@RESULT"].Value), Convert.ToInt16(aSqlCommand.Parameters["@RETCODE"].Value));
            }
            catch (Exception ex)
            {
                aResult = new Result(ex.ToString(), -1);
            }
            finally
            {
                aSqlConnection.Close();
            }
        }

        public void Delete_User()
        {
            SqlConnection aSqlConnection = Util.getDbConnection();
            try
            {
                SqlCommand aSqlCommand = new SqlCommand();
                aSqlConnection.Open();
                aSqlCommand.Connection = aSqlConnection;
                aSqlCommand.CommandType = CommandType.StoredProcedure;
                aSqlCommand.CommandText = "DELETE_USER_ROLE_MANAGEMENT";
                aSqlCommand.Parameters.AddWithValue("@TOKEN", DelToken);
                aSqlCommand.Parameters.AddWithValue("@ID", DelUserId);
                var resultParameter = aSqlCommand.Parameters.Add("@RESULT", SqlDbType.VarChar);
                resultParameter.Size = 2000;
                resultParameter.Direction = ParameterDirection.Output;
                var retcodeParameter = aSqlCommand.Parameters.Add("@RETCODE", SqlDbType.Int);
                retcodeParameter.Size = 5;
                retcodeParameter.Direction = ParameterDirection.Output;
                SqlDataReader aSqlDataReader = aSqlCommand.ExecuteReader();
                aSqlDataReader.Read();
                aResult = new Result(Convert.ToString(aSqlCommand.Parameters["@RESULT"].Value), Convert.ToInt32(aSqlCommand.Parameters["@RETCODE"].Value));
            }
            catch (Exception ex)
            {
                aResult = new Result(ex.ToString(), -1);
            }
            finally
            {
                aSqlConnection.Close();
            }
        }
    }
}