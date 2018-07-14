<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="IBT_Bulk_Receipting.aspx.cs" Inherits="BranchMIS.IBT.IBT_Bulk_Receipting" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<link href="GridViewStyles.css" rel="stylesheet" type="text/css" />--%>
    <style type="text/css">
        .page_result {
            padding: 10px;
            font-size: 16px;
            color: white;
        }

        .HiddenCol {
            display: none;
        }

        .RightAlign {
            text-align: right;
        }

        .LeftAlign {
            text-align: left;
        }

        /*----------------------------------Download Panel------------------------------------*/

        .dwPanel {
            width: 100px;
            float: right;
            margin-top: 5px;
        }




        /*-------------------------------Popup-----------------------------------------------*/
        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }

        .modalPopup {
            background-color: #FFFFFF;
            width: 980px; /*height: 610px;*/
            height: 195px;
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
            width: 400px;
            font-size: 12px;
            padding: 3px 3px 3px 3px;
        }

        .PopuptextBoxMultiLine {
            border: 1px solid #c4c4c4;
            height: 55px;
            width: 400px;
            font-size: 12px;
            padding: 3px 3px 3px 3px;
            resize: none;
        }
        /*.auto-style1 {
             height: 23px;
         }*/

        .PopupHeaderLable {
            font-size: large;
            color: white;
        }

        .PopupBtnPanel {
            text-align: right;
        }

        .HeaderTopic {
            text-align: left;
            font-size: large;
            color: white;
        }

        .selectedRowStyle td {
            background-color: #ffd800;
        }


        .divRow {
            float: left;
            width: 900px;
            /*padding: 5px 5px 5px 5px;*/
        }

        .divColumns01 {
            float: left;
            margin-left: 5px;
        }

        .divColumns02 {
            float: left;
            margin-left: 15px;
        }

        .divColumns03 {
            float: left;
            margin-left: 25px;
        }

        .divBorder {
            border: 1px solid #69b5e8;
            height: 130px;
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

        .txt_PolicyNo {
            margin-left: 130px;
        }

        .txt_Description {
            margin-left: 130px;
        }

        .lbl_receiptNumber {
            margin-left: 20px;
        }

        .txt_ReceiptNumber {
            margin-left: 470px;
        }

        .lbl_ReceiptNarration {
            margin-left: 268px;
            width: 150px;
        }

        .divcol_01 {
            float: left;
            width: 140px;
        }

        .divcol_02 {
            float: left;
            width: 200px;
        }

        .divcol_03 {
            float: left;
            width: 140px;
        }

        .divcol_04 {
            float: left;
            width: 140px;
        }

        .divcol_05 {
            float: left;
            width: 140px;
        }

        .divcol_06 {
            float: left;
            width: 160px;
        }

        .btnAlign {
            text-align: right;
            /*width: 200px;*/
            width: 180px;
        }

        .updatebtn {
            background-color: #fbb612;
            color: white;
            font-family: 'Helvetica Neue',sans-serif;
            font-size: 12px;
            line-height: 20px;
            border-radius: 1px;
            border: 0;
            /*text-shadow: #C17C3A 0 -1px 0;*/
            width: 50px;
            height: 35px;
        }

            .updatebtn:hover {
                background-color: #ff5205;
                color: white;
            }

        .specialCol {
            float: left;
            width: auto;
            overflow: hidden;
        }

        .diSPvcol_01 {
            float: left;
            width: 140px;
        }

        .divSPcol_02 {
            float: left;
            width: 480px;
        }








        

/*custom gridview*/

        /*----------------data Table GridView Css----------------------*/

        .SearchGridView_T {
            max-height: 275px;
            overflow: auto;
            border: 1px solid #ccc;
            table-layout: fixed;
            Width: 2000px;
        }

        .SearchGridView_T {
            border-collapse: collapse;
            width: 100%;
        }

            .SearchGridView_T tr th {
                /*background-color: #428BCA;*/
                background-color: rgba(35, 105, 111, 0.75);
                color: #ffffff;
                padding: 10px 5px 10px 5px;
                border: 1px solid #cccccc;
                font-family: 'Open Sans', sans-serif;
                font-size: 11px;
                font-weight: normal;
                text-transform: capitalize;
                /*min-width:150px;*/
            }

            .SearchGridView_T tr:nth-child(2n+2) {
                background-color: #f3f4f5;
            }
            /*--------------Alternating--------------------*/
            .SearchGridView_T tr:nth-child(2n+1) td {
                background-color: none;
                color: #454545;
            }

            .SearchGridView_T tr td {
                padding: 5px 10px 5px 10px;
                color: #454545;
                font-family: 'Open Sans', sans-serif;
                font-size: 11px;
                border: 1px solid #cccccc;
                vertical-align: middle;
            }

                .SearchGridView_T tr td:first-child {
                    text-align: center;
                }
        /*search result grid selected row*/
        .selectedRowStyle td {
            background-color: #ffd800;
        }

        /*-----------------------------------------------------------------*/



    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form runat="server">
        <!-- BEGIN PAGE HEADER-->
        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>IBT-Bulk Receipting
                        <asp:HiddenField ID="hf_selectedRowID" runat="server" />
                    </h3>
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
                    <label id="lblSnumber">Serial Number</label>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtSerialNo" runat="server" CssClass="form-control" Enabled="False"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <label>Total</label>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtTotal" runat="server" CssClass="form-control RightAlign" Enabled="False"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <label>Balance</label>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtAvailableBal" runat="server" CssClass="form-control RightAlign" Enabled="False"></asp:TextBox>
                </div>
            </div>

            <%--Empty Row--%>
            <div class="row">
                <div class="col-md-1">&nbsp;</div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label>Reference No</label>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtRefNo" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <label class="control-label">Percentage</label>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtPerc" runat="server" CssClass="form-control RightAlign"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="regexpName" runat="server"
                        ErrorMessage="Invalid figure."
                        ControlToValidate="txtPerc"
                        ValidationExpression="^(100(?:\.00)?|0(?:\.\d\d)?|\d?\d(?:\.\d\d)?)$" />
                </div>
                <div class="col-md-2">
                    <label>Amount </label>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control RightAlign"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                        ErrorMessage="Invalid figure."
                        ControlToValidate="txtAmount"
                        ValidationExpression="\d+(\.\d{1,2})?" />
                </div>
            </div>

            <%--Empty Row--%>
            <%--            <div class="row">
                <div class="col-md-1">&nbsp;</div>
            </div>--%>
            <div class="row">
                <div class="col-md-11">&nbsp;</div>
                <div class="col-md-1">
                    <asp:Button ID="cmdAdd" runat="server" Text="Add" CssClass="btn btn-success btn-md" OnClick="cmdAdd_Click" />
                </div>
            </div>

        </div>



        <div class="panel-group">

            <div class="panel panel-default">
                <asp:Panel ID="Panel3" runat="server" Height="300px" ScrollBars="Vertical" Style="z-index: 102;">
                    <asp:GridView ID="grdTransList" runat="server" CssClass="SearchGridView_T" OnRowDeleting="grdTransList_RowDeleting" OnRowDataBound="grdTransList_RowDataBound" OnRowCommand="grdTransList_RowCommand">
                        <Columns>
                            <asp:ButtonField CommandName="Remove" Text="Remove" />
                            <asp:ButtonField CommandName="Manual_Receipt" Text="Manual Receipt" />
                        </Columns>
                        <RowStyle Wrap="False" />
                    </asp:GridView>
                </asp:Panel>
            </div>
        </div>




        <div class="row">
            <div class="col-md-11">&nbsp;</div>
            <div class="col-md-1">
                <asp:Button ID="cmdConfirm" runat="server" Text="Confirm" CssClass="btn btn-success btn-md" OnClick="cmdConfirm_Click" />
            </div>
        </div>

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <%--Style="display: none"--%>


        <%--popup one--%>

        <asp:LinkButton Text="" ID="lnkFake" runat="server" />
        <cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="lnkFake" CancelControlID="btnClose" BackgroundCssClass="modalBackground"></cc1:ModalPopupExtender>

        <%--Style="display:none"--%>
        <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
            <div class="header_a" runat="server" id="popupHeader">
                <div class="HeaderTopic">
                    <table style="width: 100%;">
                        <tr>
                            <td>Manual Receipt
                            </td>
                            <td>
                                <asp:ImageButton ID="BtnClose" runat="server" CssClass="PopClose" Height="35px" ImageUrl="~/assets/img/FAS/P_2.png" Width="35px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <%-- <hr class="hrLine_style" />--%>
            </div>
            <div class="body">

                <div class="divBorder_mini_large">
                    <div class="divcol_01">

                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:Label ID="Label35" runat="server" CssClass="boldLable" Text="Refferance Number:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label32" runat="server" CssClass="boldLable" Text="Receipt Number:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label33" runat="server" CssClass="boldLable" Text="Receipt Narration:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label34" runat="server" CssClass="boldLable" Text="User Comment:"></asp:Label>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="divcol_02">

                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_refNo" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_Rec_No" runat="server" CssClass="PopuptextBox" ValidationGroup="vg_UpdateIBT"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_Rec_Narration" runat="server" CssClass="PopuptextBox" ValidationGroup="vg_UpdateIBT"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>

                                    <asp:TextBox ID="txt_usrComment" runat="server" CssClass="PopuptextBox" ValidationGroup="vg_UpdateIBT"></asp:TextBox>

                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="divcol_03">

                        <table class="nav-justified">
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                        </table>

                    </div>
                    <div class="divcol_04">

                        <table class="nav-justified">
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                        </table>

                    </div>
                    <div class="divcol_05">

                        <table class="nav-justified">
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_usrComment" Display="Dynamic" ErrorMessage="Comment Required" SetFocusOnError="True" ValidationGroup="vg_UpdateIBT"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="btnAlign">
                                        <asp:Button ID="btn_UpdateRecord" runat="server" CssClass="updatebtn" OnClick="btn_UpdateRecord_Click" Text="Update" ValidationGroup="vg_UpdateIBT" />
                                        <asp:Button ID="Button1" runat="server" CssClass="updatebtn" Text="Cancel" />
                                    </div>
                                </td>
                            </tr>
                        </table>

                    </div>




                </div>
            </div>
            <!--Style="display:none">-->
        </asp:Panel>
    </form>
</asp:Content>
