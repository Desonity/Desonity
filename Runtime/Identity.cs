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
        // Signing and submitting transactions through derived keys
        public static string READ_WRITE_DERIVED = "READ_WRITE_DERIVED";
    }

    public class Identity
    {
        public string backendURL = null;
        public string appName = null;
        private string PublicKeyBase58Check = null;
        private string seedHex = null;
        private string scope;
        private string DerivedPublicKey = null;
        private string DerivedSeedHex = null;

        public Identity() { }

        public Identity(string PublicKeyBase58Check)
        {
            this.PublicKeyBase58Check = PublicKeyBase58Check;
            // Scope is READ_ONLY by default if only Public Key is passed
            this.scope = IdentityScopes.READ_ONLY;
        }

        public Identity(string PublicKeyBase58Check, string seedHex)
        {
            this.appName = UnityWebRequest.EscapeURL(appName);
            this.PublicKeyBase58Check = PublicKeyBase58Check;
            this.seedHex = seedHex;
            // Scope is READ_WRITE_LOCAL_SEED by default is you pass public key and seed hex
            this.scope = IdentityScopes.READ_WRITE_LOCAL_SEED;
        }

        public Identity(string PublicKeyBase58Check, string DerivedPublicKey, string DerivedSeedHex)
        {
            this.scope = IdentityScopes.READ_WRITE_DERIVED;
            this.PublicKeyBase58Check = PublicKeyBase58Check;
            this.DerivedSeedHex = DerivedSeedHex;
            this.DerivedPublicKey = DerivedPublicKey;
        }

        public void setLoginURL(string backendURL, string appName)
        {
            this.backendURL = backendURL;
            this.appName = appName;
        }

        public string getScope()
        {
            return this.scope;
        }

        private async Task<Response> checkLoggedIn(string keyUrl, string uuid)
        {
            string postData = "{\"uuid\":\"" + uuid + "\"}";
            int remainingTries = 40;
            int cooldownSeconds = 3;
            string returnedData = null;
            int failedAttempts = 0;
            while (remainingTries > 0)
            {
                using (UnityWebRequest uwr = new UnityWebRequest(keyUrl, "POST"))
                {
                    byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(postData);
                    uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
                    uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                    uwr.SetRequestHeader("Content-Type", "application/json");

                    await uwr.SendWebRequest();

                    if (uwr.result == UnityWebRequest.Result.ConnectionError)
                    {
                        returnedData = null;
                        failedAttempts++;
                        break;
                    }
                    else
                    {
                        if (uwr.downloadHandler.text == "" || uwr.downloadHandler.text == null)
                        {
                            returnedData = null;
                            await new WaitForSeconds(cooldownSeconds);
                        }
                        else
                        {
                            returnedData = uwr.downloadHandler.text;
                            break;
                        }
                    }
                    remainingTries--;
                    /* Debug.Log("waiting for login (" + remainingTries + ")"); */
                }
            }
            if (remainingTries == 0 && (returnedData == null || returnedData == ""))
            {
                throw new Exception("Login timed out");
            }
            else if (failedAttempts > 0)
            {
                throw new Exception("Login failed due to one or more connection errors");
            }
            else
            {
                return new Response
                {
                    /* json = JObject.Parse("{\"PublicKeyBase58Check\":\"" + returnedKey + "\"}"), */
                    json = JObject.Parse(returnedData),
                    statusCode = 200
                };
            }
        }

        public async Task Login(bool derive = false)
        {

            if (this.backendURL == null)
            {
                throw new Exception("Backend URL was not passed");
            }
            if (this.appName == null)
            {
                throw new Exception("App Name was not passed");
            }
            if (this.scope != null && this.PublicKeyBase58Check != null && (this.seedHex != null || this.DerivedSeedHex != null))
            {
                throw new Exception("Identity already initialized with scope " + this.scope);
            }
            Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();

            string url = this.backendURL + "/login?uuid=" + myuuidAsString + "&appname=" + System.Uri.EscapeUriString(this.appName) + "&derive=" + derive.ToString().ToLower();
            /* Debug.Log("Login URL: " + url); */
            Application.OpenURL(url);

            Response response = await checkLoggedIn(keyUrl: (backendURL + "/getKey"), uuid: myuuidAsString);
            if (response.statusCode == 200 && response.json["publicKey"] != null)
            {
                /* Debug.Log(response.json); */
                this.PublicKeyBase58Check = (string)response.json["publicKey"];
                this.scope = IdentityScopes.READ_ONLY;
                if (response.json["derivedKey"] != null)
                {
                    this.DerivedPublicKey = (string)response.json["derivedKey"];
                    this.DerivedSeedHex = (string)response.json["derivedSeed"];
                    this.scope = IdentityScopes.READ_WRITE_DERIVED;
                }

            }
            else
            {
                throw new Exception("Error " + response.statusCode + " while logging in: " + response.json["error"]);
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

        public async Task<string> getSignedTxn(string txnHex)
        {
            if (this.scope == IdentityScopes.READ_ONLY)
            {
                throw new Exception("Identity must me initialised with a scope of READ_WRITE_LOCAL_SEED to sign transactions.");
            }
            else if (this.scope == IdentityScopes.READ_WRITE_LOCAL_SEED)
            {
                return Desonity.Sign.SignTransaction(this.seedHex, txnHex);
            }
            else if (this.scope == IdentityScopes.READ_WRITE_DERIVED)
            {
                JObject extraData = new JObject();
                extraData["DerivedPublicKey"] = this.DerivedPublicKey;
                var response = await Route.appendExtraData(txnHex, extraData);
                string appendedTxnHex = (string)response.json["TransactionHex"];
                return Desonity.Sign.SignTransaction(this.DerivedSeedHex, appendedTxnHex);
            }
            else
            {
                throw new Exception("Unknown Identity scope");
            }
        }

        public async Task<Response> submitTxn(string txn)
        {
            string signed = await this.getSignedTxn(txn);
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
