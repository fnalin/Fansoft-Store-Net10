# 🚀 DemoAuth - Autenticação e Autorização com ASP.NET Core MVC + Minimal API (.NET 10)

Este projeto demonstra uma arquitetura moderna de **autenticação e autorização desacopladas**, utilizando:

- ASP.NET Core MVC (Front-end / BFF)
- ASP.NET Core Minimal API (Back-end)
- JWT interno entre aplicações
- Autorização dinâmica baseada em banco de dados
- Simulação de Active Directory (mock)

---

## 🧠 Conceito da Arquitetura

```
Usuário → MVC (simula AD) → API → Banco (permissões)
```

### 🔐 Autenticação
- O usuário realiza login no **MVC**
- O MVC simula o Active Directory
- Após autenticação, é criado um **cookie local**

### 🔑 Comunicação MVC → API
- O MVC gera um **JWT interno**
- Esse token contém apenas **identidade do usuário**
- O token é enviado para a API via:

```
Authorization: Bearer <token>
```

### 🛡️ Autorização
- A API valida o token (assinatura, issuer, audience)
- A API consulta o banco:
  - Usuário
  - Permissões por módulo
- A autorização é feita via **Policies + AuthorizationHandlers customizados**

---

## 📦 Tecnologias utilizadas

- .NET 10
- ASP.NET Core MVC
- ASP.NET Core Minimal API
- JWT Bearer Authentication
- Entity Framework Core (InMemory)
- Policy-based Authorization

---

## 🔐 Modelo de Segurança

### ✔ O que é seguro

- Tokens são assinados (HMAC)
- A API só aceita tokens válidos
- O front (MVC) é o único emissor do token
- A API **não confia no front para permissões**
- Permissões são validadas **no backend**

### ⚠️ O que é mock na demo

- Login (simula Active Directory)
- Base de dados em memória
- Chave JWT compartilhada (simétrica)

---

## 👥 Usuários de teste

Senha para todos: `123`

| Usuário | Descrição |
|--------|----------|
| fulano | Financeiro Visualizador + Compras Atendimento |
| ciclano | Financeiro Editor + Atendimento |
| maria | Compras Editor + Atendimento + Financeiro Visualizador |
| ze | Admin Financeiro + Admin Compras |
| ti | God Mode (acesso total) |

---

## 📊 Matriz de Permissões

| Usuário | Financeiro Produtos | Financeiro Chamados | Compras Pedidos | Compras Chamados |
|--------|--------------------|---------------------|------------------|------------------|
| fulano | ✔ | ✖ | ✔ | ✔ |
| ciclano | ✔ | ✔ | ✖ | ✖ |
| maria | ✔ | ✖ | ✔ | ✔ |
| ze | ✔ | ✖ | ✔ | ✔ |
| ti | 🔓 Tudo | 🔓 Tudo | 🔓 Tudo | 🔓 Tudo |

---

## 🧩 Modelo de Permissão

```
Usuário → Módulo → Nível + Atendimento
```

### Níveis:

- Visualizador (1)
- Editor (2)
- Admin (3)

### Ação adicional:

- Atendimento (CanHandleTickets)

---

## 🏗️ Estrutura do Projeto

```
DemoAuth/
├── DemoAuth.Api
│   ├── Minimal API
│   ├── JWT Validation
│   ├── AuthorizationHandlers
│   └── Banco (InMemory)
│
└── DemoAuth.Mvc
    ├── Login (Mock AD)
    ├── Cookie Auth
    ├── JWT interno
    ├── HttpClient
    └── Views
```

---

## 🔄 Fluxo completo

```
1. Usuário faz login no MVC
2. MVC valida (mock AD)
3. MVC cria cookie
4. MVC chama API
5. MVC gera JWT interno
6. API valida token
7. API consulta permissões no banco
8. API autoriza ou bloqueia
9. MVC renderiza resposta
```

---

## ⚙️ Como executar

### 1. Rodar a API

```bash
cd DemoAuth.Api
dotnet run
```

### 2. Rodar o MVC

```bash
cd DemoAuth.Mvc
dotnet run
```

### 3. Acessar

```
https://localhost:{porta-mvc}
```

---

## 🧪 Testes

- Acesse a tela de login
- Utilize os usuários da tabela
- Navegue pelos menus:
  - Financeiro → Produtos / Chamados
  - Compras → Pedidos / Chamados
- Observe o comportamento conforme as permissões

---

## 🧠 Decisões arquiteturais

### ❌ Não usamos Roles fixas

Problema:

```
FinanceiroAdmin
FinanceiroEditor
ComprasAdmin
...
```

Explode em complexidade.

---

### ✅ Usamos Policies + banco

Benefícios:

- Flexível
- Dinâmico
- Escalável
- Fácil manutenção

---

## 🚀 Evolução para Produção

Para produção, recomenda-se:

### 🔐 Autenticação
- Integrar com **Active Directory real**
- Ou usar:
  - OpenIddict
  - Azure Entra ID
  - Keycloak

---

### 🔑 Tokens
- Usar **chave assimétrica (RSA)**
- MVC assina com chave privada
- API valida com chave pública

---

### ♻️ Tokens
- Access Token: 5–15 minutos
- Refresh Token

---

### 🗄️ Banco
- Substituir InMemory por:
  - SQL Server
  - PostgreSQL

---

### ⚡ Performance
- Cache de permissões (Redis)

---

## 📌 Conclusão

Este projeto demonstra:

- Separação clara entre **autenticação e autorização**
- Uso de **JWT interno entre aplicações**
- Autorização dinâmica baseada em dados
- Arquitetura preparada para evolução em produção

---

## 👨‍💻 Autor
Fabiano Nalin
