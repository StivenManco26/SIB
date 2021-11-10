<%@ Page Title="" Language="C#" MasterPageFile="~/frmPrincipal.Master" AutoEventWireup="true" CodeBehind="frmLogin.aspx.cs" Inherits="webSib.Formulario_web11" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Menu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Cuerpo" runat="server">
    <table align="center" class="auto-style7">
        <tr>
            <td class="auto-style8"><strong>Ingreso SIB</strong></td>
        </tr>
        <tr>
            <td class="auto-style8">&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style8">
                <table class="auto-style2">
                    <tr>
                        <td style="width: 40%"><strong>Usuario:</strong></td>
                        <td>
                            <asp:TextBox ID="txtUsuario" runat="server" Width="80%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 40%"><strong>Contraseña:</strong></td>
                        <td>
                            <asp:TextBox ID="txtClave" runat="server" Width="80%" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="auto-style8">&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style8">
                <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" OnClick="btnIngresar_Click" />
            </td>
        </tr>
        <tr>
            <td class="auto-style8">&nbsp;</td>
        </tr>
        <tr>
            <td class="auto-style8">
                <asp:Label ID="lblMsj" runat="server" Font-Bold="True" Font-Names="Century Gothic" Font-Size="Small" ForeColor="#CC0000" Width="90%"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="auto-style8">&nbsp;</td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content5" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style7 {
            width: 98%;
        }
        .auto-style8 {
            font-size: medium;
        }
    </style>
</asp:Content>

