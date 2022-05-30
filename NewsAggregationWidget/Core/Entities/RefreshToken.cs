using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NewsAggregationWidget.Core.Entities;

// [Owned]
public class RefreshToken
{
	[Key]
	[JsonIgnore]
	public virtual Guid Id { get; set; }
	
	[JsonIgnore]
	public virtual Guid UserId { get; set; }

	[JsonIgnore]
	public virtual User User { get; set; }
	public virtual string Token { get; set; }
	public virtual DateTime Expires { get; set; }
	public virtual DateTime Created { get; set; }
	public virtual string CreatedByIp { get; set; }
	public virtual DateTime? Revoked { get; set; }
	public virtual string RevokedByIp { get; set; }
	public virtual string ReplacedByToken { get; set; }
	public virtual string ReasonRevoked { get; set; }
	public virtual bool IsExpired => DateTime.UtcNow >= Expires;
	public virtual bool IsRevoked => Revoked != null;
	public virtual bool IsActive => !IsRevoked && !IsExpired;
}