using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Desonity.Objects
{
    [Serializable]
    public class SendDesoResponse
    {
        public JObject json;
        public long TotalInputNanos;
        public long SpendAmountNanos;
        public long ChangeAmountNanos;
        public long FeeNanos;
        public string TransactionIDBase58Check;
        public string TransactionHex;
        public string TxnHashHex;

        public Identity identity;
        public async Task<Response> approveTxn()
        {
            Response res = await identity.submitTxn(TransactionHex);
            return res;
        }
    }

    [Serializable]
    public class CreatorCoinBuyOrSellResponse
    {
        public JObject json;
        public long ExpectedDesoReturnedNanos;
        public long ExpectedCreatorCoinReturnedNanos;
        public long FounderRewardGeneratedNanos;
        public long SpendAmountNanos;
        public long TotalInputNanos;
        public long ChangeAmountNanos;
        public long FeeNanos;
        public string TransactionHex;
        public string TxnHashHex;

        public Identity identity;
        public async Task<Response> approveTxn()
        {
            Response res = await identity.submitTxn(TransactionHex);
            return res;
        }
    }

    [Serializable]
    public class CreatorCoinTransferResponse
    {
        public JObject json;
        public long SpendAmountNanos;
        public long TotalInputNanos;
        public long ChangeAmountNanos;
        public long FeeNanos;
        public string TransactionHex;
        public string TxnHashHex;

        public Identity identity;
        public async Task<Response> approveTxn()
        {
            Response res = await identity.submitTxn(TransactionHex);
            return res;
        }
    }
}
