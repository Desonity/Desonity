using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Desonity.Endpoints
{
    [Serializable]
    public class GetSingleProfile
    {
        public string PublicKeyBase58Check;
        public string Username;
        public bool NoErrorOnMissing = false;
    }

    [Serializable]
    public class FollowUser
    {
        public string FollowerPublicKeyBase58Check;
        public string FollowedPublicKeyBase58Check;
        public bool IsUnfollow;
        public int MinFeeRateNanosPerKB = 1000;
    }
}
