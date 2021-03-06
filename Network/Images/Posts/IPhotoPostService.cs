﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Network.Images.Posts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPhotoPostService" in both code and config file together.
    [ServiceContract]
    public interface IPhotoPostService
    {
        [OperationContract]
        PhotoPost CreatePhotoPost(int id_user, string access_token, int id_image, string description);

        [OperationContract]
        PhotoPost EditPhotoDescription(int id_user, string access_token, string description);

        [OperationContract]
        bool DeletePhotoPost(int id_user, string access_token, int id_post);

        [OperationContract]
        bool AddLike(int id_user, string access_token, int id_post);

        [OperationContract]
        PhotoComment AddComment(int id_user, string access_token, int id_post, string comment, List<int> photos);

        [OperationContract]
        bool AddCommentLike(int id_user, string access_token, int id_comment);

        [OperationContract]
        bool DeleteComment(int id_user, string access_token, int id_comment);
    }
}
