using NewsAggregationWidget.Core;
using NHibernate;
using NHibernate.SqlCommand;

namespace NewsAggregationWidget.Logging;

public class SqlLogInterceptor : EmptyInterceptor
{
	private readonly ILogger<IMapperSession> _logger;

	public SqlLogInterceptor(ILogger<IMapperSession> logger)
	{
		_logger = logger;
	}

	public override SqlString OnPrepareStatement(SqlString sql)
	{
		if (_logger != null)
		{
			_logger.LogDebug(sql.ToString());
		}

		return base.OnPrepareStatement(sql);
	}
}