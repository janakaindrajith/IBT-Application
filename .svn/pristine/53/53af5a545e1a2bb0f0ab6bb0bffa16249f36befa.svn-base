<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="IBT_Mails.aspx.cs" Inherits="BranchMIS.IBT.IBT_Mails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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


        /*-------------------------------Popup-----------------------------------------------*/
        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }

        .modalPopup {
            background-color: #FFFFFF;
            width: 850px; /*height: 610px;*/
            height: 350px;
            border-radius: 12px;
            box-shadow: 0 8px 16px rgba(0, 0, 0, 0.3), 0 0 40px rgba(0, 0, 0, 0.1) inset;
        }

            .modalPopup .header_a {
                background-color: #18919b;
                height: 40px;
                text-align: right;
                font-weight: normal;
                border-top-left-radius: 12px;
                border-top-right-radius: 12px;
                border-bottom-color: #5C5C5C;
                padding: 3px 3px 3px 3px;
            }

            .modalPopup .body {
                min-height: 50px;
                line-height: 30px;
                text-align: left;
                padding: 1px;
            }

            .modalPopup .footer {
                padding: 3px;
                text-align: right;
                vertical-align: central;
            }

        hr.hrLine_style {
            border: 0;
            height: 0;
            border-top: 1px solid rgba(0,0,0,0.1);
            border-bottom: 1px solid rgba(255,255,255,0.3);
        }

        .PopClose {
            /*position: relative;
             top: -225px;
             right: -15px;*/
            width: 35px;
            height: 35px;
            float: right;
        }

        .boldLable {
            font-weight: bold;
            color: #5b5757;
        }

        .PopuptextBox {
            border: 1px solid #c4c4c4;
            height: 25px;
            width: 700px;
            font-size: 12px;
            padding: 3px 3px 3px 3px;
        }

        .PopupDropDown {
            border: 1px solid #c4c4c4;
            height: 35px;
            width: 700px;
            font-size: 12px;
            padding: 3px 3px 3px 3px;
        }

        .multiple_lines_textBox {
            border: 1px solid #c4c4c4;
            height: 65px;
            width: 700px;
            font-size: 12px;
            padding: 3px 3px 3px 3px;
        }




        .PopupHeaderLable {
            font-size: large;
            color: white;
        }


        .HeaderTopic {
            text-align: left;
            font-size: large;
            color: white;
        }




        .divBorder {
            border: 1px solid #69b5e8;
            height: 290px;
            margin-left: 10px;
            margin-top: 5px;
            margin-right: 10px;
            padding: 5px 5px 5px 5px;
            /*font-size: small;*/
        }

        .divBorder_mini {
            border: 1px solid #69b5e8;
            height: 70px;
            margin-left: 10px;
            margin-top: 5px;
            margin-right: 10px;
            padding: 5px 5px 5px 5px;
            /*font-size: small;*/
        }

        .divBorder_mini_large {
            border: 1px solid #69b5e8;
            height: 138px;
            margin-left: 10px;
            margin-top: 5px;
            margin-right: 10px;
            padding: 5px 5px 5px 5px;
            /*font-size: small;*/
        }

        /*-------------------------------------------------------------------------*/
        .auto-style1 {
            height: 33px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <form runat="server">

        <!-- BEGIN PAGE HEADER-->
        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>IBT - Mails Configuration</h3>
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
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-sm-2">
                        <h5>Description:</h5>
                    </div>
                    <div class="col-sm-10">
                        <asp:DropDownList ID="ddl_description" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddl_description_SelectedIndexChanged" ValidationGroup="vg_Mail">
                        </asp:DropDownList>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-2">
                        <h5>To:</h5>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txt_To" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="2" ValidationGroup="vg_Mail"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="*" ControlToValidate="txt_To" ValidationGroup="vg_Mail" Display="Dynamic" SetFocusOnError="True" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*"></asp:RegularExpressionValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-2">
                        <h5>CC:</h5>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txt_CC" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="2" ValidationGroup="vg_Mail"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt_CC" Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*" ValidationGroup="vg_Mail"></asp:RegularExpressionValidator>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-2">
                        <h5>Display Name:</h5>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txt_displayName" runat="server" CssClass="form-control" ValidationGroup="vg_Mail"></asp:TextBox>
                    </div>
                </div>


                <div class="form-group">
                    <div class="col-sm-2">
                        <h5>Subject:</h5>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txt_subject" runat="server" CssClass="form-control" ValidationGroup="vg_Mail"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <div class="col-sm-2">
                        <h5>Body:</h5>
                    </div>
                    <div class="col-sm-10">
                        <asp:TextBox ID="txt_body" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="3" ValidationGroup="vg_Mail"></asp:TextBox>
                    </div>
                </div>


                <div class="form-group">
                    <div class="col-sm-11">
                        <%--<h3>Description:</h3>--%>
                    </div>
                    <div class="col-sm-1">
                        <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" Text="Submit" ValidationGroup="vg_Mail" CssClass="btn btn-success btn-md"/>
                    </div>
                </div>

            </div>
        </div>
    </form>
</asp:Content>
