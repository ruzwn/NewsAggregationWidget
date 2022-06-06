using NewsAggregationWidget.Models;

namespace NewsAggregationWidget.Core.Entities;

public class News
{
	public virtual Guid Id { get; set; }
	public virtual Guid UserId => User.Id; // todo: why NHibernate does not working when this property has different name
	public virtual User User { get; set; }
	public virtual string Name { get; set; }
	public virtual string Topic { get; set; }
	public virtual string Description { get; set; }
	public virtual string Content { get; set; }
	public virtual DateTime Created { get; set; }
	public virtual DateTime? Edited { get; set; }

	public News() { }

	public News(CreateNews model)
	{
		Name = model.Name;
		Topic = model.Topic;
		Description = model.Description;
		Content = model.Content;
		Created = DateTime.UtcNow;
	}
}