using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace TodoList2Api
{
    public class TodoApi
    {
        private readonly ITodoRepository _todoRepo;

        public TodoApi(ITodoRepository todoRepo)
        {
            _todoRepo = todoRepo;
        }

        private class AddRequestBody
        {
            public string Label { get; set; }
        }

        [FunctionName("Add")]
        [OpenApiOperation(operationId: "Run")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "APIKEY", In = OpenApiSecurityLocationType.Header)]
        [OpenApiParameter(name: "label", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The label for the todo.")]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK)]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            using var streamReader = new StreamReader(req.Body);
            var requestBodyString = await streamReader.ReadToEndAsync();
            AddRequestBody requestBody;

            try
            {
                requestBody = JsonConvert.DeserializeObject<AddRequestBody>(requestBodyString);
            }
            catch (JsonException)
            {
                return new BadRequestResult();
            }

            var todo = new Todo()
            await _todoRepo.AddAsync(todo);

            return new OkResult();
        }
    }
}

