﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HttpErrorPage.aspx.cs" Inherits="Errors_HttpErrorPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Engineering Conference 2015 Website - HTTP Error Page</title>
        <link href="~/Styles/Site.css" rel="stylesheet" type="text/css" />
</head>
<body>
  <form id="form1" runat="server">
  <div>
    <h2>
      Engineering Conference 2015 Registration Website Http Error Page</h2>
    <asp:Panel ID="InnerErrorPanel" runat="server" Visible="false">
      <asp:Label ID="innerMessage" runat="server" Font-Bold="true" 
        Font-Size="Large" /><br />
      <pre>
        <asp:Label ID="innerTrace" runat="server" />
      </pre>
    </asp:Panel>
    Error Message:<br />
    <asp:Label ID="exMessage" runat="server" Font-Bold="true" 
      Font-Size="Large" />
    <pre>
      <asp:Label ID="exTrace" runat="server" Visible="false" />
    </pre>
    <br />
    Return to the <a href='../Default.aspx'>Engineering Conference 2015 Registration Home Page</a>
  </div>
  </form>
</body>
</html>
