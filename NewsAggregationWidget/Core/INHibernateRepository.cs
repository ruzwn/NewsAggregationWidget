using NewsAggregationWidget.Core.Entities;

namespace NewsAggregationWidget.Core;

public interface INHibernateRepository
{
	List<User> GetAll();
	User GetById(Guid id);
	Task<Guid> Add(User entity);
}