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
}