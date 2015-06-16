<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Ict4EventsWebApp.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="styles/reset.css" rel="stylesheet" />
    <link href="styles/Login.css" rel="stylesheet" />
    <link href="styles/style.css" rel="stylesheet" />
    <script src="scripts/jquery-1.11.2.min.js"></script>
    <script src="scripts/ui.js"></script>
    <script src="scripts/Login.js"></script>
    <title></title>
</head>
<body>
    <div id="topBar">
		<div id="menuButton">
		    <img src="images/Menu.png" alt="Menu"/>
		</div>
	</div>
	<div id="leftBar">
		<input type="text" id="search" placeholder="Search..."/><input type="button" id="searchButton" value=" "/>
		<div class="leftBarButton" id="btAddPost">Add A Post</div>
	</div>
    <div id="content">
        <form id="form1" runat="server">
            <asp:Label runat="server" ID="lblError"></asp:Label><br/>
            <asp:Label ID="lblUsername" runat="server" Text="Gebruikersnaam" CssClass="lblUsername" Font-Size="X-Large"></asp:Label>
            <asp:TextBox ID="tbUsername" runat="server" CssClass ="tbUsername" Height="20px" Width="200px" placeholder="Vul gebruikersnaam in"></asp:TextBox>
            <asp:Label ID="lblPassword" runat="server" Text="Wachtwoord" CssClass="lblPassword" Font-Size="X-Large"></asp:Label>
            <asp:TextBox ID="tbPassword" runat="server" CssClass ="tbPassword" Height="20px" Width="200px" placeholder="Vul wachtwoord in"></asp:TextBox>
            <div id="ButtonMiddle">
            <asp:Button ID="BtnLogin" runat="server" Text="Login" CssClass="BtnLog" OnClick="BtnLogin_Click"/>
            </div>
        <div id="clearDiv"></div>
        </form>
    </div>
</body>
</html>
