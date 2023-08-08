using FluentValidation;
using Library.Api.Data;
using Library.Api.Endpoints.Internal;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
// remove default logging providers
builder.Logging.ClearProviders();
// Serilog configuration        
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
// Register Serilog
builder.Logging.AddSerilog(logger);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMongoDbClientFactory>(_ =>
    new MongoDbClientFactory(
        builder.Configuration.GetValue<string>("Database:ConnectionString")));
builder.Services.AddEndpoints<Program>(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseEndpoints<Program>();

app.Run();
