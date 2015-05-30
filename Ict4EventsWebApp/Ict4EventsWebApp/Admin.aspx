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
            <div class="makeEvent">
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
            </div>
            <div class="mngUsers">
                <h1>Beheer gebruikers</h1>
                <div class="Users">
                    <asp:Label ID="lblUsers" runat="server" Text="Gebruikers"></asp:Label>
                    <asp:ListBox ID="lbUsers" runat="server"></asp:ListBox>
                </div>
                <div class="Messages">
                    <asp:Label ID="lblMsgUsers" runat="server" Text="Berichten van gebruiker:"></asp:Label>
                    <asp:ListBox ID="lbMessages" runat="server"></asp:ListBox>
                    <asp:Label ID="lblGroupMng" runat="server" Text="De groepsleider is:"></asp:Label>
                    <asp:Label ID="lblPaid" runat="server" Text="De groepsleider heeft betaald"></asp:Label>
                </div>
                <div class="rentedMat">
                    <asp:CheckBox ID="cbBlock" runat="server" />
                    <asp:Label ID="lblBlock" runat="server" Text="Gebruiker blokkeren?" AssociatedControlID="cbBlock"></asp:Label>
                    <asp:Label ID="lblRentedMat" runat="server" Text="Gehuurde materialen"></asp:Label>
                    <asp:ListBox ID="lbRentedMat" runat="server"></asp:ListBox>
                </div>
            </div>
            <div class="matMng">
                <div class="matList">
                    <asp:Label ID="lblMats" runat="server" Text="Materialen"></asp:Label>
                    <asp:ListBox ID="lbMaterials" runat="server"></asp:ListBox>
                    <asp:Button ID="btnAddCopy" runat="server" Text="Voeg een exemplaar toe" />
                    <asp:Button ID="btnRmvCopy" runat="server" Text="Haal een exemplaar weg" />
                </div>
                <div class="addMat">
                    <asp:Label ID="lblBrand" runat="server" Text="Merk"></asp:Label>
                    <asp:TextBox ID="tbBrand" runat="server"></asp:TextBox>
                    <asp:Label ID="lblSeries" runat="server" Text="Serie"></asp:Label>
                    <asp:TextBox ID="tbSeries" runat="server"></asp:TextBox>
                    <asp:Label ID="lblTypeNr" runat="server" Text="Typenummer"></asp:Label>
                    <asp:TextBox ID="tbTypeNr" runat="server"></asp:TextBox>
                    <asp:Label ID="lblPrice" runat="server" Text="Prijs"></asp:Label>
                    <asp:TextBox ID="tbPrice" runat="server"></asp:TextBox>
                    <asp:Label ID="lblCat" runat="server" Text="Categorie"></asp:Label>
                    <asp:DropDownList ID="ddlCat" runat="server"></asp:DropDownList>
                    <asp:Button ID="btnUpdate" runat="server" Text="Product bijwerken" />
                    <asp:Button ID="btnDelete" runat="server" Text="Item verwijderen" />
                </div>
            </div>
            <div class="debug"></div>
        </form>
    </div>
</body>
</html>
