using NewsAggregationWidget.Core.Entities;

namespace NewsAggregationWidget;

// todo: create async methods ???
public interface IMapperSession
{
	void BeginTransaction();
	Task Commit();
	Task Rollback();
	void CloseTransaction();
	Task<Guid> Save(User entity);
	Task Delete(User entity);

	IQueryable<User> Users { get; }
}