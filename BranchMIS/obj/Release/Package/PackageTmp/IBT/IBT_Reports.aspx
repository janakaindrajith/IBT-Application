<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="IBT_Reports.aspx.cs" Inherits="BranchMIS.IBT.IBT_Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <form runat="server">
        <!-- BEGIN PAGE HEADER-->
        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>Search Result Export</h3>
                </div>
            </div>
            <%--<div class="col-md-4">.col-md-4</div>--%>
        </div>
        <div class="row" runat="server">
            <div class="col-md-12">
                <div runat="server" role="alert" visible="true" id="page_result">
                    <asp:Label ID="lblResult" runat="server" Text="" Visible="false"></asp:Label>
                </div>
            </div>
        </div>


        <div class="panel-group">
            <%--            <div class="panel panel-default">
                <div class="panel-body"></div>
            </div>--%>
            <div class="panel panel-default">
            <div>
                <asp:Literal id="ltrlExcelExport" runat="server" >

                </asp:Literal>                
            </div>
        </div>
    </form>
</asp:Content>
