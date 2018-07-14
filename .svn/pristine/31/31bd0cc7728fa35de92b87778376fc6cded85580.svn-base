<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="IBTUpload.aspx.cs" Inherits="BranchMIS.IBT.IBTUpload" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">


    <style type="text/css">
        /*----------------------------------Progress Status-----------------------------------*/

        .LockOff {
            display: none;
            visibility: hidden;
        }

        .LockOn {
            background-position: 630px 300px;
            display: block;
            visibility: visible;
            position: absolute;
            z-index: 999;
            top: 0px;
            left: 0px;
            width: 100%;
            height: 115%;
            background-color: #2b2828; /*#cfcfcf;*/ /*#ccc;*/
            margin-top: -100px;
            text-align: center;
            padding-top: 20%;
            filter: alpha(opacity=75);
            opacity: 0.8;
            background-image: url('../assets/img/FAS/spiffygif_134x134.gif');
            background-repeat: no-repeat;
        }

        /*------------------------------------------------------------------------------------*/






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


    <script type="text/javascript">
        function Test() {
            alert("hi");
            $("#ButtonRow").show();
        }
        function AnotherFunction() {
            alert("This is another function");
        }


        function skm_LockScreen(str) {
            var lock = document.getElementById('skm_LockPane');

            if (lock)
                lock.className = 'LockOn';
            //lock.innerHTML = str;
        }

    </script>

    <%-----------------------------------------------%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form runat="server">
        <!-- BEGIN PAGE HEADER-->
        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>Bank Statement Upload</h3>
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


        <%--Panel--%>
        <%-- <div class="panel-group">
            <div class="panel panel-info">--%><%--panel-success--%>
        <%--                <div class="panel-heading">MT940 Download History</div>
                <div class="panel-body">

                </div>
            </div>
        </div>--%>


        <div class="well well-lg">
            <div class="row">
                <div class="col-md-2">
                    <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="True" />
                </div>
                <%--         <div class="col-md-1">
                    &nbsp
                </div>--%>
                <div class="col-md-2">
                    <asp:DropDownList ID="DDLAccounts" AutoPostBack="true" runat="server" CssClass="form-control" OnSelectedIndexChanged="DDLAccounts_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <asp:Button ID="cmdShowData" runat="server" Text="View Statement Records" CssClass="btn btn-primary btn-md" OnClick="cmdShowData_Click" />
                </div>
                <div class="col-md-4">
                    <asp:Button ID="cmdClearAll" runat="server" Text="Clear All" CssClass="btn btn-primary btn-md" OnClick="cmdClearAll_Click" />
                </div>
                <div class="col-md-2">
                    <asp:Button ID="cmdUpload" runat="server" Text="Upload Bank Statements" CssClass="btn btn-success btn-md" OnClick="cmdUpload_Click" OnClientClick="skm_LockScreen('We Are Processing Your Request...');" />
                </div>
            </div>
        </div>






        <div class="panel-group">
            <%--            <div class="panel panel-default">
                <div class="panel-body"></div>
            </div>--%>
            <div class="panel panel-default">

                <asp:Panel ID="Panel1" runat="server" ScrollBars="Both" CssClass="panel-body" Height="350px">
                    <asp:GridView ID="GrdUpload" runat="server" CssClass="SearchGridView_T" OnRowEditing="GrdUpload_RowEditing" OnRowCancelingEdit="GrdUpload_RowCancelingEdit" OnRowUpdating="GrdUpload_RowUpdating" OnRowDataBound="GrdUpload_RowDataBound" AllowPaging="True" OnPageIndexChanging="GrdUpload_PageIndexChanging">

                        <Columns>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </div>
        </div>








        <%--        <asp:Panel ID="pnlUserGrid" runat="server" Height="390px" ScrollBars="Both" Style="z-index: 102;"
            Width="100%" BorderColor="#C0C0FF" BorderWidth="1px">
            <asp:GridView ID="GrdUpload" runat="server" BackColor="White" BorderColor="#CCCCCC"
                BorderStyle="None" BorderWidth="1px" CellPadding="3" Font-Bold="False" Font-Names="Tahoma"
                Font-Size="8pt" Style="z-index: 102;" Width="100%" CssClass="SearchGridView" RowStyle-Wrap="false" OnRowEditing="GrdUpload_RowEditing" OnRowCancelingEdit="GrdUpload_RowCancelingEdit" OnRowUpdating="GrdUpload_RowUpdating" OnRowDataBound="GrdUpload_RowDataBound" AllowPaging="True" OnPageIndexChanging="GrdUpload_PageIndexChanging">

                <Columns>
                </Columns>

                <FooterStyle BackColor="White" ForeColor="#000066" />
                <RowStyle ForeColor="Black" BackColor="White" Height="15px" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <HeaderStyle BackColor="#006699" Font-Bold="True" Font-Names="Tahoma" Font-Size="Larger"
                    ForeColor="White" Height="20px" />
                <AlternatingRowStyle BackColor="WhiteSmoke" Font-Names="Tahoma" Font-Size="8pt" Height="15px" />
            </asp:GridView>
        </asp:Panel>--%>

        <div class="row">
            <div class="sub-menu">
            </div>
        </div>


        <div class="well well-lg">
            <div class="row">
                <div class="col-md-1">
                    <h5>Search Log</h5>
                </div>
                <div class="col-md-3">



                    <%--<div class="form-group">
                        <div class="input-prepend input-group" data-date-format="dd/mm/yyyy">
                            <span class="add-on input-group-addon"><i class="glyphicon glyphicon-calendar fa fa-calendar"></i></span>
                            <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>--%>



                    <div class="input-group input-large date date-picker" data-date-format="dd/mm/yyyy">
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server"></asp:TextBox>
                        <span class="input-group-btn">
                            <button class="btn default" type="button"><i class="fa fa-calendar"></i></button>
                        </span>
                    </div>

                </div>
                <div class="col-md-1">
                    <asp:Button ID="btnSearchLog" runat="server" Text="Search" CssClass="btn btn-success btn-md" OnClick="btnSearchLog_Click" ValidationGroup="vgLogSearch" />
                </div>






            </div>
        </div>






        <div class="select2-container fluid">
        </div>



        <asp:Panel ID="Panel3" runat="server" Height="300px" ScrollBars="Vertical" Style="z-index: 102;"
            Width="100%" BorderColor="#C0C0FF" BorderWidth="1px">
            <asp:GridView ID="grdUploadResult" runat="server" AutoGenerateColumns="true" CssClass="SearchGridView_T">
                <Columns>
                </Columns>
                <RowStyle Wrap="False" />
            </asp:GridView>

        </asp:Panel>
        <asp:LinkButton ID="lbSetFocus" runat="server" Style="color: white;"></asp:LinkButton>



        <div id="skm_LockPane" class="LockOff"></div>



    </form>
</asp:Content>
