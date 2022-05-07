<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SigningReturn.aspx.cs" Inherits="SigningReturn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Download</title>
    <link type="text/css" href="css/core.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table cellpadding="0" cellspacing="4" width="100%" align="center">
        <tr>
            <td align="center">
                <table cellpadding="0" cellspacing="2" width="98%" align="center">
                    <tr>
                        <td align="left"
                        <!-- Logo -->
					    <div class="logo"> 
						    <a href="dashboard.aspx"><img src="assets/inner_page_logo.png" alt="Logo" /></a> 
					    </div>
					    <!-- End of Logo -->
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            <h3> <asp:Label ID="statusLabel" runat="server" Text="Label" ForeColor="Green"></asp:Label></h3>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="height: 10px">
                            </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="downloadPdf" runat="server" CssClass="si_re" 
                                OnClick="downloadPdf_Click" Text="Download PDF" />
                            <asp:Button ID="btnClose" runat="server" Text="Close" 
                                OnClientClick="javascript:window.close()" CssClass="si_re" OnClick="btnClose_Click" />
                        </td>
                    </tr>
                    
                </table>
            </td>
        </tr>
    </table>
    
    </div>
    </form>
   
</body>
</html>
