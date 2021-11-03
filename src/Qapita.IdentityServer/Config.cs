// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Duende.IdentityServer.Models;
using System.Collections.Generic;
using Qapita.IdentityServer.Constants;

namespace Qapita.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId()
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("scope.qapita.qfund.api", "Qapita QFund API",
                    new[] {QapitaClaimTypes.TenantId, QapitaClaimTypes.DelegatedSubject})
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "qapita.qfund.api",
                    AllowedGrantTypes = {"tenant_delegation",},
                    ClientSecrets = {new Secret("super-secret".Sha256())},
                    AllowedScopes = {"scope.qapita.qfund.api"}
                }
            };
    }
}