<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FileUpload_old.aspx.cs" Inherits="FileUpload_old" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<link type="text/css" href="css/core.css" rel="stylesheet" />	
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table cellpadding="0" cellspacing="2" width="100%">
                                    <tr>
                                        <td align="center">
                                            <table>
                            <tr>
                                <div id="div1" class="custom_file_upload" runat="server">
                                <input type="text" class="file" name="file_info">
                                <div class="file_upload">
                                <asp:FileUpload ID="FileUpload1" runat="server" 
                                         onchange="this.form.file_info.value=this.value;"/>
                                </div>
                                </div>
                                <td class="default2Label" align="left" valign="top">
                                    <asp:Label ID="lblOutput" runat="server"></asp:Label>
                            </td>
                                <td align="left"  valign="top">
                                    &nbsp;</td>
                            </tr>
                                                <tr>
                                                    <td align="center" colspan="2">
                                                        <asp:button id="btnUpload" CssClass="lblUpload" runat="server"  Text="Upload"
							onclick="btnUpload_Click" Visible="False"></asp:button>
                                                    </td>
                                                    <td align="center">
                                                        &nbsp;</td>
                                                </tr>
                                          </table>
                                        </td>
                                    </tr>
                                </table>
    
    </div>
    </form>
</body>
</html>