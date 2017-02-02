<%@ Page Title="" Language="C#" MasterPageFile="/MasterPage.master" AutoEventWireup="true" CodeFile="Map.aspx.cs" Inherits="Map" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
  
    <script>
        <!--Important To Ensure Messages Count Gets Displayed Accross All Pages-->
        localStorage['messageCount'] = document.getElementById("messageBoxCount").innerText;

   
    </script> 
       <!--Allows access to Master Page Methods *IMPORTANT*-->
      <%@ MasterType VirtualPath="~/MasterPage.Master" %>

    <h1 class="text-center">Google Maps</h1>
    <!-- Js attaches to the Div id map-->
    <div id="map"></div>

    <!-- Used To Add Js File to Page Via Programatically-->
    <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>

    <%--Called From Web Config File--%>
    <script id="googleApi" src="<%=ConfigurationManager.AppSettings["GOOGLE_API"] %>&callback=initMap"> </script>

</asp:Content>

