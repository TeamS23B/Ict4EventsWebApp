<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReservationMaterial.aspx.cs" Inherits="Ict4EventsWebApp.ReservationMaterial" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="styles/reset.css" rel="stylesheet" />
    <link href="styles/style.css" rel="stylesheet" />
    <link href="styles/ReservationMaterial.css" rel="stylesheet" />
   <%-- <link href="styles/RegistrationCustomerInfo.css" rel="stylesheet" />--%>
    <script src="scripts/jquery-1.11.2.min.js"></script>
    <script src="scripts/ui.js"></script>
    <script>
        $(document).ready(function () {
            $("#showImage").click(function () {
                $("#fullscreen").css("visibility", "visible");
            });
            $("#fullscreen").click(function(e) {
                $(this).css("visibility", "hidden");
                var offset = $(".displayed").offset();
                var x = e.pageX - offset.left;
                var y = e.pageY - offset.top;
                $("#XValue").attr("value", x);
                $("#YValue").attr("value", y);
                alert("x = " + x + "y = " + y);
                
                //todo find out xy location on image
            });
        });
    </script>
    <title></title>
</head>
<body>
    <div id="topBar">
        <div id="menuButton">
            <img src="images/Menu.png" alt="Menu" />
        </div>
    </div>
    <div id=" "></div>
    <div id="leftBar">
        <input type="text" id="search" placeholder="Search..." /><input type="button" id="searchButton" value=" " />
        <div class="leftBarButton" id="btAddPost">Add A Post</div>
    </div>
    <div id="content">
        <form id="form1" runat="server">
            <div class="frm">
        <div class="labelleft">
            <asp:Label ID="lblFirstName" CssClass="labelMargin" runat="server" Text="Voornaam:" ClientIDMode="Static"></asp:Label>
            <asp:Label ID="lblInfix" CssClass="labelMargin" runat="server" Text="Tussenvoegsel:" ClientIDMode="Static"></asp:Label>
            <asp:Label ID="lblSurname" CssClass="labelMargin" runat="server" Text="Achternaam:" ClientIDMode="Static"></asp:Label>
            <asp:Label ID="lblEmail" CssClass="labelMargin" runat="server" Text="Email:" ClientIDMode="Static"></asp:Label>
            <asp:Label ID="lblStreetHouseNr" CssClass="labelMargin" runat="server" Text="Straat + huisnummer:" ClientIDMode="Static"></asp:Label>
            <asp:Label ID="lblPostalCodeCity" CssClass="labelMargin" runat="server" Text="Postcode + woonplaats:" ClientIDMode="Static"></asp:Label>
            <asp:Label ID="lblIban" CssClass="labelMargin" runat="server" Text="Iban:" ClientIDMode="Static"></asp:Label>
            
        </div>
        
        <div class="textboxleft">
            <asp:TextBox ID="tbFirstName" CssClass="textBoxMargin" runat="server"></asp:TextBox>
            <asp:TextBox ID="tbInfix" CssClass="textBoxMargin" runat="server"></asp:TextBox>
            <asp:TextBox ID="tbSurname" CssClass="textBoxMargin" runat="server"></asp:TextBox>
            <asp:TextBox ID="tbEmail" CssClass="textBoxMargin" runat="server"></asp:TextBox>
            <div class="streetHouse">
                        <asp:TextBox ID="tbStreet" runat="server"></asp:TextBox>
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
            <asp:TextBox ID="TextBox2" CssClass="textBoxMargin" runat="server"></asp:TextBox>

            <asp:Label ID="lblInfixGM" CssClass="labelMargin" runat="server" Text="Tussenvoegsel"></asp:Label>
            <asp:TextBox ID="TextBox3" CssClass="textBoxMargin" runat="server"></asp:TextBox>

            <asp:Label ID="lblSurnameGM" CssClass="labelMargin" runat="server" Text="Achternaam"></asp:Label>
            <asp:TextBox ID="TextBox4" CssClass="textBoxMargin" runat="server"></asp:TextBox>

            <asp:Label ID="lblEmailGM" CssClass="labelMargin" runat="server" Text="Email"></asp:Label>
            <asp:TextBox ID="TextBox5" CssClass="textBoxMargin" runat="server"></asp:TextBox>

            <asp:Button ID="btnAdd" CssClass="buttonMargin" runat="server" Text="Toevoegen" />
            <asp:Button ID="btnRemove" CssClass="buttonMargin" runat="server" Text="Verwijderen" />

            <asp:Button ID="btnNextStep" runat="server" Text="Volgende stap" OnClick="btnNextStep_Click" />
        </div>
                <div class="clearDiv"></div>
        </div>
        
            <asp:Panel ID="pnlMap" runat="server">
            <div class="frm">
                <div class="top">
                <asp:Label runat="server" Font-Bold="True">Plek</asp:Label>
                    <img id="showImage" alt="Camping" src="images/Camping.png" />
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
                    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                    <asp:TextBox ID="voorbeeld" runat="server"></asp:TextBox>
                </div>
            </div>
            <asp:HiddenField ID="XValue" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="YValue" runat="server" ClientIDMode="Static" />


            </asp:Panel>

            
            <asp:Panel ID="pnlMaterial" runat="server">
            
            <div class="frm">
            <div class="top">
                <asp:Label runat="server" Font-Bold="True">Reservering materiaal</asp:Label>
            </div>
            <div class="left">
                <div class="textmiddle">
                    <asp:Label runat="server">Wat je wilt reserveren</asp:Label>
                </div>
                <asp:ListBox ID="lbMaterialToReserve" runat="server"></asp:ListBox>
                <div class="ScanInfo">
                    <asp:Label ID="Details" runat="server">Totale bedrag: &#8364;0,-<br/></asp:Label>
                </div>

            </div>
            <div class="middle">
                <div>
                    <div class="btmiddle">
                        <asp:Button ID="btMaterialAdd" runat="server" Text="<<Toevoegen" />
                    </div>
                    <div class="btmiddle">
                        <asp:Button ID="btMaterialDelete" runat="server" Text="Verwijderen>>" />
                    </div>
                </div>
            </div>
            <div class="right">
                <div class="textmiddle">
                    <asp:Label runat="server">Beschikbaar materiaal</asp:Label>
                </div>
                <asp:ListBox ID="lbavailableMaterial" runat="server"></asp:ListBox>

            </div>
            <div class="clearDiv"></div>
            <div>
                <asp:Button ID="btRMaterialTerug" runat="server" Text="Terug" />
                        <asp:Button ID="btRMAterialVerder" runat="server" Text="Verder" OnClick="btRMAterialVerder_Click" />
            </div>
            </div>
            </asp:Panel>
            
            <asp:Panel ID="pnlOverview" runat="server">
            <div class="frm">
            <div class="top">
                <asp:Label runat="server" Font-Bold="True">Totaaloverzicht</asp:Label>
            </div>
            <div id="Confirmation">
                <div class="textmiddle margenperfect">Gereserveerde plaasten</div>
                <div>
                    <asp:ListBox ID="ConfirmLocation" runat="server"></asp:ListBox>
                </div>
                <div class="textmiddle margenperfect">Gereserveerde locaties</div>
                <div>
                    <asp:ListBox ID="ConfrimMaterial" runat="server"></asp:ListBox>
                </div>
                <div class="margenperfect">
                    <asp:Label ID="ReserveringDetails" class="ScanInfo" runat="server">Totale bedrag: &#8364;0,-<br/></asp:Label>
                </div>
            </div>

            <div>
                <asp:Button ID="btCMaterialTerug" runat="server" Text="Terug" />
                <asp:Button ID="btCMaterialVerder" runat="server" Text="Verder" />
            </div>
            </div>
            </asp:Panel>




            
        </form>
        <div id="fullscreen">
           <img class="displayed" src="images/Camping.png" alt="Camping" />
        </div>
    </div>
</body>
</html>

