using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Catalog.Application.Services.CategoryCQRS.Commands.CreateCategory;
using Catalog.Application.Services.CategoryCQRS.Queries;
using Catalog.API.CustomAttributes;
using Dapr.Client;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IMediator _mediator;
        private const string DAPR_STORE_NAME = "statestore";
        private readonly DaprClient _daprClient;
        private readonly ILogger _logger;

        public CategoryController(IMediator mediator, DaprClient daprClient, ILogger<CategoryController> logger)
        {
            _mediator = mediator;
            _daprClient = daprClient;
            _logger = logger;
        }

        [HttpPost]
        [CheckPermission("Category_Add")]
        public async Task<IActionResult> Post([FromForm] CreateCategoryRequest createCategoryRequest)
        {
            createCategoryRequest.UserId = UserId;
            var result = await _mediator.Send(createCategoryRequest);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<List<string>> Get(Guid id)
        {
            var appName = "catalog";
            var stateKey = $"{UserId}{appName}";

            //await _daprClient.SaveStateAsync<string>(DAPR_STORE_NAME, stateKey.ToLower(), "Mohsen");

            _logger.LogInformation(stateKey.ToLower());
            var result = await _daprClient.GetStateAsync<List<string>>(DAPR_STORE_NAME, stateKey.ToLower());

            return result;

            //var query = new GetCategoryQuery { Id = id};
            //var result = await _mediator.Send(query);
            //return Ok(result);
        }

        [HttpGet]
        [CheckPermission("Category_Get_All")]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetCategoryListQuery { };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
