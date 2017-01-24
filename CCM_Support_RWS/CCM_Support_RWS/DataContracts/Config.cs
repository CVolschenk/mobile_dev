using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Text;

namespace CCM_Support_RWS
{
    [DataContract]
    public class Config
    {
        public Config(string Client)
        {
            this.Client = Client;
        }
        #region Properties

        [DataMember]
        public string Client { get; set; }

        private List<ActionType> actionType = new List<ActionType>();

        [DataMember]
        public List<ActionType> ActionType
        {
            get
            {
                return actionType;
            }
            set
            {
                actionType = value;
            }
        }
        #endregion
    }

    public partial class ConfigItems
    {
        private static readonly ConfigItems RR = new ConfigItems();

        private ConfigItems() { }

        public static ConfigItems Instance
        {
            get { return RR; }
        }

        //Connect to SQL
        public List<Config> Execute(string ConnectionString, string SqlCommand)
        {
            List<Config> clientList = new List<Config>();
            Dictionary<string, Config> dik = new Dictionary<string, Config>();
            
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
                using (SqlCommand command = new SqlCommand(SqlCommand, con))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!dik.ContainsKey(reader.GetString(1)))
                        {
                            var j = new Config(reader.GetString(1));
                            var m = new ActionType(reader.GetInt32(0), reader.GetString(2), reader.GetString(3), reader.GetString(4), EncodeBase64(reader.GetString(5)));
                            j.ActionType.Add(m);

                            dik.Add(reader.GetString(1), j);
                        }
                        else
                        {
                            //var n = new ActionType(reader.GetString(1), reader.GetString(2), reader.GetString(3));
                            var n = new ActionType(reader.GetInt32(0), reader.GetString(2), reader.GetString(3), reader.GetString(4), EncodeBase64(reader.GetString(5)));

                            dik[reader.GetString(1)].ActionType.Add(n);

                        }
                    }
                }
            }

            foreach (var item in dik)
            {
                clientList.Add(item.Value);
            }

            return clientList;
        }
        public string EncodeBase64(string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input)).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
    }
}