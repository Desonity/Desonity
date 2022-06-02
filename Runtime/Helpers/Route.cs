using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using Desonity.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Desonity.Endpoints
{
    [Serializable]
    public class SubmitTransaction
    {
        public string TransactionHex;
    }
}
namespace Desonity
{
    public class Response
    {
        public JObject json { get; set; }
        public JArray array { get; set; }
        public long statusCode { get; set; }
        public string data { get; set; }
    }
    public static class Route
    {
        /* public static string ROUTE = "https://api.desodev.com/api/v0"; */
        public static string ROUTE = "https://node.deso.org/api/v0";
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
            using (UnityWebRequest uwr = new UnityWebRequest(ROUTE + endpoint, "POST"))
            {
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

        public static async Task<Response> appendExtraData(string TransactionHex, JObject extraData)
        {
            JObject postData = new JObject();
            postData["TransactionHex"] = TransactionHex;
            postData["ExtraData"] = extraData;
            var response = await POST("/append-extra-data", postData.ToString());
            if (response.statusCode == 200)
            {
                return response;
            }
            else
            {
                throw new Exception("Error " + response.statusCode + " while appending extra data\n" + response.json);
            }
        }


    }
}
