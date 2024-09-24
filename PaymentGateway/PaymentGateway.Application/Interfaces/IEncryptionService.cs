namespace PaymentGateway.Application.Interfaces;
public interface IEncryptionService
{
    string Decrypt(string encryptedData, string key);
    string Encrypt(string plainText, string key);
}
