using DCF.FileStream.api;
using DCF.FileStream.Entities.Generic;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

AppSettings appSettings = new();
builder.Configuration.Bind(appSettings);

var loggerConfig = new LoggerConfiguration()
    //.WriteTo.Console()
    .MinimumLevel.Information()
    .Enrich.WithThreadId();

StartUp.ConfigureLogger(loggerConfig, appSettings.GlobalConfig.SystemName, appSettings.GlobalConfig.PathLog, "logTrxBase -.log");

builder.Logging.AddSerilog(loggerConfig.CreateLogger());

StartUp.ConfigureServices(builder, appSettings);

var app = builder.Build();

StartUp.ShowSwaggerAndUseHttps(appSettings, app);

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
