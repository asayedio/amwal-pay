namespace PaymentGateway.Application.Dtos.Request;

public class EncryptedRequestDto
{
    public string EncryptedData { get; set; }
    public string Key { get; set; }
}
