﻿<%@ Page Title="" Language="C#" MasterPageFile="/MasterPage.master" AutoEventWireup="true" CodeFile="LoginPage.aspx.cs" Inherits="LoginPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script type="text/javascript">
        $(document).ready(function () {
            $('#LogInForm').bootstrapValidator({
                container: '#messages',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    <%=TbEmail.UniqueID%>: {
                        validators: {
                            notEmpty: {
                                message: 'The email Address is Required And Cannot Be Empty'
                            },
                            regexp: {
                                regexp: '^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$',
                                message: 'Email can only consist of alphabetical and numbers'
                            },
                            emailAddress: {
                                message: 'The Email Address is not valid'
                            }
                        }
                    },
                    <%=TbPassword.UniqueID%>: {
                        validators: {
                            notEmpty: {
                                message: 'The Password is Required And Cannot Be Empty'
                            },
                        }
                    },

                }
            });
        });
    </script>


    <div id="LogInForm">
        <div id="loginModal" aria-hidden="true">
            <br />
            <br />
            <div class="modal-dialog">
                <div class="modal-content ">
                    <div class="modal-header">
                        <h1 class="text-center">Login</h1>
                    </div>
                    <div class="modal-body">


                        <div class="form-group input-group input-group-lg">
                            <span class="input-group-addon">
                                <i class="fa fa-envelope iconWidth"></i>
                            </span>
                            <asp:TextBox ID="TbEmail" runat="server" class="form-control" placeholder="Email" TextMode="Email" AutoCompleteType="Email"></asp:TextBox>
                        </div>


                        <div class="form-group input-group input-group-lg">
                            <span class="input-group-addon">
                                <i class="fa fa-lock iconWidth"></i>
                            </span>
                            <asp:TextBox ID="TbPassword" runat="server" class="form-control" placeholder="Password" TextMode="Password"></asp:TextBox>
                        </div>

                        <div class="form-group text-center">
                            <div id="messages"></div>
                        </div>
                    </div>

                    <div class="modal-footer">

                        <div class="form-group">
                            <asp:Button class="btn btn-success btn-lg btn-block" ID="BtnSignIn" runat="server" Text="Sign In" OnClick="singInBtn_Click" />
                        </div>

                        <div class="form-group">
                            <asp:Button class="btn btn-info btn-lg btn-block" ID="BtnRegister" runat="server" Text="Register" />
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>

