<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Check.aspx.cs" Inherits="Ict4EventsWebApp.Check" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="styles/reset.css" rel="stylesheet" />
    <link href="styles/style.css" rel="stylesheet" />
    <link href="styles/CheckBetaling.css" rel="stylesheet" />
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
            <asp:Label ID="lblBarcodeText" runat="server" Text="Barcode:" CssClass="BarcodeText" Font-Size="X-Large"></asp:Label>
            <asp:Label ID="lblBarcodeObject" runat="server" Text="Test 1233443" Font-Bold="True" Font-Size="XX-Large" Width="300px" CssClass="BarcodeObject"></asp:Label>
            <asp:Label ID="lblBetaalstatusText" runat="server" Text="Betaalstatus" CssClass="BetaalstatusText" Font-Size="X-Large"></asp:Label>
            <asp:Label ID="lblBetaaldobject" runat="server" Text="Betaald" Font-Bold="True" ForeColor="Lime" CssClass="BetaalstatusObject" Font-Size="50px"></asp:Label>
        <div id="clearDiv"></div>
        </form>
    </div>
</body>
</html>
