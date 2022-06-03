using NewsAggregationWidget.Core.Entities;

namespace NewsAggregationWidget.Core;

public interface INHibernateRepository
{
	IEnumerable<User> GetAll();
	User GetById(Guid id);
	Guid Add(User user);
	void Update(User user);

	Guid AddToken(RefreshToken token);
	void UpdateToken(RefreshToken token);
}