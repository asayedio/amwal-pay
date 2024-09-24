using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Domain.Entities;
using System.Text.Json;
using System.Security.Cryptography;
using PaymentGateway.Application.Dtos.Request;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("Process")]
        public IActionResult ProcessTransaction([FromBody] EncryptedRequestDto encryptedRequest)
        {
            try
            {
                // Decrypt the request
                var decryptedData = DecryptData(encryptedRequest.EncryptedData, encryptedRequest.Key);

                // Deserialize the transaction data
                var transactionRequest = JsonSerializer.Deserialize<Transaction>(decryptedData);

                // Process transaction data
                var response = _transactionService.ProcessTransaction(transactionRequest);

                // Serialize and encrypt the response
                var responseData = JsonSerializer.Serialize(response);
                var encryptedResponseData = EncryptData(responseData, encryptedRequest.Key);

                return Ok(new { EncryptedData = encryptedResponseData });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Message = "Validation Error", Details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error", Details = ex.Message });
            }
        }

        private string DecryptData(string encryptedData, string key)
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

        private string EncryptData(string plainText, string key)
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