using System;
using System.Text;
using System.Security.Cryptography;

namespace Desonity
{
    public static class Encryption
    {
        private static string KEY32;
        private static string IV16;
        private static int blockSize = 128;
        private static int keySize = 256;
        private static CipherMode cipherMode = CipherMode.CBC;
        private static PaddingMode paddingMode = PaddingMode.PKCS7;

        public static void setKEYAndIV(string key32, string iv16)
        {
            if (KEY32.Length != 32) { throw new Exception("KEY must be 32 characters long"); }
            if (IV16.Length != 16) { throw new Exception("IV must be 16 characters long"); }
            KEY32 = key32;
            IV16 = iv16;
        }

        public static string Encrypt(string data)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = blockSize;
            aes.KeySize = keySize;
            aes.Mode = cipherMode;
            aes.Padding = paddingMode;
            aes.Key = Encoding.UTF8.GetBytes(KEY32);
            aes.IV = Encoding.UTF8.GetBytes(IV16);

            byte[] dataEnc = aes.CreateEncryptor().TransformFinalBlock(Encoding.UTF8.GetBytes(data), 0, data.Length);
            return Convert.ToBase64String(dataEnc);
        }

        public static string Decrypt(string data){
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = blockSize;
            aes.KeySize = keySize;
            aes.Mode = cipherMode;
            aes.Padding = paddingMode;
            aes.Key = Encoding.UTF8.GetBytes(KEY32);
            aes.IV = Encoding.UTF8.GetBytes(IV16);

            byte[] dataDec = aes.CreateDecryptor().TransformFinalBlock(Convert.FromBase64String(data), 0, data.Length);
            return ASCIIEncoding.ASCII.GetString(dataDec);
        }
    }
}
