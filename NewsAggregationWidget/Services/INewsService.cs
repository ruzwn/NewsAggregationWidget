using NewsAggregationWidget.Core.Entities;
using NewsAggregationWidget.Models;

namespace NewsAggregationWidget.Services;

public interface INewsService
{
	News GetById(Guid id);
	IEnumerable<News> GetAll();
	Guid CreateNews(CreateNews model);
	void DeleteNews(News news);
}