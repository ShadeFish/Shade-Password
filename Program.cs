using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ConsoleTables;
using System.Drawing;
using Console = Colorful.Console;

namespace ShadePass
{
    public static class MainClass
    {
        public static string DATA_FILENAME = "data.bin";
        private static SecurityController securityController = new SecurityController();
        public static void Main()
        {
            Console.WriteAscii("SHADE PASSWORD", Color.Tomato);

            string password = string.Empty;
            if(!File.Exists(DATA_FILENAME))
            {
                Console.WriteLine("Creating new database.",Color.AliceBlue);
                password = EnterPassword("Set Password: ");
                Console.ResetColor();
                DataFileManager.CreateEmptyCryptedFile(DATA_FILENAME, password);
                Console.WriteLine("New database [" + DATA_FILENAME + "] was created successfuly.",Color.GreenYellow);
                mainLoop(password);
            }
            else
            {
                while(true)
                {
                    password = EnterPassword("Enter Password: ");
                    Console.ResetColor();
                    string data = securityController.Decrypt(password, File.ReadAllBytes(DATA_FILENAME));
                    if(data != null) { break; }
                    Console.WriteLine("Wrong Password!", Color.Tomato);
                }
                mainLoop(password);
            }
        }

        private static string EnterPassword(string prefix)
        {
            Console.Write(prefix, Color.Yellow);
            Console.BackgroundColor = Color.Black;
            Console.ForegroundColor = Console.BackgroundColor;
            return Console.ReadLine();
        }

        public static void mainLoop(string password)
        {
            while(true)
            {
                Console.Write("$>", Color.Yellow);
                string[] cmd = Console.ReadLine().Split(' ');

                switch(cmd[0])
                {
                    case "list":
                        cmd_list(password);
                        break;

                    case "export":
                        cmd_export(cmd[1], password);
                        break;

                    case "update":
                        cmd_update(new DataRow(cmd[1], cmd[2], cmd[3]), password);
                        break;

                    case "add":
                        if(cmd.Length > 2) { cmd_add(new DataRow(cmd[1], cmd[2], cmd[3]), password); }
                        break;

                    case "del":
                        if(cmd.Length > 1) { cmd_del(new DataRow(cmd[1], String.Empty, String.Empty), password); }
                        break;

                    case "help":
                        Console.WriteLine("list - list data in table");
                        Console.WriteLine("export [filename] - export dato to file");
                        Console.WriteLine("update [name] [login] [password] - update single row");
                        Console.WriteLine("add [name] [login] [password] - add new row");
                        Console.WriteLine("del [name] - del row");
                        break;
                }
            }
        }

        public static void cmd_list(string password)
        {
            DataFile dataFile = DataFileManager.DecryptAndReadDataFile(DATA_FILENAME, password);
            var table = new ConsoleTable("Name", "Login", "Password");
            foreach (DataRow row in dataFile.Rows)
            {
                if(row.Name != string.Empty)
                {
                    table.AddRow(row.Name, row.Login, row.Password);
                }  
            }
            table.Write();
        }

        public static void cmd_export(string fileName,string password)
        {
            DataFile dataFile = DataFileManager.DecryptAndReadDataFile(DATA_FILENAME, password);
            var table = new ConsoleTable("Name", "Login", "Password");
            foreach (DataRow row in dataFile.Rows)
            {
                if(row.Name != string.Empty)
                {
                    table.AddRow(row.Name, row.Login, row.Password);
                }
            }
            File.WriteAllText(fileName,table.ToString());
        }

        public static void cmd_update(DataRow dataRow,string password)
        {
            DataFile dataFile = DataFileManager.DecryptAndReadDataFile(DATA_FILENAME,password);
            try
            {
                DataRow row = dataFile.Rows.Single(x => x.Name == dataRow.Name);
                dataFile.Rows[dataFile.Rows.IndexOf(row)] = dataRow;
                DataFileManager.CryptAndSaveDataFile(DATA_FILENAME, password, dataFile);
            }
            catch { Console.WriteLine("Wrong Row Name", Color.Tomato); }
        }
        public static void cmd_add(DataRow dataRow,string password)
        {
            DataFile dataFile = DataFileManager.DecryptAndReadDataFile(DATA_FILENAME, password);
            dataFile.Rows.Add(dataRow);
            DataFileManager.CryptAndSaveDataFile(DATA_FILENAME, password, dataFile);
        }

        public static void cmd_del(DataRow dataRow,string password)
        {
            DataFile dataFile = DataFileManager.DecryptAndReadDataFile(DATA_FILENAME,password); 
            dataFile.Rows.RemoveAll(x => x.Name == dataRow.Name);
            if(dataFile.Rows.Count == 0) { dataFile.Rows.Add(DataRow.Empty()); }
            DataFileManager.CryptAndSaveDataFile(DATA_FILENAME,password, dataFile);
        }
    }
}
