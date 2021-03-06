<%@ Page Title="Crew Details" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="crewdetails.aspx.cs" Inherits="crewdetails" %>
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
         
            text-align: center;
            text-shadow: 0 1px 1px rgba(255,255,255,0.75);
            vertical-align: middle;
            cursor: pointer;
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table id="Table5" align="center" width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="center" class="cssHeader">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td align="left"><span class="titleNu">
                                    <asp:Label ID="lblHeaderTitle" CssClass="cssTitleHeader" runat="server">Add New Crew</asp:Label></span></td>
                                <td align="right"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="Table2" width="98%" align="center" border="0" cellpadding="0" cellspacing="3" onclick="return TABLE1_onclick()">
                            <tr>
                                <td align="right">
                                    <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Red" Text="* required"></asp:Label>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="35%" align="right">
                                    <span class="required">* </span><b>First Name: </b>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtFirstName" runat="server" Width="200px" TabIndex="1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td width="35%" align="right">
                                    <span class="required">* </span><b>Last Name: </b>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtLastName" runat="server" Width="200px" TabIndex="1"></asp:TextBox>
                                </td>
                            </tr>
                           <%-- <tr>
                                <td align="right" valign="top">
                                    <b>Address: </b>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtAddress" runat="server" Height="40px"
                                        TabIndex="3" TextMode="MultiLine" Width="204px"></asp:TextBox></td>
                            </tr>--%>
                           <%-- <tr>
                                <td align="right">
                                    <b>City: </b>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtCity" runat="server" Width="200px"
                                        TabIndex="4"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <b>State: </b>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlState" runat="server" TabIndex="5" Width="212px">
                                    </asp:DropDownList></td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <b>Zip: </b>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtZip" runat="server" TabIndex="6" Width="200px"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <tr>
                                <td align="right">
                                    <b>Phone: </b>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtPhone" runat="server" Width="200px"
                                        TabIndex="7"></asp:TextBox></td>
                            </tr>
                            <%--<tr>
                                <td align="right">
                                    <b>Fax: </b>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtFax" runat="server" Width="200px"
                                        TabIndex="8"></asp:TextBox></td>
                            </tr>

                            <tr>
                                <td align="right">
                                    <b>Email: </b></td>
                                <td align="left">
                                    <asp:TextBox ID="txtEmailAddress" runat="server" TabIndex="9" Width="200px"></asp:TextBox>
                                </td>
                            </tr>--%>

                            <tr>
                                <td align="right"><span class="required">*</span>
                                    <b>User Name: </b>
                                </td>
                                <td align="left" colspan="2">
                                    <asp:TextBox ID="txtUsername" runat="server" TabIndex="10" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" width="35%">
                                    <asp:Label ID="lblPasswordRequ" runat="server" Font-Bold="True" Visible="true" ForeColor="Red" Text="*"></asp:Label>
                                    <b>Password: </b>
                                </td>
                                <td align="left" width="265px">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td><asp:TextBox ID="txtPassword" runat="server" TextMode="Password" TabIndex="11" Width="200px"></asp:TextBox> </td>
                                            <div style="position: absolute;margin-top: 5px;text-align: right;margin-left:190px;"><asp:ImageButton ID="imgPasswordShow" runat="server" CssClass="noEffectNew" ImageUrl="~/assets/customerstatemind/icon_hide.png" Width="20px" OnClick="imgPasswordShow_Click" />
                                    <asp:ImageButton ID="imgPasswordHide" runat="server" CssClass="noEffectNew" ImageUrl="~/assets/customerstatemind/icon_view.png" Width="20px" OnClick="imgPasswordHide_Click" Visible="false" /></div>
                                        </tr>
                                    </table>
                                </td>
                                <td align="left" style="height: 12px">
                                   
                                </td>
                            </tr>


                           <%-- <tr>
                                <td align="right">
                                    <asp:Label ID="lblReTypePasswordReq" runat="server" Visible="true" Font-Bold="True" ForeColor="Red" Text="*"></asp:Label>
                                    <b>Confirm Password:</b>
                                </td>
                                <td align="left" colspan="2">
                                    <asp:TextBox ID="txtConfirmPass" runat="server" TextMode="Password"
                                        TabIndex="12" Width="200px"></asp:TextBox>
                                </td>
                            </tr>--%>
                            <%--<tr>
                                <td align="right">
                                    <span class="required">*</span>
                                    <b>Password Verification Question: </b>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlQuestion" runat="server" TabIndex="13"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <span class="required">*</span>
                                    <b>Answer: </b>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtAnswer" runat="server" TabIndex="14" Width="200px"></asp:TextBox>
                                </td>
                            </tr>--%>

                            <%--                            <tr>
                                <td align="right"><b>Labor Rate/Hr: </b></td>
                                <td align="left">
                                    <asp:TextBox ID="txtLaborRate" runat="server" TabIndex="15" Width="200px"></asp:TextBox>
                                </td>
                            </tr>--%>

                            <tr>
                                <td align="right">
                                    <b>Status: </b></td>
                                <td align="left">
                                    <asp:CheckBox ID="chkIsActive" Width="200px" runat="server" Checked="True" TabIndex="16" Text="Active" />
                                </td>
                            </tr>

                            <tr>
                                <td align="right">&nbsp;</td>
                                <td align="left">
                                    <asp:Label ID="lblResult" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>

                              <tr>
                                <td colspan="4">
                                    <table width="100%">
                                        <tr>
                                            <td align="center" class="cssHeader">
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <td align="left"><span class="titleNu"><span id="spnPnlSection" class="cssTitleHeader">
                                                            <asp:ImageButton ID="ImageSectionMain" runat="server" ImageUrl="~/Images/expand.png" CssClass="blindInput" Style="margin: 0px; background: none; border: none; box-shadow: none; padding: 0 0 4px; vertical-align: middle;" TabIndex="40" />
                                                            <font style="font-size: 16px; cursor: pointer;">Sections</font></span></span>
                                                            <cc1:collapsiblepanelextender id="CollapsiblePanelExtender7" runat="server" collapsecontrolid="spnPnlSection" collapsed="True" collapsedimage="Images/expand.png" expandcontrolid="spnPnlSection" expandedimage="Images/collapse.png" imagecontrolid="ImageSectionMain" suppresspostback="true" targetcontrolid="pnlSection">
                                                </cc1:collapsiblepanelextender>
                                                        </td>
                                                        <td align="right"></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Panel ID="pnlSection" runat="server">
                                                    <table align="center" cellpadding="0" cellspacing="2" class="wrapper" width="100%">
                                                        <tr>
                                                            <td align="center">
                                                                <asp:CheckBoxList ID="chkSections" runat="server" RepeatColumns="4"
                                                                    Width="100%" TabIndex="2">
                                                                </asp:CheckBoxList>
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
                                <td>&nbsp;</td>
                                <td align="left">
                                    <asp:Button ID="btnSubmit" runat="server"
                                        TabIndex="17" Text="Submit" CssClass="button" OnClick="btnSubmit_Click"
                                        Width="80px" />
                                    &nbsp;<asp:Button ID="btnCancel" runat="server" TabIndex="18" Text="Cancel"
                                        CausesValidation="False" CssClass="button" OnClick="btnCancel_Click"
                                        Width="80px" />
                                    <asp:Label ID="lblLoadTime" runat="server" Text="" ForeColor="White"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hdnCrewId" runat="server" Value="0" />
                                    &nbsp;

                                </td>
                            </tr>
                            
                        </table>
                    </td>
                </tr>
            </table>
            

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="UpdatePanel1" DynamicLayout="False">
        <ProgressTemplate>
            <div class="overlay" />
            <div class="overlayContent">
                <p>
                    Please wait while your data is being processed
                </p>
                <img src="Images/ajax_loader.gif" alt="Loading" border="1" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

</asp:Content>


