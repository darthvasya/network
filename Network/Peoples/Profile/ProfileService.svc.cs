using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Network.Peoples.Profile
{
    public class ProfileService : IProfileService
    {
        NetworkEntities context = new NetworkEntities();

        public bool EditStatus(int id_user, string access_token, string status)
        {
            try
            {
                bool auth = isAuth(id_user, access_token);
                if (!auth)
                    return false;
                else
                {
                    People user = context.Peoples.Where(p => p.id == id_user).FirstOrDefault();
                    if (user == null)
                        return false;
                    else
                    {
                        user.status = status;
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

        public bool EditPhone(int id_user, string access_token, string phone)
        {
            try
            {
                bool auth = isAuth(id_user, access_token);
                if (!auth)
                    return false;
                else
                {
                    People user = context.Peoples.Where(p => p.id == id_user).FirstOrDefault();
                    if (user == null)
                        return false;
                    else
                    {
                        user.mobile_phone = phone;
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

        public bool DeletePage(int id_user, string access_token)
        {
            try
            {
                bool auth = isAuth(id_user, access_token);
                if (!auth)
                    return false;
                else
                {
                    People user = context.Peoples.Where(p => p.id == id_user).FirstOrDefault();
                    if (user == null)
                        return false;
                    else
                    {
                        user.deleted = true;
                        user.date_delete = DateTime.Now;
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

        public bool RestorePage(int id_user, string access_token)
        {
            try
            {
                bool auth = isAuth(id_user, access_token);
                if (!auth)
                    return false;
                else
                {
                    People user = context.Peoples.Where(p => p.id == id_user).FirstOrDefault();
                    if (user == null)
                        return false;
                    else
                    {
                        user.deleted = false;
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

        public bool EditSocial(int id_user, string access_token, string skype, string instagram, string twitter, string vkontakte)
        {
            try
            {
                bool auth = isAuth(id_user, access_token);
                if (!auth)
                    return false;
                else
                {
                    People user = context.Peoples.Where(p => p.id == id_user).FirstOrDefault();
                    if (user == null)
                        return false;
                    else
                    {
                        user.skype = skype;
                        user.instagram = instagram;
                        user.twitter = twitter;
                        user.vkontakte = vkontakte;
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

        public bool EditLanguage(int id_user, string access_token, List<int> languages)
        {
            try
            {
                bool auth = isAuth(id_user, access_token);
                if (!auth)
                    return false;
                else
                {
                    People user = context.Peoples.Where(p => p.id == id_user).FirstOrDefault();
                    if (user == null)
                        return false;
                    else
                    {
                        //
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
        
        public bool EditCommentWall(int id_user, string access_token, bool wall_comments)
        {
            try
            {
                bool auth = isAuth(id_user, access_token);
                if (!auth)
                    return false;
                else
                {
                    People user = context.Peoples.Where(p => p.id == id_user).FirstOrDefault();
                    if (user == null)
                        return false;
                    else
                    {
                        user.wall_comments = wall_comments;
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
