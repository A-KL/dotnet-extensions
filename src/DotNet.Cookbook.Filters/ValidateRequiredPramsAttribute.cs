using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNet.Cookbook.Filters
{
    /// <summary>
    /// Validate that the <see cref="HttpRequest"/> contains relevant query parameter(s) marked as <see cref="BindRequiredAttribute"/> or <see cref="RequiredAttribute"/>.
    /// </summary>
    public class ValidateRequiredPramsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var expected = string.Join(", ", context.ModelState.Keys);
                var received = context.HttpContext.Request.QueryString.Value;

                context.Result = new BadRequestObjectResult($"Missing or invalid query param(s). Expected: `{expected}` Param(s) received: '{received}'");
            }
        }
    }
}
