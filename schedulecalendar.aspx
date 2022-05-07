<%@ Page Language="C#" MasterPageFile="~/schedulemaster.master" AutoEventWireup="true"
    CodeFile="schedulecalendar.aspx.cs" Inherits="schedulecalendar" Title="Schedule Calendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .ui-autocomplete-loading {
            background: white url("images/ui-anim_basic_16x16.gif") right center no-repeat;
        }
    </style>
    <table cellpadding="0" cellspacing="0" width="100%" align="center">
        <tr>
            <td align="center">
                <table cellpadding="0" cellspacing="0" width="100%" style="padding: 0px; margin: 0px;">
                    <tr>
                        <td align="center" class="cssHeader" style="padding: 0px !important;">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left"><span class="titleNu">
                                        <asp:Label ID="lbltopHead" CssClass="cssTitleHeader cssTitleHeaderCal" runat="server" Text="Schedule Calendar"></asp:Label></span></td>

                                    <td align="right">
                                        <table cellpadding="0" cellspacing="0" style="padding: 0px; margin: 0px;">
                                            <tr>
                                                <td align="left" valign="middle">
                                                    <asp:LinkButton ID="btnBack" Font-Bold="true" ForeColor="#555555" runat="server"
                                                        Text="Return to Customer list" Visible="true" OnClick="btnBack_Click"></asp:LinkButton>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td align="right" style="padding-right: 30px;">
                                                    <asp:ImageButton ID="btnSalesCalendar" ImageUrl="~/images/calendar_money.png" runat="server" CssClass="imageBtn" OnClick="btnSalesCalendar_Click" ToolTip="Go to Sales Calendar" />

                                                    <asp:HyperLink ID="btnOperationCalendar" ToolTip="Open Operation Calendar (Read-only)" Target="_blank" CssClass="opreadonly"
                                                        runat="server" ImageUrl="~/images/hard_hat_calendar_read_only.png" NavigateUrl="~/schedulecalendarreadonly.aspx?TypeID=1" />
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
        <tr>
            <td align="center">
                <table style="width: 100%; padding: 0px; margin: 0px;">
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td valign="top" align="right" style="padding: 0px; margin: 0px; width: 15%;">
                            <table style="padding: 0px; margin-top: -6px;">
                                <tr>
                                    <td align="left">
                                        <span class="fc-UnassignedSpan">
                                            <input type="checkbox" name="Unassigned" id="chkUnassigned" class="fc-UnassignedChk" value="1" />
                                            Unassigned (flashing events)
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>



                                            <td valign="top" runat="server" id="trCalStateAction">
                                                <div class="">
                                                    <input type="text" id="miniCalendar" style="display: none;" />&nbsp;&nbsp;
                                                    <asp:Label ID="lblCalState" Style="font-weight: bold; border-radius: 5px; padding: 5px;" runat="server"></asp:Label>
                                                    <asp:Button ID="btnCalStateAction" runat="server" CssClass="CssCalStateActionBtn" Text="Go Online" OnClick="btnCalStateAction_Click" OnClientClick="return CalStateAction();" />
                                                </div>

                                            </td>
                                            <asp:HiddenField ID="hdnCalStateAction" runat="server" Value="" />
                                            <asp:HiddenField ID="hdnEventLinkCount" runat="server" Value="0" />

                                            <asp:Button ID="btnHdnSendCalEmail" runat="server" OnClick="btnHdnSendCalEmail_Click" Style="display: none;" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:Button ID="btnHdn" runat="server" OnClick="btnHdn_Click" Style="display: none;" />
                                </tr>
                                <tr id="trSearchCal" runat="server">
                                    <td align="left">
                                        <table style="padding: 0px; margin: 0px;" id="tblSearch" runat="server">
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblSearchLastName" runat="server">Last Name:&nbsp;</asp:Label></td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSearch" CssClass="acSearch" runat="server" onkeypress="return searchKeyPress(event);"></asp:TextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblSearchSection" runat="server">Section:&nbsp;</asp:Label></td>
                                                <td>
                                                    <asp:TextBox ID="txtSection" CssClass="acSearch" runat="server" onkeypress="return searchKeyPress(event);"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="lblSearchAssignedTo" runat="server">Assigned To:&nbsp;</asp:Label></td>
                                                <td>
                                                    <asp:TextBox ID="txtUser" CssClass="acSearch" runat="server" onkeypress="return searchKeyPress(event);"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">Superintendent:&nbsp;</td>
                                                <td>
                                                    <asp:TextBox ID="txtSuperintendent" CssClass="acSearch" runat="server" onkeypress="return searchKeyPress(event);"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right"></td>
                                                <td align="left">
                                                    <asp:Button ID="btnSearch" runat="server" CssClass="btnSearch" Text="Search" OnClientClick="return false;" />&nbsp;
                                                    <asp:LinkButton ID="lnkViewAll" runat="server" Text="View All" OnClientClick="return false;" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <a id="linkCalendarProjectLink" href="schedulecalendar.aspx?TypeID=1" title="Go to Schedule"></a>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%-- <asp:Label ID="lblStartOfJob" Font-Bold="true" runat="server"></asp:Label>--%>
                                        <table style="padding: 0px; margin: 0px;" id="tblProjectStartDate" runat="server" visible="false">
                                            <tr>
                                                <td style="padding: 0px; margin: 0px; font-weight: bold;" align="right" valign="middle">Project Start Date: </td>
                                                <td style="padding: 0px; margin: 0px;">
                                                    <asp:TextBox ID="txtProjectStartDate" runat="server" CssClass="acSearch" Width="90px"></asp:TextBox></td>

                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td style="padding: 0px; margin: 0px;">
                                                    <asp:Button ID="btnSaveProjectStartDate" runat="server" CssClass="btnSearch" Text="Save" OnClick="btnSaveProjectStartDate_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                        <%-- <div class="CssCalStateActionDiv">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblCalState" Style="font-weight: bold; border-radius: 5px; padding: 5px;" runat="server"></asp:Label>
                                                 
                                                    <asp:Button ID="btnCalStateAction" CssClass="CssCalStateActionBtn" runat="server" Text="Go Online" OnClick="btnCalStateAction_Click" OnClientClick="return confirm('Are You Sure?');" />
                                                    <asp:HiddenField ID="hdnCalStateAction" runat="server" Value="" />
                                                    <asp:Button ID="btnHdn" runat="server" OnClick="btnHdn_Click" Style="display: none;" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>--%>
                                        <%--<div id="miniCalendar"></div>--%>
                                       
                                    </td>
                                </tr>
                                <tr id="trProjects" runat="server" visible="false">
                                    <td style="padding-top: 8px; padding-bottom: 0px; margin-bottom: 5px;">
                                        <%--   <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>--%>
                                        <font style="font-weight: bold;">Projects:</font>
                                        <asp:CheckBoxList ID="chkEst" runat="server" AutoPostBack="True" OnSelectedIndexChanged="chkEst_SelectedIndexChanged"></asp:CheckBoxList>
                                        <%--<asp:DropDownList ID="ddlEst" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlEst_SelectedIndexChanged" Style="max-width: 245px;">
                                                </asp:DropDownList>--%>
                                        <%--</ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                    </td>
                                </tr>
                                <tr id="trSectionsList" runat="server" visible="false">
                                    <td>
                                        <table style="padding: 0px; margin: 0px; width: 100%">
                                            <tr>
                                                <td>
                                                    <p style="padding-top: 8px; padding-bottom: 0px; margin-bottom: 5px; color: #000000;">
                                                        <strong>Template Sections (Read-only)</strong>
                                                    </p>
                                                </td>
                                            </tr>
                                            <tr style="padding: 0px; margin: 0px;">
                                                <td style="padding: 0px; margin: 0px;">
                                                    <asp:CheckBoxList ID="chkTemplateMainSection" CssClass="csscalMainSectionList" runat="server" AutoPostBack="true" OnSelectedIndexChanged="chkTemplateMainSection_SelectedIndexChanged"></asp:CheckBoxList>
                                                </td>
                                            </tr>
                                            <tr style="padding: 0px; margin: 0px;">
                                                <td style="padding: 0px; margin: 0px;">
                                                    <div id='external-events'>
                                                        <asp:GridView ID="grdDragTemplateSection" runat="server" AutoGenerateColumns="False" Width="100%" ShowHeader="false" CssClass="cssgrdTemplateSectionList" AllowSorting="false"
                                                            OnRowDataBound="grdDragTemplateSection_RowDataBound">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" ShowHeader="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSection" CssClass="fc-event fc-default" runat="server" Text='<%# Eval("section_name")%>' />
                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="100%" HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>

                                                        <asp:GridView ID="grdEstimatesPT" runat="server" AutoGenerateColumns="false" CssClass="mGrid" Width="100%" OnRowDataBound="grdEstimatesPT_RowDataBound" ShowHeader="false">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="" ShowHeader="false">
                                                                    <ItemTemplate>
                                                                        <table style="padding: 0px; margin: 0px; Width: 100%;">
                                                                            <tr style="padding: 0px; margin: 0px; background-color: #EEEEEE;">
                                                                                <td style="padding: 5px 5px 5px 10px; margin: 0px; font-weight: bold;">
                                                                                    <asp:Label ID="lblEstimateName" runat="server" Text='<%# Eval("estimate_name")%>'></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="padding: 0px; margin: 0px;">
                                                                                <td style="padding: 0px; margin: 0px;">
                                                                                    <asp:GridView ID="grdDragPaymentTerms" runat="server" AutoGenerateColumns="False" Width="100%" ShowHeader="false" CssClass="cssgrdTemplateSectionList" AllowSorting="false"
                                                                                        OnRowDataBound="grdDragPaymentTerms_RowDataBound">
                                                                                        <Columns>
                                                                                            <asp:TemplateField HeaderText="" ShowHeader="false">
                                                                                                <ItemTemplate>
                                                                                                    <asp:Label ID="lblSection" CssClass="fc-event fc-default" runat="server" Text='<%# Eval("section_name")%>' />
                                                                                                    <asp:HiddenField ID="customer_id" runat="server" Value='<%#Eval("customer_id") %>' />
                                                                                                    <asp:HiddenField ID="estimate_id" runat="server" Value='<%#Eval("estimate_id") %>' />
                                                                                                    <asp:HiddenField ID="cssClassName" runat="server" Value='<%#Eval("cssClassName") %>' />
                                                                                                </ItemTemplate>
                                                                                                <ItemStyle Width="100%" HorizontalAlign="Left" />
                                                                                            </asp:TemplateField>
                                                                                        </Columns>

                                                                                    </asp:GridView>
                                                                                </td>
                                                                            </tr>
                                                                        </table>

                                                                    </ItemTemplate>
                                                                    <ItemStyle Width="100%" HorizontalAlign="Left" />
                                                                    <HeaderStyle Width="100%" HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                            </Columns>

                                                        </asp:GridView>


                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td></td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="center" style="padding: 0px; margin: 0px; width: 60%;">
                            <div id="calendar">
                            </div>
                        </td>
                        <td valign="top" align="left" style="padding: 0px; margin: 0px; width: 20%;">
                            <table id="tblSectionDragDrop" runat="server" visible="false">
                                <tr>
                                    <td>
                                        <p style="color: #000; font-weight: bold;">Project Sections (Drag & Drop)</p>
                                    </td>
                                    <td>
                                        <p>
                                            <asp:HyperLink ID="hypSOW" Style="color: #000; font-weight: bold;" ToolTip="Open composite workoerder (Read-only)" Target="_blank" Text="SOW" runat="server" />
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <div id='estimate-sections'>
                                            <asp:GridView ID="grdEstimates" runat="server" AutoGenerateColumns="false" CssClass="mGrid" Width="100%" OnRowDataBound="grdEstimates_RowDataBound" ShowHeader="false">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" ShowHeader="false">
                                                        <ItemTemplate>
                                                            <table style="padding: 0px; margin: 0px; Width: 100%;">
                                                                <tr style="padding: 0px; margin: 0px; background-color: #EEEEEE;">
                                                                    <td style="padding: 5px 5px 5px 10px; margin: 0px; font-weight: bold;">
                                                                        <asp:Label ID="lblEstimateName" runat="server" Text='<%# Eval("estimate_name")%>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr style="padding: 0px; margin: 0px;">
                                                                    <td style="padding: 0px; margin: 0px;">
                                                                        <asp:GridView ID="grdProjectSection" runat="server" AutoGenerateColumns="False" Width="100%" ShowHeader="false" CssClass="cssgrdProjectSectionList"
                                                                            OnRowDataBound="grdProjectSection_RowDataBound">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="" ShowHeader="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblSection" CssClass="fc-event " runat="server" Text='<%# Eval("section_name")%>' />
                                                                                        <asp:HiddenField ID="customer_id" runat="server" Value='<%#Eval("customer_id") %>' />
                                                                                        <asp:HiddenField ID="estimate_id" runat="server" Value='<%#Eval("estimate_id") %>' />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle Width="100%" HorizontalAlign="Left" />
                                                                                    <HeaderStyle Width="100%" HorizontalAlign="Left" />
                                                                                </asp:TemplateField>
                                                                            </Columns>

                                                                        </asp:GridView>
                                                                    </td>
                                                                </tr>
                                                            </table>

                                                        </ItemTemplate>
                                                        <ItemStyle Width="100%" HorizontalAlign="Left" />
                                                        <HeaderStyle Width="100%" HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>

                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                            </table>


                        </td>
                    </tr>
                </table>

            </td>
        </tr>
        <tr>
            <%-- <td align="center">
                <table style="width: 100%; padding: 0px; margin: 0px;">
                    
                </table>

            </td>--%>
        </tr>
    </table>
    <div id="updatedialog" class="popWindow popWindowUpdate" title="Update Event">
        <p style="margin-bottom: 0px; float: left; color: #2779aa; display: none;">( TYPE <font style="font-size: 24px; font-weight: bold; position: relative; bottom: -8px;">*</font> TO VIEW ALL )</p>

        <table cellpadding="0" style="padding: 5px; width: 97%;">
            <tr>
                <td class="alignRight" valign="top">Section/Title:
                    <br />
                    <font style="color: #6e5555;">(type * to show all items)</font>
                </td>
                <td class="alignLeft" valign="top">
                    <textarea id="eventName" class="scTextArea" cols="40" rows="3" onchange="txtChange(this.id)" title=" Type * to view all "></textarea>
                </td>
                <td class="alignLeft" valign="bottom">
                    <label id="lbleventName" class="hidden">
                        Required</label>
                </td>
            </tr>
            <tr>
                <td class="alignRight">Location:
                     <br />
                    <font style="color: #6e5555;">(type * to show all items)</font>
                </td>
                <td class="alignLeft">
                    <input id="eventLocation" type="text" onchange="txtLocationChange(this.id)" style="width: 226px; overflow: scroll;" title=" Type * to view all " />
                </td>
                <td class="alignLeft">
                    <label id="lbleventLocation" class="hidden" style="padding-bottom: 0px;">
                        Required</label>
                </td>
            </tr>
            <tr>
                <td class="alignRight">Assigned To:
                     <br />
                    <font style="color: #6e5555;">(type * to show all items)</font>
                </td>
                <td class="alignLeft">
                    <input id="eventSalesPerson" type="text" onchange="txtChange(this.id)" style="width: 226px; overflow: scroll;" title=" Type * to view all " />
                </td>
                <td class="alignLeft">
                    <label id="lbleventSalesPerson" class="hidden" style="padding-bottom: 0px;">
                        Required</label>
                </td>
            </tr>
            <tr>
                <td class="alignRight">Start:
                </td>
                <td class="alignLeft">
                    <input type="text" id="eventStart" onchange="setEventStartTime('Date')" />
                    <select name="eventStartTime" id="eventStartTime" onchange="setEventStartTime('Time')">
                        <option value="5:00 AM">5:00 AM</option>
                        <option value="5:00 AM" selected="selected">5:00 AM</option>
                        <option value="5:30 AM">5:30 AM</option>
                        <option value="6:00 AM">6:00 AM</option>
                        <option value="6:30 AM">6:30 AM</option>
                        <option value="7:00 AM">7:00 AM</option>
                        <option value="7:30 AM">7:30 AM</option>
                        <option value="8:00 AM">8:00 AM</option>
                        <option value="8:30 AM">8:30 AM</option>
                        <option value="9:00 AM">9:00 AM</option>
                        <option value="9:30 AM">9:30 AM</option>
                        <option value="10:00 AM">10:00 AM</option>
                        <option value="10:30 AM">10:30 AM</option>
                        <option value="11:00 AM">11:00 AM</option>
                        <option value="11:30 AM">11:30 AM</option>
                        <option value="12:00 PM">12:00 PM</option>
                        <option value="12:30 PM">12:30 PM</option>
                        <option value="1:00 PM">1:00 PM</option>
                        <option value="1:30 PM">1:30 PM</option>
                        <option value="2:00 PM">2:00 PM</option>
                        <option value="2:30 PM">2:30 PM</option>
                        <option value="3:00 PM">3:00 PM</option>
                        <option value="3:30 PM">3:30 PM</option>
                        <option value="4:00 PM">4:00 PM</option>
                        <option value="4:30 PM">4:30 PM</option>
                        <option value="5:00 PM">5:00 PM</option>
                        <option value="5:30 PM">5:30 PM</option>
                        <option value="6:00 PM">6:00 PM</option>
                        <option value="6:30 PM">6:30 PM</option>
                        <option value="7:00 PM">7:00 PM</option>
                        <option value="7:30 PM">7:30 PM</option>
                        <option value="8:00 PM">8:00 PM</option>
                        <option value="8:30 PM">8:30 PM</option>
                        <option value="9:00 PM">9:00 PM</option>
                        <option value="9:30 PM">9:30 PM</option>
                        <option value="10:00 PM">10:00 PM</option>
                        <option value="10:30 PM">10:30 PM</option>
                        <option value="11:00 PM">11:00 PM</option>
                        <option value="11:30 PM">11:30 PM</option>
                    </select>
                </td>
                <td></td>
            </tr>
            <tr>
                <td class="alignRight">End:
                </td>
                <td class="alignLeft">
                    <input type="text" id="eventEnd" onchange="setEventEndTime('Date')" />
                    <select name="eventEndTime" id="eventEndTime" onchange="setEventEndTime('Time')" style="display: none;">
                        <option value="5:00 AM">5:00 AM</option>
                        <option value="5:30 AM">5:30 AM</option>
                        <option value="6:00 AM">6:00 AM</option>
                        <option value="6:00 AM" selected="selected">6:00 AM</option>
                        <option value="6:30 AM">6:30 AM</option>
                        <option value="7:00 AM">7:00 AM</option>
                        <option value="7:30 AM">7:30 AM</option>
                        <option value="8:00 AM">8:00 AM</option>
                        <option value="8:30 AM">8:30 AM</option>
                        <option value="9:00 AM">9:00 AM</option>
                        <option value="9:30 AM">9:30 AM</option>
                        <option value="10:00 AM">10:00 AM</option>
                        <option value="10:30 AM">10:30 AM</option>
                        <option value="11:00 AM">11:00 AM</option>
                        <option value="11:30 AM">11:30 AM</option>
                        <option value="12:00 PM">12:00 PM</option>
                        <option value="12:30 PM">12:30 PM</option>
                        <option value="1:00 PM">1:00 PM</option>
                        <option value="1:30 PM">1:30 PM</option>
                        <option value="2:00 PM">2:00 PM</option>
                        <option value="2:30 PM">2:30 PM</option>
                        <option value="3:00 PM">3:00 PM</option>
                        <option value="3:30 PM">3:30 PM</option>
                        <option value="4:00 PM">4:00 PM</option>
                        <option value="4:30 PM">4:30 PM</option>
                        <option value="5:00 PM">5:00 PM</option>
                        <option value="5:30 PM">5:30 PM</option>
                        <option value="6:00 PM">6:00 PM</option>
                        <option value="6:30 PM">6:30 PM</option>
                        <option value="7:00 PM">7:00 PM</option>
                        <option value="7:30 PM">7:30 PM</option>
                        <option value="8:00 PM">8:00 PM</option>
                        <option value="8:30 PM">8:30 PM</option>
                        <option value="9:00 PM">9:00 PM</option>
                        <option value="9:30 PM">9:30 PM</option>
                        <option value="10:00 PM">10:00 PM</option>
                        <option value="10:30 PM">10:30 PM</option>
                        <option value="11:00 PM">11:00 PM</option>
                        <option value="11:30 PM">11:30 PM</option>
                    </select>
                </td>
                <td>
                    <input type="hidden" id="eventId" />
                </td>
            </tr>
            <tr id="trScheduleDayException">
                <td>&nbsp;</td>
                <td colspan="2" align="left" valign="top">
                    <div id="divScheduleDayException">
                        <input id="chkScheduleDayException" type="checkbox" title="Schedule day exception" /><span id="spnScheduleDayException">Schedule day exception</span>
                    </div>
                    <input id="chkComplete" type="checkbox" title="Complete" /><span id="spnComplete">Complete</span>
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
            </tr>
            <tr id="EventLinkSection">
                <td colspan="3">
                    <div class="tab">
                        <button class="tablinks" onclick="openTab(event, 'ParentEventLink')" id="btnParentEventLink">Parent Link</button>
                        <button class="tablinks" onclick="openTab(event, 'EventLink')" id="btnEventLink">Child Link</button>
                        <button class="tablinks" onclick="openTab(event, 'Notes')">Notes</button>

                    </div>

                    <!-- Tab content -->
                    <div id="ParentEventLink" class="tabcontent">
                        <h3>Parent Link</h3>
                        <table style="width: 99%;">
                            <tr id="trParentSection" style="display: none;">
                                <td class="alignRight">Parent Section:
                                     <br />
                                    <font style="color: #6e5555;">(type * to show all items)</font>
                                </td>
                                <td class="alignLeft">
                                    <table style="padding: 0px; margin: 0px;">
                                        <tr style="padding: 0px; margin: 0px;">
                                            <td style="padding: 0px; margin: 0px;">
                                                <input id="linkToParent" type="text" style="width: 150px; overflow: scroll;" title=" Type * to view all " /></td>
                                            <td style="padding: 0px; margin: 0px;">
                                                <select id="ddlParentdependencyType" onchange="ddlParentdependencyType_Onchange();">
                                                    <option value="1">Start Same Time</option>
                                                    <option value="2">Start After Finish</option>
                                                    <option value="3">Offset days</option>
                                                </select><br />
                                                <input id="txtParentOffsetdays" type="text" style="width: 100px;" />
                                            </td>
                                        </tr>

                                    </table>
                                </td>
                                <td>
                                    <button id="btnAddParentLink" title="Add Link" onclick="AddParentEventLink();" class="ui-button ui-corner-all ui-widget" style="display: none;">Add Link</button></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="text-align: right;">
                                    <button id="btnUpdateParentLink" title="Update Link" onclick="UpdateParentEventLink();" class="ui-button ui-corner-all ui-widget" style="display: none;">Update Link</button>&nbsp;&nbsp;&nbsp;
                                    <button id="btnDeleteParentLink" title="Delete Link" onclick="DeleteParentEventLink();" class="ui-button ui-corner-all ui-widget" style="display: none;">Delete Link</button>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="3">
                                    <table id="ParentLinkTbl" class="mGrid" style="width: 100%;"></table>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div id="EventLink" class="tabcontent">
                        <h3>Child Link</h3>
                        <table style="width: 99%;">
                            <tr id="trChildSection">
                                <td class="alignRight">Subsequent Section:
                                     <br />
                                    <font style="color: #6e5555;">(type * to show all items)</font>
                                </td>
                                <td class="alignLeft">
                                    <table style="padding: 0px; margin: 0px;">
                                        <tr style="padding: 0px; margin: 0px;">
                                            <td style="padding: 0px; margin: 0px;">
                                                <input id="linkToSubsequent" type="text" style="width: 150px; overflow: scroll;" title=" Type * to view all " /></td>
                                            <td style="padding: 0px; margin: 0px;">
                                                <select id="ddldependencyType" onchange="ddldependencyType_Onchange();">
                                                    <option value="1">Start Same Time</option>
                                                    <option value="2">Start After Finish</option>
                                                    <option value="3">Offset days</option>
                                                </select><br />
                                                <input id="txtOffsetdays" type="text" style="width: 100px;" />
                                            </td>
                                        </tr>

                                    </table>
                                </td>
                                <td style="vertical-align: middle;">
                                    <button id="btnAddLink" title="Add Link" onclick="AddEventLink();" class="ui-button ui-corner-all ui-widget">Add Link</button></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="text-align: right;" colspan="2">
                                    <button id="btnUpdateLink" title="Update Link" onclick="UpdateEventLink();" class="ui-button ui-corner-all ui-widget" style="display: none;">Update Link</button>&nbsp;&nbsp;&nbsp;
                                    <button id="btnDeleteLink" title="Delete Link" onclick="DeleteEventLink();" class="ui-button ui-corner-all ui-widget" style="display: none;">Delete Link</button>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="3">
                                    <table id="ChildLinkTbl" class="mGrid" style="width: 100%;"></table>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div id="Notes" class="tabcontent">
                        <h3>Notes</h3>
                        <table>
                            <tr>
                                <td class="alignRight" valign="top">Description:
                                </td>
                                <td class="alignLeft" valign="top">
                                    <textarea id="eventDesc" class="" style="width: 400px;" cols="40" rows="3" onchange="txtChange(this.id)"></textarea>
                                </td>
                                <td class="alignLeft" valign="bottom">
                                    <label id="lbleventDesc" class="hidden">
                                        Required</label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>


            <tr id="trTradePartner" style="display: none;">
                <td class="alignRight" style="padding-bottom: 12px;">Trade:
                </td>
                <td class="alignLeft">
                    <input id="txtTradePartner" maxlength="40" style="width: 225px" />
                </td>
                <td>
                    <label id="lblTradePartner" class="hidden" style="padding-bottom: 10px;">Required</label>
                </td>
            </tr>
            <tr id="trNotes" style="display: none;">
                <td class="alignRight" valign="top" style="padding-top: 3px;">Notes:
                </td>
                <td class="alignLeft">
                    <textarea id="txtNotes" maxlength="180" class="scTextArea" cols="40" rows="3"></textarea>
                </td>
                <td>
                    <label id="lblNotes" class="hidden">Required</label>
                </td>
            </tr>
            <tr>
                <td class="alignRight">&nbsp;
                </td>
                <td colspan="2">
                    <label id="lblRequired" class="hidden">End time must be later than start time</label>
                </td>
            </tr>
        </table>
    </div>
    <div id="loading">
        <div class="overlay" />
        <div class="overlayContent">
            <p>
                Please wait while your data is being processed
            </p>
            <img src="images/ajax_loader.gif" alt="Loading" border="1" />
        </div>
    </div>
    <div id="addDialog" class="popWindow popWindowAdd" title="Add Event">
        <table cellpadding="0">
            <tr>
                <td class="alignRight">Section/Title:
                     <br />
                    <font style="color: #6e5555;">(type * to show all items)</font>
                </td>
                <td class="alignLeft">
                    <input id="addEventName" type="text" onchange="txtChange(this.id)" style="width: 226px; overflow: scroll;" title=" Type * to view all " />
                </td>
                <td class="alignLeft">
                    <label id="lbladdEventName" class="hidden" style="padding-bottom: 0px;">
                        Required</label>
                </td>
            </tr>
            <tr>
                <td class="alignRight">Location:
                     <br />
                    <font style="color: #6e5555;">(type * to show all items)</font>
                </td>
                <td class="alignLeft">
                    <input id="addLocation" type="text" onchange="txtLocationChange(this.id)" style="width: 226px; overflow: scroll;" title=" Type * to view all " />
                </td>
                <td class="alignLeft">
                    <label id="lbladdLocation" class="hidden" style="padding-bottom: 0px;">
                        Required</label>
                </td>
            </tr>
            <tr>
                <td class="alignRight">Assigned To:
                     <br />
                    <font style="color: #6e5555;">(type * to show all items)</font>
                </td>
                <td class="alignLeft">
                    <input id="addSalesPersonName" type="text" onchange="txtChange(this.id)" style="width: 226px; overflow: scroll;" title=" Type * to view all " />
                </td>
                <td class="alignLeft">
                    <label id="lbladdSalesPersonName" class="hidden" style="padding-bottom: 0px;">
                        Required</label>
                </td>
            </tr>

            <tr>
                <td class="alignRight">Start:
                </td>
                <td colspan="3" class="alignLeft">
                    <input type="text" id="addEventStartDate" onchange="setAddEventStartTime('Date')" />
                    <select name="addEventStartTime" id="addEventStartTime" onchange="setAddEventStartTime('Time')">
                        <option value="5:00 AM">5:00 AM</option>
                        <option value="5:00 AM" selected="selected">5:00 AM</option>
                        <option value="5:30 AM">5:30 AM</option>
                        <option value="6:00 AM">6:00 AM</option>
                        <option value="6:30 AM">6:30 AM</option>
                        <option value="7:00 AM">7:00 AM</option>
                        <option value="7:30 AM">7:30 AM</option>
                        <option value="8:00 AM">8:00 AM</option>
                        <option value="8:00 AM">8:00 AM</option>
                        <option value="8:30 AM">8:30 AM</option>
                        <option value="9:00 AM">9:00 AM</option>
                        <option value="9:30 AM">9:30 AM</option>
                        <option value="10:00 AM">10:00 AM</option>
                        <option value="10:30 AM">10:30 AM</option>
                        <option value="11:00 AM">11:00 AM</option>
                        <option value="11:30 AM">11:30 AM</option>
                        <option value="12:00 PM">12:00 PM</option>
                        <option value="12:30 PM">12:30 PM</option>
                        <option value="1:00 PM">1:00 PM</option>
                        <option value="1:30 PM">1:30 PM</option>
                        <option value="2:00 PM">2:00 PM</option>
                        <option value="2:30 PM">2:30 PM</option>
                        <option value="3:00 PM">3:00 PM</option>
                        <option value="3:30 PM">3:30 PM</option>
                        <option value="4:00 PM">4:00 PM</option>
                        <option value="4:30 PM">4:30 PM</option>
                        <option value="5:00 PM">5:00 PM</option>
                        <option value="5:30 PM">5:30 PM</option>
                        <option value="6:00 PM">6:00 PM</option>
                        <option value="6:30 PM">6:30 PM</option>
                        <option value="7:00 PM">7:00 PM</option>
                        <option value="7:30 PM">7:30 PM</option>
                        <option value="8:00 PM">8:00 PM</option>
                        <option value="8:30 PM">8:30 PM</option>
                        <option value="9:00 PM">9:00 PM</option>
                        <option value="9:30 PM">9:30 PM</option>
                        <option value="10:00 PM">10:00 PM</option>
                        <option value="10:30 PM">10:30 PM</option>
                        <option value="11:00 PM">11:00 PM</option>
                        <option value="11:30 PM">11:30 PM</option>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="alignRight">End:
                </td>
                <td colspan="3" class="alignLeft">
                    <input type="text" id="addEventEndDate" onchange="setAddEventEndTime('Date')" />
                    <select name="addEventEndTime" id="addEventEndTime" onchange="setAddEventEndTime('Time')">
                        <option value="5:00 AM">5:00 AM</option>
                        <option value="5:30 AM">5:30 AM</option>
                        <option value="6:00 AM">6:00 AM</option>
                        <option value="6:00 AM" selected="selected">6:00 AM</option>
                        <option value="6:30 AM">6:30 AM</option>
                        <option value="7:00 AM">7:30 AM</option>
                        <option value="7:30 AM">7:30 AM</option>
                        <option value="8:00 AM">8:00 AM</option>
                        <option value="8:30 AM">8:30 AM</option>
                        <option value="9:00 AM">9:00 AM</option>

                        <option value="9:30 AM">9:30 AM</option>
                        <option value="10:00 AM">10:00 AM</option>
                        <option value="10:30 AM">10:30 AM</option>
                        <option value="11:00 AM">11:00 AM</option>
                        <option value="11:30 AM">11:30 AM</option>
                        <option value="12:00 PM">12:00 PM</option>
                        <option value="12:30 PM">12:30 PM</option>
                        <option value="1:00 PM">1:00 PM</option>
                        <option value="1:30 PM">1:30 PM</option>
                        <option value="2:00 PM">2:00 PM</option>
                        <option value="2:30 PM">2:30 PM</option>
                        <option value="3:00 PM">3:00 PM</option>
                        <option value="3:30 PM">3:30 PM</option>
                        <option value="4:00 PM">4:00 PM</option>
                        <option value="4:30 PM">4:30 PM</option>
                        <option value="5:00 PM">5:00 PM</option>
                        <option value="5:30 PM">5:30 PM</option>
                        <option value="6:00 PM">6:00 PM</option>
                        <option value="6:30 PM">6:30 PM</option>
                        <option value="7:00 PM">7:00 PM</option>
                        <option value="7:30 PM">7:30 PM</option>
                        <option value="8:00 PM">8:00 PM</option>
                        <option value="8:30 PM">8:30 PM</option>
                        <option value="9:00 PM">9:00 PM</option>
                        <option value="9:30 PM">9:30 PM</option>
                        <option value="10:00 PM">10:00 PM</option>
                        <option value="10:30 PM">10:30 PM</option>
                        <option value="11:00 PM">11:00 PM</option>
                        <option value="11:30 PM">11:30 PM</option>
                    </select>
                </td>

            </tr>
            <tr id="traddScheduleDayException">
                <td>&nbsp;</td>
                <td colspan="2" align="left" valign="top">
                    <div id="divaddScheduleDayException">
                        <input id="chkaddScheduleDayException" type="checkbox" title="Schedule day exception" /><span id="spnaddScheduleDayException">Schedule day exception&nbsp;</span>
                    </div>
                    <input id="chkaddComplete" type="checkbox" title="Complete" /><span id="spnaddComplete">Complete</span>
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
            </tr>
            <tr id="addEventLinkSection">
                <td colspan="3">
                    <div class="tab">
                        <button class="tablinks" onclick="openTab(event, 'addParentEventLink')" id="btnaddParentEventLink">Parent Link</button>
                        <button class="tablinks" onclick="openTab(event, 'addEventLink')" id="btnaddEventLink">Child Link</button>
                        <button class="tablinks" onclick="openTab(event, 'addNotes')">Notes</button>

                    </div>

                    <!-- Tab content -->
                    <div id="addParentEventLink" class="tabcontent">
                        <h3>Parent Link</h3>
                        <table style="width: 99%;">
                            <tr id="traddParentSection">
                                <td class="alignRight">Parent Section:
                                     <br />
                                    <font style="color: #6e5555;">(type * to show all items)</font>
                                </td>
                                <td class="alignLeft">
                                    <table style="padding: 0px; margin: 0px;">
                                        <tr style="padding: 0px; margin: 0px;">
                                            <td style="padding: 0px; margin: 0px;">
                                                <input id="addlinkToParent" type="text" style="width: 150px; overflow: scroll;" title=" Type * to view all " /></td>
                                            <td style="padding: 0px; margin: 0px;">
                                                <select id="ddladdParentdependencyType" onchange="ddladdParentdependencyType_Onchange();">
                                                    <option value="1">Start Same Time</option>
                                                    <option value="2">Start After Finish</option>
                                                    <option value="3">Offset days</option>
                                                </select><br />
                                                <input id="txtaddParentOffsetdays" type="text" style="width: 100px;" />
                                            </td>
                                        </tr>

                                    </table>
                                </td>
                                <td></td>
                            </tr>



                        </table>
                    </div>

                    <div id="addEventLink" class="tabcontent">
                        <h3>Child Link</h3>
                        <table style="width: 99%;">
                            <tr id="traddChildSection">
                                <td class="alignRight">Subsequent Section:
                                     <br />
                                    <font style="color: #6e5555;">(type * to show all items)</font>
                                </td>
                                <td class="alignLeft">
                                    <table style="padding: 0px; margin: 0px;">
                                        <tr style="padding: 0px; margin: 0px;">
                                            <td style="padding: 0px; margin: 0px;">
                                                <input id="addlinkToSubsequent" type="text" style="width: 150px; overflow: scroll;" title=" Type * to view all " /></td>
                                            <td style="padding: 0px; margin: 0px;">
                                                <select id="ddladddependencyType" onchange="ddladddependencyType_Onchange();">
                                                    <option value="1">Start Same Time</option>
                                                    <option value="2">Start After Finish</option>
                                                    <option value="3">Offset days</option>
                                                </select><br />
                                                <input id="txtaddOffsetdays" type="text" style="width: 100px;" />
                                            </td>
                                        </tr>

                                    </table>
                                </td>
                                <td style="vertical-align: middle;"></td>
                            </tr>

                        </table>
                    </div>

                    <div id="addNotes" class="tabcontent">
                        <h3>Notes</h3>
                        <table>
                            <tr>
                                <td class="alignRight" valign="top">Description:
                                </td>
                                <td class="alignLeft" valign="top">
                                    <textarea id="addEventDesc" class="" style="width: 295px;" cols="40" rows="3" onchange="txtaddChange(this.id)"></textarea>
                                </td>
                                <td class="alignLeft" valign="bottom">
                                    <label id="lbladdEventDesc" class="hidden">
                                        Required</label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <%-- <tr>
                <td class="alignRight" valign="top">Description:
                </td>
                <td class="alignLeft">
                    <textarea id="addEventDesc" class="scTextArea" cols="40" rows="3" onchange="txtChange(this.id)"></textarea>
                </td>
                <td class="alignLeft">
                    <label id="lbladdEventDesc" class="hidden" style="padding-top: 50px;">
                        Required</label>
                </td>
            </tr>--%>
            <%--  <tr>
                <td class="alignRight">Subsequent Section:
                </td>
                <td colspan="2" class="alignLeft">
                    <input id="addlinkToSubsequent" type="text" style="width: 150px; overflow: scroll;" />
               
                    <select id="ddladddependencyType">
                        <option value="1">Start Same Time</option>
                        <option value="2">Start After Finish</option>
                        <option value="3">Offset days</option>
                    </select>
                </td>
               
            </tr>--%>
            <tr>
                <td colspan="3" style="text-align: center;">
                    <label id="lblTime" class="hidden">
                        End time must be later than start time</label></td>
            </tr>

        </table>
    </div>
    <div id="dialog-confirm" class="ui-dialog-content-confirm" title="Schedule change" hidden="true">
        <p style="color: Black; font-size: 16px; line-height: 18px;">
            Make changes to this and the subsequent entries?
        </p>
    </div>
    <div runat="server" id="jsonDiv" />
    <%--<input type="hidden" id="hdClient" runat="server" />--%>

    <asp:HiddenField ID="hdnAddEventName" runat="server" Value="" />
    <asp:HiddenField ID="hdnEventDesc" runat="server" Value="" />
    <asp:HiddenField ID="hdnEditeventName" runat="server" Value="" />
    <asp:HiddenField ID="hdnEstimateID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnCustomerID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnEmployeeID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnTypeID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnEventStartDate" runat="server" Value="" />
    <asp:HiddenField ID="hdnServiceCssClass" runat="server" Value="fc-default" />
    <asp:HiddenField ID="hdnEstIDSelected" runat="server" Value="0" />
    <asp:HiddenField ID="hdnCustIDSelected" runat="server" Value="0" />
    <asp:HiddenField ID="hdnAutoCustID" runat="server" Value="0" />
    <asp:HiddenField ID="hdnEventId" runat="server" Value="0" />
    <asp:HiddenField ID="hdnUpdateDialogShow" runat="server" Value="false" />

    <br />
    <br />

</asp:Content>

