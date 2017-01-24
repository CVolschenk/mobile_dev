using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CCM_Support_RWS.DataContracts
{
    [DataContract]
    public class Token
    {
        [DataMember]
        public string TokenID { get; set; }

        [DataMember]
        public string Type{ get; set; }

        [DataMember]
        public string User { get; set; }

    }
}