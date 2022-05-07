<%@ Page Language="C#" AutoEventWireup="true" CodeFile="COReport.aspx.cs" Inherits="COReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change Order Report</title>
    <link type="text/css" href="css/layout.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table width="100%">
                <tr>
                    <td>
                        <table width="100%" align="center">
                            <tr>
                                <td align="center" valign="top" width="85%">
                                    <h2> <asp:Label ID="lblTitle" runat="server" Text="Change Order Report"></asp:Label></h2>
                                </td>
                                <td align="right" valign="top">
                                    <asp:ImageButton ID="btnExpList" ImageUrl="~/images/button_csv.png" runat="server" CssClass="cssCSV" OnClick="btnExpList_Click" ToolTip="Export List to CSV" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:GridView ID="grdChangeOrders" runat="server"
                            AutoGenerateColumns="False" Width="100%" PageSize="20" CssClass="mGrid">
                            <PagerSettings Position="TopAndBottom" />
                            <Columns>
                                <asp:BoundField DataField="C/O Date" HeaderText="C/O Date">
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Last Name" HeaderText="Last Name">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="First Name" HeaderText="First Name">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Estimate" HeaderText="Estimate">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="C/O Name" HeaderText="C/O Name">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Type" HeaderText="Type">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderText="Status">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Execution Date" HeaderText="Execution Date">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Amount" HeaderText="Amount">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>

                            </Columns>
                            <PagerStyle CssClass="pgr" HorizontalAlign="Left" />
                            <AlternatingRowStyle CssClass="alt" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
