using System;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace Desonity
{
    public class Profile
    {
        private string PublicKeyBase58Check;
        private Identity identity;

        public Profile(string PublicKeyBase58Check)
        {
            this.PublicKeyBase58Check = PublicKeyBase58Check;
        }
        public Profile(Identity identity)
        {
            this.identity = identity;
            this.PublicKeyBase58Check = identity.getPublicKey();
        }

        public async Task<string> getProfile(Action<string> onComplete)
        {
            string endpoint = "/get-single-profile";
            var endpointClass = new Endpoints.getSingleProfile
            {
                PublicKeyBase58Check = this.PublicKeyBase58Check
            };
            string postData = JsonConvert.SerializeObject(endpointClass);

            Route route = new Route();
            string response = await route.POST(endpoint, postData);
            return response;
        }

        public async Task<string> getNftsForUser(string UserPublicKeyBase58Check = null, Nullable<bool> forSale = null)
        {
            // forSale:true  -> only nfts for sale
            // forSale:false -> only nfts not for sale
            // forSale:null  -> all owned nfts
            var nft = new Nft(this.identity);
            if (UserPublicKeyBase58Check == null) { UserPublicKeyBase58Check = this.PublicKeyBase58Check; }
            string response = await nft.getNftsForUser(UserPublicKeyBase58Check, forSale);
            return response;
        }

        public async Task<string> createPost(string body, bool IsHidden = false)
        {
            string endpoint = "/submit-post";
            var endpointClass = new Endpoints.submitPost
            {
                UpdaterPublicKeyBase58Check = this.PublicKeyBase58Check,
                BodyObj = new Endpoints.submitPost.postBody
                {
                    Body = body
                },
                IsHidden = IsHidden
            };
            string postData = JsonConvert.SerializeObject(endpointClass);
            Route route = new Route();
            string submitPostResponse = await route.POST(endpoint, postData);
            JToken json = JToken.Parse(submitPostResponse);
            string TransactionHex = (string)JToken.Parse((string)json.SelectToken("Response")).SelectToken("TransactionHex");
            string response = await route.signAndSubmitTxn(TransactionHex, identity);
            return response;
        }

        public string avatarUrl()
        {
            Route route = new Route();
            return route.getRoute() + "/get-single-profile-picture/" + this.PublicKeyBase58Check + "?fallback=https://bitclout.com/assets/img/default_profile_pic.png";
        }
    }
}
