using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddControllers()
      .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();

// Configure database and authentication

ConfigurationManager config = builder.Configuration;

builder.Services.ConfigureDbContext(config);
builder.Services.ConfigureSwagger();
builder.Services.ConfigureAuthentication(config);
builder.Services.AddCustomServices();

// Build the app
var app = builder.Build();

// Configure middlewares
var env = builder.Environment;
app.ConfigureSwagger(env);
app.UseCustomMiddlewares();
app.Run();