<%@ Page Title="DashBoard" Language="C#" MasterPageFile="/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script src="https://d3js.org/d3.v3.min.js"></script>

    <script src="../Scripts/BarChart.js"></script>

    <script>
        //Bind Server Data to js
        var arr = <%=VisitData%>;
    </script>


    <div class="text-center alignCenter HalfWidth">

        <div class="btn-group btn-group-lg TxtAreaSize BtnGroupDefault" role="group">
            <button type="button" class="btn btn-info BtnDefaultPage" onclick="DisplayArivals()">Arrivals</button>
            <button type="button" id="BtnTopVisited" runat="server" onserverclick="BtnTopVisited_Click" class="btn btn-info BtnDefaultPage">Top Visitors</button>
            <button type="button" id="BtnLeastRecent" runat="server" onserverclick="BtnTopVisited_Click" class="btn btn-info BtnDefaultPage">Least Recent</button>
            <button type="button" id="BtnBarChart" class="btn btn-info BtnDefaultPage" onserverclick="BtnBarChart_ServerClick" runat="server">Bar Chart</button>
        </div>

        <h1 id="HeaderTitle" class="page-header HeaderColour"></h1>

        <div id="ArivalsData" class="row placeholders text-capitalize">
        </div>
    </div>
    <div id="chart" class="center-block"></div>


    <%--Modal Top Ten--%>
    <div id="TopTenModal" style="display: none;">
        <div class="modal-dialog">
            <div class="modal-content ">
                <div class="modal-header" id="modalHeader" runat="server">

                </div>

                <%--Modal Body--%>
                <div class="modal-body">
                    <ul class="list-group" id="UlTopChart" runat="server">
                    </ul>
                </div>
            </div>
        </div>
    </div>


    <%--Get Arivals from local storage--%>
    <script src="../Scripts/DisplayArivals.js"></script>

    <script>
        function TopTenToggle() {
            var x = document.getElementById('TopTenModal');
            if (x.style.display === 'none') {
                x.style.display = 'block';
            } else {
                x.style.display = 'none';
            }
        }
    </script>

</asp:Content>

