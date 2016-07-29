using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace Network.Images.Posts
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PhotoPostService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PhotoPostService.svc or PhotoPostService.svc.cs at the Solution Explorer and start debugging.
    public class PhotoPostService : IPhotoPostService
    {
        NetworkEntities context = new NetworkEntities();

        public PhotoPost CreatePhotoPost(int id_user, string access_token, int id_image)
        {
            PhotoPost post = new PhotoPost();
            try
            {
                bool auth = isAuth(id_user, access_token);
                if (!auth)
                    return null;
                else
                {
                    post.id_owner = id_user;
                    post.id_image = id_image;
                    post.date_creation = DateTime.Now;
                    post.deleted = false;
                    context.PhotoPosts.Add(post);
                    context.SaveChanges();

                    return post;
                }
            }
            catch
            {
                return null;
            }
        }

        public bool DeletePhotoPost(int id_user, string access_token, int id_post)
        {
            try
            {
                bool auth = isAuth(id_user, access_token);
                if (!auth)
                    return false;
                else
                {
                    PhotoPost post = context.PhotoPosts.Where(p => p.id == id_post).FirstOrDefault();
                    post.deleted = true;
                    post.date_delete = DateTime.Now;
                    context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
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
