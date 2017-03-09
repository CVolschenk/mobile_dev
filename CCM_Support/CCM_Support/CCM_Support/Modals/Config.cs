using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCM_Support
{
    public class Config
    {
        public Config(string Client)
        {
            this.Client = Client;
        }
        #region Properties

        public string Client { get; set; }

        private List<ActionType> actionType = new List<ActionType>();

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
}
