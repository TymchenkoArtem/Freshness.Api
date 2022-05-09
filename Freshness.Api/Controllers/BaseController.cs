using Freshness.Common.EcxeptionModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Net;

namespace Freshness.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected ExceptionResponseModel Errors;

        public BaseController()
        {
            Errors = new ExceptionResponseModel();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public ContentResult ValidationError(ModelStateDictionary modelState)
        {
            return new ContentResult()
            {
                Content = JsonConvert.SerializeObject(new ExceptionResponseModel(ModelState), new JsonSerializerSettings { Formatting = Formatting.Indented }),
                StatusCode = (int)HttpStatusCode.BadRequest,
                ContentType = "application/json"
            };
        }
    }
}
