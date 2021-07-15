using System;
using System.Collections.Generic;
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
        [OpenApiOperation(operationId: "Add")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "APIKEY", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody("application/json", typeof(AddRequestBody))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK)]
        public async Task<IActionResult> Add(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
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

            var todo = new Todo(false, requestBody.Label);
            await _todoRepo.AddAsync(todo);

            return new OkResult();
        }

        public class SetDoneRequestBody
        {
            public Guid Id { get; set; }
            public bool Done { get; set; }
        }

        [FunctionName("SetDone")]
        [OpenApiOperation(operationId: "SetDone")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "APIKEY", In = OpenApiSecurityLocationType.Header)]
        [OpenApiRequestBody("application/json", typeof(SetDoneRequestBody))]
        [OpenApiResponseWithoutBody(statusCode: HttpStatusCode.OK)]
        public async Task<IActionResult> SetDone(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            using var streamReader = new StreamReader(req.Body);
            var requestBodyString = await streamReader.ReadToEndAsync();
            SetDoneRequestBody requestBody;

            try
            {
                requestBody = JsonConvert.DeserializeObject<SetDoneRequestBody>(requestBodyString);
            }
            catch (JsonException)
            {
                return new BadRequestResult();
            }

            await _todoRepo.SetDoneAsync(requestBody.Id, requestBody.Done);

            return new OkResult();
        }

        [FunctionName("List")]
        [OpenApiOperation(operationId: "List")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "APIKEY", In = OpenApiSecurityLocationType.Header)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Todo>))]
        public async Task<IActionResult> List(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            var list = await _todoRepo.ListAsync();

            return new OkObjectResult(list);
        }
    }
}

