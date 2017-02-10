<%@ Page Title="DashBoard" Language="C#" MasterPageFile="/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField runat="server" ID="HfCustomer" />


    <script type="text/javascript">
        if (localStorage) {

            var lblBrand = document.getElementById('<%= HfCustomer.ClientID %>');

            var custList = JSON.parse(localStorage.getItem("TodaysArivals"));
            console.log(custList);
            if (custList != null) {
                $.ajax({
                    type: "POST",
                    url: "Default.aspx/ArivalsToList",
                    data: "{data:" + JSON.stringify(custList) + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",

                    success: function (response) {
                        console.log("Success, Arivals Recived");
                    },
                    failure: function (response) {
                        alert(response.d);
                    }
                });
            }
        }
        else {
            alert("Your Browser Does Not Support This Feature")
        }
    </script>


    <div class="text-center alignCenter HalfWidth">

        <h1 class="page-header">Dashboard</h1>
        <asp:Button ID="BtnCurrentCustomers" class="btn btn-primary btn-block btn-lg" runat="server" OnClick="BtnCurrentCustomers_Click" Text="Stalling....." />
        <br />
         <br />
        <div id="PeoplePlaceHolder" runat="server" class="row placeholders text-capitalize">
        </div>

        <h2 class="sub-header">Data From Neo4j hosted on heroku</h2>
        <div class="table-responsive">
            <asp:DataGrid runat="server" ID="GridView1" AutoGenerateColumns="False" CellPadding="0" CssClass="table table-striped">
                <Columns>
                    <asp:BoundColumn HeaderText="Name" DataField="name" />
                    <asp:BoundColumn HeaderText="Address" DataField="address" />
                    <asp:BoundColumn HeaderText="Phone" DataField="phone" />
                    <asp:BoundColumn HeaderText="Email" DataField="email" />
                </Columns>
            </asp:DataGrid>
        </div>

    </div>
</asp:Content>

