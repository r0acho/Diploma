using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Diploma.Domain.Responses
{
    public record FiscalPaymentResponse : BaseResponse
    {
        public string? RequestId { get; init; }
        public string? ClientId { get; init; }
        public string? Path { get; init; }
        [JsonPropertyName("Response:Error")]
        public int ResponseCode { get; init; }
        public uint FiscalDocNumber { get; init; }
        public uint DocNumber { get; init; }
        public DateTime Date { get; init; }
        public ulong GrandTotal { get; init; }
        public ulong FiscalSign { get; init; }
        public string? Qr { get; init; }
        public string? FNSerialNumber { get; init; }
        public string? DeviceSerialNumber { get; init; }
        public string? DeviceRegistrationNumber { get; init; }

    }
}
