using NewsAggregationWidget.Core.Entities;

namespace NewsAggregationWidget.Core;

public interface IMapperSession
{
	void BeginTransaction();
	Task Commit();
	Task Rollback();
	void CloseTransaction();
	Task<Guid> Save(User entity);
	Task Delete(User entity);

	IQueryable<User> Users { get; }

	Task<Guid> SaveToken(RefreshToken token);
	Task DeleteToken(RefreshToken token);

	IQueryable<RefreshToken> Tokens { get; }
}