using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Network.Peoples.Registration
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RegistrationService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RegistrationService.svc or RegistrationService.svc.cs at the Solution Explorer and start debugging.
    public class RegistrationService : IRegistrationService
    {
        NetworkEntities context = new NetworkEntities();

 

        //private string ServerDirectory = "h:/root/home/vasya18-001/www/site1";
        private string ServerDirectory = "D:/Network";
        public string Registration(string urlid, int community_id, string name, string surname, bool gender, DateTime date_birth, string email, string password)
        {
            try
            {
                People registerObject = new People();
                People buf = context.Peoples.Where(p => p.urlid == urlid).FirstOrDefault();
                if (buf == null)
                {
                    registerObject.urlid = urlid;
                    registerObject.date_registration = DateTime.Now;
                    registerObject.community = community_id;
                    registerObject.name = name;
                    registerObject.surname = surname;
                    registerObject.gender = gender;
                    registerObject.date_birth = date_birth;
                    registerObject.email = email;
                    registerObject.password = password;

                    //default parametrs
                    registerObject.confirm_email = false;
                    registerObject.deleted = false;
                    registerObject.avatars = "0";

                    context.Peoples.Add(registerObject);
                    context.SaveChanges();

                    Album album = new Album();
                    album.name = "Avatars";
                    album.date_creation = DateTime.Now;
                    album.id_owner = context.Peoples.Where(p => p.urlid == urlid).FirstOrDefault().id;
                    context.Albums.Add(album);
                    context.SaveChanges();

                    Directory.CreateDirectory(ServerDirectory + "\\" + urlid);
                    Directory.CreateDirectory(ServerDirectory + "\\" + urlid + "\\" + "images");
                    Directory.CreateDirectory(ServerDirectory + "\\" + urlid + "\\" + "documents");
                    Directory.CreateDirectory(ServerDirectory + "\\" + urlid + "\\" + "videos");

                    Directory.CreateDirectory(ServerDirectory + "\\" + urlid + "\\" + "images" + "\\" + "avatars");

                    return "OK";
                }
                else
                {
                    return "URLID error";
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
