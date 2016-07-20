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
       
        private string ServerDirectory = "D:/Network";

        public bool UploadImage(byte[] buffer, string name, int id_user, string access_token)
        {
            try
            {
                Auth.AuthService auth_service = new Auth.AuthService();
                Auth.Auth auth_object = new Auth.Auth();
                auth_object = auth_service.Authentication(access_token, id_user);
                
                if (auth_object.access == true)
                {
                    //generate name
                    string current_name = DateTime.Now.ToString("yyyy") + Guid.NewGuid() + DateTime.Now.ToString("MMddHHmmssfff");
                    //urlid
                    string urlid = context.Peoples.Where(p => p.id == id_user).FirstOrDefault().urlid;

                    Picture image = new Picture();
                    Image file;
                    Bitmap bmp;

                    image.id_owner = id_user;
                    image.start_name = name;
                    image.current_name = current_name;
                    image.date_upload = DateTime.Now;

                    string path = ServerDirectory + "\\" + urlid + "\\" + "images" + "\\" + current_name;
                    File.WriteAllBytes(path + ".jpg", buffer);
                    file = Image.FromFile(path + ".jpg");
                    bmp = new Bitmap(file, 200, 200);
                    bmp.Save(path + "_200" + ".jpg", ImageFormat.Jpeg);
                    //other variants 
                    //..

                    image.url = path + ".jpg";
                    context.Pictures.Add(image);
                    context.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
            
        }

        public bool UploadMiniature(byte[] buffer, int id_image, int urlid, int id_user, string access_token)
        {
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
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}
