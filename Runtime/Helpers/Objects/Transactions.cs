using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Desonity.Objects
{
    [Serializable]
    public class TxnResponse
    {   /*
         * TxnResponse class will be inherited by every other class that requires seed signing and approval
         * Since most of the fields are same
         */
        public JObject json;
        public long TotalInputNanos;
        public long ChangeAmountNanos;
        public long SpendAmountNanos;
        public long FeeNanos;
        public string TransactionHex;

        public Identity identity;
        public async Task<Response> approveTxn()
        {
            Response response = await identity.submitTxn(TransactionHex);
            return response;
        }
    }

    [Serializable]
    public class SendDesoResponse : TxnResponse
    {
        public string TransactionIDBase58Check;
        public string TxnHashHex;
    }

    [Serializable]
    public class CreatorCoinBuyOrSellResponse : TxnResponse
    {
        public long ExpectedDesoReturnedNanos;
        public long ExpectedCreatorCoinReturnedNanos;
        public long FounderRewardGeneratedNanos;
        public string TxnHashHex;
    }

    [Serializable]
    public class CreatorCoinTransferResponse : TxnResponse
    {
        public string TxnHashHex;
    }
}
