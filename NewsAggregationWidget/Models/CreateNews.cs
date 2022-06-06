namespace NewsAggregationWidget.Models;

public class CreateNews
{
	public virtual string Name { get; set; }
	public virtual string Topic { get; set; }
	public virtual string Description { get; set; }
	public virtual string Content { get; set; }
}