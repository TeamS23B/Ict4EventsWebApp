<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="Ict4EventsWebApp.Admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="styles/reset.css" rel="stylesheet" />
    <link href="styles/style.css" rel="stylesheet" />
    <link href="styles/Admin.css" rel="stylesheet" />
    <script src="scripts/jquery-1.11.2.min.js"></script>
    <script src="scripts/ui.js"></script>
    <title>Beheerder | ICT4Events</title>
</head>
<body>
    <div id="topBar">
        <div id="menuButton">
            <img src="images/Menu.png" alt="Menu" />
        </div>
    </div>
    <div id="leftBar">
        <input type="text" id="search" placeholder="Search..." /><input type="button" id="searchButton" value=" " />
        <div class="leftBarButton" id="btAddPost">Add A Post</div>
    </div>
    <div id="content">
        <form id="form1" runat="server">
            <h1>Event aanmaken</h1>
            <div class="leftSettings">
                <asp:Label ID="lblNaam" runat="server" Text="Naam"></asp:Label>
                <asp:TextBox ID="tbName" runat="server"></asp:TextBox>
                <asp:Label ID="lblLocatie" runat="server" Text="Locatie:"></asp:Label>
                <asp:DropDownList ID="ddlLocation" runat="server"></asp:DropDownList>
                <asp:Label ID="lblMaxVis" runat="server" Text="Maximaal aantal bezoekers"></asp:Label>
                <asp:TextBox ID="tbMaxVis" runat="server" type="number" min="0"></asp:TextBox>
                <asp:Button ID="btnAddEvent" runat="server" Text="Maak event" />
            </div>
            <div class="rightSettings">
                <asp:Label ID="lblStartDate" runat="server" Text="Startdatum"></asp:Label>
                <asp:Calendar ID="clStartDate" runat="server"></asp:Calendar>
                <asp:Label ID="lblEndDate" runat="server" Text="Einddatum"></asp:Label>
                <asp:Calendar ID="clEndDate" runat="server"></asp:Calendar>
            </div>
            <div class="Users">
                <asp:Label ID="lblUsers" runat="server" Text="Gebruikers"></asp:Label>
                <asp:ListBox ID="lbUsers" runat="server"></asp:ListBox>
            </div>
            <div class="debug"></div>
        </form>
    </div>
</body>
</html>
