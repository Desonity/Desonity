using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Desonity.Objects
{

    [Serializable]
    public class PostEntry
    {
        public JObject json;
        public string PostHashHex;
        public string PosterPublicKeyBase58Check;
        public dynamic ParentStakeID; // dynamic coz idk its type
        public string Body;
        public JArray ImageURLs;
        public JArray VideoURLs;
        public PostEntry RepostsPostEntryResponse;
        public int CreatorBasisPoints;
        public int StakeMultipleBasePoints;
        public long TimestampNanos;
        public bool IsHidden;
        public long ConfirmationBlockHeight;
        public bool InMempool;
        public ProfileEntry ProfileEntryResponse;
        public dynamic Comments; // dynamic coz idk its type
        public int LikeCount;
        public int DiamondCount;
        public dynamic PostEntryReaderState; // dynamic coz idk its type
        public bool IsPinned;
        public JObject PostExtraData;
        public int CommentCount;
        public int RepostCount;
        public int QuoteRepostCount;
        public dynamic ParentPosts; // dynamic coz idk its type
        public bool IsNFT;
        public int NumNFTCopies;
        public int NumNFTCopiesForSale;
        public int NumNFTCopiesBurned;
        public bool HasUnlockable;
        public int NFTRoyaltyToCreatorBasisPoints;
        public int NFTRoyaltyToCoinBasisPoints;
        public JObject AdditionalDESORoyaltiesMap;
        public JObject AdditionalCoinRoyaltiesMap;
        public int DiamondsFromSender;
        public int HotnessScore;
        public int PostMultiplier;
    }

    [Serializable]
    public class PostsList
    {
        public JObject json;
        public List<PostEntry> Posts;
        public string LastPostHashHex;
    }

    [Serializable]
    public class SubmitPostResponse
    {
        public JObject json;
        public long TstampNanos;
        public string PostHashHex;
        public long TotalInputNanos;
        public long ChangeAmountNanos;
        public long FeeNanos;
        public string TransactionHex;

        public Identity identity;
        public async Task<Response> approveTxn()
        {
            Response res = await identity.submitTxn(TransactionHex);
            return res;
        }
    }
}
