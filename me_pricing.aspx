﻿<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="me_pricing.aspx.cs" Inherits="me_pricing" Title="Estimation Template Pricing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="Javascript" type="text/javascript">
        function ChangeImage(id) {
            document.getElementById(id).src = 'Images/loading.gif';
        }
    </script>
     <script language="javascript" type="text/javascript">
         function selected_ItemName(sender, e) {
             document.getElementById('<%=btnSearch.ClientID%>').click();
        }
        function selected_ItemNameAll(sender, e) {
            document.getElementById('<%=btnSearchAll.ClientID%>').click();
        }
    </script>
    <script type="text/javascript">
<!--
    function Check_Click(objRef) {
        //Get the Row based on checkbox
        var row = objRef.parentNode.parentNode;

        //Get the reference of GridView
        var GridView = row.parentNode;

        //Get all input elements in Gridview
        var inputList = GridView.getElementsByTagName("input");

        for (var i = 0; i < inputList.length; i++) {
            //The First element is the Header Checkbox
            var headerCheckBox = inputList[0];

            //Based on all or none checkboxes
            //are checked check/uncheck Header Checkbox
            var checked = true;
            if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                if (!inputList[i].checked) {
                    checked = false;
                    break;
                }
            }
        }
        headerCheckBox.checked = checked;

    }
    function checkAll(objRef) {
        var GridView = objRef.parentNode.parentNode.parentNode;
        var inputList = GridView.getElementsByTagName("input");
        for (var i = 0; i < inputList.length; i++) {
            var row = inputList[i].parentNode.parentNode;
            if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                if (objRef.checked) {
                    inputList[i].checked = true;
                }
                else {
                    inputList[i].checked = false;
                }
            }
        }
    }
    //-->
    </script>
    <table cellpadding="0" cellspacing="2" width="100%" align="center">
        <tr>
            <td align="center" class="cssHeader">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="left"><span class="titleNu">Estimation Template Pricing</span></td>
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
                            <table width="100%" border="0" cellspacing="4" cellpadding="4" align="center">
                                   <tr>
                                    <td align="center">
                                        <table class="wrapper" width="100%">
                                            <tr>
                                                <td style="width: 260px; border-right: 1px solid #ddd;" align="left" valign="top">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td align="left">
                                                                <h2><b>Estimate</b>Information</h2>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="width: 390px;" align="left" valign="top">
                                                    <table style="width: 520px;">
                                                        <tr>
                                                            <td style="height: 18px;" align="left" valign="top" colspan="2">
                                                                <asp:Label ID="Label6" runat="server" Font-Bold="True" ForeColor="Red" Text="* required"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 150px; height:28px;" align="left" valign="middle"><b>Estimate Name: </b></td>
                                                            <td style="width: auto; height:28px;">
                                                                <asp:Label ID="lblModelEstimateName" runat="server" Font-Bold="True"></asp:Label>
                                                                <asp:LinkButton ID="lnkUpdateModelEstimate" runat="server"><span 
                                    style="color:#2d7dcf; text-decoration:underline; font-weight:bold; ">Rename</span></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 150px;" valign="top"><b>Locations: </b></td>
                                                            <td style="width: auto;">
                                                                <asp:DropDownList ID="ddlCustomerLocations" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomerLocations_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                                                                <asp:LinkButton ID="lnkAddMoreLocation" runat="server" OnClick="lnkAddMoreLocation_Click"><span style="color:#2d7dcf; text-decoration:underline; font-weight:bold;">Add/Remove Location</span></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label ID="lblSelectLocation" runat="server" Text=""></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="left" valign="top">
                                                    <table style="width: 520px;">
                                                        <tr>
                                                            <td align="left" valign="top" colspan="2"><b>&nbsp;<asp:CheckBox ID="chkIsPublic" runat="server" Checked="True" TabIndex="14" Text="Public" />
                                                                &nbsp;(Viewable to Others) </b></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: auto;" align="left" valign="top">
                                                                &nbsp;</td>
                                                            <td style="width: auto; height: 25px;" align="left" valign="top">
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: auto;" valign="top"><b>Sections: </b></td>
                                                            <td align="left" style="width: auto;" valign="top">
                                                                <asp:DropDownList ID="ddlSections" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSections_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:Label ID="Label4" runat="server" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                                                                <asp:LinkButton ID="lnkAddMoreSections" runat="server" OnClick="lnkAddMoreSections_Click"><span style="color:#2d7dcf; text-decoration:underline; font-weight:bold;">Add/Remove Section</span></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                 <tr>
                                    <td align="left" valign="top">
                                        <table style="width: 100%;" class="wrapper" runat="server" id="tblPricingWrapper" visible="false">
                                            <tr>
                                                <td align="left" rowspan="5" valign="top">
                                                    <asp:TreeView ID="trvSection" runat="server" ImageSet="Msdn" NodeIndent="10" OnSelectedNodeChanged="trvSection_SelectedNodeChanged">
                                                        <ParentNodeStyle Font-Bold="False" />
                                                        <HoverNodeStyle BackColor="#CCCCCC" BorderColor="#888888" BorderStyle="Solid" Font-Underline="True" />
                                                        <SelectedNodeStyle BackColor="White" BorderColor="#888888" BorderStyle="Solid" BorderWidth="1px"
                                                            Font-Underline="False" HorizontalPadding="3px" VerticalPadding="1px" />
                                                        <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                                                            NodeSpacing="1px" VerticalPadding="2px" />
                                                    </asp:TreeView>
                                                </td>
                                                <td align="left" valign="top">
                                                    <asp:Label ID="lblParent" runat="server" Font-Bold="True" ForeColor="Blue"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top">
                                                    <asp:TextBox ID="txtSearchItemName" runat="server" Width="50%" Visible="False"></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" runat="server" CompletionInterval="500" CompletionListCssClass="AutoExtender" CompletionSetCount="10" DelimiterCharacters="" EnableCaching="true" Enabled="True" MinimumPrefixLength="1" OnClientItemSelected="selected_ItemName" ServiceMethod="GetItemName" TargetControlID="txtSearchItemName" UseContextKey="True">
                                                    </cc1:AutoCompleteExtender>
                                                    <cc1:TextBoxWatermarkExtender ID="wtmFileNumber" runat="server" TargetControlID="txtSearchItemName" WatermarkText="Search by Item Name" />
                                                    <asp:Button ID="btnSearch" runat="server" CssClass="button" OnClick="btnSearch_Click" Text="Search" Visible="False" />
                                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnkViewAll_Click" Visible="False">View All</asp:LinkButton>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top">
                                                    <asp:GridView ID="grdItemPrice" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                                        DataKeyNames="item_id" OnRowCommand="grdItemPrice_RowCommand" PageSize="200"
                                                        TabIndex="2" Width="100%" >
                                                        <Columns>
                                                            <asp:ButtonField CommandName="Add" Text="Add" />
                                                            <asp:BoundField DataField="item_id" HeaderText="Item Id" />
                                                            <asp:BoundField DataField="section_serial" HeaderText="SL" />
                                                            <asp:BoundField DataField="section_name" HeaderText="Item Name">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" Width="280px" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Short Notes">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtShortNote" runat="server" MaxLength="98"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Labor">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlLabor" runat="server" SelectedValue='<%# Eval("labor_id") %>'>
                                                                        <asp:ListItem Value="2">Yes</asp:ListItem>
                                                                        <asp:ListItem Value="1">No</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Direct">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlDirect" runat="server">
                                                                        <asp:ListItem Value="1">No</asp:ListItem>
                                                                        <asp:ListItem Value="2">Yes</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="measure_unit" HeaderText="UoM"  NullDisplayText=" ">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="item_cost" DataFormatString="{0:c}" HeaderText="Unit Price">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="minimum_qty" HeaderText="Min Code">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Code">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtQty" runat="server" Style="text-align: center;" Text='<%# Eval("minimum_qty") %>' Width="40px"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                           <%-- <asp:TemplateField HeaderText="Code">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtQty" runat="server" AutoPostBack="True" Style="text-align: center;" Text='<%# Eval("minimum_qty") %>'
                                                                        Width="40px" OnTextChanged="ItemPrice_calculation"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Ext. Price">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTotalPrice" runat="server" Text='<%# Eval("ext_item_cost","{0:c}").ToString() %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>--%>


                                                        </Columns>
                                                        <AlternatingRowStyle CssClass="alt" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top">
                                                    <table class="tabGrid" width="100%" cellpadding="0" cellspacing="0" id="tdlOther" runat="server" height="auto" visible="True">
                                                        <tr>
                                                            <td></td>
                                                            <td align="left"><b style="font-weight:bold;color:#555;">Other Item Name</b>
                                                            </td>
                                                            <td align="left"><b style="font-weight:bold;color:#555;">UoM</b>
                                                            </td>
                                                            <td align="left"><b style="font-weight:bold;color:#555;">Unit Price</b>
                                                            </td>
                                                            <td align="left"><b style="font-weight:bold;color:#555;">Code</b>
                                                            </td>
                                                            <%-- <td align="center">Ext. Price
                                                            </td>--%>
                                                            <td align="left"><b style="font-weight:bold;color:#555;">Direct</b>
                                                            </td>
                                                            <td align="left"><b style="font-weight:bold;color:#555;">Short Notes</b>
                                                            </td>

                                                        </tr>
                                                        <tr>
                                                            <td class="tabGridtd" valign="top">
                                                                <asp:Button ID="btnAddOthers" runat="server" CssClass="button" OnClick="btnAddOthers_Click"
                                                                    Text="Add" />
                                                            </td>
                                                            <td class="tabGridtd" valign="top">
                                                                <asp:TextBox ID="txtOther" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td class="tabGridtd" valign="top">
                                                                <asp:TextBox ID="txtO_Unit" runat="server"></asp:TextBox>
                                                            </td>
                                                            <td class="tabGridtd" valign="top">
                                                                <asp:TextBox ID="txtO_Price" runat="server" Style="text-align: right;"></asp:TextBox>
                                                            </td>
                                                            <td class="tabGridtd" valign="top">
                                                                <asp:TextBox ID="txtO_Qty" runat="server" Style="text-align: center;"></asp:TextBox>
                                                            </td>
                                                            <%-- <td valign="top">
                                                                <asp:TextBox ID="txtO_TotalPrice" Style="text-align: right;" runat="server" ReadOnly="True" Width="40px"></asp:TextBox>
                                                            </td>--%>
                                                            <td class="tabGridtd" valign="top">
                                                                <asp:DropDownList ID="ddlO_Direct" runat="server">
                                                                    <asp:ListItem Value="1">No</asp:ListItem>
                                                                    <asp:ListItem Value="2">Yes</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="tabGridtd" valign="top">
                                                                <asp:TextBox ID="txtO_ShortNotes" runat="server"></asp:TextBox>
                                                            </td>

                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <table width="100%" id="tblMultiPricing" runat="server" visible="false" class="wrapper">
                                            <tr>
                                                <td align="left" valign="top" width="70%">
                                                    <asp:TextBox ID="txtSearchAll" runat="server" Width="50%" ></asp:TextBox>
                                                    <cc1:AutoCompleteExtender ID="AutoCompleteExtender1" runat="server" CompletionInterval="500" CompletionListCssClass="AutoExtender" CompletionSetCount="10" DelimiterCharacters="" EnableCaching="true" Enabled="True" MinimumPrefixLength="1" OnClientItemSelected="selected_ItemNameAll" ServiceMethod="GetItemNameAll" TargetControlID="txtSearchAll" UseContextKey="True">
                                                    </cc1:AutoCompleteExtender>
                                                    <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" TargetControlID="txtSearchAll" WatermarkText="Search by Item Name" />
                                                    <asp:Button ID="btnSearchAll" runat="server" CssClass="button" OnClick="btnSearchAll_Click" Text="Search"  />
                                                    <asp:LinkButton ID="lnkAllViewA" runat="server" OnClick="lnkAllViewA_Click" >Reset</asp:LinkButton>
                                                </td>
                                                <td align="right">

                                                    <asp:Button ID="btnAddMultiple" runat="server" CssClass="button" Visible="false" Text="Add Selected Item in Pricing" OnClick="btnAddMultiple_Click" />

                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:GridView ID="grdItemPriceAll" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                                        DataKeyNames="item_id" PageSize="200"
                                                        TabIndex="2" Width="99%" OnRowDataBound="grdItemPriceAll_RowDataBound">
                                                        <Columns>
                                                            <asp:BoundField DataField="item_id" HeaderText="Item Id" />
                                                            <asp:BoundField HeaderText="Section Name">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="section_name" HeaderText="Item Name">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" Width="40%" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Short Notes">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtShortNote" runat="server" MaxLength="98"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Labor">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlLabor" runat="server" SelectedValue='<%# Eval("labor_id") %>'>
                                                                        <asp:ListItem Value="2">Yes</asp:ListItem>
                                                                        <asp:ListItem Value="1">No</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Direct">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlDirect" runat="server">
                                                                        <asp:ListItem Value="1">No</asp:ListItem>
                                                                        <asp:ListItem Value="2">Yes</asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="measure_unit" HeaderText="UoM"  NullDisplayText=" ">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="item_cost" DataFormatString="{0:c}" HeaderText="Unit Price">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="LaborUnitCost" DataFormatString="{0:c}" HeaderText="Labor Rate">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="minimum_qty" HeaderText="Min Code">
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Code">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtQty" runat="server" Style="text-align: center;" Text='<%# Eval("minimum_qty") %>' Width="40px"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField HeaderText="Ext. Price">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTotalPrice" runat="server" Text='<%# Eval("ext_item_cost","{0:c}").ToString() %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkIsSelectedAll" runat="server" onclick="checkAll(this);" Text="All" TextAlign="Left" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkIsSelected" runat="server" onclick="Check_Click(this)" />
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBox_CheckChanged" onclick="checkAll(this);" Text="All" TextAlign="Left" />
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chk" runat="server" AutoPostBack="true" OnCheckedChanged="CheckBox_CheckChanged" onclick="Check_Click(this)" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>--%>
                                                        </Columns>
                                                        <AlternatingRowStyle CssClass="alt" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                                <tr>
                                    <td align="center" valign="top">
                                        <asp:Label ID="lblAdd" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table id="tblTotalProjectPrice" visible="false" runat="server" cellpadding="8" cellspacing="0"
                                            style="border: 1px solid #c0c0c0;" align="center" width="60%">
                                            <tr>
                                                <td colspan="3" style="border: 1px solid #c0c0c0;" align="center">
                                                    <h3>Total Project Price</h3>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border: 1px solid #c0c0c0;" align="center" valign="top">
                                                    <strong>Total Price </strong>
                                                </td>
                                                <td style="border: 1px solid #c0c0c0;" align="center" valign="top">
                                                    <strong>Direct Price</strong>
                                                </td>
                                                <td style="border: 1px solid #c0c0c0;" align="center" valign="top">
                                                    <strong>Total Price +&nbsp; Direct Price</strong>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="border: 1px solid #c0c0c0;" align="center" valign="top">
                                                    <asp:Label ID="lblRetailTotalCost" runat="server"></asp:Label>
                                                </td>
                                                <td style="border: 1px solid #c0c0c0;" align="center" valign="top">
                                                    <asp:Label ID="lblDirctTotalCost" runat="server"></asp:Label>
                                                </td>
                                                <td style="border: 1px solid #c0c0c0;" align="center" valign="top">
                                                    <asp:Label ID="lblGrandTotalCost" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:RadioButtonList ID="rdoSort" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rdoSort_SelectedIndexChanged"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True" Value="1">View by Locations</asp:ListItem>
                                            <asp:ListItem Value="2">View by Sections</asp:ListItem>
                                        </asp:RadioButtonList>
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
                                        <asp:GridView ID="grdGrouping" runat="server" ShowFooter="True" OnRowDataBound="grdGrouping_RowDataBound"
                                            AutoGenerateColumns="False" CssClass="mGrid" OnRowCommand="grdGrouping_RowCommand">
                                            <FooterStyle CssClass="white_text" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("colName").ToString() %>' CssClass="grid_header" />
                                                        &nbsp;<asp:LinkButton ID="lnkMove1" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>'
                                                            CommandName="Move" CssClass="moveUp">Move to Top</asp:LinkButton>
                                                        <asp:GridView ID="grdSelectedItem1" runat="server" AutoGenerateColumns="False" ShowFooter="True"
                                                            DataKeyNames="item_id" OnRowDataBound="grdSelectedItem_RowDataBound" OnRowDeleting="grdSelectedItem_RowDeleting"
                                                            OnRowEditing="grdSelectedItem_RowEditing" Width="100%" CssClass="mGrid" OnRowUpdating="grdSelectedItem1_RowUpdating">
                                                            <Columns>
                                                                <asp:BoundField DataField="item_id" HeaderText="Item Id" ReadOnly="True">
                                                                    <HeaderStyle Width="5%" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="section_serial" HeaderText="SL" ReadOnly="True">
                                                                    <HeaderStyle Width="5%" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblHeader" runat="server" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("section_name").ToString() %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblSubTotalLabel" runat="server" Font-Bold="true" Font-Size="13px" />
                                                                    </FooterTemplate>
                                                                    <HeaderStyle Width="11%" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="item_name" HeaderText="Item Name" ReadOnly="True">
                                                                    <HeaderStyle Width="25%" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="measure_unit" HeaderText="UoM" ReadOnly="True"  NullDisplayText=" ">
                                                                    <HeaderStyle Width="6%" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="item_cost" HeaderText="Unit Price" Visible="false">
                                                                    <HeaderStyle Width="5%" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblquantity" runat="server" Text='<%# Eval("quantity") %>' />
                                                                        <asp:TextBox ID="txtquantity" runat="server" Style="text-align: center;" Visible="false" Width="40px" Text='<%# Eval("quantity") %>'
                                                                            AutoPostBack="True" OnTextChanged="NonDirect_calculation" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="5%" />
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Ext. Price">
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblSubTotal" runat="server" Font-Bold="true" Font-Size="13px" />
                                                                    </FooterTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblTotal_price" runat="server" Text='<%# Eval("total_retail_price","{0:c}").ToString() %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="7%" />
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="labor_rate" HeaderText="Labor Rate" Visible="False" ReadOnly="True">
                                                                    <HeaderStyle Width="1%" />
                                                                </asp:BoundField>
                                                                <asp:TemplateField HeaderText="Short Notes">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblshort_notes" runat="server" Text='<%# Eval("short_notes") %>' />
                                                                        <asp:TextBox ID="txtshort_notes" runat="server" Visible="false" Text='<%# Eval("short_notes") %>'
                                                                           TextMode="MultiLine" />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle Width="20%" />
                                                                </asp:TemplateField>
                                                                <asp:ButtonField CommandName="Edit" Text="Edit">
                                                                    <HeaderStyle Width="5%" />
                                                                </asp:ButtonField>
                                                                <asp:ButtonField CommandName="Delete" Text="Delete">
                                                                    <HeaderStyle Width="5%" />
                                                                </asp:ButtonField>
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
                                                    <asp:GridView ID="grdGroupingDirect" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                                        OnRowDataBound="grdGroupingDirect_RowDataBound" ShowFooter="True" CaptionAlign="Top"
                                                        OnRowCommand="grdGroupingDirect_RowCommand">
                                                        <FooterStyle CssClass="white_text" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label2" runat="server" CssClass="grid_header" Text='<%# Eval("colName").ToString() %>' />
                                                                    &nbsp;<asp:LinkButton ID="lnkMove2" runat="server" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>'
                                                                        CommandName="Move1" CssClass="moveUp">Move to Top</asp:LinkButton>
                                                                    <asp:GridView ID="grdSelectedItem2" runat="server" AutoGenerateColumns="False" CssClass="mGrid"
                                                                        DataKeyNames="item_id" OnRowDataBound="grdSelectedItem2_RowDataBound" OnRowDeleting="grdSelectedItem2_RowDeleting"
                                                                        OnRowEditing="grdSelectedItem2_RowEditing" ShowFooter="True" Width="100%" OnRowUpdating="grdSelectedItem2_RowUpdating">
                                                                        <Columns>
                                                                            <asp:BoundField DataField="item_id" HeaderText="Item Id" ReadOnly="True">
                                                                                <HeaderStyle Width="5%" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="section_serial" HeaderText="SL" ReadOnly="True">
                                                                                <HeaderStyle Width="5%" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <asp:Label ID="lblHeader2" runat="server" />
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblItemName2" runat="server" Text='<%# Eval("section_name").ToString() %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblSubTotalLabel2" runat="server" Font-Bold="true" Font-Size="13px" />
                                                                                </FooterTemplate>
                                                                                <HeaderStyle Width="11%" />
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="item_name" HeaderText="Item Name" ReadOnly="True">
                                                                                <HeaderStyle Width="25%" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="measure_unit" HeaderText="UoM" ReadOnly="True"  NullDisplayText=" ">
                                                                                <HeaderStyle Width="6%" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:BoundField>
                                                                            <asp:BoundField DataField="item_cost" HeaderText="Unit Price" Visible="false">
                                                                                <HeaderStyle Width="5%" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Code">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblquantity1" runat="server" Text='<%# Eval("quantity") %>' />
                                                                                    <asp:TextBox ID="txtquantity1" runat="server" Style="text-align: center;" Visible="false" Width="40px" Text='<%# Eval("quantity") %>'
                                                                                        AutoPostBack="True" OnTextChanged="Direct_calculation" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="5%" />
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Direct Price">
                                                                                <FooterTemplate>
                                                                                    <asp:Label ID="lblSubTotal2" runat="server" Font-Bold="true" Font-Size="13px" />
                                                                                </FooterTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTotal_price2" runat="server" Text='<%# Eval("total_direct_price","{0:c}").ToString() %>' />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="7%" />
                                                                                <ItemStyle HorizontalAlign="Right" />
                                                                                <FooterStyle HorizontalAlign="Right" />
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="labor_rate" HeaderText="Labor Rate" Visible="False">
                                                                                <HeaderStyle Width="1%" />
                                                                            </asp:BoundField>
                                                                            <asp:TemplateField HeaderText="Short Notes">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblshort_notes1" runat="server" Text='<%# Eval("short_notes") %>' />
                                                                                    <asp:TextBox ID="txtshort_notes1" runat="server" Visible="false" Text='<%# Eval("short_notes") %>'
                                                                                       TextMode="MultiLine" />
                                                                                </ItemTemplate>
                                                                                <HeaderStyle Width="20%" />
                                                                            </asp:TemplateField>
                                                                            <asp:ButtonField CommandName="Edit" Text="Edit">
                                                                                <HeaderStyle Width="5%" />
                                                                            </asp:ButtonField>
                                                                            <asp:ButtonField CommandName="Delete" Text="Delete">
                                                                                <HeaderStyle Width="5%" />
                                                                            </asp:ButtonField>
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
                                                <td align="right" valign="top" width="15%">
                                                    <b>Comments: </b>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtComments" runat="server" Height="44px" TabIndex="1" TextMode="MultiLine"
                                                        Width="85%"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top">
                                        <asp:Label ID="lblResult1" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top">
                                        <asp:Button ID="btnSave" runat="server" CssClass="button" OnClick="btnSave_Click"
                                            Text="Update" Width="80px" />
                                        <asp:Button ID="btnAssignToCustomer" runat="server" CssClass="button" Text="Assign to a Customer"
                                            EnableViewState="False" OnClick="btnAssignToCustomer_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top"></td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top"></td>
                                </tr>
                                <tr>
                                    <td align="center"></td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">
                                        <table cellpadding="2" cellspacing="2" align="center" width="100%">
                                            <tr>
                                                <td align="right">&nbsp;
                                                </td>
                                                <td align="left">&nbsp;
                                                </td>
                                                <td align="right">
                                                    <asp:HiddenField ID="hdnSalesPersonId" runat="server" Value="0" />
                                                </td>
                                                <td align="left">
                                                    <asp:HiddenField ID="hdnEstimateId" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnOtherId" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">&nbsp;
                                                </td>
                                                <td align="left">&nbsp;
                                                </td>

                                                <td align="left">
                                                    <asp:HiddenField ID="hdnItemCnt" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnSectionLevel" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnParentId" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnPricingId" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnSortDesc" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnSectionSerial" runat="server" Value="0" />
                                                    <asp:HiddenField ID="hdnSectionId" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top"></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <cc1:ModalPopupExtender ID="modUpdateEstimate" runat="server" BackgroundCssClass="modalBackground"
                                            DropShadow="false" PopupControlID="pnlUpdateEstimate" TargetControlID="lnkUpdateModelEstimate">
                                        </cc1:ModalPopupExtender>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:Panel ID="pnlUpdateEstimate" runat="server" Width="550px" Height="150px" BackColor="Snow">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <table cellpadding="0" cellspacing="2" width="100%">
                                    <tr>
                                        <td align="center">
                                            <b>Update Estimate Name</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <table cellpadding="0" cellspacing="2" width="98%">
                                                <tr>
                                                    <td align="right" width="30%">
                                                        <b>Estimate Name: </b>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Label ID="lblExistingModelEstimateName" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="right">
                                                        <b>New Estimate Name: </b>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtNewEstimateName" runat="server" TabIndex="1" Width="200px"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" TabIndex="2" Width="80px"
                                                            OnClick="btnSubmit_Click" CssClass="button" />
                                                        &nbsp;<asp:Button ID="btnClose" runat="server" Text="Close" TabIndex="3" Width="80px"
                                                            OnClick="btnClose_Click" CssClass="button" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
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

