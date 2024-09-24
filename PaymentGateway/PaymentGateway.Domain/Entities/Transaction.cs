using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Domain.Entities;
public class Transaction
{
    [Required]
    [StringLength(6, MinimumLength = 6)]
    public string ProcessingCode { get; set; }

    [Required]
    public string SystemTraceNr { get; set; }

    [Required]
    public string FunctionCode { get; set; }

    [Required]
    [CreditCard]
    public string CardNo { get; set; }

    [Required]
    public string CardHolder { get; set; }

    [Required]
    public string AmountTrxn { get; set; }

    [Required]
    public string CurrencyCode { get; set; }
}
