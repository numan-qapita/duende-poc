# duende-poc
Duende PoC for custom stuff:
 * Custom GrantType
 * Custom Claims per client
 * MySql for storage
 * Increased Token expiry to 3 hours (10800 s)

Setup a local docker container for MySql to talk to:
```
docker volume create mysql-qapita-volume
docker run --name=qapita-idp-mysql -p3306:3306 -v mysql-qapita-volume:/var/lib/mysql -e MYSQL_ROOT_PASSWORD=my-secret-pw -d mysql:latest
```

For migrations run (only needed when switching the storage db provider):
```
dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
```

Test locally using the following request
```
curl --insecure -X POST https://localhost:5001/connect/token \
-H 'Content-Type: application/x-www-form-urlencoded' \
-d "client_id=qapita.qfund.api&client_secret=super-secret&grant_type=tenant_delegation&scope=scope.qapita.qfund.api&tenant_id=42"
```
