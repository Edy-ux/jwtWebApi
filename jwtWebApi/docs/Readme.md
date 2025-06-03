## Diferença entre geração e validação do Audience no JWT

- **Geração do token:**  
O valor do campo `aud` (audience) do token JWT é definido no momento da criação do token, usando o valor passado no serviço responsável pela geração (por exemplo, `Audience = ...` no `SecurityTokenDescriptor`).  
A configuração `ValidAudience` da API **não interfere** na geração do token.

- **Validação do token:**  
Quando a API recebe um token, ela verifica se o valor do campo `aud` do token é igual ao valor configurado em `ValidAudience` (desde que `ValidateAudience = true`).  
Se forem diferentes, a API rejeita o token.

**Resumo:**  
- O token sempre será gerado, independentemente do valor de `ValidAudience`.
- O valor de `ValidAudience` só é usado para aceitar ou rejeitar tokens recebidos pela API.
- Para garantir que o token seja gerado com o audience correto, confira o código do serviço de geração do token e utilize o valor de configuração adequado (ex: `_jwtOptions.Audience`).
- Se quiser impedir a geração do token com audience errado, adicione uma validação manual no serviço de geração do token.