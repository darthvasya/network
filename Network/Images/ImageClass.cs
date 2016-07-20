using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Network.Images
{
    [DataContract]
    public class ImageClass
    {
        [DataMember]
        public string image_url { get; set; }

        [DataMember]
        public int image_id { get; set; }

        [DataMember]
        public string exception { get; set; }
    }
}
