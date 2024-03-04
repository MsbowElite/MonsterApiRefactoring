using API.Endpoints.Internal;
using API.Extensions;
using API.Infrastructure;
using FluentValidation;
using Serilog;

const string CorsPolicy = "CorsPolicy";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Configuration.ConfigureLogging(builder.Services);
builder.Configuration.ConfigureSerilog();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

//app.UseAuthorization();
app.UseEndpoints<Program>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(CorsPolicy);
app.UseHttpsRedirection();
app.UseRequestContextLogging();
app.UseSerilogRequestLogging();
app.UseExceptionHandler();
app.Run();

public partial class Program { }