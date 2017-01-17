<%@ Page Title="" Language="C#" MasterPageFile="/MasterPage.master" AutoEventWireup="true" CodeFile="AddMember.aspx.cs" Inherits="Web_Customer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="modal-dialog">
        <div class="modal-content textFontFamilyBody">
            
            <div class="modal-header customerHeader">
                <h1 class="text-center">Create New Member</h1>
            </div>
            <div class="modal-body">
                <div class="container-fluid">
                    
                    <asp:Label ID="LblName" runat="server" Text="Name Of Participant" Font-Bold="True"></asp:Label>
                    <div class="form-group input-group input-group">
                        <span class="input-group-addon">
                            <i class="fa fa-id-card-o iconWidth" aria-hidden="true"></i>
                        </span>
                        <asp:TextBox ID="TbName" runat="server" CssClass="form-control" placeholder="Enter Name"></asp:TextBox>
                    </div>

                    <asp:Label ID="LblGender" runat="server" Text="Sex" Font-Bold="True"></asp:Label>
                    <div class="form-group input-group input-group">
                        <span class="input-group-addon">
                            <i class="fa fa-venus-mars iconWidth" aria-hidden="true"></i>
                        </span>
                        <asp:TextBox ID="TbGender" runat="server" CssClass="form-control" placeholder="Male/Female"></asp:TextBox>
                    </div>

                    <asp:Label ID="LblEmail" runat="server" Text="Email Address" Font-Bold="True"></asp:Label>
                    <div class="form-group input-group input-group">
                        <span class="input-group-addon">
                            <i class="fa fa-envelope-o iconWidth" aria-hidden="true"></i>
                        </span>
                        <asp:TextBox ID="TbEmail" runat="server" CssClass="form-control" placeholder="Email Address" TextMode="Email"></asp:TextBox>
                    </div>

                    <asp:Label ID="LblDob" runat="server" Text="Date Of Birth" Font-Bold="True"></asp:Label>
                    <div class="form-group input-group input-group">
                        <span class="input-group-addon">
                            <i class="fa fa-birthday-cake iconWidth" aria-hidden="true"></i>
                        </span>
                        <asp:TextBox ID="TbDob" runat="server" CssClass="form-control" placeholder="Date Of Birth" TextMode="Date"></asp:TextBox>
                    </div>

                    <asp:Label ID="LblContactNum" runat="server" Text="Contact Number" Font-Bold="True"></asp:Label>
                    <div class="form-group input-group input-group">
                        <span class="input-group-addon ">
                            <i class="fa fa-phone iconWidth" aria-hidden="true"></i>
                        </span>
                        <asp:TextBox ID="TbContactNum" runat="server" CssClass="form-control" placeholder="Contact Number" TextMode="Phone"></asp:TextBox>
                    </div>

                    <asp:Label ID="LblEmergencyNum" runat="server" Text="Emergency Contact Number" Font-Bold="True"></asp:Label>
                    <div class="form-group input-group input-group">
                        <span class="input-group-addon">
                            <i class="fa fa-phone iconWidth" aria-hidden="true"></i>
                        </span>
                        <asp:TextBox ID="TbEmergencyNum" runat="server" CssClass="form-control" placeholder="Emergency Contact Number" TextMode="Phone"></asp:TextBox>
                    </div>
                    <asp:Label ID="LblEmergencyName" runat="server" Text="Emergency Contact Name" Font-Bold="True"></asp:Label>
                    <div class="form-group input-group input-group">
                        <span class="input-group-addon" >
                            <i class="fa fa-user-plus iconWidth" aria-hidden="true"></i>
                        </span>
                        <asp:TextBox ID="TbEmergencyName" runat="server" CssClass="form-control" placeholder="Emergency Contact Name"></asp:TextBox>
                    </div>

                    <asp:Label ID="LblAddress" runat="server" Text="Address" Font-Bold="True"></asp:Label>
                    <div class="form-group input-group input-group">
                        <span class="input-group-addon">
                            <i class="fa fa-map-marker iconWidth" aria-hidden="true"></i>
                        </span>
                        <asp:TextBox ID="TbAddress" runat="server" CssClass="form-control" placeholder="Current Address"></asp:TextBox>
                    </div>

                    <h1 class="text-center">Under 18s Only</h1>
                    <asp:Label ID="LblGuardianName" runat="server" Text="Guardian Name" Font-Bold="True"></asp:Label>
                    <div class="form-group input-group input-group">
                        <span class="input-group-addon">
                            <i class="fa fa-user-circle iconWidth" aria-hidden="true"></i>
                        </span>
                        <asp:TextBox ID="TbGuardianName" runat="server" CssClass="form-control" placeholder="Guardian Name"></asp:TextBox>
                    </div>

                    <asp:Label ID="LblGuardianNumber" runat="server" Text="Guardian Number" Font-Bold="True"></asp:Label>
                    <div class="form-group input-group input-group">
                        <span class="input-group-addon">
                            <i class="fa fa-phone iconWidth" aria-hidden="true"></i>
                        </span>
                        <asp:TextBox ID="TbGuardianNumber" runat="server" CssClass="form-control" placeholder="Guardian Number" TextMode="Phone"></asp:TextBox>
                    </div>
                </div>
            </div>

            <div class="modal-footer">
                <div class="form-group">
                    <asp:Button ID="BtnSubmit" class="btn btn-primary btn-block btn-lg" runat="server" Text="Submit" OnClick="BtnSubmit_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>

