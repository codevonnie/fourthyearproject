<%@ Page Title="Login" Language="C#" MasterPageFile="/MasterPage.master" AutoEventWireup="true" CodeFile="LoginPage.aspx.cs" Inherits="LoginPage" %>

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
                   
                    // -- Email --
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

                    // -- Password --
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

        $(document).ready(function () {
            $('#myModal').bootstrapValidator({
                container: '#errMeesages',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {

                    // -- PasswordAdd --
                    <%=TbPasswordNew.UniqueID%>: {
                        validators: {
                            notEmpty: {
                                message: 'The Password is Required And Cannot Be Empty'
                            },
                        }
                    },


                    // -- NAME --
                    <%=TbName.UniqueID%>: {
                        validators: {
                            notEmpty: {
                                message: 'Name is required and cannot be empty'
                            },
                            stringLength: {
                                max: 50,
                                message: 'Name must be less Than 50 Characters Long'
                            }
                        }
                    },


                    // -- EMAIL --
                    <%=TbEmailNew.UniqueID%>: {
                        validators: {
                            notEmpty: {
                                message: 'Email Address is required and cannot be empty'
                            },
                            regexp: {
                                regexp: '^[^@\\s]+@([^@\\s]+\\.)+[^@\\s]+$',
                                message: 'Email can only consist of alphabetical and numbers'
                            },
                            emailAddress: {
                                message: 'Email Address is not valid'
                            }
                        }
                    },

                    // -- CONTACT NUM --
                    <%=TbContactNum.UniqueID%>: {
                        validators: {
                            notEmpty: {
                                message: 'Contact Number is required and cannot be empty'
                            },
                            regexp: {
                                regexp: '^[0-9]*$',
                                message: 'Contact Number Can Only Consist of Numbers'
                            },
                        }
                    },


                    // -- EMERGENCY NUM --
                    <%=TbEmergencyNum.UniqueID%>: {
                        validators: {
                            notEmpty: {
                                message: 'Emergency Number is required and cannot be empty'
                            },
                            regexp: {
                                regexp: '^[0-9]*$',
                                message: 'Emergency Number Can Only Consist of Numbers'
                            },
                        }
                    },  


                    // -- ADDRESS --
                    <%=TbAddress.UniqueID%>: {
                        validators: {
                            notEmpty: {
                                message: 'Address is required and cannot be empty'
                            },
                            stringLength: {
                                max: 70,
                                message: 'Address must be less Than 70 Characters Long'
                            }
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

                    <%--Modal Body--%>
                    <div class="modal-body">
                        <%--Email--%>
                        <div class="form-group input-group input-group-lg">
                            <span class="input-group-addon">
                                <i class="fa fa-envelope iconWidth"></i>
                            </span>
                            <asp:TextBox ID="TbEmail" runat="server" class="form-control" placeholder="Email" TextMode="Email" AutoCompleteType="Email"></asp:TextBox>
                        </div>

                        <%--Password--%>
                        <div class="form-group input-group input-group-lg">
                            <span class="input-group-addon">
                                <i class="fa fa-lock iconWidth"></i>
                            </span>
                            <asp:TextBox ID="TbPassword" runat="server" CssClass="form-control" placeholder="Password" TextMode="Password"></asp:TextBox>
                        </div>

                        <%--Error Messages--%>
                        <div class="form-group text-center">
                            <div id="messages"></div>
                        </div>

                        <%-- Failed Log In--%>
                        <div id="DivFailed" runat="server">
                            <div class="alert alert-danger text-center" role="alert">
                                <strong>OOPS! Invalid Login Credentials, Please Try Again.. </strong>
                            </div>
                        </div>

                        <%-- No Connection--%>
                        <div id="DivConnectionErr" runat="server">
                            <div class="alert alert-danger text-center" role="alert">
                                <strong>OOPS! Connection Error - Reconnect Or Try Again Later! </strong>
                            </div>
                        </div>
                    </div>

                    <%--Footer--%>
                    <div class="modal-footer">
                        <div class="form-group">
                            <asp:Button CssClass="btn btn-success btn-lg btn-block" ID="BtnSignIn" runat="server" Text="Sign In" OnClick="singInBtn_Click" />
                        </div>

                        <div class="form-group">
                            <button class="btn btn-info btn-lg btn-block" id="BtnRegister" onclick="toggleModal();return false" data-target="#myModal">Register</button>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>


    <!-- Modal -->
    <div class="modal fade MarginTop60" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div id="AddPersonForm">
            <div class="modal-dialog">
                <div class="modal-content textFontFamilyBody">

                    <%-- Custome Modal Header--%>
                    <div class="modal-header customerHeader">
                        <h1 class="text-center">Company Registration<span class="CloseModal">
                            <span class="fa fa-times" id="BtnCloseModal" onclick="toggleModal()"></span>
                        </span></h1>
                    </div>

                    <div class="modal-body">
                        <div class="container-fluid">

                            <%-- Name--%>
                            <asp:Label ID="LblName" runat="server" Text="Company Name" Font-Bold="True"></asp:Label>
                            <div class="form-group input-group input-group">
                                <span class="input-group-addon">
                                    <i class="fa fa-id-card-o iconWidth" aria-hidden="true"></i>
                                </span>
                                <asp:TextBox ID="TbName" runat="server" CssClass="form-control" placeholder="Name"></asp:TextBox>
                            </div>

                            <%-- Email--%>
                            <asp:Label ID="LblEmail" runat="server" Text="Email Address" Font-Bold="True"></asp:Label>
                            <div class="form-group input-group input-group">
                                <span class="input-group-addon">
                                    <i class="fa fa-envelope-o iconWidth" aria-hidden="true"></i>
                                </span>
                                <asp:TextBox ID="TbEmailNew" runat="server" CssClass="form-control" placeholder="Email Address" TextMode="Email" AutoCompleteType="Email"></asp:TextBox>
                            </div>

                            <%-- Pwd--%>
                            <asp:Label ID="LblPassword" runat="server" Text="Password" Font-Bold="True"></asp:Label>
                            <div class="form-group input-group input-group">
                                <span class="input-group-addon">
                                    <i class="fa fa-id-card-o iconWidth" aria-hidden="true"></i>
                                </span>
                                <asp:TextBox ID="TbPasswordNew" runat="server" TextMode="Password" CssClass="form-control" placeholder="Password"></asp:TextBox>
                            </div>

                            <%-- Num--%>
                            <asp:Label ID="LblContactNum" runat="server" Text="Contact Number" Font-Bold="True"></asp:Label>
                            <div class="form-group input-group input-group">
                                <span class="input-group-addon ">
                                    <i class="fa fa-phone iconWidth" aria-hidden="true"></i>
                                </span>
                                <asp:TextBox ID="TbContactNum" runat="server" CssClass="form-control" placeholder="Contact Number"></asp:TextBox>
                            </div>

                            <%-- Enum--%>
                            <asp:Label ID="LblEmergencyNum" runat="server" Text="Emergency Contact Number" Font-Bold="True"></asp:Label>
                            <div class="form-group input-group input-group">
                                <span class="input-group-addon">
                                    <i class="fa fa-phone iconWidth" aria-hidden="true"></i>
                                </span>
                                <asp:TextBox ID="TbEmergencyNum" runat="server" CssClass="form-control" placeholder="Emergency Contact Number"></asp:TextBox>
                            </div>

                            <%-- Add--%>
                            <asp:Label ID="LblAddress" runat="server" Text="Address" Font-Bold="True"></asp:Label>
                            <div class="form-group input-group input-group">
                                <span class="input-group-addon">
                                    <i class="fa fa-map-marker iconWidth" aria-hidden="true"></i>
                                </span>
                                <asp:TextBox ID="TbAddress" runat="server" CssClass="form-control" placeholder="Current Address"></asp:TextBox>
                            </div>


                            <%-- Failed New Comp--%>
                            <div id="DivFailedNewComp" runat="server" class="alert alert-danger text-center" role="alert">
                            </div>

                             <%-- Success New Comp--%>
                            <div id="DivSuccess" runat="server" class="alert alert-success text-center" role="alert">
                            </div>
                        </div>
                    </div>

                    <!--Modal Footer-->
                    <div class="modal-footer RoundBottom">
                        <asp:Button CssClass="btn btn-block btn-success btn-lg" ID="BtnSubmitCompany" runat="server" Text="Submit" OnClick="BtnCheckCompanyIn_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>


    <!--Function called Via C# programaticaly to show Modal -->
    <script type="text/javascript">
        function toggleModal() {
            $('#myModal').modal('toggle');
        };      
    </script>

</asp:Content>

