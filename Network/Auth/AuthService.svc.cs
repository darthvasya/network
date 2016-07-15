using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Network.Auth
{
    public class AuthService : IAuthService
    {
        NetworkEntities context = new NetworkEntities();
        public DateTime refresh_time = new DateTime();

        public Auth Authorization(string email, string password, DateTime time)
        {
            Auth auth = new Auth();
            People that = context.Peoples.Where(p => p.email == email).Where(p => p.password == password).FirstOrDefault();
            if (that == null)
            {
                auth.access_token = null;
                auth.refresh_token = null;
                auth.exception = "Login error";
                return auth;
            }
            else
            {
                try
                {
                    AccessToken token = context.AccessTokens.Where(p => p.id == that.id).FirstOrDefault();
                    int id_user = that.id;

                    if (token == null)
                    {
                        token.id = id_user;
                        token.access_token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                        token.refresh_token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                        token.last_login = DateTime.Now;
                        token.last_refresh = DateTime.Now;

                        context.AccessTokens.Add(token);
                        context.SaveChanges();

                        auth.access_token = token.access_token;
                        auth.refresh_token = token.refresh_token;
                        return auth;
                    }
                    else
                    {
                        //update token body
                        return auth;
                    }

                }
                catch (Exception ex)
                {
                    auth.exception = ex.Message;
                    return auth;
                }
            }
        }

        public string Test(string email, string password, DateTime time)
        {
            People that = context.Peoples.Where(p => p.email == email).Where(p => p.password == password).FirstOrDefault();
            if (that == null)
                return "False";
            AccessToken thatToken = context.AccessTokens.Where(p => p.id == that.id).FirstOrDefault();
            if (thatToken == null)
                return "Make Add";
            else
            {
                context.AccessTokens.Remove(thatToken);
                context.SaveChanges();
                //update - new add
                string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                return token;
            }
          
        }
    }
}
