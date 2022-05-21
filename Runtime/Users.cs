using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Desonity.Endpoints;
using Desonity.Objects;

namespace Desonity
{
    public class Users
    {
        public static async Task<ProfileEntry> getSingleProfile(GetSingleProfile getSingleProfile)
        {
            string endpoint = "/get-single-profile";
            string postData = JsonConvert.SerializeObject(getSingleProfile);

            Response response = await Route.POST(endpoint, postData);

            if (response.statusCode == 200)
            {
                ProfileEntry profileEntry = JsonConvert.DeserializeObject<ProfileEntry>(response.json["Profile"].ToString());
                profileEntry.json = (JObject)response.json["Profile"];
                profileEntry.AvatarUrl = Desonity.Route.getRoute() + "/get-single-profile-picture/" + profileEntry.json["AvatarUrl"].ToString() + "?fallback=" + Desonity.Route.getRoute() + "/assets/img/default_prfile_pic.png";
                return profileEntry;
            }
            else
            {
                throw new Exception("Error while getting single profile: " + response.json);
            }

        }

        public static async Task<Response> followUser(Identity identity, FollowUser followUser)
        {
            string endpoint = "/follow-user";
            string postData = JsonConvert.SerializeObject(followUser);

            Response response = await Route.POST(endpoint, postData);
            if (response.statusCode == 200)
            {
                string transactionHex = response.json["TransactionHex"].ToString();
                string signedTransaction = identity.getSignedTxn(transactionHex);

                Response submitResponse = await identity.submitTxn(signedTransaction);
                if (submitResponse.statusCode == 200)
                {
                    return submitResponse;
                }
                else
                {
                    throw new Exception("Error while submitting transaction: " + submitResponse.json);
                }
            }
            else
            {
                throw new Exception("Error while following user: " + response.json);
            }
        }
    }
}
