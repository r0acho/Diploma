using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Diploma.Domain.Dto;
using Diploma.Domain.Enums;

namespace Diploma.Domain.Entities;

/// <summary>
/// Класс модели состояния сессии.
/// </summary>
public record SessionStateModel
{
    /// <summary>
    /// Идентификатор сессии.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public ulong Id { get; set; }

    /// <summary>
    /// Список элементов фискального чека.
    /// </summary>
    public List<ItemFiscalReceiptDto> Items { get; set; } = new();

    /// <summary>
    /// Статус сессии.
    /// </summary>
    [EnumDataType(typeof(SessionStatus))]
    public SessionStatus Status { get; set; } = SessionStatus.InProgress;

    /// <summary>
    /// Общая сумма сессий по банковским картам.
    /// </summary>
    public decimal SumOfSessionsByBank { get; set; } = 0;

    /// <summary>
    /// Общая сумма сессий по ТАЧу.
    /// </summary>
    public decimal SumOfSessionsByTouch { get; set; } = 0;

    /// <summary>
    /// Разность между общей суммой сессий по банковским картам и общей суммой сессий по ТАЧу.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public decimal DifferenceSum => SumOfSessionsByBank - SumOfSessionsByTouch;
}