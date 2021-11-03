﻿<%@ Page Title="" Language="C#" MasterPageFile="~/frmPrincipal.Master" AutoEventWireup="true" CodeBehind="Formulario web2.aspx.cs" Inherits="webElectroHogar.Formulario_web22" %>
<asp:Content ID="Content4" ContentPlaceHolderID="Cuerpo" runat="server">
    <table class="auto-style2">
        <tbody class="auto-style7">
            <tr>
                <td class="auto-style20">Usuarios</td>
            </tr>
            <tr>
                <td class="auto-style21">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style21">
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style19">NIT:</td>
                            <td class="auto-style18"><strong>
                                <asp:TextBox ID="TextBox1" runat="server" CssClass="auto-style17" Width="244px"></asp:TextBox>
                                </strong>
                            </td>
                        </tr>
                    </table>
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style19">Nombre:</td>
                            <td class="auto-style18"><strong>
                                <asp:TextBox ID="TextBox4" runat="server" CssClass="auto-style17" Width="244px"></asp:TextBox>
                                </strong>
                            </td>
                        </tr>
                    </table>
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style19">Correo:</td>
                            <td class="auto-style18"><strong>
                                <asp:TextBox ID="TextBox5" runat="server" CssClass="auto-style17" Width="244px"></asp:TextBox>
                                </strong>
                            </td>
                        </tr>
                    </table>
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style19">Celular:</td>
                            <td class="auto-style18"><strong>
                                <asp:TextBox ID="TextBox6" runat="server" CssClass="auto-style17" Width="244px"></asp:TextBox>
                                </strong>
                            </td>
                        </tr>
                    </table>
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style19">Perfil:</td>
                            <td class="auto-style18"><strong>
                                <asp:TextBox ID="TextBox7" runat="server" CssClass="auto-style17" Width="244px"></asp:TextBox>
                                </strong>
                            </td>
                        </tr>
                    </table>
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style19">Usuario:</td>
                            <td class="auto-style18"><strong>
                                <asp:TextBox ID="TextBox8" runat="server" CssClass="auto-style17" Width="244px"></asp:TextBox>
                                </strong>
                            </td>
                        </tr>
                    </table>
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style19">Contraseña:</td>
                            <td class="auto-style18"><strong>
                                <asp:TextBox ID="TextBox9" runat="server" CssClass="auto-style17" Width="244px"></asp:TextBox>
                                </strong>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="auto-style21">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style20">
                <asp:Menu ID="mnuOpciones" runat="server" BorderStyle="Solid" BorderWidth="2px" DynamicHorizontalOffset="2" Font-Bold="True" Font-Names="Century Gothic" Font-Size="Small" Orientation="Horizontal" RenderingMode="Table" Width="100%">
                    <Items>
                        <asp:MenuItem Text="Buscar" Value="opcBuscar"></asp:MenuItem>
                        <asp:MenuItem Text="Agregar" Value="opcAgregar"></asp:MenuItem>
                        <asp:MenuItem Text="Modificar" Value="opcModificar"></asp:MenuItem>
                        <asp:MenuItem Text="Grabar" Value="opcGrabar"></asp:MenuItem>
                        <asp:MenuItem Text="Cancelar" Value="opcCancelar"></asp:MenuItem>
                    </Items>
                    <StaticHoverStyle BorderStyle="Solid" BorderWidth="2px" ForeColor="Black" />
                    <StaticMenuItemStyle HorizontalPadding="20px" />
                </asp:Menu>
                </td>
            </tr>
            <tr>
                <td class="auto-style24">
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style21">
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

        </style>
</asp:Content>

