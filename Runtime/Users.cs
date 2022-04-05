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
    public class Users
    {
        public static async Task<ProfileEntry> getSingleProfile(GetSingleProfile getSingleProfile)
        {
            string endpoint = "/get-single-profile";
            string postData = JsonConvert.SerializeObject(getSingleProfile);

            Response response = await Route.POST(endpoint, postData);

            ProfileEntry profileEntry = JsonConvert.DeserializeObject<ProfileEntry>(response.json["Profile"].ToString());
            profileEntry.json = (JObject)response.json["Profile"];
            return profileEntry;
        }
    }
}
