using System.Text.Json;
using Diploma.Domain.Dto;
using Diploma.Domain.Entities;
using Diploma.Domain.Responses;
using Microsoft.EntityFrameworkCore;


namespace Diploma.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    
    public DbSet<SessionStateModel> Sessions { get; set; }
    public DbSet<RecurringPaymentModel> RecurringPaymentModels { get; set; }
    public DbSet<RecurOperationResponse> RecurOperationResponses { get; set; } 
    public DbSet<FiscalPaymentResponse> FiscalPaymentResponses { get; set; }
    public DbSet<SessionResponse> SessionResponses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        
        modelBuilder.Entity<SessionStateModel>()
            .Property(s => s.Items)
            .HasConversion(
                v => JsonSerializer.Serialize(v, options),
                v => JsonSerializer.Deserialize<List<ItemFiscalReceiptDto>>(v, options));
        
        modelBuilder.Entity<SessionStateModel>()
            .Property(e => e.Status)
            .HasConversion<string>();
    }
    
}
