using System.ComponentModel;

namespace Diploma.Domain.Entities;

public record RecurringPaymentModel : PaymentModel
{
    [DisplayName("INT_REF")] public string? IntRef { get; set; }

    [DisplayName("RECUR_REF")] public ulong Rrn { get; set; }
}