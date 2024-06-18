using Catalog.API.Configurations;
using Catalog.Application.Common.FileStorage;
using Catalog.Infrastructure;
using Catalog.Infrastructure.FileStorage;
using Dapr.Client;
using Google.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Path.GetFullPath(Directory.GetCurrentDirectory()),
    WebRootPath = Path.GetFullPath(Directory.GetCurrentDirectory()),
    Args = args
});

var daprHttpPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3601";
var daprGrpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT") ?? "60001";
builder.Services.AddDaprClient(builder => builder
    .UseHttpEndpoint($"http://localhost:{daprHttpPort}")
    .UseGrpcEndpoint($"http://localhost:{daprGrpcPort}"));

//Service Invocation
builder.Services.AddTransient<IFileStorageService>(_ =>
    new LocalFileStorageService(DaprClient.CreateInvokeHttpClient(
        "fileserverervice", $"http://localhost:{daprHttpPort}")));

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddControllers().AddDapr();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.RegisterDI(builder.Configuration);
builder.WebHost.ConfigureKestrel(options => options.ListenLocalhost(6001));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

//app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(app.Environment.ContentRootPath, "files")),
    RequestPath = "/files"
});

app.UseAuthentication();
app.UseAuthorization();

app.MapSubscribeHandler();
app.MapControllers();
app.UseCors(
            options => options.WithOrigins("http://localhost:6001").AllowAnyMethod()
            );
app.Run();
//app.Run("http://localhost:6001");
