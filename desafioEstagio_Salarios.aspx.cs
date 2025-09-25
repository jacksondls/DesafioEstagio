using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.UI;

namespace DesafioEstagio
{
    public partial class Salarios : Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["OracleDB"].ConnectionString;

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                await CarregarCargosAsync();
                await CarregarSalariosAsync();
            }
        }

        // Carrega os cargos no DropDownList de forma assíncrona
        private async Task CarregarCargosAsync()
        {
            using (var conn = new OracleConnection(connectionString))
            {
                await conn.OpenAsync();
                OracleDataAdapter da = new OracleDataAdapter("SELECT cargo_id, cargo_nome FROM cargo", conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                DDLcargo.DataSource = dt;
                DDLcargo.DataTextField = "cargo_nome";
                DDLcargo.DataValueField = "cargo_id";
                DDLcargo.DataBind();
            }
        }

        // Carrega salários no GridView de forma assíncrona
        private async Task CarregarSalariosAsync()
        {
            using (var conn = new OracleConnection(connectionString))
            {
                await conn.OpenAsync();
                OracleDataAdapter da = new OracleDataAdapter(
                    "SELECT p.pessoa_id, p.pessoa_nome, c.cargo_nome, s.salario " +
                    "FROM pessoa p " +
                    "LEFT JOIN cargo c ON p.cargo_id = c.cargo_id " +
                    "LEFT JOIN pessoa_salario s ON p.pessoa_id = s.pessoa_id", conn);

                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewSalarios.DataSource = dt;
                GridViewSalarios.DataBind();
            }
        }

        // Inclui uma nova pessoa
        protected async void BtnIncluir_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new OracleConnection(connectionString))
                {
                    await conn.OpenAsync();
                    string sql = "INSERT INTO pessoa (pessoa_nome, cargo_id) VALUES (:nome, :cargo_id)";
                    using (var cmd = new OracleCommand(sql, conn))
                    {
                        cmd.Parameters.Add(new OracleParameter("nome", TxtNome.Text));
                        cmd.Parameters.Add(new OracleParameter("cargo_id", DDLcargo.SelectedValue));
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                lblMensagem.Text = "Pessoa incluída com sucesso!";
                TxtNome.Text = "";
                DDLcargo.SelectedIndex = 0;
                await CarregarSalariosAsync();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro: " + ex.Message;
            }
        }

        // Recalcula salários chamando a procedure Oracle
        protected async void BtnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                await RecalcularSalariosAsync();
                lblMensagem.Text = "Salários recalculados com sucesso!";
                await CarregarSalariosAsync();
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro: " + ex.Message;
            }
        }

        // Chama a procedure de forma assíncrona
        private async Task RecalcularSalariosAsync()
        {
            using (var conn = new OracleConnection(connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new OracleCommand("calcular_salarios", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Paginação do GridView
        protected async void GridViewSalarios_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            GridViewSalarios.PageIndex = e.NewPageIndex;
            await CarregarSalariosAsync();
        }
    }
}
