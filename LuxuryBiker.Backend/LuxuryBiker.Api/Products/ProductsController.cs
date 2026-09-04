using AutoMapper;
using LuxuryBiker.Api.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LuxuryBiker.Application.Products.Commands.CreateProduct;
using LuxuryBiker.Application.Products.Commands.UpdateProduct;
using LuxuryBiker.Application.Products.Queries.GetProductById;
using LuxuryBiker.Application.Products.Queries.GetProducts;
using LuxuryBiker.Application.Common.Models;
using System.Text.Json;

namespace LuxuryBiker.Api.Products
{
    [ApiController]
    [Authorize]
    [Route("[controller]/[action]")]
    public class ProductsController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProductsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductModel thirdModel)
        {
            CreateProductDto dto = _mapper.Map<CreateProductDto>(thirdModel);
            ErrorOr<string> response = await _mediator.Send(new CreateProductCommand(dto));
            return response.Match(
                value => Ok(JsonSerializer.Serialize(value)),
                errors => Problem(errors)
            );
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductModel model)
        {
            UpdateProductDto dto = _mapper.Map<UpdateProductDto>(model);
            ErrorOr<Success> response = await _mediator.Send(new UpdateProductCommand(dto));
            return response.Match(
                _ => Ok(),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            ErrorOr<ProductBriefDto> response = await _mediator.Send(new GetProductByIdQuery(id));
            return response.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] bool onlyActive = true)
        {
            ErrorOr<PaginatedList<ProductBriefDto>> response =
                await _mediator.Send(new GetProductsQuery(pageNumber, pageSize, onlyActive));

            return response.Match(
                value => Ok(value),
                errors => Problem(errors)
            );
        }
    }
}
