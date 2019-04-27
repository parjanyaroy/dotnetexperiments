namespace MqttPOC
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using System.Text;
    using uPLibrary.Networking.M2Mqtt;
    using uPLibrary.Networking.M2Mqtt.Messages;

    internal class Program
    {
       

        static void dummyMain()
        {
            Console.WriteLine("***********PUBLISHER***********");

            //MqttClient client = new MqttClient("dev.rabbitmq.com", 1883, false, null);
            MqttClient client = new MqttClient("io.adafruit.com");
            //var state = client.Connect("Client993", "guest", "guest", false, 0, false, null, null, true, 60);
            string clientId = Guid.NewGuid().ToString();
            client.ProtocolVersion = MqttProtocolVersion.Version_3_1;
            var state = client.Connect(clientId, "parjanyaroy", "a20b8948b5b240edad1b38810b1ef4ef");
            int i = 0;
            while (i<10)
            {
                client.MqttMsgPublished += client_MqttMsgPublished;

                ushort msgId = client.Publish("/skypeiot", // topic
                               Encoding.UTF8.GetBytes("TestMessage "+i), // message body
                               MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, // QoS level
                               false); // retained
                i++;
                
            }
        }
        static void client_MqttMsgPublished(object sender, MqttMsgPublishedEventArgs e)
        {
            Debug.WriteLine("MessageId = " + e.MessageId + " Published = " + e.IsPublished);
        }

        static void Main()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://io.adafruit.com/api/v2/parjanyaroy/feeds/skypeiot/ata");
            var postData = "value=hello";
            var data = Encoding.ASCII.GetBytes(postData);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.Headers["X-AIO-Key"] = "a20b8948b5b240edad1b38810b1ef4ef";
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        }
    }

}
