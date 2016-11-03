<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Customer.aspx.cs" Inherits="Web_Customer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <h1 class="text-center">ADD A NEW CUSTOMER</h1>

    <div class="container-fluid">
        <div class="form-group">
            <asp:Label ID="LblName" runat="server" Text="Name Of Participant" Font-Bold="True"></asp:Label>
            <asp:TextBox ID="TbName" runat="server" CssClass="form-control" placeholder="Enter Name"></asp:TextBox>
        </div>
                <div class="form-group">
            <asp:Label ID="LblGender" runat="server" Text="Sex" Font-Bold="True"></asp:Label>
            <asp:TextBox ID="TbGender" runat="server" CssClass="form-control" placeholder="Male/Female"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="LblEmail" runat="server" Text="Email Address" Font-Bold="True"></asp:Label>
            <asp:TextBox ID="TbEmail" runat="server" CssClass="form-control" placeholder="Email Address" TextMode="Email"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="LblDob" runat="server" Text="Date Of Birth" Font-Bold="True"></asp:Label>
            <asp:TextBox ID="TbDob" runat="server" CssClass="form-control" placeholder="Date Of Birth" TextMode="Date"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="LblContactNum" runat="server" Text="Contact Number" Font-Bold="True"></asp:Label>
            <asp:TextBox ID="TbContactNum" runat="server" CssClass="form-control" placeholder="Contact Number" TextMode="Phone"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="LblEmergencyNum" runat="server" Text="Emergency Contact Number" Font-Bold="True"></asp:Label>
            <asp:TextBox ID="TbEmergencyNum" runat="server" CssClass="form-control" placeholder="Emergency Contact Number" TextMode="Phone"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="LblAddress" runat="server" Text="Address" Font-Bold="True"></asp:Label>
            <asp:TextBox ID="TbAddress" runat="server" CssClass="form-control" placeholder="Current Address"></asp:TextBox>
        </div>

        <div class="form-group">
            <h1>Under 18s Only</h1>
            <asp:Label ID="LblGuardianName" runat="server" Text="Guardian Name" Font-Bold="True"></asp:Label>
            <asp:TextBox ID="TbGuardianName" runat="server" CssClass="form-control" placeholder="Guardian Name"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label ID="LblGuardianNumber" runat="server" Text="Guardian Number" Font-Bold="True"></asp:Label>
            <asp:TextBox ID="TbGuardianNumber" runat="server" CssClass="form-control" placeholder="Guardian Number" TextMode="Phone"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="BtnSubmit" class="btn btn-primary btn-block" runat="server" Text="Submit" OnClick="BtnSubmit_Click" />
        </div>

 <%--       <fieldset class="form-group">
            <legend>Radio buttons</legend>
            <div class="form-check">
                <label class="form-check-label">
                    <input type="radio" class="form-check-input" name="optionsRadios" id="optionsRadios1" value="option1" checked="checked" />
                    Option one is this and that&mdash;be sure to include why it's great
                </label>
            </div>
            <div class="form-check">
                <label class="form-check-label">
                    <input type="radio" class="form-check-input" name="optionsRadios" id="optionsRadios2" value="option2" />
                    Option two can be something else and selecting it will deselect option one
                </label>
            </div>
            <div class="form-check disabled">
                <label class="form-check-label">
                    <input type="radio" class="form-check-input" name="optionsRadios" id="optionsRadios3" value="option3" disabled="disabled" />
                    Option three is disabled
                </label>
            </div>
        </fieldset>
        <div class="form-check">
            <label class="form-check-label">
                <input type="checkbox" class="form-check-input">
                Check me out
            </label>
        </div>
        <button type="submit" class="btn btn-primary">Submit</button>--%>
    </div>


</asp:Content>

