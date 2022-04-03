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
        private Identity identity;
        public JObject json;
        public string PublicKeyBase58Check;
        public string Username;
        public string Description;
        public bool IsVerified;
        public string AvatarUrl;
        public bool IsBlacklisted;
        public bool IsGraylisted;
        public JObject CoinEntryJson;
        public JObject DAOCoinEntryJson;
        public long CoinPriceDeSoNanos;


        public Profile(Identity identity)
        {
            this.identity = identity;
        }

        public async Task<Response> getProfile()
        {
            string endpoint = "/get-single-profile";
            var endpointClass = new Endpoints.getSingleProfile
            {
                PublicKeyBase58Check = this.identity.getPublicKey()
            };
            string postData = JsonConvert.SerializeObject(endpointClass);

            Route route = new Route();
            Response response = await route.POST(endpoint, postData);
            this.json = response.json;
            this.Username = (string)response.json["Profile"]["Username"];
            this.Description = (string)response.json["Profile"]["Description"];
            this.IsVerified = (bool)response.json["Profile"]["IsVerified"];
            this.AvatarUrl = route.getRoute() + "/get-single-profile-picture/" + this.PublicKeyBase58Check + "?fallback=https://bitclout.com/assets/img/default_profile_pic.png";
            this.IsBlacklisted = (bool)response.json["IsBlacklisted"];
            this.IsGraylisted = (bool)response.json["IsGraylisted"];
            this.CoinEntryJson = (JObject)response.json["Profile"]["CoinEntry"];
            this.DAOCoinEntryJson = (JObject)response.json["Profile"]["DAOCoinEntry"];
            this.CoinPriceDeSoNanos = (long)response.json["Profile"]["CoinPriceDeSoNanos"];
            return response;
        }

        public async Task<Response> getNftsForUser(Identity identity = null, Nullable<bool> forSale = null)
        {
            // forSale:true  -> only nfts for sale
            // forSale:false -> only nfts not for sale
            // forSale:null  -> all owned nfts
            var nft = new Nft(this.identity);
            if (identity == null)
            {
                identity = this.identity;
            }
            Response response = await nft.getNftsForUser(identity, forSale);
            return response;
        }

        public async Task<Response> createPost(string body, bool IsHidden = false)
        {
            if (this.identity.getScope() == Desonity.IdentityScopes.READ_ONLY)
            {
                throw new Exception("Cannot create post with scope " + this.identity.getScope());
            }
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
            Response submitPostResponse = await route.POST(endpoint, postData);
            if (submitPostResponse.statusCode == 200)
            {
                JObject json = submitPostResponse.json;
                string TransactionHex = (string)JObject.Parse((string)json.SelectToken("Response")).SelectToken("TransactionHex");
                Response response = await route.signAndSubmitTxn(TransactionHex, identity);
                if (response.statusCode == 200)
                {
                    return response;
                }
                else
                {
                    throw new Exception("Error " + submitPostResponse.statusCode + " while submitting signed transaction: " + submitPostResponse.json.ToString());
                }
            }
            else
            {
                throw new Exception("Error " + submitPostResponse.statusCode + " while creating post transaction: " + submitPostResponse.json.ToString());
            }
        }
    }
}
