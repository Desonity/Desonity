using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Desonity
{
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

        public async Task<string> POST(string endpoint, string postData)
        {
            var uwr = new UnityWebRequest(ROUTE + endpoint, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(postData);
            uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json");

            await uwr.SendWebRequest();

            string returnThis;
            if (uwr.result == UnityWebRequest.Result.ConnectionError)
            {
                var error = new Endpoints.errorMessage
                {
                    Error = uwr.error
                };
                returnThis = JsonConvert.SerializeObject(error);
            }
            else
            {
                var success = new Endpoints.successMessage
                {
                    Response = uwr.downloadHandler.text
                };
                returnThis = JsonConvert.SerializeObject(success);
            }
            return returnThis;
        }

        public async Task<string> signAndSubmitTxn(string txn, Identity identity)
        {
            string signed = await identity.getSignedTxn(txn);
            if (signed != null && signed != "")
            {
                var endpointClass = new Endpoints.submitTransaction
                {
                    TransactionHex = signed
                };
                string postData = JsonConvert.SerializeObject(endpointClass);
                string response = await POST("/submit-transaction", postData);
                return response;
            }
            else
            {
                return null;
            }
        }
    }
}