<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="IBT_TypeOfProductRule.aspx.cs" Inherits="BranchMIS.IBT.IBT_TypeOfProductRule" %>

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

        hr.style-two {
            border: 0;
            height: 1px;
            background-image: linear-gradient(to right, rgba(0, 0, 0, 0), #3b9aac, rgba(0, 0, 0, 0));
        }

        /*----------------data Table GridView Css----------------------*/



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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <form runat="server">
        <!-- BEGIN PAGE HEADER-->
        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>IBT Product Rules Configuration</h3>
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




        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>Create New Data Category</h3>
                </div>
            </div>
            <%--<div class="col-md-4">.col-md-4</div>--%>
        </div>


        <div class="well well-lg">
            <div class="row">
                <div class="col-md-2">
                    <h5>Category Name:</h5>
                </div>
                <div class="col-md-3">
                    <asp:TextBox ID="txtDepartment" runat="server" CssClass="form-control" ValidationGroup="vg_addDepartment"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="String Not In Correct Format" ValidationGroup="vg_addDepartment" SetFocusOnError="True" ControlToValidate="txtDepartment" ValidationExpression="([a-z]|[A-Z]|[0-9]|[ ]|[-]|[_])*" Display="Dynamic"></asp:RegularExpressionValidator>
                </div>
                <div class="col-md-2">
                    <h5>Type:</h5>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control">
                        <asp:ListItem Value="0">---Please Select---</asp:ListItem>
                        <asp:ListItem Value="1">New Business</asp:ListItem>
                        <asp:ListItem Value="2">Renewal</asp:ListItem>
                        <asp:ListItem Value="3">Non TCS</asp:ListItem>
                        <asp:ListItem Value="4">Other</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-md-2">
                    &nbsp;
                </div>
            </div>
            <div class="row">
                <div class="col-md-2">
                    <h5>Policy Fee:</h5>
                </div>
                <div class="col-md-3">
                    <asp:TextBox ID="txtPolicyFee" runat="server" CssClass="form-control" ValidationGroup="vg_addDepartment"></asp:TextBox>
                </div>


                <div class="col-md-2">
                    <h5>Transaction Code:</h5>
                </div>
                <div class="col-md-3">
                    <asp:TextBox ID="txtTransactionCode" runat="server" CssClass="form-control" ValidationGroup="vg_addDepartment"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btnInsert" runat="server" Text="Create" CssClass="btn btn-success btn-md" ValidationGroup="vg_addDepartment" OnClick="btnInsert_Click" />
                </div>
            </div>
        </div>


        <%--Panel--%>
        <div class="panel-group">
            <div class="panel panel-info">
                <%--panel-success--%>
                <div class="panel-heading">Available Data Categories</div>
                <div class="panel-body">
                    <asp:Panel ID="Panel1" runat="server" Height="200px" ScrollBars="Vertical">
                        <asp:GridView ID="grdDataCategory" runat="server" AutoGenerateColumns="False" CssClass="SearchGridView_T" ShowHeader="true" OnRowDataBound="grdDataCategory_RowDataBound">
                            <Columns>

                                <asp:BoundField DataField="Value" HeaderText="Value">
                                    <HeaderStyle CssClass="HiddenCol" />
                                    <ItemStyle CssClass="HiddenCol" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind ("DEPARTMENT_DESCRIPTION") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblType" runat="server" Text='<%# Bind ("Type") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Transaction Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransactionCode" runat="server" Text='<%# Bind ("transaction_code") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Policy Fee">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPolicyFee" runat="server" Text='<%# Bind ("policy_fee") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Status" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind ("INACTIVE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkStatus" runat="server" AutoPostBack="True" ValidationGroup="vgFormatInactive" OnCheckedChanged="chkStatus_CheckedChanged" />
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


        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>Create New Product Rule</h3>
                </div>
            </div>
            <%--<div class="col-md-4">.col-md-4</div>--%>
        </div>



        <div class="well well-lg">
            <div class="row">
                <div class="col-md-3">
                    <h4>Product Rules Available:</h4>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlIBTRules" AutoPostBack="true" runat="server" CssClass="form-control" ValidationGroup="vgAddrule" OnSelectedIndexChanged="ddlIBTRules_SelectedIndexChanged1">
                    </asp:DropDownList>
                </div>
                <div class="col-md-7">
                    <asp:TextBox ID="txtDescription" Enabled="false" TextMode="MultiLine" CssClass="form-control" Rows="2" runat="server" ValidationGroup="vgAddrule"></asp:TextBox>
                </div>
            </div>
        </div>

        <div class="well well-lg">
            <div class="row">
                <div class="col-md-1">
                    <h5>Product:</h5>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlAllProducts" runat="server" CssClass="form-control" ValidationGroup="vgAddrule">
                    </asp:DropDownList>
                </div>
                <div class="col-md-1">
                    <h5>Category:</h5>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control" ValidationGroup="vgAddrule">
                        <asp:ListItem Value="0">---Please Select---</asp:ListItem>
                        <asp:ListItem Value="1">MCP</asp:ListItem>
                        <asp:ListItem Value="1">MCR</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <h5>Filtering Column:</h5>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="DDLRules" runat="server" CssClass="form-control" ValidationGroup="vgAddrule">
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <asp:Button ID="cmdAddRule" runat="server" Text="Create" CssClass="btn btn-success btn-md" OnClick="cmdAddRule_Click" ValidationGroup="vgAddrule" />
                </div>
            </div>
        </div>



        <%--Panel--%>
        <div class="panel-group">
            <div class="panel panel-info">
                <%--panel-success--%>
                <div class="panel-heading">Conditions</div>
                <div class="panel-body">
                    <asp:Panel ID="Panel3" runat="server" Height="300px" ScrollBars="Vertical">
                        <asp:GridView ID="GrdRules" runat="server" CssClass="SearchGridView_T" OnRowDataBound="GrdRules_RowDataBound" OnRowDeleting="GrdRules_RowDeleting">
                            <Columns>
                                <asp:CommandField ShowDeleteButton="True" />
                                <asp:TemplateField HeaderStyle-VerticalAlign="NotSet" HeaderText="Condition" ItemStyle-Width="100px">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label1" runat="server" Text="Condition"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlConditions" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="0">--</asp:ListItem>
                                            <asp:ListItem Value="1">And</asp:ListItem>
                                            <asp:ListItem Value="2">Or</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <HeaderStyle VerticalAlign="NotSet" />
                                    <ItemStyle Width="50px" />
                                </asp:TemplateField>




                                <asp:TemplateField ItemStyle-Width="10px" HeaderStyle-VerticalAlign="NotSet" HeaderText="Condition">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label1" runat="server" Text="Condition"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlOpenBracket" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="0">--</asp:ListItem>
                                            <asp:ListItem Value="1">(</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <HeaderStyle VerticalAlign="NotSet" />
                                    <ItemStyle Width="50px" />
                                </asp:TemplateField>



                                <asp:TemplateField HeaderText="Column ID" ItemStyle-Width="150" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblcolID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                        <asp:TextBox ID="txtcolID" runat="server" Text='<%# Eval("ID") %>' Visible="true" Enabled="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>



                                <asp:TemplateField HeaderText="Statement Details" ItemStyle-Width="190">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Visible="true" Text='<%# Eval("DESCRIPTION") %>'></asp:Label>
                                        <asp:TextBox ID="txtDescription" runat="server" Text='<%# Eval("DESCRIPTION") %>' Visible="false" Enabled="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="80px" HorizontalAlign="Left" />
                                </asp:TemplateField>


                                <%--Operator--%>

                                <asp:TemplateField ItemStyle-Width="100px" HeaderStyle-VerticalAlign="Middle" HeaderText="Operator">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label1" runat="server" Text="Operator"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlOperator" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="0">---Select---</asp:ListItem>
                                            <asp:ListItem Value="1">Equal to</asp:ListItem>
                                            <asp:ListItem Value="2">Not equal to</asp:ListItem>
                                            <asp:ListItem Value="3">Less than</asp:ListItem>
                                            <asp:ListItem Value="4">Less than or equal to</asp:ListItem>
                                            <asp:ListItem Value="5">Greater than</asp:ListItem>
                                            <asp:ListItem Value="6">Greater than or equal to</asp:ListItem>
                                            <asp:ListItem Value="7">Like</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <HeaderStyle VerticalAlign="Middle" />
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>


                                <%--Value--%>

                                <asp:TemplateField HeaderText="Value" ItemStyle-Width="190">
                                    <ItemTemplate>
                                        <%--<asp:TextBox ID="txtDescription" runat="server" Text='<%# Eval("DESCRIPTION") %>' Visible="true" Enabled="true" CssClass="form-control"></asp:TextBox>--%>
                                        <asp:TextBox ID="txtValue"
                                            ToolTip=" LEFT(Index) &#10; RIGHT(Index) &#10; MID(StartingIndex,Length) &#10;"
                                            runat="server" Visible="true" Enabled="true" CssClass="form-control"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>


                                <%--Policy Details--%>

                                <asp:TemplateField ItemStyle-Width="100px" HeaderStyle-VerticalAlign="Middle" HeaderText="User Input">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label1" runat="server" Text="User Input"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtUsrInput" runat="server" Visible="true" Enabled="true" CssClass="form-control" ValidationGroup="vgUsrInput"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUsrInput" Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="vgUsrInput"></asp:RequiredFieldValidator>
                                    </ItemTemplate>
                                    <HeaderStyle VerticalAlign="Middle" />
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>




                                <asp:TemplateField ItemStyle-Width="10px" HeaderStyle-VerticalAlign="NotSet" HeaderText="Condition">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label1" runat="server" Text="Condition"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlCloseBracket" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="0">--</asp:ListItem>
                                            <asp:ListItem Value="1">)</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <HeaderStyle VerticalAlign="NotSet" />
                                    <ItemStyle Width="50px" />
                                </asp:TemplateField>

                                <asp:TemplateField ItemStyle-Width="10px" HeaderStyle-VerticalAlign="NotSet" HeaderText="Date Time" Visible="false">
                                    <HeaderTemplate>
                                        <asp:Label ID="Label1" runat="server" Text="Date Time"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDateTime" runat="server" Text='<%# Eval("CreateDate") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle VerticalAlign="NotSet" />
                                    <ItemStyle Width="50px" />
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </asp:Panel>

                </div>
            </div>
        </div>

        <div class="well well-lg">
            <div class="row">
                <div class="col-md-12">
                    <asp:TextBox ID="txtViewRule" Enabled="false" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-md-10">
                    &nbsp;
                </div>
            </div>
            <div class="row">
                <div class="col-md-10">
                    &nbsp;
                </div>
                <div class="col-md-1">
                    <asp:Button ID="cmdViewRule" runat="server" Text="View Rule" CssClass="btn btn-primary btn-md" OnClick="cmdViewRule_Click" ValidationGroup="vgUsrInput" />
                </div>
                <div class="col-md-1">
                    <asp:Button ID="cmdSaveRule" runat="server" Text="Save Rule" CssClass="btn btn-success btn-md" ValidationGroup="UpdateValidate" OnClick="cmdSaveRule_Click" />
                </div>
            </div>
        </div>
    </form>
</asp:Content>
