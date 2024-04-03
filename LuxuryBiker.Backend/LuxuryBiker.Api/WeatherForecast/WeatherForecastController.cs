using LuxuryBiker.Api.Common;
using LuxuryBiker.Application.WeatherForecasts.Queries.GetWeatherForecasts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuxuryBiker.Api.WeatherForecast
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ApiController
    {
        private readonly IMediator _mediator;

        public WeatherForecastController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetWeatherForecastsQuery());
            return result.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }
    }
}