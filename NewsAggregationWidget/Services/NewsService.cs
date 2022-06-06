using NewsAggregationWidget.Core;
using NewsAggregationWidget.Core.Entities;
using NewsAggregationWidget.Models;

namespace NewsAggregationWidget.Services;

public class NewsService : INewsService
{
	private readonly INHibernateRepository<News> _newsRepository;


	public NewsService(INHibernateRepository<News> newsRepository)
	{
		_newsRepository = newsRepository;
	}

	public News GetById(Guid id)
	{
		return _newsRepository.GetById(id);
		
	}

	public IEnumerable<News> GetAll()
	{
		return _newsRepository.GetAll();
	}

	public Guid CreateNews(CreateNews model)
	{
		var news = new News(model);
		var id = _newsRepository.Add(news);

		return id;
	}

	public void DeleteNews(News news)
	{
		_newsRepository.Delete(news);
	}
}