using EFDataAccessLibrary.DataAccess;
using Microsoft.EntityFrameworkCore;
using OnlineFraudDetection.ApiHelper;
using OnlineFraudDetection.Models;
using OnlineFraudDetection.Repositories.Abstraction;
using OnlineFraudDetection.Repositories.Implementation;
using OnlineFraudDetection.Validators.Abstraction;
using OnlineFraudDetection.Validators.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default") ?? throw new Exception("Database not found"));
});
builder.Services.AddScoped<IAccountHolderRepository, AccountHolderRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IApiHelper, ApiHelper>();
builder.Services.AddScoped<IValidator, Validator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var settings = builder.Configuration.GetSection("Settings").Get<Settings>();
builder.Services.AddSingleton<Settings>(settings);
//TODO Validate setting

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
