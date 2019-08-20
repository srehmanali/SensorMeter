using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SensorMeter.Domain
{
    public class Message
    {
        public int SensorId { get; set; }
        public string Key { get; set; }
        public decimal Pressure { get; set; }
        public DateTime Time { get; set; }

        public Message()
        {
        }
        public Message(string msg)
        {
            Parse(msg);
        }
        public Message(byte[] bytes)
        {
            var msgStr = GetString(bytes);
            Parse(msgStr);
        }

        public override string ToString()
        {
            return "<" + SensorId + "," + Key + "," + Pressure + "," + Time.Ticks + ">";
        }

        public byte[] GetBytes()
        {
            var str = ToString();
            return ASCIIEncoding.ASCII.GetBytes(str);
        }
        public static byte[] GetBytes(string str)
        {
            return ASCIIEncoding.ASCII.GetBytes(str);
        }
        public string GetString(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes, 0, bytes.Length);
        }
        private void Parse(string str)
        {
            try
            {
                var msgStartIdx = str.IndexOf("<");
                var msgEndIdx = str.IndexOf(">");
                var msgArray = str.Substring(msgStartIdx + 1, msgEndIdx - msgStartIdx - 1).Split(',');
                SensorId = Convert.ToInt32(msgArray[0]);
                Key = msgArray[1];
                Pressure = Convert.ToDecimal(msgArray[2]);
                //Time = Convert.ToDateTime(msgArray[3]);
                Time = new DateTime(long.Parse(msgArray[3]));
            }
            catch { }
        }
        public int Insert()
        {
            var ds = new DataService();
            var ht = new Hashtable()
            {
                {"RowCount", 0},
                {"JobDetailID", Key},
                {"Pressure", Pressure},
                {"Time", Time}
            };
            ht = ds.ExecuteStoredProcedure(ht, "InsertMessage", new string[1] { "RowCount" });
            return Convert.ToInt32(ht["RowCount"].ToString());
        }
    }
}