using NewsAggregationWidget.Core.Entities;

namespace NewsAggregationWidget.Core;

public interface IMapperSession
{
	void BeginTransaction();
	void Commit();
	void Rollback();
	void CloseTransaction();
	Guid SaveOrUpdate(User entity);
	void Delete(User entity);

	IQueryable<User> Users { get; }

	Guid SaveOrUpdateToken(RefreshToken token);
	void DeleteToken(RefreshToken token);

	IQueryable<RefreshToken> Tokens { get; }
}