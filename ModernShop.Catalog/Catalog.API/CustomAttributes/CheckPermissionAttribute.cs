using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Web.Helpers;
using static System.Net.Mime.MediaTypeNames;

namespace Catalog.API.CustomAttributes
{
    public class CheckPermissionAttribute : ActionFilterAttribute
    {
        string Permission { get; set; }
        public CheckPermissionAttribute(string permission)
        {
            this.Permission = permission;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //Get Injection Services from context
            var daprClient = context.HttpContext.RequestServices.GetService<DaprClient>();
            var logger = context.HttpContext.RequestServices.GetService<ILogger<CheckPermissionAttribute>>();
            var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();

            var userId = Guid.Parse(context.HttpContext.User.Claims.FirstOrDefault(q => q.Type == "userId").Value);
            string appName = configuration.GetValue<string>("AppName");
            string DAPR_STORE_NAME = configuration.GetValue<string>("DaprStoreName");

            var stateKey = (userId.ToString() + appName).ToLower();

            logger.LogInformation("Identity get state with key");
            logger.LogInformation(stateKey);

            var result = daprClient.GetStateAsync<List<string>>(DAPR_STORE_NAME, stateKey).Result;

            logger.LogInformation("state result");
            if (result == null)
            {
                context.Result = new BadRequestObjectResult("Access Denied !");
                base.OnActionExecuting(context);
            }


            if (result != null && !result.Contains(Permission))
            {
                logger.LogInformation(String.Join("\n", result));
                context.Result = new BadRequestObjectResult("Access Denied !");
                base.OnActionExecuting(context);
            }

            base.OnActionExecuting(context);
        }
    }
}
