using System;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Desonity.Endpoints;
using Desonity.Objects;

namespace Desonity
{
    public class Nft
    {
        public static async Task<NFTsMap> getNftsForUser(GetNftsForUser getNftsForUser)
        {
            string endpoint = "/get-nfts-for-user";
            string postData = JsonConvert.SerializeObject(getNftsForUser);

            Response response = await Route.POST(endpoint, postData);
            if (response.statusCode == 200)
            {
                JObject nftJson = JObject.Parse(response.json["NFTsMap"].ToString());
                return new NFTsMap { json = nftJson };

            }
            else
            {
                throw new Exception("Error " + response.statusCode + " while getting nfts for user: " + response.json.ToString());
            }
        }

        public static async Task<NFTEntries> getNftEntriesForNftPost(GetNftEntriesForNftPost getNftEntriesForNftPost)
        {
            string endpoint = "/get-nft-entries-for-nft-post";
            string postData = JsonConvert.SerializeObject(getNftEntriesForNftPost);

            Response response = await Route.POST(endpoint, postData);
            if (response.statusCode == 200)
            {
                NFTEntries nfts = JsonConvert.DeserializeObject<NFTEntries>(response.json.ToString());
                nfts.array = (JArray)response.json["NFTEntryResponses"];
                return nfts;
            }
            else
            {
                throw new Exception("Error " + response.statusCode + " while getting nft entries for nft post: " + response.json.ToString());
            }

        }

        public static async Task<string> createNft(Identity identity, CreateNft createNft)
        {
            if (identity.getScope() == Desonity.IdentityScopes.READ_ONLY)
            {
                throw new Exception("Cannot mint nft with scope " + identity.getScope());
            }
            if (createNft.UpdaterPublicKeyBase58Check == null)
            {
                createNft.UpdaterPublicKeyBase58Check = identity.getPublicKey();
            }
            string endpoint = "/create-nft";
            string postData = JsonConvert.SerializeObject(createNft);
            Response response = await Route.POST(endpoint, postData);
            if (response.statusCode == 200)
            {
                JObject json = response.json;
                string TransactionHex = (string)json["TransactionHex"];
                Response signResponse = await Route.signAndSubmitTxn(TransactionHex, identity);
                if (signResponse.statusCode == 200)
                {
                    return (string)signResponse.json["TxnHashHex"];
                }
                else
                {
                    throw new Exception("Error " + signResponse.statusCode + " while submitting signed transaction: " + signResponse.json.ToString());
                }
            }
            else
            {
                throw new Exception("Error " + response.statusCode + " while creating nft transaction: " + response.json.ToString());
            }
        }
    }
}