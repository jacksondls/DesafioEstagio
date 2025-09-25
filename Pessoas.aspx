<%@ Page Title="Cadastro/Editar Pessoas" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="Pessoas.aspx.cs" Inherits="DesafioEstagio.Pessoas" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Cadastro/Edição de Pessoas</h2>

    <!-- Formulário para incluir nova pessoa -->
    <asp:TextBox ID="TxtNome" runat="server" Placeholder="Nome"></asp:TextBox>
    <asp:DropDownList ID="DDLcargo" runat="server"></asp:DropDownList>
    <asp:Button ID="BtnIncluir" runat="server" Text="Incluir Pessoa" OnClick="BtnIncluir_Click" />

    <br /><br />

    <!-- GridView para listar, editar, excluir e paginar -->
    <asp:GridView ID="GridViewPessoa" runat="server" AutoGenerateColumns="false" DataKeyNames="pessoa_id"
        OnRowEditing="GridViewPessoa_RowEditing"
        OnRowUpdating="GridViewPessoa_RowUpdating"
        OnRowCancelingEdit="GridViewPessoa_RowCancelingEdit"
        OnRowDeleting="GridViewPessoa_RowDeleting"
        AllowPaging="True" PageSize="50" OnPageIndexChanging="GridViewPessoa_PageIndexChanging">

        <Columns>
            <asp:BoundField DataField="pessoa_id" HeaderText="ID" ReadOnly="true" />

            <asp:TemplateField HeaderText="Nome">
                <ItemTemplate>
                    <%# Eval("pessoa_nome") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtNomeEdit" runat="server" Text='<%# Bind("pessoa_nome") %>' />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Cargo">
                <ItemTemplate>
                    <%# Eval("cargo_nome") %>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="DDLcargoEdit" runat="server"></asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
        </Columns>
    </asp:GridView>
</asp:Content>
