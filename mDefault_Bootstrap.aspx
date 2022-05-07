<%@ Page Title="" Language="C#" MasterPageFile="~/MobileSite.master" AutoEventWireup="true" CodeFile="mDefault_Bootstrap.aspx.cs" Inherits="mDefault_Bootstrap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <div class="main-content">
        <div class="form-horizontal">
            <div class="form-group form-group-ext">
                <label for="inputEmail3" class="col-sm-2 control-label">Email</label>
                <div class="col-sm-10">
                    <input type="email" class="form-control" id="inputEmail3" placeholder="Email">
                </div>
            </div>
            <div class="form-group form-group-ext">
                <label for="inputPassword3" class="col-sm-2 control-label">Password</label>
                <div class="col-sm-10">
                    <input type="password" class="form-control" id="inputPassword3" placeholder="Password">
                </div>
            </div>
            <div class="form-group form-group-ext">
                <div class="col-sm-offset-2 col-sm-10">
                    <div class="checkbox">
                        <label>
                            <input type="checkbox">
                            Remember me
                        </label>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                    <button type="submit" class="btn btn-default">Sign in</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

