<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MateriaalVerhuur.aspx.cs" Inherits="Ict4EventsWebApp.MateriaalVerhuur" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="styles/reset.css" rel="stylesheet" />
    <link href="styles/style.css" rel="stylesheet" />
    <link href="styles/MateriaalVerhuur.css" rel="stylesheet" />
    <script src="scripts/jquery-1.11.2.min.js"></script>
    <script src="scripts/ui.js"></script>
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
        <div class="left">
            <asp:Label runat="server">Producten op voorraad</asp:Label>
            <asp:ListBox ID="lbProducten" runat="server"></asp:ListBox>
            <asp:Label ID="Details" runat="server">Detail:<br/>Huurkosten: &#8364;0,-<br/>Status: onbekend</asp:Label>
        </div>
        <div class="right">
            <div class="ScanInfo">
                <asp:Label runat="server">Gescande Barcode: </asp:Label>
                <asp:Label ID="lblBar" runat="server" Text="Barcode: "></asp:Label>
                <asp:Label ID="lblNaam" runat="server" Text="Naam: "></asp:Label>
            </div>
            <div class="EndDate">
                <asp:Label runat="server" Text="Einddatum kiezen"></asp:Label>
                
                <asp:Calendar ID="clEndDate" runat="server"></asp:Calendar>
            </div>
            <asp:Button ID="btSubmit" runat="server" Text="Submit" OnClick="btSubmit_Click" />
        </div>
        <div id="clearDiv"></div>
        </form>
    </div>
</body>
</html>
