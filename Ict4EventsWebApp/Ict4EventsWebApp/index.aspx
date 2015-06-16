<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Ict4EventsWebApp.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Social Media Sharing | Ict4Events</title>
    <link rel="stylesheet" href="styles/reset.css" />
    <link rel="stylesheet" href="styles/style.css" />
    <link rel="stylesheet" href="styles/SMSStyle.css">
    <script type="text/javascript" src="scripts/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="scripts/ui.js"></script>
    <script type="text/javascript" src="scripts/index.js"></script>
    <script>
        var username = "<%=Session["username"]%>";
        var token = "<%=Session["token"]%>";
        
        $(document).ready(function() {
            loadPosts(<%=string.IsNullOrEmpty(Request.QueryString["id"])?"1":Request.QueryString["id"]%>);
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
        <div id="categories">
            
        </div>
        <div id="posts">
           
        </div>
        <div class="clear"></div>
    </div>
</body>
</html>
