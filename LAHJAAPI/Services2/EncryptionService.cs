using LAHJAAPI.Utilities;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace LAHJAAPI.Services2
{
    public class EncryptionService(IOptions<AppSettings> options)
    {
        private static readonly string Key = "MySecretEncryptionKey123"; // يجب أن يكون 32 بايت لـ AES-256
        private static readonly string IV = "MySecretIV12345678"; // يجب أن يكون 16 بايت

        public AppSettings AppSettings => options.Value;

        public (string publicKey, string privateKey) GenerateKeys()
        {
            using var rsa = RSA.Create();
            return (Convert.ToBase64String(rsa.ExportRSAPublicKey()),
                    Convert.ToBase64String(rsa.ExportRSAPrivateKey()));
        }

        public string Encrypt(string plainText, string publicKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);

            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var encryptedBytes = rsa.Encrypt(plainBytes, RSAEncryptionPadding.OaepSHA256);

            return Convert.ToBase64String(encryptedBytes);
        }

        public string Decrypt(string encryptedText, string privateKey)
        {
            using var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);

            var cipherBytes = Convert.FromBase64String(encryptedText);
            var plainBytes = rsa.Decrypt(cipherBytes, RSAEncryptionPadding.OaepSHA256);

            return Encoding.UTF8.GetString(plainBytes);
        }


        public static string EncryptUsingKey(string plainText, string key)
        {
            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.GenerateIV(); // إنشاء IV عشوائي
            var iv = aes.IV;

            using var encryptor = aes.CreateEncryptor(aes.Key, iv);
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            // إرجاع النص المشفر مع IV
            var result = Convert.ToBase64String(iv) + ":" + Convert.ToBase64String(encryptedBytes);
            return result;
        }

        public static string DecryptUsingKey(string encryptedText, string key)
        {
            var parts = encryptedText.Split(':');
            var iv = Convert.FromBase64String(parts[0]);
            var cipherBytes = Convert.FromBase64String(parts[1]);

            using var aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
        }

    }

}
