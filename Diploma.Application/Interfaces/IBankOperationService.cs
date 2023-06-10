using System.Collections.Generic;

namespace Diploma.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса для запросов в банк.
/// </summary>
public interface IBankOperationService
{
    /// <summary>
    /// Получение модели для запроса в банк.
    /// </summary>
    /// <returns>Модель для запроса в банк.</returns>
    IDictionary<string, string> GetRequestingModel();
}