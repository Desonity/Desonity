using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Desonity.Endpoints
{
    [Serializable]
    public class submitTransaction
    {
        public string TransactionHex { get; set; }
    }

    [Serializable]
    public class errorMessage
    {
        public string Error { get; set; }
    }

    [Serializable]
    public class successMessage
    {
        public string Response { get; set; }
    }

    [Serializable]
    public class getSingleProfile
    {
        public bool NoErrorOnMissing = false;
        public string PublicKeyBase58Check { get; set; }
    }

    [Serializable]
    public class getNftEntriesForNftPost
    {
        public string PostHashHex { get; set; }
        public string ReaderPublicKeyBase58Check { get; set; }

    }

    [Serializable]
    public class getNftsForUser
    {
        public string ReaderPublicKeyBase58Check { get; set; }
        public string UserPublicKeyBase58Check { get; set; }
        public Nullable<bool> IsForSale { get; set; }
    }

    [Serializable]
    public class submitPost
    {
        [Serializable]
        public class postBody
        {
            public string Body { get; set; }
            public List<string> ImageURLs { get; set; }

        }

        public string UpdaterPublicKeyBase58Check { get; set; }
        public bool IsHidden { get; set; } = false;
        public int MinFeeRateNanosPerKB { get; set; } = 1000;
        public postBody BodyObj { get; set; }

    }
}