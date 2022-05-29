using NewsAggregationWidget.Core.Entities;
using NHibernate;
using ISession = NHibernate.ISession;

namespace NewsAggregationWidget;

// Analogue of DbContext in EntityFramework
public class NHibernateMapperSession : IMapperSession
{
	private readonly ISession _session;
	private ITransaction? _transaction;

	public NHibernateMapperSession(ISession session)
	{
		_session = session;
	}
	
	public IQueryable<User> Users => _session.Query<User>();

	public void BeginTransaction()
	{
		_transaction = _session.BeginTransaction();
	}

	public async Task Commit()
	{
		await _transaction.CommitAsync();
	}

	public async Task Rollback()
	{
		await _transaction.RollbackAsync();
	}

	public void CloseTransaction()
	{
		if (_transaction != null)
		{
			_transaction.Dispose();
			_transaction = null;
		}
	}

	public async Task<Guid> Save(User entity)
	{
		// await _session.SaveOrUpdateAsync(entity); - how it works?
		if (_session.Contains(entity))
		{
			await _session.UpdateAsync(entity);
		}
		else
		{
			await _session.SaveAsync(entity);
		}

		return entity.Id;
	}

	public async Task Delete(User entity)
	{
		await _session.DeleteAsync(entity);
	}
}