# Azure SignalR Services Lab

## Run

Configure remote environment:

```sh
$ cd src/environment
$ az login
$ terraform init
$ terraform apply
```

Configure applications secrets:

```sh
$ cd src/environment
# Set Azure SignalR Service Connection String to API project
$ dotnet user-secrets --project ../backend/Backend.Api set "Azure:SignalR:ConnectionString" "$(terraform output -raw SignalRServiceConnectionString)"
# Set Azure SignalR Service Connection String to Jobs project
$ dotnet user-secrets --project ../backend/Backend.Job set "Azure:SignalR:ConnectionString" "$(terraform output -raw SignalRServiceConnectionString)"
```

Run backend applications:

```sh
$ cd src/backend
$ dotnet watch run --project Backend.Api
$ dotnet watch run --project Backend.Job
```

Configure frontend application:

- Rename `src/frontend/.env.example` to `src/frontend/.env.local`
- Fix URLs to match the running port for API application

Run frontend application:

```sh
$ cd src/frontend
$ npm ci
$ npm start
```
