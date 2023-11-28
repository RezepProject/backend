# Backend

## Setup Database

1. Start docker desktop
2. run `docker pull postgres`
3. run `docker run --name rezep-database -e POSTGRES_PASSWORD=postgres -p 5432:5432 -d postgres`
4. confirm that the container is running by using the command `docker ps`

## Setup Backend

1. Download and install Nugets
2. Setup appsettings.json
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Jwt": {
    "Key": "<HASH KEY SHA384>",
    "Issuer": "<ADDRESS (ex. http://localhost:5260)>",
    "Audience": "<ADDRESS (ex. http://localhost:5260)>",
  },
  "AllowedHosts": "*",
  "ConnectionString": "<DB CONNECTION STRING (ex. Host=localhost;Database=rezep-database;Username=postgres;Password=postgres)>"
}

```