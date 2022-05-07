<%@ Page Title="Changeorder Report" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="changeorder_report.aspx.cs" Inherits="changeorder_report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="2" width="100%">
                <tr>
                    <td align="center">
                        <h1>Change Order Report</h1>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table cellpadding="0" cellspacing="2" width="100%">
                            <tr>
                                <td align="right" width="45%">
                                    <b>Start Date: </b>
                                </td>
                                <td align="left" valign="middle">
                                    <asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                                    <asp:ImageButton CssClass="nostyleCalImg" ID="imgStartDate" runat="server" ImageUrl="~/images/calendar.gif" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right" width="45%">
                                    <b>End Date: </b>
                                </td>
                                <td align="left" valign="middle">
                                    <asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                                    <asp:ImageButton CssClass="nostyleCalImg" ID="imgEndDate" runat="server" ImageUrl="~/images/calendar.gif" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="height: 11px">
                                    <asp:Button ID="btnViewReport" runat="server" Text="View Report"
                                        CssClass="button" Width="100px" OnClick="btnViewReport_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button"
                                        Width="100px" OnClick="btnCancel_Click" />
                                    <asp:HiddenField ID="hdnOrder" runat="server" Value="ASC" />

                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <cc1:CalendarExtender ID="startdate" runat="server"
                                        Format="MM/dd/yyyy" PopupButtonID="imgStartDate"
                                        PopupPosition="BottomLeft" TargetControlID="txtStartDate">
                                    </cc1:CalendarExtender>
                                    <cc1:CalendarExtender ID="EndDate" runat="server"
                                        Format="MM/dd/yyyy" PopupButtonID="imgEndDate"
                                        PopupPosition="BottomLeft" TargetControlID="txtEndDate">
                                    </cc1:CalendarExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2">
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <table width="100%" align="center" cellpadding="5" cellspacing="5">
                                                    <tr>
                                                        <td align="right" width="80%" valign="middle">

                                                            <b>
                                                                <asp:Label ID="lblStatus" runat="server" Text="Status:"></asp:Label></b>
                                                        </td>
                                                        <td align="left" valign="middle">
                                                            <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                                                <asp:ListItem Value="5">All</asp:ListItem>
                                                                <asp:ListItem Value="1">Draft</asp:ListItem>
                                                                <asp:ListItem Value="2">Pending</asp:ListItem>
                                                                <asp:ListItem Value="3">Executed</asp:ListItem>
                                                                <asp:ListItem Value="4">Declined</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="right" valign="middle">
                                                            <asp:ImageButton ID="btnExpList" ImageUrl="~/images/button_csv.png" runat="server" CssClass="cssCSV" OnClick="btnExpList_Click" ToolTip="Export List to CSV" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:GridView ID="grdChangeOrders" runat="server" AllowSorting="True"
                                                    AutoGenerateColumns="False" Width="100%" PageSize="20" CssClass="mGrid" OnSorting="grdChangeOrders_Sorting" OnRowDataBound="grdChangeOrders_RowDataBound">
                                                    <PagerSettings Position="TopAndBottom" />
                                                    <Columns>
                                                        <asp:BoundField DataField="C/O Date" HeaderText="C/O Date" SortExpression="C/O Date">
                                                            <ItemStyle HorizontalAlign="center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Last Name" HeaderText="Last Name" SortExpression="Last Name">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="First Name" HeaderText="First Name" SortExpression="First Name">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Estimate" HeaderText="Estimate" SortExpression="Estimate">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="C/O Name" HeaderText="C/O Name" SortExpression="C/O Name">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Sales" HeaderText="Sales" SortExpression="Sales">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Execution Date" HeaderText="Execution Date" SortExpression="Execution Date">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="Amount" HeaderText="Amount">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="hypView" runat="server">[hypTitle]</asp:HyperLink>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>

                                                    </Columns>
                                                    <PagerStyle CssClass="pgr" HorizontalAlign="Left" />
                                                    <AlternatingRowStyle CssClass="alt" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>

                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExpList" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="UpdatePanel1"
        DynamicLayout="False">
        <ProgressTemplate>
            <div class="overlay" />
            <div class="overlayContent">
                <p>
                    Please wait while your data is being processed
                </p>
                <img src="images/ajax_loader.gif" alt="Loading" border="1" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>

