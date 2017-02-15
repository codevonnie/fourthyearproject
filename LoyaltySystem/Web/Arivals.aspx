<%@ Page Title="Arivals" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Arivals.aspx.cs" Inherits="Arivals" %>

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
                    <asp:TextBox ID="TbQRCode" runat="server" class="form-control" placeholder="BarCode OR Email" TextMode="SingleLine"></asp:TextBox>
                </div>
            </div>

            <div class="modal-footer">
                <div class="form-group">

                    <div id="DivFailedScan" runat="server">
                        <div class="alert alert-danger text-center" role="alert">
                            <strong>Person Not Found Or Invalid Input!</strong>
                        </div>
                    </div>

                    <div id="DivConnectionErr" runat="server">
                        <div class="alert alert-danger text-center" role="alert">
                            <strong>OOPS! Connection Error - Reconnect Or Try Again Later!</strong>
                        </div>
                    </div>

                    <asp:Button ID="BtnCheckMember" class="btn btn-primary btn-block btn-lg" runat="server" Text="Check Member" OnClick="BtnCheckMember_Click" data-target="#myModal" />
                </div>
            </div>
        </div>
    </div>



    <!-- Modal -->
    <div class="modal fade MarginTop60" id="myModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">

        <!-- Content -->
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header text-center ModalHeadColor RoundTop">

                    <div class="dropdown" data-toggle="dropdown">
                        <button class="fa fa-cogs fa-2x floatLeft BtnInvis" id="DdmSettings" data-toggle="dropdown" aria-hidden="true" aria-haspopup="true" aria-expanded="false">
                            <span class="caret"></span>
                        </button>
                        <ul class="dropdown-menu MarginTop35 DropDownMenu">

                            <li class="text-center h5">Update Person</li>
                            <div class="DropDownContentColour">

                                <li>
                                    <asp:Button class="btn DropDownBtns btn-sm btn-block" ID="BtnUpName" runat="server" Text="Name" OnClick="UpdateChoice_Click" /></li>

                                <li>
                                    <asp:Button class="btn DropDownBtns btn-sm btn-block" ID="BtnUpEmail" runat="server" Text="Email" OnClick="UpdateChoice_Click" />
                                </li>

                                <li>
                                    <asp:Button class="btn DropDownBtns btn-sm btn-block" ID="BtnUpGuardName" runat="server" Text="Guardian Name" OnClick="UpdateChoice_Click" />
                                </li>

                                <li>
                                    <asp:Button class="btn DropDownBtns btn-sm btn-block" ID="BtnUpGuardNum" runat="server" Text="Guardian Number" OnClick="UpdateChoice_Click" />
                                </li>

                                <li>
                                    <asp:Button class="btn DropDownBtns btn-sm btn-block" ID="BthUpMembershipEndDate" runat="server" Text="Add New Membership" OnClick="UpdateChoice_Click" />
                                </li>

                                <li role="presentation" class="divider"></li>

                                <li>
                                    <div class="btn DropDownBtns DangerBtn btn-sm btn-block" id="DeleteDanger">Enable Danger Buttons</div>
                                </li>

                                <li>
                                    <asp:Button class="btn DropDownBtns DangerBtn btn-sm btn-block" disabled="true" ID="BtnRemoveGuard" runat="server" Text="Remove Guardian" OnClick="UpdatePersonInfo_Click" />
                                </li>
                                <li>
                                    <asp:Button class="btn DropDownBtns DangerBtn btn-sm btn-block" disabled="true" ID="BtnRemoveMembership" runat="server" Text="Remove Membership" OnClick="UpdatePersonInfo_Click" />
                                </li>

                                <li>
                                    <asp:Button class="btn DropDownBtns DangerBtn btn-sm btn-block" disabled="true" ID="BntResetPwd" runat="server" Text="Reset Password" OnClick="BntResetPwd_Click" />
                                </li>
                            </div>
                        </ul>
                    </div>

                    <asp:Label ID="LblName" runat="server" CssClass="h1 MarginRight100 text-capitalize"></asp:Label>

                    <div class="fa fa-times fa-2x CloseModal" id="BtnCloseModal" onclick="toggleModal()"></div>

                </div>

                <div class="modal-body text-center">

                    <div class="text-muted">
                        <b>Joined:</b>
                        <span>
                            <asp:Label ID="LblJoined" runat="server"></asp:Label>
                        </span>
                    </div>

                    <br />
                    <asp:Image ID="ImgPerson" runat="server" Width="200" Height="200" class="img-responsive alignCenter img-rounded" />
                    <br />

                    <div class="container-fluid LineHeight25 text-left maxWidthContainer">

                        <div id="MembershipControl" runat="server">
                            <div class="text-muted">
                                <b>MemberShip Finished: </b>
                                <span>
                                    <asp:Label ID="LblMember" runat="server"></asp:Label>
                                </span>
                            </div>
                        </div>

                        <div class="text-muted">
                            <b>Email:</b>
                            <span>
                                <asp:Label ID="LblEmail" runat="server"></asp:Label>
                            </span>
                        </div>

                        <div class="text-muted">
                            <b>Age:</b>
                            <span>
                                <asp:Label ID="LblAge" runat="server"></asp:Label>
                            </span>
                        </div>

                        <div class="text-muted">
                            <b>IceName: </b>
                            <span>
                                <asp:Label ID="LblIceName" runat="server"></asp:Label>
                            </span>
                        </div>

                        <div class="text-muted">
                            <b>IceNum: </b>
                            <span>
                                <asp:Label ID="LblIceNum" runat="server"></asp:Label>
                            </span>
                        </div>

                        <div id="HideVisited" runat="server">
                            <div class="text-muted">
                                <b>Times Visited: </b>
                                <span>
                                    <asp:Label ID="LblTimesVisited" runat="server"></asp:Label>
                                </span>
                            </div>
                        </div>

                        <div id="GuardName" runat="server">
                            <div class="text-muted">
                                <b>Guardian Name:</b>
                                <span>
                                    <asp:Label ID="LblGuardName" runat="server"></asp:Label>
                                </span>
                            </div>
                        </div>
                        <div id="GuardNum" runat="server">
                            <div class="text-muted">
                                <b>Guardian Num:</b>
                                <span>
                                    <asp:Label ID="LblGuardNum" runat="server"></asp:Label>
                                </span>
                            </div>
                        </div>

                         <div id="LastVisited" runat="server">
                            <div class="text-muted">
                                <b>Last Time Visited:</b>
                                <span>
                                    <asp:Label ID="LblLastVisited" runat="server"></asp:Label>
                                </span>
                            </div>
                        </div>
                        <!--HideGuard-->
                    </div>
                    <!--Container-->

                    <br />

                    <div id="DivDisplay" runat="server">
                        <div class="form-group input-group has-danger" id="update-form">
                            <span class="input-group-addon">
                                <asp:Button class="btn btn-danger btn-sm btn-block" ID="BtnChange" runat="server" Text="Change" OnClick="UpdatePersonInfo_Click" />
                            </span>
                            <asp:TextBox ID="TbUpdate" runat="server" CssClass="form-control BoxHeight form-control-danger"></asp:TextBox>
                        </div>
                    </div>

                    <div id="DivSuccess" runat="server">
                        <div class="alert alert-success text-center" role="alert">
                            <strong>Update Successful!</strong>
                        </div>
                    </div>

                        <div id="DivTempPwd" class="alert alert-success text-center" runat="server" role="alert">
    <%--                        <strong>Update Successful!</strong>--%>
                        </div>

                    <div id="DivSuccessCheckIn" runat="server">
                        <div class="alert alert-success text-center" role="alert">
                            <strong>Success! Person Checked In</strong>
                        </div>
                    </div>

                    <div id="DivFailed" runat="server">
                        <div class="alert alert-danger text-center" role="alert">
                            <strong>Update Failed!</strong>
                        </div>
                    </div>

                    <div id="DivFailedEmail" runat="server">
                        <div class="alert alert-danger text-center" role="alert">
                            <strong>Update Failed! Email Already In Use. Try Another</strong>
                        </div>
                    </div>


                </div>
                <!--Modal Footer-->
                <div class="modal-footer RoundBottom">
                    <asp:Button class="btn btn-block btn-success btn-lg" ID="BtnCheckin" runat="server" Text="Check Person In" OnClick="UpdatePersonInfo_Click" />
                </div>

                <script type="text/javascript">  
                   
                    //Toggle The Modal Per Person
                    function toggleModal() {
                        $('#myModal').modal('toggle');
                    };

                    //Enables The Delete buttons in the DDL
                    $('#DeleteDanger').on('click', function (event) {
                        event.preventDefault(); // To prevent following the link (optional)
                        document.getElementById('<%=BtnRemoveGuard.ClientID%>').disabled = false;
                        document.getElementById('<%=BtnRemoveMembership.ClientID %>').disabled = false;
                        document.getElementById('<%=BntResetPwd.ClientID %>').disabled = false;
                    });

                    //Stops The Drop Down From Closing On Clicks inside The DDL
                    $('.dropdown-menu').click(function (e) {
                        e.stopPropagation();
                    });


                     var check = "<%=NewPerson %>";

                    console.log("Check For New Person: ", check);
                    if (check === "True") {
                        var cust = <%=GetCustomer%>;
                        var custList = new Array();

                        custObj = cust//Parse the data

                        var arvlObj = {};//Create a New Object Each Time an Arival Comes In
                        arvlObj.email = custObj.email;
                        arvlObj.name = custObj.name;
                        arvlObj.dob = custObj.dob;
                        arvlObj.phone = custObj.phone;
                        arvlObj.imgUrl = custObj.imgUrl;
                        arvlObj.joined = custObj.joined;
                        arvlObj.iceName = custObj.iceName;
                        arvlObj.icePhone = custObj.icePhone;
                        arvlObj.visited = custObj.visited;
                        arvlObj.membership = custObj.membership;
                        arvlObj.day = Date.now();

                        //push the obj to an Array
                        custList.push(arvlObj);

                        //CHECK local Storage for other Arivals
                        var ListOfCust =JSON.parse(localStorage.getItem("TodaysArivals"));
                        
                        //Loop over all the Objects From Local Storage
                        if(ListOfCust!=null){
                            //Check For New Arivals Only and add to Array
                            ListOfCust.forEach(function (entry) {
                                //Removes The Local Storage Every Day
                                if(arvlObj.day < entry.day){
                                    return;
                                }
                                if (entry.name != arvlObj.name)
                                    custList.push(entry);
                            });                          
                        }

                        //Store The Array as a JSON.stringify in local storage / over write if all ready there
                        localStorage.setItem("TodaysArivals", JSON.stringify(custList));
                    }

                </script>
</asp:Content>

