using System.Runtime.Serialization;

namespace CCM_Support_RWS
{
    [DataContract]
    public class ActionType
    {
        public ActionType(int Indno,string Type, string Description, string Url, string Params)
        {
            this.Indno = Indno;
            this.Type = Type;
            this.Description = Description;
            this.Url = Url;
            this.Params = Params;
        }
        [DataMember]
        public int Indno { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public string Params { get; set; }
    }
}