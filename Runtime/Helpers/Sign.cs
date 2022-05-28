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
using System.Security.Cryptography;

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
    }
}
