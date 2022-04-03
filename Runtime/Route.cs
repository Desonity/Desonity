using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Desonity
{
    public class Response
    {
        public JObject json { get; set; }
        public JArray array { get; set; }
        public long statusCode { get; set; }
    }
    public class Route
    {
        public string ROUTE = "https://bitclout.com/api/v0";
        public string getRoute()
        {
            return ROUTE;
        }
        public void setRoute(string newRoute)
        {
            ROUTE = newRoute;
        }

        public async Task<Response> POST(string endpoint, string postData)
        {
            var uwr = new UnityWebRequest(ROUTE + endpoint, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(postData);
            uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            await uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError)
            {
                var returnThis = new Response
                {
                    json = JObject.Parse("{\"error\":\"ConnectionError\"}"),
                    statusCode = uwr.responseCode
                };
                return returnThis;
            }
            else
            {
                var returnThis = new Response
                {
                    json = JObject.Parse(uwr.downloadHandler.text),
                    statusCode = uwr.responseCode
                };
                return returnThis;
            }
        }

        public async Task<Response> signAndSubmitTxn(string txn, Identity identity)
        {
            string signed = await identity.getSignedTxn(txn);
            if (signed != null && signed != "")
            {
                var endpointClass = new Endpoints.submitTransaction
                {
                    TransactionHex = signed
                };
                string postData = JsonConvert.SerializeObject(endpointClass);
                Response response = await POST("/submit-transaction", postData);
                return response;
            }
            else
            {
                return new Response
                {
                    json = JObject.Parse("{\"error\":\"No signed transaction\"}"),
                    statusCode = 0
                };
            }
        }
    }
}