using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Desonity.Endpoints
{
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
}
