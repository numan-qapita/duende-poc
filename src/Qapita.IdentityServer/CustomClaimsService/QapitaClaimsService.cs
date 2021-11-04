// using System.Collections.Generic;
// using System.Linq;
// using System.Security.Claims;
// using System.Threading.Tasks;
// using Duende.IdentityServer.Services;
// using Duende.IdentityServer.Validation;
// using Microsoft.Extensions.Logging;
// using Qapita.IdentityServer.Constants;
//
// namespace Qapita.IdentityServer.CustomClaimsService
// {
//     public class QapitaClaimsService : DefaultClaimsService
//     {
//         public QapitaClaimsService(IProfileService profile, ILogger<QapitaClaimsService> logger) : base(profile, logger)
//         {
//             logger.LogInformation("ProfileType={profileType}", profile.GetType().FullName);
//         }
//
//         public override async Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ClaimsPrincipal subject,
//             ResourceValidationResult resourceResult,
//             ValidatedRequest request)
//         {
//             var claims = await base.GetAccessTokenClaimsAsync(subject, resourceResult, request);
//             //var tenantClaim = subject.FindFirst(QapitaClaimTypes.TenantId);
//             return claims; // tenantClaim == null ? claims : claims.Union(new[] {tenantClaim});
//         }
//     }
// }