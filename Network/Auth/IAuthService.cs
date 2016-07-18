using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Network.Auth
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAuthService" in both code and config file together.
    [ServiceContract]
    public interface IAuthService
    {
        [OperationContract]
        Auth Authorization(string email, string password, DateTime time);

        [OperationContract]
        string Test(string email, string password, DateTime time);

        [OperationContract]
        Auth UpdateToken(Auth auth_inf);
    }
}
