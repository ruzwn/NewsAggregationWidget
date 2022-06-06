using NewsAggregationWidget.Core.Entities;

namespace NewsAggregationWidget.Core;

public class TokenRepository : INHibernateRepository<RefreshToken>
{
	private readonly IMapperSession _session;

	public TokenRepository(IMapperSession session)
	{
		_session = session;
	}

	public IEnumerable<RefreshToken> GetAll()
	{
		return _session.Tokens.ToList();
	}

	public RefreshToken GetById(Guid id)
	{
		var token = _session.Tokens.FirstOrDefault(token => token.Id == id);
		if (token == null)
		{
			// todo: log ...
		}

		return token;
	}

	public Guid Add(RefreshToken token)
	{
		_session.BeginTransaction();
		var id = _session.SaveOrUpdateToken(token);
		_session.Commit();
		_session.CloseTransaction();

		return id;
	}

	public void Update(RefreshToken token)
	{
		_session.BeginTransaction();
		_session.SaveOrUpdateToken(token);
		_session.Commit();
		_session.CloseTransaction();
	}

	public void Delete(RefreshToken token)
	{
		_session.BeginTransaction();
		_session.DeleteToken(token);
		_session.Commit();
		_session.CloseTransaction();
	}
}