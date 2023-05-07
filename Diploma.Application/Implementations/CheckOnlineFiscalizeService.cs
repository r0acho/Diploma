using Diploma.Application.Implementations.BankOperations;
using Diploma.Application.Interfaces;
using Diploma.Domain.Dto;
using Diploma.Domain.Entities;
using Diploma.Domain.Enums;
using Diploma.Domain.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Application.Implementations
{
    internal class CheckOnlineFiscalizeService : BankOperationService, IFiscalizePaymentService
    {
        /// <summary>
        ///     Поля, которые нужно отправить для проведения транзакции
        /// </summary>
        protected override List<string> RequestKeys { get; } = new()
        {
            "TERMINAL", "TIMESTAMP", "RECEIPT_INFO"
        };

        /// <summary>
        ///     Порядок полей для вычисления параметра P_SIGN
        /// </summary>
        protected override List<string> PSignOrder { get; } = new()
        {
            "TERMINAL", "TIMESTAMP", "NONCE"
        };

        private string _checkApiUrl;

        protected override TrType OperationType => throw new NotImplementedException();

        public CheckOnlineFiscalizeService(PaymentModel model, byte[] secretKey, string checkApiUrl) : base(model, secretKey)
        {
            _checkApiUrl = checkApiUrl;
        }

        protected override void ChangeModelFieldsByInheritMembers()
        {
            SendingModel["RECEIPT_INFO"] = 
        }

        public FiscalPaymentResponse FiscalizePayment(FiscalReceiptDto receiptDto)
        {
            var bankClient = new BankHttpClient(_checkApiUrl);

        }
    }
}
