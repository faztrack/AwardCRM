<%@ Page Title="Selection Sheet" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="SelectionSheet.aspx.cs" Inherits="SelectionSheet" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        function confirmDelete() {
            return confirm("Are you sure that you want to delete Cabinet?");
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table cellpadding="0" cellspacing="2" width="100%">
                <tr>
                    <td align="center">
                        <h1>Selection Sheet</h1>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblResult" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="2" width="100%">
                            <tr>
                                <td style="background-color: #172087; padding: 4px; color: #fff; text-align: left;" align="center">
                                    <asp:Panel ID="Panel4" runat="server">
                                        <b style="color: #fff;">
                                            <asp:CheckBox ID="chkCabinet" runat="server" Style="color: #fff;" Text="Cabinet" OnCheckedChanged="chkSelection_CheckedChanged" AutoPostBack="true" /></b>
                                    </asp:Panel>
                                    <cc1:CollapsiblePanelExtender ID="cpnlExtCabinet" runat="server" CollapseControlID="chkCabinet"
                                        ExpandControlID="chkCabinet" SuppressPostBack="false" TargetControlID="pnlCabinet" Collapsed="True">
                                    </cc1:CollapsiblePanelExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Panel ID="pnlCabinet" runat="server" Width="100%">
                                        <table width="100%">
                                            <tr>
                                                <td align="right">
                                                    <asp:Button ID="btnSave" runat="server" CssClass="button" OnClick="btnSave_Click" Text="Save" />
                                                    <asp:Button ID="btnAddItem" runat="server" CssClass="button" OnClick="btnAddItem_Click" Text="Add New Item" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="grdCabinetSelectionSheet" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                                        PageSize="20" Width="100%" CssClass="mGrid" OnRowDataBound="grdCabinetSelectionSheet_RowDataBound">
                                                        <PagerSettings Position="TopAndBottom" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <table style="padding: 0px; margin: 0px; width: 100%">
                                                                        <tr>
                                                                            <td align="right" colspan="4">
                                                                                <asp:LinkButton ID="lnkDelete" Visible="false" OnClick="lnkDelete_Click" OnClientClick="return confirmDelete();" runat="server" Text="Delete"></asp:LinkButton></td>

                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtCabinetSheetName" runat="server" Style="width: 95%; margin-left: 0;" Text='<%# Eval("CabinetSheetName") %>'></asp:TextBox>
                                                                                <asp:Label ID="lblCabinetSheetName" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td style="font-weight: bold;">Upper Wall Cabinets</td>
                                                                            <td style="font-weight: bold;">Base Cabinets</td>
                                                                            <td style="font-weight: bold;">Misc Cabinets</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="font-weight: bold;">Door Style</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtUpperWallDoor" Style="width: 95%; margin-left: 0;" runat="server" Text='<%# Eval("UpperWallDoor") %>'></asp:TextBox>
                                                                                <asp:Label ID="lblUpperWallDoor" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtBaseDoor" Style="width: 95%; margin-left: 0;" runat="server" Text='<%# Eval("BaseDoor") %>'></asp:TextBox>
                                                                                <asp:Label ID="lblBaseDoor" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtMiscDoor" Style="width: 95%; margin-left: 0;" runat="server" Text='<%# Eval("MiscDoor") %>'></asp:TextBox>
                                                                                <asp:Label ID="lblMiscDoor" runat="server"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="font-weight: bold;">Wood Species</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtUpperWallWood" Style="width: 95%; margin-left: 0;" runat="server" Text='<%# Eval("UpperWallWood") %>'></asp:TextBox>
                                                                                <asp:Label ID="lblUpperWallWood" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtBaseWood" Style="width: 95%; margin-left: 0;" runat="server" Text='<%# Eval("BaseWood") %>'></asp:TextBox>
                                                                                <asp:Label ID="lblBaseWood" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtMiscWood" Style="width: 95%; margin-left: 0;" runat="server" Text='<%# Eval("MiscWood") %>'></asp:TextBox>
                                                                                <asp:Label ID="lblMiscWood" runat="server"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="font-weight: bold;">Stain Color</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtUpperWallStain" Style="width: 95%; margin-left: 0;" runat="server" Text='<%# Eval("UpperWallStain") %>'></asp:TextBox>
                                                                                <asp:Label ID="lblUpperWallStain" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtBaseStain" Style="width: 95%; margin-left: 0;" runat="server" Text='<%# Eval("BaseStain") %>'></asp:TextBox>
                                                                                <asp:Label ID="lblBaseStain" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtMiscStain" Style="width: 95%; margin-left: 0;" runat="server" Text='<%# Eval("MiscStain") %>'></asp:TextBox>
                                                                                <asp:Label ID="lblMiscStain" runat="server"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="font-weight: bold;">Exterior Sheen</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtUpperWallExterior" Style="width: 95%; margin-left: 0;" runat="server" Text='<%# Eval("UpperWallExterior") %>'></asp:TextBox>
                                                                                <asp:Label ID="lblUpperWallExterior" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtBaseExterior" Style="width: 95%; margin-left: 0;" runat="server" Text='<%# Eval("BaseExterior") %>'></asp:TextBox>
                                                                                <asp:Label ID="lblBaseExterior" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtMiscExterior" Style="width: 95%; margin-left: 0;" runat="server" Text='<%# Eval("MiscExterior") %>'></asp:TextBox>
                                                                                <asp:Label ID="lblMiscExterior" runat="server"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="font-weight: bold;">Interior Color</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtUpperWallInterior" Style="width: 95%; margin-left: 0;" runat="server" Text='<%# Eval("UpperWallInterior") %>'></asp:TextBox>
                                                                                <asp:Label ID="lblUpperWallInterior" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtBaseInterior" Style="width: 95%; margin-left: 0;" runat="server" Text='<%# Eval("BaseInterior") %>'></asp:TextBox>
                                                                                <asp:Label ID="lblBaseInterior" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtMiscInterior" Style="width: 95%; margin-left: 0;" runat="server" Text='<%# Eval("MiscInterior") %>'></asp:TextBox>
                                                                                <asp:Label ID="lblMiscInterior" runat="server"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="font-weight: bold;">Other</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtUpperWallOther" Style="width: 95%; margin-left: 0;" runat="server" Text='<%# Eval("UpperWallOther") %>'></asp:TextBox>
                                                                                <asp:Label ID="lblUpperWallOther" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtBaseOther" Style="width: 95%; margin-left: 0;" runat="server" Text='<%# Eval("BaseOther") %>'></asp:TextBox>
                                                                                <asp:Label ID="lblBaseOther" runat="server"></asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtMiscOther" Style="width: 95%; margin-left: 0;" runat="server" Text='<%# Eval("MiscOther") %>'></asp:TextBox>
                                                                                <asp:Label ID="lblMiscOther" runat="server"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Center" />
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>

                                                        </Columns>
                                                        <PagerStyle HorizontalAlign="Left" CssClass="pgr" />
                                                        <AlternatingRowStyle CssClass="alt" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                </tr>
                <tr>
                    <td>

                        <table cellpadding="0" cellspacing="2" width="100%">
                            <tr>
                                <td style="background-color: #172087; padding: 4px; color: #fff; text-align: left;" align="center">
                                    <asp:Panel ID="Panel1" runat="server">
                                        <b style="color: #fff;">
                                            <asp:CheckBox ID="chkKitchen" runat="server" Style="color: #fff;" Text="Kitchen Tile" OnCheckedChanged="chkSelection_CheckedChanged" AutoPostBack="true" /></b>
                                    </asp:Panel>
                                    <cc1:CollapsiblePanelExtender ID="cpnlExtKitchen" runat="server" CollapseControlID="chkKitchen"
                                        ExpandControlID="chkKitchen" SuppressPostBack="false" TargetControlID="pnlKitchen" Collapsed="True">
                                    </cc1:CollapsiblePanelExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Panel ID="pnlKitchen" runat="server" Width="100%">
                                        <table class="nGrid" cellpadding="0" cellspacing="2" width="98%">
                                            <tr>
                                                <td align="right">&nbsp;</td>
                                                <td class="nGridHeader" align="center">QTY</td>
                                                <td class="nGridHeader" align="center">UoM</td>
                                                <td class="nGridHeader" align="center">Style</td>
                                                <td class="nGridHeader" align="center">Color</td>
                                                <td class="nGridHeader" align="center">Size</td>
                                                <td class="nGridHeader" align="center">Vendor</td>
                                            </tr>
                                            <tr>
                                                <td style="border: 0;" align="right"><b>Backsplash Tile: </b></td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBacksplashQTY1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBacksplashMOU1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBacksplashStyle1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBacksplashColor1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBacksplashSize1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBacksplashVendor1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Backsplash Tile Pattern: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtBacksplashPattern1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Backsplash Grout Color: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtBacksplashGroutColor1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Backsplash Bullnose: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBBullnoseQTY1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBBullnoseMOU1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBBullnoseStyle1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBBullnoseColor1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBBullnoseSize1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>

                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBBullnoseVendor1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>

                                                </td>

                                            </tr>
                                            <tr>
                                                <td align="right">&nbsp;</td>
                                                <td class="nGridHeader" align="center"># Of Sticks</td>
                                                <td class="nGridHeader" align="center">Color</td>
                                                <td class="nGridHeader" align="center" colspan="2">Profile</td>
                                                <td class="nGridHeader" align="center" colspan="2">Thickness</td>
                                            </tr>
                                            <tr>
                                                <td align="right"><b>Schluter: </b></td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSchluterNOSticks1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSchluterColor1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:TextBox ID="txtSchluterProfile1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:TextBox ID="txtSchluterThickness1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">&nbsp;</td>
                                                <td class="nGridHeader" align="center">QTY</td>
                                                <td class="nGridHeader" align="center">UoM</td>
                                                <td class="nGridHeader" align="center">Style</td>
                                                <td class="nGridHeader" align="center">Color</td>
                                                <td class="nGridHeader" align="center">Size</td>
                                                <td class="nGridHeader" align="center">Vendor</td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Floor Tile: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorQTY1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorMOU1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorStyle1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorColor1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorSize1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorVendor1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Floor Tile Pattern: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtFloorPattern1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Floor Tile Direction: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtFloorDirection1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Baseboard Tile: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBaseboardQTY1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBaseboardMOU1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBaseboardStyle1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBaseboardColor1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBaseboardSize1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBaseboardVendor1" runat="server" CssClass="inputFieldText" TabIndex="1"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Floor Grout Color: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtFloorGroutColor1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>

                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="center" colspan="7">
                                                    <asp:Label ID="lblKitchen" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="7">
                                                    <asp:Button ID="btnSaveKitchenSheet" runat="server" Text="Save Kitchen Sheet" TabIndex="2" CssClass="button" OnClick="btnSaveKitchenSheet_Click" />
                                                    &nbsp;</td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
                <tr>
                    <td>


                        <table cellpadding="0" cellspacing="2" width="100%">
                            <tr>
                                <td style="background-color: #172087; padding: 4px; color: #fff; text-align: left;" align="center">
                                    <asp:Panel ID="Panel2" runat="server">
                                        <b style="color: #fff;">
                                            <asp:CheckBox ID="chkShower" runat="server" Style="color: #fff;" Text="Shower Tile" OnCheckedChanged="chkSelection_CheckedChanged" AutoPostBack="true" /></b>
                                    </asp:Panel>
                                    <cc1:CollapsiblePanelExtender ID="cpnlExtShower" runat="server" CollapseControlID="chkShower"
                                        ExpandControlID="chkShower" SuppressPostBack="false" TargetControlID="pnlShower" Collapsed="True">
                                    </cc1:CollapsiblePanelExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Panel ID="pnlShower" runat="server" Width="100%">
                                        <table class="nGrid" cellpadding="0" cellspacing="2" width="98%">
                                            <tr>
                                                <td align="right">&nbsp;</td>
                                                <td class="nGridHeader" align="center">QTY</td>
                                                <td class="nGridHeader" align="center">UoM</td>
                                                <td class="nGridHeader" align="center">Style</td>
                                                <td class="nGridHeader" align="center">Color</td>
                                                <td class="nGridHeader" align="center">Size</td>
                                                <td class="nGridHeader" align="center">Vendor</td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Wall Tile: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWallQTY1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWallMOU1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWallStyle1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWallColor1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWallSize1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWallVendor1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Wall Tile Pattern: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtWallPattern1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Wall Grout Color: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtWallGroutColor1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Shower Wall Bullnose: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSWBullnoseQTY1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSWBullnoseMOU1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSWBullnoseStyle1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSWBullnoseColor1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSWBullnoseSize1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSWBullnoseVendor1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td align="right">&nbsp;</td>
                                                <td class="nGridHeader" align="center"># Of Sticks</td>
                                                <td class="nGridHeader" align="center">Color</td>
                                                <td class="nGridHeader" align="center" colspan="2">Profile</td>
                                                <td class="nGridHeader" align="center" colspan="2">Thickness</td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Schluter: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSchluterNOSticks2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSchluterColor2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:TextBox ID="txtSchluterProfile2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:TextBox ID="txtSchluterThicknes2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">&nbsp;</td>
                                                <td class="nGridHeader" align="center">QTY</td>
                                                <td class="nGridHeader" align="center">UoM</td>
                                                <td class="nGridHeader" align="center">Style</td>
                                                <td class="nGridHeader" align="center">Color</td>
                                                <td class="nGridHeader" align="center">Size</td>
                                                <td class="nGridHeader" align="center">Vendor</td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Shower Pan: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerPanQTY1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerPanMOU1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerPanStyle1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerPanColor1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerPanSize1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerPanVendor1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>

                                            </tr>

                                            <tr>
                                                <td align="right">
                                                    <b>Shower Pan Grout Color: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtShowerPanGroutColor1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Decoband : </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDecobandQTY1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDecobandMOU1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>

                                                <td align="left">
                                                    <asp:TextBox ID="txtDecobandStyle1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>

                                                <td align="left">
                                                    <asp:TextBox ID="txtDecobandColor1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>

                                                <td align="left">
                                                    <asp:TextBox ID="txtDecobandSize1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>

                                                <td align="left">
                                                    <asp:TextBox ID="txtDecobandVendor1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="right">
                                                    <b>Decoband Height: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtDecobandHeight1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Niche Tile: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNicheTileQTY1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNicheTileMOU1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNicheTileStyle1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNicheTileColor1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNicheTileSize1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNicheTileVendor1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Niche Location: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtNicheLocation1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Niche Size: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtNicheSize1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Bench Tile: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBenchTileQTY1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBenchTileMOU1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBenchTileStyle1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBenchTileColor1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBenchTileSize1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBenchTileVendor1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Bench Location: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtBenchLocation1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Bench Size: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtBenchSize1" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Floor Tile: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorQTY2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorMOU2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorStyle2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorColor2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorSize2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorVendor2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Floor Tile Pattern: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtFloorPattern2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Floor Tile Direction: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtFloorDirection2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Baseboard Tile: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBaseboardQTY2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBaseboardMOU2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBaseboardStyle2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBaseboardColor2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBaseboardSize2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBaseboardVendor2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Floor Grout Color: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtFloorGroutColor2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Tile to: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtTileto2" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="center" colspan="7">
                                                    <asp:Label ID="lblShower" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="7">
                                                    <asp:Button ID="btnSaveShowerSheet" runat="server" CssClass="button" TabIndex="2" Text="Save Shower Sheet" OnClick="btnSaveShowerSheet_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="2" width="100%">
                            <tr>
                                <td style="background-color: #172087; padding: 4px; color: #fff; text-align: left;" align="center">
                                    <asp:Panel ID="Panel3" runat="server">
                                        <b style="color: #fff;">
                                            <asp:CheckBox ID="chkTub" runat="server" Style="color: #fff;" Text="Tub Tile" OnCheckedChanged="chkSelection_CheckedChanged" AutoPostBack="true" /></b>
                                    </asp:Panel>
                                    <cc1:CollapsiblePanelExtender ID="cpnlExtTub" runat="server" CollapseControlID="chkTub"
                                        ExpandControlID="chkTub" SuppressPostBack="false" TargetControlID="pnlTub" Collapsed="True">
                                    </cc1:CollapsiblePanelExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Panel ID="pnlTub" runat="server" Width="100%">
                                        <table class="nGrid" cellpadding="0" cellspacing="2" width="98%">
                                            <tr>
                                                <td align="right">&nbsp;</td>
                                                <td class="nGridHeader" align="center">QTY</td>
                                                <td class="nGridHeader" align="center">UoM</td>
                                                <td class="nGridHeader" align="center">Style</td>
                                                <td class="nGridHeader" align="center">Color</td>
                                                <td class="nGridHeader" align="center">Size</td>
                                                <td class="nGridHeader" align="center">Vendor</td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Wall Tile: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWallQTY3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWallMOU3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWallStyle3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWallColor3" runat="server" TabIndex="3" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWallSize3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWallVendor3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>


                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Wall Tile Pattern: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtWallPattern3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Wall Grout Color: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtWallGroutColor3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Wall Bullnose: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWBullnoseQTY3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWBullnoseMOU3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWBullnoseStyle3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWBullnoseColor3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWBullnoseSize3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWBullnoseVendor3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>


                                            </tr>
                                            <tr>
                                                <td align="right">&nbsp;</td>
                                                <td class="nGridHeader" align="center"># Of Sticks</td>
                                                <td class="nGridHeader" align="center">Color</td>
                                                <td class="nGridHeader" align="center" colspan="2">Profile</td>
                                                <td class="nGridHeader" align="center" colspan="2">Thickness</td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Schluter: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSchluterNOSticks3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSchluterColor3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:TextBox ID="txtSchluterProfile3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left" colspan="2">
                                                    <asp:TextBox ID="txtSchluterThicknes3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="right">&nbsp;</td>
                                                <td class="nGridHeader" align="center">QTY</td>
                                                <td class="nGridHeader" align="center">UoM</td>
                                                <td class="nGridHeader" align="center">Style</td>
                                                <td class="nGridHeader" align="center">Color</td>
                                                <td class="nGridHeader" align="center">Size</td>
                                                <td class="nGridHeader" align="center">Vendor</td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Decoband : </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDecobandQTY3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDecobandMOU3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDecobandStyle3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDecobandColor3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDecobandSize3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDecobandVendor3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>

                                            </tr>

                                            <tr>
                                                <td align="right">
                                                    <b>Decoband Height: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtDecobandHeight3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Niche Tile: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNicheTileQTY3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNicheTileMOU3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNicheTileStyle3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNicheTileColor3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNicheTileSize3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNicheTileVendor3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>



                                            </tr>

                                            <tr>
                                                <td align="right">
                                                    <b>Niche Location: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtNicheLocation3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Niche Size: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtNicheSize3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Shelf Location: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtShelfLocation3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="right">
                                                    <b>Floor Tile: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorQTY3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorMOU3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorStyle3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorColor3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorSize3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorVendor3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>


                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Floor Tile Pattern: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtFloorPattern3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Floor Tile Direction: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtFloorDirection3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Baseboard Tile: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBaseboardQTY3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBaseboardMOU3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBaseboardStyle3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBaseboardColor3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBaseboardSize3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBaseboardVendor3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Floor Grout Color: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtFloorGroutColor3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Tile to: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtTileto3" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="center" colspan="7">
                                                    <asp:Label ID="lblTub" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="7">
                                                    <asp:Button ID="btnSaveTubSheet" runat="server" CssClass="button" OnClick="btnSaveTubSheet_Click" TabIndex="2" Text="Save Tub Sheet" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>



                <tr>
                    <%--    BathRoom--%>
                    <td>
                        <table cellpadding="0" cellspacing="2" width="100%">
                            <tr>
                                <td style="background-color: #172087; padding: 4px; color: #fff; text-align: left;" align="center">
                                    <asp:Panel ID="Panel5" runat="server">
                                        <b style="color: #fff;">
                                            <asp:CheckBox ID="chkBathroom" runat="server" Style="color: #fff;" Text="Bath Room" OnCheckedChanged="chkSelection_CheckedChanged" AutoPostBack="true" /></b>
                                    </asp:Panel>
                                    <cc1:CollapsiblePanelExtender ID="cpnlExtBathroom" runat="server" CollapseControlID="chkBathroom"
                                        ExpandControlID="chkBathroom" SuppressPostBack="false" TargetControlID="pnlBathroom" Collapsed="True">
                                    </cc1:CollapsiblePanelExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Panel ID="pnlBathroom" runat="server" Width="100%">
                                        <table class="nGrid" cellpadding="0" cellspacing="2" width="98%">
                                            <tr>
                                                <td align="right" width="120px">&nbsp;</td>
                                                <td class="nGridHeader" align="center">QTY</td>
                                                <td class="nGridHeader" align="center">Style</td>
                                                <td class="nGridHeader" align="center">Where To Order</td>

                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Sink: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSinkQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSinkStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSinkOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Sink Faucet: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSinkFaucentQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSinkFaucentStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSinkFaucentOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Sink Drain: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSinkDrainQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSinkDrainStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSinkdrainOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    <b>Sink Valve: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSinkValveQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSinkValveStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSinkValveOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>


                                             <tr>
                                                <td align="right">
                                                    <b>Bathtub: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBathTubQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBathTubStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBathTubOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    <b>Tub Faucet: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTubFaucentQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTubFaucentStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTubFaucentOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    <b>Tub Valve: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTubValveQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTubValveStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTubValveOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    <b>Tub Drain: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTubDrainQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTubDrainStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTubDrainOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    <b>Toilet: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtToiletQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtToiletStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtToiletOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    <b>Shower/Tub System: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShower_TubSystemQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShower_TubSystemStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShower_TubSystemOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    <b>Shower Valve: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerValveQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerValveStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerValveOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    <b>Handheld Shower: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtHandheldShowerQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtHandheldShowerStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtHandheldShowerOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    <b>Body Spray: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBodySprayQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBodySprayStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBodySprayOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    <b>Body Spray Valve: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBodySprayValveQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBodySprayValveStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBodySprayValveOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    <b>Shower Drain: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerDrainQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerDrainStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerDrainOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    <b>Shower Drain Body & Plug: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerDrainBody_PlugQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerDrainBody_PlugStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerDrainBody_PlugOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    <b>Shower Drain Cover: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerDrainCoverQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerDrainCoverStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerDrainCoverOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    <b>Counter Top: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtCounterTopQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtCounterTopStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtCounterTopOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Additional places getting countertop: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtAdditionalplacesgettingcountertopQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtAdditionalplacesgettingcountertopStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtAdditionalplacesgettingcountertopOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Granite/Quartz Backsplash: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtGranite_Quartz_BacksplashQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtGranite_Quartz_BacksplashStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtGranite_Quartz_BacksplashOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Counter Top Edge: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtCounterTopEdgeQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtCounterTopEdgeStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtCounterTopEdgeOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                              <tr>
                                                <td align="right">
                                                    <b>Counter Top Overhang: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtCounterTop_OverhangQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtCounterTop_OverhangStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtCounterTop_OverhangOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Tub wall tile: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTubwalltileQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTubwalltileStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTubwalltileOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Wall Tile layout: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWallTilelayoutQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWallTilelayoutStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWallTilelayoutOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Tub skirt tile: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTubskirttileQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTubskirttileStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTubskirttileOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                              <tr>
                                                <td align="right">
                                                    <b>Shower Wall Tile: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerWallTileQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerWallTileStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerWallTileOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                              <tr>
                                                <td align="right">
                                                    <b>Wall Tile layout: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWall_Tile_layoutQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWall_Tile_layoutStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWall_Tile_layoutOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                              <tr>
                                                <td align="right">
                                                    <b>Shower Floor Tile: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerFloorTileQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerFloorTileStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerFloorTileOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                              <tr>
                                                <td align="right">
                                                    <b>Shower/Tub Tile Height: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerTubTileHeightQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerTubTileHeightStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtShowerTubTileHeightOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                              <tr>
                                                <td align="right">
                                                    <b>Floor Tile: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorTiletQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorTiletstyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorTiletOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                              <tr>
                                                <td align="right">
                                                    <b>Floor Tile layout : </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorTilelayoutQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorTilelayoutStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFloorTilelayoutOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                              <tr>
                                                <td align="right">
                                                    <b>Bullnose Tile: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBullnoseTileQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBullnoseTileStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtBullnoseTileOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Decoband: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDecobandQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDecobandStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDecobandOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Decoband Height: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDecobandHeightQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDecobandHeightStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDecobandHeightOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Tile Baseboard: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTileBaseboardQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTileBaseboardStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtTileBaseboardOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Grout Selection: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtGroutSelectionQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtGroutSelectionStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtGroutSelectionOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Niche Location: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNicheLocationQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNicheLocationStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNicheLocationOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Niche Size: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNicheSizeQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNicheSizeStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtNicheSizeOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Glass: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtGlassQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtGlassStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtGlassOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Window: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWindowQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWindowStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWindowOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Door: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDoorQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDoorStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtDoorOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    <b>Grab Bar: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtGrabBarQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtGrabBarStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtGrabBarOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Cabinet Door Style and Color: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtCabinetDoorStyleColorQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtCabinetDoorStyleColorStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtCabinetDoorStyleColorOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Medicine Cabinet: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtMedicineCabinetQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtMedicineCabinetStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtMedicineCabinetOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Mirror: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtMirrorQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtMirrorStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtMirrorOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Wood Baseboard: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWoodBaseboardQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWoodBaseboardStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtWoodBaseboardOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    <b>Paint Color: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtPaintColorQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtPaintColorStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtPaintColorOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Lighting: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtLightingQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtLightingStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtLightingOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    <b>Hardware: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtHardwareQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtHardwareStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtHardwareOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Special Notes: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtBathpecialNotes" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>





                                            <tr>
                                                <td align="center" colspan="7">
                                                    <asp:Label ID="lblBathroom" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="7">
                                                    <asp:Button ID="btnSaveBathroom" runat="server" CssClass="button" OnClick="btnSaveBathroom_Click" TabIndex="2" Text="Save Bath Room Sheet" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>


                   <tr>
                    <%--    Kitchen--%>
                    <td>
                        <table cellpadding="0" cellspacing="2" width="100%">
                            <tr>
                                <td style="background-color: #172087; padding: 4px; color: #fff; text-align: left;" align="center">
                                    <asp:Panel ID="Panel6" runat="server">
                                        <b style="color: #fff;">
                                            <asp:CheckBox ID="chkitchen2" runat="server" Style="color: #fff;" Text="Kitchen" OnCheckedChanged="chkSelection_CheckedChanged" AutoPostBack="true" /></b>
                                    </asp:Panel>
                                    <cc1:CollapsiblePanelExtender ID="cpnlExtkitchen2" runat="server" CollapseControlID="chkitchen2"
                                        ExpandControlID="chkitchen2" SuppressPostBack="false" TargetControlID="pnlKitchen2" Collapsed="True">
                                    </cc1:CollapsiblePanelExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Panel ID="pnlKitchen2" runat="server" Width="100%">
                                        <table class="nGrid" cellpadding="0" cellspacing="2" width="98%">
                                            <tr>
                                                <td align="right" width="120px">&nbsp;</td>
                                                <td class="nGridHeader" align="center">QTY</td>
                                                <td class="nGridHeader" align="center">Style</td>
                                                <td class="nGridHeader" align="center">Where To Order</td>

                                            </tr>
                                             <tr>
                                                <td align="right" >
                                                    <b>Sink: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenSinkQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenSinkStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenSinkOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Sink Faucet: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenSinkFaucetQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenSinkFaucetStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenSinkFaucetOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <b>Sink Drain: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenSinkDrainQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenSinkDrainStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenSinkDrainOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Counter Top: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenCounterTopQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenCounterTopStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenCounterTopOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                           

                                           

                                             <tr>
                                                <td align="right">
                                                    <b>Granite/Quartz Backsplash: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenGraniteQuartzBacksplashQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenGraniteQuartzBacksplashStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenGraniteQuartzBacksplashOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                          

                                             <tr>
                                                <td align="right">
                                                    <b>Counter Top Overhang: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenCounterTopOverhangQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenCounterTopOverhangStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenCounterTopOverhangOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            
                                             <tr>
                                                <td align="right">
                                                    <b>Additional places getting countertop: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenAdditionalplacesgettingcountertopQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenAdditionalplacesgettingcountertopStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenAdditionalplacesgettingcountertopOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            
                                             <tr>
                                                <td align="right">
                                                    <b>Counter Top Edge: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenCounterTopEdgeQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenCounterTopEdgeStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenCounterTopEdgeOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Cabinets: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenCabinetsQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenCabinetsStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenCabinetsOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Disposal: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenDisposalQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenDisposalStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenDisposalOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                            
                                             <tr>
                                                <td align="right">
                                                    <b>Baseboard: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenBaseboardQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenBaseboardStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenBaseboardOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    <b>Windows: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenWindowsQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenWindowsStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenWindowsOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Doors: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenDoorsQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenDoorsStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenDoorsOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                            
                                            
                                            
                                             <tr>
                                                <td align="right">
                                                    <b>Lighting: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenLightingQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenLightingStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenLightingOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>

                                             <tr>
                                                <td align="right">
                                                    <b>Hardware: </b>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenHardwareQty" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenHardwareStyle" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtKitchenHardwareOrder" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="right">
                                                    <b>Special Notes: </b>
                                                </td>
                                                <td align="left" colspan="6">
                                                    <asp:TextBox ID="txtKitchenSpecialNotes" runat="server" TabIndex="1" CssClass="inputFieldText"></asp:TextBox>
                                                </td>
                                            </tr>





                                            <tr>
                                                <td align="center" colspan="7">
                                                    <asp:Label ID="lblKitchen2" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="7">
                                                    <asp:Button ID="btnSaveKitchen" runat="server" CssClass="button" OnClick="btnSaveKitchen_Click" TabIndex="2" Text="Save Kitchen Sheet" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>


            </table>
            <asp:HiddenField ID="hdnCustomerId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnEstimateId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnCabinetSheetID" runat="server" Value="0" />
        </ContentTemplate>
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

