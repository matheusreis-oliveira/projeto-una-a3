using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddControllers()
      .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();

// Configure database and authentication
ConfigurationManager config = builder.Configuration;
var env = builder.Environment;

builder.Services.ConfigureDbContext(config, env);
builder.Services.ConfigureSwagger();
builder.Services.ConfigureAuthentication(config);
builder.Services.AddCustomServices();

builder.Host.UseDefaultServiceProvider(options =>
    options.ValidateScopes = false);
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Trace);

// Build the app
var app = builder.Build();

// Configure middlewares
app.UseSeeder(env);
app.ConfigureSwagger(env);
app.UseCustomMiddlewares();
app.Run();