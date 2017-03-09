using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCM_Support
{
    public class ActionType
    {
        public ActionType(int Indno, string Type, string Description, string Url, string Params)
        {
            this.Indno = Indno;
            this.Type = Type;
            this.Description = Description;
            this.Url = Url;
            this.Params = Params;
        }
        public int Indno { get; set; }
        public string Type { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string Params { get; set; }
    }
}
