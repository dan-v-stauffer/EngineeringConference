<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Balloon.aspx.cs" Inherits="Balloon" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
        <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
            span
            {
                 color:#1E076A !important;   
            }
        
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="pnl_Main" runat="server" Width="450">
        <asp:Panel ID="pnl_Paper" Visible="false" runat="server" type="paper" Width="450">
            <asp:DataList ID="ds_PaperDetails" OnItemDataBound="ds_PaperDetails_ItemDataBound"
                runat="server">
                <ItemTemplate>
                    <table>
                        <tr>
                            <td>
                                <strong>
                                    <asp:HyperLink ID="lbl_Title" runat="server" Width="440" Style="margin-left: 5px; margin-bottom: 10px" />
                                </strong>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>
                                    <div>
                                        Author:</div>
                                </b>
                                <asp:Label ID="lbl_PrimaryAuthor" runat="server" Style="margin-left: 5px; margin-bottom: 10px"
                                    Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:DataList ID="ds_CoAuthors" runat="server" OnItemDataBound="ds_CoAuthors_ItemDataBound">
                                    <HeaderTemplate>
                                        <b>Co-Authors: </b>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <span style="margin-left: 5px;">-&nbsp;
                                            <asp:Label ID="lbl_CoAuthor" runat="server" /></span>
                                    </ItemTemplate>
                                </asp:DataList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>
                                    <div>
                                        Summary:</div>
                                </b><i>
                                    <asp:Label ID="lbl_Description" runat="server" Style="margin-left: 5px; margin-bottom: 10px"
                                        Text="Sample"></asp:Label></i>
                            </td>
                        </tr>
                        <tr class="hidden">
                            <td>
                                <b>
                                    <div>
                                        Paper & Slidedeck</div>
                                </b>
                                <br />
                                <table>
                                    <tr>
                                        <td>
                                            <asp:HyperLink ID="hl_FileImage" runat="server" Target="_blank">
                                                <asp:Image runat="server" ID="img_PaperIcon" ImageUrl="~/Images/doc.png" BorderStyle="None" /></asp:HyperLink>
                                        </td>
                                        <td>
                                            <asp:HyperLink ID="hl_File" runat="server" Target="_blank" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnl_SlideDeck" Visible="false" runat="server">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:HyperLink ID="hl_SlideDeckImg" runat="server" Target="_blank">
                                                                <asp:Image runat="server" ID="img_SlideDeckIcon" BorderStyle="None" />
                                                            </asp:HyperLink>
                                                        </td>
                                                        <td>
                                                            <asp:HyperLink ID="hl_SlideDeck" runat="server" Text="Slide Deck" Target="_blank" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </asp:Panel>
        <asp:Panel ID="pnl_TechPanel" Visible="false" runat="server" type="techpanel" Width="450">
            <asp:DataList ID="ds_Tech_PanelDetails" OnItemDataBound="ds_TechPanelDetails_ItemDataBound"
                runat="server">
                <ItemTemplate>
                    <table>
                        <tr>
                            <td>
                                <strong>
                                    <asp:Label ID="lbl_TechPanelTitle" runat="server" Width="425" />
                                </strong>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <i>
                                    <asp:Label ID="lbl_TechPanelTopic" runat="server" Text="" Width="425"></asp:Label>
                                </i>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>Panel Moderator:</span><br />
                                <asp:Label ID="lbl_TechPanelModerator" runat="server" Width="425" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <span>Panel Members:</span><br />
                                <asp:DataList ID="ds_TechPanelMembers" OnItemDataBound="ds_TechPanelMembers_ItemDataBound"
                                    runat="server">
                                    <HeaderTemplate>
                                        <ul>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li>
                                            <asp:Label ID="lbl_TechPanelModerator" runat="server" Width="400" /></li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul></FooterTemplate>
                                </asp:DataList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_TechPanelDescription" runat="server" Width="425" />
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </asp:Panel>
        <asp:Panel ID="pnl_ExecSpeaker" Visible="false" runat="server" type="execspeaker"
            Width="1000">
            <asp:DataList ID="dl_ExecSpeaker" runat="server" OnItemDataBound="dl_ExecSpeaker_ItemDataBound">
                <ItemTemplate>
                    <table style="width: 1000px">
                        <tr>
                            <td rowspan="2" valign="top" style="padding-right:25px">
                                <asp:Image ID="img_Speaker" runat="server" ImageUrl="~/Images/speakers/zsaidin.png"
                                    Height="375" Width="375" />
                            </td>
                            <td valign="top"><h2>
                                <asp:Label ID="lbl_Speaker" Font-Bold="true" runat="server" /></h2><br />
                                <asp:Label ID="lbl_Title" Font-Italic="true" runat="server" />
                            </td>
                        </tr>
                        
                        <tr>
                            <td valign="top" style="padding-top:10px">
                                <asp:Label ID="lbl_Bio" runat="server" />
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </asp:Panel>
    </asp:Panel>
    </form>
</body>
</html>
