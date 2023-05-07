﻿using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Diploma.Domain.Enums;
using Diploma.Domain.Dto;
using Diploma.Application.Interfaces;
using Diploma.Domain.Entities;

namespace Diploma.Application.Implementations.BankOperations;

/// <summary>
///     Класс для хранения полей, необходимых для оплаты транзакции в ПСБ
/// </summary>
public class Payment : BankOperationService
{
    public Payment(PaymentModel model, byte[] secretKey) : base(model, secretKey)
    {
    }

    /// <summary>
    ///     Тип банковой операции
    /// </summary>
    protected override TrType OperationType  => TrType.Pay;

    /// <summary>
    ///     Поля, которые нужно отправить для проведения транзакции
    /// </summary>
    protected override List<string> RequestKeys { get; } = new()
    {
        "AMOUNT", "CURRENCY", "ORDER", "DESC", "TERMINAL", "TRTYPE", "MERCH_NAME",
        "MERCHANT", "EMAIL", "TIMESTAMP", "NONCE", "BACKREF", "NOTIFY_URL"
    };

    /// <summary>
    ///     Порядок полей для вычисления параметра P_SIGN
    /// </summary>
    protected override List<string> PSignOrder { get; } = new()
    {
        "AMOUNT", "CURRENCY", "ORDER", "MERCH_NAME", "MERCHANT",
        "TERMINAL", "EMAIL", "TRTYPE", "TIMESTAMP", "NONCE", "BACKREF"
    };

    protected override void ChangeModelFieldsByInheritMembers()
    {
        SendingModel["CARDHOLDER_NOTIFY"] = "EMAIL";
        SendingModel["MERCHANT_NOTIFY"] = "EMAIL";
    }
}