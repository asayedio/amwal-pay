using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Application.Interfaces;
public interface ITransactionService
{
    TransactionResponse ProcessTransaction(Transaction transaction);
}
