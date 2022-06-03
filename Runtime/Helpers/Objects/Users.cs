using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Desonity.Objects
{
    [Serializable]
    public class ProfileEntry
    {
        public JObject json;

        [Serializable]
        public class CoinEntryObj
        {
            public int CreatorBasisPoints;
            public long DeSoLockedNanos;
            public long NumberOfHolders;
            public long CoinsInCirculationNanos;
            public long CoinWatermarkNanos;
        }

        [Serializable]
        public class DAOCoinEntryObj
        {
            public long NumberOfHolders;
            public string CoinsInCirculationNanos;
            public bool MintingDisabled;
            public string TransferRestrictionStatus;
        }

        public string PublicKeyBase58Check;
        public string Username;
        public string Description;
        public bool IsVerified;
        public string AvatarUrl;
        public bool IsBlacklisted;
        public bool IsGraylisted;
        public CoinEntryObj CoinEntry;
        public DAOCoinEntryObj DAOCoinEntry;
        public long CoinPriceDeSoNanos;
    }

    [Serializable]
    public class GetFollowsResponse
    {
        public JObject json;
        public int NumFollowers;
        public Dictionary<string, ProfileEntry> PublicKeyToProfileEntry;
    }

    [Serializable]
    public class BalanceEntry
    {
        public string MODlerPublicKeyBase58Check;
        public string CreatorPublicKeyBase58Check;
        public bool HasPurchased;
        public long BalanceNanos;
        public string BalanceNanosUint256;
        public long NetBalanceInMempool;
        public ProfileEntry ProfileEntryResponse;
    }

    [Serializable]
    public class GetHodlersResponse
    {
        public JObject json;
        public List<BalanceEntry> Hodlers;
    }

    [Serializable]
    public class IsHodlingResponse
    {
        public JObject json;
        public bool IsHodling;
        public BalanceEntry BalanceEntry;
    }

    [Serializable]
    public class FollowUserResponse : TxnResponse { }
}
