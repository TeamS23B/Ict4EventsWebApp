<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReservationMaterial.aspx.cs" Inherits="Ict4EventsWebApp.ReservationMaterial" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="styles/reset.css" rel="stylesheet" />
    <link href="styles/style.css" rel="stylesheet" />
    <link href="styles/ReservationMaterial.css" rel="stylesheet" />
    <script src="scripts/jquery-1.11.2.min.js"></script>
    <script src="scripts/ui.js"></script>
    <script>
        $(document).ready(function() {
            $("#showImage").click(function() {
                $("#fullscreen").css("visibility","visible");
            });
            $("#fullscreen").click(function() {
                $(this).css("visibility", "hidden");
                //todo find out xy location on image
            });
        });
    </script>
    <title></title>
</head>
<body>
    <div id="topBar">
        <div id="menuButton">
            <img src="images/Menu.png" alt="Menu" />
        </div>
    </div>
    <div id=" "></div>
    <div id="leftBar">
        <input type="text" id="search" placeholder="Search..." /><input type="button" id="searchButton" value=" " />
        <div class="leftBarButton" id="btAddPost">Add A Post</div>
    </div>
    <div id="content">
        <form id="form1" runat="server">
            <div class="frm">
            <div class="top">
                <asp:Label runat="server" Font-Bold="True">Reservering materiaal</asp:Label>
            </div>
            <div class="left">
                <div class="textmiddle">
                    <asp:Label runat="server">Wat je wilt reserveren</asp:Label>
                </div>
                <asp:ListBox ID="lbMaterialToReserve" runat="server"></asp:ListBox>
                <div class="ScanInfo">
                    <asp:Label ID="Details" runat="server">Totale bedrag: &#8364;0,-<br/></asp:Label>
                </div>

            </div>
            <div class="middle">
                <div>
                    <div class="btmiddle">
                        <asp:Button ID="btMaterialAdd" runat="server" Text="<<Toevoegen" />
                    </div>
                    <div class="btmiddle">
                        <asp:Button ID="btMaterialDelete" runat="server" Text="Verwijderen>>" />
                    </div>
                </div>
            </div>
            <div class="right">
                <div class="textmiddle">
                    <asp:Label runat="server">Beschikbaar materiaal</asp:Label>
                </div>
                <asp:ListBox ID="lbavailableMaterial" runat="server"></asp:ListBox>

            </div>
            <div id="clearDiv"></div>

            <div>
                <asp:Button ID="btRMaterialTerug" runat="server" Text="Terug" />
                <asp:Button ID="btRMAterialVerder" runat="server" Text="Verder" />
            </div>
            </div>
            <div class="frm">
            <div class="top">
                <asp:Label runat="server" Font-Bold="True">Totaaloverzicht</asp:Label>
            </div>
            <div id="Confirmation">
                <div class="textmiddle margenperfect">Gereserveerde plaasten</div>
                <div>
                    <asp:ListBox ID="ConfirmLocation" runat="server"></asp:ListBox>
                </div>
                <div class="textmiddle margenperfect">Gereserveerde locaties</div>
                <div>
                    <asp:ListBox ID="ConfrimMaterial" runat="server"></asp:ListBox>
                </div>
                <div class="margenperfect">
                    <asp:Label ID="ReserveringDetails" class="ScanInfo" runat="server">Totale bedrag: &#8364;0,-<br/></asp:Label>
                </div>
            </div>

            <div>
                <asp:Button ID="btCMaterialTerug" runat="server" Text="Terug" />
                <asp:Button ID="btCMaterialVerder" runat="server" Text="Verder" />
            </div>
            </div>
            <div class="frm">
                <div class="top">
                <asp:Label runat="server" Font-Bold="True">Plek</asp:Label>
                    <img id="showImage" alt="Camping" src="images/Camping.png" width="200px"/>
                </div>
            </div>
        </form>
        <div id="fullscreen">
           <img class="displayed" src="images/Camping.png" alt="Camping" />
        </div>
    </div>
</body>
</html>

