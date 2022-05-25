using System;
using System.Collections;
using System.Threading.Tasks;
using Desonity.Endpoints;
using Desonity.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

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

        public static async Task<CreateNftResponse> createNft(Identity identity, CreateNft createNft)
        {
            string endpoint = "/create-nft";
            createNft.UpdaterPublicKeyBase58Check = identity.getPublicKey();
            if (identity.getScope() == IdentityScopes.READ_WRITE_DERIVED)
            {
                createNft.MinFeeRateNanosPerKB = 1700;
            }
            string postData = JsonConvert.SerializeObject(createNft);
            Response response = await Route.POST(endpoint, postData);
            if (response.statusCode == 200)
            {
                CreateNftResponse createNftResponse = JsonConvert.DeserializeObject<CreateNftResponse>(response.json.ToString());
                createNftResponse.json = (JObject)response.json;
                createNftResponse.identity = identity;
                return createNftResponse;
            }
            else
            {
                throw new Exception("Error " + response.statusCode + " while creating nft transaction: " + response.json.ToString());
            }
        }
    }
}
