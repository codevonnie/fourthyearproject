<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TestServicePage.aspx.cs" Inherits="Web_TestServicePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">





    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="http://ajax.cdnjs.com/ajax/libs/json2/20110223/json2.js"></script>

    <br />
    <br />

    <br />
    <div id="state"></div>
    <div id="message"></div>
    <textarea id="messageList"></textarea>


    <%-- If a message comes in, save it to a list straight away. then wipe the message obj on the server side --%>

    <script>
        if (!!window.EventSource) {
            var source = new EventSource('http://localhost:3000/stream')
            var MessageArray = [];

            source.addEventListener('message', function (e) {
                document.getElementById("messageList").value = "";
                messageObj = JSON.parse(e.data)
                    MessageArray.push(messageObj);

                    MessageArray.forEach(function (entry) {
                        document.getElementById("messageList").value += "\n" + entry.status + "\n" + entry.message;
                    });
            }, false)

            source.addEventListener('open', function (e) {
                $("#state").text("Connected")
            }, false)

            source.addEventListener('error', function (e) {
                if (e.target.readyState == EventSource.CLOSED) {
                    $("#state").text("Disconnected")
                }
                else if (e.target.readyState == EventSource.CONNECTING) {
                    $("#state").text("Connecting...")
                }
            }, false)
        } else {
            console.log("Your browser doesn't support SSE")
        }
    </script>







</asp:Content>

