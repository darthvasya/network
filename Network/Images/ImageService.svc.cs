using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Network.Images
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ImageService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ImageService.svc or ImageService.svc.cs at the Solution Explorer and start debugging.
    public class ImageService : IImageService
    {
        NetworkEntities context = new NetworkEntities();

        public void DoWork()
        {
             
        }

        private string ServerDirectory = "h:/root/home/vasya18-001/www/site1";

        public bool UploadImage(byte[] buffer)
        {
            try
            {
                if (Directory.Exists(ServerDirectory + "\\ImageStorage") == false)
                { Directory.CreateDirectory(ServerDirectory + "\\ImageStorage"); }
                // создаём файл
                File.WriteAllBytes(ServerDirectory + "\\" + "name", buffer);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

    }
}
