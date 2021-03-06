﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Check.aspx.cs" Inherits="Ict4EventsWebApp.Check1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="styles/reset.css" rel="stylesheet" />
    <link href="styles/style.css" rel="stylesheet" />
    <link href="styles/CheckBetaling.css" rel="stylesheet" />
    <script src="scripts/jquery-1.11.2.min.js"></script>
    <script src="scripts/ui.js"></script>
    <title>Toegangscontrole | ICT4Events</title>
</head>
<body>
    <div id="topBar">
        <div id="menuButton">
            <img src="images/Menu.png" alt="Menu" />
        </div>
    </div>
    <div id="leftBar">
        <input type="text" id="search" placeholder="Search..." /><input type="button" id="searchButton" value=" " />
        <div class="leftBarButton" onclick="location.href='index.aspx';">Social Media</div>
        <div class="leftBarButton" onclick="location.href='Check.aspx';">Toegangs Controle</div>
        <div class="leftBarButton" onclick="location.href='MateriaalVerhuur.aspx';">Materiaal Verhuur</div>
        <div class="leftBarButton" onclick="location.href='Admin.aspx';">Administratie</div>
    </div>
    <div id="content">
        <form id="form1" runat="server">
            <asp:TextBox ID="tbBarcode" runat="server"></asp:TextBox>
            <asp:Button ID="btnCheck" runat="server" Text="Controleer" OnClick="btnCheck_Click" />
            <asp:Label ID="lblBarcodeText" runat="server" Text="Barcode:" CssClass="BarcodeText" Font-Size="X-Large"></asp:Label>
            <asp:Label ID="lblBarcodeObject" runat="server" Text="Nog niet gescand" Font-Bold="True" Font-Size="XX-Large" Width="300px" CssClass="BarcodeObject"></asp:Label>
            <asp:Label ID="lblBetaalstatusText" runat="server" Text="Betaalstatus" CssClass="BetaalstatusText" Font-Size="X-Large"></asp:Label>
            <asp:Label ID="lblBetaaldobject" runat="server" Text="Nog niet gescand" Font-Bold="True" ForeColor="Lime" CssClass="BetaalstatusObject" Font-Size="50px"></asp:Label>
            <asp:Label ID="lblAanwezigObject" runat="server" Text="Nog niet gescand"></asp:Label>
            <div id="clearDiv"></div>
        </form>
    </div>
</body>
</html>
