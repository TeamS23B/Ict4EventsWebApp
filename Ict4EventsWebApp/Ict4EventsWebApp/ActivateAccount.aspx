<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivateAccount.aspx.cs" Inherits="Ict4EventsWebApp.ActivateAccount" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Activate Account | Ict4Events</title>
    <link rel="stylesheet" href="styles/reset.css" />
    <link rel="stylesheet" href="styles/style.css" />
    <link href="styles/activate.css" rel="stylesheet" />
    <script type="text/javascript" src="scripts/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="scripts/ui.js"></script>
</head>
<body>
    <div id="topBar">
        <div id="menuButton">
            <img src="images/Menu.png" alt="Menu">
        </div>
        <div id="categorieTrace"></div>
    </div>
    <div id="leftBar">
        <input type="text" id="search" placeholder="Search..." /><input type="button" id="searchButton" value=" " />
        <div class="leftBarButton" onclick="location.href='index.aspx';">Login</div>
        <div id="leftBarButtons">
        </div>
    </div>
    <div id="content">
        <form id="frmActivate" runat="server">
            <div>
                <asp:Label Text="" runat="server" ID="lblError"/>
                <asp:Label Text="Wachtwoord" runat="server" />
                <asp:TextBox runat="server" ID="newPassword" TextMode="Password" />
                <br />
                <asp:Label Text="Nieuw Wachtwoord" runat="server" />
                <asp:TextBox runat="server" ID="confirmPassword" TextMode="Password" />
                <br />
                <asp:Button ID="setPassword" runat="server" Text="Instellen" OnClick="setPassword_Click" />
            </div>
        </form>
    </div>
</body>
</html>
