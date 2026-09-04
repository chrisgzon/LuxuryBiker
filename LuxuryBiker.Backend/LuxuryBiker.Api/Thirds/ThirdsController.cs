using AutoMapper;
using LuxuryBiker.Api.Common;
using LuxuryBiker.Application.Common.Models;
using LuxuryBiker.Application.Thirds.Commands.CreateThird;
using LuxuryBiker.Application.Thirds.Commands.UpdateThird;
using LuxuryBiker.Application.Thirds.Queries.GetThirdById;
using LuxuryBiker.Application.Thirds.Queries.GetThirds;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LuxuryBiker.Api.Thirds
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class ThirdsController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public ThirdsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ThirdModel thirdModel)
        {
            ThirdDto dto = _mapper.Map<ThirdDto>(thirdModel);
            ErrorOr<int> response = await _mediator.Send(new CreateThirdCommand(dto));
            return response.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateThirdModel model)
        {
            UpdateThirdDto dto = _mapper.Map<UpdateThirdDto>(model);
            ErrorOr<Success> response = await _mediator.Send(new UpdateThirdCommand(dto));
            return response.Match(
                _ => Ok(),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            ErrorOr<ThirdBriefDto> response = await _mediator.Send(new GetThirdByIdQuery(id));
            return response.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] int? typeId = null)
        {
            ErrorOr<PaginatedList<ThirdBriefDto>> response =
                await _mediator.Send(new GetThirdsQuery(pageNumber, pageSize, typeId));

            return response.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }
    }
}
