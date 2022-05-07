<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="schedulereport.aspx.cs" Inherits="schedulereport" Title="Schedule Report" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="4" cellspacing="4" width="100%">
                <tr>
                    <td align="center"><<h1>
                        <asp:Label ID="lblTitle" runat="server" Text="Schedule Report"></asp:Label></<h1>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table cellpadding="2" cellspacing="2" width="40%">
                            <tr>
                                <td align="right">
                                    <asp:DropDownList ID="ddlSearchBy" runat="server" AutoPostBack="True" 
                                        OnSelectedIndexChanged="ddlSearchBy_SelectedIndexChanged" TabIndex="1">
                                        <asp:ListItem Value="2" Selected="True">Last Name</asp:ListItem>
                                        <asp:ListItem Value="1">First Name</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtSearch" runat="server" TabIndex="2"></asp:TextBox>
                                    <cc1:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" runat="server" DelimiterCharacters=""
                                        Enabled="True" TargetControlID="txtSearch" ServiceMethod="GetLastName" MinimumPrefixLength="1"
                                        CompletionSetCount="10" EnableCaching="true" CompletionInterval="500"
                                        CompletionListCssClass="AutoExtender" UseContextKey="True">
                                    </cc1:AutoCompleteExtender>
                                    <cc1:TextBoxWatermarkExtender ID="wtmFileNumber" runat="server" TargetControlID="txtSearch"
                                        WatermarkText="Search by Last Name" />
                                </td>
                                <td>                                    
                                    <asp:CheckBox ID="chkAll" runat="server" Text="View All" TabIndex="3" AutoPostBack="True" OnCheckedChanged="chkAll_CheckedChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right"><b>Sales Person:</b></td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlSalesRep" Visible="true" TabIndex="3" runat="server" >
                                    </asp:DropDownList>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="right"><b>Start Date:</b></td>
                                <td align="left" valign="middle">
                                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="textBox" TabIndex="4" AutoPostBack="True" OnTextChanged="txtStartDate_TextChanged"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Format="MM/dd/yyyy" PopupButtonID="imgStartDate" PopupPosition="BottomLeft" TargetControlID="txtStartDate">
                                    </cc1:CalendarExtender>
                                    <asp:ImageButton ID="imgStartDate" CssClass="nostyleCalImg" TabIndex="4" runat="server" ImageUrl="~/Images/calendar.gif" />
                                </td>

                                <td></td>
                            </tr>
                            <tr>
                                <asp:Panel ID="tblEndDate" runat="server">
                                    <td align="right"><b>End Date:</b></td>
                                    <td align="left" valign="middle">
                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="textBox" TabIndex="5" AutoPostBack="True" OnTextChanged="txtEndDate_TextChanged"></asp:TextBox>
                                        <cc1:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Format="MM/dd/yyyy" PopupButtonID="imgEndDate" PopupPosition="BottomLeft" TargetControlID="txtEndDate">
                                        </cc1:CalendarExtender>
                                        <asp:ImageButton ID="imgEndDate" CssClass="nostyleCalImg" TabIndex="5"  runat="server" ImageUrl="~/Images/calendar.gif" />
                                    </td>
                                    <td></td>
                                </asp:Panel>
                            </tr>
                            <tr>
                                <td align="center" colspan="3">
                                    <asp:Label ID="lblResult" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td align="left">
                                    <asp:Button ID="btnReport" runat="server" Text="View Report" TabIndex="6" Width="100px" CssClass="button" OnClick="btnReport_Click" /></td>
                                <td></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

