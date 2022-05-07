<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AutoComplete.aspx.cs" Inherits="AutoComplete" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style>
        .working {
            background: url('../images/ajax-loader-2.gif') no-repeat right center;
        }
    </style>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js" type="text/javascript"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.9.2/jquery-ui.min.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            AutoComplete("#txtExAutoComlete");
        });
        function AutoComplete(control) {
            $(control).autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        url: "AutoComplete.aspx/GetCustomer",
                        data: "{'keyword':'" + $("#txtExAutoComlete").val() + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            var result = data.d;
                            response($.map(data.d, function (item) {
                                return {
                                    label: item.first_name1,
                                    desc: item.customer_id,
                                    value: item.first_name1
                                }
                            }));
                        },
                        error: function () {
                            // alert("there is some error");
                            console.log("there is some error");
                        }
                    });
                },
                minLength: 1,
                select: function (event, ui) {
                     alert(ui.item.desc);
                },
                change: function (event, ui) {
                    alert(ui.item.desc);
                },
                messages: {
                    noResults: "",
                    results: function () { }
                },
                search: function () { $(this).addClass('progress'); },
                open: function () { $(this).removeClass('progress'); },
                response: function () { $(this).removeClass('progress'); }
            });
        }
        //function AutoComplete(control) {
        //    $(control).autocomplete({
        //        source: function (request, response) {
        //            $.ajax({
        //                type: "POST",
        //                url: "AutoComplete.aspx/GetObjectDTO",
        //                data: "{}",
        //                contentType: "application/json; charset=utf-8",
        //                dataType: "json",
        //                success: function (data) {
        //                    var result = data.d;
        //                    response($.map(data.d, function (item) {
        //                        return {
        //                            label: item,
        //                            value: item
        //                        }
        //                    }));
        //                }
        //            });
        //        },
        //        minLength: 1,
        //        select: function (event, ui) {
        //        },
        //        change: function (event, ui) {
        //        },
        //        search: function () { $(this).addClass('progress'); },
        //        open: function () { $(this).removeClass('progress'); },
        //        response: function () { $(this).removeClass('progress'); }
        //    });
        //}
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtExAutoComlete" runat="server" ClientIDMode="Static" Style="text-transform: capitalize;" TabIndex="1" ></asp:TextBox>
        </div>
    </form>
</body>
</html>
