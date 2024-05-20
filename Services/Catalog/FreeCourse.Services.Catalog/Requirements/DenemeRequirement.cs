using Microsoft.AspNetCore.Authorization;

namespace FreeCourse.Services.Catalog.Requirements
{
    public class DenemeRequirement:IAuthorizationRequirement
    {
    }

    public class DenemeHandler : AuthorizationHandler<DenemeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DenemeRequirement requirement)
        {   
            var test = context.User.Claims.ToList();//buranın null olmasını kontrol et 

            return Task.CompletedTask;
        }
    }
}
