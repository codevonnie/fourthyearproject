<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Arivals.aspx.cs" Inherits="Web_SignInPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="container-fluid modal-dialog">
        <div class="modal-content ">
            <div class="modal-header">
                <h1 class="text-center">Sign Member In</h1>
            </div>
            <div class="modal-body">

                <asp:Label runat="server" Text="Scan Members QR Code" Font-Bold="True"></asp:Label>
                <div class="form-group input-group input-group-lg">
                    <span class="input-group-addon">
                        <i class="fa fa-barcode iconWidth"></i>
                    </span>
                    <asp:TextBox ID="TbQRCode" runat="server" class="form-control" placeholder="BarCode OR ID" TextMode="SingleLine"></asp:TextBox>
                </div>
            </div>

            <div class="modal-footer">
                <div class="form-group">
                    <asp:Button ID="BtnCheckMember" class="btn btn-primary btn-block btn-lg" runat="server" Text="Check Member" OnClick="BtnCheckMember_Click" />
                </div>
            </div>
        </div>
    </div>



    <br />
    <br />
    <br />

    <h1>IF(RESPONSE = SUCCESS)</h1>
    DISPLAY USERS DETAILS
      <br />
    <br />

    <div class="container-fluid modal-dialog">
        <div class="modal-content ">
            <div class="modal-header">
                <h1 class="text-center">Member Name</h1>
            </div>
            <div class="modal-body text-center">
                <br />
                <div class="text-muted">Image Below</div>              
                <img src="data:image/gif;base64,R0lGODlhAQABAIAAAHd3dwAAACH5BAAAAAAALAAAAAABAAEAAAICRAEAOw==" width="200" height="200" class="img-responsive alignCenter" alt="Generic placeholder thumbnail" />
                <br />
                 <div class="text-muted">DOB- Age or dd/mm/yyyy</div>
                <div class="text-muted">Emergency Num - 56789</div>
                <div class="text-muted">Something else</div>
                <br />
            </div>
        <div class="modal-footer ">
                <button type="button" class="btn btn-primary btn-sm">More</button>
            <button type="button" class="btn btn-primary btn-sm">AnotherBtn</button>
            </div>
        </div>
    </div>







</asp:Content>

