using UnityEngine;
using Desonity;

public class ProfileExample : MonoBehaviour
{
    async void Start()
    {
        string myPublicKey = "BC1YLhF5DHfgqM7rwYK8PVnfKDmMXyVeQqJyeJ8YGsmbVb14qTm123G";

        var identity = new Desonity.Identity(myPublicKey);

        var profile = await Profile.getProfile(identity);

        Debug.Log(profile.Username);
        Debug.Log(profile.CoinEntry.CoinsInCirculationNanos);
        Debug.Log(profile.DAOCoinEntry.CoinsInCirculationNanos);
        Debug.Log(profile.json);

    }
}