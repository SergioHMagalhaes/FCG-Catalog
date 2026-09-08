# FCG Catalog API

API de catalogo da plataforma **FIAP Cloud Games (FCG)**, desenvolvida como parte do Tech Challenge da pos-graduacao.

O desafio da fase propõe a evolução de uma aplicação monolítica para uma arquitetura de microsserviços orientada a eventos. Dentro desse contexto, o domínio de Catalogo foi separado em dois repositórios: este repositório contem a **Catalog API**, responsável pela exposição HTTP do catalogo e por iniciar o fluxo de compra, enquanto o worker de catalogo fica em outro repositório.

## Grupo

- Igor Anthony - igor.anthony.iop@gmail.com
- Nathalia Greice - nponce410@gmail.com
- Otávio de Andrade - otavio_andrade@live.com
- Pedro Henrique Barros - pedrobarros0101@outlook.com
- Sérgio Henrique - ssergioh3@gmail.com

## Sobre o projeto

A Catalog API e o microsserviço responsável por gerenciar o catalogo de jogos da FCG. Ela permite consultar jogos e categorias, executar operações administrativas de cadastro, atualização e remoção, consultar pedidos e biblioteca do usuário, além de publicar eventos para iniciar o fluxo assíncrono de compra.

No fluxo de compra, a API recebe a solicitação para adicionar um jogo a biblioteca do usuário, cria um pedido com status pendente e publica o evento `OrderPlacedEvent` via RabbitMQ. Esse evento deve ser consumido pelo microsserviço de pagamentos, conforme a arquitetura proposta no Tech Challenge.

## Principais funcionalidades

- CRUD de categorias.
- CRUD de jogos.
- Listagem paginada e ordenada de jogos.
- Cache distribuído com Redis (padrão Cache-Aside e invalidação automática).
- Criação de pedidos de compra de jogos.
- Consulta de pedidos do usuário.
- Consulta de pedido por identificador.
- Consulta da biblioteca do usuário.
- Publicação de eventos com RabbitMQ e MassTransit.
- Persistência com PostgreSQL e Entity Framework Core.
- Health check em `/Health` (incluindo status do PostgreSQL, RabbitMQ e Redis).
- Documentação Swagger em ambiente de desenvolvimento.
- Manifestos Kubernetes em `k8s/`.

## Tecnologias

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Redis / StackExchange.Redis
- RabbitMQ
- MassTransit
- JWT Bearer
- Swagger / OpenAPI
- Docker
- Kubernetes
- xUnit

## Estrutura do repositório

```text
src/
  FCG.Catalog.Api/             # Entrada HTTP, controllers, filtros e configuração da API
  FCG.Catalog.Application/     # Casos de uso e regras de aplicação
  FCG.Catalog.Communication/   # Requests, responses e enums expostos pela API
  FCG.Catalog.Domain/          # Entidades, repositórios, serviços de domínio e contratos
  FCG.Catalog.Exception/       # Exceções de negocio
  FCG.Catalog.Infrastructure/  # EF Core, PostgreSQL, RabbitMQ, Redis e implementações externas
  FCG.Shared/                  # Eventos compartilhados
tests/
  CommonTestUtilities/         # Builders e utilitários para testes
  UseCases.Test/               # Testes unitários dos casos de uso
k8s/                           # Manifests de Deployment, Service, ConfigMap e Secret
```

## Pre-requisitos

- .NET SDK 10 ou superior.
- Docker e Docker Compose.
- PostgreSQL, caso nao utilize o `docker-compose.yml` do projeto.
- Redis, caso nao utilize o `docker-compose.yml` do projeto.
- RabbitMQ, caso nao utilize o `docker-compose.yml` do projeto.
- Opcional: Kubernetes local, como Docker Desktop Kubernetes, Kind, Minikube ou k3d.

## Configuração

