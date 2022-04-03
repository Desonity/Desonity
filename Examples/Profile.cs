using UnityEngine;
using Desonity;

public class Profile : MonoBehaviour
{
    async void Start()
    {
        string myPublicKey = "BC1YLhF5DHfgqM7rwYK8PVnfKDmMXyVeQqJyeJ8YGsmbVb14qTm123G";

        var identity = new Desonity.Identity(myPublicKey);

        var profile = new Desonity.Profile(identity);
        await profile.getProfile();

        Debug.Log(profile.Username);
        Debug.Log(profile.CoinEntryJson.ToString());
        Debug.Log(profile.json.ToString());

    }
}
