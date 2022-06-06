namespace NewsAggregationWidget.Core;

public interface INHibernateRepository<T> where T : class
{
	IEnumerable<T> GetAll();
	T GetById(Guid id);
	Guid Add(T entity);
	void Update(T entity);
	void Delete(T entity);
}