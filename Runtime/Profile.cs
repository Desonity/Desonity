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
    public class Profile
    {
        public static async Task<ProfileEntry> getProfile(Identity identity)
        {
            string endpoint = "/get-single-profile";
            var endpointClass = new Endpoints.getSingleProfile
            {
                PublicKeyBase58Check = identity.getPublicKey()
            };
            string postData = JsonConvert.SerializeObject(endpointClass);

            Response response = await Route.POST(endpoint, postData);

            ProfileEntry profileEntry = JsonConvert.DeserializeObject<ProfileEntry>(response.json["Profile"].ToString());
            profileEntry.json = (JObject)response.json["Profile"];
            return profileEntry;
        }

        public static async Task<PostEntry> getPost(Identity identity, string postHashHex)
        {
            string endpoint = "/get-single-post";
            var endpointClass = new Endpoints.getSinglePost
            {
                PostHashHex = postHashHex
            };
            string postData = JsonConvert.SerializeObject(endpointClass);
            Response response = await Route.POST(endpoint, postData);
            if (response.statusCode == 200)
            {
                PostEntry postEntry = JsonConvert.DeserializeObject<PostEntry>(response.json["PostFound"].ToString());
                postEntry.json = (JObject)response.json["PostFound"];
                return postEntry;
            }
            else
            {
                throw new Exception("Error " + response.statusCode + " while creating getting post: " + response.json.ToString());
            }
        }

        public static async Task<PostEntry> submitPost(Identity identity, submitPost post)
        {
            if (identity.getScope() == Desonity.IdentityScopes.READ_ONLY)
            {
                throw new Exception("Cannot create post with scope " + identity.getScope());
            }
            string endpoint = "/submit-post";
            string postData = JsonConvert.SerializeObject(post);
            Response submitPostResponse = await Route.POST(endpoint, postData);
            if (submitPostResponse.statusCode == 200)
            {
                JObject json = submitPostResponse.json;
                string TransactionHex = (string)json["TransactionHex"];
                Response signResponse = await Route.signAndSubmitTxn(TransactionHex, identity);
                if (signResponse.statusCode == 200)
                {
                    PostEntry postEntry = JsonConvert.DeserializeObject<PostEntry>(signResponse.json["PostEntryResponse"].ToString());
                    postEntry.json = (JObject)signResponse.json["PostEntryResponse"];
                    return postEntry;
                }
                else
                {
                    throw new Exception("Error " + signResponse.statusCode + " while submitting signed transaction: " + signResponse.json.ToString());
                }
            }
            else
            {
                throw new Exception("Error " + submitPostResponse.statusCode + " while creating post transaction: " + submitPostResponse.json.ToString());
            }
        }
    }
}
