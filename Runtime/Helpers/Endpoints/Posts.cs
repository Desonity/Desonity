using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Desonity.Endpoints
{
    [Serializable]
    public class GetSinglePost
    {
        public string PostHashHex;
    }

    [Serializable]
    public class SubmitPost
    {
        [Serializable]
        public class PostBody
        {
            public string Body;
            public List<string> ImageURLs;
            public JObject PostExtraData;

        }

        public string UpdaterPublicKeyBase58Check;
        public bool IsHidden = false;
        public int MinFeeRateNanosPerKB = 1000;
        public PostBody BodyObj;

    }

    [Serializable]
    public class GetPostsForPublicKey
    {
        public string PublicKeyBase58Check;
        public string Username;
        public string ReaderPublicKeyBase58Check;
        public string LastPostHashHex;
        public int NumToFetch;
        public bool MediaRequired;
    }
}
