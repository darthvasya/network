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

        [OperationContract]
        bool DeletePage(int id_user, string access_token);

        [OperationContract]
        bool RestorePage(int id_user, string access_token);

        [OperationContract]
        bool EditSocial(int id_user, string access_token, string skype, string instagram, string twitter, string vkontakte);

        [OperationContract]
        bool EditCommentWall(int id_user, string access_token, bool wall_comments);
    }
}
