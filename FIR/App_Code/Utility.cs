using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace ENVHR_CSHARP.App_Code
{
    public class LogWriter
    {
        public static LogWriter Instance = new LogWriter();
        public static string LogFile = null;
        public static string FileName = null;
        public TextWriter aStreamWriter = null;
        public LogWriter()
        {

        }
        public void SetLogFile(string aLogFile)
        {
            FileName = aLogFile;
            LogFile = FileName + "_" +
                    DateTime.Now.ToString("dd") + "_" +
                    DateTime.Now.ToString("MMM") + "_" +
                    DateTime.Now.ToString("yy") + ".txt";

        }
        public void WriteLog(string message)
        {
            try
            {
                if ((DateTime.Now.Minute % 15) == 0)
                {
                    LogFile = FileName + "_" +
                              DateTime.Now.ToString("dd") + "_" +
                              DateTime.Now.ToString("MMM") + "_" +
                              DateTime.Now.ToString("yy") + ".txt";

                    if (!File.Exists(LogFile))
                        aStreamWriter = new StreamWriter(LogFile);
                }
                aStreamWriter = new StreamWriter(LogFile, true);

                string Prepend = String.Format("{0}:{1}", DateTime.Now.ToString(), message);
                aStreamWriter.WriteLine(Prepend);
                aStreamWriter.Close();
            }
            catch (Exception ex)
            {

            }
        }


    }
    public class Config
    {
        public string getValueFromConfig(string key)
        {
            return ConfigurationManager.AppSettings[key].ToString();
        }
    }
    public class Utility
    {
        public static Utility Instance = new Utility();
        public StreamWriter aStreamWriter = null;

        public string value(string AppSetting)
        {
            return ConfigurationManager.AppSettings[AppSetting].ToString();
        }

        public string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public SqlConnection getDbConnection()
        {
            SqlConnection aSqlConnection = null;
            try
            {
                aSqlConnection = new SqlConnection(new Config().getValueFromConfig("DatabaseConnection"));
                return aSqlConnection;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (aSqlConnection.State == ConnectionState.Open)
                    aSqlConnection.Close();
            }
            return null;
        }

        public SqlConnection getHostDbConnection()
        {
            SqlConnection aSqlConnection = null;
            try
            {
                aSqlConnection = new SqlConnection(new Config().getValueFromConfig("HostDB"));
                return aSqlConnection;
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return null;
        }

        public SqlConnection getDataSourceDbConnection(string Server, string database, string UserName, string Password)
        {
            SqlConnection aSqlConnection = null;
            try
            {
                string aConnectionString = String.Format("DataSource={0};Database={1};User Id={2};Password={3}", Server, database, UserName, Password);
                aSqlConnection = new SqlConnection(aConnectionString);
                return aSqlConnection;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //  if (aSqlConnection.State == ConnectionState.Open)
                //      aSqlConnection.Close();
            }
            return null;
        }

        public void UpdateConfig(string key, string value)
        {
            try
            {
                Configuration myConfiguration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                myConfiguration.AppSettings.Settings[key].Value = value; // custom value.
                myConfiguration.Save();
            }
            catch (Exception ex)
            {

            }
        }
        public SqlConnection getReportDbConnection(string ConnectionString)
        {
            SqlConnection aSqlConnection = null;
            try
            {
                aSqlConnection = new SqlConnection(ConnectionString);
                return aSqlConnection;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //  if (aSqlConnection.State == ConnectionState.Open)
                //      aSqlConnection.Close();
            }
            return null;
        }



    }
}