<%@ Page Title="DashBoard" Language="C#" MasterPageFile="/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="text-center alignCenter HalfWidth">

        <h1 class="page-header">Dashboard</h1>


        <div id="ArivalsData" class="row placeholders text-capitalize">
        </div>


        <h2 class="sub-header">Data From Neo4j hosted on heroku</h2>
        <div class="table-responsive">
            <asp:datagrid runat="server" id="GridView1" autogeneratecolumns="False" cellpadding="0" cssclass="table table-striped">
                <Columns>
                    <asp:BoundColumn HeaderText="Name" DataField="name" />
                    <asp:BoundColumn HeaderText="Address" DataField="address" />
                    <asp:BoundColumn HeaderText="Phone" DataField="phone" />
                    <asp:BoundColumn HeaderText="Email" DataField="email" />
                </Columns>
            </asp:datagrid>
        </div>
    </div>

    <%--Get Arivals from local storage--%>
    <script src="../Scripts/Arivals.js"></script>
</asp:Content>

