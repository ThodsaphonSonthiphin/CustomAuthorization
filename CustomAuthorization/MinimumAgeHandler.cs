using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CustomAuthorization;

public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    {
        var dateOfBirthClaim = context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth);
        
        if (dateOfBirthClaim != null)
        {
            var dateOfBirth = Convert.ToDateTime(dateOfBirthClaim.Value);
            int calculatedAge = DateTime.Today.Year - dateOfBirth.Year;
            
            if (calculatedAge >= requirement.MinimumAge)
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}