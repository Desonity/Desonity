using UnityEngine;
using Desonity;
using System.Collections.Generic;
using Desonity.Endpoints;
using Desonity.Objects;

public class NewBehaviourScript : MonoBehaviour
{
    async void Start()
    {
        string myPublicKey = "YOUR PUBLIC KEY";
        string testAccountSeed = "YOUR SEED HEX";

        var identity = new Identity("test", "https://desonity-login.herokuapp.com", myPublicKey, testAccountSeed);
        var notweeblet = new Identity(myPublicKey);

        var a = await profile.submitPost(identity, new submitPost
        {
            UpdaterPublicKeyBase58Check = notweeblet.getPublicKey(),
            BodyObj = new submitPost.postBody
            {
                Body = "weeblets log, day XXX, just smashed a big bug, found another one, brb~"
            }
        });

        Debug.Log(a.Body);
        Debug.Log(a.PostHashHex);
        Debug.Log(a.json);
        string postHash = a.PostHashHex;

        await new WaitForSeconds(1); // let the api catch up

        var s = await profile.getPost(identity, postHash);
        Debug.Log(s.Body);
        Debug.Log(s.json);
        Debug.Log(s.ProfileEntryResponse.Username);
    }
}
