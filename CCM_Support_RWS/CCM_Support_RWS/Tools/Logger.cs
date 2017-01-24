using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace CCM_Support_RWS
{
    public static class Logger
    {
        public static void WriteLog(string Message, string Level)
        {
            //string _path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "CCM_Support.log");

            string sSource;
            string sLog;
            string sEvent;
            sSource = "CCM Support App";
            sLog = "Application";

            try
            {
                string _path = @"C:\CCM_Support.log";
                File.AppendAllText(_path, string.Format("{0} \t ({1}) \t {2} {3}", DateTime.Now, Level, Message, Environment.NewLine));

                //event log

                sEvent = Message;

                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, sEvent);
                EventLog.WriteEntry(sSource, sEvent,
                EventLogEntryType.Warning, 234);
            }
            catch (Exception ex)
            {
                //event log
                sEvent = ex.Message;

                if (!EventLog.SourceExists(sSource))
                    EventLog.CreateEventSource(sSource, sLog);

                EventLog.WriteEntry(sSource, sEvent);
                EventLog.WriteEntry(sSource, sEvent,
                EventLogEntryType.Warning, 234);

                throw;
            }
            

            //try
            //{
            //    using (StringWriter sw = new StringWriter())
            //    {
            //        HttpResponse response = new HttpResponse(sw);
            //        response.AppendToLog(string.Format("{0} \t ({1}) \t {2} {3}", DateTime.Now, Level, Message, Environment.NewLine));
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
            //try
            //{
            //    using (SqlConnection con = new SqlConnection(@"Password=Sybrin;Persist Security Info=True;User ID=SybrinDotNetServer;Initial Catalog=GWRobot;Data Source=10.75.241.22\SQL14"))
            //    {
            //        // Open the SqlConnection.
            //        con.Open();

            //        // The following code uses an SqlCommand based on the SqlConnection.
            //        using (SqlCommand command = new SqlCommand(string.Format("Insert into AppLogs Values (SYSDATETIME(),'{0}','{1}')", Level, Message), con))
            //        {
            //            command.ExecuteNonQuery();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    File.AppendAllText(@"C:\CCM_Support.log", string.Format("{0} \t ({1}) \t {2} {3}", DateTime.Now, Level, ex.Message, Environment.NewLine));
            //    throw;
            //}
        }
    }
}