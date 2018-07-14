<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="IBT_ReportsView.aspx.cs" Inherits="BranchMIS.IBT.IBT_ReportsView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form runat="server">
        <!-- BEGIN PAGE HEADER-->
        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>Receipt View</h3>
                </div>
            </div>
            <%--<div class="col-md-4">.col-md-4</div>--%>
        </div>
        <div class="row" runat="server" visible="false">
            <div class="col-md-12">
                <div id="page_result" runat="server" class="alert alert-danger" role="alert">
                    <asp:Label ID="lblResult" runat="server" Text="Label"></asp:Label>
                </div>
            </div>
        </div>


        <div class="panel-group">
            <%--            <div class="panel panel-default">
                <div class="panel-body"></div>
            </div>--%>
            <div class="panel panel-default">
                <asp:Literal ID="ltrlExcelExport" runat="server">
                </asp:Literal>
            </div>
        </div>
    </form>
</asp:Content>
