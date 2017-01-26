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
                    <asp:Button ID="BtnCheckMember" class="btn btn-primary btn-block btn-lg" runat="server" Text="Check Member" OnClick="BtnCheckMember_Click" data-target="#myModal" />
                </div>
            </div>
        </div>
    </div>



    <!-- Modal -->
    <div class="modal fade MarginTop60" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">

        <!-- Content -->
        <div class="modal-dialog">
            <div class="modal-content ">

                <div class="modal-header text-center padRight65 ModalHeadColor RoundTop">

                    <div class="dropdown">
                        <button class="fa fa-cogs fa-2x floatLeft BtnInvis" id="DdmSettings" data-toggle="dropdown" aria-hidden="true" aria-haspopup="true" aria-expanded="false">
                            <span class="caret"></span>
                        </button>
                        <ul class="dropdown-menu MarginTop35">
                            <li>
                                <asp:Button class="btn DropDownBtns btn-sm btn-block" ID="BtnUpName" runat="server" Text="Name" OnClick="UpdateChoice_Click" /></li>
                            <li>
                                <asp:Button class="btn DropDownBtns btn-sm btn-block" ID="BtnUpGuardName" runat="server" Text="Guardian Name" OnClick="UpdateChoice_Click" /></li>
                            <li>
                                <asp:Button class="btn DropDownBtns btn-sm btn-block" ID="BtnUpGuardNum" runat="server" Text="Guardian Number" OnClick="UpdateChoice_Click" /></li>
                            <li>
                                <asp:Button class="btn DropDownBtns btn-sm btn-block" ID="BtnUpEmail" runat="server" Text="Email" OnClick="UpdateChoice_Click" /></li>
                        </ul>
                    </div>

                    <asp:Label ID="LblName" runat="server" Text="Scott Coyne" CssClass="h1"></asp:Label>
                </div>

                <div class="modal-body text-center">
                    <br />
                    <asp:Image ID="ImgPerson" runat="server" Width="200" Height="200" class="img-responsive alignCenter img-rounded" ImageUrl="http://images.digopaul.com/wp-content/uploads/related_images/2015/09/08/ape_2.jpg" />

                    <br />

                    <div class="container-fluid text-left maxWidthContainer">

                        <div class="text-muted">
                            Joined:
                <span>
                    <asp:Label ID="LblJoined" runat="server" Text="14/12/1988"></asp:Label></span>
                        </div>

                        <div class="text-muted">
                            Age:
                <span>
                    <asp:Label ID="LblAge" runat="server" Text="28"></asp:Label></span>
                        </div>

                        <div class="text-muted">
                            IceName:
                <span>
                    <asp:Label ID="LblIceName" runat="server" Text="Mary"></asp:Label></span>
                        </div>

                        <div class="text-muted">
                            IceNum:
                <span>
                    <asp:Label ID="LblIceNum" runat="server" Text="180696969"></asp:Label></span>
                        </div>


                        <div class="text-muted">
                            MemberShip:
                <span>
                    <asp:Label ID="LblMember" runat="server" Text="No"></asp:Label></span>
                        </div>

                        <div id="HideGuard" runat="server">
                            <div class="text-muted">
                                Guardian Name:
                <span>
                    <asp:Label ID="LblGuardName" runat="server" Text="Null / Tim"></asp:Label></span>
                            </div>


                            <div class="text-muted">
                                Guardian Num:
                <span>
                    <asp:Label ID="LblGuardNum" runat="server" Text="Null / 567890876"></asp:Label></span>
                            </div>
                        </div>
                        <!--HideGuard-->
                    </div>
                    <!--Container-->

                    <br />

                    <div id="divDisplay" runat="server">
                        <div class="form-group input-group has-danger" id="update-form">
                            <span class="input-group-addon">
                                <asp:Button class="btn btn-danger btn-sm btn-block" ID="BtnChange" runat="server" Text="Change" OnClick="UpdatePersonInfo_Click" />
                            </span>
                            <asp:TextBox ID="TbUpdate" runat="server" CssClass="form-control BoxHeight form-control-danger"></asp:TextBox>
                        </div>
                    </div>

                    <div id="DivSuccess" runat="server">
                        <div class="alert alert-success" role="alert">
                            <strong>Update Successful!</strong>
                        </div>
                    </div>

                    <div id="DivFailed" runat="server">
                        <div class="alert alert-danger" role="alert">
                            <strong>Update Failed!</strong>
                        </div>
                    </div>

                </div>
                <!--Modal Body-->
                <div class="modal-footer RoundBottom">
                    <asp:Button class="btn btn-block btn-success btn-lg" ID="BtnCheckin" runat="server" Text="Check Person In" OnClick="BtnCheckPersonIn_Click" />
                </div>
            </div>
        </div>
    </div>

    <!--Function called Via C# programaticaly to show Modal -->
    <script type="text/javascript">

        function openModal() {
            $('#myModal').modal('show');
        };      
    </script>



</asp:Content>

