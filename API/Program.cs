using API.Configurations;
using API.Endpoints.Internal;
using API.Extensions;
using FluentValidation;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors();
builder.Services.AddApplicationServices();
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpoints<Program>(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Configuration.ConfigureLogging(builder.Services);
builder.Configuration.ConfigureSerilog();

WebApplication app = builder.Build();

//app.UseAuthorization();
app.UseEndpoints<Program>();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.Run();

public partial class Program { }