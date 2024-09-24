using PaymentGateway.Application.Interfaces;
using System.Text;

namespace PaymentGateway.API.Middlewares
{
    public class EncryptionMiddleware
    {
        private readonly RequestDelegate _next;

        public EncryptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Resolve the scoped service within the request scope
            var encryptionService = context.RequestServices.GetRequiredService<IEncryptionService>();

            // Check if the request contains the encryption key header
            if (context.Request.Headers.TryGetValue("X-Encryption-Key", out var encryptionKey))
            {
                // Decrypt the request body
                context.Request.EnableBuffering();

                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
                {
                    var encryptedData = await reader.ReadToEndAsync();
                    context.Request.Body.Position = 0;

                    var decryptedData = encryptionService.Decrypt(encryptedData, encryptionKey);

                    // Replace the request body with the decrypted data
                    var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(decryptedData));
                    context.Request.Body = memoryStream;
                }

                // Replace the response body stream with a memory stream
                var originalResponseBodyStream = context.Response.Body;
                using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                await _next(context);

                // Encrypt the response
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var plainResponseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
                var encryptedResponseBody = encryptionService.Encrypt(plainResponseBody, encryptionKey);

                // Write the encrypted response
                var encryptedResponseBytes = Encoding.UTF8.GetBytes(encryptedResponseBody);
                context.Response.Body = originalResponseBodyStream;
                context.Response.ContentLength = encryptedResponseBytes.Length;
                context.Response.ContentType = "text/plain"; // Adjust content type as needed
                await context.Response.Body.WriteAsync(encryptedResponseBytes, 0, encryptedResponseBytes.Length);
            }
            else
            {
                // Proceed without encryption if no key is provided
                await _next(context);
            }
        }
    }
}
