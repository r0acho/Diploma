using Diploma.Application.Implementations;
using Diploma.Application.Interfaces;
using Diploma.Infrastructure.Interfaces;
using Diploma.Infrastructure.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ISessionsPoolHandlerService, SessionsPoolHandlerService>();
builder.Services.AddSingleton(typeof(IDictBaseRepository<>), typeof(SessionsPoolRepository<>));
builder.Services.AddLogging();
builder.Services.AddHostedService<KafkaConsumerService>();


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