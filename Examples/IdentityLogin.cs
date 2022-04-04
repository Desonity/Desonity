using UnityEngine;
using Desonity;

public class IdentityLogin : MonoBehaviour
{
    async void Start()
    {
        // SIMPLE LOGIN
        string appName = "Desonity-Example";
        string backendUrl = "https://desonity-login.herokuapp.com";

        var identity = new Desonity.Identity(appName, backendUrl);

        await identity.Login();
        Debug.Log(identity.getPublicKey() + " has logged in");
    }
}
