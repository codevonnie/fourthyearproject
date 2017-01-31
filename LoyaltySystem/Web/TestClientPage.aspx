<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TestClientPage.aspx.cs" Inherits="Web_TestClientPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">




                <!-- Content -->
                <div class="modal-dialog">
                    <div class="modal-content ">

                        <div class="modal-header text-center padRight65 ModalHeadColor RoundTop">
                            <asp:Label ID="LblName" runat="server" Text="Persons Name" CssClass="h1"></asp:Label>
                        </div>

                        <!--Modal Body-->
                        <div class="modal-body text-center">
                            <br />
                            <asp:TextBox ID="TaMessageBox" TextMode="multiline" CssClass="TxtAreaSize" runat="server" ClientIDMode="Static" />
                        </div>

                        <!--Modal Footer-->
                        <div class="modal-footer RoundBottom">
                            FOOTER
                        </div>

                    </div>
                </div>

</asp:Content>

