# jwtWebApi

API de autenticação JWT com refresh token, modelagem segura e boas práticas em .NET

---

## Sumário

- [jwtWebApi](#jwtwebapi)
  - [Sumário](#sumário)
  - [Sobre o Projeto](#sobre-o-projeto)
  - [Tecnologias Utilizadas](#tecnologias-utilizadas)
  - [Como Rodar o Projeto](#como-rodar-o-projeto)
  - [Configuração de Ambiente](#configuração-de-ambiente)
  - [Exemplos de Uso da API](#exemplos-de-uso-da-api)
    - [1. Registrar Usuário](#1-registrar-usuário)
    - [2. Login e obtenção de tokens](#2-login-e-obtenção-de-tokens)
    - [3. Acesso a endpoint protegido](#3-acesso-a-endpoint-protegido)
    - [4. Renovar JWT usando refresh token](#4-renovar-jwt-usando-refresh-token)
  - [Como rodar](#como-rodar)
  - [Fluxo de Autenticação e Refresh Token](#fluxo-de-autenticação-e-refresh-token)
  - [Boas Práticas e Segurança](#boas-práticas-e-segurança)
- [🌐 Mensagens de Erro e Validação com Tradução Automática](#-mensagens-de-erro-e-validação-com-tradução-automática)
  - [✅ Como funciona](#-como-funciona)
  - [📥 Exemplo de envio com cultura definida](#-exemplo-de-envio-com-cultura-definida)
  - [🔎 Exemplo de resposta de erro (en-US)](#-exemplo-de-resposta-de-erro-en-us)
  - [📁 Estrutura de Localização](#-estrutura-de-localização)
  - [⚙️ Configuração em `Program.cs`](#️-configuração-em-programcs)
  - [Exemplo usando placeholdes `Program.cs`.](#exemplo-usando-placeholdes-programcs)
  - [Como Contribuir](#como-contribuir)

---

## Sobre o Projeto

Esta API demonstra autenticação baseada em JWT (JSON Web Token) com suporte a refresh token, controle de usuários, roles e boas práticas de modelagem e segurança.  
Ideal para estudos, portfólio e como base para aplicações reais.

---

## Tecnologias Utilizadas

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- JWT Bearer Authentication
- User Secrets (para configuração segura)
- Swagger (para documentação e testes)

---

## Como Rodar o Projeto

1. **Clone o repositório**
   ```sh
   git clone https://github.com/seuusuario/jwtWebApi.git
   cd jwtWebApi
   ```

2. **Configure o banco de dados**
   - Altere a connection string no `appsettings.json` ou use User Secrets.

3. **Configure os secrets do JWT**
   ```sh
   dotnet user-secrets set "JWT:Secret_Key" "sua-chave-secreta"
   dotnet user-secrets set "JWT:Audience" "www.seuapp.com"
   ```

4. **Rode as migrations**
   ```sh
   dotnet ef database update
   ```

5. **Execute a aplicação**
   ```sh
   dotnet run
   ```

6. **Acesse o Swagger**
   - Normalmente em: [https://localhost:5001/swagger](https://localhost:5001/swagger)

---

## Configuração de Ambiente

- **appsettings.json**: configure connection string e outras opções.
- **User Secrets**: use para armazenar chaves sensíveis (JWT, etc).

---

## Exemplos de Uso da API

### 1. Registrar Usuário

```http
POST /api/v1/auth/register
Content-Type: application/json

{
  "login": "edy",
  "username": "Edy",
  "password": "Senha123!",
  "confirmePassword": "Senha123!",
  "email": "edy@email.com",
  "roles": ["Admin"]
}
```

### 2. Login e obtenção de tokens

```http
POST /api/v1/auth/login
Content-Type: application/json

{
  "login": "edy",
  "password": "Senha123!"
}
```
**Resposta:**
```json
{
  "accessToken": "<jwt_token>",
  "refreshToken": "<refresh_token>"
}
```

### 3. Acesso a endpoint protegido

```http
GET /api/v1/user/profile
Authorization: Bearer <jwt_token>
```

### 4. Renovar JWT usando refresh token

```http
POST /api/v1/auth/refresh
Content-Type: application/json

{
  "refreshToken": "<refresh_token>"
}
```
**Resposta:**
```json
{
  "accessToken": "<novo_jwt_token>",
  "refreshToken": "<novo_refresh_token>"
}
```


**Migrations e EF Core**
## Como rodar

```bash
dotnet restore
dotnet ef database update
dotnet run
```

## Fluxo de Autenticação e Refresh Token

1. Usuário faz login e recebe um JWT e um refresh token.
2. Usa o JWT para acessar endpoints protegidos.
3. Quando o JWT expira, usa o refresh token para obter um novo JWT.
4. O refresh token antigo é invalidado e um novo é emitido.

---

## Boas Práticas e Segurança

- Refresh tokens são salvos no banco, relacionados ao usuário.
- JWTs são assinados com chave secreta segura.
- Roles são usadas para autorização de endpoints.
- Dados sensíveis nunca ficam hardcoded no código.
- Validação de entrada e tratamento de erros implementados.

---



# 🌐 Mensagens de Erro e Validação com Tradução Automática

Esta API possui suporte completo a **localização (i18n)** para mensagens de **validação de dados**, retornando os erros automaticamente no idioma apropriado com base na cultura definida na requisição.

## ✅ Como funciona

- A API utiliza **Data Annotations** em conjunto com arquivos `.resx` de tradução.
- A cultura (`CultureInfo`) é detectada automaticamente via header HTTP `Accept-Language`.
- Atualmente são suportados os idiomas:
  - `pt-BR` (Português do Brasil)
  - `en-US` (Inglês Americano)

## 📥 Exemplo de envio com cultura definida

Ao fazer uma requisição HTTP para a API, basta definir o cabeçalho:

```http
Accept-Language: en-US
```

Ou:

```http
Accept-Language: pt-BR
```

A resposta de erro de validação será retornada automaticamente no idioma solicitado.

## 🔎 Exemplo de resposta de erro (en-US)

```json
{
  "errors": {
    "Password": [
      "Password must be at least 6 characters long."
    ]
  }
}
```



## 📁 Estrutura de Localização

Os arquivos de tradução estão localizados na pasta:

```
/Resources/JwtWebApi.Dto.UserDto.[cultura].resx
/Resources/Outros..
```

Por exemplo:

- `JwtWebApi.Dto.UserDto.en-US.resx`
- `JwtWebApi.Dto.UserDto.pt-BR.resx`

## ⚙️ Configuração em `Program.cs`

```csharp
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddControllers()
    .AddDataAnnotationsLocalization();

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("pt-BR"),
    SupportedCultures = new[] { new CultureInfo("pt-BR"), new CultureInfo("en-US") },
    SupportedUICultures = new[] { new CultureInfo("pt-BR"), new CultureInfo("en-US") }
});
```

## Exemplo usando placeholdes `Program.cs`.

Como substituir values em tempo de execução.

/Resoures/`OrderMessages.resx`
```xml
<data name="OrderNotification" xml:space="preserve">
  <value>Your order ("{0}") has been shipped and is on its way to "{1}".</value>
</data>

```

```csharp
app.MapGet("/{orderId}/{address}", (IStringLocalizerFactory factory, string orderId, string address) =>
{
    var localizer = factory.Create("OrderMessages", typeof(Program).Assembly.GetName().Name!);

    //logica para obter a mensagem de notificação do pedido

    // Substituindo os placeholders {0} e {1}

    string message = string.Format(localizer["OrderNotification"], orderId, address);

    return message;
});


```

**Dúvidas ou sugestões?**  
Abra uma issue ou entre em contato!

---


## Como Contribuir

1. Faça um fork do projeto
2. Crie uma branch (`git checkout -b feature/nova-feature`)
3. Commit suas alterações (`git commit -am 'Adiciona nova feature'`)
4. Push para a branch (`git push origin feature/nova-feature`)
5. Abra um Pull Request

---