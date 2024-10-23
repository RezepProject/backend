# Backend

## Setup Database

1. Start docker desktop
2. run `docker pull postgres`
3. run `docker run --name rezep-database -e POSTGRES_PASSWORD=postgres -p 5432:5432 -d postgres`
4. confirm that the container is running by using the command `docker ps`

### Test-User Setup

```sql
insert into role values (1, 'ADMIN');
insert into configuser (first_name, last_name, email, password, role_id, refresh_token, token_created, token_expires)
values ('test', 'test', 'test', '$2a$11$TxzkGMQgywQjBxMq9YcOoO66hQODh5zJzIg4npGPDzfpcefvKORD2', 1, 'refresh_token_value', current_timestamp, current_timestamp + interval '7 days');
```

## Setup Backend

1. Download and install Nugets
2. Setup appsettings.json & secrets.json
3. Run the application

## Known Issues

If you have a problem with migrations, try to drop all tables and (create) run the migrations again.

If the error "Unhandled exception. System.ArgumentException: Host can't be null" is shown on startup, the appsettings.json is most likely missing.

## Setup Minikube

1. https://minikube.sigs.k8s.io/docs/start/?arch=%2Fwindows%2Fx86-64%2Fstable%2F.exe+download
2. minikube -p minikube docker-env --shell powershell | Invoke-Expression
3. docker build -t backend:latest -f Backend/Dockerfile .
4. kubectl apply -f ./Backend/k8s/app.yaml  

