using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Network
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IShedule" in both code and config file together.
    [ServiceContract]
    public interface IShedule
    {
        [OperationContract]
        string DoWork();
    }
}
