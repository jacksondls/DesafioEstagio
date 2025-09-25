using System;
using System.Configuration;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using System.Web.UI.WebControls;

namespace DesafioEstagio
{
    public partial class Pessoas : System.Web.UI.Page
    {
        // Connection string do Oracle
        string strConexao = ConfigurationManager.ConnectionStrings["OracleDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CarregarPessoas();
                CarregarCargos();
            }
        }

        private void CarregarPessoas()
        {
            using (OracleConnection con = new OracleConnection(strConexao))
            {
                string query = @"SELECT p.pessoa_id, p.pessoa_nome, p.cargo_id, c.cargo_nome
                                 FROM pessoa p
                                 LEFT JOIN cargo c ON p.cargo_id = c.cargo_id";

                OracleDataAdapter da = new OracleDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewPessoa.DataSource = dt;
                GridViewPessoa.DataBind();
            }
        }

        private void CarregarCargos()
        {
            using (OracleConnection con = new OracleConnection(strConexao))
            {
                string query = "SELECT cargo_id, cargo_nome FROM cargo";
                OracleDataAdapter da = new OracleDataAdapter(query, con);
                DataTable dt = new DataTable();
                da.Fill(dt);

                DDLcargo.DataSource = dt;
                DDLcargo.DataTextField = "cargo_nome";
                DDLcargo.DataValueField = "cargo_id";
                DDLcargo.DataBind();
            }
        }

        protected void BtnIncluir_Click(object sender, EventArgs e)
        {
            using (OracleConnection con = new OracleConnection(strConexao))
            {
                con.Open();

                // Gerar novo ID manualmente
                int novoId;
                using (OracleCommand cmdId = new OracleCommand("SELECT NVL(MAX(pessoa_id),0)+1 FROM pessoa", con))
                {
                    novoId = Convert.ToInt32(cmdId.ExecuteScalar());
                }

                // Inserir pessoa com o novo ID
                string query = "INSERT INTO pessoa (pessoa_id, pessoa_nome, cargo_id) VALUES (:id, :nome, :cargo_id)";
                using (OracleCommand cmd = new OracleCommand(query, con))
                {
                    cmd.Parameters.Add(":id", OracleDbType.Int32).Value = novoId;
                    cmd.Parameters.Add(":nome", OracleDbType.Varchar2).Value = TxtNome.Text.Trim();
                    cmd.Parameters.Add(":cargo_id", OracleDbType.Int32).Value = Convert.ToInt32(DDLcargo.SelectedValue);

                    cmd.ExecuteNonQuery();
                }
            }

            CarregarPessoas();
        }

        protected void GridViewPessoa_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridViewPessoa.EditIndex = e.NewEditIndex;
            CarregarPessoas();

            // Preencher o DropDownList do cargo na edição
            DropDownList ddlCargoEdit = (DropDownList)GridViewPessoa.Rows[e.NewEditIndex].FindControl("DDLcargoEdit");
            if (ddlCargoEdit != null)
            {
                using (OracleConnection con = new OracleConnection(strConexao))
                {
                    string query = "SELECT cargo_id, cargo_nome FROM cargo";
                    OracleDataAdapter da = new OracleDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ddlCargoEdit.DataSource = dt;
                    ddlCargoEdit.DataTextField = "cargo_nome";
                    ddlCargoEdit.DataValueField = "cargo_id";
                    ddlCargoEdit.DataBind();
                }

                // Selecionar o cargo atual
                int cargoIdAtual = Convert.ToInt32(GridViewPessoa.DataKeys[e.NewEditIndex]["cargo_id"]);
                ddlCargoEdit.SelectedValue = cargoIdAtual.ToString();
            }
        }

        protected void GridViewPessoa_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridViewPessoa.EditIndex = -1;
            CarregarPessoas();
        }

        protected void GridViewPessoa_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int pessoa_id = Convert.ToInt32(GridViewPessoa.DataKeys[e.RowIndex].Value);

            TextBox txtNomeEdit = (TextBox)GridViewPessoa.Rows[e.RowIndex].FindControl("txtNomeEdit");
            string nome = txtNomeEdit.Text;

            DropDownList ddlCargoEdit = (DropDownList)GridViewPessoa.Rows[e.RowIndex].FindControl("DDLcargoEdit");
            int cargoId = Convert.ToInt32(ddlCargoEdit.SelectedValue);

            using (OracleConnection con = new OracleConnection(strConexao))
            {
                string query = "UPDATE pessoa SET pessoa_nome = :nome, cargo_id = :cargo_id WHERE pessoa_id = :pessoa_id";
                OracleCommand cmd = new OracleCommand(query, con);
                cmd.Parameters.Add(":nome", OracleDbType.Varchar2).Value = nome;
                cmd.Parameters.Add(":cargo_id", OracleDbType.Int32).Value = cargoId;
                cmd.Parameters.Add(":pessoa_id", OracleDbType.Int32).Value = pessoa_id;
                con.Open();
                cmd.ExecuteNonQuery();
            }

            GridViewPessoa.EditIndex = -1;
            CarregarPessoas();
        }

        protected void GridViewPessoa_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int pessoa_id = Convert.ToInt32(GridViewPessoa.DataKeys[e.RowIndex].Value);

            using (OracleConnection con = new OracleConnection(strConexao))
            {
                string query = "DELETE FROM pessoa WHERE pessoa_id = :pessoa_id";
                OracleCommand cmd = new OracleCommand(query, con);
                cmd.Parameters.Add(":pessoa_id", OracleDbType.Int32).Value = pessoa_id;
                con.Open();
                cmd.ExecuteNonQuery();
            }

            CarregarPessoas();
        }

        protected void GridViewPessoa_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewPessoa.PageIndex = e.NewPageIndex;
            CarregarPessoas();
        }
    }
}
