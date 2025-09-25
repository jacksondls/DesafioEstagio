---

# Desafio Estágio – ASP.NET Web Forms + Oracle

## 📌 Sobre o Projeto

Aplicação ASP.NET Web Forms desenvolvida para o desafio de estágio, integrada a um banco de dados Oracle.
O objetivo principal foi **gerenciar pessoas e cargos**, além de gerar a tabela `pessoa_salario` com os salários calculados.

---

## ✅ Funcionalidades Implementadas

* CRUD de **Pessoa** (Incluir, Editar, Excluir, Listar).
* Associação de **Cargo** à Pessoa.
* Tabela `pessoa_salario` preenchida a partir das tabelas `pessoa` e `cargo`.
* Cálculo dos salários implementado no banco de dados.
* Página para **listagem de salários** com opção de calcular/recalcular.

### Diferenciais incluídos

* Processamento assíncrono do cálculo de salários.
---

## ⚙️ Requisitos

* Oracle Database 11g ou superior.
* Visual Studio 2019/2022 com suporte a **ASP.NET Web Forms**.
* .NET Framework 4.7.2 ou superior.
* Pacote **Oracle.ManagedDataAccess** instalado via NuGet.
---

## 🚀 Como Executar Localmente

1. Clone este repositório:

   ```bash
   git clone https://github.com/seuusuario/DesafioEstagio.git
   ```
2. Abra o projeto no **Visual Studio** (`.sln`).
3. Configure a **Connection String** no `Web.config` com os dados do seu Oracle:

   ```xml
   <connectionStrings>
     <add name="OracleDB" 
          connectionString="User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=localhost:1521/XEPDB1;" 
          providerName="Oracle.ManagedDataAccess.Client" />
   </connectionStrings>
   ```
4. Execute os scripts SQL (tabelas e inserts) disponíveis na pasta `/Database`.
5. Pressione **F5** para rodar a aplicação.
---

