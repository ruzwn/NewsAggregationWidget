using NewsAggregationWidget;
using NewsAggregationWidget.Core;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddNHibernate();
services.AddScoped(typeof(INHibernateRepository), typeof(UserRepository));
services.AddControllers();
services.AddSwaggerGen();
services.AddScoped<IUserService, UserService>();

var app = builder.Build();

app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI();

//app.UseAuthentication();
//app.UseAuthorization();
app.UseMiddleware<JwtMiddleware>();

app.UseEndpoints(routeBuilder => routeBuilder.MapControllers());

// app.MapGet("/", () => "Widget with news aggregation");

app.Run();
