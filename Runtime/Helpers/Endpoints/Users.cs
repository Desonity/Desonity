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
    public class GetFollowsStateless{
        public string PublicKeyBase58Check;
        public string Username;
        public bool GetEntriesFollowingUsername; // True -> get followers, False -> get following
        public string LastPublicKeyBase58Check;
        public uint NumToFetch = 100;
    }

    [Serializable]
    public class GetHodlersForPublicKey
    {
        public string PublicKeyBase58Check;
        public string Username;
        public string LastPublicKeyBase58Check;
        public int NumToFetch;
        public bool FetchHodlings;
        public bool FetchAll;
        public bool IsDAOCoin = false;
    }

    [Serializable]
    public class IsHodlingPublicKey
    {
        public string PublicKeyBase58Check;
        public string IsHodlingPublicKeyBase58Check;
        public bool IsDAOCoin = false;
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
