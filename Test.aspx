<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <table id="Table5" align="center" width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td align="center" class="cssHeader">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="left"><span class="titleNu">Google Calendar test</span></td>
                        <td align="right"></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <br />
                <br />
                <asp:Button ID="btnSend" runat="server" CssClass="button" Text="Test" OnClick="btnSend_Click" />
                <br />
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table id="Table1" align="center" width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td align="center" class="cssHeader" colspan="3">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="left"><span class="titleNu">Formate and Update Customer Phone/Mobile/Fax Number</span></td>
                        <td align="right"></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <asp:Label ID="lblMessage2" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnUpdateCustomerPhoneNumber" runat="server" CssClass="button" Text="Update Customer Phone Number" OnClick="btnUpdateCustomerPhoneNumber_Click" />

                <asp:Button ID="btnUpdateUserPhoneNumber" runat="server" CssClass="button" Text="Update User Phone Number" OnClick="btnUpdateUserPhoneNumber_Click" />

                <asp:Button ID="btnUpdateSalesPersonPhoneNumber" runat="server" CssClass="button" Text="Update Sales Person Phone Number" OnClick="btnUpdateSalesPersonPhoneNumber_Click" />
            </td>
            <td></td>
        </tr>

    </table>

    <table id="Table2" align="center" width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td align="center" class="cssHeader" colspan="3">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="left"><span class="titleNu">Update Customer Appointment</span></td>
                        <td align="right"></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <asp:Label ID="lblMessage3" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnUpdateCustomerAppointment" runat="server" CssClass="button" Text="Update Customer Appointment" OnClick="btnUpdateCustomerAppointment_Click" />

            </td>
            <td></td>
        </tr>

    </table>
    <br />
    <table width="100%">
        <tr>
            <td align="center" class="cssHeader" colspan="3">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="left"><span class="titleNu">Menu</span></td>
                        <td align="right"></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:GridView ID="grdMenu" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                    TabIndex="2" Width="75%">
                    <Columns>
                        <asp:BoundField DataField="menu_id" HeaderText="MenuId" />
                        <asp:BoundField DataField="menu_code" HeaderText="MenuCode" />
                        <asp:BoundField DataField="menu_name" HeaderText="MenuName" />

                        <asp:BoundField DataField="parent_id" HeaderText="ParentId" />
                        <asp:BoundField DataField="client_id" HeaderText="ClientId" />
                        <asp:BoundField DataField="menu_url" HeaderText="MenuUrl" />

                        <asp:BoundField DataField="isShow" HeaderText="IsShow" />
                        <asp:BoundField DataField="serial" HeaderText="Serial" />
                    </Columns>
                    <AlternatingRowStyle CssClass="alt" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table style="width: 80%;">
                    <tr>
                        <td style=" text-align:center;">Menu ID</td>
                        <td style=" text-align:center;">MenuCode</td>
                        <td style=" text-align:center;">MenuName</td>

                        <td style=" text-align:center;">Parent</td>
                        <td style=" text-align:center;">ClientId</td>
                        <td style=" text-align:center;">MenuUrl</td>

                        <td style=" text-align:center;">IsShow</td>
                        <td style=" text-align:center;">Serial</td>
                        <td style=" text-align:center;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td >&nbsp;</td>
                        <td style=" text-align:center;">
                            <asp:TextBox ID="txtMenuCode" runat="server" Width="100px"></asp:TextBox></td>
                        <td style=" text-align:center;">
                            <asp:TextBox ID="txtMenuName" runat="server" Width="350px"></asp:TextBox></td>


                        <td style=" text-align:center;">
                            <asp:DropDownList ID="ddlParent" runat="server"></asp:DropDownList></td>
                        <td style=" text-align:center;">
                            <asp:Label ID="lblClientId" runat="server" Text="1"></asp:Label></td>
                        <td style=" text-align:center;">
                            <asp:TextBox ID="txtMenuUrl" runat="server" Width="350px"></asp:TextBox></td>

                        <td style=" text-align:center;">
                            <asp:DropDownList ID="ddlisShow" runat="server">
                                <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                <asp:ListItem Value="0" Text="No"></asp:ListItem>
                            </asp:DropDownList></td>
                        <td style=" text-align:center;">
                            <asp:TextBox ID="txtSerial" runat="server"></asp:TextBox></td>
                        <td style=" text-align:center;">
                            <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="Add" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblResultMenu" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>

