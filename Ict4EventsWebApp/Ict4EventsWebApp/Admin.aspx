<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="Ict4EventsWebApp.Admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="styles/reset.css" rel="stylesheet" />
    <link href="styles/style.css" rel="stylesheet" />
    <link href="styles/Admin.css" rel="stylesheet" />
    <script src="scripts/jquery-1.11.2.min.js"></script>
    <script src="scripts/ui.js"></script>
    <title>Beheersysteem | ICT4Events</title>
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
            <div id="makeEvent" class="subFrm">
                <h1>Event aanmaken</h1>
                <div class="leftSettings">
                    <asp:Label ID="lblNaam" runat="server" Text="Naam:"></asp:Label>
                    <asp:TextBox ID="tbName" runat="server"></asp:TextBox>

                    <asp:Label ID="lblLocatie" runat="server" Text="Locatie:"></asp:Label>
                    <asp:DropDownList ID="ddlLocation" runat="server"></asp:DropDownList>
                    <asp:Label ID="lblMaxVis" runat="server" Text="Maximaal aantal bezoekers"></asp:Label>
                    <asp:TextBox ID="tbMaxVis" runat="server" type="number" min="0"></asp:TextBox>

                    <asp:Button ID="btnAddEvent" runat="server" Text="Maak event" OnClick="btnAddEvent_Click" />
                    <div class="validatorDisplay">
                        <asp:RequiredFieldValidator ID="FVEventname" runat="server"
                            ControlToValidate="tbName"
                            ErrorMessage="Naam is verplicht"
                            ForeColor="Green" CssClass="validator"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="FVMaxVisitors" runat="server"
                            ControlToValidate="tbMaxVis"
                            ErrorMessage="Er moet een maximum aantal worden ingevoerd"
                            ForeColor="Green" CssClass="validator"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="rightSettings">
                    <asp:Label ID="lblStartDate" runat="server" Text="Startdatum"></asp:Label>
                    <asp:Calendar ID="clStartDate" runat="server"></asp:Calendar>
                    <asp:Label ID="lblEndDate" runat="server" Text="Einddatum"></asp:Label>
                    <asp:Calendar ID="clEndDate" runat="server"></asp:Calendar>
                </div>
                <div class="debug"></div>
            </div>
            <div id="mngUsers" class="subFrm">
                <h1>Beheer gebruikers</h1>
                <div class="Users">
                    <asp:Label ID="lblUsers" runat="server" Text="Gebruikers"></asp:Label>
                    <asp:ListBox ID="lbUsers" runat="server" AutoPostBack="True" OnSelectedIndexChanged="lbUsers_SelectedIndexChanged"></asp:ListBox>
                </div>
                <div class="Messages">
                    <asp:Label ID="lblMsgUsers" runat="server" Text="Berichten van gebruiker:"></asp:Label>
                    <asp:ListBox ID="lbMessages" runat="server" AutoPostBack="True"></asp:ListBox>
                </div>
                <div id="rentedMat">
                    <asp:Label ID="lblRentedMat" runat="server" Text="Gehuurde materialen"></asp:Label>
                    <asp:ListBox ID="lbRentedMat" runat="server"></asp:ListBox>
                    <asp:CheckBox ID="cbBlock" runat="server" Enabled="False" OnCheckedChanged="cbBlock_CheckedChanged" AutoPostBack="True" />
                    <asp:Label ID="lblBlock" runat="server" Text="Gebruiker blokkeren?" AssociatedControlID="cbBlock"></asp:Label>
                </div>
                <div class="debug"></div>
            </div>
            <div id="matMng" class="subFrm">
                <div class="matList">
                    <asp:Label ID="lblMats" runat="server" Text="Materialen"></asp:Label>
                    <asp:ListBox ID="lbMaterials" runat="server" AutoPostBack="true" OnSelectedIndexChanged="lbMaterials_SelectedIndexChanged"></asp:ListBox>
                    <asp:Button ID="btnAddCopy" runat="server" Text="Voeg een exemplaar toe" OnClick="btnAddCopy_Click" AutoPostBack="true" Enabled="False" />
                    <asp:Button ID="btnRmvCopy" runat="server" Text="Haal een exemplaar weg" AutoPostBack="true" Enabled="False" OnClick="btnRmvCopy_Click" />
                </div>
                <div class="addMat">
                    <asp:Label ID="lblBrand" runat="server" Text="Merk"></asp:Label>
                    <asp:TextBox ID="tbBrand" runat="server" Enabled="False"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator ID="FVBrand" runat="server"
                        ControlToValidate="tbBrand"
                        ErrorMessage="Merk is verplicht"
                        ForeColor="Green"></asp:RequiredFieldValidator>--%>
                    <asp:Label ID="lblSeries" runat="server" Text="Serie"></asp:Label>
                    <asp:TextBox ID="tbSeries" runat="server" Enabled="False"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator ID="FVSerie" runat="server"
                        ControlToValidate="tbSeries"
                        ErrorMessage="Serie is verplicht"
                        ForeColor="Green"></asp:RequiredFieldValidator>--%>
                    <asp:Label ID="lblTypeNr" runat="server" Text="Typenummer"></asp:Label>
                    <asp:TextBox ID="tbTypeNr" runat="server" Enabled="False"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator ID="FVTypeNumber" runat="server"
                        ControlToValidate="tbTypeNr"
                        ErrorMessage="Typenummer is verplicht"
                        ForeColor="Green"></asp:RequiredFieldValidator>--%>
                    <asp:Label ID="lblPrice" runat="server" Text="Prijs"></asp:Label>
                    <asp:TextBox ID="tbPrice" runat="server" Enabled="False"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator ID="FVPrice" runat="server"
                        ControlToValidate="tbPrice"
                        ErrorMessage="prijs is verplicht"
                        ForeColor="Green"></asp:RequiredFieldValidator>--%>
                    <asp:Label ID="lblCat" runat="server" Text="Categorie"></asp:Label>
                    <asp:DropDownList ID="ddlCat" runat="server" Enabled="False"></asp:DropDownList>
                    <asp:Button ID="btnUpdate" runat="server" Text="Product bijwerken" Enabled="False" OnClick="btnUpdate_Click" />
                    <asp:Button ID="btnNew" runat="server" Text="Product aanmaken" Enabled="false" OnClick="btnNew_Click" />
                </div>
                <div class="debug"></div>
            </div>

        </form>
    </div>
</body>
</html>
