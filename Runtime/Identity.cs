using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Desonity
{
    public class IdentityScopes
    {
        // No signing and submitting of transactions 
        public static string READ_ONLY = "READ_ONLY";

        // Signing and submitting of transactions using seedHex provided in code, will sign using the flask backend
        public static string READ_WRITE_LOCAL_SEED = "READ_WRITE_LOCAL_SEED";

        // In this case seedHex is stored with the flask app, transactionHex will be sent to backend where it will be signed and returned
        public static string READ_WRITE_BACKEND_SEED = "READ_WRITE_BACKEND_SEED";

        // Signing and submitting of transactions using Identity window from the flask backend
        public static string READ_WRITE_IDENTITY = "READ_WRITE_IDENTITY";
    }
    public class Identity
    {
        public string backendURL = null;
        public string appName = null;
        private string PublicKeyBase58Check = null;
        private string seedHex = null;
        private string scope;

        public Identity(string PublicKeyBase58Check)
        {
            this.PublicKeyBase58Check = PublicKeyBase58Check;
            this.scope = IdentityScopes.READ_ONLY;

        }
        public Identity(string appName, string backendUrl, string scope, string PublicKeyBase58Check = null, string seedHex = null)
        {
            this.appName = UnityWebRequest.EscapeURL(appName);
            this.backendURL = backendUrl;
            this.PublicKeyBase58Check = PublicKeyBase58Check;
            this.scope = scope;
            this.seedHex = seedHex;
            if (scope == IdentityScopes.READ_WRITE_LOCAL_SEED && seedHex == null)
            {
                // If you dont pass seed hex while using READ_WRIE_LOCAL_SEED scope
                throw new Exception("SeedHex is required for scope: " + IdentityScopes.READ_WRITE_LOCAL_SEED);
            }
            if (seedHex != null)
            {
                // If you pass a seedHex, READ_WRITE_LOCAL_SEED will always be used
                this.scope = IdentityScopes.READ_WRITE_LOCAL_SEED;
            }
            if (PublicKeyBase58Check == null && seedHex == null)
            {
                scope = IdentityScopes.READ_ONLY;
            }
        }

        private async Task<string> checkLoggedIn(string keyUrl, string uuid)
        {
            string postData = "{\"uuid\":\"" + uuid + "\"}";
            int remainingTries = 20;
            string returnedKey = null;
            while (remainingTries > 0)
            {
                var uwr = new UnityWebRequest(keyUrl, "POST");
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(postData);
                uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
                uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                uwr.SetRequestHeader("Content-Type", "application/json");

                await uwr.SendWebRequest();

                if (uwr.result == UnityWebRequest.Result.ConnectionError)
                {
                    returnedKey = null;
                    break;
                }
                else
                {
                    if (uwr.downloadHandler.text == "" || uwr.downloadHandler.text == null)
                    {
                        returnedKey = null;
                        await new WaitForSeconds(3);
                    }
                    else
                    {
                        returnedKey = uwr.downloadHandler.text;
                    }
                }
                remainingTries--;
                Debug.Log("waiting for login (" + remainingTries + ")");
            }
            if (remainingTries == 0 && returnedKey == null)
            {
                return null;
            }
            else
            {
                return returnedKey;
            }
        }

        public async Task<string> Login()
        {
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();

            Application.OpenURL(backendURL + "/login/" + myuuidAsString + "?appname=" + appName);

            string loggedInKey = await checkLoggedIn(keyUrl: backendURL + "/getKey", uuid: myuuidAsString);
            this.PublicKeyBase58Check = loggedInKey;
            return loggedInKey;
        }

        public string getPublicKey()
        {
            if (this.PublicKeyBase58Check != null)
            {
                return this.PublicKeyBase58Check;
            }
            Debug.Log("You Must Login Using Identity.Login() before using the Identity object with any other class or method");
            return null;
        }

        public async Task<string> getSignedTxn(string txnHex)
        {
            if (this.scope == IdentityScopes.READ_ONLY)
            {
                throw new Exception("Cannot sign transaction with scope: " + IdentityScopes.READ_ONLY);
            }
            else if (this.scope == IdentityScopes.READ_WRITE_LOCAL_SEED || this.scope == IdentityScopes.READ_WRITE_BACKEND_SEED)
            {
                string postData;
                if (seedHex != null)
                {
                    postData = "{\"txnHex\":\"" + txnHex + "\",\"seedHex\":\"" + seedHex + "\"}";
                }
                else
                {
                    postData = "{\"txnHex\":\"" + txnHex + "\"}";
                }
                var uwr = new UnityWebRequest(backendURL + "/signTxn", "POST");
                byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(postData);
                uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
                uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                uwr.SetRequestHeader("Content-Type", "application/json");

                await uwr.SendWebRequest();

                if (uwr.result == UnityWebRequest.Result.ConnectionError)
                {
                    Debug.Log("Error while getting Signed Txn Hex \n\n" + uwr.error + "\n\n" + postData);
                    return null;
                }
                else
                {
                    string JSONStr = uwr.downloadHandler.text;
                    return JSONStr;
                }
            }
            else if (this.scope == IdentityScopes.READ_WRITE_IDENTITY)
            {
                // WPA
                return null;
            }
            else
            {
                throw new Exception("Cannot sign transaction with scope: " + this.scope);
            }
        }
    }
}
