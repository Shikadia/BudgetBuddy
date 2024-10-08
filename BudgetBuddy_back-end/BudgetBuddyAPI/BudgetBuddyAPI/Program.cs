using BudgetBuddy.Infrastructure;
using BudgetBuddyAPI.Extensions;
using BudgetBuddyAPI.MiddleWares;
using Serilog;



var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var isDevelopment = environment == Environments.Development;

IConfiguration config = ConfigurationSetup.GetConfig(isDevelopment);
LogSetting.SetUpSerilog(config);
Log.Logger.Information("the application has started");

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddRegisterServices(configuration);
builder.Services.AddControllersExtension();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExtension();
//builder.Services.AddAutoMapper();
builder.Services.AddCors();
builder.Services.AddAuthenticationExtension(configuration);
builder.Services.AddAuthorizationExtension();
builder.Services.AddDbContextAndConfigurations(builder.Environment, builder.Configuration);
builder.Logging.AddSerilog();


var app = builder.Build();
await BudgetBuddyDbInitializer.Seed(app);

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(setupAction =>
{
    setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "BudgetBuddy API");
});


//app.UseHttpsRedirection();
app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
app.UseMiddleware<ExceptionMiddleware>();


app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();