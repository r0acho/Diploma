using Diploma.Domain.Dto;
using Diploma.Domain.Enums;
using System.ComponentModel;
using System.Security.Cryptography;

namespace Diploma.Domain.Entities
{
    public record PaymentModel
    {
        [DisplayName("AMOUNT")]
        public decimal Amount { get; set; }

        [DisplayName("ORDER")]
        public ulong Order { get; set; }

        [DisplayName("DESC")]
        public string? Description { get; set; }

        [DisplayName("MERCH_NAME")]
        public string? MerchantName { get; set; }

        [DisplayName("MERCHANT_NOTIFY_EMAIL")]
        public string? MerchantEmail { get; set; }

        [DisplayName("EMAIL")]
        public string? Email { get; set; }

        [DisplayName("MERCHANT")]
        public string? MerchantId { get; set; }

        [DisplayName("TERMINAL")]
        public ulong TerminalId { get; set; }

        [DisplayName("CURRENCY")]
        public string? Currency { get; set; } = "RUB";

        [DisplayName("TRTYPE")]
        public TrType trType { get; set; }

        [DisplayName("TIMESTAMP")]
        public string Timestamp { get; set; } = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

        [DisplayName("NONCE")]
        public string Nonce { get; set; } = GetRandomHexString();

        [DisplayName("BACKREF")]
        public string? ModuleUrl { get; set; }

        [DisplayName("NOTIFY_URL")]
        public string? ModuleNotifyUrl { get; set; }

        [DisplayName("PaymentStatus")]
        public PaymentStatus Status { get; set; } = PaymentStatus.InProgress;

        public void SetFromDtoModel(BankOperationDto operationDto)
        {
            Amount = operationDto.Amount;
            Order = operationDto.Order;
            Description = operationDto.Description;
            MerchantId = operationDto.MerchantId;
            MerchantName = operationDto.MerchantName;
            Email = operationDto.ClientEmail;
            TerminalId = operationDto.TerminalId;
            MerchantEmail = operationDto.MerchantEmail;
        }

        private static string GetRandomHexString(int length = 32)
        {
            using var csprng = RandomNumberGenerator.Create();
            var bytes = new byte[length];

            csprng.GetNonZeroBytes(bytes);
            return Convert.ToHexString(bytes);
        }
    }
}
