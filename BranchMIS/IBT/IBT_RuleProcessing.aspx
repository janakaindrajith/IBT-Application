<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="IBT_RuleProcessing.aspx.cs" Inherits="BranchMIS.IBT.IBT_RuleProcessing" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .page_result {
            padding: 10px;
            font-size: 16px;
            color: white;
        }

        .HiddenCol {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <form runat="server">
        <!-- BEGIN PAGE HEADER-->
        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>IBT Rules Processing</h3>
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

        <div class="well well-lg">
            <div class="row">
                <div class="col-md-1">
                    <asp:Button ID="cmdBifurcation" runat="server" Text="IBT Bifurcation Rules" CssClass="btn btn-primary btn-md" OnClick="cmdBifurcation_Click" />
                </div>
                <div class="col-md-1">
                    &nbsp
                </div>
                <div class="col-md-1">
                    <asp:Button ID="cmdMatching" runat="server" Text="IBT Matching Rules" CssClass="btn btn-primary btn-md" OnClick="cmdMatching_Click" />
                </div>
                <div class="col-md-1">
                    &nbsp
                </div>
                <div class="col-md-2">
                    <asp:Button ID="cmdDeptDecompose" runat="server" Text="Department Decompose Rules" CssClass="btn btn-primary btn-md" OnClick="cmdDeptDecompose_Click" />
                </div>
                <div class="col-md-1">
                    &nbsp
                </div>
            </div>
        </div>



    </form>
</asp:Content>
