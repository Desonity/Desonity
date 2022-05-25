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
    public class Posts
    {
        public static async Task<PostEntry> getSinglePost(GetSinglePost getSinglePost)
        {
            string endpoint = "/get-single-post";
            string postData = JsonConvert.SerializeObject(getSinglePost);
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

        public static async Task<PostsList> getPostsForPublicKey(GetPostsForPublicKey getPostsForPublicKey)
        {
            string endpoint = "/get-posts-for-public-key";
            string postData = JsonConvert.SerializeObject(getPostsForPublicKey);
            Response response = await Route.POST(endpoint, postData);
            if (response.statusCode == 200)
            {
                PostsList postsList = JsonConvert.DeserializeObject<PostsList>(response.json.ToString());
                postsList.json = (JObject)response.json;
                return postsList;
            }
            else
            {
                throw new Exception("Error " + response.statusCode + " while getting posts for public key: " + response.json.ToString());
            }
        }

        public static async Task<SubmitPostResponse> submitPost(Identity identity, SubmitPost submitPost)
        {
            string endpoint = "/submit-post";
            submitPost.UpdaterPublicKeyBase58Check = identity.getPublicKey();
            if (identity.getScope() == IdentityScopes.READ_WRITE_DERIVED)
            {
                submitPost.MinFeeRateNanosPerKB = 1700;
            }
            string postData = JsonConvert.SerializeObject(submitPost);
            Response response = await Route.POST(endpoint, postData);
            if (response.statusCode == 200)
            {
                SubmitPostResponse submitPostResponse = JsonConvert.DeserializeObject<SubmitPostResponse>(response.json.ToString());
                submitPostResponse.json = (JObject)response.json;
                submitPostResponse.identity = identity;
                return submitPostResponse;
            }
            else
            {
                throw new Exception("Error " + response.statusCode + " while creating post transaction: " + response.json.ToString());
            }
        }
    }
}
