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

        public PhotoPost CreatePhotoPost(int id_user, string access_token, int id_image, string description)
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
                    post.description = description;
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

        public PhotoPost EditPhotoDescription(int id_user, string access_token, string description)
        {
            PhotoPost post = new PhotoPost();
            try
            {
                bool auth = isAuth(id_user, access_token);
                if (!auth)
                    return null;
                else
                {
                    post.description = description;
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
                    if (post != null && post.id_owner == id_user)
                    {
                        post.deleted = true;
                        post.date_delete = DateTime.Now;
                        context.SaveChanges();
                        return true;
                    }
                    else
                        return false;

                }
            }
            catch
            {
                return false;
            }
        }

        public bool AddLike(int id_user, string access_token, int id_post)
        {
            try
            {
                bool auth = isAuth(id_user, access_token);
                if (!auth)
                    return false;
                else
                {
                    PhotoPost post = context.PhotoPosts.Where(p => p.id == id_post).FirstOrDefault();
                    if (post.deleted == true)
                        return false;
                    string likes = post.likes;
                    if (likes != null && likes != "")
                    {
                        List<int> like_list = likes.Split(',').Select(int.Parse).ToList();
                        if (like_list.Contains(id_user))
                        {
                            like_list.Remove(id_user);
                            post.likes = string.Join(",", like_list.ToArray());
                        }
                        else
                        {
                            like_list.Add(id_user);
                            post.likes = string.Join(",", like_list.ToArray());
                        }
                    }
                    else
                    {
                        post.likes = id_user.ToString();
                    }

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                string es = ex.Message;
               
                return false;
            }
        }

        public PhotoComment AddComment(int id_user, string access_token, int id_post, string comment, List<int> photos)
        {
            PhotoComment ph_com = new PhotoComment();
            PhotoPost ph_post = context.PhotoPosts.Where(p => p.id == id_post).FirstOrDefault();
            try
            {
                bool auth = isAuth(id_user, access_token);
                if (!auth && ph_post == null)
                    return null;
                else
                {
                    ph_com.body = comment;
                    ph_com.id_owner = id_user;
                    ph_com.date_creation = DateTime.Now;
                    ph_com.deleted = false;
                    // 10 photos max in comment
                    if (photos != null && photos.Count <= 10)
                        ph_com.photos = string.Join(",", photos.ToArray());
                
                    context.PhotoComments.Add(ph_com);
                    context.SaveChanges();

                    string comments = ph_post.comments;
                    if (comments != null && comments != "")
                    {
                        List<int> comment_list = comments.Split(',').Select(int.Parse).ToList();
                        comment_list.Add(ph_com.id);
                        ph_post.comments = string.Join(",", comment_list.ToArray());
                    }
                    else
                    {
                        ph_post.comments = ph_com.id.ToString();
                    }
                    context.SaveChanges();
                    return ph_com;
                }
            }
            catch
            {
                return null;
            }
            
        }

        public bool AddCommentLike(int id_user, string access_token, int id_comment)
        {
            try
            {
                bool auth = isAuth(id_user, access_token);
                if (!auth)
                    return false;
                else
                {
                    PhotoComment ph_com = context.PhotoComments.Where(p => p.id == id_comment).FirstOrDefault();
                    if (ph_com == null)
                        return false;
                    else
                    {
                        string likes = ph_com.likes;
                        if (likes != null && likes != "")
                        {
                            List<int> like_list = likes.Split(',').Select(int.Parse).ToList();
                            if (like_list.Contains(id_user))
                            {
                                like_list.Remove(id_user);
                                ph_com.likes = string.Join(",", like_list.ToArray());
                            }
                            else
                            {
                                like_list.Add(id_user);
                                ph_com.likes = string.Join(",", like_list.ToArray());
                            }
                        }
                        else
                        {
                            ph_com.likes = id_user.ToString();
                        }
                        context.SaveChanges();
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteComment(int id_user, string access_token, int id_comment)
        {
            try
            {
                bool auth = isAuth(id_user, access_token);
                if (!auth)
                    return false;
                else
                {
                    PhotoComment ph_com = context.PhotoComments.Where(p => p.id == id_comment).FirstOrDefault();
                    if (ph_com != null && ph_com.id_owner == id_user)
                    {
                        ph_com.deleted = true;
                        ph_com.date_delete = DateTime.Now;
                        context.SaveChanges();
                        return true;
                    }
                    else
                        return false;
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
