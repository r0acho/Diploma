using System.ComponentModel;
using System.Globalization;
using Diploma.Domain.Dto;
using Diploma.Domain.Entities;
using Diploma.Domain.Enums;

namespace Diploma.Domain.Extensions;

/// <summary>
/// Методы расширения для классов-сущностей.
/// </summary>
public static class EntityExtensions
{
    /// <summary>
    /// Преобразует экземпляр <see cref="PaymentModel"/> в коллекцию пар ключ-значение с учетом списка ключей.
    /// </summary>
    /// <param name="payment">Экземпляр <see cref="PaymentModel"/>.</param>
    /// <param name="keys">Список ключей, по которым следует производить фильтрацию.</param>
    /// <returns>Коллекция пар ключ-значение.</returns>
    public static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairsString(this PaymentModel payment,
        List<string> keys)
    {
        var properties = TypeDescriptor.GetProperties(payment);
        var idnMapping = new IdnMapping();
        foreach (PropertyDescriptor prop in properties)
        {
            var displayName = prop.DisplayName;

            if (!keys.Contains(displayName)) continue;
            var value = prop.GetValue(payment);
            switch (value)
            {
                case decimal decimalValue:
                    yield return new KeyValuePair<string, string>(displayName,
                        decimalValue.ToString(CultureInfo.InvariantCulture));
                    break;
                case string stringValue:
                    try
                    {
                        stringValue = idnMapping.GetAscii(stringValue);
                    }
                    catch
                    {
                        // ignored
                    }

                    yield return new KeyValuePair<string, string>(displayName, stringValue);
                    break;
                case TrType trType:
                    yield return new KeyValuePair<string, string>(displayName, ((int)trType).ToString());
                    break;
                default:
                    yield return new KeyValuePair<string, string>(displayName, value?.ToString() ?? string.Empty);
                    break;
            }
        }
    }

    /// <summary>
    /// Преобразует экземпляр <see cref="RecurringBankOperationDto"/> в экземпляр <see cref="RecurringPaymentModel"/>.
    /// </summary>
    /// <param name="recurringBankOperationDto">Экземпляр <see cref="RecurringBankOperationDto"/>.</param>
    /// <returns>Экземпляр <see cref="RecurringPaymentModel"/>.</returns>
    public static RecurringPaymentModel GetModelFromDto(this RecurringBankOperationDto recurringBankOperationDto)
    {
        var model = new RecurringPaymentModel
        {
            Amount = recurringBankOperationDto.Amount,
            Order = recurringBankOperationDto.Order,
            Description = recurringBankOperationDto.Description,
            MerchantId = recurringBankOperationDto.MerchantId,
            MerchantName = recurringBankOperationDto.MerchantName,
            Email = recurringBankOperationDto.ClientEmail,
            TerminalId = recurringBankOperationDto.TerminalId,
            MerchantEmail = recurringBankOperationDto.MerchantEmail,
            IntRef = recurringBankOperationDto.IntRef,
            Rrn = recurringBankOperationDto.Rrn
        };
        return model;
    }
}
