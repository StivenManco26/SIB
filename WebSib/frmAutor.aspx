<%@ Page Title="" Language="C#" MasterPageFile="~/frmPrincipal.Master" AutoEventWireup="true" CodeBehind="frmAutor.aspx.cs" Inherits="webSib.frmAutor" %>
<asp:Content ID="Content4" ContentPlaceHolderID="Cuerpo" runat="server">
    <table class="auto-style2">
        <tbody class="auto-style7">
            <tr>
                <td class="auto-style25" colspan="2">
                <asp:Label ID="lblUsu" runat="server" CssClass="auto-style13"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style20" colspan="2">Autor</td>
            </tr>
            <tr>
                <td class="auto-style21" colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style21" colspan="2">
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style19">Código:</td>
                            <td class="auto-style18"><strong>
                                <asp:TextBox ID="txtCodigo" runat="server" CssClass="auto-style17" Width="335px"></asp:TextBox>
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
                <td class="auto-style20" colspan="2">
                <asp:Label ID="lblMsj" runat="server" CssClass="auto-style13"></asp:Label>
                    </td>
            </tr>
            <tr>
                <td class="auto-style20" colspan="2">
                <asp:Menu ID="mnuOpciones" runat="server" BorderStyle="Solid" BorderWidth="2px" DynamicHorizontalOffset="2" Font-Bold="True" Font-Names="Century Gothic" Font-Size="Small" Orientation="Horizontal" RenderingMode="Table" Width="100%" OnMenuItemClick="mnuOpciones_MenuItemClick1">
                    <Items>
                        <asp:MenuItem Text="Buscar" Value="opcBuscar"></asp:MenuItem>
                        <asp:MenuItem Text="Agregar" Value="opcAgregar"></asp:MenuItem>
                        <asp:MenuItem Text="Modificar" Value="opcModificar" Selectable="False"></asp:MenuItem>
                        <asp:MenuItem Text="Limpiar" Value="opcLimpiar"></asp:MenuItem>
                    </Items>
                    <StaticHoverStyle BorderStyle="Solid" BorderWidth="2px" ForeColor="Black" />
                    <StaticMenuItemStyle HorizontalPadding="20px" />
                </asp:Menu>
                </td>
            </tr>
            <tr>
                <td class="auto-style24" colspan="2">
                    <asp:GridView ID="grvDatosAutor" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="98%">
                        <AlternatingRowStyle BackColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td class="auto-style21" colspan="2">
                    &nbsp;</td>
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
            width: 98%;
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

