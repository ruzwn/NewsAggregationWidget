using FluentNHibernate.Mapping;
using NewsAggregationWidget.Core.Entities;

namespace NewsAggregationWidget.Core.Mappings;

public class NewsMap : ClassMap<News>
{
	public NewsMap()
	{
		Id(news => news.Id).GeneratedBy.Guid();
		Map(news => news.Name, "name");
		Map(news => news.Topic, "topic");
		Map(news => news.Description, "description");
		Map(news => news.Content, "content");
		Map(news => news.Created, "created");
		Map(news => news.Edited, "edited");
		References(x => x.User);

		Table("news");
	}
}