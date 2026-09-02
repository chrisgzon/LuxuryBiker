using AutoMapper;
using LuxuryBiker.Api.Common;
using LuxuryBiker.Application.Common.Models;
using LuxuryBiker.Application.Purchases.Commands.ChangePurchaseStatus;
using LuxuryBiker.Application.Purchases.Commands.CreatePurchase;
using LuxuryBiker.Application.Purchases.Queries.GetPurchaseFormData;
using LuxuryBiker.Application.Purchases.Queries.GetPurchases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuxuryBiker.Api.Purchases
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class PurchasesController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PurchasesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(PurchaseModel model)
        {
            CreatePurchaseDto dto = _mapper.Map<CreatePurchaseDto>(model);
            ErrorOr<CreatePurchaseResult> response = await _mediator.Send(new CreatePurchaseCommand(dto));

            return response.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(ChangeStatusRequest request)
        {
            ErrorOr<ChangeStatusResult> response =
                await _mediator.Send(new ChangePurchaseStatusCommand(request.Id));

            return response.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] DateTimeOffset? dateFrom = null,
            [FromQuery] DateTimeOffset? dateTo = null)
        {
            ErrorOr<PaginatedList<PurchaseBriefDto>> response =
                await _mediator.Send(new GetPurchasesQuery(pageNumber, pageSize, dateFrom, dateTo));

            return response.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetFormData()
        {
            ErrorOr<PurchaseFormDataDto> response = await _mediator.Send(new GetPurchaseFormDataQuery());

            return response.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }
    }
}
