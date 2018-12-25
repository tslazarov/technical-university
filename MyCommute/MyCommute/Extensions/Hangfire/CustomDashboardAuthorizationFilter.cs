using Hangfire.Annotations;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyCommute.Extensions.Hangfire
{
    public class CustomDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpcontext = context.GetHttpContext();
            var claims = httpcontext.User.Claims;
            var emailClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            return emailClaim != null ? emailClaim.Value == "lazarov.tsvetoslav@gmail.com" : false;
        }
    }
}
