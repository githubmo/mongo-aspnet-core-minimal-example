using FluentValidation;
using Library.Api.Data;
using Library.Api.Endpoints.Internal;

var builder = WebApplication.CreateBuilder(args);

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
