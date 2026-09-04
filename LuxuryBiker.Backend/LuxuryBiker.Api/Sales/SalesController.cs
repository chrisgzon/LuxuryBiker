using AutoMapper;
using LuxuryBiker.Api.Common;
using LuxuryBiker.Application.Common.Models;
using LuxuryBiker.Application.Sales.Commands.ChangeSaleStatus;
using LuxuryBiker.Application.Sales.Commands.CreateSale;
using LuxuryBiker.Application.Sales.Queries.GetSaleFormData;
using LuxuryBiker.Application.Sales.Queries.GetSales;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuxuryBiker.Api.Sales
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class SalesController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SalesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaleModel model)
        {
            CreateSaleDto dto = _mapper.Map<CreateSaleDto>(model);
            ErrorOr<CreateSaleResult> response = await _mediator.Send(new CreateSaleCommand(dto));

            return response.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(ChangeStatusRequest request)
        {
            ErrorOr<ChangeStatusResult> response =
                await _mediator.Send(new ChangeSaleStatusCommand(request.Id));

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
            ErrorOr<PaginatedList<SaleBriefDto>> response =
                await _mediator.Send(new GetSalesQuery(pageNumber, pageSize, dateFrom, dateTo));

            return response.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetFormData()
        {
            ErrorOr<SaleFormDataDto> response = await _mediator.Send(new GetSaleFormDataQuery());

            return response.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }
    }
}
