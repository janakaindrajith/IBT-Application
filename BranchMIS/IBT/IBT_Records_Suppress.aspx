<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="IBT_Records_Suppress.aspx.cs" Inherits="BranchMIS.IBT.IBT_Records_Suppress" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <form runat="server">

        <!-- BEGIN PAGE HEADER-->
        <div class="row">
            <div class="col-md-12">
                <div class="page-header">
                    <h3>Suppress Records Request</h3>
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
                <div class="col-md-5">
                    <h4>Get Unmatched Transations (6 months earlier from today)</h4>

                </div>
                <div class="col-md-1">
                    <asp:Button ID="btn_getData" runat="server" Width="150px" Height ="50px" Text="GET" OnClick="btn_getData_Click1" CssClass="btn btn-success" />
                </div>
            </div>
        </div>

        <div class="body">
            <div class="panel-body">
                <div class="form-group">
                    <asp:Panel ID="Panel1" runat="server" Height="300px" ScrollBars="Vertical" Style="z-index: 102;"
                        Width="100%" BorderColor="#FFFFFF" BorderWidth="1px">
                        <div class="grdAlign">
                            <asp:GridView ID="grd_suppressData" runat="server" AllowPaging="True" PageSize="13" CssClass="SearchGridView_T" OnRowDataBound="grd_suppressData_RowDataBound">
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
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                </div>
            </div>

            <div class="row">
                <div class="form-group">
                    <div class="col-md-7">
                        <asp:TextBox ID="txtUserComment" runat="server" CssClass="form-control" Rows="3" placeholder="User Comment" TextMode="MultiLine"></asp:TextBox>
 <%--                       <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtUserComment" SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                    </div>
                </div>
                
            </div>
            <div class="row">&nbsp;</div>
            <div class="row">

                    <div class="col-md-2">
                        <asp:Button ID="btn_suppressed" Width="150px" Height ="50px" runat="server" Text="Suppressed" OnClick="btn_suppressed_Click" CssClass="btn btn-warning" />
                    </div>
                </div>
        </div>
    </form>
</asp:Content>
