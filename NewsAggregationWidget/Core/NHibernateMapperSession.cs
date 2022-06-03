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

	public void Commit()
	{
		_transaction.Commit();
	}

	public void Rollback()
	{
		_transaction.Rollback();
	}

	public void CloseTransaction()
	{
		if (_transaction != null)
		{
			_transaction.Dispose();
			_transaction = null;
		}
	}

	public Guid SaveOrUpdate(User entity)
	{
		if (_session.Contains(entity))
		{
			_session.Update(entity);
		}
		else
		{
			_session.Save(entity);
		}

		return entity.Id;
	}

	public void Delete(User entity)
	{
		_session.Delete(entity);
	}


	public IQueryable<RefreshToken> Tokens => _session.Query<RefreshToken>();

	public Guid SaveOrUpdateToken(RefreshToken token)
	{
		if (_session.Contains(token))
		{
			_session.Update(token);
		}
		else
		{
			_session.Save(token);
		}

		return token.Id;
	}

	public void DeleteToken(RefreshToken token)
	{
		_session.Delete(token);
	}
}