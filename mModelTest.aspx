<%@ Page Title="" Language="C#" MasterPageFile="~/MobileSite.master" AutoEventWireup="true" CodeFile="mModelTest.aspx.cs" Inherits="mModelTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script type="text/javascript">

         function openModal() {
             $('#Testmodel').modal({ show: true });
         }
        
    </script>

     <div class="row">
        <div class="col-lg-6">
            &nbsp;
        </div>
        <div class="col-lg-3 text-right">
            <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#Testmodel">
                <span class="glyphicon glyphicon-plus-sign"></span>&nbsp;Show Model
            </button>
        </div>
    </div>

     <div class="modal fade" id="Testmodel" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLongTitle">Add Title</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-6  col-lg-offset-1">
                            <div role="form">
                                <div class="form-group">
                                    <label>Name</label>
                                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label>Designation</label>
                                    <asp:TextBox ID="txtDesignation" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                                <asp:Label ID="lblResult" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnAdd_Click" /> 
                    <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary" Text="Save"  />                   
                </div>
            </div>
        </div>
    </div>
</asp:Content>

