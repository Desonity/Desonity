using System;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;

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

        public async Task<string> getSingleNft(string PostHashHex)
        {
            string endpoint = "/get-nft-entries-for-nft-post";
            var endpointClass = new Endpoints.getNftEntriesForNftPost
            {
                PostHashHex = PostHashHex,
                ReaderPublicKeyBase58Check = this.ReaderPublicKeyBase58Check
            };
            string postData = JsonConvert.SerializeObject(endpointClass);

            Route route = new Route();
            string response = await route.POST(endpoint, postData);
            return response;

        }

        public async Task<string> getNftsForUser(string UserPublicKeyBase58Check, Nullable<bool> forSale = null)
        {

            // forSale:true  -> only nfts for sale
            // forSale:false -> only nfts not for sale
            // forSale:null  -> all owned nfts

            string endpoint = "/get-nfts-for-user";
            var endpointClass = new Endpoints.getNftsForUser
            {
                ReaderPublicKeyBase58Check = this.ReaderPublicKeyBase58Check,
                UserPublicKeyBase58Check = UserPublicKeyBase58Check,
            };
            if (forSale.HasValue) { endpointClass.IsForSale = forSale; }
            string postData = JsonConvert.SerializeObject(endpointClass);

            Route route = new Route();
            string response = await route.POST(endpoint, postData);
            return response;
        }
        // public async Task<string> mint()
        // {
        //     return "OK";
        // }
    }
}