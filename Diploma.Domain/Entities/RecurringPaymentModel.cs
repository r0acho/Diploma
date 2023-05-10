using Diploma.Domain.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Diploma.Domain.Entities
{
    public record RecurringPaymentModel : PaymentModel
    {
        [DisplayName("INT_REF")]
        public string? IntRef { get; set; }

        [DisplayName("RECUR_REF")]
        public ulong Rrn { get; set; }

        public void SetFromDtoModel(RecurringBankOperationDto recurringBankOperationDto)
        {
            base.SetFromDtoModel(recurringBankOperationDto);
            IntRef = recurringBankOperationDto.IntRef;
            Rrn = recurringBankOperationDto.Rrn;
        }
    }
}
