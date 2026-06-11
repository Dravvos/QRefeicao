# 🍽️ QRefeicao — Cardápios Digitais com QR Code

Plataforma para **criação e gerenciamento de cardápios digitais** acessados via **QR Code**. Desenvolvida em **C# com ASP.NET Core**, utiliza uma arquitetura em camadas com suporte a banco de dados relacional e **NoSQL**, além de autenticação de usuários e pipeline de CI/CD.

Restaurantes, lanchonetes e estabelecimentos alimentícios podem criar seu cardápio digital e disponibilizá-lo aos clientes através de um simples QR Code.

---

## ✨ Funcionalidades

- **Criação de cardápios digitais** com categorias e itens personalizados
- **Geração de QR Code** para acesso rápido ao cardápio pelo cliente
- **Gerenciamento de itens**: nome, descrição, preço e foto
- **Autenticação de usuários** (estabelecimentos) via Identity
- **Visualização pública** do cardápio pelos clientes (sem necessidade de login)
- Suporte a armazenamento **relacional (SQL)** e **não-relacional (NoSQL)**
- Pipeline de **CI/CD** automatizado via GitHub Actions

---

## 🏗️ Arquitetura em Camadas

```
QRefeicao/
├── QRefeicao.API           → Endpoints REST (Controllers, middlewares)
├── QRefeicao.BLL           → Regras de negócio
├── QRefeicao.Data          → Acesso a dados relacionais (EF Core / SQL Server)
├── QRefeicao.Data.NoSQL    → Acesso a dados não-relacionais (NoSQL)
├── QRefeicao.DTO           → Objetos de transferência de dados
└── QRefeicao.Identity      → Autenticação e gerenciamento de usuários
```

---

## 🛠️ Tecnologias Utilizadas

| Tecnologia              | Uso                                               |
|-------------------------|---------------------------------------------------|
| C# / ASP.NET Core       | Framework principal da API                        |
| Entity Framework Core   | ORM para acesso ao banco de dados relacional      |
| SQL Server              | Banco de dados relacional                         |
| NoSQL                   | Armazenamento de dados não-relacionais            |
| ASP.NET Identity / JWT  | Autenticação e autorização de usuários            |
| Geração de QR Code      | Criação de QR Codes para os cardápios             |
| GitHub Actions          | Pipeline de CI/CD automatizado                    |

---

## 📋 Pré-requisitos

- [.NET SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/) instalado e configurado
- Instância MongoDB configurada
- [Visual Studio](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/)

---

## 🚀 Como Executar

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/Dravvos/QRefeicao.git
   cd QRefeicao
   ```

2. **Configure as strings de conexão:**
   - Crie uma variável de ambiente chamada `QRConnection`
   - Atualize o valor dela com os dados da sua conexão PostgreSQL

3. **Aplique as migrations:**
   ```bash
   dotnet ef database update --project QRefeicao.Data --startup-project QRefeicao.API
   ```

4. **Execute a aplicação:**
   ```bash
   cd QRefeicao.API
   dotnet run
   ```

5. **Acesse a documentação (Swagger):**
   - Navegue até `https://localhost:{porta}/swagger`

---

## 📌 Endpoints Principais

| Método | Rota                         | Descrição                                      |
|--------|------------------------------|------------------------------------------------|
| POST   | `/api/auth/register`         | Cadastro do estabelecimento                    |
| POST   | `/api/auth/login`            | Login e geração de token JWT                   |
| GET    | `/api/cardapio/{id}`         | Visualiza cardápio público (acesso via QR Code)|
| POST   | `/api/cardapio`              | Cria um novo cardápio                          |
| PUT    | `/api/cardapio/{id}`         | Atualiza um cardápio existente                 |
| POST   | `/api/cardapio/{id}/item`    | Adiciona um item ao cardápio                   |
| DELETE | `/api/cardapio/Deleteitem/{itemId}` | Remove um item do cardápio              |

> Rotas de gerenciamento requerem autenticação via `Bearer Token`. A rota de visualização pública é aberta.

---

## 📁 Estrutura do Projeto

```
QRefeicao/
├── .github/workflows/          # CI/CD (GitHub Actions)
├── QRefeicao.API/              # Camada de apresentação (Controllers, Startup)
├── QRefeicao.BLL/              # Regras de negócio
├── QRefeicao.Data/             # Acesso a dados relacionais (EF Core)
├── QRefeicao.Data.NoSQL/       # Acesso a dados não-relacionais
├── QRefeicao.DTO/              # Objetos de transferência de dados
├── QRefeicao.Identity/         # Autenticação e identidade
├── QRefeicao.sln               # Solution file
├── LICENSE.txt                 # Licença MIT
└── README.md
```

---

## 🔐 Autenticação

A API utiliza **JWT (JSON Web Tokens)**. Após o login, inclua o token no header das requisições protegidas:

```
Authorization: Bearer {seu_token_aqui}
```

---

## 🤝 Contribuições

Contribuições são bem-vindas! Para contribuir:

1. Faça um **fork** do repositório
2. Crie uma branch: `git checkout -b feature/minha-feature`
3. Commit: `git commit -m 'feat: descrição da melhoria'`
4. Push: `git push origin feature/minha-feature`
5. Abra um **Pull Request**

---

## 📄 Licença

Este projeto está licenciado sob a [MIT License](LICENSE.txt).

---

Desenvolvido por [Daniel Oliveira Dias (Dravvos)](https://github.com/Dravvos)
