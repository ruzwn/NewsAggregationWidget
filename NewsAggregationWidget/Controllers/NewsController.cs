using Microsoft.AspNetCore.Mvc;
using NewsAggregationWidget.Authorization;
using NewsAggregationWidget.Models;
using NewsAggregationWidget.Services;

namespace NewsAggregationWidget.Controllers;

[Authorize]
[ApiController]
[Route("api/news")]
public class NewsController : ControllerBase
{
	private readonly INewsService _newsService;

	public NewsController(INewsService newsService)
	{
		_newsService = newsService;
	}

	[HttpPost]
	public IActionResult CreateNews(CreateNews model)
	{
		var id = _newsService.CreateNews(model);

		return Ok(id);
	}

	[HttpGet]
	public IActionResult GetAll()
	{
		var news = _newsService.GetAll();

		return Ok(news);
	}

	[HttpGet("{id:guid}")]
	public IActionResult GetById(Guid id)
	{
		var news = _newsService.GetById(id);
		if (news == null)
		{
			return NotFound($"News with id {id} not found.");
		}

		return Ok(news);
	}

	[HttpDelete("{id:guid}")]
	public IActionResult DeleteNews(Guid id)
	{
		var news = _newsService.GetById(id);
		if (news == null)
		{
			return NotFound($"News with id {id} not found.");
		}

		_newsService.DeleteNews(news);

		return NoContent();
	}
}