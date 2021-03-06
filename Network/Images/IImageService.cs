﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Network.Images
{
    [ServiceContract]
    public interface IImageService
    {
        [OperationContract]
        ImageClass UploadImage(byte[] buffer, string name, int id_user, string access_token);

        [OperationContract]
        ImageClass UploadMiniature(byte[] buffer, int id_image, string urlid, int id_user, string access_token);

        [OperationContract]
        ImageClass UploadAvatar(byte[] buffer, string name, int id_user, string access_token);

        [OperationContract]
        string GetImageUrlById(int id_image);

        [OperationContract]
        Picture GetImageById(int id_image);

        [OperationContract]
        bool DeleteImage(int id_image, int id_user, string access_token);

        [OperationContract]
        Album CreateAlbum(string name, int id_user, string access_token);

        [OperationContract]
        Album AddImageToAlbum(byte[] image, string name, int id_album, int id_user, string access_token);

        [OperationContract]
        bool DeleteImageFromAlbum(int id_album, int id_alb_photo, int id_user, string access_token);
    }
}
