using Diploma.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Diploma.Presentation.Extensions;
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
builder.Services.InitializeSettings(configuration);
//добавляем репозитории в DI-конвейер
builder.Services.InitializeRepositories();
//добавляем сервисы в DI-конвейер
builder.Services.InitializeServices();
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

