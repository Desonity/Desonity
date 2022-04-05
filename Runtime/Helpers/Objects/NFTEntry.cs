using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Desonity.Objects
{
    [Serializable]
    public class NFTEntries
    {
        [Serializable]
        public class NFTEntry
        {
            public string OwnerPublicKeyBase58Check;
            public int SerialNumber;
            public bool IsForSale;
            public bool IsPending;
            public bool IsBuyNow;
            public long BuyNowPriceNanos;
            public long MinBidAmountNanos;
            public long LastAcceptedBidAmountNanos;
            public long HighestBidAmountNanos;
            public long LowestBidAmountNanos;
        }

        public JArray array;
        public List<NFTEntry> NFTEntryResponses;
    }

    [Serializable]
    public class NFTsMap
    {
        public JObject json;
        // response json is a bit complicated for me to convert into C# native objects; so use this json object for now
    }

}