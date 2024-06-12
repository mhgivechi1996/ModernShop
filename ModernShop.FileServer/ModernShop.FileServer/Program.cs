using ModernShop.FileServer.Common;
using ModernShop.FileServer.Infrastructure;
using ModernShop.FileServer.Services;

var builder = WebApplication.CreateBuilder(args);

var daprHttpPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3602";
var daprGrpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT") ?? "60002";
builder.Services.AddDaprClient(builder => builder
    .UseHttpEndpoint($"http://localhost:{daprHttpPort}")
    .UseGrpcEndpoint($"http://localhost:{daprGrpcPort}"));

builder.Services.AddControllers().AddDapr();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions();
builder.Services.Configure<FileServerConfig>(builder.Configuration.GetSection("fileServer"));

builder.Services.AddSingleton<FileUtility>();
builder.Services.AddSingleton<IFileService, FileService>();
builder.WebHost.ConfigureKestrel(options => options.ListenLocalhost(6002));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

//app.Run("http://localhost:6002");
app.Run();
