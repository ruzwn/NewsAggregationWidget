using NewsAggregationWidget.Core.Entities;
using NHibernate;
using ISession = NHibernate.ISession;

namespace NewsAggregationWidget.Core;

public class NHibernateMapperSession : IMapperSession
{
	private readonly ISession _session;
	private ITransaction? _transaction;

	public NHibernateMapperSession(ISession session)
	{
		_session = session;
	}

	public IQueryable<User> Users => _session.Query<User>();
	public IQueryable<RefreshToken> Tokens => _session.Query<RefreshToken>();
	public IQueryable<News> News => _session.Query<News>();

	public void BeginTransaction()
	{
		_transaction = _session.BeginTransaction();
	}

	public void Commit()
	{
		_transaction?.Commit();
	}

	public void Rollback()
	{
		_transaction?.Rollback();
	}

	public void CloseTransaction()
	{
		if (_transaction != null)
		{
			_transaction.Dispose();
			_transaction = null;
		}
	}

	public Guid SaveOrUpdateUser(User entity)
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

	public Guid DeleteUser(User entity)
	{
		var id = entity.Id;
		_session.Delete(entity);
		
		return id;
	}
	
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

	public Guid DeleteToken(RefreshToken token)
	{
		var id = token.Id;
		_session.Delete(token);

		return id;
	}

	public Guid SaveOrUpdateNews(News news)
	{
		if (_session.Contains(news))
		{
			_session.Update(news);
		}
		else
		{
			_session.Save(news);
		}

		return news.Id;
	}

	public Guid DeleteNews(News news)
	{
		var id = news.Id;
		_session.Delete(news);

		return id;
	}
}