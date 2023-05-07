using System.ComponentModel.DataAnnotations;

namespace Diploma.Domain.Enums;

public enum OperationType
{
    [Display(Name = "Оплата зарядной сессии")]
    Pay = 1,

    [Display(Name = "Оплата долга")] 
    DebtPayment = 2,

    [Display(Name = "Зарезервировать деньги для оплаты")]
    Reserve = 3,

    [Display(Name = "Вернуть зарезервированные деньги")]
    Cancel = 4
}