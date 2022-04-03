using UnityEngine;
using Desonity;

public class NewBehaviourScript : MonoBehaviour
{
    async void Start()
    {
        string myPublicKey = "BC1YLhF5DHfgqM7rwYK8PVnfKDmMXyVeQqJyeJ8YGsmbVb14qTm123G";

        var identity = new Desonity.Identity(myPublicKey);

        var nft = new Desonity.Nft(ReaderIdentity: identity);

        var a = await nft.getSingleNft(PostHashHex: "0799dffd168430bed40cf7f21ec9b6236b534b4f1a3770d9195a33f225412bcb");
        Debug.Log(a.array); // gives an array of all copies of that NFT

        var b = await nft.getNftsForUser(identity);
        Debug.Log(b.json); // gives NFTsMap of all owned NFTs of `identity`

    }
}
