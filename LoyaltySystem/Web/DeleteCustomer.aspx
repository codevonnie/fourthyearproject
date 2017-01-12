<%@ Page Title="" Language="C#" MasterPageFile="/MasterPage.master" AutoEventWireup="true" CodeFile="DeleteCustomer.aspx.cs" Inherits="Web_AddCustomer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <h1 class="text-center">Delete Customer</h1>

    <div class="container-fluid">
        <div class="form-group">
            <asp:Label ID="LblMember" runat="server" Text="Delete Member" Font-Bold="True"></asp:Label>
            <asp:TextBox ID="TbMember" runat="server" CssClass="form-control" placeholder="Unique Field"></asp:TextBox>

        </div>
        <div class="form-group">
            <asp:Button ID="BtnDeleteMember" class="btn btn-primary btn-block" runat="server" Text="Delete Member" OnClick="BtnDeleteMember_Click"/>
        </div>
    </div>

</asp:Content>

