using NewsAggregationWidget.Authorization;
using NewsAggregationWidget.Core;
using NewsAggregationWidget.Helpers;
using NewsAggregationWidget.Logging;
using NewsAggregationWidget.Services;
using NHibernate;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

var services = builder.Services;

services.AddNHibernate();
services.AddCors();
services.AddControllers();

services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

services.AddScoped<IInterceptor, SqlLogInterceptor>();
services.AddScoped<INHibernateRepository, UserRepository>();
services.AddScoped<IJwtUtils, JwtUtils>();
services.AddScoped<IUserService, UserService>();

services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(x => x
	.SetIsOriginAllowed(_ => true)
	.AllowAnyMethod()
	.AllowAnyHeader()
	.AllowCredentials());

//app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseRouting();

app.UseMiddleware<JwtMiddleware>();

app.UseEndpoints(routeBuilder => routeBuilder.MapControllers());

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
