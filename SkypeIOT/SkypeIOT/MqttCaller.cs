using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIOT
{
    class MqttCaller
    {
        private static string MacId= null;
        public static string adafruitUserName = "parjanyaroy";
        public static string autheticationKey = "a20b8948b5b240edad1b38810b1ef4ef";

        public static void setMacId()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            MacId = sMacAddress;
        }

        public static string getMacId()
        {
            return MacId;
        }

        public static void checkFeedExist()
        {
            string html = string.Empty;
            string url = @"https://io.adafruit.com/api/v2/"+adafruitUserName+ "feeds/";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("X-AIO-Key:"+ autheticationKey);

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }

            Console.WriteLine(html);

        }






    }
}
