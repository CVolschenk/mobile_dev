using CCM_Support_RWS.DataContracts;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Net.Http.Headers;


namespace CCM_Support_RWS
{
    public class CCM_Support_RESTService : ICCM_SupportRESTService
    {
        public string ConnectionString { get; set; }
        
        #region Constructor
        public CCM_Support_RESTService()
        {
            ConnectionString = @"Password=Sybr!n123;Persist Security Info=True;User ID=CCM_APP;Initial Catalog=GWRobot;Data Source=db.prod.sybgw.local\SQL14";
        }
        #endregion

        #region Implemented
        public string GetServicesState(string Parameter)
        {
            try
            {
                var Param = GetParams(Parameter);
                Logger.WriteLog(string.Format("GetServicesState : {0}", Param), "Info");
                //"10.75.241.31|Sybrin10.ServerChase,Sybrin10.DomainCopier_Chase,Sybrin10.LicenseServer,Sybrin10.MailServervlive,Sybrin10.NoteLoaderChaseV3,Sybrin10.GWNotificationChase Statements"
                //var split = DecodeBase64(Parameter).Split('|');
                var split = Param.Split('|');
                var splitServices = split[1].Split(',');
                return CCM_Support_RWS.GetServicesState.Instance.Execute(split[0], splitServices);
            }
            catch (Exception ex)
            {
                Logger.WriteLog(string.Format("GetServicesState : {0}", ex.Message), "ERROR");
                throw;
            }
        }

        public List<Config> GetConfigList()
        {
            try
            {
                return ConfigItems.Instance.Execute(ConnectionString, string.Format("Select Indno, Client, ActionType, Description, Url, Params from AppConfig Order by Client,ActionType"));
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message, "ERROR");
                throw;
            }
        }

        public string GetReturnData(string Parameter)
        {
            try
            {
                Logger.WriteLog("GetReturnData Started", "Info");
                return ReturnData.Instance.Execute(ConnectionString, GetParams(Parameter));
            }
            catch(Exception ex)
            {
                Logger.WriteLog(ex.Message, "ERROR");
                throw;
            }
        }

        public string RegisterToken(Token token)
        {
            //Register Token in Database
            try
            {
                //string SqlCommand = "INSERT INTO AppToken (Token, Type) Values('" + token.TokenID + "','" + token.Type + "')";
                string SqlCommand = "MERGE AppToken as Target USING (Select '" + token.TokenID + "' as [Token], '" + token.Type + "' as [Type], '" + token.User + "' as [User]) as Source ON Target.[Token] = Source.[Token] and Target.[Type] = Source.Type WHEN NOT MATCHED THEN INSERT ([Token], [Type], [User]) Values('" + token.TokenID + "','" + token.Type + "','" + token.User + "');";
                //string SqlCommand = "Update AppToken set Token = Concat(Token,'_123')";
                //string SqlCommand = "Update AppToken set Token = replace(Token,'_123','')";
                Logger.WriteLog(SqlCommand, "Info");
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    // Open the SqlConnection.
                    con.Open();

                    // The following code uses an SqlCommand based on the SqlConnection.
                    using (SqlCommand command = new SqlCommand(SqlCommand, con))
                    {
                        int rows = command.ExecuteNonQuery();
                        Logger.WriteLog(string.Format("{0} Rows inserted",rows), "Info");

                        if (rows > 0)                      
                            return "true";
                        else
                            return "false";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message, "ERROR");
                throw;
            }
        }

        public string UnRegisterToken(Token token)
        {
            //Register Token in Database
            try
            {
                string SqlCommand = "DELETE AppToken Where Token = '" + token.TokenID + "' and Type = '" + token.Type + "'";
                Logger.WriteLog(SqlCommand, "Info");
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    // Open the SqlConnection.
                    con.Open();

                    // The following code uses an SqlCommand based on the SqlConnection.
                    using (SqlCommand command = new SqlCommand(SqlCommand, con))
                    {
                        int rows = command.ExecuteNonQuery();
                        Logger.WriteLog(string.Format("{0} Rows Deleted", rows), "Info");

                        if (rows > 0)
                            return "true";
                        else
                            return "false";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message, "ERROR");
                throw;
            }
        }

        public string SendNote(string Note)
        {
            var jGcmData = new JObject();
            var jData = new JObject();
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                //
                // Open the SqlConnection.
                //
                con.Open();
                //
                // The following code uses an SqlCommand based on the SqlConnection.
                //
                string SqlCommand = "Select Distinct Token from AppToken";
                string API_KEY = "AIzaSyBCePIvILHTOWxSeknzKkjh7hEArqC-ycs";

                using (SqlCommand command = new SqlCommand(SqlCommand, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        jData.Add("message", Note);
                        jGcmData.Add("to", reader.GetString(0));
                        jGcmData.Add("data", jData);

                        var url = new Uri("https://gcm-http.googleapis.com/gcm/send");
                        try
                        {
                            using (var client = new HttpClient())
                            {
                                client.DefaultRequestHeaders.Accept.Add(
                                    new MediaTypeWithQualityHeaderValue("application/json"));

                                client.DefaultRequestHeaders.TryAddWithoutValidation(
                                    "Authorization", "key=" + API_KEY);

                                Logger.WriteLog(jGcmData.ToString(), "DEBUG");

                                Task.WaitAll(client.PostAsync(url,
                                    new StringContent(jGcmData.ToString(), Encoding.Default, "application/json"))
                                        .ContinueWith(response =>
                                        {
                                            Logger.WriteLog(response.ToString(), "Info");
                                            Logger.WriteLog("Message sent: check the client device notification tray.", "Info");
                                        }));
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.WriteLog("Unable to send GCM message: "+ e.Message.ToString(), "Info");
                        }
                        finally
                        {
                            jGcmData = new JObject();
                            jData = new JObject();
                        }
                    }
                }
            }
            return "True";
        }

        public string GetTest()
        {
            Logger.WriteLog("TEst GET", "Info");
            return "test";
        }

        public string PostTest()
        {
            Logger.WriteLog("Test - POST", "Info");
            return "test";
        }

        #endregion

        #region Custom
        public string DecodeBase64(string input)
        {
            string incoming = input.Replace('_', '/').Replace('-', '+');
            switch (input.Length % 4)
            {
                case 2: incoming += "=="; break;
                case 3: incoming += "="; break;
            }
            byte[] bytes = Convert.FromBase64String(incoming);
            return Encoding.UTF8.GetString(bytes,0,bytes.Length);
        }

        public string GetParams(string Indno)
        {
            try
            {
                string returnVal = string.Empty;
                //Get connection
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    //
                    // Open the SqlConnection.
                    //
                    con.Open();
                    //
                    // The following code uses an SqlCommand based on the SqlConnection.
                    //
                    using (SqlCommand command = new SqlCommand("Select Params from AppConfig where Indno = " + Indno, con))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            returnVal = reader.GetString(0);
                        }
                    }
                }
                return returnVal;
            }

            catch (Exception ex)
            {
                Logger.WriteLog(ex.Message, "ERROR");
                throw;
            }
        }


        #endregion
    }
}
