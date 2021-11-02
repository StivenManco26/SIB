<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmSplash.aspx.cs" Inherits="webSib.frmSplash" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <style type="text/css">
        .auto-style1 {
            text-align: center;
        }
        .auto-style2 {
            width: 98%;
        }
        .auto-style3 {
            width: 960px;
            height: 560px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="auto-style1">
            
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <table align="center" class="auto-style2">
                <tr>
                    <td>
                        <img alt="" class="auto-style3" src="imagenes/splash.jpg" /><asp:Timer ID="tmrSplash" runat="server" OnTick="tmrSplash_Tick" Interval="2000">
                        </asp:Timer>
                    </td>
                </tr>
            </table>
            
        </div>
    </form>
</body>
</html>
