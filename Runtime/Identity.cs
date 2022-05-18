using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Desonity
{
    public class IdentityScopes
    {
        // No signing and submitting of transactions 
        public static string READ_ONLY = "READ_ONLY";
        // Signing and submitting of transactions using seedHex provided in code, will sign using the flask backend
        public static string READ_WRITE_LOCAL_SEED = "READ_WRITE_LOCAL_SEED";
        // Signing and submitting of transactions using Identity window from the flask backend
        // UNSUPPORTED AS ON NOW, SEEMS COMPLICATED~
        public static string READ_WRITE_IDENTITY = "READ_WRITE_IDENTITY";
    }
    public class Identity
    {
        /* [DllImport("__Internal")] */
        /* private static extern void callLogin(string objectName, string functionName); */
        /* [DllImport("__Internal")] */
        /* private static extern void approveTxn(string objectName, string functionName, string txnHex); */
        public string backendURL = null;
        public string appName = null;
        private string PublicKeyBase58Check = null;
        private string seedHex = null;
        private string scope;

        /* #if UNITY_WEBGL && !UNITY_EDITOR */
        /*         private bool useWebGL = true; */
        /* #else */
        /*         private bool useWebGL = false; */
        /* #endif */
        /* private string objectName; */
        /* private string loginFunction; */



        public Identity(string PublicKeyBase58Check)
        {
            this.PublicKeyBase58Check = PublicKeyBase58Check;
            // Scope is READ_ONLY by default if only Public Key is passed
            this.scope = IdentityScopes.READ_ONLY;

        }

        /* public Identity(string appName, string backendUrl) */
        /* { */
        /*     this.appName = UnityWebRequest.EscapeURL(appName); */
        /*     this.backendURL = backendUrl; */
        /*     // Scope is READ_ONLY by default if nothing is passed */
        /*     this.scope = IdentityScopes.READ_ONLY; */
        /* } */

        public Identity(string PublicKeyBase58Check, string seedHex)
        {
            this.appName = UnityWebRequest.EscapeURL(appName);
            this.PublicKeyBase58Check = PublicKeyBase58Check;
            this.seedHex = seedHex;
            // Scope is READ_WRITE_LOCAL_SEED by default is you pass public key and seed hex
            this.scope = IdentityScopes.READ_WRITE_LOCAL_SEED;
        }


        public void setLoginURL(string backendURL, string appName)
        {
            this.backendURL = backendURL;
            this.appName = appName;
        }

        /* public void setOptions(string objectName, string loginFunction) */
        /* { */
        /*     this.objectName = objectName; */
        /*     this.loginFunction = loginFunction; */
        /* #if UNITY_WEBGL && !UNITY_EDITOR */
        /*             useWebGL = true; */
        /* #else */
        /*     useWebGL = false; */
        /*     Debug.Log("Platform is not WebGL, Seperate Backend will be used instead. If you plan to build for webGL ignore this message."); */
        /* #endif */
        /* } */

        public string getScope()
        {
            return this.scope;
        }

        private async Task<Response> checkLoggedIn(string keyUrl, string uuid)
        {
            string postData = "{\"uuid\":\"" + uuid + "\"}";
            int remainingTries = 60;
            int cooldownSeconds = 2;
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
                        await new WaitForSeconds(cooldownSeconds);
                    }
                    else
                    {
                        returnedKey = uwr.downloadHandler.text;
                        break;
                    }
                }
                remainingTries--;
                Debug.Log("waiting for login (" + remainingTries + ")");
            }
            if (remainingTries == 0 && (returnedKey == null || returnedKey == ""))
            {
                return new Response
                {
                    json = JObject.Parse("{\"error\":\"Login Timed Out\"}"),
                    statusCode = 0
                };
            }
            else
            {
                return new Response
                {
                    json = JObject.Parse("{\"PublicKeyBase58Check\":\"" + returnedKey + "\"}"),
                    statusCode = 200
                };
            }
        }

        public async void Login()
        {
            /* if (this.useWebGL) */
            /* { */
            /*     callLogin(this.objectName, this.loginFunction); */
            /* } */
            /* else */
            {
                if (this.backendURL == null)
                {
                    throw new Exception("Backend URL was not passed");
                }
                if (this.appName == null)
                {
                    throw new Exception("App Name was not passed");
                }
                if (this.scope != null && this.PublicKeyBase58Check != null && this.seedHex != null)
                {
                    throw new Exception("Identity already initialized with scope " + this.scope);
                }
                Guid myuuid = Guid.NewGuid();
                string myuuidAsString = myuuid.ToString();

                Application.OpenURL(backendURL + "/login/" + myuuidAsString + "?appname=" + appName);

                Response response = await checkLoggedIn(keyUrl: (backendURL + "/getKey"), uuid: myuuidAsString);
                if (response.statusCode == 200 && response.json["PublicKeyBase58Check"] != null)
                {
                    this.PublicKeyBase58Check = (string)response.json["PublicKeyBase58Check"];
                    this.scope = IdentityScopes.READ_ONLY;
                    /* GameObject.Find(this.objectName).SendMessage(this.loginFunction, this.PublicKeyBase58Check); */
                }
                else
                {
                    throw new Exception("Error " + response.statusCode + " while logging in: " + response.json["error"]);
                }
            }
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

        public string getSignedTxn(string txnHex)
        {
            if (this.scope == IdentityScopes.READ_ONLY)
            {
                throw new Exception("Identity must me initialised with a scope of READ_WRITE_LOCAL_SEED to sign transactions.");
            }
            else if (this.scope == IdentityScopes.READ_WRITE_LOCAL_SEED)
            {
                return Desonity.Sign.SignTransaction(this.seedHex, txnHex);
            }
            else if (this.scope == IdentityScopes.READ_WRITE_IDENTITY)
            {
                throw new Exception("Signing transactions for users with Identity is not supported yet.");
            }
            else
            {
                throw new Exception("Unknown Identity scope");
            }
        }

        public async Task<Response> submitTxn(string txn)
        {
            string signed = this.getSignedTxn(txn);
            if (signed == "NO_ERR")
            {
                return new Response
                {
                    json = JObject.Parse("{\"status\":\"FUNCTION_WILL_BE_CALLED_MANUALLY\"}"),
                    statusCode = 200
                };
            }
            if (signed != null && signed != "")
            {
                var endpointClass = new Endpoints.SubmitTransaction
                {
                    TransactionHex = signed
                };
                string postData = JsonConvert.SerializeObject(endpointClass);
                Response response = await Route.POST("/submit-transaction", postData);
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
