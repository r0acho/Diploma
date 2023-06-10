using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Diploma.Domain.Dto;

/// <summary>
/// Класс DTO для элемента фискального чека.
/// </summary>
public class ItemFiscalReceiptDto
{
    /// <summary>
    /// Идентификатор элемента фискального чека.
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    [JsonIgnore]
    public ulong Id { get; set; }

    /// <summary>
    /// Описание товара/услуги в чеке.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Цена товара/услуги в копейках.
    /// </summary>
    public uint Price { get; init; }

    /// <summary>
    /// Количество товара/услуги (вес/объем), до 3 знаков после запятой.
    /// </summary>
    public decimal QtyDecimal { get; init; }

}