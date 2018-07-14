<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FAS_Home.aspx.cs" Inherits="BranchMIS.FAS_Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="FAS_Custom.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    </style>
    <%--    <script type="text/javascript">
        function toggleFullScreen() {
            if ((document.fullScreenElement && document.fullScreenElement !== null) ||
             (!document.mozFullScreen && !document.webkitIsFullScreen)) {
                if (document.documentElement.requestFullScreen) {
                    document.documentElement.requestFullScreen();
                } else if (document.documentElement.mozRequestFullScreen) {
                    document.documentElement.mozRequestFullScreen();
                } else if (document.documentElement.webkitRequestFullScreen) {
                    document.documentElement.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT);
                }
            } else {
                if (document.cancelFullScreen) {
                    document.cancelFullScreen();
                } else if (document.mozCancelFullScreen) {
                    document.mozCancelFullScreen();
                } else if (document.webkitCancelFullScreen) {
                    document.webkitCancelFullScreen();
                }
            }
        }
    </script>--%>






</head>
<body>
    <form id="form1" runat="server">
        <div class="wrapper">

                <div>hgghjbjbjbhj</div>

            <%--Logo Image--%>
            <div class="Logo">
                <img alt="" src="assets/img/FAS/SystemLogo.png" />
            </div>


            <%--Navigation Panel Start--%>
            <table class="IndexNavigation">
                <tr>
                    <td>
                        <%--Main Navigation Button 01--%>
                        <%--<div class="dropdown" style="float: left;">--%>


                        <div class="btnContainor">
                            <%--Button Animated Icon--%>
                            <div class="btnLogo">
                                <div class="container">
                                    <asp:Button ID="Button2" runat="server" class="pulse-button" Enabled="false" />
                                </div>
                            </div>
                            <%--End Of Button Animated Icon--%>

                            <%--Main Navigation Button 01 Text--%>
                            <div class="dropdown" style="float: left;">
                                <div class="btnText">
                                    <asp:LinkButton ID="lbtn_AdminControls" runat="server" CssClass="button dropbtn" Enabled="false">IBT Admin Controls</asp:LinkButton>
                                    <%--Sub Menu Items--%>
                                    <div class="dropdown-content" style="left: 0">
                                        <asp:HyperLink ID="h100" runat="server" NavigateUrl="~/IBT/IBT_Upload_Config.aspx">Statement Upload Configuration</asp:HyperLink>
                                        <asp:HyperLink ID="h40" runat="server" NavigateUrl="~/IBT/IBT_Format_Configuration.aspx">Statement Format Configuration</asp:HyperLink>
                                        <asp:HyperLink ID="h80" runat="server" NavigateUrl="~/IBT/IBT_Rules.aspx">Create Rules</asp:HyperLink>
                                        <asp:HyperLink ID="h30" runat="server" NavigateUrl="~/IBT/IBT_Filtering.aspx">Bifurcation Rules Configuration</asp:HyperLink>
                                        <asp:HyperLink ID="h50" runat="server" NavigateUrl="~/IBT/IBT_MatchingRule.aspx">Matching Rules Configuration</asp:HyperLink>
                                        <asp:HyperLink ID="h90" runat="server" NavigateUrl="~/IBT/IBT_TypeOfProductRule.aspx">Product Rules Configuration</asp:HyperLink>
                                        <asp:HyperLink ID="h170" runat="server" NavigateUrl="~/IBT/IBT_Mails.aspx">Mails Configuration</asp:HyperLink>
                                        <asp:HyperLink ID="h140" runat="server" NavigateUrl="~/IBT/UserAdministration.aspx">System Users Administration</asp:HyperLink>

                                    </div>
                                    <%--End Of Sub Menu Items--%>
                                </div>
                            </div>
                        </div>
                        <%--End Of Main Navigation Button 01--%>
                    </td>






                    <td><%--Main Navigation Button 02--%>
                        <div class="btnContainor_02">
                            <%--Button Animated Icon--%>
                            <div class="btnLogo">
                                <div class="container">
                                    <asp:Button ID="Button1" runat="server" class="pulse-button" Enabled="false" />
                                </div>
                            </div>
                            <%--End Of Button Animated Icon--%>

                            <%--Main Navigation Sub Menu Item--%>
                            <div class="dropdown" style="float: left;">
                                <%--Main Navigation Button 02 Text--%>
                                <div class="btnText">
                                    <asp:LinkButton ID="lbtn_UserControls" runat="server" CssClass="button dropbtn" Enabled="false">IBT User Controls</asp:LinkButton>
                                    <%--Sub Menu Items--%>
                                    <div class="dropdown-content" style="left: 0">
                                        <asp:HyperLink ID="h110" runat="server" NavigateUrl="~/IBT/IBTUpload.aspx">Bank Statement Upload</asp:HyperLink>
                                        <asp:HyperLink ID="h20" runat="server" NavigateUrl="~/IBT/IBT_Edit_Uploads.aspx">Edit Statement Data</asp:HyperLink>
                                        <asp:HyperLink ID="h10" runat="server" NavigateUrl="~/IBT/IBT_Confirmation.aspx">IBT Receipt Confirmation</asp:HyperLink>
                                        <asp:HyperLink ID="h70" runat="server" NavigateUrl="~/IBT/IBT_RuleProcessing.aspx">IBT Rules Manual Execute</asp:HyperLink>
                                        <asp:HyperLink ID="h60" runat="server" NavigateUrl="~/IBT/IBT_MT940Files_Download.aspx">MT940 Download</asp:HyperLink>
                                        <asp:HyperLink ID="h150" runat="server" NavigateUrl="~/IBT/IBT_MRP_Confirmation.aspx">Division Confirmation</asp:HyperLink>
                                        <asp:HyperLink ID="h220" runat="server" NavigateUrl="~/IBT/IBT_HistoryUpdate.aspx">IBT Possible Data Update</asp:HyperLink>
                                        <asp:HyperLink ID="h230" runat="server" NavigateUrl="~/IBT/IBT_Records_Suppress.aspx">IBT Suppress Request</asp:HyperLink>
                                        <asp:HyperLink ID="h250" runat="server" NavigateUrl="~/IBT/IBT_Records_Suppress_Approve.aspx">IBT Suppress Approve</asp:HyperLink>
                                        <asp:HyperLink ID="h240" runat="server" NavigateUrl="~/IBT/IBT_Records_Suppress_Confirm.aspx">IBT Suppress Confirm</asp:HyperLink>



                                        <%--<a href="IBT/IBTUpload.aspx">Bank Statement Upload</a>
                                    <a href="IBT/IBT_Edit_Uploads.aspx">Edit Statement Data</a>
                                    <a href="IBT/IBT_Confirmation.aspx">IBT Receipt Confirmation</a>
                                    <a href="IBT/IBT_RuleProcessing.aspx">IBT Rules Manual Execute</a>
                                    <a href="IBT/IBT_MT940Files_Download.aspx">MT940 Download</a>
                                    <a href="IBT/IBT_MRP_Confirmation.aspx">Division Confirmation</a>--%>
                                    </div>
                                    <%--End Of Sub Menu Items--%>
                                </div>
                                <%--End Of Main Navigation Button 02 Text--%>
                            </div>
                        </div>
                        <%--End Of Main Navigation Button 02--%>
                    </td>



                    <td><%--Main Navigation Button 03--%>
                        <div class="btnContainor_02">
                            <%--Button Animated Icon--%>
                            <div class="btnLogo">
                                <div class="container">
                                    <asp:Button ID="Button3" runat="server" class="pulse-button" />
                                </div>
                            </div>
                            <%--End Of Button Animated Icon--%>

                            <%--Main Navigation Button 01 Text--%>
                            <div class="dropdown" style="float: left;">
                                <div class="btnText">
                                    <asp:LinkButton ID="lbtn_ManualUploads" runat="server" CssClass="button dropbtn" Enabled="false">Manual Uploads</asp:LinkButton>
                                    <%--Sub Menu Items--%>
                                    <div class="dropdown-content" style="left: 0">
                                        <asp:HyperLink ID="h190" runat="server" NavigateUrl="~/IBTManual_Uploads/IBT_Life_Uploads.aspx">IBT Life Upload</asp:HyperLink>
                                        <asp:HyperLink ID="h180" runat="server" NavigateUrl="~/IBTManual_Uploads/IBT_General_Uploads.aspx">IBT General Upload</asp:HyperLink>
                                        <asp:HyperLink ID="h260" runat="server" NavigateUrl="~/IBTManual_Uploads/IBT_NonTCS_Uploads.aspx">IBT NonTCS Upload</asp:HyperLink>
                                    </div>
                                    <%--End Of Sub Menu Items--%>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <%--Navigation Panel End--%>



            <div class="UserNameMainDiv">
                <div class="Usr_Image">
                    <img alt="" src="assets/img/FAS/usr_image_01.png" height="35px" width="35px" />
                </div>
                <div class="User_Name">
                    <asp:Label ID="lbl_ADName" runat="server" Text="Authentication Fail"></asp:Label>
                </div>

            </div>



            <%--Home Page System Messege--%>
            <div class="IndexPageWarningMsg" runat="server" visible="false" id="systemMessege">
                <asp:Label ID="lblLoginError" runat="server" Text=""></asp:Label>
            </div>
            <%-- Home Page System Messege End--%>

            <%--<div class="FullScreenButton">
                <input type="input" value="click to toggle fullscreen" onclick="toggleFullScreen()">
            </div>--%>
        </div>
    </form>
</body>
</html>
