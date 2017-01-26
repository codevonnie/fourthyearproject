<%@ Page Title="" Language="C#" MasterPageFile="/MasterPage.master" AutoEventWireup="true" CodeFile="DeleteMember.aspx.cs" Inherits="Web_AddCustomer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="container-fluid modal-dialog">
        <div class="modal-content ">
            <div class="modal-header">
                <h1 class="text-center">Delete Member</h1>
            </div>
            <div class="modal-body">

                <asp:Label runat="server" Text="Delete Member" Font-Bold="True"></asp:Label>
                <div class="form-group input-group input-group-lg">
                    <span class="input-group-addon">
                        <i class="fa fa-envelope iconWidth"></i>
                    </span>
                    <asp:TextBox ID="TbEmail" runat="server" class="form-control" placeholder="Enter Members Email Address" TextMode="Email"></asp:TextBox>
                </div>
            </div>

            <div class="modal-footer">
                <div class="form-group">
                    <asp:Button ID="BtnDeleteMember" class="btn btn-primary btn-block btn-lg" runat="server" Text="Delete Member" OnClick="BtnDeleteMember_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>

