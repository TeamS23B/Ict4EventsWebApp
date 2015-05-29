<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistrationCustomerInfo.aspx.cs" Inherits="Ict4EventsWebApp.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="styles/reset.css" rel="stylesheet" />
    <link href="styles/style.css" rel="stylesheet" />
    <link href="styles/RegistrationCustomerInfo.css" rel="stylesheet" />
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
        <div class="labelleft">
            <asp:Label ID="lblFirstName" CssClass="labelMargin" runat="server" Text="Voornaam:"></asp:Label>
            <asp:Label ID="lblInfix" CssClass="labelMargin" runat="server" Text="Tussenvoegsel:"></asp:Label>
            <asp:Label ID="lblSurname" CssClass="labelMargin" runat="server" Text="Achternaam:"></asp:Label>
            <asp:Label ID="lblEmail" CssClass="labelMargin" runat="server" Text="Email:"></asp:Label>
            <asp:Label ID="lblStreetHouseNr" CssClass="labelMargin" runat="server" Text="Straat + huisnummer:"></asp:Label>
            <asp:Label ID="lblPostalCodeCity" CssClass="labelMargin" runat="server" Text="Postcode + woonplaats:"></asp:Label>
            <asp:Label ID="lblIban" CssClass="labelMargin" runat="server" Text="Iban:"></asp:Label>
            
        </div>
        
        <div class="textboxleft">
            <asp:TextBox ID="tbFirstName" CssClass="textBoxMargin" runat="server"></asp:TextBox>
            <asp:TextBox ID="tbInfix" CssClass="textBoxMargin" runat="server"></asp:TextBox>
            <asp:TextBox ID="tbSurname" CssClass="textBoxMargin" runat="server"></asp:TextBox>
            <asp:TextBox ID="tbEmail" CssClass="textBoxMargin" runat="server"></asp:TextBox>
            <div class="streetHouse">
                <asp:TextBox ID="tbStreet"  runat="server"></asp:TextBox>
                <asp:TextBox ID="tbHouseNr" runat="server"></asp:TextBox>
            </div>
            
            <div class="postal">
                <asp:TextBox ID="tbPostalCode" runat="server"></asp:TextBox>
                <asp:TextBox ID="tbCity" runat="server"></asp:TextBox>
            </div>
            

            <asp:TextBox ID="tbIban" CssClass="textBoxMargin" runat="server"></asp:TextBox>
        </div>
        <div class="selectGroupMembers">
            <asp:Label ID="lblGroupMembers" runat="server" Text="Andere deelnemers"></asp:Label>
            <asp:ListBox ID="lbGroupMembers" runat="server"></asp:ListBox>
        </div>
        <div class="groupMembersInfo">
            <asp:Label ID="lblFirstNameGM" CssClass="labelMargin" runat="server" Text="Voornaam"></asp:Label>
            <asp:TextBox ID="TextBox1" CssClass="textBoxMargin" runat="server"></asp:TextBox>

            <asp:Label ID="lblInfixGM" CssClass="labelMargin" runat="server" Text="Tussenvoegsel"></asp:Label>
            <asp:TextBox ID="TextBox2" CssClass="textBoxMargin" runat="server"></asp:TextBox>

            <asp:Label ID="lblSurnameGM" CssClass="labelMargin" runat="server" Text="Achternaam"></asp:Label>
            <asp:TextBox ID="TextBox3" CssClass="textBoxMargin" runat="server"></asp:TextBox>

            <asp:Label ID="lblEmailGM" CssClass="labelMargin" runat="server" Text="Email"></asp:Label>
            <asp:TextBox ID="TextBox4" CssClass="textBoxMargin" runat="server"></asp:TextBox>

            <asp:Button ID="btnAdd" CssClass="buttonMargin" runat="server" Text="Toevoegen" />
            <asp:Button ID="btnRemove" CssClass="buttonMargin" runat="server" Text="Verwijderen" />

            <asp:Button ID="btnNextStep" runat="server" Text="Volgende stap" />
        </div>
        
        <div id="clearDiv">
        </div>
        

        
        
        
        </form>
    </div>
</body>
</html>

