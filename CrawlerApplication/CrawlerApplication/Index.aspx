<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="CrawlerApplication.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Donald Trump</title>
    <meta name="description" content="Latest news and Tweets of Donald Trump" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <link rel="stylesheet" type="text/css" href="css/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="css/style.css" />
    <script src="scripts/jquery-1.12.0.min.js"></script>
    <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="scripts/tweetList.js"></script>
</head>
<body>
    <div class="row">

        <div class="col-xs-12 heading">
            Donald Trump
        </div>

    </div>
    <div style="clear: both"></div>
    <ul class="nav nav-tabs">
        <li class="active"><a data-toggle="tab" href="#menu_cnn" style="width: 101%;" onfocus="showmydiv_menu1()">CNN News</a></li>
        <li><a data-toggle="tab" href="#menu_twitter" style="width: 101%;" onfocus="showmydiv_menu2()">Twitter</a></li>

    </ul>

    <div id="menu_cnn" class="tab-pane fade in active contact_details">
        <form id="form_CNN" runat="server">
            <asp:Panel ID="Panel_CNN" runat="server"></asp:Panel>
        </form>
    </div>
    <div id="menu_twitter" class="tab-pane fade contact_details">
        <div id="Profile"></div>
    </div>


</body>
</html>
