using System;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Desonity.Endpoints;

namespace Desonity
{
    public class Nft
    {
        private string ReaderPublicKeyBase58Check;
        private Identity ReaderIdentity;

        public Nft(Identity ReaderIdentity)
        {
            this.ReaderIdentity = ReaderIdentity;
            this.ReaderPublicKeyBase58Check = ReaderIdentity.getPublicKey();
        }

        public async Task<Response> getSingleNft(string PostHashHex)
        {
            string endpoint = "/get-nft-entries-for-nft-post";
            var endpointClass = new Endpoints.getNftEntriesForNftPost
            {
                PostHashHex = PostHashHex,
                ReaderPublicKeyBase58Check = this.ReaderPublicKeyBase58Check
            };
            string postData = JsonConvert.SerializeObject(endpointClass);

            Response response = await Route.POST(endpoint, postData);
            if (response.statusCode == 200)
            {
                JArray NFTarray = JArray.Parse(response.json["NFTEntryResponses"].ToString());
                return new Response
                {
                    array = NFTarray,
                    statusCode = response.statusCode
                };
            }
            else
            {
                return response;
            }

        }

        public async Task<Response> getNftsForUser(Identity identity, Nullable<bool> forSale = null)
        {

            // forSale:true  -> only nfts for sale
            // forSale:false -> only nfts not for sale
            // forSale:null  -> all owned nfts

            string endpoint = "/get-nfts-for-user";
            var endpointClass = new Endpoints.getNftsForUser
            {
                ReaderPublicKeyBase58Check = this.ReaderPublicKeyBase58Check,
                UserPublicKeyBase58Check = identity.getPublicKey(),
            };
            if (forSale.HasValue) { endpointClass.IsForSale = forSale; }
            string postData = JsonConvert.SerializeObject(endpointClass);

            Response response = await Route.POST(endpoint, postData);
            if (response.statusCode == 200)
            {
                JObject nftJson = JObject.Parse(response.json["NFTsMap"].ToString());
                return new Response
                {
                    json = nftJson,
                    statusCode = response.statusCode
                };
            }
            else
            {
                return response;
            }
        }

        // public async Task<string> mint()
        // {
        //     return "OK";
        // }
    }
}