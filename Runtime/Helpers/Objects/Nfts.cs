using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Desonity.Objects
{
    [Serializable]
    public class NFTEntry
    {
        public JObject json;
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

    [Serializable]
    public class NFTEntries
    {
        public JArray array;
        public List<NFTEntry> NFTEntryResponses;
    }

    [Serializable]
    public class NFTsMap
    {
        public JObject json;
        // response json is a bit complicated for me to convert into C# native objects; so use this json object for now
    }

    [Serializable]
    public class CreateNftResponse : TxnResponse
    {
        public string NFTPostHashHex;
    }

}
