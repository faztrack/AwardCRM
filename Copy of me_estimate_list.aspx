<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="Copy of me_estimate_list.aspx.cs" Inherits="me_estimate_list" Title="Estimation Template List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="Javascript" type="text/javascript">
        function ChangeImage(id) {
            document.getElementById(id).src = 'Images/loading.gif';
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="2" width="100%" align="center">
                <tr>
                    <td align="center" class="cssHeader">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td align="left"><span class="titleNu">Estimation Template List</span></td>
                                <td align="right"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table cellpadding="4" cellspacing="4" width="900px">
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td align="right">
                                                <b>Estimation Template Name: </b>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtSearch" style="width:350px;" runat="server"></asp:TextBox>
                                                &nbsp;<asp:Button ID="btnSearch" runat="server" Text="Search"
                                                    OnClick="btnSearch_Click" CssClass="button" />
                                            </td>
                                            <td align="right">&nbsp;</td>
                                            <td align="left">&nbsp;</td>
                                            <td align="right">
                                                <asp:Button ID="btnAddNew" runat="server" OnClick="btnAddNew_Click"
                                                    Text="Add New Estimation Template" CssClass="button" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">&nbsp;</td>
                                            <td align="right">&nbsp;</td>
                                            <td align="left">&nbsp;
                                            </td>
                                            <td align="left">&nbsp;</td>
                                            <td align="right">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="labelTitleGrn" align="center" colspan="5">
                                                <asp:Label ID="lblEstimate2" style="color:#fff;" runat="server" Text="Public Templates"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="5">
                                                <asp:GridView ID="grdPublicEstimationList" runat="server"
                                                    AutoGenerateColumns="False" CellPadding="5" CssClass="mGrid"
                                                    OnRowDataBound="grdPublicEstimationList_RowDataBound" PageSize="10"
                                                    Width="100%">
                                                    <PagerSettings Position="TopAndBottom" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Estimate Name">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="hypEstName1" runat="server">[hypEstName]</asp:HyperLink>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="sales_person_name" HeaderText="Sales Person">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="create_date" HeaderText="Create Date">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <PagerStyle CssClass="pgr" HorizontalAlign="Left" />
                                                    <AlternatingRowStyle CssClass="alt" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">&nbsp;</td>
                                            <td align="right"></td>
                                            <td align="left"></td>
                                            <td align="left">&nbsp;</td>
                                            <td align="right">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td class="labelTitleBlu" align="center" colspan="5">
                                                <asp:Label ID="lblTemplate1" style="color:#fff;" runat="server" Text="Your Templates"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="5">
                                                <asp:GridView ID="grdEstimationList" runat="server" AutoGenerateColumns="False"
                                                    CellPadding="5" CssClass="mGrid"
                                                    OnRowDataBound="grdEstimationList_RowDataBound" PageSize="10" Width="100%">
                                                    <PagerSettings Position="TopAndBottom" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Estimate Name">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="hypEstName" runat="server">[hypEstName]</asp:HyperLink>
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="sales_person_name" HeaderText="Sales Person">
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="create_date" HeaderText="Create Date">
                                                            <HeaderStyle HorizontalAlign="Center" />
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <PagerStyle CssClass="pgr" HorizontalAlign="Left" />
                                                    <AlternatingRowStyle CssClass="alt" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
 <%--   <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="False">
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

