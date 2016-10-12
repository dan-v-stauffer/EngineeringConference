<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Exhibits.aspx.cs" Inherits="Exhibits" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
         <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
    <div style="margin:auto; width:900px">
    <asp:DataList ID="dl_Exhibits" RepeatDirection="Horizontal" RepeatColumns="3" runat="server"
     OnItemDataBound="dl_Exhibits_ItemDataBound">

    <ItemTemplate>
    <div style="text-align:center; margin: 10px 10px 10px 10px">
        <asp:HyperLink ID="hl_Exhibit" runat="server">
        <asp:Image ID="img_Thumbnail" Width="270" BorderStyle="None" runat="server" /><br />
            <asp:Label ID="lbl_Org" runat="server" />
    </asp:HyperLink>
    </div>
    </ItemTemplate>

    </asp:DataList>

    </div>
    </form>
</body>
</html>
