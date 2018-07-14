<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="IBT_Filtering.aspx.cs" Inherits="BranchMIS.IBT.IBT_Filtering" %>

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
                    <h3>IBT Bifurcation Rules Configuration</h3>
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
                <div class="col-md-3">
                    <h4>Bifurcation Rules Available</h4>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlIBTRules" AutoPostBack="true" runat="server" CssClass="form-control" ValidationGroup="UpdateValidate" OnSelectedIndexChanged="ddlIBTRules_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div class="col-md-7">
                    <asp:TextBox ID="txtDescription" Enabled="false" TextMode="MultiLine" CssClass="form-control" Rows="2" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>Add New Bifurcation Rule</h3>
                </div>
            </div>
            <%--<div class="col-md-4">.col-md-4</div>--%>
        </div>


        <div class="well well-lg">
            <div class="row">
                <div class="col-md-1">
                    <h5>IBT Account:</h5>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlIbtAcc" runat="server" CssClass="form-control" ValidationGroup="UpdateValidate">
                    </asp:DropDownList>
                </div>
                <div class="col-md-1">
                    <h5>Types:</h5>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddlTrans" runat="server" CssClass="form-control" ValidationGroup="UpdateValidate">
                        <asp:ListItem Value="0">---Please Select---</asp:ListItem>
                        <asp:ListItem Value="1">All</asp:ListItem>
                        <asp:ListItem Value="2">Debit</asp:ListItem>
                        <asp:ListItem Value="3">Credit</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <h5>Filtering Column:</h5>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="DDLRules" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <asp:Button ID="cmdAddRule" runat="server" Text="Continue" CssClass="btn btn-success btn-md" OnClick="cmdAddRule_Click" />
                

                    <asp:Label ID="lblCount" runat="server" Font-Bold="True"  style="text-align:right;" Width="100px" Font-Size="Medium"></asp:Label>
                

                </div>
            </div>
        </div>



        <%--Panel--%>
        <div class="panel-group">
            <div class="panel panel-info">
                <%--panel-success--%>
                <div class="panel-heading">Conditions</div>
                <div class="panel-body">
                    <asp:Panel ID="Panel3" runat="server" Height="200px" ScrollBars="Vertical" Style="z-index: 102;">
                        <asp:GridView ID="GrdRules" runat="server" CssClass="SearchGridView_T" AutoGenerateColumns="False" OnRowDataBound="GrdRules_RowDataBound" OnRowDeleting="GrdRules_RowDeleting">
                            <Columns>
                                <asp:CommandField ShowDeleteButton="True" ItemStyle-Width="70px" />

                                <asp:TemplateField ItemStyle-Width="100px" HeaderStyle-VerticalAlign="NotSet" HeaderText="Condition">
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



                                <asp:TemplateField HeaderText="Description" ItemStyle-Width="190">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Visible="true" Text='<%# Eval("DESCRIPTION") %>'></asp:Label>
                                        <asp:TextBox ID="txtDescription" runat="server" Text='<%# Eval("DESCRIPTION") %>' Visible="false" Enabled="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle Width="80px" HorizontalAlign="Left" />
                                </asp:TemplateField>



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
                                            <asp:ListItem Value="8">Not Like</asp:ListItem>
                                            <asp:ListItem Value="9">In</asp:ListItem>
                                            <asp:ListItem Value="10">Not In</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <HeaderStyle VerticalAlign="Middle" />
                                    <ItemStyle Width="150px" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Value" ItemStyle-Width="190">
                                    <ItemTemplate>
                                        <%--<asp:TextBox ID="txtDescription" runat="server" Text='<%# Eval("DESCRIPTION") %>' Visible="true" Enabled="true" CssClass="form-control"></asp:TextBox>--%>
                                        <asp:TextBox ID="txtValue" runat="server" Visible="true" Enabled="true" CssClass="form-control"></asp:TextBox>
                                    </ItemTemplate>
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


                            </Columns>

                        </asp:GridView>
                    </asp:Panel>
                </div>
            </div>
        </div>


        <div class="well well-lg">
            <div class="row">
                <%--<div class="col-md-1">
                <h5>Condition:</h5>
            </div>--%>
                <div class="col-md-12">
                    <asp:TextBox ID="txtViewRule" Enabled="false" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="2"></asp:TextBox>
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
                    <asp:Button ID="cmdViewRule" runat="server" Text="View Rule" CssClass="btn btn-primary btn-md" OnClick="cmdViewRule_Click" />
                </div>
                <div class="col-md-1">
                    <asp:Button ID="cmdSaveRule" runat="server" Text="Save Rule" CssClass="btn btn-success btn-md" OnClick="cmdSaveRule_Click" ValidationGroup="UpdateValidate" />
                </div>
            </div>
        </div>
    </form>

</asp:Content>
