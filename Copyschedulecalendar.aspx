<%@ Page Language="C#" MasterPageFile="~/Copyschedulemaster.master" AutoEventWireup="true"
    CodeFile="Copyschedulecalendar.aspx.cs" Inherits="Copyschedulecalendar" Title="Schedule Calendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

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
                                                        Text="Return to Service Ticket" Visible="true" OnClick="btnBack_Click"></asp:LinkButton>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td align="right" style="padding-right: 30px;">
                                                    <asp:ImageButton ID="btnSalesCalendar" ImageUrl="~/images/calendar_money.png" runat="server" CssClass="imageBtn" OnClick="btnSalesCalendar_Click" ToolTip="Go to Sales Calendar" />
                                                    <asp:ImageButton ID="btnOperationCalendar" ToolTip="Go to Operation Calendar" CssClass="imageBtn"
                                                        runat="server" ImageUrl="~/images/helmet_celendar.png" OnClick="btnOperationCalendar_Click" />
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
                        <td valign="top" align="right">
                            <table style="padding: 0px; margin-top: -6px;">
                                <tr>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>

                                            <td valign="top">
                                                <div class="">
                                                    <asp:Label ID="lblCalState" Style="font-weight: bold; border-radius: 5px; padding: 5px;" runat="server"></asp:Label>
                                                    <asp:Button ID="btnCalStateAction" runat="server" CssClass="CssCalStateActionBtn" Text="Go Online" OnClick="btnCalStateAction_Click" OnClientClick="return CalStateAction();" />
                                                </div>

                                            </td>
                                            <asp:HiddenField ID="hdnCalStateAction" runat="server" Value="" />
                                            <asp:HiddenField ID="hdnEventLinkCount" runat="server" Value="0" />
                                            <asp:Button ID="btnHdn" runat="server" OnClick="btnHdn_Click" Style="display: none;" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </tr>
                                <tr id="trSearchCal" runat="server">
                                    <td align="left">
                                        <table style="padding: 0px; margin: 0px;">
                                            <tr>
                                                <td align="right">Last Name:&nbsp;</td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSearch" CssClass="acSearch" runat="server" onkeypress="return searchKeyPress(event);"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">Section:&nbsp;</td>
                                                <td>
                                                    <asp:TextBox ID="txtSection" CssClass="acSearch" runat="server" onkeypress="return searchKeyPress(event);"></asp:TextBox>
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
                                    <td>

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
                                        <div id="miniCalendar"></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-top: 8px; padding-bottom: 0px; margin-bottom: 5px;">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>
                                                <font style="font-weight: bold;">Projects:</font>
                                                <asp:DropDownList ID="ddlEst" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlEst_SelectedIndexChanged" style="max-width: 245px;">
                                                </asp:DropDownList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr style="display: normal;">
                                    <td>
                                        <div id='external-events'>
                                            <p style="padding-top: 8px; padding-bottom: 0px; margin-bottom: 5px; color: #000000;">
                                                <strong>Sections</strong>
                                            </p>

                                            <asp:GridView ID="grdDragSectionList" runat="server" AutoGenerateColumns="False" Width="100%" ShowHeader="false" CssClass="cssgrdDragSectionList"
                                                OnRowDataBound="grdDragSectionList_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="" ShowHeader="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSection" CssClass="fc-event fc-default" runat="server" Text='<%# Eval("section_name")%>' />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="100%" HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                </Columns>

                                            </asp:GridView>

                                            <%-- <div class='fc-event'>My Event 1</div>
                                            <div class='fc-event'>My Event 2</div>
                                            <div class='fc-event'>My Event 3</div>
                                            <div class='fc-event'>My Event 4</div>
                                            <div class='fc-event'>My Event 5</div>--%>
                                            <%-- <p>
                                                <input type='checkbox' id='drop-remove' />
                                                <label for='drop-remove'>remove after drop</label>
                                            </p>--%>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top" align="left">
                            <div id="calendar" style="margin-left: 5px;">
                            </div>
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
        <table cellpadding="0">
            <tr>
                <td class="alignRight" valign="top">Title:
                </td>
                <td class="alignLeft" valign="top">
                    <textarea id="eventName" class="scTextArea" cols="40" rows="3" onchange="txtChange(this.id)"></textarea>
                </td>
                <td class="alignLeft" valign="bottom">
                    <label id="lbleventName" class="hidden">
                        Required</label>
                </td>
            </tr>
            <tr>
                <td class="alignRight">Assigned To:
                </td>
                <td class="alignLeft">
                    <input id="eventSalesPerson" type="text" onchange="txtChange(this.id)" style="width: 226px; overflow: scroll;" />
                </td>
                <td class="alignLeft">
                    <label id="lbleventSalesPerson" class="hidden" style="padding-bottom: 0px;">
                        Required</label>
                </td>
            </tr>
            <tr>
                <td class="alignRight">Start:
                </td>
                <td class="alignLeft">&nbsp;&nbsp;
                    <input type="text" id="eventStart" onchange="setEventStartTime()" />
                    <select name="eventStartTime" id="eventStartTime" onchange="setEventStartTime()">
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
                <td></td>
            </tr>
            <tr>
                <td class="alignRight">End:
                </td>
                <td class="alignLeft">&nbsp;&nbsp;
                    <input type="text" id="eventEnd" onchange="setEventEndTime()" />
                    <select name="eventEndTime" id="eventEndTime" onchange="setEventEndTime()">
                        <option value="6:00 AM">6:00 AM</option>
                        <option value="6:30 AM">6:30 AM</option>
                        <option value="7:00 AM" selected="selected">7:00 AM</option>
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
            <tr>
                <td colspan="3">
                    <div class="tab">
                        <button class="tablinks" onclick="openTab(event, 'EventLink')" id="btnEventLink">Link</button>
                        <button class="tablinks" onclick="openTab(event, 'Notes')">Notes</button>

                    </div>

                    <!-- Tab content -->
                    <div id="EventLink" class="tabcontent">
                        <h3>Link</h3>
                        <table>
                            <tr>
                                <td class="alignRight">Subsequent Section:
                                </td>
                                <td class="alignLeft">
                                    <table style="padding: 0px; margin: 0px;">
                                        <tr style="padding: 0px; margin: 0px;">
                                            <td style="padding: 0px; margin: 0px;">
                                                <input id="linkToSubsequent" type="text" style="width: 150px; overflow: scroll;" /></td>
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
                                <td>
                                    <button id="btnAddLink" title="Add Link" onclick="AddEventLink();" style="display: none;">Add Link</button></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="text-align: right;">
                                    <button id="btnUpdateLink" title="Update Link" onclick="UpdateEventLink();" class="ui-button ui-corner-all ui-widget">Update Link</button>&nbsp;&nbsp;&nbsp;
                                    <button id="btnDeleteLink" title="Delete Link" onclick="DeleteEventLink();" class="ui-button ui-corner-all ui-widget">Delete Link</button>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="grdCalLinkInfo" runat="server" AutoGenerateColumns="False" Width="100%" DataKeyNames="link_id" OnRowDataBound="grdCalLinkInfo_RowDataBound"
                                        CssClass="mGrid">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Link">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkLink" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="5%" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="title" HeaderText="Title" ItemStyle-Width="65%" ItemStyle-HorizontalAlign="Left" />

                                            <asp:TemplateField HeaderText="Start">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStart" CssClass="csslblStart" runat="server" Text='<%# Eval("start", "{0:MM/dd/yyyy hh:mm tt}")%>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="End">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEnd" CssClass="csslblEnd" runat="server" Text='<%# Eval("end", "{0:MM/dd/yyyy hh:mm tt}")%>' />
                                                </ItemTemplate>
                                                <ItemStyle Width="15%" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="parent_event_id" ItemStyle-CssClass="cssparent_event_id hdnColumnCss" ShowHeader="false" HeaderText="" HeaderStyle-CssClass="hdnColumnCss" />
                                            <asp:BoundField DataField="link_id" ItemStyle-CssClass="csslink_id hdnColumnCss" ShowHeader="false" HeaderText="" HeaderStyle-CssClass="hdnColumnCss" />
                                            <asp:TemplateField HeaderText="">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="grdddldependencyType" CssClass="cssddldependencyType" runat="server">
                                                        <asp:ListItem Value="1" Text="Start Same Time"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="Start After Finish"></asp:ListItem>
                                                        <asp:ListItem Value="3" Text="Offset days"></asp:ListItem>
                                                    </asp:DropDownList><br />
                                                    <asp:TextBox ID="grdtxtOffsetdays" CssClass="csstxtOffsetdays" Width="50px" runat="server" Text='<%# Eval("lag")%>'></asp:TextBox>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" Width="5%" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="pgr" HorizontalAlign="Left" />
                                        <AlternatingRowStyle CssClass="alt" />
                                    </asp:GridView>
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
                                    <textarea id="eventDesc" class="scTextArea" cols="40" rows="3" onchange="txtChange(this.id)"></textarea>
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
                </td>
                <td class="alignLeft">
                    <input id="addEventName" type="text" onchange="txtChange(this.id)" style="width: 226px; overflow: scroll;" />
                </td>
                <td class="alignLeft">
                    <label id="lbladdEventName" class="hidden" style="padding-bottom: 0px;">
                        Required</label>
                </td>
            </tr>
            <tr>
                <td class="alignRight">Assigned To:
                </td>
                <td class="alignLeft">
                    <input id="addSalesPersonName" type="text" onchange="txtChange(this.id)" style="width: 226px; overflow: scroll;" />
                </td>
                <td class="alignLeft">
                    <label id="lbladdSalesPersonName" class="hidden" style="padding-bottom: 0px;">
                        Required</label>
                </td>
            </tr>

            <tr>
                <td class="alignRight">Start:
                </td>
                <td colspan="3" class="alignLeft">&nbsp;&nbsp;
                    <input type="text" id="addEventStartDate" onchange="setAddEventStartTime()" />
                    <select name="addEventStartTime" id="addEventStartTime" onchange="setAddEventStartTime()">
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
            </tr>
            <tr>
                <td class="alignRight">End:
                </td>
                <td colspan="3" class="alignLeft">&nbsp;&nbsp;
                    <input type="text" id="addEventEndDate" onchange="setAddEventEndTime()" />
                    <select name="addEventEndTime" id="addEventEndTime" onchange="setAddEventEndTime()">
                        <option value="6:00 AM">6:00 AM</option>
                        <option value="6:30 AM">6:30 AM</option>
                        <option value="7:00 AM" selected="selected">7:00 AM</option>
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
            <tr>
                <td class="alignRight" valign="top">Description:
                </td>
                <td class="alignLeft">
                    <textarea id="addEventDesc" class="scTextArea" cols="40" rows="3" onchange="txtChange(this.id)"></textarea>
                </td>
                <td class="alignLeft">
                    <label id="lbladdEventDesc" class="hidden" style="padding-top: 50px;">
                        Required</label>
                </td>
            </tr>
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

