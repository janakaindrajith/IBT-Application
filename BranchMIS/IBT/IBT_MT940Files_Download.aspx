<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="IBT_MT940Files_Download.aspx.cs" Inherits="BranchMIS.IBT.MT940_Files_Download" %>

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
                    <h3>History Transaction Download [MT940]</h3>
                </div>
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
            <div class="row">
                <div class="col-md-2">
                    <label for="email">Date Range [From] :</label>
                </div>
                <div class="col-md-3">
                    <div class="input-group input-large date date-picker" data-date-format="dd/mm/yyyy">
                        <asp:TextBox ID="txtFromDate" CssClass="form-control" runat="server" ValidationGroup="vg_search"></asp:TextBox>
                        <span class="input-group-btn">
                            <button class="btn default" type="button"><i class="fa fa-calendar"></i></button>
                        </span>
                    </div>
                </div>


                <div class="col-md-2">
                    <label for="DateRange">Date Range [To] :</label>
                </div>
                <div class="col-md-3">
                    <div class="input-group input-large date date-picker" data-date-format="dd/mm/yyyy">
                        <asp:TextBox ID="txtToDate" CssClass="form-control" runat="server" ValidationGroup="vg_search"></asp:TextBox>
                        <span class="input-group-btn">
                            <button class="btn default" type="button"><i class="fa fa-calendar"></i></button>
                        </span>
                    </div>
                </div>
            </div>

            <%--Empty Row--%>
            <div class="row">
                <div class="col-md-1">&nbsp;</div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    <label for="Serial">Serial Range [From] :</label>
                </div>
                <div class="col-md-3">
                    <asp:TextBox ID="txtSerialFrom" runat="server" CssClass="form-control" ValidationGroup="vg_search"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="String Not In Correct Format" ValidationGroup="vg_search" SetFocusOnError="True" ControlToValidate="txtSerialFrom" ValidationExpression="([a-z]|[A-Z]|[0-9]|[ ]|[-]|[_])*" Display="Dynamic"></asp:RegularExpressionValidator>
                </div>

                <div class="col-md-2">
                    <label for="Serial To">Serial Range [To] :</label>
                </div>
                <div class="col-md-3">
                    <asp:TextBox ID="txtSerialTo" runat="server" CssClass="form-control" ValidationGroup="vg_search"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="String Not In Correct Format" ValidationGroup="vg_search" SetFocusOnError="True" ControlToValidate="txtSerialTo" ValidationExpression="([a-z]|[A-Z]|[0-9]|[ ]|[-]|[_])*" Display="Dynamic"></asp:RegularExpressionValidator>
                </div>
            </div>


            <%--Empty Row--%>
            <div class="row">
                <div class="col-md-1">&nbsp;</div>
            </div>


            <div class="row">
                <div class="col-md-2">
                    <label for="OB">Openning Balance :</label>
                </div>
                <div class="col-md-3">
                    <asp:TextBox ID="txtOpenningBal" runat="server" CssClass="form-control" ValidationGroup="vg_search"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="Field Allowed Only Numbers" ValidationGroup="vg_search" SetFocusOnError="True" ControlToValidate="txtOpenningBal" ValidationExpression="([0-9]|[ ]|[.])*" Display="Dynamic"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtOpenningBal" Display="Dynamic" SetFocusOnError="True" ValidationGroup="vg_search"></asp:RequiredFieldValidator>
                </div>

                <div class="col-md-2">
                    <label for="Acc">Select Account :</label>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlAccounts" runat="server" CssClass="form-control" ValidationGroup="vg_search">
                    </asp:DropDownList>
                </div>
            </div>


            <%--Empty Row--%>
            <div class="row">
                <div class="col-md-1">&nbsp;</div>
            </div>


            <div class="row">
                <%--Empty Column--%>
                <div class="col-md-7">
                    &nbsp;
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btnDownload" runat="server" Text="Download File" CssClass="btn btn-success btn-md" OnClick="btnDownload_Click" ValidationGroup="vg_search" />
                </div>
            </div>
        </div>


        <%--Panel--%>
        <div class="panel-group">
            <div class="panel panel-info"><%--panel-success--%>
                <div class="panel-heading">MT940 Download History</div>
                <div class="panel-body">

                    <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" CssClass="panel-body" Height="300px">
                        <asp:GridView ID="grdDownloadHistory" CssClass="SearchGridView_T" runat="server">
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
