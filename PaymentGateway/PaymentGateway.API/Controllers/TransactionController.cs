using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Domain.Entities;
using System.Text.Json;
using System.Security.Cryptography;
using PaymentGateway.Application.Dtos.Request;
using System.ComponentModel.DataAnnotations;
using PaymentGateway.Application.Services;

namespace PaymentGateway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IEncryptionService _encryptionService;

        public TransactionController(ITransactionService transactionService, IEncryptionService encryptionService)
        {
            _transactionService = transactionService;
            _encryptionService = encryptionService;
        }

        [HttpPost("Process")]
        public IActionResult ProcessTransaction([FromBody] EncryptedRequestDto encryptedRequest)
        {
            try
            {
                // Decrypt the request
                var decryptedData = _encryptionService.Decrypt(encryptedRequest.EncryptedData, encryptedRequest.Key);

                // Deserialize the transaction data
                var transactionRequest = JsonSerializer.Deserialize<Transaction>(decryptedData);

                // Process transaction data
                var response = _transactionService.ProcessTransaction(transactionRequest);

                // Serialize and encrypt the response
                var responseData = JsonSerializer.Serialize(response);
                var encryptedResponseData = _encryptionService.Encrypt(responseData, encryptedRequest.Key);

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
    }
}