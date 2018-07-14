﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="IBT_Edit_Uploads.aspx.cs" Inherits="BranchMIS.IBT.IBT_Edit_Uploads" %>

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

        td.locked, th.locked {
            position: relative;
            left: expression((this.parentElement.parentElement.parentElement.parentElement.scrollLeft-2)+'px');
        }

        .SearchGridView_T ItemWidth {
            /*width: 440px;
            padding-left: 150px;
            padding-right: 150px;*/
        }
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

        a.back-to-top {
            display: none;
            width: 60px;
            height: 60px;
            text-indent: -9999px;
            position: fixed;
            z-index: 999;
            right: 20px;
            bottom: 20px;
            background: #27AE61 url('../IBT Images/white-left-arrow.png') no-repeat center 43%;
            -webkit-border-radius: 30px;
            -moz-border-radius: 30px;
            border-radius: 30px;
        }

        a:hover.back-to-top {
            background-color: #000;
        }

        .footerContainer {
            max-width: 960px;
            margin: 0 auto;
            background: #FFF;
        }

        .padding2 {
            padding: 20px;
        }

        /*--------------------------------------------------------*/

        .summaryPanel {
            background-color: #18919b; /*#0c575d;*/
            padding: 12px 15px 11px 15px;
            color: white;
            font-size: 12px;
            box-shadow: 0px 13px 10px -10px rgba(191,191,191,0.75);
            /*position: fixed;*/
            /*z-index: 1;*/
            /*top: 35px;*/
            /*margin-left: -16px;*/
        }

        .buttonWidth {
            width: 150px;
            margin-left: 15px;
        }


        /*----------------------------------Progress Status-----------------------------------*/

        .LockOff {
            display: none;
            visibility: hidden;
        }

        .LockOn {
            background-position: 630px 550px;
            display: block;
            visibility: visible;
            position: absolute;
            z-index: 999;
            top: 0px;
            left: 0px;
            width: 100%;
            height: 100%;
            background-color: #cfcfcf; /*#ccc;*/
            ;
            text-align: center;
            padding-top: 20%;
            filter: alpha(opacity=75);
            opacity: 0.75;
            background-image: url('../IBT Images/spiffygif_126x126.gif');
            background-repeat: no-repeat;
        }

        /*------------------------------------------------------------------------------------*/

        /*----------------------------------Download Panel------------------------------------*/

        .dwPanel {
            /*width: 100px;*/
            width: 200px;
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
            width: 1020px; /*height: 610px;*/
            height: 635px;
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

        .PopuptextBox_Multiline {
            border: 1px solid #c4c4c4;
            height: 35px;
            width: 400px;
            font-size: 12px;
            padding: 3px 3px 3px 3px;
        }

        .PopuptextBoxMultiLine {
            border: 1px solid #c4c4c4;
            height: 65px;
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
            height: 162px;
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
            height: 35px;
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
            width: 75px;
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
    </style>

    <script type="text/javascript" src="../IBT Scripts/jquery-1.11.3.min.js"></script>

    <script type="text/javascript">


        function skm_LockScreen(str) {
            var lock = document.getElementById('skm_LockPane');

            if (lock)
                lock.className = 'LockOn';
            //lock.innerHTML = str;
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form runat="server">

        <!-- BEGIN PAGE HEADER-->
        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>Search And Edit Uploaded Data</h3>
                </div>
                <%--<div class="col-md-1">
                    <asp:Button ID="btnSetFocusTop" BackColor="White" ForeColor="White" BorderStyle="None" runat="server" Text="|" />
                </div>--%>
            </div>
        </div>

        <div class="row" runat="server">
            <div class="col-md-12">
                <div runat="server" role="alert" visible="true" id="page_result">
                    <asp:Label ID="lblResult" runat="server" Text="" Visible="false"></asp:Label>
                </div>
            </div>
        </div>

        <div class="well well-lg">

            <%------------------------------------------------------Row One Start----------------------------------------%>
            <div class="row">
                <%--History--%>
                <div class="col-md-2">
                    <h5>Filter History:</h5>
                </div>
                <div class="col-sm-2">
                    <asp:DropDownList ID="ddlHistory" runat="server" CssClass="form-control">
                        <asp:ListItem Value="0">Active Only</asp:ListItem>
                        <asp:ListItem Value="1">With History</asp:ListItem>
                    </asp:DropDownList>
                </div>


                <%--IBT Status--%>
                <div class="col-md-2">
                    <h5>IBT Status:</h5>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlIBT" runat="server" CssClass="form-control">
                        <asp:ListItem Value="0">All</asp:ListItem>
                        <asp:ListItem Value="1">IBT</asp:ListItem>
                        <asp:ListItem Value="2">Non IBT</asp:ListItem>
                    </asp:DropDownList>
                </div>


                <%--Bulk Receipt Indicator--%>
                <div class="col-md-2">
                   <h5>Bulk Receipt Status:</h5>
                </div>
                <div class="col-md-2">

                    <asp:DropDownList ID="ddlBulkReceiptStatus" runat="server" CssClass="form-control">
                        <asp:ListItem Value="0">All</asp:ListItem>
                        <asp:ListItem Value="1">Yes</asp:ListItem>
                        <asp:ListItem Value="2">No</asp:ListItem>
                    </asp:DropDownList>
                </div>                
            </div>

<%------------------------------------------------------Row One End-------------------------------------------------------------%>




            <%--Empty Row--%>
            <div class="row">
                <div class="col-md-1">&nbsp;</div>
            </div>



<%------------------------------------------------------Row Two Start----------------------------------------%>

            <div class="row">
                 <%--product--%>
                <div class="col-md-2">
                    <h5>Product:</h5>
                </div>
                <div class="col-sm-2">
                    <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-control">
                        <asp:ListItem Value="0">All</asp:ListItem>
                        <asp:ListItem Value="1">Life</asp:ListItem>
                        <asp:ListItem Value="2">General</asp:ListItem>
                    </asp:DropDownList>
                </div>
                

                <%--Data Category--%>
                <div class="col-md-2">
                    <h5>Data Category:</h5>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlDataCategory" runat="server" CssClass="form-control">
                        <asp:ListItem Value="0">All</asp:ListItem>
                    </asp:DropDownList>
                </div>

                
                <%--Matching Status--%>
                <div class="col-md-2">
                    <h5>Matching Status:</h5>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlMatching" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>

            </div>

            <%------------------------------------------------------Row Two Ends----------------------------------------%>




            <%--Empty Row--%>
            <div class="row">
                <div class="col-md-1">&nbsp;</div>
            </div>



            <%------------------------------------------------------Row Three Start----------------------------------------%>

            <div class="row">                
                <%--Receipt Status--%>
                <div class="col-md-2">
                    <h5>Receipt Status:</h5>
                </div>

                <div class="col-sm-2">
                    <asp:DropDownList ID="ddlreceipt_status" runat="server" CssClass="form-control">
                        <asp:ListItem Value="0">All</asp:ListItem>
                    </asp:DropDownList>
                </div>



                <div class="col-md-2">
                    <h5>Date:</h5>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlDate" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlDate_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Value="0">All</asp:ListItem>
                        <asp:ListItem Value="1">Today</asp:ListItem>
                        <asp:ListItem Value="2">History</asp:ListItem>
                        <asp:ListItem Value="3">Date Range</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>


            <%--Empty Row--%>
            <div class="row">
                <div class="col-md-1">&nbsp;</div>
            </div>


            <div class="row">
                <%--<div class="col-md-2">--%>
                <div id="cal_div" runat="server" visible="false" class="col-md-4">

                    <div class="col-md-2">
                        <h5>From:</h5>
                    </div>
                    <div class="col-sm-2 ">
                        <div class="input-group input-medium date date-picker" data-date-format="dd/mm/yyyy">
                            <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                            <span class="input-group-btn">
                                <button class="btn default" type="button"><i class="fa fa-calendar"></i></button>
                            </span>
                        </div>
                    </div>

                </div>

                <%--<div class="col-md-2">--%>
                <div id="cal_div2" runat="server" visible="false" class="col-md-4">
                    <div class="col-md-2">
                        <h5>To:</h5>
                    </div>
                    <div class="col-sm-2">
                        <div class="input-group input-medium date date-picker" data-date-format="dd/mm/yyyy">
                            <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server"></asp:TextBox>
                            <span class="input-group-btn">
                                <button class="btn default" type="button"><i class="fa fa-calendar"></i></button>
                            </span>
                        </div>
                    </div>

                </div>


                <%--                <div class="col-md-2">
                    <label id="lb"></label>
                </div>--%>
            </div>





            <%--Empty Row--%>
            <div class="row">
                <div class="col-md-1">&nbsp;</div>
            </div>



            <div class="row">
                <%--IBT Column Filter--%>
                <div class="col-md-2">
                    <h5>IBT Custom Filters:</h5>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlIbtColumns" runat="server" CssClass="form-control">
                        <asp:ListItem>--- Please Select ---</asp:ListItem>
                        <asp:ListItem>Account_No</asp:ListItem>
                        <asp:ListItem>Bank_Ref</asp:ListItem>
                        <asp:ListItem>Bank_Branch</asp:ListItem>
                        <asp:ListItem>Cheque_No</asp:ListItem>
                        <asp:ListItem>SERIAL_NO</asp:ListItem>
                        <asp:ListItem>RECEIPT_NO</asp:ListItem>
                        <asp:ListItem>CASH_ACC_NO</asp:ListItem>
                    </asp:DropDownList>
                </div>


                <div class="col-md-2">
                    <h5>Search Value:</h5>
                </div>
                <div class="col-md-2">
                    <asp:TextBox ID="txtIbtColumn" runat="server" CssClass="form-control" ValidationGroup="vgSearch"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="String Not In Correct Format" ValidationGroup="vgSearch" SetFocusOnError="True" ControlToValidate="txtIbtColumn" ValidationExpression="([a-z]|[A-Z]|[0-9]|[-]|[_]|[/]|[ ])*" Display="Dynamic"></asp:RegularExpressionValidator>
                </div>

                <div class="col-md-2">
                    &nbsp;
                </div>

                <div class="col-md-1">
                    <asp:Button ID="btnSearch" runat="server" Text="Filter Data" OnClick="btnSearch_Click" CssClass="btn btn-success btn-md" ValidationGroup="vgSearch" />
                </div>
                <div class="col-md-1">
                    <asp:ImageButton ID="ibtnExcelReport" runat="server" Height="35px" ValidationGroup="vgExport" OnClick="ibtnExcelReport_Click" ImageUrl="~/assets/img/FAS/dw.PNG" ToolTip="Get Report" />
                </div>

            </div>


            <%--Empty Row--%>
            <%--            <div class="row">
                <div class="col-md-1">&nbsp;</div>
            </div>



            <div class="row">
                <div class="col-md-10">
                    &nbsp;
                    </div>
                <div class="col-md-1">
                    
                </div>
                <div class="col-md-1">
                   
                    </div>
            </div>--%>
        </div>

        <%--End Of Search Content--%>

        <%--Search Summary--%>
        <div id="summaryDiv" runat="server" class="page_result summaryPanel" visible="false">

            <asp:Label ID="lblSummaryTotal" runat="server" Text="0" Visible="true"></asp:Label>
            |  
                                <asp:Label ID="lblSummaryIBT" runat="server" Text="0" Visible="true"></asp:Label>
            |  
                                <asp:Label ID="lblSummaryNonIBT" runat="server" Text="0" Visible="true"></asp:Label>
            |
                                <asp:Label ID="lblSummaryUnmatched" runat="server" Text="0" Visible="true"></asp:Label>
            | 
                                <asp:Label ID="lblSummaryPossible" runat="server" Text="0" Visible="true"></asp:Label>
            | 
                                <asp:Label ID="lblSummaryExact" runat="server" Text="0" Visible="true"></asp:Label>
            | 
                                <asp:Label ID="lblSummaryConfirmed" runat="server" Text="0" Visible="true"></asp:Label>
            |
                                <asp:Label ID="lblSummaryC_General" runat="server" Text="0" Visible="true"></asp:Label>
            | 
                                <asp:Label ID="lblSummaryC_Life" runat="server" Text="0" Visible="true"></asp:Label>
            |
                                <asp:Label ID="lblSummaryT_General" runat="server" Text="0" Visible="true"></asp:Label>
            | 
                                <asp:Label ID="lblSummaryT_Life" runat="server" Text="0" Visible="true"></asp:Label>
            |
                                <asp:Label ID="lblSummary_MRP" runat="server" Text="0" Visible="true"></asp:Label>
            |
                                <asp:Label ID="lblSummary_MCR" runat="server" Text="0" Visible="true"></asp:Label>
            |
                                <asp:Label ID="lblSummary_MOTOR" runat="server" Text="0" Visible="true"></asp:Label>
            |
                                <asp:Label ID="lblSummary_NON_MOTOR" runat="server" Text="0" Visible="true"></asp:Label>
            |
                                <asp:Label ID="lblSummary_NEW_BUSINESS" runat="server" Text="0" Visible="true"></asp:Label>
            |
                                <asp:Label ID="lblSummary_OTHER" runat="server" Text="0" Visible="true"></asp:Label>
            |
                                <asp:Label ID="lblSummary_BULK" runat="server" Text="0" Visible="true"></asp:Label>

            |                   <asp:Label ID="lblSerial_Count" runat="server" Text="0" Visible="false"></asp:Label>
            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="String Not In Correct Format" ValidationGroup="vgUpdate" SetFocusOnError="True" ControlToValidate="txtDescription" ValidationExpression="([a-z]|[A-Z]|[0-9]|[ ]|[-]|[_][.][\//])*" Display="Dynamic"></asp:RegularExpressionValidator>--%>
        </div>
        <%--End Of Search Summary--%>

        <div class="row">
            &nbsp;
        </div>

        <div class="panel-group">
            <div class="panel panel-default">
                <asp:Panel ID="Panel3" runat="server" Height="470px" ScrollBars="Both">
                    <asp:GridView ID="grdSearchData" runat="server" AllowPaging="True" PageSize="30" CssClass="SearchGridView_T" OnPageIndexChanging="grdSearchData_PageIndexChanging" OnRowCommand="grdSearchData_RowCommand" OnRowDataBound="grdSearchData_RowDataBound">
                        <Columns>
                            <asp:ButtonField CommandName="Select" Text="Select" />
                            <asp:ButtonField CommandName="BulkReceipt" Text="BulkReceipt" />
                        </Columns>
                        <FooterStyle Font-Size="XX-Large" />
                        <PagerStyle Font-Size="Large" />
                        <SelectedRowStyle CssClass="selectedRowStyle td" />
                        <PagerStyle Font-Names="Arial" Font-Size="XX-Large" Font-Bold="true" ForeColor="red" />
                    </asp:GridView>
                </asp:Panel>

                <asp:LinkButton ID="cmdViewRule" runat="server"></asp:LinkButton>
            </div>
        </div>


        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <%--popup one--%>

        <asp:LinkButton Text="" ID="lnkFake" runat="server" />
        <cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="lnkFake" CancelControlID="btnClose" BackgroundCssClass="modalBackground"></cc1:ModalPopupExtender>

        <%--Style="display:none"--%>
        <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup">
            <div class="header_a" runat="server" id="popupHeader">
                <div class="HeaderTopic">
                    <table style="width: 100%;">
                        <tr>
                            <td>Edit IBT Record 
                                    <asp:Label ID="lbl_PolicyNo" runat="server" Visible="False"></asp:Label>
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
                <div class="divBorder">
                    <div class="divcol_01">
                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:Label ID="Label1" runat="server" CssClass="boldLable" Text="Account Number"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label3" runat="server" CssClass="boldLable" Text="Cash Account"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="divcol_02">
                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:Label ID="Label41" runat="server" Text=":"></asp:Label>
                                    <asp:Label ID="lbl_AccountNo" runat="server" Text="lbl_AccountNo"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label42" runat="server" Text=":"></asp:Label>
                                    <asp:Label ID="lbl_CashAccount" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="divcol_03">

                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" CssClass="boldLable" Text="Serial"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_mainID" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                        </table>

                    </div>

                    <div class="divcol_04">

                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:Label ID="Label43" runat="server" Text=":"></asp:Label>
                                    <asp:Label ID="lbl_Serial" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_ReceiptStatusVal" runat="server" Visible="False"></asp:Label>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="divcol_05">
                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:Label ID="Label4" runat="server" CssClass="boldLable" Text="Transaction Date"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label37" runat="server" CssClass="boldLable" Text="Created Date"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="divcol_06">

                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:Label ID="Label46" runat="server" Text=":"></asp:Label>
                                    <asp:Label ID="lbl_TransactionDate" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label48" runat="server" Text=":"></asp:Label>
                                    <asp:Label ID="lbl_Created_Date" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="specialCol">
                        <div class="divcol_01">
                            <table class="nav-justified">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label22" runat="server" CssClass="boldLable" Text="Branch Code"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" CssClass="boldLable" Text="Bank Refference"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="divSPcol_02">
                            <table>
                                <tr>
                                    <td>

                                        <asp:Label ID="Label44" runat="server" Text=":"></asp:Label>
                                        <asp:Label ID="lbl_BranchCode" runat="server" CssClass="maxLengthLable"></asp:Label>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label45" runat="server" Text=":"></asp:Label>
                                        <asp:Label ID="lbl_BankRef" runat="server" CssClass="maxLengthLable"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="divcol_03">
                            <table class="nav-justified">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label39" runat="server" CssClass="boldLable" Text="Effective End Date"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </div>
                        <div class="divcol_06">
                            <table class="nav-justified">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label49" runat="server" Text=":"></asp:Label>
                                        <asp:Label ID="lbl_Ef_End_Date" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>

                <div class="divBorder">
                    <div class="divcol_01">

                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:Label ID="Label16" runat="server" CssClass="boldLable" Text="Running Total"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label10" runat="server" CssClass="boldLable" Text="Cheque Number"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label18" runat="server" CssClass="boldLable" Text="Openning Balance"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Lable20" runat="server" CssClass="boldLable" Text="Closing Ballance"></asp:Label>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="divcol_02">
                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:Label ID="Label50" runat="server" Text=":"></asp:Label>
                                    <asp:Label ID="lbl_RunningTot" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label40" runat="server" Text=":"></asp:Label>
                                    <asp:Label ID="lbl_Chequeno" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label53" runat="server" Text=":"></asp:Label>
                                    <asp:Label ID="lbl_CalCulatedOB" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label54" runat="server" Text=":"></asp:Label>
                                    <asp:Label ID="lbl_ClossingBal" runat="server"></asp:Label>
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
                                <td>
                                    <asp:Label ID="lbl_OpenningBal" runat="server" Visible="False"></asp:Label>
                                </td>
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
                                <td>
                                    <asp:Label ID="Lable12" runat="server" CssClass="boldLable" Text="Debit"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Lable13" runat="server" CssClass="boldLable" Text="Credit"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label35" runat="server" CssClass="boldLable" Text="Created By"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="divcol_06">
                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:Label ID="Label51" runat="server" Text=":"></asp:Label>
                                    <asp:Label ID="lbl_Debit" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label52" runat="server" Text=":"></asp:Label>
                                    <asp:Label ID="lbl_Credit" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label55" runat="server" Text=":"></asp:Label>
                                    <asp:Label ID="lbl_CreatedBy" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div class="divBorder_mini">
                    <div class="divcol_01">

                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:Label ID="Label25" runat="server" CssClass="boldLable" Text="IBT Status   :"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label24" runat="server" CssClass="boldLable" Text="Product:"></asp:Label>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="divcol_02">

                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddl_IBTStatus" runat="server" ValidationGroup="vg_UpdateIBT">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddl_Produt" runat="server" ValidationGroup="vg_UpdateIBT">
                                    </asp:DropDownList>
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
                        </table>

                    </div>
                    <div class="divcol_05">

                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:Label ID="Label26" runat="server" CssClass="boldLable" Text="Matching Status:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label27" runat="server" CssClass="boldLable" Text="Data Category:"></asp:Label>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="divcol_06">
                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddl_MatchingStatus" runat="server" ValidationGroup="vg_UpdateIBT">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddl_DataCategory" runat="server" ValidationGroup="vg_UpdateIBT">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div class="divBorder_mini">
                    <div class="divcol_01">

                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:Label ID="Label23" runat="server" CssClass="boldLable" Text="Policy Number:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label28" runat="server" CssClass="boldLable" Text="Description:"></asp:Label>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="divcol_02">

                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_PolicyNo" runat="server" CssClass="PopuptextBox" ValidationGroup="vg_UpdateIBT"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_Description" runat="server" CssClass="PopuptextBox" ValidationGroup="vg_UpdateIBT"></asp:TextBox>
                                </td>
                            </tr>
                        </table>

                    </div>
                </div>

                <div class="divBorder_mini_large">
                    <div class="divcol_01">

                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:Label ID="Label29" runat="server" CssClass="boldLable" Text="Receipt Status"></asp:Label>
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
                                    <%--<asp:Label ID="Label34" runat="server" CssClass="boldLable" Text="Feedback Type:"></asp:Label>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label5" runat="server" CssClass="boldLable" Text="User Comment:"></asp:Label>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="divcol_02">

                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:Label ID="Label56" runat="server" Text=":"></asp:Label>
                                    <asp:Label ID="lbl_Rec_Status" runat="server"></asp:Label>
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


                                    <%--<asp:DropDownList ID="ddlFeedbackType" runat="server" ValidationGroup="vg_UpdateIBT"></asp:DropDownList>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_usrComment" runat="server" CssClass="PopuptextBox_Multiline" ValidationGroup="vg_UpdateIBT" TextMode="MultiLine"></asp:TextBox>

                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="divcol_03">

                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:Label ID="Label57" runat="server" CssClass="boldLable" Text="Bulk Receipted"></asp:Label>
                                </td>
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
                                <td>
                                    <asp:DropDownList ID="ddl_bulkReceiptStatus" runat="server" Enabled="False" ValidationGroup="vg_UpdateIBT">
                                    </asp:DropDownList>
                                </td>
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
                                <td>
                                    <asp:Label ID="Label31" runat="server" CssClass="boldLable" Text="Manually Receipted :"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                                                <td>
                                    <asp:Label ID="Label6" runat="server" CssClass="boldLable" Text="Add History :"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_usrComment" Display="Dynamic" ErrorMessage="Comment Required" SetFocusOnError="True" ValidationGroup="vg_UpdateIBT"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>

                    </div>
                    <div class="divcol_06">

                        <table class="nav-justified">
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddl_ReceiptedMethod" runat="server" ValidationGroup="vg_UpdateIBT">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="ddl_AddHistory" runat="server" ValidationGroup="vg_UpdateIBT">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>

                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="btnAlign">
                                        <asp:Button ID="btn_UpdateRecord" runat="server" CssClass="updatebtn" OnClick="btn_UpdateRecord_Click" Text="Update" ValidationGroup="vg_UpdateIBT" />
                                        <%--<asp:Button ID="Button1" runat="server" CssClass="updatebtn" Text="Cancel" />--%>
                                        <asp:Button ID="btn_bulkDetails" runat="server" CssClass="updatebtn" OnClick="btn_bulkDetails_Click" Text="Bulk Details" />
                                    </div>
                                </td>
                            </tr>
                        </table>

                    </div>




                </div>
            </div>
            <!--Style="display:none">-->
        </asp:Panel>

        <%--popup two--%>

        <asp:LinkButton Text="" ID="lnkFake2" runat="server" />
        <cc1:ModalPopupExtender ID="mpe2" runat="server" PopupControlID="pnlPopup2" TargetControlID="lnkFake2" CancelControlID="btnClose" BackgroundCssClass="modalBackground"></cc1:ModalPopupExtender>

        <asp:Panel ID="pnlPopup2" runat="server" CssClass="modalPopup" Style="display: none">
            <div class="header_a" runat="server" id="popupHeader2">
                <div class="HeaderTopic">
                    <table style="width: 100%;">
                        <tr>
                            <td>&nbsp; IBT Bulk Receipt Details </td>
                            <td>
                                <asp:ImageButton ID="ImageButton1" runat="server" CssClass="PopClose" Height="35px" ImageUrl="~/assets/img/FAS/P_2.png" Width="35px" OnClick="ImageButton1_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="body">
                <div class="panel-body">
                    <div class="form-group">
                        <asp:Panel ID="Panel1" runat="server" Height="500px" ScrollBars="Vertical" Style="z-index: 102;"
                            Width="100%" BorderColor="#FFFFFF" BorderWidth="1px">
                            <div class="grdAlign">
                                <asp:GridView ID="grdBulkReceiptDetails" runat="server" CssClass="mGrid" OnRowDataBound="grdBulkReceiptDetails_RowDataBound">
                                    <SelectedRowStyle CssClass="selectedRowStyle td" />
                                </asp:GridView>
                            </div>
                        </asp:Panel>
                        <div class="form-group">
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

    </form>
    </div>
</asp:Content>
