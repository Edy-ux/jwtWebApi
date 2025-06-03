## Diferença entre geração e validação do Audience no JWT

- **Geração do token:**  
O valor do campo `aud` (audience) do token JWT é definido no momento da criação do token, usando o valor passado no serviço responsável pela geração (por exemplo, `Audience = ...` no `JwtSecurityToken`).  
A configuração `ValidAudience` da API **não interfere** na geração do token.

- **Validação do token:**  
Quando a API recebe um token, ela verifica se o valor do campo `aud` do token é igual ao valor configurado em `ValidAudience` (desde que `ValidateAudience = true`).  
Se forem diferentes, a API rejeita o token.

**Resumo:**  
- O token sempre será gerado, independentemente do valor de `ValidAudience`.
- O valor de `ValidAudience` só é usado para aceitar ou rejeitar tokens recebidos pela API.
- Para garantir que o token seja gerado com o audience correto, confira o código do serviço de geração do token e utilize o valor de configuração adequado (ex: `_jwtOptions.Audience`).
- Se quiser impedir a geração do token com audience errado, adicione uma validação manual no serviço de geração do token.


## Exemplo de geração de Refresh Token seguro

Para gerar um refresh token seguro em .NET, utilize um array de bytes aleatórios e converta para Base64. Exemplo prático:

```csharp
using System.Security.Cryptography;

public class RefreshToken
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; } = string.Empty;
}

public RefreshToken GenerateRefreshToken(string ipAddress)
{
    var randomBytes = new byte[64];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomBytes);

    return new RefreshToken
    {
        Token = Convert.ToBase64String(randomBytes), // Este é o novo refresh token seguro
        Expires = DateTime.UtcNow.AddDays(7),
        Created = DateTime.UtcNow,
        CreatedByIp = ipAddress
    };
}
```

**Dicas de produção:**
- Salve o refresh token no banco de dados, associado ao usuário.
- Sempre gere um novo refresh token ao renovar o JWT.
- Invalide o refresh token antigo após o uso.