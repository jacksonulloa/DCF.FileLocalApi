using DCF.FileStream.Entities.Generic;
using DCF.FileStream.Services.Implementations;
using DCF.FileStream.Services.Interfaces;
using DCF.FileStream.Services.Profiles;
using DCF.FileStream.Utils.Implementations;
using DCF.FileStream.Utils.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

namespace DCF.FileStream.api
{
    public class StartUp
    {
        public static LoggerConfiguration ConfigureLogger(LoggerConfiguration _config, string _systemName, string _logPath,
            string _fileName)
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var partilaFilePath = Path.Combine(baseDir, _logPath);
            var totalFilePath = Path.Combine(partilaFilePath, _fileName);
            _config.WriteTo.Logger(lg => lg
                .WriteTo.Conditional(
                    logEvent => logEvent.MessageTemplate.Text.Contains(_systemName) ||
                                logEvent.Properties.Any(p => p.Value.ToString().Contains(_systemName)),
                    wt => wt.File(
                        path: totalFilePath,
                        rollingInterval: RollingInterval.Day,
                        fileSizeLimitBytes: 10_000_000,
                        rollOnFileSizeLimit: true,
                        shared: true,
                        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{ThreadId}] {Message:lj}{NewLine}{Exception}")
            ));
            return _config;
        }
        public static void ConfigureServices(WebApplicationBuilder _builder, AppSettings _appSettings)
        {
            _builder.Services.Configure<AppSettings>(_builder.Configuration);
            ConfigSecurity(_builder, _appSettings);
            ConfigSwagger(_builder.Services);
            ConfigRepositories(_builder.Services);
            ConfigBusinessServices(_builder.Services);
            ConfigAutoMapper(_builder.Services);
            ConfigUtilities(_builder.Services);
            _builder.Services.AddControllers();
        }
        public static void ConfigRepositories(IServiceCollection ListRepositories)
        {
            //ListRepositories.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        }
        public static void ConfigBusinessServices(IServiceCollection ListServices)
        {
            ListServices.AddSingleton<IDependencyProviderService, DependencyProviderService>();
            ListServices.AddTransient<IFileService, FileService>();
            ListServices.AddTransient<ISecurityService, SecurityService>();
        }
        public static void ConfigAutoMapper(IServiceCollection ListProfiles)
        {
            ListProfiles.AddAutoMapper(config =>
            {
                config.AddProfile<ContractProfile>();
            });
        }
        public static void ConfigUtilities(IServiceCollection ListUtilities)
        {
            ListUtilities.AddTransient<IToolsService, ToolsService>();
            ListUtilities.AddTransient<ILogService, LogService>();
        }
        public static void ConfigSecurity(WebApplicationBuilder _builder, AppSettings _appSettings)
        {
            _builder.Services.AddCors(setup =>
            {
                setup.AddPolicy("CORS", policy =>
                {
                    policy.AllowAnyOrigin(); //Que cualquiera pueda consumir el api
                    policy.AllowAnyHeader(); //Que aca se puede usar bearer token
                    policy.AllowAnyMethod(); //Se habilite el patch y el curl
                });
            });
            _builder.Services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                var key = Encoding.UTF8.GetBytes(_appSettings.Jwt.SecretKey ??
                    throw new InvalidOperationException("No se configuro el Jwt"));
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _appSettings.Jwt.Issuer,
                    ValidAudience = _appSettings.Jwt.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
            _builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("credentialtipo1", policy =>
                    policy.RequireClaim("users", "superadmin"));
                options.AddPolicy("credentialtipo2", policy =>
                    policy.RequireClaim("users", new[] { "commonuser", "superadmin" }));
            });
        }
        public static void ConfigSwagger(IServiceCollection _services)
        {
            _services.AddEndpointsApiExplorer();
            _services.AddSwaggerGen(c =>
            {
                //    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //    c.IncludeXmlComments(xmlPath);

                //    var modelXmlFile = "CtaRec.Transac.Dtos.xml"; // Reemplaza con el nombre real del proyecto del modelo
                //    var modelXmlPath = Path.Combine(AppContext.BaseDirectory, modelXmlFile);
                //    c.IncludeXmlComments(modelXmlPath);

                //    var responseXmlFile = "CtaRec.Transac.Entities.xml"; // Reemplaza con el nombre real del proyecto de la respuesta
                //    var responseXmlPath = Path.Combine(AppContext.BaseDirectory, responseXmlFile);
                //    c.IncludeXmlComments(responseXmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header utilizando el esquema Bearer. Ejemplo: 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new List<string>()
                        }
                });
            });
        }
        public static void ShowSwaggerAndUseHttps(AppSettings _appSettings, WebApplication _app)
        {
            _app.UseSwagger();
            _app.UseSwaggerUI();
            _app.UseHttpsRedirection();
            //if (_appSettings.GlobalConfig.ShowOpenApi.Equals("On"))
            //{
            //    _app.UseSwagger();
            //    _app.UseSwaggerUI();
            //}
            //if (_appSettings.GlobalConfig.UseHttps.Equals("On"))
            //{
            //    _app.UseHttpsRedirection();
            //}
        }
    }
}
