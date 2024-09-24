using PaymentGateway.Application.Interfaces;
using System.Security.Cryptography;

namespace PaymentGateway.Application.Services
{
    public class EncryptionService : IEncryptionService
    {
        public string Decrypt(string encryptedData, string key)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedData);
            byte[] keyBytes = Convert.FromBase64String(key);

            using var aes = Aes.Create();
            aes.Key = keyBytes;

            // Extract IV from the cipher text
            byte[] iv = new byte[aes.BlockSize / 8];
            Array.Copy(cipherTextBytes, iv, iv.Length);

            aes.IV = iv;

            int cipherTextOffset = iv.Length;
            int cipherTextLength = cipherTextBytes.Length - cipherTextOffset;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(cipherTextBytes, cipherTextOffset, cipherTextLength);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var reader = new StreamReader(cs);

            return reader.ReadToEnd();
        }

        public string Encrypt(string plainText, string key)
        {
            byte[] keyBytes = Convert.FromBase64String(key);

            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            ms.Write(aes.IV, 0, aes.IV.Length); // Prepend IV to the cipher text
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var writer = new StreamWriter(cs))
            {
                writer.Write(plainText);
            }

            var encryptedBytes = ms.ToArray();
            return Convert.ToBase64String(encryptedBytes);
        }
    }
}
