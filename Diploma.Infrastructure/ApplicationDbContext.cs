using System.Text.Encodings.Web;
using System.Text.Json;
using Diploma.Domain.Dto;
using Diploma.Domain.Entities;
using Diploma.Domain.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Diploma.Infrastructure;

/// <summary>
/// Класс контекста БД
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }

    /// <summary>
    /// Метод отключения логирования SQL-команд БД
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddFilter((category, level) => false)));
    }
    
    /// <summary>
    /// Таблица сессий
    /// </summary>
    public DbSet<SessionStateModel> Sessions { get; set; }
    /// <summary>
    /// Таблица платежей
    /// </summary>
    public DbSet<RecurOperationResponse> Payments { get; set; }
    /// <summary>
    /// Таблица чеков
    /// </summary>
    public DbSet<FiscalizeResponse> Checks { get; set; }
    
    /// <summary>
    /// Переопрелить форму записи некоторых элементов БД
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping//добавить корректное отображение кириллицы
        };

        //поле items сущности sessions фиксировать как JSON строку
        modelBuilder.Entity<SessionStateModel>()
            .Property(s => s.Items)
            .HasConversion(
                v => JsonSerializer.Serialize(v, options),
                v => JsonSerializer.Deserialize<List<ItemFiscalReceiptDto>>(v, options));

        //статус сессии добавлять как строку, а не число
        modelBuilder.Entity<SessionStateModel>()
            .Property(e => e.Status)
            .HasConversion<string>();
        
        //автоматически производить сверку
        modelBuilder.Entity<SessionStateModel>()
            .Property(s => s.DifferenceSum)
            .HasComputedColumnSql("(\"SumOfSessionsByTouch\" - \"SumOfSessionsByBank\")")
            .IsRequired();
    }
}