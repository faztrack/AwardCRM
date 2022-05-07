<%@ Page Title="Selection Log" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="selectionlog.aspx.cs" Inherits="selectionlog" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td class="cssHeader" align="center">
                <table cellpadding="0" cellspacing="0" width="100%" align="center">
                    <tr>
                        <td align="center" class="cssHeader">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left">
                                        <span class="titleNu">
                                            <asp:Label ID="lblHeaderTitle" runat="server" CssClass="cssTitleHeader">Selection Log</asp:Label></span><asp:Label runat="server" CssClass="titleNu" ID="lblTitelJobNumber"></asp:Label>
                                    </td>
                                   
                                </tr>
                            </table>
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table width="100%">
                    <tr>
                        <td>
                        
                           <table class="wrapper" cellpadding="0" cellspacing="0" width="100%">
                                  

                                    <tr>
                                        <td align="center">
                                             <asp:GridView ID="grdSelection" runat="server" AutoGenerateColumns="False"
                                                CssClass="mGrid"
                                                PageSize="200" TabIndex="2" Width="100%" OnRowDataBound="grdSelection_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDate" runat="server" Text='<%# Eval("CreateDate","{0:d}")%>' />
                                                          
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" Width="120px" />
                                                        <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Section">
                                                        <ItemTemplate>
                                                           
                                                            <asp:Label ID="lblSectiong" runat="server" Text='<%# Eval("section_name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Location">
                                                        <ItemTemplate>
                                                           
                                                            <asp:Label ID="lblLocation" runat="server" Text='<%# Eval("location_name") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Title">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Title") %>' />
                                                            
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Notes">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNotes" runat="server" Text='<%# Eval("Notes") %>' Style="display: inline;" />
                                                            

                                                            <pre style="height: auto; white-space: pre-wrap; display: inline; font-family: 'Open Sans', Arial, Tahoma, Verdana, sans-serif;"><asp:Label ID="lblNotes_r" runat="server" Text='<%# Eval("Notes") %>' Visible="false" ></asp:Label></pre>
                                                            <asp:LinkButton ID="lnkOpen" Style="display: inline;" Text="More" Font-Bold="true" ToolTip="Click here to view more" OnClick="lnkOpen_Click" runat="server" ForeColor="Blue"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="15%" HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Price">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPrice" runat="server" Text='<%# Eval("Price","{0:c}") %>' />
                                                            
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle Width="5%" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Valid Till">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblVDate" runat="server" Text='<%# Eval("ValidTillDate","{0:d}")%>' />
                                                           
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" Width="120px" />
                                                        <ItemStyle HorizontalAlign="Center" Width="120px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="">
                                                        <ItemTemplate>
                                                            <table style="padding: 0px; margin: 0px; border: none;">
                                                                <tr style="padding: 0px; margin: 0px; border: none;">
                                                                    <td style="padding: 0px; margin: 0px; border: none;">

                                                                        <asp:GridView ID="grdUploadedFileList" runat="server" AutoGenerateColumns="False"
                                                                            CssClass="uGrid" ShowHeader="false" ShowFooter="false" BorderStyle="None" Style="padding: 0px; margin: 0px; border: none;"
                                                                            OnRowDataBound="grdUploadedFileList_RowDataBound">
                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <div class="clearfix">
                                                                                            <div class="divImageCss">
                                                                                                <asp:HyperLink ID="hypImg" runat="server" CssClass="hypimgCss" data-imagelightbox="g" data-ilb2-caption="" Visible="false" ToolTip="Click here to view file">
                                                                                                    <asp:Image ID="img" onerror="this.src='Images/No_image_available.jpg';" runat="server" CssClass="imgCss blindInput" />
                                                                                                </asp:HyperLink>
                                                                                                <asp:HyperLink ID="hypPDF" runat="server" CssClass="hypimgCss" data-imagelightbox="g" data-ilb2-caption="" Visible="false" ToolTip="Click here to view file">
                                                                                                    <asp:Image ID="imgPDF" onerror="this.src='Images/No_image_available.jpg';" runat="server" CssClass="imgCss blindInput" />

                                                                                                </asp:HyperLink>
                                                                                                <asp:HyperLink ID="hypExcel" runat="server" CssClass="hypimgCss" data-imagelightbox="g" data-ilb2-caption="" Visible="false" ToolTip="Click here to view file">
                                                                                                    <asp:Image ID="imgExcel" onerror="this.src='Images/No_image_available.jpg';" runat="server" CssClass="imgCss blindInput" />

                                                                                                </asp:HyperLink>
                                                                                                <asp:HyperLink ID="hypDoc" runat="server" CssClass="hypimgCss" data-imagelightbox="g" data-ilb2-caption="" Visible="false" ToolTip="Click here to view file">
                                                                                                    <asp:Image ID="imgDoc" onerror="this.src='Images/No_image_available.jpg';" runat="server" CssClass="imgCss blindInput" />

                                                                                                </asp:HyperLink>
                                                                                                <asp:HyperLink ID="hypTXT" runat="server" CssClass="hypimgCss" data-imagelightbox="g" data-ilb2-caption="" Visible="false" ToolTip="Click here to view file">
                                                                                                    <asp:Image ID="imgTXT" onerror="this.src='Images/No_image_available.jpg';" runat="server" CssClass="imgCss blindInput" />

                                                                                                </asp:HyperLink>

                                                                                            </div>
                                                                                            <div style="text-align: center; padding: 5px; font-weight: bold;">
                                                                                                <asp:Label ID="lblFileName" runat="server" Text=""></asp:Label>
                                                                                               
                                                                                            </div>
                                                                                        </div>

                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>

                                                                            </Columns>
                                                                        </asp:GridView>

                                                                    </td>
                                                                </tr>
                                                               
                                                            </table>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="28%" />
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Deleted By">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDeletedBy" runat="server" Text='<%# Eval("DeleteBy")%>' />
                                                           
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" Width="150px" />
                                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Deleted Date">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDeletedDate" runat="server" Text='<%# Eval("DeletedDate","{0:d}")%>' />
                                                           
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" Width="150px" />
                                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                                    </asp:TemplateField>


                                                    
                                                </Columns>
                                                <AlternatingRowStyle CssClass="alt" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="hdnCustomerID" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdnEstimateID" runat="server" Value="0" />
                                            <asp:HiddenField ID="hdnSiteReviewId" runat="server" Value="0" />
                                        </td>
                                    </tr>

                                </table>
                       
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
     
    </table>

    <div id="LoadingProgress" style="display: none">
        <div class="overlay" />
        <div class="overlayContent">
            <p>
                Please wait while your data is being processed
            </p>
            <img src="images/ajax_loader.gif" alt="Loading" border="1" />
        </div>
    </div>
</asp:Content>


