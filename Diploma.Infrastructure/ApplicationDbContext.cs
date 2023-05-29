using System.Text.Encodings.Web;
using System.Text.Json;
using Diploma.Domain.Dto;
using Diploma.Domain.Entities;
using Diploma.Domain.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Diploma.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddFilter((category, level) => false)));
    }
    
    public DbSet<SessionStateModel> Sessions { get; set; }
    public DbSet<RecurOperationResponse> Payments { get; set; }
    public DbSet<FiscalizeResponse> Checks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        modelBuilder.Entity<SessionStateModel>()
            .Property(s => s.Items)
            .HasConversion(
                v => JsonSerializer.Serialize(v, options),
                v => JsonSerializer.Deserialize<List<ItemFiscalReceiptDto>>(v, options));

        modelBuilder.Entity<SessionStateModel>()
            .Property(e => e.Status)
            .HasConversion<string>();
        
        modelBuilder.Entity<SessionStateModel>()
            .Property(s => s.DifferenceSum)
            .HasComputedColumnSql("(\"SumOfSessionsByTouch\" - \"SumOfSessionsByBank\")")
            .IsRequired();
    }
}