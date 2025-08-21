using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EdgeSync.Common.Net.Attributes;

/// <summary>
/// Attribute to mark methods that should return 201 Created status
/// </summary>
public class CreatedResponseAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ObjectResult objectResult)
        {
            // If this attribute is applied, set status to 201 Created
            if (objectResult.StatusCode == null || objectResult.StatusCode == 200)
            {
                objectResult.StatusCode = StatusCodes.Status201Created;
            }
        }
        
        base.OnActionExecuted(context);
    }
}