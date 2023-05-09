using Diploma.Application.Implementations;
using Diploma.Application.Interfaces;
using Diploma.Application.Settings;
using Diploma.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Diploma.Infrastructure.Interfaces;
using Diploma.Infrastructure.Implementations;



var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var bankSettings = GetBankSetting(configuration);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(bankSettings);
builder.Services.AddScoped<IFiscalizePaymentService, CheckOnlineFiscalizeService>();
builder.Services.AddScoped<ISessionHandlerService, SessionHandlerService>();
builder.Services.AddScoped<ISessionStatesRepository, SessionStatesRepository>();
builder.Services.AddScoped<ISessionsPoolHandlerService, SessionsPoolHandlerService>();
builder.Services.AddLogging();
builder.Services.AddHostedService<KafkaConsumerService>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


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

app.Run();

BankSettings GetBankSetting(IConfiguration fileConfiguration)
{
    return fileConfiguration.GetSection("BankSettings").Get<BankSettings>() 
           ?? throw new NullReferenceException("Не найдены настройки банка");
}