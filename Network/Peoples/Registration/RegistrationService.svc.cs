﻿using System;
using System.Collections.Generic;
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

        public string DoWork(string test)
        {
            Error er = new Error();
            er.message = "sa";
            er.time = DateTime.Now;
            er.da = false;
            context.Errors.Add(er);
            context.SaveChanges();
            return test;
        }
 

        public string Registration(string urlid, int community_id, string name, string surname, bool gender, DateTime date_birth, string email, string password)
        {
            try
            {
                People registerObject = new People();
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

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
