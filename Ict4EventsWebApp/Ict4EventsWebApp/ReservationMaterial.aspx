<%@ Page Language="C#" MaintainScrollPositionOnPostback="true" AutoEventWireup="true" CodeBehind="ReservationMaterial.aspx.cs" Inherits="Ict4EventsWebApp.ReservationMaterial" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="styles/reset.css" rel="stylesheet" />
    <link href="styles/style.css" rel="stylesheet" />
    <link href="styles/ReservationMaterial.css" rel="stylesheet" />
    <script src="scripts/jquery-1.11.2.min.js"></script>
    <script src="scripts/ui.js"></script>
    <script>
        $(document).ready(function () {
            $("#showImage").click(function () {
                $("#fullscreen").css("visibility", "visible");
            });
            $("#fullscreen").click(function (e) {
                $(this).css("visibility", "hidden");
                var offset = $(".displayed").offset();
                var x = e.pageX - offset.left;
                var y = e.pageY - offset.top;

                $("#XValue").attr("value", parseInt(x));
                $("#YValue").attr("value", parseInt(y));
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
            <asp:Panel ID="pnlRegistration" runat="server">
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
                        <asp:TextBox ID="tbFirstName" CssClass="textBoxMargin" runat="server" OnTextChanged="tbFirstName_TextChanged">Jim</asp:TextBox>
                        <asp:TextBox ID="tbInfix" CssClass="textBoxMargin" runat="server"></asp:TextBox>
                        <asp:TextBox ID="tbSurname" CssClass="textBoxMargin" runat="server">Sanders</asp:TextBox>
                        <asp:TextBox ID="tbEmail" CssClass="textBoxMargin" runat="server">jimsanders11@gmail.com</asp:TextBox>
                        <div class="streetHouse">
                            <asp:TextBox ID="tbStreet" runat="server">Barbarastraat</asp:TextBox>
                            <asp:TextBox ID="tbHouseNr" runat="server">29</asp:TextBox>
                        </div>

                        <div class="postal">
                            <asp:TextBox ID="tbPostalCode" runat="server">6361VK</asp:TextBox>
                            <asp:TextBox ID="tbCity" runat="server">Nuth</asp:TextBox>
                        </div>


                        <asp:TextBox ID="tbIban" CssClass="textBoxMargin" runat="server">NL64RABO0137789440</asp:TextBox>
                        
                    </div>

                    <div class="selectGroupMembers">
                        <asp:Label ID="lblGroupMembers" runat="server" Text="Andere deelnemers"></asp:Label>
                        <asp:ListBox ID="lbGroupMembers"  runat="server" ></asp:ListBox>

                        <asp:RequiredFieldValidator ID="rfvFirstName" CssClass="validator" ControlToValidate="tbFirstName"  ForeColor="Red" runat="server" ErrorMessage="Vul een voornaam in."></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfcLastName" CssClass="validator" ControlToValidate="tbSurname"  ForeColor="Red" runat="server" ErrorMessage="Vul een achternaam in."></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfcEmail" CssClass="validator" ControlToValidate="tbEmail"  ForeColor="Red" runat="server" ErrorMessage="Vul een email adres in."></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfcPostalCode" CssClass="validator" ControlToValidate="tbPostalCode"  ForeColor="Red" runat="server" ErrorMessage="Vul een postcode in."></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfcCity" CssClass="validator" ControlToValidate="tbCity"  ForeColor="Red" runat="server" ErrorMessage="Vul een woonplaats in."></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfcIban" CssClass="validator" ControlToValidate="tbIban"  ForeColor="Red" runat="server" ErrorMessage="Vul een iban nummer in."></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfcStreet" CssClass="validator" ControlToValidate="tbStreet"  ForeColor="Red" runat="server" ErrorMessage="Vul een straatnaam in."></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ID="rfcHouseNr" CssClass="validator" ControlToValidate="tbHouseNr"  ForeColor="Red" runat="server" ErrorMessage="Vul een huisnummer in."></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="regexEmail" CssClass="validator" runat="server" ErrorMessage="Email adres ongeldig." ForeColor="Red" ControlToValidate="tbEmail" ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"></asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="regexIban" CssClass="validator" runat="server" ErrorMessage="Ongeldig NL Iban." ForeColor="Red" ControlToValidate="tbIban" ValidationExpression="^NL([0-9]{2})([A-Z]{4})([0-9]{10})$"></asp:RegularExpressionValidator>
                        <asp:RegularExpressionValidator ID="regexPostalCode" CssClass="validator" runat="server" ErrorMessage="Postcode ongeldig." ForeColor="Red" ControlToValidate="tbPostalCode" ValidationExpression="^([0-9]{4})([A-z]{2})$"></asp:RegularExpressionValidator>
                    </div>

                    <div class="groupMembersInfo">
                        <asp:Label ID="lblFirstNameGM" CssClass="labelMargin" runat="server" Text="Voornaam"></asp:Label>
                        <asp:TextBox ID="TextBox2" CssClass="textBoxMargin" runat="server">Je</asp:TextBox>

                        <asp:Label ID="lblInfixGM" CssClass="labelMargin" runat="server" Text="Tussenvoegsel"></asp:Label>
                        <asp:TextBox ID="TextBox3" CssClass="textBoxMargin" runat="server"></asp:TextBox>

                        <asp:Label ID="lblSurnameGM" CssClass="labelMargin" runat="server" Text="Achternaam"></asp:Label>
                        <asp:TextBox ID="TextBox4" CssClass="textBoxMargin" runat="server">Moeder</asp:TextBox>

                        <asp:Label ID="lblEmailGM" CssClass="labelMargin" runat="server" Text="Email"></asp:Label>
                        <asp:TextBox ID="TextBox5" CssClass="textBoxMargin" runat="server">jeweet@zelluf.nl</asp:TextBox>

                        <asp:Button ID="btnAdd" CssClass="buttonMargin" runat="server" Text="Toevoegen" OnClick="btnAdd_Click" />
                        <asp:Button ID="btnRemove" CssClass="buttonMargin" runat="server" Text="Verwijderen" OnClick="btnRemove_Click" />

                        <asp:Button ID="btnNextStep" runat="server" Text="Verder" OnClick="btnNextStep_Click" />
                    </div>
                    <div class="clearDiv"></div>
                </div>
            </asp:Panel>


            <asp:Panel ID="pnlMap" runat="server">
                <div class="frm">
                    <div class="top">
                        <asp:Label runat="server" Font-Bold="True">Plek</asp:Label>
                        <img id="showImage" alt="Camping" src="images/Camping.png" />
                        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Verder" />
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
                            <asp:Label ID="lbReserveringDetails" class="ScanInfo" runat="server" Text="Prijs :"></asp:Label>
                            <asp:Label ID="lbPrice" runat="server" Text="0"></asp:Label>

                        </div>

                    </div>
                    <div class="middle">
                        <div>
                            <div class="btmiddle">
                                <asp:Button ID="btMaterialAdd" runat="server" Text="<<Toevoegen" OnClick="btMaterialAdd_Click" />
                            </div>
                            <div class="btmiddle">
                                <asp:Button ID="btMaterialDelete" runat="server" Text="Verwijderen>>" OnClick="btMaterialDelete_Click" />
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
                            <asp:Label ID="Details" runat="server">Totale bedrag: &#8364;0,-<br/></asp:Label>
                        </div>
                    </div>

                    <div>
                        <asp:Button ID="btCMaterialTerug" runat="server" Text="Terug" />
                        <asp:Button ID="btCMaterialVerder" runat="server" Text="Voltooien" OnClick="btCMaterialVerder_Click" />
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

