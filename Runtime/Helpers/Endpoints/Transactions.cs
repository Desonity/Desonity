using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Desonity.Endpoints
{
    [Serializable]
    public class SendDeso
    {
        public string SenderPublicKeyBase58Check;
        public string ReceiverPublicKeyBase58Check;
        public long AmountNanos;
        public int MinFeeRateNanosPerKB = 1000;
    }

    [Serializable]
    public class BuyCreatorCoin
    {
        public string UpdaterPublicKeyBase58Check;
        public string CreatorPublicKeyBase58Check;
        public string OperationType = "buy";
        public long DesoToSellNanos;
        public long MinCreatorCoinsExpectedNanos;
        public int MinFeeRateNanosPerKB = 1000;
    }

    [Serializable]
    public class SellCreatorCoin
    {
        public string UpdaterPublicKeyBase58Check;
        public string CreatorPublicKeyBase58Check;
        public string OperationType = "sell";
        public long CreatorCoinToSellNanos;
        public long MinDesoExpectedNanos;
        public int MinFeeRateNanosPerKB = 1000;
    }

    [Serializable]
    public class TransferCreatorCoin
    {
        public string SenderPublicKeyBase58Check;
        public string CreatorPublicKeyBase58Check;
        public string ReceiverUsernameOrPublicKeyBase58Check;
        public string CreatorCoinToTransferNanos;
        public int MinFeeRateNanosPerKB = 1000;
    }
}
