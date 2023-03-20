using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShadePass
{
    /* EXAMPLE JSON FILE 
     * 
     * {"Name":"Google","Login":"email","Password":"jebanie"}
     * {"Name":"Google","Login":"email","Password":"jebanie"}
     * {"Name":"Google","Login":"email","Password":"jebanie"}
     * {"Name":"Google","Login":"email","Password":"jebanie"}
     * 
     */
    public class DataFile
    {
        private List<DataRow> rows;
        public List<DataRow> Rows { get { return rows; } set { rows = value; } }

        public DataFile(string jsonString)
        {
            List<DataRow> rows = new List<DataRow>();
            foreach(string jsonStringObject in jsonString.Split("\r\n"))
            {
                DataRow dataRow = JsonConvert.DeserializeObject<DataRow>(jsonStringObject);
                if(dataRow != null)
                {
                    rows.Add(dataRow);
                }
            }
            Rows = rows;
        }

        public string ToJsonString()
        {
            string jsonString = string.Empty;
            foreach(DataRow row in Rows)
            {
                string singleObject = JsonConvert.SerializeObject(row);
                jsonString += singleObject + "\r\n";
            }
            return jsonString;
        }
    }
}
