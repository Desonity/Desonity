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
                throw new Exception("Error " + response.statusCode + " while creating getting posts for public key: " + response.json.ToString());
            }
        }

        public static async Task<PostEntry> submitPost(Identity identity, SubmitPost submitPost)
        {
            if (identity.getScope() == Desonity.IdentityScopes.READ_ONLY)
            {
                throw new Exception("Cannot create post with scope " + identity.getScope());
            }
            string endpoint = "/submit-post";
            string postData = JsonConvert.SerializeObject(submitPost);
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
