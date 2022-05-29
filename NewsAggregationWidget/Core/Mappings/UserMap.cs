using FluentNHibernate.Mapping;
using NewsAggregationWidget.Core.Entities;

namespace NewsAggregationWidget.Core.Mappings;

public class UserMap : ClassMap<User>
{
	public UserMap()
	{
		// Можно ли мапить так, чтобы при изменении названий столбцов в таблице,
		// не нужно было менять маппинги ???
		Id(user => user.Id).GeneratedBy.Guid().Column("id");
		Map(user => user.FirstName).Column("firstname");
		Map(user => user.LastName).Column("lastname");
		Map(user => user.MiddleName).Column("middlename");
		Map(user => user.UserName).Column("username");
		Map(user => user.Email).Column("email"); // из-за того, что забыл смаппить email, кидало ошибку
		Map(user => user.Password).Column("password");

		Table("users");
	}
}