using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using Microsoft.Extensions.Logging;
using Qapita.IdentityServer.Constants;

namespace Qapita.IdentityServer.CustomGrants
{
    public class QapitaTenantGrantValidator : IExtensionGrantValidator
    {
        private const string TenantDelegationGrantType = "tenant_delegation";
        private readonly ILogger<QapitaTenantGrantValidator> _logger;

        public QapitaTenantGrantValidator(ILogger<QapitaTenantGrantValidator> logger)
        {
            _logger = logger;
        }

        public Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var tenantId = context.Request.Raw.Get(QapitaClaimTypes.TenantId);

            if (string.IsNullOrEmpty(tenantId))
            {
                context.Result =
                    new GrantValidationResult(TokenRequestErrors.InvalidRequest,
                        $"required parameter [{QapitaClaimTypes.TenantId}] missing");
                return Task.CompletedTask;
            }

            _logger.LogInformation($"{QapitaClaimTypes.TenantId} received: {tenantId}");

            context.Result = new GrantValidationResult("nauman", GrantType,
                new[]
                {
                    new Claim(QapitaClaimTypes.TenantId, tenantId),
                    new Claim(QapitaClaimTypes.DelegatedSubject, "vamsee")
                });
            return Task.CompletedTask;
        }

        public string GrantType => TenantDelegationGrantType;
    }
}