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
        ////                                                     ///
        ///////////////////////// UPLOAD METHODS //////////////////////
        ///                                                      ///
        ///                                                      
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
        ///////////////////////// CREATE METHODS //////////////////////
        ///                                                      ///
        public Album CreateAlbum(string name, int id_user, string access_token)
        {
            Album album = new Album();
            try
            {
                bool auth = isAuth(id_user, access_token);
                if (!auth)
                    return album;
                else
                {
                    album = context.Albums.Where(p => p.id_owner == id_user).Where(p => p.name == name).FirstOrDefault();
                    string urlid = context.Peoples.Where(p => p.id == id_user).FirstOrDefault().urlid;
                    if (album == null)
                    {
                        album = new Album();
                        album.id_owner = id_user;
                        album.name = name;
                        album.date_creation = DateTime.Now;

                        context.Albums.Add(album);
                        context.SaveChanges();
                        //Directory.CreateDirectory(ServerDirectory + "\\" + urlid + "\\" + "images" + "\\" + album.name);
                    }
                    else
                    {
                        return album;
                    }

                    return album;
                }
            }
            catch (Exception ex)
            {
                return album;
            }
            
            
        }

        public Album AddImageToAlbum(byte[] image, string name, int id_album, int id_user, string access_token)
        {
            Album album = null;
            try
            {
                album = context.Albums.Where(p => p.id == id_album).FirstOrDefault();
                ImageClass response = UploadImage(image, name, id_user, access_token);
                if (response.image_id != 0 || album != null)
                {
                    string photos = album.photos;
                    photos = response.image_id + "," + photos;
                    album.photos = photos;
                    context.SaveChanges();
                    return album;
                }
                else
                    return album;
            }
            catch
            {
                return album;
            }
            
        }
        ////                                                     ///
        ///////////////////////// DELETE METHODS //////////////////////
        ///                                                      ///
        ///                                                      

        public bool DeleteImage(int id_image, int id_user, string access_token)
        {
            try
            {
                if (isAuth(id_user, access_token))
                {
                    Picture image = context.Pictures.Where(p => p.id == id_image).Where(p => p.id_owner == id_user).FirstOrDefault();
                    if (image == null)
                        return false;
                    else
                    {
                        image.deleted = true;
                        image.date_delete = DateTime.Now;
                        context.SaveChanges();
                        return true;
                    }
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

        public bool DeleteImageFromAlbum(int id_image, int id_album, int id_user, string access_token)
        {
            try
            {
                bool auth = isAuth(id_user, access_token);
                if (!auth)
                    return false;
                else
                {
                    Album album = context.Albums.Where(p => p.id == id_album).FirstOrDefault();
                    if (album == null)
                        return false;
                    else
                    {
                        string photos = album.photos;
                        if (photos == "")
                            return false;
                        else
                        {
                            List<int> photos_list = photos.Split(',').Select(int.Parse).ToList();
                            photos_list.Remove(id_image);
                            photos = string.Join(",", photos_list.ToArray()); 
                            album.photos = photos;
                            context.SaveChanges();
                            return true;
                        }
                    }
                }
            }
            catch
            {
                return false;
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
                Picture image = context.Pictures.FirstOrDefault(p => p.id == id_image);
                if (image == null || image.deleted == true)
                {
                    return "url/image/404jpg";
                }
                else
                {
                    return image.url;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public Picture GetImageById(int id_image)
        {
            Picture image = context.Pictures.FirstOrDefault(p => p.id == id_image);
            try
            {
                if (image == null || image.deleted == true)
                {
                    return new Picture();
                }
                else
                {
                    return image;
                }
            }
            catch (Exception ex)
            {
                return image;
            }
        }

        ////                                                     ///
        ///////////////////////// AUTH METHOD //////////////////////
        ///                                                      ///
        ///                                                      

        private bool isAuth(int id_user, string access_token)
        {
            try
            {
                Auth.AuthService service = new Auth.AuthService();
                Auth.Auth authObj = service.Authentication(access_token, id_user);
                if (authObj.access == true)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
