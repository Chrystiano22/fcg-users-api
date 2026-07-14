# FCG UsersAPI

Microsservico responsavel por usuarios, autenticacao, autorizacao e emissao de JWT da plataforma FIAP Cloud Games.

## Responsabilidades

- Cadastro de usuarios.
- Login.
- Geracao de JWT.
- Consulta do usuario autenticado.
- Administracao basica de usuarios.
- Publicacao de `UserCreatedEvent` via log local ou RabbitMQ/MassTransit.

## Endpoints

- `POST /usuarios`
- `POST /auth/login`
- `GET /usuarios/me`
- `GET /usuarios`
- `PATCH /usuarios/{userId}/perfil`
- `GET /health`

## Execucao local

```powershell
dotnet restore
dotnet run --project src/Fcg.Users.Api --launch-profile http
```

Swagger:

- `http://localhost:5101/swagger`

Health check:

- `http://localhost:5101/health`

Administrador local semeado automaticamente:

- email: `admin@fcg.local`
- senha: `Admin@123`

## Variaveis de ambiente

- `ASPNETCORE_ENVIRONMENT`
- `ASPNETCORE_URLS`
- `ConnectionStrings__UsersDb`
- `Jwt__Issuer`
- `Jwt__Audience`
- `Jwt__SecretKey`
- `Jwt__ExpirationInMinutes`
- `RabbitMq__Enabled`
- `RabbitMq__Host`
- `RabbitMq__VirtualHost`
- `RabbitMq__Username`
- `RabbitMq__Password`

## Mensageria

- Exchange publicado: `fcg.user-created`
- Publicador padrao local: `LoggingEventPublisher`
- Publicador com broker: `MassTransitEventPublisher`
- Serializacao com broker: raw JSON, para interoperar com contratos duplicados entre repositorios.
- Para usar RabbitMQ, configure `RabbitMq__Enabled=true`.

## Validacao

```powershell
dotnet restore
dotnet build --no-restore
dotnet test --no-build --no-restore
```
