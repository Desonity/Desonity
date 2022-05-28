using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Newtonsoft.Json;

namespace Desonity
{
    public static class Sign
    {
        public static string SignTransaction(string privateKey, string TxnHex)
        {
            var curve = SecNamedCurves.GetByName("secp256k1");
            var domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H);
            var keyParameters = new ECPrivateKeyParameters(new BigInteger(privateKey, 16), domain);

            var signer = new ECDsaSigner(new HMacDsaKCalculator(new Sha256Digest()));
            signer.Init(true, keyParameters);

            var TxnBytes = cConvert(TxnHex);
            var hash = GetHash(GetHash(TxnBytes));
            var signature = signer.GenerateSignature(hash);

            var r = signature[0];
            var s = signature[1];

            var otherS = curve.Curve.Order.Subtract(s);

            if (s.CompareTo(otherS) == 1)
            {
                s = otherS;
            }

            var derSignature = new DerSequence
            (
                new DerInteger(new BigInteger(1, r.ToByteArray())),
                new DerInteger(new BigInteger(1, s.ToByteArray()))
            )
            .GetDerEncoded();

            var signatureStr = cConvert(derSignature);
            string signedTxn = TxnHex.Substring(0, TxnHex.Length - 2) + (signatureStr.Length / 2).ToString("x") + signatureStr;
            return signedTxn;
        }

        private static byte[] GetHash(byte[] data)
        {
            var digest = new Sha256Digest();
            var hash = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(data, 0, data.Length);
            digest.DoFinal(hash, 0);
            return hash;
        }

        public static string cConvert(byte[] input)
        {
            return string.Concat(input.Select(x => x.ToString("x2")));
        }

        public static byte[] cConvert(string input)
        {
            if (input.StartsWith("0x")) input = input.Remove(0, 2);

            return Enumerable.Range(0, input.Length / 2).Select(x => System.Convert.ToByte(input.Substring(x * 2, 2), 16)).ToArray();
        }

        // WIP
        public static string GetJWT(string privateKey)
        {
            List<string> segments = new List<string>();
            var header = new { alg = "ES256", typ = "JWT" };

            DateTime issued = DateTime.Now;
            DateTime expire = DateTime.Now.AddHours(10);

            byte[] headerBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header, Formatting.None));
            byte[] payloadBytes = Encoding.UTF8.GetBytes("");// payload

            segments.Add(Base64UrlEncode(headerBytes));
            segments.Add(Base64UrlEncode(payloadBytes));

            string stringToSign = string.Join(".", segments.ToArray());

            byte[] bytesToSign = Encoding.UTF8.GetBytes(stringToSign);

            byte[] keyBytes = Convert.FromBase64String(privateKey);

            var privKeyObj = Asn1Object.FromByteArray(keyBytes);
            var privStruct = RsaPrivateKeyStructure.GetInstance((Asn1Sequence)privKeyObj);

            ISigner sig = SignerUtilities.GetSigner("SHA256withRSA");

            sig.Init(true, new RsaKeyParameters(true, privStruct.Modulus, privStruct.PrivateExponent));

            sig.BlockUpdate(bytesToSign, 0, bytesToSign.Length);
            byte[] signature = sig.GenerateSignature();

            segments.Add(Base64UrlEncode(signature));
            return string.Join(".", segments.ToArray());
        }
        // from JWT spec
        private static string Base64UrlEncode(byte[] input)
        {
            var output = Convert.ToBase64String(input);
            output = output.Split('=')[0]; // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }

        private static byte[] Base64UrlDecode(string input)
        {
            var output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 1: output += "==="; break; // Three pad chars
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                default: throw new System.Exception("Illegal base64url string!");
            }
            var converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }
    }
}
