<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="salespersonmanagement.aspx.cs"
    Inherits="salespersonmanagement" Title="Sales Person Information" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table id="Table5" align="center" width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center">
                        <h1>Sales Person Management</h1>
                    </td>
                </tr>
                <tr>
                    <td  align="center">
                        <asp:Label ID="lblResult" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grdUserList" runat="server" AutoGenerateColumns="False"
                            DataKeyNames="user_id"
                            Width="100%" CssClass="mGrid" OnRowDataBound="grdUserList_RowDataBound" OnRowEditing="grdUserList_RowEditing" OnRowUpdating="grdUserList_RowUpdating">
                            <PagerSettings Position="TopAndBottom" />
                            <Columns>
                                <asp:BoundField DataField="last_name" HeaderText="Last Name" ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="first_name" HeaderText="First Name" ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Address" ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="company_email" HeaderText="Email" ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="role_id" HeaderText="Role" ReadOnly="true">
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Sales Person Type" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblgrdSalesPersonType" runat="server" />
                                        <asp:DropDownList ID="ddlSalesPersonType" runat="server" AutoPostBack="true" Visible="false" 
                                            OnSelectedIndexChanged="ddlSalesPersonType_OnSelectedIndexChanged">
                                            <asp:ListItem Value="1" Text="Type 1"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="Type 2"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="left" Width="10%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Commission">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAddiotional_Commission" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtAddiotional_Commission" runat="server" Visible="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="6%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Assign">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAssignedTo" runat="server" />
                                        <asp:DropDownList ID="ddlAssignedTo" runat="server" Visible="false" 
                                             DataValueField="sales_person_id" DataTextField="sales_person_name" DataSource="<%#dtSalesperson%>">                                            
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="6%" />
                                </asp:TemplateField>
                                <asp:ButtonField CommandName="Edit" Text="Edit">
                                    <ItemStyle HorizontalAlign="Center" Width="5%" />
                                </asp:ButtonField>
                            </Columns>
                            <PagerStyle HorizontalAlign="Left" CssClass="pgr" />
                            <AlternatingRowStyle CssClass="alt" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:HiddenField ID="hdnUserId" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnSalesPersonId" runat="server" Value="0" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
   <%-- <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="UpdatePanel1"
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
    </asp:UpdateProgress>--%>
</asp:Content>