Em desenvolvimento, as configurações principais estão em `src/FCG.Catalog.Api/appsettings.Development.json`.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=seu_banco;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "SigningKey": "sua-chave-jwt",
    "Issuer": "FCGames"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "VirtualHost": "/",
    "Username": "guest",
    "Password": "guest"
  },
  "Redis": {
    "ConnectionString": "localhost:6379",
    "InstanceName": "fcg:catalog:",
    "DefaultTtlMinutes": 15
  }
}
```

Variáveis esperadas:

| Variável | Descrição |
| --- | --- |
| `ConnectionStrings__DefaultConnection` | Connection string do PostgreSQL. |
| `Jwt__SigningKey` | Chave usada para validar tokens JWT. |
| `Jwt__Issuer` | Emissor configurado para os tokens. |
| `RabbitMQ__Host` | Host do RabbitMQ. |
| `RabbitMQ__VirtualHost` | Virtual host do RabbitMQ. |
| `RabbitMQ__Username` | Usuario do RabbitMQ. |
| `RabbitMQ__Password` | Senha do RabbitMQ. |
| `Redis__ConnectionString` | Connection string do Redis (ex: `localhost:6379` ou `redis:6379`). |
| `Redis__InstanceName` | Prefixo/namespace das chaves no Redis (ex: `fcg:catalog:`). |
| `Redis__DefaultTtlMinutes` | Tempo padrão de expiração do cache em minutos (ex: `15`). |

## Estratégia de Caching com Redis

A aplicação adota o padrão **Cache-Aside** com suporte a **resiliência graciosa** através do `RedisCacheService`:

- **Consultas**: As listagens e consultas por identificador de jogos (`games:*`) e categorias (`categories:*`) consultam primeiramente o Redis. Em caso de *cache miss*, os dados são buscados no PostgreSQL e gravados no Redis com TTL configurável.
- **Invalidação**: Operações de mutação (cadastro, atualização e exclusão de jogos ou categorias) invalidam automaticamente as chaves específicas e coleções afetadas via prefixo.
- **Resiliência e Fallback**: Se o Redis estiver offline ou ocorrer falha de conectividade, a aplicação efetua fallback transparente para o banco de dados sem interromper as requisições, registrando logs de aviso (*warning*).
- **Health Check**: A integridade do Redis é monitorada e reportada no endpoint `/Health`.

## Executando localmente

Suba as dependências locais de banco, cache e mensageria:

```bash
docker compose -f src/FCG.Catalog.Api/docker-compose.yml up -d
```

Restaure os pacotes e execute a API:

```bash
dotnet restore FGC.Catalog.slnx
dotnet run --project src/FCG.Catalog.Api/FCG.Catalog.Api.csproj
```

Com o perfil HTTP, a API fica disponível em:

- API: `http://localhost:5117`
- Swagger: `http://localhost:5117/swagger`
- Health check: `http://localhost:5117/Health`
- RabbitMQ Management: `http://localhost:15672`

As migrations do Entity Framework Core sao aplicadas automaticamente na inicialização da api.

## Docker

Build da imagem:

```bash
docker build -t fcg-catalog-api .
```

Execução da API em container:

```bash
docker run --rm -p 8080:8080 \
  -e ASPNETCORE_URLS=http://+:8080 \
  -e ConnectionStrings__DefaultConnection="Host=host.docker.internal;Port=5433;Database=fcg_catalogdb;Username=postgres;Password=postgres" \
  -e Jwt__SigningKey="sua-chave-jwt" \
  -e Jwt__Issuer="FCGames" \
  -e RabbitMQ__Host="host.docker.internal" \
  -e RabbitMQ__VirtualHost="/" \
  -e RabbitMQ__Username="rabbitmq" \
  -e RabbitMQ__Password="rabbitmq" \
  -e Redis__ConnectionString="host.docker.internal:6379" \
  -e Redis__InstanceName="fcg:catalog:" \
  -e Redis__DefaultTtlMinutes="15" \
  fcg-catalog-api
```

## Kubernetes

Os manifests estão na pasta `k8s/`:

- `catalog-api-deployment.yaml`
- `catalog-api-service.yaml`
- `catalog-api-configmap.yaml`
- `catalog-api-secret.yaml`

Antes de aplicar os manifests, preencha os valores sensíveis em `k8s/catalog-api-secret.yaml`:

```yaml
stringData:
  ConnectionStrings__DefaultConnection: "Host=postgres;Port=5432;Database=seu_banco;Username=postgres;Password=postgres"
  Jwt__SigningKey: "sua-chave-jwt"
  RabbitMQ__Username: "guest"
  RabbitMQ__Password: "guest"
```

As configurações não sensíveis (incluindo host do RabbitMQ e Redis) são definidas em `k8s/catalog-api-configmap.yaml`:

```yaml
data:
  ASPNETCORE_ENVIRONMENT: "Development"
  ASPNETCORE_URLS: "http://+:8080"
  Jwt__Issuer: "FCGames"
  RabbitMQ__Host: "rabbitmq"
  RabbitMQ__VirtualHost: "/"
  Redis__ConnectionString: "redis:6379"
  Redis__InstanceName: "fcg:catalog:"
  Redis__DefaultTtlMinutes: "15"
```

Aplicação dos manifests:

```bash
kubectl apply -f k8s/
kubectl get pods
kubectl get services
```

O service `catalog-api` expoe a porta `8081` e encaminha para a porta `8080` do container.

## Autenticação e autorização

A API utiliza JWT Bearer. Para acessar rotas protegidas, faça login na api de usuário e envie o token no header:

```http
Authorization: Bearer seu-token-jwt
```

Operações administrativas de jogos e categorias exigem a role `ADMIN`. Operações de pedidos e biblioteca exigem usuário autenticado.

## Postman

O repositório contem a coleção `postman_collection.json`, que pode ser importada no Postman para testar os endpoints da API.
