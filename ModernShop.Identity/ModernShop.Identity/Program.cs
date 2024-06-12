using Google.Api;
using Identity.API;
using Identity.API.Data;
using Identity.API.GrpcServices;
using Identity.API.Infra;
using Identity.API.Models;
using Identity.API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOptions();
builder.Services.Configure<Configs>(builder.Configuration.GetSection("Configs"));

var connectionString = builder.Configuration.GetConnectionString("IdentityConnectionString");
builder.Services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(connectionString));

var daprHttpPort = Environment.GetEnvironmentVariable("DAPR_HTTP_PORT") ?? "3605";
var daprGrpcPort = Environment.GetEnvironmentVariable("DAPR_GRPC_PORT") ?? "60005";
builder.Services.AddDaprClient(builder => builder
    .UseHttpEndpoint($"http://localhost:{daprHttpPort}")
    .UseGrpcEndpoint($"http://localhost:{daprGrpcPort}"));


builder.Services.AddSingleton<EncryptionUtility>();
builder.Services.AddScoped<IPermissionStateRepository, PermissionStateRepository>();

builder.Services.AddControllers().AddDapr();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = "localhost:6379,password=redis";
//    options.InstanceName = "identitystore";
//});

//builder.Services.AddGrpc();

//Add JWT Authentication
var tokenTimeOut = builder.Configuration.GetValue<int>("Configs:TokenTimeOut");
var tokenKey = Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Configs:TokenKey"));
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
          .AddJwtBearer(x =>
          {
              x.RequireHttpsMetadata = false;
              x.SaveToken = true;
              x.TokenValidationParameters = new TokenValidationParameters
              {
                  ClockSkew = TimeSpan.FromMinutes(tokenTimeOut),
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                  ValidateIssuer = false,
                  ValidateAudience = false
              };
          });



var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();


//app.UseHttpsRedirection();

//app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapSubscribeHandler();
app.MapControllers();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapGrpcService<PermissionService>();
//});

//app.Run();
app.Run("http://localhost:6005");
