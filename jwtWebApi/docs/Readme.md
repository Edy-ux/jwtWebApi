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
  - [Fluxo de Autenticação e Refresh Token](#fluxo-de-autenticação-e-refresh-token)
  - [Boas Práticas e Segurança](#boas-práticas-e-segurança)
  - [Como Contribuir](#como-contribuir)
- [EF CORE](#ef-core)
    - [🧠 Como o EF Core rastreia alterações em coleções](#-como-o-ef-core-rastreia-alterações-em-coleções)

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

---

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

## Como Contribuir

1. Faça um fork do projeto
2. Crie uma branch (`git checkout -b feature/nova-feature`)
3. Commit suas alterações (`git commit -am 'Adiciona nova feature'`)
4. Push para a branch (`git push origin feature/nova-feature`)
5. Abra um Pull Request



# EF CORE
### 🧠 Como o EF Core rastreia alterações em coleções


```csharp
  var rt = _refreshTokens.FirstOrDefault(t => t.Token == token);
```
**O EF Core usa um mecanismo chamado Change Tracker, que faz o seguinte:**

Quando você carrega a entidade do banco (Include(u => u.RefreshTokens)), o EF cria um snapshot interno do estado atual da entidade e da coleção.

Quando você altera a coleção, mesmo que seja por um campo privado como _refreshTokens, se o EF tiver acesso a esse campo (via SetPropertyAccessMode(Field)), ele detecta a diferença entre o snapshot original e o novo estado.

No momento do SaveChanges(), o EF compara:

O que existia antes


**qual a melhor abordagem remover o refreshToken ou marcalo com IsExpired = true?**

<p>

A melhor abordagem é marcar o refresh token como inválido <strong>(soft delete)</strong>.Por exemplo, marcando o prop IsExpired ou IsActive para true, em vez de removê-lo fisicamente do banco de dados.</p>


**Motivos para marcar como inválido (soft delete):**

- Auditoria e segurança: Você mantém o histórico de uso dos tokens, podendo - identificar tentativas de reutilização ou ataques.
- Reutilização: Permite bloquear tokens antigos e evitar que sejam usados novamente.
- Debug e rastreabilidade: Facilita investigações futuras sobre acessos e problemas de autenticação.


```csharp

public async Task<(string accessToken, string refreshToken)> AuthenticateAsync(string login, string password, string ipAddress)
{
    const int maxRetries = 3;
    int retryCount = 0;

    while (true)
    {
        using var context = _contextFactory.CreateDbContext();

        var user = await context.Users
            .Include(u => u.RefreshTokens)
            .FirstOrDefaultAsync(u => u.Login == login);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new UnauthorizedAccessException("User or passoword invalid.");

        var accessToken = _tokenService.GenerateToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken(ipAddress);

        user.AddRefreshToken(refreshToken);

        try
        {
            await context.SaveChangesAsync();
            return (accessToken, refreshToken.Token);
        }
        catch (DbUpdateConcurrencyException)
        {
            retryCount++;
            if (retryCount >= maxRetries)
                throw new InvalidOperationException("Conflito de concorrência ao salvar o refresh token. Tente novamente mais tarde.");

            // Aguarda um pequeno tempo antes de tentar novamente (opcional)
            await Task.Delay(100);

            // Continua o loop para tentar novamente
        }
    }
}

```

**Dúvidas ou sugestões?**  
Abra uma issue ou entre em contato!