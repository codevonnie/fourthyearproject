<%@ Page Title="Message Map" Language="C#" MasterPageFile="/MasterPage.master" AutoEventWireup="true" CodeFile="Map.aspx.cs" Inherits="Map" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!--Allows access to Master Page Methods *IMPORTANT*-->
    <%@ MasterType VirtualPath="~/MasterPage.Master" %>

    <h1 class="text-center">Google Maps</h1>

    <!-- Js attaches to the Div id map-->
    <div id="map"></div>

    <%--Called From Web Config File--%>
    <script src="../Scripts/CustGoogleJs.js"></script>
    
</asp:Content>

