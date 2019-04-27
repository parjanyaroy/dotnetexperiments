using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SkypeIOT
{
    class RequestGenerator
    {
        public static string authenticationKey = "a20b8948b5b240edad1b38810b1ef4ef";
        public static bool postRequest(string skypeStatus)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://io.adafruit.com/api/v2/parjanyaroy/feeds/skypeiot/data");
            var postData = "value="+skypeStatus;
            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.Headers["X-AIO-Key"] = authenticationKey;
            using (var stream = request.GetRequestStream()){stream.Write(data, 0, data.Length);}
            var response = (HttpWebResponse)request.GetResponse();
            if(response.StatusCode!= HttpStatusCode.OK) { return false; }
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            return true;
        }
    }
}
