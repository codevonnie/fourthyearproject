<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TestClientPage.aspx.cs" Inherits="Web_TestClientPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script>
        (function poll() {
            setTimeout(function () {
                $.ajax({
                    url: "/server/api/function",
                    type: "GET",
                    success: function (data) {
                        console.log("polling");
                    },
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded',
                        'Autentication': 'Bearer token',
                    },
                    dataType: "json",
                    complete: poll,
                    
                    timeout: 2000
                })
            }, 5000);
        })();
    </script>

    stuff 
</asp:Content>

