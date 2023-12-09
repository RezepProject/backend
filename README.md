# Backend

## Setup Database

1. Start docker desktop
2. run `docker pull postgres`
3. run `docker run --name rezep-database -e POSTGRES_PASSWORD=postgres -p 5432:5432 -d postgres`
4. confirm that the container is running by using the command `docker ps`

### Test-User Setup

```sql
insert into role values (1, 'ADMIN');
insert into configuser values (7,'test','test','test','$2a$11$TxzkGMQgywQjBxMq9YcOoO66hQODh5zJzIg4npGPDzfpcefvKORD2',1)
```

## Setup Backend

1. Download and install Nugets
2. Setup appsettings.json
```
{
  "Urls": "http://localhost:5260",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Mail": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Address": "<EMAIL ADDRESS>",
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

3. Run the application

## Known Issues

If you have a problem with migrations, try to drop all tables and (create) run the migrations again.