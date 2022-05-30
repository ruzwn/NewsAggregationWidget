using NewsAggregationWidget.Core.Entities;

namespace NewsAggregationWidget.Core;

public interface INHibernateRepository
{
	IEnumerable<User> GetAll();
	User GetById(Guid id);
	Task<Guid> Add(User user);
	Task Update(User user);

	Task<Guid> AddToken(RefreshToken token);
	Task UpdateToken(RefreshToken token);
}