using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Desonity.Endpoints
{
    [Serializable]
    public class SubmitTransaction
    {
        public string TransactionHex;
    }

    [Serializable]
    public class GetSingleProfile
    {
        public string PublicKeyBase58Check;
        public string Username;
        public bool NoErrorOnMissing = false;
    }
    [Serializable]
    public class GetSinglePost
    {
        public string PostHashHex;
    }

    [Serializable]
    public class GetNftEntriesForNftPost
    {
        public string PostHashHex;
        public string ReaderPublicKeyBase58Check;

    }

    [Serializable]
    public class GetNftsForUser
    {
        public string ReaderPublicKeyBase58Check;
        public string UserPublicKeyBase58Check;
        public Nullable<bool> IsForSale;
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
    public class CreateNft
    {
        public string UpdaterPublicKeyBase58Check;
        public string NFTPostHashHex;
        public int NumCopies;
        public int NFTRoyaltyToCreatorBasisPoints;
        public int NFTRoyaltyToCoinBasisPoints;
        public bool HasUnlockable;
        public bool IsForSale;
        public bool IsBuyNow;
        public long BuyNowPriceNanos;
        public JObject AdditionalDESORoyaltiesMap;
        public JObject AdditionalDESOCoinsMap;
        public int MinFeeRateNanosPerKB = 1000;
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