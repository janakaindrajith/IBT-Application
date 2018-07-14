<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UserAdministration.aspx.cs" Inherits="BranchMIS.IBT.UserAdministration" %>

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


        /*-------------------------------Popup-----------------------------------------------*/
        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }

        .modalPopup {
            background-color: #FFFFFF;
            width: 800px; /*height: 610px;*/
            height: 200px;
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
            width: 500px;
            font-size: 12px;
            padding: 3px 3px 3px 3px;
        }

        .PopupDropDown {
            border: 1px solid #c4c4c4;
            height: 35px;
            width: 200px;
            font-size: 12px;
            padding: 3px 3px 3px 3px;
        }






        .PopupHeaderLable {
            font-size: large;
            color: white;
        }


        .HeaderTopic {
            text-align: left;
            font-size: large;
            color: white;
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

        /*-------------------------------------------------------------------------*/



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

        .checkboxlist-inline li, .radiobuttonlist-inline li {
            display: inline-block;
        }

        .checkboxlist-inline, .radiobuttonlist-inline {
            margin-left: 20px;
        }

            .checkboxlist-inline label, .radiobuttonlist-inline label {
                padding-left: 0;
                padding-right: 30px;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <form runat="server">
        <!-- BEGIN PAGE HEADER-->
        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>System Administration</h3>
                </div>
            </div>
            <%--<div class="col-md-4">.col-md-4</div>--%>
        </div>

        <div class="row" runat="server">
            <div class="col-md-12">
                <div runat="server" role="alert" visible="true" id="page_result">
                    <asp:Label ID="lblResult" runat="server" Text="" Visible="false"></asp:Label>
                    <asp:Button ID="btnSetFocusTop1" BackColor="White" ForeColor="White" BorderStyle="None" runat="server" Text="|" CssClass="HiddenCol" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-9">
                &nbsp;
            </div>
            <div class="col-md-3">
                <asp:LinkButton ID="lb_createUserRole" runat="server" OnClick="lb_createUserRole_Click" Enabled="false" Visible="false">Create User Role And Assign Users</asp:LinkButton>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>Assign Pages</h3>
                </div>
            </div>
            <%--<div class="col-md-4">.col-md-4</div>--%>
        </div>

        <div class="well well-lg">

            <div class="row">
                <div class="col-md-1">
                    <h5>User Role:</h5>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddl_userRoles" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddl_userRoles_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div class="col-md-6">
                    <asp:CheckBoxList ID="chkb_IBTPages" runat="server" CssClass="checkbox-list inline">
                    </asp:CheckBoxList>
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btn_assignPages" runat="server" Text="Assign Pages" CssClass="btn btn-success btn-md" OnClick="btn_assignPages_Click" />
                    <asp:Button ID="btnSetFocusTop2" BackColor="White" ForeColor="White" BorderStyle="None" runat="server" Text="|" />
                </div>
            </div>

        </div>





        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>Assign Data Categories</h3>
                </div>
            </div>
            <%--<div class="col-md-4">.col-md-4</div>--%>
        </div>

        <div class="well well-lg">

            <div class="row">
                <div class="col-md-1">
                    <h5>User Role:</h5>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddl_userRoles_D_Category" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddl_userRoles_D_Category_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                </div>
                <div class="col-md-6">
                    <asp:CheckBoxList ID="chkb_IBT_Departments" runat="server" CssClass="checkbox-list inline">
                    </asp:CheckBoxList>
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btn_Assign_DataCategories" runat="server" Text="Assign Data Categories" CssClass="btn btn-success btn-md" OnClick="btn_Assign_DataCategories_Click" />
                </div>
            </div>

        </div>





        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>Assign Form Controls</h3>
                </div>
            </div>
            <%--<div class="col-md-4">.col-md-4</div>--%>
        </div>

        <div class="well well-lg">

            <div class="row">
                <div class="col-md-1">
                    <h5>User Role:</h5>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddl_usrRoles_PageControls" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddl_usrRoles_PageControls_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div class="col-md-1">
                    <h5>Page:</h5>
                </div>
                <div class="col-md-2">
                    <asp:DropDownList ID="ddl_assignedPages" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddl_assignedPages_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <div class="col-md-1">
                    <h5>Control:</h5>
                </div>
                <div class="col-md-3">
                    <asp:CheckBoxList ID="chkb_IBT_FormControls" runat="server" CssClass="checkbox-list inline">
                    </asp:CheckBoxList>
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btnSetFocusTop4" BackColor="White" ForeColor="White" BorderStyle="None" runat="server" Text="|" />
                    <asp:Button ID="btnAssignFormControls" runat="server" Text="Assign Form Controls" CssClass="btn btn-success btn-md" OnClick="btnAssignFormControls_Click" />
                </div>
            </div>

        </div>











        <%--Panel--%>
        <div class="panel-group">
            <div class="panel panel-info">
                <%--panel-success--%>
                <div class="panel-heading">IBT Users</div>
                <div class="panel-body">
                    <asp:Panel ID="pnlgrdIBTUsers" runat="server" Height="462px" ScrollBars="Both">
                        <asp:GridView ID="grdIBTUsers" runat="server" CssClass="SearchGridView_T" AutoGenerateColumns="false" OnRowDataBound="grdIBTUsers_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="user_name" HeaderText="User Code" ReadOnly="true"></asp:BoundField>

                                <asp:TemplateField HeaderText="User Role">
                                    <ItemTemplate>
                                        <asp:Label ID="grd_lblUserRole" runat="server" CssClass="form-control" Text='<%#Eval("role_description")%>' Visible="false"></asp:Label>
                                        <asp:DropDownList ID="grd_ddlUserRole" runat="server">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="User Status">
                                    <ItemTemplate>
                                        <asp:Label ID="grd_lblUserStatus" runat="server" CssClass="form-control" Text='<%#Eval("inactive")%>' Visible="false"></asp:Label>
                                        <asp:DropDownList ID="grd_userStatus" runat="server">
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Update">
                                    <ItemTemplate>
                                        <asp:Button ID="btn_update" runat="server" Text="Update" OnClick="Update_Click" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                    <asp:Button ID="btnSetFocusTop5" BackColor="White" ForeColor="White" BorderStyle="None" runat="server" Text="|" />
                </div>
            </div>
        </div>







        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <%--popup one--%>

        <asp:LinkButton Text="" ID="lnkFake" runat="server" />
        <cc1:ModalPopupExtender ID="mpe" runat="server" PopupControlID="pnlPopup" TargetControlID="lnkFake" CancelControlID="btnClose" BackgroundCssClass="modalBackground"></cc1:ModalPopupExtender>

        <%--Style="display:none"--%>
        <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none">
            <div class="header_a" runat="server" id="popupHeader">
                <div class="HeaderTopic">
                    <table style="width: 100%;">
                        <tr>
                            <td>IBT Users Administration 
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
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="User Role"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUserRole" runat="server" ValidationGroup="vg_UserRoleAssign"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtUserRole" Display="Dynamic" ErrorMessage="String Error!" SetFocusOnError="True" ValidationGroup="vg_UserRoleAssign" ValidationExpression="([A-Z]|[a-z]|[1-9])*"></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="Company"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCompany" runat="server" ValidationGroup="vg_UserRoleAssign" CssClass="PopupDropDown">
                                    <asp:ListItem>--Please Selaect--</asp:ListItem>
                                    <asp:ListItem>ALL</asp:ListItem>
                                    <asp:ListItem>LIFE</asp:ListItem>
                                    <asp:ListItem>GENERAL</asp:ListItem>
                                </asp:DropDownList>
                                <asp:Button ID="btn_CreateRole" runat="server" OnClick="btn_CreateRole_Click" Text="Create Role" ValidationGroup="vg_UserRoleAssign" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="User Name"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txt_UserName" runat="server" ValidationGroup="vg_AssignUser"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="User Role"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_UserRole_popup" runat="server" ValidationGroup="vg_AssignUser" CssClass="PopupDropDown">
                                </asp:DropDownList>
                                <asp:Button ID="btnAssignUser" runat="server" OnClick="btnAssignUser_Click" Text="Assign User" ValidationGroup="vg_AssignUser" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </div>
            </div>
        </asp:Panel>
    </form>
</asp:Content>
