﻿using Diploma.Application.Implementations.BankOperations;
using Diploma.Application.Interfaces;
using Diploma.Domain.Dto;
using Diploma.Domain.Entities;
using Diploma.Domain.Enums;
using Diploma.Domain.Responses;
using System.Text.Json;
using System.Text.Json.Serialization;
using Diploma.Domain.Extensions;


namespace Diploma.Application.Implementations
{
    public class CheckOnlineFiscalizeService : BankOperationService, IFiscalizePaymentService
    {
        /// <summary>
        ///     Поля, которые нужно отправить для проведения транзакции
        /// </summary>
        protected override List<string> RequestKeys { get; } = new()
        {
            "TERMINAL", "TIMESTAMP", "NONCE", "RECEIPT_INFO"
        };

        /// <summary>
        ///     Порядок полей для вычисления параметра P_SIGN
        /// </summary>
        protected override List<string> PSignOrder { get; } = new()
        {
            "TERMINAL", "TIMESTAMP", "NONCE"
        };

        private FiscalReceiptDto _receipt;
        private readonly string _checkApiUrl;

        protected override TrType OperationType => TrType.Pay;

        public CheckOnlineFiscalizeService(byte[] secretKey, string checkApiUrl) : base(secretKey)
        {
            _checkApiUrl = checkApiUrl;
        }

        protected override void ChangeModelFieldsByInheritMembers()
        {
            SendingModel["RECEIPT_INFO"] = JsonSerializer.Serialize(_receipt);
        }

        public async Task<FiscalPaymentResponse> FiscalizePayment(FiscalReceiptDto receiptDto, RecurringPaymentModel lastPaymentModel)
        {
            _receipt = receiptDto;
            var bankClient = new BankHttpClient(_checkApiUrl);
            _model = lastPaymentModel;
            var sendingModel = GetRequestingModel();
            sendingModel["OFD"] = "STARRYS"; //уточнить
            var responseMessage = await bankClient.SendModelToBankAsync(sendingModel);
            string responseJson = await responseMessage.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
            };
            var responseBody = JsonSerializer.Deserialize<FiscalPaymentResponse>(responseJson, options);
            return responseBody ?? throw new NullReferenceException();
        }
    }
}
