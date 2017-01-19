<%@ Page Title="" Language="C#" MasterPageFile="/MasterPage.master" AutoEventWireup="true" CodeFile="UpdateMember.aspx.cs" Inherits="Web_Customer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <h1 class="text-center">Update Customer</h1>



    <div class="fileinput fileinput-new input-group" data-provides="fileinput">
        <div class="form-control" data-trigger="fileinput"><i class="glyphicon glyphicon-file fileinput-exists"></i><span class="fileinput-filename"></span></div>
        <span class="input-group-addon btn btn-default btn-file"><span class="fileinput-new">Select file</span><span class="fileinput-exists">Change</span><input type="file" name="..."/></span>
        <a href="#" class="input-group-addon btn btn-default fileinput-exists" data-dismiss="fileinput">Remove</a>
    </div>
    <br />
    <br />

    <label for="upload" class="fileDragArea">
        <input type="file" class="DragAndDrop" id="upload" multiple="multiple" />
    </label>
    <br />
    <br />

    <label class="custom-file-upload" runat="server">
        <input type="file" />
        <span id="file-selected"></span>
        <i class="fa fa-cloud-upload"></i>Custom Upload
    </label>



    <%--        <label class="custom-file-upload" runat="server">
        <input type="file" runat="server" onserverdrop="DragAndDrop_Click"/>
         <span><button id="fileSelected"  runat="server">ENTER </button></span>
        <i class="fa fa-cloud-upload"></i>Custom Upload
    </label>--%>
</asp:Content>

