using NewsAggregationWidget.Core.Entities;

namespace NewsAggregationWidget.Core;

// Analogue of DbContext in EntityFramework
// todo: make a generic (for each entity? but then we have several ISession instances?)
public interface IMapperSession
{
	void BeginTransaction();
	void Commit();
	void Rollback();
	void CloseTransaction();
	
	IQueryable<User> Users { get; }
	Guid SaveOrUpdateUser(User entity);
	Guid DeleteUser(User entity);

	IQueryable<RefreshToken> Tokens { get; }
	Guid SaveOrUpdateToken(RefreshToken token);
	Guid DeleteToken(RefreshToken token);

	IQueryable<News> News { get; }
	Guid SaveOrUpdateNews(News news);
	Guid DeleteNews(News news);
}