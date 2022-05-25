using System;
using System.Threading.Tasks;
using Desonity.Endpoints;
using Desonity.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


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
                profileEntry.AvatarUrl = Desonity.Route.getRoute() + "/get-single-profile-picture/" + profileEntry.PublicKeyBase58Check + "?fallback=" + Desonity.Route.getRoute() + "/assets/img/default_prfile_pic.png";
                return profileEntry;
            }
            else
            {
                throw new Exception("Error while getting single profile: " + response.json);
            }

        }

        public static async Task<FollowUserResponse> followUser(Identity identity, FollowUser followUser)
        {
            string endpoint = "/follow-user";
            followUser.FollowerPublicKeyBase58Check = identity.getPublicKey();
            if(identity.getScope() == IdentityScopes.READ_WRITE_DERIVED){
                followUser.MinFeeRateNanosPerKB = 1700;
            }
            string postData = JsonConvert.SerializeObject(followUser);

            Response response = await Route.POST(endpoint, postData);
            if (response.statusCode == 200)
            {
                FollowUserResponse followUserResponse = JsonConvert.DeserializeObject<FollowUserResponse>(response.json.ToString());
                followUserResponse.json = (JObject)response.json;
                followUserResponse.identity = identity;
                return followUserResponse;
            }
            else
            {
                throw new Exception("Error while following user: " + response.json);
            }
        }
    }
}
