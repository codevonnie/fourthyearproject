<%@ Page Title="" Language="C#" MasterPageFile="/MasterPage.master" AutoEventWireup="true" CodeFile="AddMember.aspx.cs" Inherits="AddMember" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <script type="text/javascript">
        $(document).ready(function () {
            $('#AddPersonForm').bootstrapValidator({
                container: '#messages',
                feedbackIcons: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {

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
                    <%=TbEmail.UniqueID%>: {
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

                    // -- DOB --
                    <%=TbDob.UniqueID%>: {
                        validators: {
                            notEmpty: {
                                message: 'DOB is required and cannot be empty'
                            },
                            date: {
                                format: 'DD/MM/YYYY',
                                message: 'The value is not a valid date'
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

                    // -- EMERGENCY NAME --
                    <%=TbEmergencyName.UniqueID%>: {
                        validators: {
                            notEmpty: {
                                message: 'Emergency Name is required and cannot be empty'
                            },
                            stringLength: {
                                max: 50,
                                message: 'Emergency Name must be less Than 50 Characters Long'
                            }
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
                    
                    // -- FILE UPLOAD --
                    FileInput: {
                        validators: {
                            notEmpty: {
                                message: 'Name is required and cannot be empty'
                            },
                            maxFiles:{
                                max: 1,
                                message: 'Only One Image Can Be uploaded At A Time'
                            },
                            file: {
                                extension: 'jpeg,jpg,png',// Jpg,Jpeg,Png only
                                type: 'image/jpeg,image/png',
                                maxSize: 2097152,   // 2048 * 1024 8MB Important as cloudinary is 10mb max
                                message: 'The selected file is not valid.<br/>JPG/JPEG/PNG Only<br/>File Size Limit: 10Mb'
                            }
                        }
                    },   

                    // -- GUARDIAN NUM --
                    <%=TbGuardianNumber.UniqueID%>: {
                        validators: {
                            regexp: {
                                regexp: '^[0-9]*$',
                                message: 'Guardian Number Can Only Consist of Numbers'
                            },
                        }
                    },   

                    // -- GUARDIAN NAME --
                    <%=TbGuardianName.UniqueID%>: {
                        validators: {
                            stringLength: {
                                max: 50,
                                message: 'Guardian Name Must Be Less Than 50 Characters Long'
                            }
                        }
                    },   
                }
            });
        });
    </script>


    <div id="AddPersonForm">
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

                        <asp:Label ID="LblEmail" runat="server" Text="Email Address" Font-Bold="True"></asp:Label>
                        <div class="form-group input-group input-group">
                            <span class="input-group-addon">
                                <i class="fa fa-envelope-o iconWidth" aria-hidden="true"></i>
                            </span>
                            <asp:TextBox ID="TbEmail" runat="server" CssClass="form-control" placeholder="Email Address" TextMode="Email" AutoCompleteType="Email"></asp:TextBox>
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
                            <asp:TextBox ID="TbContactNum" runat="server" CssClass="form-control" placeholder="Contact Number"></asp:TextBox>
                        </div>

                        <asp:Label ID="LblEmergencyNum" runat="server" Text="Emergency Contact Number" Font-Bold="True"></asp:Label>
                        <div class="form-group input-group input-group">
                            <span class="input-group-addon">
                                <i class="fa fa-phone iconWidth" aria-hidden="true"></i>
                            </span>
                            <asp:TextBox ID="TbEmergencyNum" runat="server" CssClass="form-control" placeholder="Emergency Contact Number"></asp:TextBox>
                        </div>

                        <asp:Label ID="LblEmergencyName" runat="server" Text="Emergency Contact Name" Font-Bold="True"></asp:Label>
                        <div class="form-group input-group input-group">
                            <span class="input-group-addon">
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

                        <asp:Label ID="LblUpload" runat="server" Text="Person's Image" Font-Bold="True"></asp:Label>
                        <div class="form-group input-group input-group boxArea">
                            <div class="fileinput fileinput-new input-group" data-provides="fileinput">

                                <span class="input-group-addon">
                                    <i class="fa fa-upload iconWidth" aria-hidden="true"></i>
                                </span>

                                <div class="form-control" data-trigger="fileinput">
                                    <i class="glyphicon glyphicon-file fileinput-exists"></i>
                                    <div class="fileinput-filename" runat="server"></div>
                                </div>

                                <span class="input-group-addon btn btn-default btn-file">
                                    <span class="fileinput-new">Select file</span>
                                    <span class="fileinput-exists">Change</span>
                                    <input type="file" name="..." runat="server" id="ImagePath" />
                                </span>

                                <a href="#" class="input-group-addon btn btn-default fileinput-exists" data-dismiss="fileinput">Remove</a>
                            </div>
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

                        <div class="form-group text-center">
                            <div id="messages"></div>
                        </div>

                    </div>
                </div>

                <div class="modal-footer">
                    <div class="form-group">
                        <div id="DivSuccess" runat="server">
                            <div class="alert alert-success text-center" role="alert">
                                <strong>New Member Created!</strong>
                            </div>
                        </div>

                        <div id="DivFailed" runat="server">
                            <div class="alert alert-danger text-center" role="alert">
                                <strong>OOPS! Something Went Wrong, Please Try Agin Later.</strong>
                            </div>
                        </div>
                        <asp:Button ID="BtnSubmit" class="btn btn-primary btn-block btn-lg" runat="server" Text="Submit" OnClick="BtnSubmit_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

