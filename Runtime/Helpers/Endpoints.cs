using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Desonity.Endpoints
{
    [Serializable]
    public class submitTransaction
    {
        public string TransactionHex;
    }

    [Serializable]
    public class getSingleProfile
    {
        public bool NoErrorOnMissing = false;
        public string PublicKeyBase58Check;
    }
    [Serializable]
    public class getSinglePost
    {
        public string PostHashHex;
    }

    [Serializable]
    public class getNftEntriesForNftPost
    {
        public string PostHashHex;
        public string ReaderPublicKeyBase58Check;

    }

    [Serializable]
    public class getNftsForUser
    {
        public string ReaderPublicKeyBase58Check;
        public string UserPublicKeyBase58Check;
        public Nullable<bool> IsForSale;
    }

    [Serializable]
    public class submitPost
    {
        [Serializable]
        public class postBody
        {
            public string Body;
            public List<string> ImageURLs;

        }

        public string UpdaterPublicKeyBase58Check;
        public bool IsHidden = false;
        public int MinFeeRateNanosPerKB = 1000;
        public postBody BodyObj;

    }
}