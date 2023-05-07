using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Domain.Responses;

public class PaymentReferenceResponse : BaseResponse
{
    public string PaymentRef { get; set; }
}

