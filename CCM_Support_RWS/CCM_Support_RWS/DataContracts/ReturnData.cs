using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCM_Support_RWS
{
    //[DataContract]
    //public class ReturnData
    //{
    //    [DataMember]
    //    public string Data { get; set; }
    //}


    public class ReturnData
    {
        private static readonly ReturnData RR = new ReturnData();

        private ReturnData() { }

        public static ReturnData Instance
        {
            get { return RR; }
        }

        //Connect to SQL
        public string Execute(string ConnectionString, string SqlCommand)
        {
            Logger.WriteLog("ReturnData Execute", "INFO");

            Table t = new Table();
            t.Attributes.Add("border", "1");
            //t.Attributes.Add("BackColor", "Black");
            t.Attributes.Add("BordorColor", "White");
            //t.Attributes.Add("ForeColor", "White");

            //Get connection
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                Logger.WriteLog("REturnData entered", "DEBUG");
                bool bColumns = true;
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
                    using (StringWriter sw = new StringWriter())
                    {
                        while (reader.Read())
                        {
                            TableRow tr = new TableRow();
                            TableRow trC = new TableRow();

                            //tr.BackColor = System.Drawing.Color.DarkGray;
                            //tr.ForeColor = System.Drawing.Color.White;
                            //trC.BackColor = System.Drawing.Color.DarkBlue;
                            //trC.ForeColor = System.Drawing.Color.White;

                            TableCell td;
                            TableCell tdC;

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                if (!reader.IsDBNull(i))
                                {

                                    if (bColumns)
                                    {
                                        Logger.WriteLog(string.Format("Column : {0} ", reader.GetName(i).ToString()), "DEBUG");
                                        tdC = new TableCell { Text = reader.GetName(i).ToString() };

                                        //tdC.BackColor = System.Drawing.Color.DarkBlue;
                                        //tdC.ForeColor = System.Drawing.Color.White;

                                        trC.Cells.Add(tdC);
                                    }

                                    switch (reader.GetFieldType(i).ToString().ToLower().Replace("system.", ""))
                                    {
                                        case "string":
                                            td = new TableCell { Text = reader.GetString(i) };
                                            break;
                                        case "int16":
                                            td = new TableCell { Text = reader.GetInt16(i).ToString() };
                                            break;
                                        case "int32":
                                            td = new TableCell { Text = reader.GetInt32(i).ToString() };
                                            break;
                                        case "int64":
                                            td = new TableCell { Text = reader.GetInt64(i).ToString() };
                                            break;
                                        case "datetime":
                                            td = new TableCell { Text = reader.GetDateTime(i).ToString() };
                                            break;
                                        default:
                                            td = new TableCell { Text = reader.GetString(i) };
                                            break;
                                    }

                                }
                                else
                                {
                                    td = new TableCell { Text = "Null" };
                                }
                                //td.BackColor = System.Drawing.Color.DarkGray;
                                //td.ForeColor = System.Drawing.Color.White;
                                tr.Cells.Add(td);
                            }

                            //Check if we need to add header columns
                            if (bColumns)
                            {
                                t.Rows.Add(trC);
                                bColumns = false;
                                Logger.WriteLog("Columns added", "DEBUG");
                            }
                            t.Rows.Add(tr);
                        }
                        t.RenderControl(new HtmlTextWriter(sw));
                        Logger.WriteLog(sw.ToString(),"DEBUG");
                        return EncodeBase64(sw.ToString());

                        //return sw.ToString();
                    }
                }
            }
        }
        public string EncodeBase64(string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input.Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace("\\", ""))).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
    }
}