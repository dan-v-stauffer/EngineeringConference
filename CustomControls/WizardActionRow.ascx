<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WizardActionRow.ascx.cs" Inherits="CustomControls_WizardActionRow" %>
<link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />


<asp:Panel runat="server" ID="container" style="float:right; width:100px">
    <asp:Panel ID="nav_panel" runat="server" Width="125" style="float:right;">
        <asp:Button ID="Previous" runat="server" CssClass="aspHoverButton" OnClick="OnPreviousClicked" Text="Previous" />
        <asp:Button ID="Next" runat="server" CssClass="aspHoverButton" OnClick="OnNextClicked" Text="Next" />
    </asp:Panel>
        <asp:Panel ID="action_panel" runat="server" Width="125" style="float:right; margin-left:15px;">
            <asp:Button ID="Finish" runat="server" CssClass="aspHoverButton" OnClick="OnFinishClicked" Text="Submit" />
            <asp:Button ID="Cancel" runat="server" CssClass="aspHoverButton" OnClick="OnCancelClicked" Text="Cancel" />
    </asp:Panel>
</asp:Panel>