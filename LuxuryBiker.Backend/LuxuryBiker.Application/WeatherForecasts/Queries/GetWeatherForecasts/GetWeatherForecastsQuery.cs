using LuxuryBiker.Domain.Constants;

namespace LuxuryBiker.Application.WeatherForecasts.Queries.GetWeatherForecasts
{
    [Authorize(Policy = Policies.CanChangeStatusSales)]
    public record GetWeatherForecastsQuery : IRequest<ErrorOr<IEnumerable<WeatherForecast>>>;

    public class GetWeatherForecastsQueryHandler : IRequestHandler<GetWeatherForecastsQuery, ErrorOr<IEnumerable<WeatherForecast>>>
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        public Task<ErrorOr<IEnumerable<WeatherForecast>>> Handle(GetWeatherForecastsQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult<ErrorOr<IEnumerable<WeatherForecast>>>(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray());
        }
    }
}
