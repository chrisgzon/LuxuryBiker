using LuxuryBiker.Api.Common;
using LuxuryBiker.Application.Dashboard.Queries.GetDashboard;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuxuryBiker.Api.Dashboard
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class DashboardController : ApiController
    {
        private readonly IMediator _mediator;

        public DashboardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            ErrorOr<DashboardDto> response = await _mediator.Send(new GetDashboardQuery());

            return response.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }
    }
}
