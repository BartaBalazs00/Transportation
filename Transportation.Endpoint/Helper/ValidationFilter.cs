using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Transportation.Endpoint.Helper
{
    public class ValidationFilter :IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var error = new ErrorModel
                (
                    string.Join(",",
                    (context.ModelState.Values
                        .SelectMany(v => v.Errors
                        .Select(e => e.ErrorMessage))).ToArray())
                );
                context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Result = new JsonResult(error);
            }
        }
        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
