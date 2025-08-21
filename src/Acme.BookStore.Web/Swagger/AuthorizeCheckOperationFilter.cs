using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Acme.BookStore.Web.Swagger;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // 檢查是否有AllowAnonymous屬性
        var hasAllowAnonymous = context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
            .Union(context.MethodInfo.GetCustomAttributes(true))
            .OfType<AllowAnonymousAttribute>()
            .Any() ?? false;

        if (hasAllowAnonymous)
        {
            return; // 如果有AllowAnonymous，則不需要認證
        }

        // 檢查是否有Authorize屬性
        // var hasAuthorize = context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
        //     .Union(context.MethodInfo.GetCustomAttributes(true))
        //     .OfType<AuthorizeAttribute>()
        //     .Any() ?? false;

        // if (hasAuthorize)
        // {
            // 添加安全要求
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        []
                    }
                }
            };

            // 確保401回應被包含在內
            if (!operation.Responses.ContainsKey("401"))
            {
                operation.Responses.Add("401", new OpenApiResponse
                {
                    Description = "Unauthorized - Authentication required"
                });
            }
        // }
    }
}