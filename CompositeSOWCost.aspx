<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="CompositeSOWCost.aspx.cs" Inherits="CompositeSOWCost" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   <style>
         input[type=radio] + label, input[type=checkbox] + label {
            display: inline-block;
            margin: -6px 0 0 0;
            padding: 4px 5px;
            margin-bottom: 0;
            font-size: 12px;
            line-height: 20px;
            color: #333;
            text-align: center;
            text-shadow: 0 1px 1px rgba(255,255,255,0.75);
            vertical-align: middle;
            cursor: pointer;
        }
    </style>
      <table cellpadding="0" cellspacing="0" width="100%" align="center">
        <tr>
            <td align="center" class="cssHeader">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="left"><span class="titleNu">Composite SOW (Cost)</span></td>
                        <td align="right"></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center" valign="top">
                <div style="margin: 0 auto; width: 100%">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" align="center">
                                <tr>
                                    <td align="center">
                                        <table class="wrapper" width="100%">
                                            <tr>
                                                <td style="width: 220px; border-right: 1px solid #ddd;" align="left" valign="top">
                                                    <table width="100%">
                                                        <tr>
                                                            <td width="74px">
                                                                <img src="images/icon-customer-info.png" /></td>
                                                            <td align="left">
                                                                <h2>Customer Information</h2>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="width: 390px;" align="left" valign="top">
                                                    <table style="width: 390px;">
                                                        <tr>
                                                            <td style="width: 112px;" align="left" valign="top"><b>Customer Name: </b></td>
                                                            <td style="width: auto;">
                                                                <asp:Label ID="lblCustomerName" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 112px;" align="left" valign="top"><b>Phone: </b></td>
                                                            <td style="width: auto;">
                                                                <asp:Label ID="lblPhone" runat="server" Text=""></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 112px;" align="left" valign="top"><b>Email: </b></td>
                                                            <td style="width: auto;">
                                                                <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="left" valign="top">
                                                    <table style="width: 420px;">
                                                        <tr>
                                                            <td style="width: 64px;" align="left" valign="top"><b>Address: </b></td>
                                                            <td style="width: auto;" align="left" valign="top">
                                                                <asp:Label ID="lblAddress" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: auto;" align="left" valign="top">&nbsp;</td>
                                                            <td style="width: auto;" align="left" valign="top">
                                                                <asp:HyperLink ID="hypGoogleMap" runat="server" ImageUrl="~/images/img_map.gif" Target="_blank"></asp:HyperLink>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table class="wrapper" width="100%">
                                            <tr>
                                                <td style="width: 220px; border-right: 1px solid #ddd;" align="left" valign="top">
                                                    <table width="100%">
                                                        <tr>
                                                            <td width="74px">
                                                                <img src="images/icon-estimate-info.png" /></td>
                                                            <td align="left">
                                                                <h2>Estimate<br />
                                                                    Information</h2>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="width: 390px;" align="left" valign="top">
                                                    <table style="width: 390px;">
                                                        <tr>
                                                            <td style="width: 112px;" align="left" valign="top"><b>Estimate Name:</b> </td>
                                                            <td style="width: auto;">
                                                                <asp:Label ID="lblEstimateName" runat="server" Font-Bold="True"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 112px;" align="left" valign="top"><b>
                                                                <asp:Label ID="lblSaleDate" runat="server" Text="Sale Date:" Visible="False"></asp:Label>
                                                            </b></td>
                                                            <td style="width: auto;">
                                                                <asp:TextBox ID="txtSaleDate" runat="server" Visible="False"></asp:TextBox>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </td>
                                                <td align="left" valign="top">
                                                    <table style="width: 420px;">
                                                        <tr>
                                                            <td style="width: 110px;" align="left" valign="top"><b>Estimate Status:</b> </td>
                                                            <td style="width: auto;" align="left" valign="top">
                                                                <table style="padding: 0px; margin: -4px;">
                                                                    <tr style="padding: 0px; margin: 0px;">
                                                                        <td style="padding: 0px; margin: 0px;">
                                                                            <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True">
                                                                                <asp:ListItem Value="1">Pending</asp:ListItem>
                                                                                <asp:ListItem Value="2">Sit</asp:ListItem>
                                                                                <asp:ListItem Value="3">Sold</asp:ListItem>
                                                                            </asp:DropDownList></td>
                                                                        <td style="padding: 0px; margin: 0px;"><b>Tax % :</b>
                                                                            <asp:Label ID="lblTax" runat="server" Text="0" Width="15%"> </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>

                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                                <%-- <tr>
                                    <td align="center">
                                        <table id="tblTotalProjectPrice" runat="server" cellpadding="8" cellspacing="0" style="border: 1px solid #c0c0c0;"
                                            align="center" width="60%">
                                            <tr>
                                                <td style="border: 1px solid #c0c0c0;" align="center">
                                                    <h3>Project Payment Information</h3>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" style="border: 1px solid #c0c0c0;">
                                                    <table style="width: 50%;">
                                                        <tr>
                                                            <td align="right">
                                                                <b>Contract Amount:</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblProjectTotal" runat="server" Width="84px" Text="0" CssClass="cssLblAmount"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <b>C/O Amount:</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblTotalCOAmount" runat="server" Text="0" Width="84px" CssClass="cssLblAmount"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <b>Total Amount (Contract+C/O):</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblTotalAmount" runat="server" Text="0" Width="84px" CssClass="cssLblAmount"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <b>Total Received Amount:</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblTotalRecievedAmount" runat="server" Text="0" Width="84px" CssClass="cssLblAmount"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <b>Balance Due:</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblTotalBalanceAmount" runat="server" Text="0" Width="84px" CssClass="cssLblAmount"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <b>Running Item Total:</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblRunning" runat="server" Text="0" Width="84px" CssClass="cssLblAmount"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="trIncentive" runat="server" visible="false">
                                                            <td align="right">
                                                                <b>Total Incentives:</b>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblIncentive" runat="server" Text="0" Width="84px" CssClass="cssLblAmount"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td align="center">
                                        <table style="padding: 0px; margin: 0px;">
                                            <tr style="padding: 0px; margin: 0px;">
                                                <td style="padding: 0px; margin: 0px;">
                                                    <asp:RadioButtonList ID="rdoSort" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rdoSort_SelectedIndexChanged"
                                                        RepeatDirection="Horizontal">
                                                        <asp:ListItem Selected="True" Value="1">View by Locations</asp:ListItem>
                                                        <asp:ListItem Value="2">View by Sections</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td style="padding: 0px; margin: 0px;">&nbsp;</td>
                                                <td style="padding: 0px; margin: 0px;">
                                                    <asp:HyperLink ID="hyp_SowPrice" runat="server">View SOW (Retail)</asp:HyperLink>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <h3>
                                            <asp:Label ID="lblRetailPricingHeader" runat="server" Text="Selected Items" Visible="false"></asp:Label>
                                        </h3>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:GridView ID="grdGrouping" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                            OnRowDataBound="grdGrouping_RowDataBound" ShowFooter="True">
                                            <FooterStyle CssClass="white_text" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" CssClass="grid_header" Text='<%# Eval("colName").ToString() %>' />
                                                        <asp:GridView ID="grdSelectedItem1" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                                            DataKeyNames="item_id" OnRowDataBound="grdSelectedItem_RowDataBound" ShowFooter="True"
                                                            Width="100%">
                                                            <Columns>
                                                                <asp:BoundField DataField="item_id" HeaderStyle-Width="6%" HeaderText="Item Id">
                                                                    <HeaderStyle Width="6%" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="section_serial" HeaderStyle-Width="5%" HeaderText="SL">
                                                                    <HeaderStyle Width="5%" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="short_notes" HeaderStyle-Width="12%" HeaderText="Short Notes">
                                                                    <HeaderStyle Width="12%" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblHeader" runat="server" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("section_name").ToString() %>'></asp:Label>
                                                                        <asp:Label ID="lblDleted" runat="server" Text=" (Deleted Later)" Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="12%" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblSubTotalLabel" runat="server" Font-Bold="true" Font-Size="13px" />
                                                                    </FooterTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="item_name" HeaderStyle-Width="28%" HeaderText="Item Name">
                                                                    <HeaderStyle Width="28%" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="measure_unit" HeaderStyle-Width="6%" HeaderText="UoM" NullDisplayText=" ">
                                                                    <HeaderStyle Width="6%" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="item_cost" HeaderStyle-Width="5%" HeaderText="Unit Price"
                                                                    Visible="false">
                                                                    <HeaderStyle Width="5%" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="quantity" HeaderStyle-Width="5%" HeaderText="Code" ItemStyle-HorizontalAlign="Right">
                                                                    <HeaderStyle Width="5%" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Ext. Cost" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblSubTotal" runat="server" Font-Bold="true" Font-Size="13px" />
                                                                    </FooterTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTotal_price" runat="server" Text='<%# Eval("total_retail_Cost","{0:c}").ToString() %>' />
                                                                        <asp:Label ID="lblT_price1" runat="server" Visible="false" Text='<%# Eval("total_retail_Cost") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="7%" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="labor_rate" HeaderStyle-Width="1%" HeaderText="Labor Rate"
                                                                    Visible="False">
                                                                    <HeaderStyle Width="1%" />
                                                                </asp:BoundField>

                                                                <asp:BoundField DataField="tmpCo" HeaderStyle-Width="8%" HeaderText="Item Status" ItemStyle-HorizontalAlign="Center">
                                                                    <HeaderStyle Width="8%" />
                                                                </asp:BoundField>
                                                            </Columns>
                                                            <PagerStyle CssClass="pgr" />
                                                            <AlternatingRowStyle CssClass="alt" />
                                                        </asp:GridView>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <%# GetTotalPrice()%>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerStyle CssClass="pgr" />
                                            <AlternatingRowStyle CssClass="alt" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <table width="100%" border="0" cellspacing="4" cellpadding="4">
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <h3>
                                                        <asp:Label ID="lblDirectPricingHeader" runat="server" Text="The following items are Direct / Outsourced"
                                                            Visible="False"></asp:Label></h3>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2" valign="top" width="15%">
                                                    <asp:GridView ID="grdGroupingDirect" runat="server" AutoGenerateColumns="False" CaptionAlign="Top"
                                                        CssClass="mGrid" OnRowDataBound="grdGroupingDirect_RowDataBound" ShowFooter="True">
                                                        <FooterStyle CssClass="white_text" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label2" runat="server" CssClass="grid_header" Text='<%# Eval("colName").ToString() %>' />
                                                                    &nbsp;<asp:GridView ID="grdSelectedItem2" runat="server" AutoGenerateColumns="False"
                                                                        CssClass="mGrid" DataKeyNames="item_id" OnRowDataBound="grdSelectedItem2_RowDataBound"
                                                                        ShowFooter="True" Width="100%">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="item_id" HeaderStyle-Width="6%" HeaderText="Item Id" />
                                                                            <asp:BoundField DataField="section_serial" HeaderStyle-Width="5%" HeaderText="SL" />
                                                                            <asp:BoundField DataField="short_notes" HeaderStyle-Width="12%" HeaderText="Short Notes" />
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lblHeader2" runat="server" />
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblItemName2" runat="server" Text='<%# Eval("section_name").ToString() %>'></asp:Label>
                                                                                      <asp:Label ID="lblDleted2" runat="server" Text=" (Deleted Later)" Visible="false"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="12%" />
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblSubTotalLabel2" runat="server" Font-Bold="true" Font-Size="13px" />
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="item_name" HeaderStyle-Width="28%" HeaderText="Item Name" />
                                                                            <asp:BoundField DataField="measure_unit" HeaderStyle-Width="6%" HeaderText="UoM" NullDisplayText=" " />
                                                                            <asp:BoundField DataField="item_cost" HeaderStyle-Width="5%" HeaderText="Unit Price"
                                                                                Visible="false" />
                                                                            <asp:BoundField DataField="quantity" HeaderStyle-Width="5%" HeaderText="Code" />
                                                                            <asp:TemplateField HeaderText="Direct Cost">
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblSubTotal2" runat="server" Font-Bold="true" Font-Size="13px" />
                                                                                </FooterTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTotal_price2" runat="server" Text='<%# Eval("total_direct_Cost","{0:c}").ToString() %>' />
                                                                                    <asp:Label ID="lblT_price2" runat="server" Visible="false" Text='<%# Eval("total_direct_Cost") %>' />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="7%" />
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="labor_rate" HeaderStyle-Width="1%" HeaderText="Labor Rate"
                                                                                Visible="False" />

                                                                            <asp:BoundField DataField="tmpCo" HeaderStyle-Width="8%" HeaderText="Item Status">
                                                                                <HeaderStyle Width="8%" />
                                                                            </asp:BoundField>
                                                                        </Columns>
                                                                        <PagerStyle CssClass="pgr" />
                                                                        <AlternatingRowStyle CssClass="alt" />
                                                                    </asp:GridView>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <%# GetTotalPriceDirect()%>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerStyle CssClass="pgr" />
                                                        <AlternatingRowStyle CssClass="alt" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top" width="15%">Comments:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtComments" runat="server" Height="44px" TabIndex="1" TextMode="MultiLine"
                                                        Width="603px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top">
                                        <table>
                                            <tr>
                                                <td align="right">Report View Options: </td>
                                                <td align="center" valign="top">
                                                    <asp:CheckBoxList ID="chkCVOptions" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Selected="False" Value="1">&nbsp;Show Code</asp:ListItem>
                                                        <asp:ListItem Selected="False" Value="2">&nbsp;Show UOM</asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                                <tr>
                                    <td align="center" valign="top">
                                        <asp:Button ID="btnGotoCustomerList" runat="server" CssClass="button" OnClick="btnGotoCustomerList_Click" Text="Go to Customer List" />
                                        &nbsp;<asp:Button ID="btnPrintByLoc" runat="server" CssClass="button" OnClick="btnPrintByLoc_Click" Text="Print SOW by Location" />
                                        <asp:Button ID="btnPrintBySec" runat="server" CssClass="button" OnClick="btnPrintBySec_Click" Text="Print SOW by Section" />
                                    </td>
                                </tr>

                                <tr>
                                    <td align="left" valign="top">
                                        <table cellpadding="2" cellspacing="2" align="center" width="100%">
                                            <tr>
                                                <td align="right">&nbsp;
                                                </td>
                                                <td align="left">&nbsp;
                                                </td>
                                                <td align="right">&nbsp;
                                                </td>
                                                <td align="left">
                                                    <asp:HiddenField ID="hdnCustomerId" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <table cellpadding="2" cellspacing="2" align="center" width="100%">
                                            <tr>
                                                <td align="right">
                                                    <asp:HiddenField ID="hdnSectionLevel" runat="server" Value="0" />
                                                </td>
                                                <td align="left">
                                                    <asp:HiddenField ID="hdnEstimateId" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnPricingId" runat="server" Value="0" />
                                                </td>
                                                <td align="right">&nbsp;
                                                </td>
                                                <td align="left">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="UpdatePanel2"
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
                </div>
            </td>
        </tr>
    </table>
</asp:Content>

