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
  "Mail": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Address": "<EMAIL ADDRESS>
    "Password": "<APP SPECIFIC PASSWORT>"
  },
  "Jwt": {
    "Key": "<SHA356 KEY>",
    "Issuer": "http://localhost:PORT",
    "Audience": "http://localhost:PORT"
  },
  "AllowedHosts": "*",
  "ConnectionString": "Host=localhost;Database=database;Username=name;Password=password"
}
```