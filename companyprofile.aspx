<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="companyprofile.aspx.cs" Inherits="companyprofile" Title="Company Profile" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript">
        function ValueChanged() {
            var textvalue;
            textvalue = document.getElementById('<%=hdnSID.ClientID %>').value;
            if (textvalue == '1') {
                return confirm('Warning: Changing markup will affect on all item price in section management and estimation template. Do you want to change markup?')
            }
        }
        //    function confirmOperation()
        //			{
        //				return confirm("Are you sure that you want to delete this schedule?");
        //			}     
    </script>
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

            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td align="center" class="cssHeader">
                        <table cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td align="left"><span class="titleNu">Company Profile</span></td>
                                <td align="right"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table cellpadding="4" cellspacing="4" width="790px" align="center">
                            <tr>
                                <td align="right">
                                    <b>Company Name: </b></td>
                                <td align="left">
                                    <asp:TextBox ID="txtCompanyName" runat="server" Width="200px" TabIndex="1"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <b>Address: </b></td>
                                <td align="left">
                                    <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine" Width="204px"
                                        TabIndex="2"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <b>City: </b></td>
                                <td align="left">
                                    <asp:TextBox ID="txtCity" runat="server" TabIndex="3" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <b>State: </b></td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlState" runat="server" TabIndex="4" Width="212px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <b>Zip Code: </b></td>
                                <td align="left">
                                    <asp:TextBox ID="txtZipCode" runat="server" Width="200px" TabIndex="5"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <b>Phone: </b></td>
                                <td align="left">
                                    <asp:TextBox ID="txtPhone" runat="server" TabIndex="6" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <b>Fax: </b></td>
                                <td align="left">
                                    <asp:TextBox ID="txtFax" runat="server" TabIndex="7" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <b>Email: </b></td>
                                <td align="left">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtEmail" runat="server" Width="200px" TabIndex="8"></asp:TextBox></td>
                                            <td>&nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                ControlToValidate="txtEmail" ErrorMessage="Invalid email address"
                                                ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red"></asp:RegularExpressionValidator></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <b>Email be printed on Contract (<font style="color: #2196f3; font-style: italic;">Payment Page</font>) : </b>
                                </td>
                                <td align="left">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtContractEmail" runat="server" Width="200px" TabIndex="9"></asp:TextBox></td>
                                            <td>&nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                                ControlToValidate="txtContractEmail" ErrorMessage="Invalid email address"
                                                ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red"></asp:RegularExpressionValidator></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top">
                                    <b>Transaction related Emails to be sent to: </b>
                                    <br />
                                    <asp:TextBox ID="txtDisplay" runat="server" BackColor="Transparent" CssClass="nostyle textRightAlign"
                                        BorderColor="Transparent" BorderStyle="None" BorderWidth="0px" Font-Bold="True"
                                        ReadOnly="True">
                                    </asp:TextBox>
                                </td>
                                <td align="left">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtCOEmail" MaxLength="1000" TextMode="MultiLine" runat="server" Width="200px" TabIndex="10" onkeydown="checkTextAreaMaxLengthWithDisplay(this,event,'1000',document.getElementById('head_txtDisplay'));"></asp:TextBox>
                                            </td>
                                            <td valign="top">(<font style="color: #2196f3; font-style: italic;">one or multiple emails separated by comma</font>)</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <b>Website: </b></td>
                                <td align="left">
                                    <asp:TextBox ID="txtWebsite" runat="server" TabIndex="11" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                             <tr>
                                <td align="right">
                                    <b>Finance(%):</b>
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtFinance" runat="server" TabIndex="12" Width="50px"></asp:TextBox>
                                     <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtFinance"
                                                                                FilterType="Custom" FilterMode="ValidChars" InvalidChars=" " ValidChars="1234567890.">
                                                                            </cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <b>Project Completion Duration:</b></td>
                                <td align="left">
                                    <asp:RadioButtonList ID="rdoCompletionType" runat="server"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Selected="True">Lead Time Based</asp:ListItem>
                                        <asp:ListItem Value="2">Start &amp; End Date Based</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                             <tr>
                                <td align="right">
                                    <b>Change Order Detail View:</b></td>
                                <td align="left">
                                    <asp:RadioButtonList ID="rdoChangeOrderQTY" runat="server"
                                        RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Selected="True">Without QTY</asp:ListItem>
                                        <asp:ListItem Value="2">With  QTY</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lblResult" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td align="left">
                                    <asp:Button ID="btnSave" runat="server" TabIndex="13" Text="Save" OnClick="btnSave_Click"
                                        Width="80px" CssClass="button" />
                                    &nbsp;<asp:Button ID="btnClose" TabIndex="14" runat="server" Text="Close"
                                        Width="80px" CausesValidation="False" OnClick="btnClose_Click"
                                        CssClass="button" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:HiddenField ID="hdnClientId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnSID" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnMarkup" runat="server" Value="0" />
                                    <asp:HiddenField ID="hdnCompanyProfileId" runat="server" Value="0" />
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
                <img src="images/ajax_loader.gif" alt="Loading" border="1" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>

