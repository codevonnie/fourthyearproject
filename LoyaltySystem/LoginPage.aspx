<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LoginPage.aspx.cs" Inherits="LoginPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



    <div id="loginModal" class="modal show col-sm-9 col-sm-offset-3 col-md-10 col-md-offset-2" tabindex="-1" aria-hidden="true">
        <br />
        <br />
        <br />
        <br />
        <br />
        <br />
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h1 class="text-center">Login</h1>
                </div>
                <div class="modal-body">

                    <div class="form-group">
                        <asp:TextBox ID="TbEmail" runat="server" class="form-control input-lg" placeholder="Email" TextMode="Email"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:TextBox ID="TbPassword" runat="server" class="form-control input-lg" placeholder="Password" TextMode="Password"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <asp:Button class="btn btn-primary btn-lg btn-block" ID="BtnSignIn" runat="server" Text="Sign In" OnClick="singInBtn_Click" />
                    </div>
                </div>

                <div class="modal-footer">

                    <div class="form-group">
                        <asp:Button class="btn btn-primary btn-lg btn-block" ID="BtnRegister" runat="server" Text="Register" />
                    </div>

                </div>
            </div>
        </div>
    </div>

</asp:Content>

