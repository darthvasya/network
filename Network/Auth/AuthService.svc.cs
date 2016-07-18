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

        // Errors codes
        /*
         *  100 - Login error 
         *  101 - Error with tokens in updating
         *  102 - Authentication error. Incorrect access_token
         */

        public Auth Authorization(string email, string password, DateTime time)
        {
            Auth auth = new Auth();
            try
            {
                People that = context.Peoples.Where(p => p.email == email).Where(p => p.password == password).FirstOrDefault();
                if (that == null)
                {
                    auth.access_token = null;
                    auth.refresh_token = null;
                    auth.exception = "100";
                    return auth;
                }
                else
                {
                    int id_user = that.id;
                    AccessToken token = context.AccessTokens.Where(p => p.id == id_user).FirstOrDefault();

                    if (token == null)
                    {
                        token = new AccessToken();
                        token.id = id_user;
                        token.access_token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                        token.refresh_token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                        token.last_refresh = DateTime.Now;

                        context.AccessTokens.Add(token);
                        context.SaveChanges();
                    }
                    else
                    {
                        token.access_token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                        token.refresh_token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                        token.last_refresh = DateTime.Now;

                        context.SaveChanges();
                    }

                    auth.access_token = token.access_token;
                    auth.refresh_token = token.refresh_token;
                    auth.exception = null;

                    return auth;
                }
            }
            catch (Exception ex)
            {
                auth.access_token = null;
                auth.refresh_token = null;
                auth.exception = ex.Message;

                return auth;
            }
        }

        public Auth UpdateToken(Auth auth_inf)
        {
            Auth auth = new Auth();

            try
            {
                AccessToken token = context.AccessTokens.Where(p => p.refresh_token ==  auth_inf.refresh_token).Where(p => p.access_token == auth_inf.access_token).FirstOrDefault();
                if (token == null)
                {
                    auth.access_token = null;
                    auth.refresh_token = null;
                    auth.exception = "101";
                }
                else
                {
                    auth.access_token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                    auth.refresh_token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                    auth.exception = null;

                    token.access_token = auth.access_token;
                    token.refresh_token = auth.refresh_token;
                    token.last_refresh = DateTime.Now;
                    
                    context.SaveChanges();
                }
                return auth;
            }
            catch (Exception ex)
            {
                auth.exception = ex.Message;
                return auth;
            }
            
        }


        public Auth Authentication(string access_token, int id)
        {
            Auth auth = new Auth();
            try
            {
                AccessToken token = context.AccessTokens.Where(p => p.access_token == access_token).Where(p => p.id == id).FirstOrDefault();
                if (token == null)
                {
                    auth.exception = "102";
                }
                else
                {
                    auth.access = true;
                }
            }
            catch (Exception ex)
            {
                auth.exception = ex.Message;
            }
            return auth;  
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
