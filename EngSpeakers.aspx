<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EngSpeakers.aspx.cs" Inherits="EngSpeakers" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="obout_Flyout2_NET" Namespace="OboutInc.Flyout2" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="Scripts/jquery-1.11.0.min.js"></script>
    <script src="Scripts/jquery-migrate-1.2.1.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.balloon.js" type="text/javascript"></script>
    <script src="Scripts/jquery.shadow.js" type="text/javascript"></script>
    <script src="Scripts/jquery.tile.js" type="text/javascript"></script>
    <script src="Scripts/cluetip/jquery.cluetip.min.js" type="text/javascript"></script>
    <link href="Scripts/cluetip/jquery.cluetip.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        //set up popup balloons

        $(document).ready(function () {
            // Handler for .ready() called.
            //                BindEvents();
        });

        function BindEvents() {
            //Here your jQuery function calls go

            $('a.PaperItemSummary').each(function () {

                $(this).cluetip(
                        {
                            activation: 'click',
                            ajaxcache: false,
                            sticky: true,
                            arrows: true,
                            closeText: '<img src="Scripts/cluetip/images/cross.png" alt="close" style="border:1px solid red" />',
                            closePosition: 'title',
                            width: 460,
                            cluetipClass: 'jtip'
                        }
                    );
            });
        }

    </script>
    <style type="text/css">
        h3
        {
            color: White;
        }
        a:link, a:active
        {
            color: #1E076A;
        }
        a:hover
        {
            color: #EC7A08;
        }
        a:visited
        {
            color: #1E076A;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <asp:Panel ID="pnl_Stage" runat="server" Width="1000px" Style="margin: auto; position: relative;">\
   
   <asp:UpdatePanel ID="upSearchToole" runat="server">
   <ContentTemplate>
    <table style="width:100%">
    <tr>
    <td>
    

        <div id="search_tools" class="floaterR">
            <h3>
                <a href="javascript:void()">Search Tools</a></h3>
        </div>
        <cc1:Flyout ID="Flyout1" AttachTo="search_tools" Position="BOTTOM_CENTER" Align="RIGHT"
            Opacity="85" runat="server">
            <asp:Panel ID="pnl_GroupAndSort" Width="300" CssClass="floaterR" Style="margin-bottom: 10px;
                padding: 7px; border: 1px solid silver; background-color: DarkSlateGray; color: White"
                runat="server">
                <table>
                    <tr>
                        <td colspan="2" style="padding-bottom: 10px">
                            <h3>
                                Group & Filter</h3>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-right: 7px; text-align: right">
                            <div>
                                Group by:
                            </div>
                        </td>
                        <td>
                            <div>
                                <asp:DropDownList ID="ddl_GroupBy" Font-Names="Century Gothic" Width="200" OnSelectedIndexChanged="ddl_GroupBy_SelectedIndexChanged"
                                    runat="server">
                                    <asp:ListItem Selected="True" Value="abstractCategory">Paper Category</asp:ListItem>
                                    <asp:ListItem Value="paperTitleStart">Paper Title</asp:ListItem>
                                    <asp:ListItem Value="authorLastNameStart">Author Name</asp:ListItem>
                                    <asp:ListItem Value="authorDivision">Author Division</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-right: 7px;text-align: right">
                            <div>
                                Filter:</div>
                        </td>
                        <td>
                            <div>
                                <asp:TextBox ID="tb_Filter" Width="200" Font-Names="Century Gothic" runat="server"></asp:TextBox>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-top:10px;">
                            <div style="float: right; width: 100%">
                                <div style="float: right; margin-left: 7px">
                                    <asp:Button ID="bn_Clear" runat="server" Text="Clear" Width="60" CssClass="aspHoverButton"
                                        OnClick="bn_Clear_Click" />
                                </div>
                                <div style="float: right; margin-left: 7px;">
                                    <asp:Button ID="bn_Find" runat="server" Text="Submit" Width="60" CssClass="aspHoverButton"
                                        OnClick="bn_Find_Click" />
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </cc1:Flyout> </td>
    </tr></table>
       </ContentTemplate>
   
   </asp:UpdatePanel>
        <asp:UpdatePanel ID="upStageData" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnl_DataListContainer" runat="server" Style="position: relative; margin: auto">
                    <asp:DataList ID="dl_StageCategories" PresentationType="Stage" RepeatDirection="Vertical"
                        Width="1000" RepeatColumns="1" runat="server" Style="position: relative; margin: auto;
                        top: 0px; left: 0px;" OnItemDataBound="dl_Categories_ItemDataBound" OnSelectedIndexChanged="dl_Categories_SelectedIndexChanged">
                        <AlternatingItemStyle BackColor="WhiteSmoke" />
                        <ItemTemplate>
                            <div style="margin-top: 5px; margin-bottom: 10px; margin-left: 60px">
                                <asp:Label runat="server" ID="category" Text='<%# Eval("category") %>' Font-Bold="true"
                                    Font-Size="Large" Style="width: 880px" />
                                <asp:DataList runat="server" ID="dl_CategoryItem" PresentationType="Stage" RepeatDirection="Horizontal"
                                    RepeatColumns="2">
                                    <ItemTemplate>
                                        <asp:Panel ID="pnlItem" runat="server" CssClass="rounded-corners" AbstractID='<%# Eval("ID") %>'
                                            Style="position: relative; width: 420px; border: 1px solid white; height: 185px;
                                            margin-left: 10px; margin-top: 10px">
                                            <div style="margin-top: 10px; margin-left: 10px; margin-right: 10px">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div style="float: left; width: 100%">
                                                                <strong>
                                                                    <asp:Label ID="lblTitle" runat="server" AbstractID='<%# Eval("ID") %>' /></strong>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <i>
                                                    <div style="float: left; margin-top: 10px; width: 400px">
                                                        <span>Lead Author: </span>
                                                        <asp:Label runat="server" ID="lblLeadAuthor" Text=""></asp:Label>
                                                    </div>
                                                    <div runat="server" id="divCoAuthors" style="display: none; float: left; width: 400px">
                                                        <span>Co-Authors: </span>
                                                        <asp:Label runat="server" ID="lblCoAuthors" Text="" />
                                                    </div>
                                                    <div runat="server" id="divPresenter" style="display: none; float: left; width: 400px">
                                                        <span>Presented by: </span>
                                                        <asp:Label runat="server" ID="lbl_Presenter" Text="" />
                                                    </div>
                                                </i>
                                            </div>
                                            <asp:Panel ID="pnlItemDocuments" runat="server" Style="float: left; margin-top: 10px;
                                                width: 400px">
                                                <table>
                                                    <tr>
                                                        <td style="width: 25px; padding-left: 7px; padding-top: 5px;">
                                                            <asp:HyperLink ID="hl_PaperPDF" Target="_blank" runat="server">
                                                                <asp:Image ID="img_PaperPDF" ImageUrl="~/Images/pdf.png" BorderStyle="None" runat="server" />
                                                            </asp:HyperLink>
                                                        </td>
                                                        <td style="width: 175px; padding-left: 7px; padding-top: 5px;">
                                                            <asp:HyperLink ID="hl_Paper" Target="_blank" runat="server">Paper</asp:HyperLink>
                                                        </td>
                                                        <td style="width: 25px; padding-left: 7px; padding-top: 5px;">
                                                            <asp:HyperLink ID="hl_imgSummary" CssClass="PaperItemSummary" Target="_blank" runat="server">
                                                                <asp:Image ID="img_Summary" ImageUrl="~/Images/info.png" Height="25" Width="25" BorderStyle="None"
                                                                    runat="server" />
                                                            </asp:HyperLink>
                                                        </td>
                                                        <td style="width: 175px; padding-left: 7px; padding-top: 5px;">
                                                            <asp:HyperLink ID="hl_Summary" CssClass="PaperItemSummary" NavigateUrl="#" onmouseover="this.style.text-decoration='underline'"
                                                                onmouseout="this.style.text-decoration='none'" runat="server">Summary</asp:HyperLink>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 25px; padding-left: 7px; padding-top: 5px;">
                                                            <asp:HyperLink ID="hl_SlidesPDF" Target="_blank" runat="server">
                                                                <asp:Image ID="img_SlidesPDF" ImageUrl="~/Images/pdf.png" BorderStyle="None" runat="server" />
                                                            </asp:HyperLink>
                                                        </td>
                                                        <td style="width: 175px; padding-left: 7px; padding-top: 5px;">
                                                            <asp:HyperLink ID="hl_Slides" Target="_blank" runat="server">Presentation</asp:HyperLink>
                                                        </td>
                                                        <td style="width: 25px; padding-left: 7px; padding-top: 5px;">
                                                            <asp:HyperLink ID="hl_imgVideo" Target="_blank" runat="server">
                                                                <asp:Image ID="img_Video" ImageUrl="~/Images/video.gif" Height="20" Width="25" BorderStyle="None"
                                                                    runat="server" />
                                                            </asp:HyperLink>
                                                        </td>
                                                        <td style="width: 175px; padding-left: 7px; padding-top: 5px;">
                                                            <asp:HyperLink ID="hl_Video" Target="_blank" NavigateUrl="#" runat="server">Video</asp:HyperLink>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:DataList>
                            </div>
                        </ItemTemplate>
                    </asp:DataList></asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    </form>
</body>
</html>
