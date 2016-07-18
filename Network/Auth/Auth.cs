using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Network.Auth
{
    [DataContract]
    public class Auth
    {
        [DataMember]
        public string access_token { get; set; }

        [DataMember]
        public string refresh_token { get; set; }

        [DataMember]
        public string exception { get; set; }

        [DataMember]
        public bool access { get; set; }
    }
}
