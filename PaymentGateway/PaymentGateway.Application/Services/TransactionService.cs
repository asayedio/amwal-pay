using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.Utilities;
using PaymentGateway.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Application.Services;
public class TransactionService : ITransactionService
{
    public TransactionResponse ProcessTransaction(Transaction transaction)
    {
        // Validate transaction data
        Validator.ValidateObject(transaction, new ValidationContext(transaction), validateAllProperties: true);

        // Convert numeric fields to BCD
        byte[] processingCodeBcd = BcdConverter.StringToBcd(transaction.ProcessingCode);
        byte[] systemTraceNrBcd = BcdConverter.StringToBcd(transaction.SystemTraceNr);
        byte[] functionCodeBcd = BcdConverter.StringToBcd(transaction.FunctionCode);
        byte[] amountTrxnBcd = BcdConverter.StringToBcd(transaction.AmountTrxn);
        byte[] currencyCodeBcd = BcdConverter.StringToBcd(transaction.CurrencyCode);

        // Generate Approval Code
        var approvalCode = GenerateApprovalCode();

        return new TransactionResponse
        {
            ResponseCode = "00",
            Message = "Success",
            ApprovalCode = approvalCode,
            DateTime = DateTime.UtcNow.ToString("yyyyMMddHHmm")
        };
    }

    private string GenerateApprovalCode()
    {
        var random = new Random();
        return random.Next(100000, 999999).ToString();
    }
}
