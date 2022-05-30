using FluentNHibernate.Mapping;
using NewsAggregationWidget.Core.Entities;
using NHibernate.Mapping;

namespace NewsAggregationWidget.Core.Mappings;

public class RefreshTokenMap : ClassMap<RefreshToken>
{
	public RefreshTokenMap()
	{
		Id(token => token.Id, "id").GeneratedBy.Guid();
		Map(token => token.Token, "token");
		Map(token => token.Expires, "expires");
		Map(token => token.Created, "created");
		Map(token => token.CreatedByIp, "created_by_ip");
		Map(token => token.Revoked, "revoked");
		Map(token => token.RevokedByIp, "revoked_by_ip");
		Map(token => token.ReplacedByToken, "replaced_by_token");
		Map(token => token.ReasonRevoked, "reason_revoked");
		References(x => x.User);

		Table("refresh_tokens");
	}
}