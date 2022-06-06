using NewsAggregationWidget.Core.Entities;

namespace NewsAggregationWidget.Core;

public class NewsRepository : INHibernateRepository<News>
{
	private readonly IMapperSession _session;

	public NewsRepository(IMapperSession session)
	{
		_session = session;
	}

	public IEnumerable<News> GetAll()
	{
		return _session.News.ToList();
	}

	public News GetById(Guid id)
	{
		var news = _session.News.FirstOrDefault(news => news.Id == id);
		if (news == null)
		{
			// todo: log ...
		}

		return news;
	}

	public Guid Add(News news)
	{
		_session.BeginTransaction();
		var id = _session.SaveOrUpdateNews(news);
		_session.Commit();
		_session.CloseTransaction();

		return id;
	}

	public void Update(News news)
	{
		_session.BeginTransaction();
		_session.SaveOrUpdateNews(news);
		_session.Commit();
		_session.CloseTransaction();
	}

	public void Delete(News news)
	{
		_session.BeginTransaction();
		_session.DeleteNews(news);
		_session.Commit();
		_session.CloseTransaction();
	}
}