<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="IBT_General_Uploads.aspx.cs" Inherits="BranchMIS.IBTManual_Uploads.IBT_General_Uploads" %>

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
                    <h3>General - Manual Upload</h3>
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
                <div class="col-md-2">
                    <asp:FileUpload ID="FileUpload2" runat="server" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FileUpload2" Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="VGGeneralUpload"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-2">
                    <asp:ImageButton ID="ibtn_Upload" ToolTip="General" runat="server" Height="30px" ImageUrl="~/assets/img/FAS/export-3.png" OnClick="ibtn_Upload_Click" ValidationGroup="VGGeneralUpload" />
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>General - Receipt Print</h3>
                </div>
            </div>
            <%--<div class="col-md-4">.col-md-4</div>--%>
        </div>

        <div class="well well-lg">
            <div class="row">
                <div class="col-md-2">
                    <h4><small>
                    <asp:Label ID="Label1" runat="server" Text="Receipt Number Range:"></asp:Label></small></h4>
                </div>
                <div class="col-md-3">
                    <%--<div class="col-xs-4">--%>
                    <asp:TextBox ID="txt_From" runat="server" CssClass="form-control" placeholder="Receipt - From" ValidationGroup="VG_ReceiptView"></asp:TextBox>
                </div>
                <div class="col-md-3" >
                    <asp:TextBox ID="txt_To" runat="server" CssClass="form-control" placeholder="Receipt - To" ValidationGroup="VG_ReceiptView"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btn_GI_Print" runat="server" Text="Continue" OnClick="btn_GI_Print_Click" CssClass="btn btn-success btn-md" ValidationGroup="VG_ReceiptView" />
                </div>
            </div>
        </div>

    </form>
</asp:Content>
