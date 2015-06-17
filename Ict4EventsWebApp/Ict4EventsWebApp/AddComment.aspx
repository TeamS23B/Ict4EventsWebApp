<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddComment.aspx.cs" Inherits="Ict4EventsWebApp.AddComment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add Comment | ICT4Events</title>
    <link rel="stylesheet" href="styles/reset.css" />
    <link rel="stylesheet" href="styles/style.css" />
    <link href="styles/addComment.css" rel="stylesheet" />
    <script type="text/javascript" src="scripts/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="scripts/ui.js"></script>
    <script>
        $(document).ready(function() {
            
        });
    </script>
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
        <div class="leftBarButton" id="btAddPost">Add A Post</div>
        <div id="leftBarButtons">
        </div>
    </div>
    <div id="content">
        <div id="postSpacerTop"></div>
        <form id="frmAddComment" runat="server">
            <div>
                <asp:Label Text="" runat="server" ID="lblCatName"/>
                <asp:Label Text="" runat="server" ID="lblError"/>
                <br />
                <asp:RadioButton runat="server" ID="rbText" Text="Tekst" GroupName="rbInput"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="rbFile" Text="Bestand" GroupName="rbInput"></asp:RadioButton>
                <asp:RadioButton runat="server" ID="rbCat" Text="Categorie" GroupName="rbInput"></asp:RadioButton>
                <br />
                <asp:FileUpload ID="uploadFl" runat="server" />
                <br />
                <asp:Label Text="Titel" runat="server" />
                <asp:TextBox ID="tbTitle" runat="server"></asp:TextBox>
                <br />
                <asp:Label Text="Tekst" runat="server" />
                <asp:TextBox ID="tbText" runat="server" TextMode="MultiLine"></asp:TextBox>
                <br />
                <asp:Button Text="Plaatsen" runat="server" ID="send" OnClick="send_Click"/>
            </div>
        </form>
    </div>
</body>
</html>
