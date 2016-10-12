<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreviewReport.aspx.cs" Inherits="PreviewReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div>
        <rsweb:ReportViewer ID="rpt_Viewer" runat="server" Font-Names="Verdana" 
            Font-Size="8pt" InteractiveDeviceInfos="(Collection)" 
            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="954px">
            <LocalReport ReportPath="Reports\BaseAgenda.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
    </div>
    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
        OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" 
        TypeName="EngConferenceDataSetTableAdapters.sp_GetDailyAgendaItemsTableAdapter">
        <SelectParameters>
            <asp:SessionParameter DefaultValue="1" Name="conferenceID" 
                SessionField="conferenceID" Type="Int32" />
            <asp:Parameter DefaultValue="4/27/2014" Name="dateOfConference" 
                Type="DateTime" />
        </SelectParameters>
    </asp:ObjectDataSource>

    </form>
</body>
</html>
