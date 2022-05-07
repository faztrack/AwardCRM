<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test2.aspx.cs" Inherits="test2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.3/themes/smoothness/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="http://code.jquery.com/ui/1.10.3/jquery-ui.js"></script>
    <script>
        $(function () {
            //Bind an event listener to the dialogbeforeclose event
            $("#dialog").on("dialogbeforeclose", function (event, ui) {
                $("#HiddenField1").val("false");
            });
            //judge if show dialog
            if ($("#HiddenField1").val() == "true") {
                $("#dialog").dialog({ draggable: false });
            }

        });

        function ShowDialog() {
            $("#dialog").dialog({ draggable: false });
            $("#HiddenField1").val("true");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="HiddenField1" runat="server" Value="false" />
        <div id="dialog" title="Basic dialog" style="display: none;">
            <asp:Button ID="Button1" runat="server" Text="I can post back" UseSubmitBehavior="false" OnClick="Button1_Click" />
        </div>
          <asp:Button ID="Button3" runat="server" Text="Show" UseSubmitBehavior="false"  OnClientClick="ShowDialog()" />
        <input id="Button2" type="button" value="Show dialog" onclick="ShowDialog()" />

    </form>
</body>
</html>
