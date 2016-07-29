using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Network.Peoples.Profile
{
      
    [ServiceContract]
    public interface IProfileService
    {
        [OperationContract]
        bool EditStatus(int id_user, string access_token, string status);

        [OperationContract]
        bool EditPhone(int id_user, string access_token, string phone);
    }
}
