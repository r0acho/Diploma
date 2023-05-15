using Diploma.Application.Implementations;
using Diploma.Application.Interfaces;
using Diploma.Application.Settings;
using Diploma.Domain.Responses;
using Diploma.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Diploma.Infrastructure.Interfaces;
using Diploma.Infrastructure.Implementations;
using Diploma.Presentation.Middlewares;


var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
//добавляем настройки DI-конвейер
builder.Services.AddOptions();
builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection("Kafka"));
builder.Services.Configure<BankSettings>(builder.Configuration.GetSection("BankSettings"));
//добавляем сервисы в DI-конвейер
builder.Services.AddScoped<IFiscalizePaymentService, CheckOnlineFiscalizeService>();
builder.Services.AddScoped<ISessionHandlerService, SessionHandlerService>();
builder.Services.AddScoped<ISessionsPoolHandlerService, SessionsPoolHandlerService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddHostedService<KafkaConsumerService>();
//добавляем репозитории в DI-конвейер
builder.Services.AddScoped<IResponsesRepository<RecurOperationResponse>, RecurPaymentResponsesRepository>();
builder.Services.AddScoped<IResponsesRepository<FiscalPaymentResponse>, FiscalResponsesRepository>();
builder.Services.AddScoped<IResponsesRepository<SessionResponse>, SessionResponsesRepository>();
builder.Services.AddScoped<ISessionStatesRepository, SessionStatesRepository>();
builder.Services.AddScoped<IRecurPaymentsRepository, RecurPaymentsRepository>();
//добавляем базу данных в DI-конвейер
builder.Services.AddDbContext<ApplicationDbContext>(options => options
    .UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

//добавляем MiddleWare
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();

