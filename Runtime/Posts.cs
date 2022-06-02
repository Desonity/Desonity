using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Desonity.Endpoints;
using Desonity.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

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

        public static async Task<Response> uploadImage(Identity identity, byte[] imageBytes)
        {
            string endpoint = "/upload-image";
            List<IMultipartFormSection> postData = new List<IMultipartFormSection>();
            postData.Add(new MultipartFormDataSection("UserPublicKeyBase58Check", identity.getPublicKey()));
            postData.Add(new MultipartFormDataSection("JWT", identity.getJwt()));
            postData.Add(new MultipartFormFileSection("file", imageBytes, "image.png", "image/png"));
            /* WWWForm form = new WWWForm(); */
            /* form.AddBinaryData("file", imageBytes, "image.png", "image/png"); */
            /* form.AddField("UserPublicKeyBase58Check", identity.getPublicKey()); */
            /* form.AddField("JWT", identity.getJwt()); */

            using (UnityWebRequest www = UnityWebRequest.Post(Route.getRoute() + endpoint, postData))
            {
                await www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError)
                {
                    throw new Exception(www.error);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                    Response uploadResponse = new Response
                    {
                        json = JObject.Parse(www.downloadHandler.text),
                        statusCode = www.responseCode,
                    };
                    if (www.downloadHandler.text.Contains("ImageURL"))
                    {
                        uploadResponse.data = (string)uploadResponse.json["ImageURL"];
                    }
                    return uploadResponse;
                }
            }
        }
    }
}
