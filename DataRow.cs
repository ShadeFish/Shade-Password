using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShadePass
{
    public class DataRow
    {
        private string _name;
        private string _login;
        private string _password;

        public string Name { get { return _name; } set { _name = value; } }
        public string Login { get { return _login; } set { _login = value; } }
        public string Password { get { return _password; } set { _password = value; } }

        public DataRow(string name, string login, string password)
        {
            Name = name;
            Login = login;
            Password = password;
        }

        public override string ToString()
        {
            return "Name: " + _name + " Login: " + _login + " Password: " + Password;
        }

        public static DataRow Empty()
        {
            return new DataRow(string.Empty, string.Empty, string.Empty);
        }

        public string ToJsonString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
