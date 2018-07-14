<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="IBT_Format_Configuration.aspx.cs" Inherits="BranchMIS.IBT.IBT_Format_Configuration" %>

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

        .btns_allignment {
            margin-right: -15px;
            margin-left: 900px;
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

        .btnPanel {
            margin-top: 50px;
            padding-left: 850px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <form runat="server">
        <!-- BEGIN PAGE HEADER-->
        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>IBT Format Configuration</h3>
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
                <div class="col-md-2">
                    <label for="txtUserCode">Account Type: </label>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlIBTFormat" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlIBTFormat_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
        </div>



        <div class="panel-group">
            <div class="panel panel-info">
                <%--panel-success--%>
                <div class="panel-heading">Available Format</div>
                <div class="panel-body">

                    <asp:Panel ID="Panel1" runat="server" Height="400px" ScrollBars="Vertical" Style="z-index: 102;"
                        Width="100%">                      

                            <asp:GridView ID="grdInsert" runat="server" AutoGenerateColumns="False" CssClass="SearchGridView_T" OnRowDataBound="grdInsert_RowDataBound">
                                <EditRowStyle Height="30px" />
                                <FooterStyle BackColor="White" ForeColor="#000066" />

                                <Columns>
                                    <asp:BoundField DataField="FORMAT_ID" HeaderText="Format ID">
                                        <HeaderStyle Width="80px" />
                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="COLUMN_ID" HeaderText="Column ID">

                                        <ItemStyle Width="80px" />
                                    </asp:BoundField>

                                    <asp:TemplateField HeaderText="Description" ItemStyle-Width="270">
                                        <ItemTemplate>
                                            <asp:Label ID="lblformatID" runat="server" Text='<%# Eval("DESCRIPTION") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ControlStyle Width="270px" />
                                        <HeaderStyle Width="270px" />
                                        <ItemStyle Width="270px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Column Index" ItemStyle-Width="150">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlCol_Index" runat="server" CssClass="form-control">
                                                <%--<asp:ListItem Text="-" Value="0" Selected="False"></asp:ListItem>--%>
                                                <asp:ListItem Text="A" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="B" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="C" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="D" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="E" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="F" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="G" Value="7"></asp:ListItem>
                                                <asp:ListItem Text="H" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="I" Value="9"></asp:ListItem>
                                                <asp:ListItem Text="J" Value="10"></asp:ListItem>
                                                <asp:ListItem Text="K" Value="11"></asp:ListItem>
                                                <asp:ListItem Text="L" Value="12"></asp:ListItem>
                                                <asp:ListItem Text="M" Value="13"></asp:ListItem>
                                                <asp:ListItem Text="N" Value="14"></asp:ListItem>
                                                <asp:ListItem Text="O" Value="15"></asp:ListItem>
                                                <asp:ListItem Text="P" Value="16"></asp:ListItem>
                                                <asp:ListItem Text="Q" Value="17"></asp:ListItem>
                                                <asp:ListItem Text="R" Value="18"></asp:ListItem>
                                                <asp:ListItem Text="S" Value="19"></asp:ListItem>
                                                <asp:ListItem Text="T" Value="20"></asp:ListItem>
                                                <asp:ListItem Text="U" Value="21"></asp:ListItem>
                                                <asp:ListItem Text="V" Value="22"></asp:ListItem>
                                                <asp:ListItem Text="W" Value="23"></asp:ListItem>
                                                <asp:ListItem Text="X" Value="24"></asp:ListItem>
                                                <asp:ListItem Text="Y" Value="25"></asp:ListItem>
                                                <asp:ListItem Text="Z" Value="26"></asp:ListItem>
                                            </asp:DropDownList>
                                            <br />
                                        </ItemTemplate>
                                        <ControlStyle Width="150px" />
                                        <HeaderStyle Width="150px" />
                                        <ItemStyle Width="150px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Row Index" ItemStyle-Width="70">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRowIndex" runat="server" Text='<%# Eval("ROW_INDEX") %>' Width="45" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ControlToValidate="txtRowIndex"></asp:RequiredFieldValidator>
                                            <asp:RangeValidator ID="RangeValidator5" runat="server" ErrorMessage="*" SetFocusOnError="True" MinimumValue="-1" MaximumValue="99" Type="Integer" ControlToValidate="txtRowIndex"></asp:RangeValidator>
                                        </ItemTemplate>
                                        <ControlStyle Width="70px" />
                                        <HeaderStyle Width="70px" />
                                        <ItemStyle Width="70px" />
                                    </asp:TemplateField>



                                    <asp:TemplateField HeaderText="Column Order" ItemStyle-Width="70" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Text='<%# Eval("COLUMN_ORDER") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ControlStyle Width="70px" />
                                        <HeaderStyle Width="70px" />
                                        <ItemStyle Width="70px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Start Index" ItemStyle-Width="70">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtStartIndex" runat="server" Text='<%# Eval("START_INDEX") %>' Width="45" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ControlToValidate="txtStartIndex"></asp:RequiredFieldValidator>
                                            <asp:RangeValidator ID="RangeValidator2" runat="server" ErrorMessage="*" SetFocusOnError="True" MinimumValue="0" MaximumValue="99" Type="Integer" ControlToValidate="txtStartIndex"></asp:RangeValidator>
                                        </ItemTemplate>
                                        <ControlStyle Width="70px" />
                                        <HeaderStyle Width="70px" />
                                        <ItemStyle Width="70px" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Length" ItemStyle-Width="70">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtEndIndex" runat="server" Text='<%# Eval("END_INDEX") %>' Width="45" CssClass="form-control"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ControlToValidate="txtEndIndex"></asp:RequiredFieldValidator>
                                            <asp:RangeValidator ID="RangeValidator3" runat="server" ErrorMessage="*" SetFocusOnError="True" MinimumValue="0" MaximumValue="99" Type="Integer" ControlToValidate="txtEndIndex"></asp:RangeValidator>
                                        </ItemTemplate>
                                        <ControlStyle Width="70px" />
                                        <HeaderStyle Width="70px" />
                                        <ItemStyle Width="70px" />
                                    </asp:TemplateField>

                                </Columns>

                                <RowStyle ForeColor="Black" BackColor="White" Height="15px" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" Height="30px" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" Font-Names="Tahoma" Font-Size="Larger" ForeColor="White" Height="20px" />
                                <AlternatingRowStyle BackColor="WhiteSmoke" Font-Names="Tahoma" Font-Size="8pt" Height="15px" />
                            </asp:GridView>
                    
                    </asp:Panel>
                    <div class="row">
                        <div class="col-md-2">
                            &nbsp;
                        </div>
                    </div>


                    <div class="row">
                        <div class="col-md-9">
                            &nbsp;
                            </div>
                        <div class="col-md-1">
                            <asp:Button ID="btnInsert" runat="server" CssClass="btn btn-success btn-md" Text="Insert" OnClick="btnInsert_Click" Visible="False" />
                        </div>
                        <div class="col-md-1">
                            <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update" CssClass="btn btn-success btn-md" Visible="False" />
                        </div>
                        <div class="col-md-1">
                            <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-success btn-md" Text="Cancel" OnClick="btnCancel_Click" Visible="false" ValidationGroup="vgCancel" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>


</asp:Content>
