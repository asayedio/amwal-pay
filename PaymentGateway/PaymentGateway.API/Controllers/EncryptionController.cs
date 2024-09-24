using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace PaymentGateway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EncryptionController : ControllerBase
    {
        [HttpGet("GenerateKey")]
        public IActionResult GenerateKey()
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.GenerateKey();
                var key = Convert.ToBase64String(aes.Key);
                return Ok(new { Key = key });
            }
        }
    }
}
