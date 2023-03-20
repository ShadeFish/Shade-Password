using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace ShadePass
{
    public class DataFileManager
    {
        public static void CreateEmptyCryptedFile(string fileName,string password)
        {
            SecurityController securityController = new SecurityController();
            
            byte[] data = securityController.Encrypt(password,DataRow.Empty().ToJsonString());

            File.WriteAllBytes(fileName, data);
        }

        public static DataFile DecryptAndReadDataFile(string fileName,string password)
        {
            SecurityController securityController = new SecurityController();
            string file = securityController.Decrypt(password, File.ReadAllBytes(MainClass.DATA_FILENAME));
            return new DataFile(file);
        }

        public static void CryptAndSaveDataFile(string fileName,string password,DataFile dataFile)
        {
            SecurityController securityController=new SecurityController();
            byte[] data = securityController.Encrypt(password, dataFile.ToJsonString());
            File.WriteAllBytes(fileName , data);
        }
    }
}
