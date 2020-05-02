using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Bale007.Util
{
    public sealed class CryptoString
    {
        private CryptoString() { }
        private static byte[] savedKey = ASCIIEncoding.UTF8.GetBytes(Setting.STRING_KEY);
        private static byte[] savedIV = ASCIIEncoding.UTF8.GetBytes(Setting.STRING_IV);
        public static byte[] Key
        {
            get { return savedKey; }
            set { savedKey = value; }
        }
        public static byte[] IV
        {
            get { return savedIV; }
            set { savedIV = value; }
        }
        private static void RdGenerateSecretKey(RijndaelManaged rdProvider)
        {
            if (savedKey == null)
            {
                rdProvider.KeySize = 256;
                rdProvider.GenerateKey();
                savedKey = rdProvider.Key;
            }
        }
        private static void RdGenerateSecretInitVector(RijndaelManaged rdProvider)
        {
            if (savedIV == null)
            {
                rdProvider.GenerateIV();
                savedIV = rdProvider.IV;
            }
        }
        public static string Encrypt(string originalStr)
        {
            // Encode data string to be stored in memory.
            byte[] originalStrAsBytes = Encoding.ASCII.GetBytes(originalStr);
            byte[] originalBytes = { };
            // Create MemoryStream to contain output.
            using (MemoryStream memStream = new
                     MemoryStream(originalStrAsBytes.Length))
            {
                using (RijndaelManaged rijndael = new RijndaelManaged())
                {
                    // Generate and save secret key and init vector.
                    RdGenerateSecretKey(rijndael);
                    RdGenerateSecretInitVector(rijndael);
                    if (savedKey == null || savedIV == null)
                    {
                        throw (new NullReferenceException(
                                "savedKey and savedIV must be non-null."));
                    }
                    // Create encryptor and stream objects.
                    using (ICryptoTransform rdTransform =
                           rijndael.CreateEncryptor((byte[])savedKey.
                           Clone(), (byte[])savedIV.Clone()))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memStream,
                              rdTransform, CryptoStreamMode.Write))
                        {
                            // Write encrypted data to the MemoryStream.
                            cryptoStream.Write(originalStrAsBytes, 0,
                                       originalStrAsBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            originalBytes = memStream.ToArray();
                        }
                    }
                }
            }
            // Convert encrypted string.
            string encryptedStr = Convert.ToBase64String(originalBytes);
            return (encryptedStr);
        }
        public static string Decrypt(string encryptedStr)
        {
            // Unconvert encrypted string.
            byte[] encryptedStrAsBytes = Convert.FromBase64String(encryptedStr);
            byte[] initialText = new Byte[encryptedStrAsBytes.Length];
            using (RijndaelManaged rijndael = new RijndaelManaged())
            {
                using (MemoryStream memStream = new MemoryStream(encryptedStrAsBytes))
                {
                    if (savedKey == null || savedIV == null)
                    {
                        throw (new NullReferenceException(
                                "savedKey and savedIV must be non-null."));
                    }
                    // Create decryptor and stream objects.
                    using (ICryptoTransform rdTransform =
                         rijndael.CreateDecryptor((byte[])savedKey.
                         Clone(), (byte[])savedIV.Clone()))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memStream,
                         rdTransform, CryptoStreamMode.Read))
                        {
                            // Read in decrypted string as a byte[].
                            cryptoStream.Read(initialText, 0, initialText.Length);
                        }
                    }
                }
            }
            // Convert byte[] to string.
            string decryptedStr = Encoding.ASCII.GetString(initialText);
            return (decryptedStr);
        }
    }
}