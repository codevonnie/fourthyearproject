<%@ Page Title="" Language="C#" MasterPageFile="/MasterPage.master" AutoEventWireup="true" CodeFile="Map.aspx.cs" Inherits="Map" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">



    <h1 class="text-center">Google Maps</h1>

    <!-- Js attaches to the Div id map-->
    <div id="map"></div>

    <!-- Used To Add Js File to Page Via Programatically-->
    <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>

    <!-- HardCodded Api Key Src -->
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyAaJMX4i5TEKA3UL5I7DArt42MRSxqg4LI&callback=initMap"> </script>


</asp:Content>

