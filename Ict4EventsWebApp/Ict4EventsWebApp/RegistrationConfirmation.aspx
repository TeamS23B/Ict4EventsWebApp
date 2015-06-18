<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistrationConfirmation.aspx.cs" Inherits="Ict4EventsWebApp.RegistrationConfirmation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="styles/reset.css" rel="stylesheet" />
    <link href="styles/Confirmation.css" rel="stylesheet" />
    <link href="styles/style.css" rel="stylesheet" />
    <script src="scripts/jquery-1.11.2.min.js"></script>
    <script src="scripts/ui.js"></script>
    <title>Login | Ict4Events</title>
</head>
<body>
    <div id="topBar">
    </div>

    <div id="content">
        <form id="form1" runat="server">

            <asp:Label ID="lblGratz" runat="server" CssClass="label" Text="Gefeliciteerd, je hebt je (groep) succesvol aangemeld voor het evenement!"></asp:Label>
            <asp:Label ID="lblEmail" runat="server" CssClass="label" Text="Er is een email met activatielink verstuurd naar elk opgegeven emailadres."></asp:Label>
            <asp:Label ID="lblRequired" runat="server" CssClass="label" Text="Zorg ervoor dat je account geactiveerd is voor het evenement begint."></asp:Label>
            <asp:Label ID="lblEnjoy" runat="server" CssClass="label" Text="Veel plezier op het evenement!"></asp:Label>

            <div id="clearDiv"></div>
        </form>
    </div>
</body>
</html>
