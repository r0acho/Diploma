using Diploma.Application.Implementations;
using Diploma.Application.Interfaces;
using Diploma.Application.Settings;
using Diploma.Domain.Responses;
using Diploma.Infrastructure.Implementations;
using Diploma.Infrastructure.Interfaces;

namespace Diploma.Presentation.Extensions;

public static class Initializer
{
    public static void InitializeRepositories(this IServiceCollection services)
    {
        services.AddScoped<IResponsesRepository<RecurOperationResponse>, RecurPaymentResponsesRepository>();
        services.AddScoped<IChecksRepository, ChecksRepository>();
        services.AddScoped<ISessionStatesRepository, SessionStatesRepository>();
    }

    public static void InitializeServices(this IServiceCollection services)
    {
        services.AddScoped<IFiscalizePaymentService, AtolFiscalizeService>();
        services.AddScoped<ISessionHandlerService, SessionHandlerService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<ISessionInformationService, SessionInformationService>();
        services.AddScoped<IPaymentInformationService, PaymentInformationService>();
        services.AddHostedService<KafkaConsumerService>();
    }

    public static void InitializeSettings(this IServiceCollection services, IConfiguration config)
    {
        services.AddOptions();
        services.Configure<KafkaSettings>(config.GetSection("Kafka"));
        services.Configure<BankSettings>(config.GetSection("BankSettings"));
        services.Configure<AtolSettings>(config.GetSection("AtolSettings"));
    }
}