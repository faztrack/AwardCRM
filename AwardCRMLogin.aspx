﻿<%@ Page Language="C#" MasterPageFile="~/adminmasterlogin.master" AutoEventWireup="true" CodeFile="AwardCRMLogin.aspx.cs" Inherits="AwardCRMLogin" Title="Award CRM Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <div style="padding:0 0 20px 0;position: relative; background-image: url('./assets/loginbg0.jpg'); background-repeat: no-repeat; background-position: center top; font-family: Tahoma, Arial, sans-serif; -webkit-background-size: 100%; -moz-background-size: 100%; -o-background-size: 100%; background-size: 100%;" id="loginTop50">
        <div>&nbsp;</div>
        <div style="margin: 0 auto; background-color:transparent; width: 550px; border-radius: 10px; height: 520px;">
            <div id="header">
                <div>
                    <img width="220px" src="assets/client_login_logo.png" title="Login" alt="Award CRM" />
                </div>
            </div>
            <div id="box">
                <p class="main" style="text-align:center;">
                    <asp:Label Style="text-transform: uppercase; color:#0088cc; font-weight:normal; font-size:20px;" class="panel-title" ID="lblLoginPanelTitle" runat="server"></asp:Label>
                </p>
                <p class="main">
                    <label style="vertical-align: middle;">Username: </label>
                    <asp:TextBox ID="txtUserName" runat="server" autofocus="autofocus" TabIndex="1"></asp:TextBox>
                </p>
                <p class="main">
                    <label style="vertical-align: middle;">Password:</label>
                    <asp:TextBox ID="txtPassword" runat="server" title="Enter Password" CssClass="tooltip" TabIndex="2" TextMode="Password"></asp:TextBox>
                </p>
                <p class="space">
                    <div align="center">
                        <span>
                            <asp:Label ID="lblResult" runat="server" CssClass="newSpan"></asp:Label></span>
                    </div>
                    <div align="center">
                        <table cellpadding="0" cellspacing="0" align="center">
                           <tr>
                                <td  colspan="2" align="center" valign="top">
                                    <asp:Button ID="btnLogIn" runat="server"
                                        Text="Login" TabIndex="3" CssClass="login" OnClick="btnLogIn_Click" Width="400px" /></td>
                            </tr>
                            <tr><td colspan="2">&nbsp;</td></tr>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnkForgotPassword" runat="server" CssClass="black" TabIndex="4" OnClick="lnkForgotPassword_Click">Forgot Password?</asp:LinkButton>
                                </td>
                                 <td align="right">
                                    <asp:HyperLink  ID="lnkMobileSiteLogin" runat="server" CssClass="black" Target="_blank" TabIndex="4" style="padding-right: 10px;">Mobile Site Login</asp:HyperLink>
                                </td>
                            </tr>
                        </table>
                    </div>
                </p>                
            </div>
        </div>
    </div>
    <div align="center" style="padding: 10px 0;">
        <a href="http://www.faztrack.com/" target="_blank">
            <img alt="FazTrack" src="assets/faztrack-logo.png" title="FazTrack" />
        </a>
    </div>
</asp:Content>

