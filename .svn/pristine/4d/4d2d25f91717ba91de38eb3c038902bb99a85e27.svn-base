<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="IBT_Rules.aspx.cs" Inherits="BranchMIS.IBT.IBT_Rules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .page_result {
            padding: 10px;
            font-size: 16px;
            color: white;
        }

        .hidden-field {
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
                    <h3>Create Rules</h3>
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

        <div class="well well-sm">
            <div class="row">
                <div class="col-md-1">
                    <label>Rule Type</label>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlRuleType" runat="server" CssClass="form-control" ValidationGroup="vgIBT_Rules">
                        <asp:ListItem Value="0">---Select---</asp:ListItem>
                        <asp:ListItem Value="1">IBT Bifurcate</asp:ListItem>
                        <asp:ListItem Value="2">IBT Macthing</asp:ListItem>
                        <asp:ListItem Value="3">IBT Type Of Product</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="col-md-2">
                    <asp:DropDownList ID="ddl_company" runat="server" CssClass="form-control" ValidationGroup="vgIBT_Rules">
                        <%--<asp:ListItem Value="0">---Select---</asp:ListItem>--%>
                        <asp:ListItem Value="LIFE">LIFE</asp:ListItem>
                        <asp:ListItem Value="GENERAL">GENERAL</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <div class="col-md-3">
                    <asp:TextBox ID="txtRule_Description" runat="server" CssClass="form-control" ValidationGroup="vgIBT_Rules" placeholder="Rule Description"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="String Not In Correct Format" ValidationGroup="vgIBT_Rules" SetFocusOnError="True" ControlToValidate="txtRule_Description" ValidationExpression="([a-z]|[A-Z]|[0-9]|[ ]|[-]|[_]|[/])*" Display="Dynamic"></asp:RegularExpressionValidator>
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btnInsertRule" runat="server" Text="Insert Rule" ValidationGroup="vgIBT_Rules" CssClass="btn btn-success btn-md" OnClick="btnInsertRule_Click" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>Available Rules</h3>
                </div>
            </div>
        </div>



        <div class="well well-sm">
            <div class="row">
                <div class="col-md-1">
                    <label>Rule Type</label>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlRuleTypeSearch" runat="server" CssClass="form-control" ValidationGroup="vgIBT_Rules" AutoPostBack="true" OnSelectedIndexChanged="ddlRuleTypeSearch_SelectedIndexChanged">
                        <asp:ListItem Value="0">---Select---</asp:ListItem>
                        <asp:ListItem Value="1">IBT Bifurcate</asp:ListItem>
                        <asp:ListItem Value="2">IBT Macthing</asp:ListItem>
                        <asp:ListItem Value="3">IBT Type Of Product</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>

        <div class="panel-group">
            <div class="panel panel-info">
                <%--panel-success--%>
                <%--<div class="panel-heading">Test</div>--%>
                <div class="panel-body">
                    <asp:Panel ID="pnlUserGrid" runat="server" Height="380px" ScrollBars="Vertical">
                        <asp:GridView ID="grdRules" runat="server" AutoGenerateColumns="False" OnRowDataBound="grdRules_RowDataBound" CssClass="SearchGridView_T">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderText="ID">
                                    <HeaderStyle Width="50px" CssClass="hidden-field" />
                                    <ItemStyle HorizontalAlign="Center" Width="50px" CssClass="hidden-field" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind ("Description") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="rule" HeaderText="Rule">
                                    <%-- <HeaderStyle Width="650px" />
                                                <ItemStyle HorizontalAlign="Left" Width="300px" />--%>
                                </asp:BoundField>
                                <asp:BoundField DataField="product_id" HeaderText="Product">
                                    <%--<HeaderStyle Width="650px" />
                                                <ItemStyle HorizontalAlign="Left" Width="350px" />--%>
                                </asp:BoundField>
                                <asp:BoundField DataField="dept_id" HeaderText="Category">
                                    <%--<HeaderStyle Width="650px" />
                                                <ItemStyle HorizontalAlign="Left" Width="350px" />--%>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Status" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind ("Inactive") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkStatus" runat="server" AutoPostBack="True" OnCheckedChanged="chkStatus_CheckedChanged" ValidationGroup="vgRuleActive" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="50px" />
                                    <ItemStyle HorizontalAlign="Center" Width="50px" />
                                </asp:TemplateField>
                            </Columns>
                            <RowStyle Wrap="False" />
                        </asp:GridView>
                    </asp:Panel>
                    <asp:LinkButton ID="btnSetFocus" runat="server" Text="DeshapriyaTest" ForeColor="White"></asp:LinkButton>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
