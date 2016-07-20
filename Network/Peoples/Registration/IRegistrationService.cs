using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Network.Peoples.Registration
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRegistrationService" in both code and config file together.
    [ServiceContract]
    public interface IRegistrationService
    {
        [OperationContract]
        string Registration(string urlid, int community_id, string name, string surname, bool gender, DateTime date_birth, string email, string password);

    }
}
