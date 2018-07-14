<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="IBT_Upload_Config.aspx.cs" Inherits="BranchMIS.IBT.IBT_Upload_Config" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<asp:ListItem Value="1">Motor</asp:ListItem>
                            <asp:ListItem Value="2">Non Motor</asp:ListItem>--%>
    <style type="text/css">
        .page_result {
            padding: 10px;
            font-size: 16px;
            color: white;
        }

        .hidden-field {
            display: none;
        }

        .chkbox_alignment {
            margin-left: 275px;
        }


        /*----------------data Table GridView Css----------------------*/

        /*.SearchGridView_IBT {
            max-height: 275px;
            overflow: auto;
            border: 1px solid #ccc;
        }

        .SearchGridView_IBT {
            border-collapse: collapse;
            width: 100%;
        }

            .SearchGridView_IBT tr th {
                background-color: #428BCA;
                color: #ffffff;
                padding: 10px 5px 10px 5px;
                border: 1px solid #cccccc;
                font-family: 'Open Sans', sans-serif;
                font-size: 14px;
                font-weight: normal;
                text-transform: capitalize;
            }

            .SearchGridView_IBT tr:nth-child(2n+2) {
                background-color: #f3f4f5;
            }

            .SearchGridView_IBT tr:nth-child(2n+1) td {
                background-color: #d6dadf;
                color: #454545;
            }

            .SearchGridView_IBT tr td {
                padding: 5px 10px 5px 10px;
                color: #454545;
                font-family: 'Open Sans', sans-serif;
                font-size: 12px;
                border: 1px solid #cccccc;
                vertical-align: middle;
            }

                .SearchGridView_IBT tr td:first-child {
                    text-align: left;
                }*/




        

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
        /*-----------------------------------------------------------------*/

        .grdHeaderManual {
            height: 40px;
            color: white;
            Font-Size: small;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form runat="server">
        <!-- BEGIN PAGE HEADER-->
        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>Statement Upload Configuration</h3>
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
        <div class="panel-group">
            <div class="panel panel-info">
                <%--panel-success--%>
                <div class="panel-heading">IBT Formats</div>
                <div class="panel-body">

                    <div class="well well-sm">
                        <div class="row">
                            <div class="col-md-2">
                                <label for="txtUserCode">Description</label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtFormatDescription" runat="server" CssClass="form-control" ValidationGroup="vgIBTFormat"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="String Not In Correct Format" ValidationGroup="vgIBTFormat" SetFocusOnError="True" ControlToValidate="txtFormatDescription" ValidationExpression="([a-z]|[A-Z]|[0-9]|[ ]|[-]|[_]|[/])*" Display="Dynamic"></asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-3">
                                <asp:CheckBox ID="cbPolNoAuto" runat="server" Text="Auto Detect Policy Number" ValidationGroup="vgIBTFormat" CssClass="form-control" />
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnInsertFormat" runat="server" Text="Insert Format" CssClass="btn btn-success btn-md" OnClick="btnInsertFormat_Click" ValidationGroup="vgIBTFormat" />
                            </div>
                            <%--               <div class="col-md-2">
                            </div>--%>
                        </div>
                    </div>

                    <div class="row">
                        <asp:Panel ID="Panel3" runat="server" Height="177px" ScrollBars="Vertical" Style="z-index: 102;">
                            <asp:GridView ID="grdUpdateFormats" runat="server" AutoGenerateColumns="False" OnRowDataBound="grdUpdateFormats_RowDataBound" CssClass="SearchGridView_T">
                                <Columns>

                                    <asp:BoundField DataField="ID" HeaderText="ID">
                                        <HeaderStyle CssClass="hidden-field" />
                                        <ItemStyle CssClass="hidden-field" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Bind ("Description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind ("Inactive") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Active">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkStatus" runat="server" AutoPostBack="True" OnCheckedChanged="chkStatus_CheckedChanged" ValidationGroup="vgFormatInactive" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="50px" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle Wrap="False" />
                            </asp:GridView>

                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>




        <%--Panel--%>
        <div class="panel-group">
            <div class="panel panel-info">
                <%--panel-success--%>
                <div class="panel-heading">IBT Accounts</div>
                <div class="panel-body">

                    <div class="well well-sm">
                        <div class="row">
                            <div class="col-md-2">
                                <label for="txtUserCode">Account Number</label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtAccountNo" runat="server" CssClass="form-control" ValidationGroup="vgAccounts"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="String Not In Correct Format" ValidationGroup="vgAccounts" SetFocusOnError="True" ControlToValidate="txtAccountNo" ValidationExpression="([a-z]|[A-Z]|[0-9]|[ ]|[-]|[_]|[/])*" Display="Dynamic"></asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-2">
                                <label for="txtUserCode">Account Name</label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtAccountName" runat="server" CssClass="form-control" ValidationGroup="vgAccounts"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="String Not In Correct Format" ValidationGroup="vgAccounts" SetFocusOnError="True" ControlToValidate="txtAccountName" ValidationExpression="([a-z]|[A-Z]|[0-9]|[ ]|[-]|[_]|[/])*" Display="Dynamic"></asp:RegularExpressionValidator>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-2">
                                &nbsp
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-2">
                                <label>Cash Account Number</label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtCashAcc" runat="server" CssClass="form-control" ValidationGroup="vgAccounts"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="String Not In Correct Format" ValidationGroup="vgAccounts" SetFocusOnError="True" ControlToValidate="txtCashAcc" ValidationExpression="([a-z]|[A-Z]|[0-9]|[ ]|[-]|[_]|[/])*" Display="Dynamic"></asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-2">
                                <label>Product Type</label>
                            </div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlProduct" CssClass="form-control" runat="server" ValidationGroup="vgAccounts">
                                    <asp:ListItem>--- Please Select ---</asp:ListItem>
                                    <asp:ListItem>Life</asp:ListItem>
                                    <asp:ListItem>General</asp:ListItem>
                                    <asp:ListItem>-</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                &nbsp
                            </div>
                            <div class="col-md-1">
                                <asp:Button ID="btnInsertAccount" runat="server" Text="Insert Account" CssClass="btn btn-success btn-md" OnClick="btnInsertAccount_Click" ValidationGroup="vgAccounts" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Style="z-index: 102;">
                            <asp:GridView ID="grdIBTAccounts" runat="server" AutoGenerateColumns="False" CssClass="SearchGridView_T" OnRowDataBound="grdIBTAccounts_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="ACC_NO" HeaderText="Account Number">
                                        <HeaderStyle Width="200px" />
                                        <ItemStyle HorizontalAlign="Center" Width="200px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Account Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Bind ("acc_name") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Status" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind ("Inactive") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cash Account" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCashAcc" runat="server" Text='<%# Bind ("cash_account") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Product" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProduct" runat="server" Text='<%# Bind ("product_description") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Active">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkChangeStatus" runat="server" AutoPostBack="True" OnCheckedChanged="chkChangeStatus_CheckedChanged" ValidationGroup="vgAccountInactive" />
                                        </ItemTemplate>
                                        <HeaderStyle Width="50px" />
                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle Wrap="False" />
                            </asp:GridView>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>







        <%--Panel--%>
        <div class="panel-group">
            <div class="panel panel-info">
                <%--panel-success--%>
                <div class="panel-heading">Assign Format For IBT Account</div>
                <div class="panel-body">

                    <div class="well well-sm">
                        <div class="row">
                            <div class="col-md-2">
                                <label>Account Number</label>
                            </div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlAccounts" runat="server" CssClass="form-control" ValidationGroup="vgAssign">
                                    <asp:ListItem Value="0">---Select---</asp:ListItem>
                                    <%--<asp:ListItem Value="1">Motor</asp:ListItem>
                            <asp:ListItem Value="2">Non Motor</asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <label>IBT Format</label>
                            </div>
                            <div class="col-md-2">
                                <asp:DropDownList ID="ddlFormats" runat="server" CssClass="form-control" ValidationGroup="vgAssign">
                                    <asp:ListItem Value="0">---Select---</asp:ListItem>
                                    <%--<asp:ListItem Value="1">Motor</asp:ListItem>
                            <asp:ListItem Value="2">Non Motor</asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnAssign" runat="server" Text="Assign" CssClass="btn btn-success btn-md" OnClick="btnAssign_Click" ValidationGroup="vgAssign" />
                            </div>
                            <div class="col-md-2">
                            </div>
                        </div>
                    </div>



                    <div class="row">
                        <asp:Panel ID="Panel2" runat="server" Height="140px" ScrollBars="Vertical" Style="z-index: 102;">
                            <asp:GridView ID="grdAssignedFormats" runat="server" AutoGenerateColumns="False" CssClass="SearchGridView_T">
                                <Columns>
                                    <asp:BoundField DataField="acc_no" HeaderText="Account Number">
                                        <HeaderStyle Width="150px" HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" Width="150px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="IBT Format">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Bind ("description") %>'></asp:Label>
                                        </ItemTemplate>
                                        <%--<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                    <ItemStyle HorizontalAlign="Center" />--%>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle Wrap="False" />
                            </asp:GridView>
                        </asp:Panel>

                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
