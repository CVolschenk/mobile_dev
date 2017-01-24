using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceProcess;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCM_Support_RWS
{
    public class GetServicesState
    {
        private static readonly GetServicesState RD = new GetServicesState();

        private GetServicesState() { }

        public static GetServicesState Instance
        {
            get { return RD; }
        }

        public string Execute(string location, string[] services)
        {
            Table t = new Table();
            t.Attributes.Add("border", "1");
            t.Attributes.Add("bordercolor", "white");
            bool bColumns = true;

            List<ServiceController> serviceControllerList = new List<ServiceController>();

            using (StringWriter sw = new StringWriter())
            {
                TableCell td;
                TableRow tr;

                foreach (string service in services)
                {

                    if (bColumns)
                    {
                        tr = new TableRow();
                        td = new TableCell { Text = "DisplayName" };
                        tr.Cells.Add(td);
                        td = new TableCell { Text = "Status" };
                        tr.Cells.Add(td);


                        //td.BackColor = System.Drawing.Color.DarkBlue;
                        //td.ForeColor = System.Drawing.Color.White;
                        //tr.BackColor = System.Drawing.Color.DarkBlue;
                        //tr.ForeColor = System.Drawing.Color.White;

                        t.Rows.Add(tr);

                        bColumns = false;
                    }

                    var sc = new ServiceController(service.Trim(' '), location);
                    tr = new TableRow();
                    td = new TableCell { Text = sc.DisplayName };
                    tr.Cells.Add(td);
                    td = new TableCell { Text = sc.Status.ToString() };
                    tr.Cells.Add(td);

                    //td.BackColor = System.Drawing.Color.DarkGray;
                    //td.ForeColor = System.Drawing.Color.White;
                    //tr.BackColor = System.Drawing.Color.DarkGray;
                    //tr.ForeColor = System.Drawing.Color.White;

                    t.Rows.Add(tr);
                }
                t.RenderControl(new HtmlTextWriter(sw));

                return EncodeBase64(sw.ToString());
            }
        }
        public string EncodeBase64(string input)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(input.Replace("\t", "").Replace("\n", "").Replace("\r", "").Replace("\\", ""))).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }
    }
}