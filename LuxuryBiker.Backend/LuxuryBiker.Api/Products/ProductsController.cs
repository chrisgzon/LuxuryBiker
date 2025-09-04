using AutoMapper;
using LuxuryBiker.Api.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LuxuryBiker.Application.Products.Commands.CreateProduct;
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
    }
}
