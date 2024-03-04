using API.Endpoints.Internal;
using API.Extensions;
using API.Infrastructure;
using Application;
using Infrastructure;
using FluentValidation;
using Serilog;

const string CorsPolicy = "CorsPolicy";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.ConfigureCors();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Configuration.ConfigureLogging(builder.Services);
builder.Configuration.ConfigureSerilog();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

WebApplication app = builder.Build();

//app.UseAuthorization();
app.UseEndpoints<Program>();

app.UseSwagger();
app.UseSwaggerUI();

app.ApplyMigrations();

app.UseCors(CorsPolicy);
app.UseHttpsRedirection();
app.UseRequestContextLogging();
app.UseSerilogRequestLogging();
app.UseExceptionHandler();
app.Run();

public partial class Program { }