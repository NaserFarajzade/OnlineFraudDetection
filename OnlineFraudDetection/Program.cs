using EFDataAccessLibrary.DataAccess;
using Microsoft.EntityFrameworkCore;
using OnlineFraudDetection.ApiHelper;
using OnlineFraudDetection.Models;
using OnlineFraudDetection.RedisCache;
using OnlineFraudDetection.Repositories.Abstraction;
using OnlineFraudDetection.Repositories.Implementation;
using OnlineFraudDetection.Validators.Abstraction;
using OnlineFraudDetection.Validators.Implementation;

var builder = WebApplication.CreateBuilder(args);
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
builder.Services.AddSingleton<RedisConnectionManager>();
builder.Services.AddSingleton<IRedisRepository,RedisRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var settings = builder.Configuration.GetSection("Settings").Get<Settings>();
var settingsAreValid = ValidateSettings(settings);
builder.Services.AddSingleton<Settings>(settings);

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

bool ValidateSettings(Settings settings)
{
    var sumOfFraudCoeffs = settings.FraudulentFactorCoefficient.Time +
                           settings.FraudulentFactorCoefficient.BankType +
                           settings.FraudulentFactorCoefficient.CardsCount +
                           settings.FraudulentFactorCoefficient.ExceedingTheAverage;
    if (sumOfFraudCoeffs != 100)
    {
        return false;
    }

    return true;
}