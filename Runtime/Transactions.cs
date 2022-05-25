using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Desonity;
using Desonity.Objects;
using Desonity.Endpoints;

namespace Desonity
{
    public static class Transactions
    {
        public static async Task<SendDesoResponse> sendDeso(Identity identity, SendDeso sendDeso)
        {
            string endpoint = "/send-deso";
            sendDeso.SenderPublicKeyBase58Check = identity.getPublicKey();
            if (identity.getScope() == IdentityScopes.READ_WRITE_DERIVED)
            {
                sendDeso.MinFeeRateNanosPerKB = 1700;
            }
            string postData = JsonConvert.SerializeObject(sendDeso);
            Response response = await Route.POST(endpoint, postData);
            if (response.statusCode == 200)
            {
                SendDesoResponse sendDesoResponse = JsonConvert.DeserializeObject<SendDesoResponse>(response.json.ToString());
                sendDesoResponse.json = (JObject)response.json;
                sendDesoResponse.identity = identity;
                return sendDesoResponse;
            }
            else
            {
                throw new Exception("Error " + response.statusCode + " while sending deso: " + response.json.ToString());
            }
        }

        public static async Task<CreatorCoinBuyOrSellResponse> buyCreatorCoin(Identity identity, BuyCreatorCoin buyCreatorCoin)
        {
            string endpoint = "/buy-or-sell-creator-coin";
            buyCreatorCoin.UpdaterPublicKeyBase58Check = identity.getPublicKey();
            if (identity.getScope() == IdentityScopes.READ_WRITE_DERIVED)
            {
                buyCreatorCoin.MinFeeRateNanosPerKB = 1700;
            }
            string postData = JsonConvert.SerializeObject(buyCreatorCoin);
            Response response = await Route.POST(endpoint, postData);
            if (response.statusCode == 200)
            {
                CreatorCoinBuyOrSellResponse creatorCoinBuyOrSellResponse = JsonConvert.DeserializeObject<CreatorCoinBuyOrSellResponse>(response.json.ToString());
                creatorCoinBuyOrSellResponse.json = (JObject)response.json;
                creatorCoinBuyOrSellResponse.identity = identity;
                return creatorCoinBuyOrSellResponse;
            }
            else
            {
                throw new Exception("Error " + response.statusCode + " while buying creator coin: " + response.json.ToString());
            }
        }

        public static async Task<CreatorCoinBuyOrSellResponse> sellCreatorCoin(Identity identity, SellCreatorCoin sellCreatorCoin)
        {
            string endpoint = "/buy-or-sell-creator-coin";
            sellCreatorCoin.UpdaterPublicKeyBase58Check = identity.getPublicKey();
            if (identity.getScope() == IdentityScopes.READ_WRITE_DERIVED)
            {
                sellCreatorCoin.MinFeeRateNanosPerKB = 1700;
            }
            string postData = JsonConvert.SerializeObject(sellCreatorCoin);
            Response response = await Route.POST(endpoint, postData);
            if (response.statusCode == 200)
            {
                CreatorCoinBuyOrSellResponse creatorCoinBuyOrSellResponse = JsonConvert.DeserializeObject<CreatorCoinBuyOrSellResponse>(response.json.ToString());
                creatorCoinBuyOrSellResponse.json = (JObject)response.json;
                creatorCoinBuyOrSellResponse.identity = identity;
                return creatorCoinBuyOrSellResponse;
            }
            else
            {
                throw new Exception("Error " + response.statusCode + " while selling creator coin: " + response.json.ToString());
            }
        }

    }
}
