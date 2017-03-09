using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCM_Support
{
    public class LoginRequest
    {
        public string SecurityModel { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
    }
}
