using NewsAggregationWidget.Core.Entities;
using NHibernate;
using ISession = NHibernate.ISession;

namespace NewsAggregationWidget.Core;

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


	public IQueryable<RefreshToken> Tokens => _session.Query<RefreshToken>();
	
	public async Task<Guid> SaveToken(RefreshToken token)
	{
		if (_session.Contains(token))
		{
			await _session.UpdateAsync(token);
		}
		else
		{
			await _session.SaveAsync(token);
		}

		return token.Id;
	}

	public async Task DeleteToken(RefreshToken token)
	{
		await _session.DeleteAsync(token);
	}
}