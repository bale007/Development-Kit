using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Bale007.Crypto
{
    public sealed class CryptoString
    {
        private CryptoString()
        {
        }

        public static byte[] Key { get; set; } = Encoding.UTF8.GetBytes(Setting.STRING_KEY);

        public static byte[] IV { get; set; } = Encoding.UTF8.GetBytes(Setting.STRING_IV);

        private static void RdGenerateSecretKey(RijndaelManaged rdProvider)
        {
            if (Key == null)
            {
                rdProvider.KeySize = 256;
                rdProvider.GenerateKey();
                Key = rdProvider.Key;
            }
        }

        private static void RdGenerateSecretInitVector(RijndaelManaged rdProvider)
        {
            if (IV == null)
            {
                rdProvider.GenerateIV();
                IV = rdProvider.IV;
            }
        }

        public static string Encrypt(string originalStr)
        {
            // Encode data string to be stored in memory.
            var originalStrAsBytes = Encoding.ASCII.GetBytes(originalStr);
            byte[] originalBytes = { };
            // Create MemoryStream to contain output.
            using (var memStream = new
                MemoryStream(originalStrAsBytes.Length))
            {
                using (var rijndael = new RijndaelManaged())
                {
                    // Generate and save secret key and init vector.
                    RdGenerateSecretKey(rijndael);
                    RdGenerateSecretInitVector(rijndael);
                    if (Key == null || IV == null)
                        throw new NullReferenceException(
                            "savedKey and savedIV must be non-null.");
                    // Create encryptor and stream objects.
                    using (var rdTransform =
                        rijndael.CreateEncryptor((byte[]) Key.Clone(), (byte[]) IV.Clone()))
                    {
                        using (var cryptoStream = new CryptoStream(memStream,
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
            var encryptedStr = Convert.ToBase64String(originalBytes);
            return encryptedStr;
        }

        public static string Decrypt(string encryptedStr)
        {
            // Unconvert encrypted string.
            var encryptedStrAsBytes = Convert.FromBase64String(encryptedStr);
            var initialText = new byte[encryptedStrAsBytes.Length];
            using (var rijndael = new RijndaelManaged())
            {
                using (var memStream = new MemoryStream(encryptedStrAsBytes))
                {
                    if (Key == null || IV == null)
                        throw new NullReferenceException(
                            "savedKey and savedIV must be non-null.");
                    // Create decryptor and stream objects.
                    using (var rdTransform =
                        rijndael.CreateDecryptor((byte[]) Key.Clone(), (byte[]) IV.Clone()))
                    {
                        using (var cryptoStream = new CryptoStream(memStream,
                            rdTransform, CryptoStreamMode.Read))
                        {
                            // Read in decrypted string as a byte[].
                            cryptoStream.Read(initialText, 0, initialText.Length);
                        }
                    }
                }
            }

            // Convert byte[] to string.
            var decryptedStr = Encoding.ASCII.GetString(initialText);
            return decryptedStr;
        }
    }
}