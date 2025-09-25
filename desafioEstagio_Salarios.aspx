<%@ Page Title="Salários" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="desafioEstagio_Salarios.aspx.cs" 
    Inherits="DesafioEstagio.Salarios" Async="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Cadastro de Pessoas e Salários</h2>

    <!-- Formulário para incluir pessoa -->
    <asp:TextBox ID="TxtNome" runat="server" Placeholder="Nome"></asp:TextBox>
    <asp:DropDownList ID="DDLcargo" runat="server"></asp:DropDownList>
    <asp:Button ID="BtnIncluir" runat="server" Text="Incluir Pessoa" OnClick="BtnIncluir_Click" />

    <br /><br />

    <!-- Botão para recalcular salários -->
    <asp:Button ID="BtnCalcular" runat="server" Text="Calcular/Atualizar Salários" OnClick="BtnCalcular_Click" />

    <br /><br />

    <!-- Grid para exibir salários dentro do UpdatePanel -->
    <asp:UpdatePanel runat="server" ID="UpdatePanelSalarios">
        <ContentTemplate>
            <asp:GridView ID="GridViewSalarios" runat="server" AutoGenerateColumns="false" Width="600px"
                AllowPaging="True" PageSize="50"
                OnPageIndexChanging="GridViewSalarios_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="pessoa_id" HeaderText="ID" ReadOnly="True" />
                    <asp:BoundField DataField="pessoa_nome" HeaderText="Nome" />
                    <asp:BoundField DataField="cargo_nome" HeaderText="Cargo" />
                    <asp:BoundField DataField="salario" HeaderText="Salário" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>

    <br />

    <asp:Label ID="lblMensagem" runat="server" ForeColor="Red"></asp:Label>

</asp:Content>
