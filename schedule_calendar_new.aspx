<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeFile="schedule_calendar_new.aspx.cs" Inherits="schedule_calendar_new" Title="Untitled Page" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<table bordercolor="silver" border="1">
				<tr vAlign="top">
					<td>
						<table borderColor="silver" border="1">
							<tr>
								<td style="PADDING-RIGHT: 10px; PADDING-LEFT: 10px; PADDING-BOTTOM: 10px; PADDING-TOP: 10px"
									vAlign="top">
									<TABLE borderColor="silver" border="1">
										<TR>
											<td vAlign="top">
												<table width="100%" align="left" border="0">
													<TR>
														<TD width="100%">
															<table id="Table3" cellspacing="1" cellpadding="1" width="100%" border="0">
																<TR>
																	<TD width="650">
																		<asp:HyperLink id="lnkPrev3" runat="server" ></asp:HyperLink>
																		<asp:HyperLink id="lnkPrev2" runat="server" ></asp:HyperLink>
																		<asp:HyperLink id="lnkPrev1" runat="server" ></asp:HyperLink></TD>
																	<TD>
																		<asp:HyperLink id="lnkNext1" runat="server" ></asp:HyperLink>
																		<asp:HyperLink id="lnkNext2" runat="server" ></asp:HyperLink></TD>
																</TR>
															</table>
														</TD>
													</TR>
												</table>
											</td>
										</TR>
										<TR>
											<TD colSpan="2" height="20"><asp:label id="lblAppointment" runat="server">Monthly Schedule Calendar</asp:label></TD>
										</TR>
										<TR>
											<TD style="HEIGHT: 18px" vAlign="middle" align="left" colSpan="2"><asp:placeholder id="plcGrid" runat="server"></asp:placeholder></TD>
										</TR>
										<TR>
											<TD colSpan="2" height="20">
												<TABLE id="Table1" width="100%">
													<TR>
														<TD>&nbsp;&nbsp;&nbsp;&nbsp;</TD>
													</TR>
												</TABLE>
												<TABLE id="Table2" cellSpacing="1" cellPadding="1" width="100%" border="0">
													<TR>
														<TD width="650">
															<asp:HyperLink id="lnkPrevD3" runat="server" ></asp:HyperLink>
															<asp:HyperLink id="lnkPrevD2" runat="server" ></asp:HyperLink>
															<asp:HyperLink id="lnkPrevD1" runat="server" ></asp:HyperLink></TD>
														<TD>
															<asp:HyperLink id="lnkNextD1" runat="server" ></asp:HyperLink>
															<asp:HyperLink id="lnkNextD2" runat="server" ></asp:HyperLink></TD>
													</TR>
													
												</TABLE>
											</TD>
										</TR>
									</TABLE>
								</td>
							</tr>
						</table>
					</td>
				</tr> </table>
</asp:Content>

