using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Network.Images
{

    public class ImageService : IImageService
    {
        NetworkEntities context = new NetworkEntities();

        //private string ServerDirectory = "h:/root/home/vasya18-001/www/site1";
        // path
        private string ServerDirectory = "D:/Network/users";

        public ImageClass UploadImage(byte[] buffer, string name, int id_user, string access_token)
        {
            ImageClass response = new ImageClass();
            try
            {
                Auth.AuthService auth_service = new Auth.AuthService();
                Auth.Auth auth_object = new Auth.Auth();
                auth_object = auth_service.Authentication(access_token, id_user);
                
                if (auth_object.access == true)
                {
                    //generate name
                    string current_name = Guid.NewGuid().ToString().Substring(0, 25).Replace("-", "");
                    //urlid
                    string urlid = context.Peoples.Where(p => p.id == id_user).FirstOrDefault().urlid;

                    Picture image = new Picture();
                    Image file;
                    Bitmap bmp;

                    image.id_owner = id_user;
                    image.start_name = name;
                    image.current_name = current_name;
                    image.date_upload = DateTime.Now;
                    image.deleted = false;
                    string path = "\\" + urlid + "\\" + "images" + "\\" + current_name;
                    File.WriteAllBytes(ServerDirectory + path + ".jpg", buffer);
                    buffer = null;
                    file = Image.FromFile(ServerDirectory + path + ".jpg");
                    bmp = new Bitmap(file, 200, 200);
                    bmp.Save(ServerDirectory + path + "_200" + ".jpg", ImageFormat.Jpeg);
                    //other variants 
                    //..

                    image.url = path + ".jpg";
                    context.Pictures.Add(image);
                    context.SaveChanges();

                    response.image_url = image.url;
                    response.image_id = context.Pictures.Where(p => p.url == image.url).FirstOrDefault().id;
                    return response;
                }
                else
                {
                    response.exception = "Access not allowed";
                    return response;
                }

            }
            catch (Exception ex)
            {
                response.exception = ex.Message;
                return response;
            }
            
        }

        public ImageClass UploadMiniature(byte[] buffer, int id_image, string urlid, int id_user, string access_token)
        {
            ImageClass response = new ImageClass();

            try
            {
                Auth.AuthService auth_service = new Auth.AuthService();
                Auth.Auth auth_object = new Auth.Auth();
                auth_object = auth_service.Authentication(access_token, id_user);
                if (auth_object.access == true)
                {
                    Picture image = context.Pictures.Where(p => p.id == id_image).FirstOrDefault();
                    if (image != null)
                    {
                        string current_name = DateTime.Now.ToString("yyyy") + Guid.NewGuid() + DateTime.Now.ToString("MMddHHmmssfff");
                        string path = ServerDirectory + "\\" + urlid + "\\" + "images" + "\\" + current_name + "_min";

                        File.WriteAllBytes(path + ".jpg", buffer);
                        image.have_miniature = true;

                        context.SaveChanges();

                        response.image_url = path + ".jpg";
                        response.image_id = id_image;
                        response.exception = null;
                    }
                    else
                    {
                        response.exception = "404. File does not exist or deleted.";
                    }

                    return response;
                }
                else
                {
                    response.exception = "Access not allowed";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.exception = ex.Message;
                return response;
            }
        }

        public ImageClass UploadAvatar(byte[] buffer, string name, int id_user, string access_token)
        {
            ImageClass response = new ImageClass();
            try
            {
                response = UploadImage(buffer, name, id_user, access_token);
                if (response.image_id != 0 && response.image_url != null)
                {
                    Album album = context.Albums.Where(p => p.id_owner == id_user).Where(p => p.name == "Avatars").FirstOrDefault();
                    string photos = album.photos;
                    photos = response.image_id + ", " + photos;
                    album.photos = photos;
                    context.SaveChanges();
                }
                else
                {
                    response.exception = "Album error";
                }
                return response;
            }
            catch (Exception ex)
            {
                response.exception = ex.Message;
                return response;
            }
        }

        ////                                                     ///
        ///////////////////////// GET METHODS //////////////////////
        ///                                                      ///
        ///                                                      

        public string GetImageUrlById(int id_image)
        {
            try
            {
                Picture image = context.Pictures.Find(id_image);
                return image.url;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Picture GetImageById(int id_image)
        {
            Picture image = new Picture();
            try
            {
                
                return image;
            }
            catch (Exception ex)
            {
                return image;
            }
        }
    }
}
