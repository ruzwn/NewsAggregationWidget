using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NewsAggregationWidget.Core.Entities;
using NHibernate;

namespace NewsAggregationWidget.Core;

public static class NHibernateExtensions
{
	public static IServiceCollection AddNHibernate(this IServiceCollection services)
	{
		var sessionFactory = Fluently.Configure()
			.Database(PostgreSQLConfiguration.PostgreSQL82
				.ConnectionString(connectionString =>
					connectionString
						.Host("localhost")
						.Port(5432)
						.Database("news")
						.Username("postgres")
						.Password("postgres"))
				.AdoNetBatchSize(5))
			.Cache(cache => cache.UseQueryCache().UseMinimalPuts())
			.Mappings(map =>
			{
				map.FluentMappings.AddFromAssemblyOf<RefreshToken>();
				map.FluentMappings.AddFromAssemblyOf<User>();
			})
			.BuildSessionFactory();

		services.AddSingleton(sessionFactory);
		services.AddScoped(serviceProvider =>
		{
			var interceptor = serviceProvider.GetService<IInterceptor>();

			return sessionFactory
				.OpenSession()
				.SessionWithOptions()
				.Interceptor(interceptor)
				.OpenSession();
		});
		services.AddScoped<IMapperSession, NHibernateMapperSession>();

		return services;
	}
}