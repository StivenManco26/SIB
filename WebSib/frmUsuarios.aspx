<%@ Page Title="" Language="C#" MasterPageFile="~/frmPrincipal.Master" AutoEventWireup="true" CodeBehind="frmUsuarios.aspx.cs" Inherits="webSib.Formulario_web22" %>
<asp:Content ID="Content4" ContentPlaceHolderID="Cuerpo" runat="server">
    <table class="auto-style2">
        <tbody class="auto-style7">
            <tr>
                <td class="auto-style25" colspan="2">
                <asp:Label ID="lblUsu" runat="server" CssClass="auto-style13"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style20" colspan="2">Usuarios</td>
            </tr>
            <tr>
                <td class="auto-style21" colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style21" colspan="2">
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style19">NIT:</td>
                            <td class="auto-style18"><strong>
                                <asp:TextBox ID="txtNit" runat="server" CssClass="auto-style17" Width="335px"></asp:TextBox>
                                </strong>
                            </td>
                        </tr>
                    </table>
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style19">Nombre:</td>
                            <td class="auto-style18"><strong>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="auto-style17" Width="335px"></asp:TextBox>
                                </strong>
                            </td>
                        </tr>
                    </table>
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style19">Correo:</td>
                            <td class="auto-style18"><strong>
                                <asp:TextBox ID="txtCorreo" runat="server" CssClass="auto-style17" Width="335px"></asp:TextBox>
                                </strong>
                            </td>
                        </tr>
                    </table>
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style19">Celular:</td>
                            <td class="auto-style18"><strong>
                                <asp:TextBox ID="txtCelular" runat="server" CssClass="auto-style17" Width="335px"></asp:TextBox>
                                </strong>
                            </td>
                        </tr>
                    </table>
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style19">Perfil:</td>
                            <td class="auto-style18">
                                <asp:DropDownList ID="ddlPerfil" runat="server" Height="19px" Width="335px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style19">Usuario:</td>
                            <td class="auto-style18"><strong>
                                <asp:TextBox ID="txtUsuario" runat="server" CssClass="auto-style17" Width="335px"></asp:TextBox>
                                </strong>
                            </td>
                        </tr>
                    </table>
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style19">Contraseña:</td>
                            <td class="auto-style18"><strong>
                                <asp:TextBox ID="txtContrasena" runat="server" CssClass="auto-style17" Width="335px"></asp:TextBox>
                                </strong>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="auto-style21" colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style27">
                    <asp:Button ID="btnGuardar" runat="server" Height="24px" OnClick="btnGuardar_Click" Text="Guardar" Visible="False" Width="111px" />
                </td>
                <td class="auto-style26">
                    <asp:Button ID="btnCancelar" runat="server" Height="24px" OnClick="btnCancelar_Click" Text="Cancelar" Visible="False" Width="111px" />
                </td>
            </tr>
            <tr>
                <td class="auto-style21" colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style20" colspan="2">
                <asp:Menu ID="mnuOpciones" runat="server" BorderStyle="Solid" BorderWidth="2px" DynamicHorizontalOffset="2" Font-Bold="True" Font-Names="Century Gothic" Font-Size="Small" Orientation="Horizontal" RenderingMode="Table" Width="100%" OnMenuItemClick="mnuOpciones_MenuItemClick1">
                    <Items>
                        <asp:MenuItem Text="Buscar" Value="opcBuscar"></asp:MenuItem>
                        <asp:MenuItem Text="Agregar" Value="opcAgregar"></asp:MenuItem>
                        <asp:MenuItem Text="Modificar" Value="opcModificar"></asp:MenuItem>
                        <asp:MenuItem Text="Limpiar" Value="opcLimpiar"></asp:MenuItem>
                    </Items>
                    <StaticHoverStyle BorderStyle="Solid" BorderWidth="2px" ForeColor="Black" />
                    <StaticMenuItemStyle HorizontalPadding="20px" />
                </asp:Menu>
                </td>
            </tr>
            <tr>
                <td class="auto-style24" colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style21" colspan="2">
                <asp:Label ID="lblMsj" runat="server" CssClass="auto-style13"></asp:Label>
                </td>
            </tr>
        </tbody>
    </table>
</asp:Content>
<asp:Content ID="Content5" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style7 {
            font-size: medium;
        }
        .auto-style8 {
            width: 756px;
        }
        .auto-style17 {
            font-weight: bold;
            font-size: medium;
        }
        .auto-style18 {
            width: 377px;
            text-align: left;
        }
        .auto-style19 {
            width: 377px;
            text-align: right;
        }

        .auto-style13 {
            color: #FF0000;
            font-size: small;
        }

        .auto-style20 {
            height: 27px;
            width: 771px;
        }

        .auto-style21 {
            width: 771px;
        }
        .auto-style24 {
            width: 771px;
            height: 76px;
        }

        .auto-style25 {
            height: 27px;
            width: 771px;
            text-align: right;
        }

        .auto-style26 {
            width: 771px;
            height: 28px;
            text-align: left;
        }
        .auto-style27 {
            width: 579px;
            height: 28px;
            text-align: right;
        }

        </style>
</asp:Content>

