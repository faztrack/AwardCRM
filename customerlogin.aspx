<%@ Page Title="Customer Login" Language="C#" MasterPageFile="~/customerlogin.master" AutoEventWireup="true" CodeFile="customerlogin.aspx.cs" Inherits="customerlogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <div style="position: relative; background-image: url('./assets/loginbg0.jpg'); background-repeat: no-repeat; background-position: center top; font-family: Tahoma, Arial, sans-serif; -webkit-background-size: 100%; -moz-background-size: 100%; -o-background-size: 100%; background-size: 100%;" id="loginTop50">
        <div>&nbsp;</div>
        <div style="margin: 0 auto; width: 500px; padding: 20px; border-radius: 10px; height: 440px;">
            <div id="header">
                <div>
                    <img width="260px" src="assets/client_login_logo.png" title="Cloud CEM" />
                </div>
            </div>
            <div style="text-align: center; font-size: 24px; font-weight: normal; color: #fff;">
                <p style="color:#fff;" class="main">Customer Portal</p>
            </div>
            <div id="box">
                <p class="main">
                    <label style="vertical-align: middle;">Username: </label>
                    <asp:TextBox ID="txtUserName" runat="server" input autofocus="autofocus" TabIndex="1"></asp:TextBox>
                </p>
                <p class="main">
                    <label style="vertical-align: middle;">Password:</label>
                    <asp:TextBox ID="txtPassword" runat="server" title="Enter Password" CssClass="tooltip" TabIndex="2" TextMode="Password"></asp:TextBox>
                </p>
                <p class="main" id="pReTypePassword" runat="server" visible="false">
                    <label style="vertical-align: middle;">Re-type Password:</label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" title="Re-type Password" CssClass="tooltip" TabIndex="2" TextMode="Password"></asp:TextBox>
                </p>
                <p class="space">
                    <div align="center">
                        <span>
                            <asp:Label ID="lblResult" runat="server"></asp:Label></span>
                    </div>
                    <div align="center">
                        <table cellpadding="0" cellspacing="0" align="center">
                            <tr>
                                <td align="center" valign="top">
                                    <asp:Button ID="btnLogIn" runat="server"
                                        Text="Login" TabIndex="3" CssClass="login" OnClick="btnLogIn_Click" Width="400px" /></td>
                            </tr>
                        </table>
                    </div>
                </p>
            </div>
        </div>
    </div>
    <div align="center" style="padding: 10px 0;">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td style="padding: 0 15px 0 0;">
                    
                </td>
                <td>
                    <!-- (c) 2005, 2019. Authorize.Net is a registered trademark of CyberSource Corporation -->
                    <div class="AuthorizeNetSeal">
                        <script type="text/javascript" language="javascript">var ANS_customer_id = "8562797d-a660-4f43-8ff5-f0582ad15e1f";</script>
                        <script type="text/javascript" language="javascript" src="//verify.authorize.net:443/anetseal/seal.js"></script>
                    </div>
                </td>
                <td style="padding: 0 0 0 15px;">
                    <a href="http://www.faztrack.com/" target="_blank">
                        <img alt="FazTrack" src="assets/cem_login_logo.png" title="FazTrack" />
                    </a></td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdnCustomerId" runat="server" Value="0" />
</asp:Content>

