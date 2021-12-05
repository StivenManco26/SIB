<%@ Page Title="" Language="C#" MasterPageFile="~/frmPrincipal.Master" AutoEventWireup="true" CodeBehind="frmPrestamos.aspx.cs" Inherits="webSib.Formulario_web23" %>
<asp:Content ID="Content4" ContentPlaceHolderID="Cuerpo" runat="server">
    <table class="auto-style2">
        <tbody class="auto-style7">
            <tr>
                <td class="auto-style25">
                    <asp:Label ID="lblFechaRegistro" runat="server" Visible="False" ViewStateMode="Enabled"></asp:Label>
                <asp:Label ID="lblUsu" runat="server" CssClass="auto-style13"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style20">Prestamo</td>
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
                                <asp:TextBox ID="txtNit" runat="server" CssClass="auto-style17" Width="206px"></asp:TextBox>
                                <asp:ImageButton ID="btnBuscarNombre" runat="server" ImageUrl="~/imagenes/Buscar.jpg" OnClick="btnBuscarNombre_Click" />
                                </strong>
                                 </td>
                            <td class="auto-style19">Nombre:</td>
                            <td class="auto-style18"><strong>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="auto-style17" Width="206px"></asp:TextBox>           
                                </strong>
                            </td>
                        </tr>

                    </table>
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style19">Codigo del Libro:</td>
                            <td class="auto-style18"><strong>
                                <asp:TextBox ID="txtCodigoLibro" runat="server" CssClass="auto-style17" Width="206px"></asp:TextBox>
                                <asp:ImageButton ID="btnBuscarLibro" runat="server" ImageUrl="~/imagenes/Buscar.jpg" OnClick="btnBuscarLibro_Click" />
                                </strong>
                            </td>
                            <td class="auto-style19">Nombre Libro: </td>
                            <td class="auto-style18">
                                <asp:Label ID="lblNombreLibro" runat="server" BorderStyle="Outset" Width="224px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table class="auto-style8">
                        <tr>                  
                            <td class="auto-style19">Estado del Libro:</td>
                            <td class="auto-style18">
                                <asp:DropDownList ID="ddlMaterialEstado" runat="server" Height="19px" Width="144px">
                                </asp:DropDownList>
                            </td>
                            <td class="auto-style19">Id Reserva:</td>
                            <td class="auto-style18"><strong>
                                <asp:TextBox ID="txtIdReserva" runat="server" CssClass="auto-style17" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style19">Fecha Prestamo</td>
                            <td class="auto-style18">
                                <asp:Label ID="lblHoraPres" runat="server" BorderStyle="Inset" BorderWidth="2px" Width="125px"></asp:Label>
                            </td>
                            <td class="auto-style19">Fecha Devolución</td>
                            <td class="auto-style18">
                                <asp:Label ID="lblFechaDevo" runat="server" BorderStyle="Inset" BorderWidth="2px" Width="125px"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style19">Max Dias:
                                <asp:Label ID="lblDiasPrestamo" runat="server"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            </td>
                            <td class="auto-style18">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Cant Max:
                                <asp:Label ID="lblMaxCantPrestamo" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style19">
                                <asp:Button ID="btnBuscarprestamo" runat="server" OnClick="btnBuscarprestamo_Click" Text="Buscar Prestamo" Width="114px" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                &nbsp;&nbsp;
                            </td>
                            
                            <td class="auto-style18">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Button1" runat="server" Text="Buscar Reserva" Width="105px" OnClick="Button1_Click" />
                                </td>
                        </tr>
                    </table>
                    <table class="auto-style8">
                        <tr>
                            <td class="auto-style20">                             
                                <asp:Button ID="btnIngresarPrestamo" runat="server" Text="Ingresar Prestamo" OnClick="btnIngresarPrestamo_Click" />
                                &nbsp
                                 <asp:Button ID="btnIngresarPrestamoReserva" runat="server" Text="Ingresar Prestamo con reserva" OnClick="btnIngresarPrestamoReserva_Click" />
                            
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
                <asp:Menu ID="mnuOpciones" runat="server" BorderStyle="Solid" BorderWidth="2px" DynamicHorizontalOffset="2" Font-Bold="True" Font-Names="Century Gothic" Font-Size="Small" Orientation="Horizontal" RenderingMode="Table" Width="100%" OnMenuItemClick="mnuOpciones_MenuItemClick1">
                    <Items>
                        <asp:MenuItem Text="Agregar" Value="opcAgregar"></asp:MenuItem>
                        <asp:MenuItem Text="Limpiar Grid" Value="opcLimpiarGrid"></asp:MenuItem>
                    </Items>
                    <StaticHoverStyle BorderStyle="Solid" BorderWidth="2px" ForeColor="Black" />
                    <StaticMenuItemStyle HorizontalPadding="20px" />
                </asp:Menu>
                </td>
            </tr>
            <tr>
                <td class="auto-style24">
                    <asp:GridView ID="grvDatosPrestamo" runat="server" Width="98%" DataField="fechaPrestamo" DataFormatString="{0:d}" HeaderText="fechaPrestamo" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="id" />
                            <asp:BoundField DataField="codigoMat" HeaderText="codigo material" />
                            <asp:BoundField DataField="estado" HeaderText="id estado material" />
                            <asp:BoundField DataField="nit" HeaderText="nit" />
                            <asp:BoundField DataField="idReserva" HeaderText="id reserva" />
                            <asp:BoundField DataField="fechaPrestamo" DataFormatString="{0:d}" HeaderText="fecha prestamo" />
                            <asp:BoundField DataField="fechaDevolucion" DataFormatString="{0:d}" HeaderText="fecha devolucion" />
                            <asp:BoundField DataField="fechaRegistro" DataFormatString="{0:d}" HeaderText="fecha registro" />
                        </Columns>
                    </asp:GridView>


                    <asp:GridView ID="grvReserva" runat="server" Width="95%" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField DataField="Reserva" HeaderText="Reserva" />
                            <asp:BoundField DataField="Material" HeaderText="Material" />
                            <asp:BoundField DataField="Fecha reserva" DataFormatString="{0:d}" HeaderText="Fecha reserva" />
                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                        </Columns>
                    </asp:GridView>
                </td>

            </tr>
            <tr>
                <td class="auto-style26">
                                <asp:ScriptManager ID="ScriptManager1" runat="server">
                                </asp:ScriptManager>
                                        <asp:Timer ID="tmrHoraActual" runat="server" OnTick="tmrHoraActual_Tick">
                                        </asp:Timer>
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
            height: 77px;
        }
        .auto-style25 {
            height: 27px;
            width: 771px;
            text-align: right;
        }
        .auto-style26 {
            width: 771px;
            height: 20px;
        }
        </style>
</asp:Content>
