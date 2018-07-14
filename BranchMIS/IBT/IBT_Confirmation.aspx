<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="IBT_Confirmation.aspx.cs" Inherits="BranchMIS.IBT.IBT_Confirmation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" language="javascript">
        function GridSelectAllColumn(spanChk) {
            var oItem = spanChk.children;
            var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0]; xState = theBox.checked;
            elm = theBox.form.elements;

            for (i = 0; i < elm.length; i++) {
                if (elm[i].type === 'checkbox' && elm[i].checked != xState)
                    elm[i].click();
            }
        }

    </script>

    <style type="text/css">
        .page_result {
            padding: 10px;
            font-size: 16px;
            color: white;
        }

        .HiddenCol {
            display: none;
            min-width:40px;
        }

        .SearchGridView_T ItemWidth {
            width: 140px;
            padding-left: 150px;
            padding-right: 150px;
            min-width:40px;
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
            height: 550px;
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

        .table_td {
            padding: 5px 5px 5px 5px;
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



        /*--------------------------------Custom GridView Style------------------------------------*/


        .mGrid {
            width: 100%;
            background-color: #fff;
            margin: 2px 0 7px 0;
            border: solid 1px #525252;
            border-collapse: collapse;
            line-height: 15px;
        }

            .mGrid td {
                padding: 2px;
                border: solid 1px #c1c1c1;
                color: #717171;
            }

            .mGrid th {
                padding: 3px 2px;
                color: #fff;
                background: #424242 url(grd_head.png) repeat-x top;
                border-left: solid 1px #525252;
                font-size: 0.9em;
            }

            .mGrid .alt {
                background: #fcfcfc url(grd_alt.png) repeat-x top;
            }

            .mGrid .pgr {
                background: #424242 url(grd_pgr.png) repeat-x top;
            }

                .mGrid .pgr table {
                    margin: 3px 0;
                }

                .mGrid .pgr td {
                    border-width: 0;
                    padding: 0 3px;
                    border-left: solid 1px #666;
                    font-weight: bold;
                    color: #fff;
                    line-height: 12px;
                }

                .mGrid .pgr a {
                    color: #666;
                    text-decoration: none;
                }

                    .mGrid .pgr a:hover {
                        color: #000;
                        text-decoration: none;
                    }



        /*-----------------------------------------------------------------------------------------*/

        

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
                width:100%;
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
                    <h3>Confirm For Receipting</h3>
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
                        <h5>Category: </h5>
                    </div>
                    <div class="col-md-2">
                        <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true" CssClass="form-control" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                            <asp:ListItem Value="0">All</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <h4><asp:Label ID="lblReceiptStatus" runat="server" Text="0"></asp:Label></h4>
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="cmdConfirmAll" runat="server" Text="Confirm - All Records" CssClass="btn btn-success btn-md" OnClick="cmdConfirmAll_Click" />
                    </div>
                    <div class="col-md-2">
                        <asp:Button ID="cmdConfirm" runat="server" Text="Confirm - Selected Records" CssClass="btn btn-success btn-md" OnClick="cmdConfirm_Click" />
                    </div>
                </div>
            </div>




        <div class="panel-group">
            <div class="panel panel-default">
                <asp:Panel ID="Panel1" runat="server" ScrollBars="Both" CssClass="panel-body" Height="550px">
                    <asp:GridView ID="GrdIBTConfirmation" runat="server" CssClass="SearchGridView_T" OnRowDataBound="GrdIBTConfirmation_RowDataBound" AllowPaging="True" OnPageIndexChanging="GrdIBTConfirmation_PageIndexChanging" OnRowCreated="GrdIBTConfirmation_RowCreated" PageSize="50" Width="100%">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="GridSelectAllColumn(this);" />
                                </HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="40px" />
                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="40px" />
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkboxselect" runat="server"></asp:CheckBox>
                                </ItemTemplate>

<%--                                <ControlStyle Width="30px" />
                                <HeaderStyle Width="30px" />
                                <ItemStyle Width="30px" />--%>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </div>
        </div>

        <%--            <div class="form-group">
                </div>--%>

        <div class="form-group col-sm-3">
            

        </div>
        <div class="form-group col-sm-4">
            
        </div>

        <div class="form-group col-sm-4">
            <%--<asp:LinkButton ID="lb_RuleProcessing" runat="server" OnClick="lb_RuleProcessing_Click">Batch & Rule Processing</asp:LinkButton>--%>
        </div>
        <div class="form-group">
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
                            <td>New Business / Renewal Validation Result
                                    <asp:Label ID="lbl_PolicyNo" runat="server" Visible="False"></asp:Label>
                            </td>
                            <td>
                                <asp:ImageButton ID="BtnClose" Enabled="false" runat="server" CssClass="PopClose" Height="35px" ImageUrl="~/assets/img/FAS/P_2.png" Width="35px" />

                            </td>
                        </tr>
                    </table>
                </div>
                <%-- <hr class="hrLine_style" />--%>
            </div>
            <div class="body">
                <asp:Panel ID="Panel2" runat="server" Height="500px" ScrollBars="Vertical" Style="z-index: 102;"
                    Width="100%" BorderColor="#FFFFFF" BorderWidth="1px">
                    <div class="grdAlign">
                        <asp:GridView ID="grdExceptions" runat="server" CssClass="mGrid">
                            <SelectedRowStyle CssClass="selectedRowStyle td" />
                        </asp:GridView>

                        <div class="form-group col-sm-3">
                            <asp:Button ID="cmd_Cancel" runat="server" Width="200px" CssClass="updatebtn" OnClick="cmd_Cancel_Click" Text="Cancel and Update Later" ValidationGroup="vg_UpdateIBT" />
                        </div>

                        <div class="form-group col-sm-5">
                            <asp:Button ID="Cmd_Continue" runat="server" Width="200px" Visible="false" CssClass="updatebtn" OnClick="Cmd_Continue_Click" Text="Update As Renewals" />
                        </div>


                    </div>
                </asp:Panel>
            </div>
            <!--Style="display:none">-->
        </asp:Panel>

    </form>

</asp:Content>
