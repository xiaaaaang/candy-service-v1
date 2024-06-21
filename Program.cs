using System.Configuration;
using Candy.Service.API.Models.DatabaseEntities;
using Candy.Service.API.Services.Interfaces;
using Candy.Service.API.Services.Realizes;
using SqlSugar;
using Telegram.Bot;

namespace Candy.Service.API;

internal class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        
        //设置跨域
        builder.Services.AddCors(x =>
        {
            var origins = builder.Configuration.GetValue<string>("AllowedHosts");
            
            if (origins == null || !origins.Any())
            {
                throw new ConfigurationErrorsException("AllowedHosts is not configured.");
            }
            
            x.AddDefaultPolicy(y =>
            {
                y.WithOrigins(origins.Split(";")).AllowAnyHeader().AllowAnyMethod();
            });
            
        });

        //依赖注入
        builder.Services.AddSingleton<IHostedService, TelegramBotHostedService>();
        builder.Services.AddSingleton<ITelegramBotClient>(x =>
        {
            var token = builder.Configuration.GetValue<string>("Telegram:Token");
            return new TelegramBotClient(token);
        });
        builder.Services.AddSingleton<ITelegramBotService, TelegramBotService>();
        builder.Services.AddSingleton<TelegramBotCommandMap>();
        builder.Services.AddSingleton<IAccountOperationService, AccountOperationService>();
        builder.Services.AddSingleton<ISqlSugarClient>(x =>
        {
            
            var conn = builder.Configuration.GetValue<string>("ConnectionStrings:MySQL");
            var scope = new SqlSugarScope(new ConnectionConfig()
            {
                DbType = DbType.MySql,
                ConnectionString = conn,
                IsAutoCloseConnection = true
            });
            scope.CodeFirst.InitTables(
                typeof(Dbe_Account)
            );
            return scope;
        });
        
        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        await app.RunAsync();
    }
}