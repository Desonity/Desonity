using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using Desonity.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Desonity
{
    public class Response
    {
        public JObject json { get; set; }
        public JArray array { get; set; }
        public long statusCode { get; set; }
    }
    public static class Route
    {
        public static string ROUTE = "https://api.desodev.com/api/v0";
        public static string getRoute()
        {
            return ROUTE;
        }
        public static void setRoute(string newRoute)
        {
            ROUTE = newRoute;
        }

        public static async Task<Response> POST(string endpoint, string postData)
        {
            var uwr = new UnityWebRequest(ROUTE + endpoint, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(postData);
            uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            await uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError)
            {
                throw new Exception("Connection Error");
            }
            else
            {
                Response returnThis = new Response
                {
                    json = JObject.Parse(uwr.downloadHandler.text),
                    statusCode = uwr.responseCode
                };
                return returnThis;
            }
        }


    }
}
