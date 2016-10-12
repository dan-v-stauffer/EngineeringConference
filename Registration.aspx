<%@ Page Title="Engineering Conference Registration Page" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeFile="Registration.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" TagName="WizardActionRow" Src="~/CustomControls/WizardActionRow.ascx" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
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
            BindEvents();
        });

        function BindEvents() {
            //Here your jQuery function calls go
            $('a.TechPanelItem').each(function () {

                $(this).cluetip(
                        {
                            activation: 'click',
                            sticky: true,
                            arrows: true,
                            closeText: '<img src="Scripts/cluetip/images/cross.png" alt="close" style="border:1px solid red" />',
                            closePosition: 'title',
                            width: 460,
                            cluetipClass: 'jtip'
                        }
                    );
            });


            $('a.PaperItemLabel').each(function () {

                $(this).cluetip(
                        {
                            activation: 'click',
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
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:UpdatePanel ID="up_Body" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="false">
        <ContentTemplate>
            <asp:Panel runat="server" ID="rsvp_form" Style="margin-top: 10px; border: 1px solid silver;">
                <table style="width: 100%;">
                    <tr>
                        <td colspan="2">
                            <div style="float: left; margin-right: 10px">
                                <h2>
                                    Registration</h2>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-right: 5px; padding-left: 5px;">
                            <h3>
                                <b>Personal Information</b></h3>
                        </td>
                    </tr>
                    <tr>
                        <td class="rsvpForm_LeftColumn" style="padding-top: 30px">
                            Name<span class="reqdNote">*</span>:
                        </td>
                        <td class="rsvpForm_RightColumn">
                            <div style="float: left; width: 800px">
                                <table>
                                    <tr>
                                        <td style="padding-left: 5px">
                                            First
                                        </td>
                                        <td style="padding-left: 5px">
                                            Last
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="up_FirstName" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="tb_FirstName" runat="server" Width="250" CssClass="input" Text=""
                                                        ClientIDMode="Static" onblur="__doPostBack(this.id,'OnBlur');" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="up_LastName" runat="server">
                                                <ContentTemplate>
                                                    <asp:TextBox ID="tb_LastName" runat="server" Width="250" CssClass="input" Text=""
                                                        ClientIDMode="Static" onblur="__doPostBack(this.id,'OnBlur');" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            (As you want your name to appear on your conference badge.)
                                        </td>
                                    </tr>
                                </table>
                                <asp:RequiredFieldValidator ID="val_Name" runat="server" InitialValue="" Display="Dynamic"
                                    CssClass="validationMsg" ControlToValidate="tb_FirstName" ErrorMessage="Name*: This is a required field. We have to know your name so we can print name tags!"></asp:RequiredFieldValidator>
                                <asp:Label ID="lblName" runat="server" CssClass="hidden" Text=""></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="rsvpForm_LeftColumn" style="padding-top: 12px">
                            Division<span class="reqdNote">*</span>:
                        </td>
                        <td class="rsvpForm_RightColumn">
                            <asp:UpdatePanel ID="up_Division" class="cboContainer" runat="server">
                                <ContentTemplate>
                                    <asp:ComboBox ID="cbo_Division" Width="250" ForeColor="#696969" AutoPostBack="true"
                                        OnSelectedIndexChanged="cbo_Division_SelectedIndexChanged" ItemInsertLocation="OrdinalText"
                                        CssClass="WindowsStyle" AutoCompleteMode="SuggestAppend" DropDownStyle="DropDownList"
                                        runat="server">
                                    </asp:ComboBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:RequiredFieldValidator ID="val_Division" runat="server" InitialValue="Please select:"
                                Display="Dynamic" CssClass="validationMsg" ControlToValidate="cbo_Division" ErrorMessage="Division*: This is a required field. If it's not listed, select 'Other' and you can enter it yourself."></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="rsvpForm_LeftColumn">
                            Work Location:
                        </td>
                        <td class="rsvpForm_RightColumn">
                            <asp:UpdatePanel ID="up_WorkLocation" class="cboContainer" runat="server">
                                <ContentTemplate>
                                    <asp:ComboBox ID="cbo_WorkLocation" Width="250" ForeColor="#696969" OnSelectedIndexChanged="cbo_WorkLocation_SelectedIndexChanged"
                                        AutoPostBack="true" ItemInsertLocation="OrdinalText" CssClass="WindowsStyle"
                                        AutoCompleteMode="SuggestAppend" DropDownStyle="DropDown" runat="server">
                                    </asp:ComboBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="rsvpForm_LeftColumn" style="padding-top: 12px">
                            Job Role<span class="reqdNote">*</span>:
                        </td>
                        <td class="rsvpForm_RightColumn">
                            <asp:UpdatePanel ID="up_EngineerType" class="cboContainer" runat="server">
                                <ContentTemplate>
                                    <asp:ComboBox ID="cbo_EngineerType" Width="250" Font-Names="Century Gothic" ForeColor="#696969"
                                        OnSelectedIndexChanged="cbo_EngineerType_SelectedIndexChanged" AutoPostBack="true"
                                        ItemInsertLocation="OrdinalText" CssClass="WindowsStyle" AutoCompleteMode="SuggestAppend"
                                        DropDownStyle="DropDown" runat="server">
                                    </asp:ComboBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:RequiredFieldValidator ID="val_EngDiscipline" runat="server" InitialValue="Please select:"
                                Display="Dynamic" CssClass="validationMsg" val_field="Job Role: " ControlToValidate="cbo_EngineerType"
                                ErrorMessage="Job Role*: This is a required field. Please select. If you don't see anything that fits, select 'Other' and you can enter something that does."></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="rsvpForm_LeftColumn" style="padding-top: 8px">
                            Mobile Phone:
                        </td>
                        <td class="rsvpForm_RightColumn">
                            <asp:UpdatePanel ID="up_MobilePhone" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="tb_MobilePhone" CssClass="input" runat="server" Width="250" ClientIDMode="Static"
                                        onblur="__doPostBack(this.id,'OnBlur');">
                                    </asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="rsvpForm_LeftColumn" style="padding-top: 18px">
                            Shirt Size<span class="reqdNote">*</span>:
                        </td>
                        <td class="rsvpForm_RightColumn">
                            <asp:UpdatePanel runat="server" ID="up_ShirtInfo" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="rb_ShirtType" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="cbo_ShirtSize" EventName="SelectedIndexChanged" />
                                </Triggers>
                                <ContentTemplate>
                                    <div class="floaterL">
                                        <asp:RadioButtonList ID="rb_ShirtType" runat="server" OnSelectedIndexChanged="rb_ShirtType_SelectedIndexChanged"
                                            Enabled="true" AutoPostBack="true" RepeatDirection="Vertical" RepeatLayout="Flow">
                                            <asp:ListItem Text="Men's" Value="M" Selected="True" />
                                            <asp:ListItem Text="Women's" Value="W" />
                                        </asp:RadioButtonList>
                                    </div>
                                    <div class="floaterLWide">
                                        <asp:ComboBox ID="cbo_ShirtSize" Width="125" Font-Names="Century Gothic" ForeColor="#696969"
                                            Enabled="true" OnSelectedIndexChanged="cbo_ShirtSize_SelectedIndexChanged" AutoPostBack="true"
                                            ItemInsertLocation="OrdinalText" CssClass="WindowsStyle" AutoCompleteMode="SuggestAppend"
                                            DropDownStyle="DropDownList" runat="server">
                                            <asp:ListItem Text="Please select:" Value="Please select:" Selected="True" />
                                            <asp:ListItem Text="Small" Value="S" />
                                            <asp:ListItem Text="Medium" Value="M" />
                                            <asp:ListItem Text="Large" Value="L" />
                                            <asp:ListItem Text="X-Large" Value="XL" />
                                            <asp:ListItem Text="XX-Large" Value="XXL" />
                                            <asp:ListItem Text="XXX-Large" Value="XXXL" />
                                        </asp:ComboBox>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div style="width: 100%">
                                <asp:RequiredFieldValidator ID="val_ShirtSize" runat="server" InitialValue="Please select:"
                                    Display="Dynamic" CssClass="validationMsg" val_field="Shirt Size: " ControlToValidate="cbo_EngineerType"
                                    ErrorMessage="Shirt Size*: This is a required field. It's a great shirt- you'll want it to fit!"></asp:RequiredFieldValidator>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-right: 5px; padding-left: 5px;">
                            <div style="width: 100%; border-top: 1px solid silver; margin-bottom: 5px;" />
                            <h3>
                                <b>Hospitality and Catering</b></h3>
                        </td>
                    </tr>
                    <tr>
                        <td class="rsvpForm_LeftColumn">
                            Attending Sunday<br />
                            Evening Reception?
                        </td>
                        <td class="rsvpForm_RightColumn">
                            <asp:UpdatePanel ID="up_SundayReception" ChildrenAsTriggers="false" UpdateMode="Conditional"
                                runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="rbl_SundayReception" EventName="SelectedIndexChanged" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:RadioButtonList ID="rbl_SundayReception" runat="server" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow" OnSelectedIndexChanged="rbl_SundayReception_SelectedIndexChanged">
                                        <asp:ListItem Selected="True" Value="True" Text="Yes" />
                                        <asp:ListItem Selected="False" Value="False" Text="No" />
                                    </asp:RadioButtonList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="rsvpForm_LeftColumn" style="padding-top: 10px">
                            Interested in Sunday Golf?
                        </td>
                        <td class="rsvpForm_RightColumn">
                            <div class="floaterL" style="margin-bottom: 10px">
                                <asp:UpdatePanel ID="upGolf" ChildrenAsTriggers="false" UpdateMode="Conditional"
                                    runat="server">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="rbl_SundayGolf" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <div class="floaterL">
                                            <asp:RadioButtonList ID="rbl_SundayGolf" runat="server" RepeatDirection="Vertical"
                                                RepeatLayout="Flow" AutoPostBack="true" OnSelectedIndexChanged="rbl_SundayGolf_SelectedIndexChanged">
                                                <asp:ListItem Selected="True" Value="0" Text="No" />
                                                <asp:ListItem Selected="False" Value="1" Text="Yes" />
                                            </asp:RadioButtonList>
                                        </div>
                                        <div class="floaterLWide">
                                            <ul>
                                            <li><a href='mailto:graye@pebblebeach.com' >Email Eric Gray</a> at the Del Monte Clubhouse to reserve your tee time.</li>
                                            <li>Discounted green fees for Conference Attendees. $110 full course (includes cart
                                                rental).</li>
                                                <li>Available tee times between 11:30AM-2:30PM. </li>
                                                <li>Credit card required at time of booking.</li>
                                            </ul>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="rsvpForm_LeftColumn" style="padding-top: 14px">
                            Hotel Reservations:
                        </td>
                        <td class="rsvpForm_RightColumn" style="width: 800px">
                            <div class="floaterL">
                                <asp:UpdatePanel ID="up_HotelReservations" runat="server">
                                    <ContentTemplate>
                                        <asp:CheckBoxList ID="cbl_HotelReservations" CssClass="checkBox" runat="server" AutoPostBack="true"
                                            Enabled="true" OnSelectedIndexChanged="cbl_HotelReservations_SelectedIndexChanged">
                                        </asp:CheckBoxList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div style="float: left; width: 600px; margin-top: 7px; margin-left: 20px;">
                                <span>NOTE: Conference administrators will make your reservations for the nights selected.
                                    <u><i>Please do not contact the hotel directly.</i></u></span>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="rsvpForm_LeftColumn" style="padding-top: 12px">
                            Transportation:
                        </td>
                        <td class="rsvpForm_RightColumn">
                            <asp:UpdatePanel runat="server" ID="up_Transportation" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="rbl_MonteryInbound" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="rbl_MonteryOutbound" EventName="SelectedIndexChanged" />
                                </Triggers>
                                <ContentTemplate>
                                    <div class="mealDatesItem">
                                        <table>
                                            <tr>
                                                <td>
                                                    <div style="width: 325px; float: left; margin: 5px 5px 5px 5px">
                                                        <b>From KT Milpitas to Hyatt Regency Monterey:</b>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div style="width: 325px; vertical-align: top; height: 85px">
                                                        <asp:UpdatePanel ID="up_MontereyInbound" runat="server">
                                                            <ContentTemplate>
                                                                <asp:RadioButtonList ID="rbl_MonteryInbound" Direction="Inbound" runat="server" SelectedIndexChanged="rbl_TransporationOptions_SelectedIndexChanged"
                                                                    CausesValidation="false" RepeatDirection="Vertical" RepeatLayout="Flow" AutoPostBack="true">
                                                                </asp:RadioButtonList>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="vertical-align: bottom">
                                                    <div style="width: 325px; vertical-align: top; height: 85px; padding-bottom: 5px">
                                                        <i>
                                                            <asp:Label ID="lbl_DepartLocationInbound" runat="server" /></i>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="mealDatesItem">
                                        <table>
                                            <tr>
                                                <td>
                                                    <div style="width: 325px; float: left; margin: 5px 5px 5px 5px">
                                                        <b>From Hyatt Regency Monterey to KT Milpitas:</b>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div style="width: 325px; vertical-align: top; height: 85px;">
                                                        <asp:RadioButtonList ID="rbl_MonteryOutbound" Direction="Outbound" runat="server"
                                                            CausesValidation="false" Width="280" SelectedIndexChanged="rbl_TransporationOptions_SelectedIndexChanged"
                                                            RepeatDirection="Vertical" RepeatLayout="Flow" AutoPostBack="true">
                                                        </asp:RadioButtonList>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div style="width: 325px; vertical-align: top; height: 85px; margin-bottom: 5px">
                                                        <i>
                                                            <asp:Label ID="lbl_DepartLocationOutbound" runat="server" /></i>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="rsvpForm_LeftColumn" style="padding-top: 12px">
                            Meal Choices:
                        </td>
                        <td class="rsvpForm_RightColumn">
                            <asp:DataList ID="dl_MealDates" runat="server" RepeatDirection="Horizontal" RepeatColumns="7">
                                <ItemTemplate>
                                    <div class="mealDatesItem">
                                        <div style="margin-bottom: 5px">
                                            <asp:Label ID="lbl_MealDate" Font-Bold="true" runat="server" /><br />
                                        </div>
                                        <%--                                  <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="rbl_SingleMealChoice" EventName="SelectedIndexChanged" />
                                                            <asp:AsyncPostBackTrigger ControlID="cbo_MultipleMealChoices" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                        --%>
                                        <asp:UpdatePanel ID="up_MealOptions" UpdateMode="Conditional" runat="server">
                                            <ContentTemplate>
                                                <asp:DataList ID="dl_MealsOnDate" runat="server" RepeatDirection="Vertical">
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td style="width: 70px; text-align: right; margin-right: 5px">
                                                                    <asp:Label ID="lbl_MealType" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <%--                                                            <asp:UpdatePanel ID="up_rbl_SingleMealChoice" runat="server" UpdateMode="Conditional">
                                                            <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="rbl_SingleMealChoice"  EventName="SelectedIndexChanged" />
                                                            </Triggers>
                                                            <ContentTemplate>
                                                                    --%>
                                                                    <asp:Panel ID="pnl_SingleMealChoice" Visible="false" runat="server">
                                                                        <asp:RadioButtonList ID="rbl_SingleMealChoice" runat="server" AutoPostBack="true"
                                                                            ClientIDMode="AutoID" RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="rbl_SingleMealChoice_SelectedIndexChanged"
                                                                            Visible="false">
                                                                            <asp:ListItem Selected="True" Text="Yes" Value="True"></asp:ListItem>
                                                                            <asp:ListItem Selected="False" Text="No" Value="False"></asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </asp:Panel>
                                                                    <%--  </ContentTemplate>
                                                            </asp:UpdatePanel>--%>
                                                                    <asp:Panel ID="pnl_MultipleMealChoices" Visible="false" runat="server">
                                                                        <asp:ComboBox ID="cbo_MultipleMealChoices" Width="135" Visible="false" AutoPostBack="true"
                                                                            OnSelectedIndexChanged="cbo_MultipleMealChoices_SelectedIndexChanged" ItemInsertLocation="OrdinalText"
                                                                            CssClass="WindowsStyle" AutoCompleteMode="Suggest" runat="server">
                                                                        </asp:ComboBox>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </ItemTemplate>
                            </asp:DataList>
                        </td>
                    </tr>
                    <tr>
                        <td class="rsvpForm_LeftColumn">
                            Food Allergies or Restrictions:
                        </td>
                        <td class="rsvpForm_RightColumn">
                            <asp:UpdatePanel ID="up_FoodRestrictions" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="tb_FoodRestrictions" CssClass="input" Width="725" TextMode="MultiLine"
                                        ClientIDMode="Static" onblur="__doPostBack(this.id,'OnBlur');" runat="server"></asp:TextBox>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="rsvpForm_LeftColumn">
                            Special Requirements or Needs:
                        </td>
                        <td class="rsvpForm_RightColumn">
                            <asp:UpdatePanel ID="up_SpecialNeeds" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="tb_SpecialNeeds" CssClass="input" Width="725" TextMode="MultiLine"
                                        runat="server" ClientIDMode="Static" onblur="__doPostBack(this.id,'OnBlur');" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-right: 5px; padding-left: 5px;">
                            <div style="width: 100%; border-top: 1px solid silver; margin-bottom: 5px;" />
                            <h3>
                                <b>Conference Participation Requests</b></h3>
                        </td>
                    </tr>
                    <tr>
                    <td colspan="2">
                    <h3>Optional Events, Sunday, Oct 25<sup>th</sup></h3>
                    </td>
                    </tr>
                    <tr>
                        <td class="rsvpForm_LeftColumn" style="padding-top: 10px">
                            Deep Learning Tutorial
                        </td>
                        <td class="rsvpForm_RightColumn">
                            <table>
                                <tr>
                                    <td style="vertical-align: top">
                                        <asp:UpdatePanel UpdateMode="Conditional" ID="up_DeepLearning" runat="server">
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="rbl_DeepLearning" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:RadioButtonList ID="rbl_DeepLearning" runat="server" AutoPostBack="true" ClientIDMode="AutoID"
                                                    OnSelectedIndexChanged="rbl_DeepLearning_SelectedIndexChanged" eventID="313">
                                                    <asp:ListItem Text="Yes" Value="True" />
                                                    <asp:ListItem Text="No" Value="False" Selected="True" />
                                                </asp:RadioButtonList>
                                                </td>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <td style="vertical-align: top; padding-top: 8px; padding-left: 8px">
                                            Would you like to register for the Deep Learning Tutorial (1:00-4:00 pm Sunday,
                                            Oct 25)?
                                        </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="rsvpForm_LeftColumn" style="padding-top: 10px">
                          <asp:Label ID="lbl_PresenterInfo" runat="server" Text="Presenter Information Meeting" />
                        </td>
                        <td class="rsvpForm_RightColumn">
                            
                            <table>
                                <tr>
                                    <td style="vertical-align: top">
                                        <asp:UpdatePanel UpdateMode="Conditional" ID="up_PresenterInfo" runat="server" >
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="rbl_PresenterInfo" EventName="SelectedIndexChanged" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:RadioButtonList ID="rbl_PresenterInfo" runat="server" AutoPostBack="true" ClientIDMode="AutoID"
                                                    OnSelectedIndexChanged="rbl_PresenterInfo_SelectedIndexChanged" eventID="310">
                                                    <asp:ListItem Text="Yes" Value="True"  />
                                                    <asp:ListItem Text="No" Value="False"  Selected="True" />
                                                </asp:RadioButtonList>
                                                </td>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <td style="vertical-align: top; padding-top: 8px; padding-left: 8px"><asp:Label ID="lbl_PresenterInfoDescription" runat="server"
                                        Text="Will you attend the Presenter Information Meeting? (8:00-8:30 pm Sunday, Oct 25)?" />
                                        </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                    <td colspan="2">
                    <h3>Scheduled Events, Monday, Oct 26<sup>th</sup> - Tuesday, Oct 27 <sup>th</sup></h3>
                    </td>
                    </tr>
                    <tr>
                        <td class="rsvpForm_LeftColumn" style="padding-top: 4px">
                            Technical Panels & Tutorials<span class="reqdNote">*</span>:
                        </td>
                        <td class="rsvpForm_RightColumn">
                            <asp:CompareValidator ID="val_TechPanels" runat="server" CssClass="validationMsg"
                                Display="Dynamic" val_field="Technical Panels: " ControlToValidate="tb_TechPanelDummy"
                                ControlToCompare="tb_TechPanelRef" Operator="NotEqual" Style="margin-top: 5px;
                                margin-bottom: 5px" ErrorMessage="Technical Panels*: You must select at least one Technical Panel. We need to know which panels need the biggest rooms."></asp:CompareValidator>
                            <asp:TextBox ID="tb_TechPanelRef" CssClass="hidden" runat="server" Text="0"></asp:TextBox>
                            <asp:TextBox ID="tb_TechPanelDummy" CssClass="hidden" runat="server" Text="0"></asp:TextBox>
                            <asp:UpdatePanel ID="up_TechPanelChoice" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <asp:DataList ID="dl_TechPanelsList" OnItemDataBound="dl_TechPanelsList_ItemDataBound"
                                        RepeatDirection="Vertical" runat="server">
                                        <HeaderTemplate>
                                            <div style="width: 800px;">
                                                <i>In order to ensure that Technical Panels have evenly distributed audiences, <u>select
                                                    your top three choices:</u><br />
                                                    (Note: We'll strive to get you into your top choice, but seats will be allocated
                                                    on a 'first-come, first-served' basis.)</i></div>
                                            <div style="width: 800px; text-align: right">
                                                <div class="floaterR" style="width: 515px; padding-left: 5px; text-align: right;
                                                    font-size: smaller; margin-top: 5px">
                                                    <div style="float: left; width: 210px;">
                                                        &nbsp;</div>
                                                    <div style="float: left; width: 75px;">
                                                        1<sup>st</sup> Choice:
                                                    </div>
                                                    <div class="firstChoiceKey">
                                                        &nbsp;</div>
                                                    <div style="float: left; width: 80px;">
                                                        2<sup>nd</sup> Choice:
                                                    </div>
                                                    <div class="secondChoiceKey">
                                                        &nbsp;</div>
                                                    <div style="float: left; width: 75px;">
                                                        3<sup>rd</sup> Choice:
                                                    </div>
                                                    <div class="thirdChoiceKey">
                                                        &nbsp;</div>
                                                </div>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="border: 1px solid silver; margin-bottom: 10px; padding: 5px; width: 800px">
                                                <div style="text-align: left; margin-top: 5px; margin-bottom: 10px;">
                                                    <b>
                                                        <asp:Label ID="lbl_TechPanelGroupTitle" runat="server" />
                                                    </b>
                                                </div>
                                                <asp:DataList ID="dl_TechPanelChoices" runat="server" OnItemDataBound="dl_TechPanelChoices_ItemDataBound"
                                                    RepeatDirection="Vertical">
                                                    <HeaderTemplate>
                                                        <div style="text-align: left">
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 600px;">
                                                                    </td>
                                                                    <td style="width: 200px;">
                                                                        <table>
                                                                            <tr>
                                                                                <td style="width: 45px; text-align: center;">
                                                                                    <div style="width: 45px">
                                                                                        1<sup>st</sup>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="width: 45px; text-align: center;">
                                                                                    <div style="width: 45px">
                                                                                        2<sup>nd</sup>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="width: 45px; text-align: center;">
                                                                                    <div style="width: 45px">
                                                                                        3<sup>rd</sup>
                                                                                    </div>
                                                                                </td>
                                                                                <td style="width: 45px; text-align: center;">
                                                                                    <div style="width: 45px">
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div runat="server" id="div_TechPanelItem">
                                                            <table>
                                                                <tr>
                                                                    <td style="width: 600px;">
                                                                        <div>
                                                                            <asp:Label ID="lbl_TechPanelTitle" CssClass="floaterL" runat="server" /></div>
                                                                        <div style="float: left; margin-left: 5px; margin-top: 1px">
                                                                            <a id="hl_TechPanelDetail" runat="server" class="TechPanelItem" title="More About This Tech Panel">
                                                                                <img src="Images/info.gif" alt="detail" style="border: 0px solid silver;" />
                                                                            </a>
                                                                        </div>
                                                                        <asp:Panel ID="pnl_ModOrMember" CssClass="hidden" runat="server">
                                                                            <img src="Images/star.png" class="floaterL" alt="Assigned" style="border: none; margin-right: 3px" />
                                                                            <asp:Label ID="lbl_ModOrMember" CssClass="floaterL" Text="" runat="server" />
                                                                            <i>
                                                                                <asp:Label ID="lbl_TechPanelTopic" CssClass="hidden" runat="server" />
                                                                            </i>
                                                                        </asp:Panel>
                                                                    </td>
                                                                    <td style="width: 200px;">
                                                                        <table>
                                                                            <tr>
                                                                                <td style="width: 45px; text-align: center;">
                                                                                    <div style="width: 45px; text-align: center">
                                                                                        <asp:RadioButton ID="rbTechPanel1st" OnCheckedChanged="rbTechPanel_CheckedChanged"
                                                                                            AutoPostBack="true" runat="server" />
                                                                                    </div>
                                                                                </td>
                                                                                <td style="width: 45px; text-align: center;">
                                                                                    <div style="width: 45px; text-align: center">
                                                                                        <asp:RadioButton ID="rbTechPanel2nd" OnCheckedChanged="rbTechPanel_CheckedChanged"
                                                                                            AutoPostBack="true" runat="server" />
                                                                                    </div>
                                                                                </td>
                                                                                <td style="width: 45px; text-align: center;">
                                                                                    <div style="width: 45px; text-align: center">
                                                                                        <asp:RadioButton ID="rbTechPanel3rd" OnCheckedChanged="rbTechPanel_CheckedChanged"
                                                                                            AutoPostBack="true" runat="server" />
                                                                                    </div>
                                                                                </td>
                                                                                <td style="width: 45px; text-align: center;">
                                                                                    <div style="width: 45px; text-align: center">
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:DataList>
                                            </div>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="rsvpForm_LeftColumn" style="padding-top: 10px">
                            Paper Symposia<span class="reqdNote">*</span>:
                        </td>
                        <td class="rsvpForm_RightColumn">
                            <asp:RequiredFieldValidator ID="val_Papers" runat="server" CssClass="validationMsg"
                                Display="Dynamic" ControlToValidate="tb_PaperDummy" InitialValue="0" Style="margin-top: 5px;
                                margin-bottom: 5px" val_field="Paper Symposia: " ErrorMessage="Paper Symposia*: You must select at least one paper in each time slot. We need to know which presentations need the most chairs in the audience!"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="tb_PaperDummy" CssClass="hidden" runat="server" Text="0"></asp:TextBox>
                            <asp:UpdatePanel ID="up_SymposiumChoice" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <asp:DataList ID="dl_PaperSymposiaList" Layout="Flow" OnItemDataBound="dl_PaperSymposiaList_ItemDataBound"
                                        CellSpacing="5" RepeatDirection="Vertical" runat="server">
                                        <HeaderTemplate>
                                            <div style="float: left; width: 800px">
                                                <i>
                                                In order to ensure that we have enough seats for everyone in these paper presentations, <u>please select your top two choices for each time slot.</u>
                                                </i></div>
                                            <div style="float: right; width: 260px; padding-left: 25px; margin-top: 5px">
                                                <div style="float: left; width: 75px;">
                                                    1<sup>st</sup> Choice:
                                                </div>
                                                <div class="firstChoiceKey">
                                                    &nbsp;</div>
                                                <div style="float: left; width: 80px;">
                                                    2<sup>nd</sup> Choice:
                                                </div>
                                                <div class="secondChoiceKey">
                                                    &nbsp;</div>
                                            </div>
                                        </HeaderTemplate>
                                        <ItemStyle CssClass="paperSymposiumDiv" />
                                        <AlternatingItemStyle CssClass="paperSymposiumDiv" />
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <b>
                                                            <asp:Label ID="lbl_SymposiaGroupTitle" runat="server" /><br />
                                                            <asp:Label ID="lbl_SymposiaGroupTimes" runat="server" />
                                                        </b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table style="width: 50%;">
                                                            <tr>
                                                                <td style="vertical-align: middle; width: 55px; text-align: center">
                                                                    <div class="vertical" style="width: 55px">
                                                                        &nbsp;</div>
                                                                </td>
                                                                <td class="paperRoomHeaderCell">
                                                                    <div class="paperRooomHeaderText">
                                                                        Rm #1</div>
                                                                </td>
                                                                <td class="paperRoomHeaderCell">
                                                                    <div class="paperRooomHeaderText">
                                                                        Rm #2</div>
                                                                </td>
                                                                <td class="paperRoomHeaderCell">
                                                                    <div class="paperRooomHeaderText">
                                                                        Rm #3</div>
                                                                </td>
                                                                <td class="paperRoomHeaderCell">
                                                                    <div class="paperRooomHeaderText">
                                                                        Rm #4</div>
                                                                </td>
                                                                <td class="paperRoomHeaderCell">
                                                                    <div class="paperRooomHeaderText">
                                                                        Rm #5</div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="vertical-align: top;">
                                                        <asp:DataList ID="dl_PaperTimeSlots" runat="server" OnItemDataBound="dl_PaperTimeSlots_ItemDataBound">
                                                            <ItemTemplate>
                                                                <table style="vertical-align: top">
                                                                    <tr>
                                                                        <td style="width: 50px">
                                                                            <b>
                                                                                <asp:Label ID="lbl_PaperTimeStart" CssClass="paperTimeSlot" Width="50" runat="server" /></b>
                                                                        </td>
                                                                        <td class="paperItem" runat="server" id="td_paper1">
                                                                            <table style="width: 100%">
                                                                                <tr class="paperCat">
                                                                                    <td>
                                                                                        <asp:Label ID="lbl_PaperCatRm1" runat="server" Font-Bold="true" Style="float: left;
                                                                                            vertical-align: top;" Width="140" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="vertical-align: top; height: 150px">
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td style="vertical-align: top; padding-top: 4px;">
                                                                                                    <a id="hl_PaperDetail_1" runat="server" class="PaperItemLabel" title="More About This Paper">
                                                                                                        <img src="Images/info.gif" alt="detail" style="border: 0px solid silver; float: left" />
                                                                                                    </a>
                                                                                                </td>
                                                                                                <td style="vertical-align: top; padding-left: 5px;">
                                                                                                    <div style="height: 100px; width: 132px; float: left">
                                                                                                        <asp:Label ID="lbl_PaperTitleRm1" runat="server" Style="float: left; vertical-align: top"
                                                                                                            Width="140" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="vertical-align: bottom">
                                                                                        <div class="paperOptionBox" id="divPaperOptionBox1" runat="server">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td colspan="3">
                                                                                                        <div class="paperOptionsLabelDiv">
                                                                                                            <div style="float: right; margin-right: 5px">
                                                                                                                2<sup>nd</sup></div>
                                                                                                            <div style="float: right; margin-right: 13px">
                                                                                                                1<sup>st</sup></div>
                                                                                                        </div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="width: 85px; text-align: right; padding-right: 5px">
                                                                                                        Select:
                                                                                                    </td>
                                                                                                    <td style="width: 30px">
                                                                                                        <asp:CheckBox ID="cb_FirstChoicePaperRm1" CssClass="paperCheckBox floaterL" AutoPostBack="true"
                                                                                                            OnCheckedChanged="cb_PaperChoiceCheckedChanged" runat="server" Checked="false"
                                                                                                            Text="" />
                                                                                                    </td>
                                                                                                    <td style="width: 30px">
                                                                                                        <asp:CheckBox ID="cb_SecondChoicePaperRm1" CssClass="paperCheckBox floaterL" AutoPostBack="true"
                                                                                                            OnCheckedChanged="cb_PaperChoiceCheckedChanged" runat="server" Checked="false"
                                                                                                            Text="" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Panel ID="pnl_ModOrMember1" Visible="false" Height="18" runat="server">
                                                                                            <img src="Images/star.png" class="floaterL" alt="Assigned" style="border: none; margin-right: 3px" />
                                                                                            <asp:Label ID="lbl_ModOrMember1" CssClass="floaterL" Text="You're presenting!" runat="server" />
                                                                                        </asp:Panel>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td class="paperItem" runat="server" id="td_paper2">
                                                                            <table style="width: 100%">
                                                                                <tr class="paperCat">
                                                                                    <td>
                                                                                        <asp:Label ID="lbl_PaperCatRm2" runat="server" Font-Bold="true" Style="float: left;
                                                                                            vertical-align: top" Width="140" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="vertical-align: top; height: 150px">
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td style="vertical-align: top; padding-top: 4px;">
                                                                                                    <a id="hl_PaperDetail_2" runat="server" class="PaperItemLabel" title="More About This Paper">
                                                                                                    </a>
                                                                                                    <img src="Images/info.gif" alt="detail" style="border: 0px solid silver;" />
                                                                                                </td>
                                                                                                <td style="vertical-align: top; padding-left: 5px;">
                                                                                                    <div style="height: 100px; width: 132px; float: left">
                                                                                                        <asp:Label ID="lbl_PaperTitleRm2" runat="server" Style="float: left; vertical-align: top"
                                                                                                            Width="140" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="vertical-align: bottom">
                                                                                        <div class="paperOptionBox" id="divPaperOptionBox2" runat="server">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td colspan="3">
                                                                                                        <div class="paperOptionsLabelDiv">
                                                                                                            <div style="float: right; margin-right: 5px">
                                                                                                                2<sup>nd</sup></div>
                                                                                                            <div style="float: right; margin-right: 13px">
                                                                                                                1<sup>st</sup></div>
                                                                                                        </div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="width: 90px; text-align: right; padding-right: 5px">
                                                                                                        Select:
                                                                                                    </td>
                                                                                                    <td style="width: 30px">
                                                                                                        <asp:CheckBox ID="cb_FirstChoicePaperRm2" CssClass="floaterL" AutoPostBack="true"
                                                                                                            OnCheckedChanged="cb_PaperChoiceCheckedChanged" runat="server" Checked="false"
                                                                                                            Text="" />
                                                                                                    </td>
                                                                                                    <td style="width: 30px">
                                                                                                        <asp:CheckBox ID="cb_SecondChoicePaperRm2" CssClass="floaterL" AutoPostBack="true"
                                                                                                            OnCheckedChanged="cb_PaperChoiceCheckedChanged" runat="server" Checked="false"
                                                                                                            Text="" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Panel ID="pnl_ModOrMember2" Visible="false" runat="server">
                                                                                            <img src="Images/star.png" class="floaterL" alt="Assigned" style="border: none; margin-right: 3px" />
                                                                                            <asp:Label ID="lbl_ModOrMember2" CssClass="floaterL" Text="You're presenting!" runat="server" />
                                                                                        </asp:Panel>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td class="paperItem" runat="server" id="td_paper3">
                                                                            <table style="width: 100%">
                                                                                <tr class="paperCat">
                                                                                    <td>
                                                                                        <asp:Label ID="lbl_PaperCatRm3" runat="server" Font-Bold="true" Style="float: left;
                                                                                            vertical-align: top" Width="140" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="vertical-align: top; height: 150px">
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td style="vertical-align: top; padding-top: 4px;">
                                                                                                    <a id="hl_PaperDetail_3" runat="server" class="PaperItemLabel" title="More About This Paper">
                                                                                                        <img src="Images/info.gif" alt="detail" style="border: 0px solid silver; float: left" />
                                                                                                    </a>
                                                                                                </td>
                                                                                                <td style="vertical-align: top; padding-left: 5px;">
                                                                                                    <div style="height: 100px; width: 132px; float: left">
                                                                                                        <asp:Label ID="lbl_PaperTitleRm3" runat="server" Style="float: left; vertical-align: top"
                                                                                                            Width="140" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="vertical-align: bottom">
                                                                                        <div class="paperOptionBox" id="divPaperOptionBox3" runat="server">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td colspan="3">
                                                                                                        <div class="paperOptionsLabelDiv">
                                                                                                            <div style="float: right; margin-right: 5px">
                                                                                                                2<sup>nd</sup></div>
                                                                                                            <div style="float: right; margin-right: 13px">
                                                                                                                1<sup>st</sup></div>
                                                                                                        </div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="width: 90px; text-align: right; padding-right: 5px">
                                                                                                        Select:
                                                                                                    </td>
                                                                                                    <td style="width: 30px">
                                                                                                        <asp:CheckBox ID="cb_FirstChoicePaperRm3" CssClass="floaterL" AutoPostBack="true"
                                                                                                            OnCheckedChanged="cb_PaperChoiceCheckedChanged" runat="server" Checked="false"
                                                                                                            Text="" />
                                                                                                    </td>
                                                                                                    <td style="width: 30px">
                                                                                                        <asp:CheckBox ID="cb_SecondChoicePaperRm3" CssClass="floaterL" AutoPostBack="true"
                                                                                                            OnCheckedChanged="cb_PaperChoiceCheckedChanged" runat="server" Checked="false"
                                                                                                            Text="" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Panel ID="pnl_ModOrMember3" Visible="false" runat="server">
                                                                                            <img src="Images/star.png" class="floaterL" alt="Assigned" style="border: none; margin-right: 3px" />
                                                                                            <asp:Label ID="lbl_ModOrMember3" CssClass="floaterL" Text="You're presenting!" runat="server" />
                                                                                        </asp:Panel>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td class="paperItem" runat="server" id="td_paper4">
                                                                            <table style="width: 100%">
                                                                                <tr class="paperCat">
                                                                                    <td>
                                                                                        <asp:Label ID="lbl_PaperCatRm4" runat="server" Font-Bold="true" Style="float: left;
                                                                                            vertical-align: top" Width="140" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="vertical-align: top; height: 150px">
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td style="vertical-align: top; padding-top: 4px;">
                                                                                                    <a id="hl_PaperDetail_4" runat="server" class="PaperItemLabel" title="More About This Paper">
                                                                                                        <img src="Images/info.gif" alt="detail" style="border: 0px solid silver; float: left" />
                                                                                                    </a>
                                                                                                </td>
                                                                                                <td style="vertical-align: top; padding-left: 5px;">
                                                                                                    <div style="height: 100px; width: 132px; float: left">
                                                                                                        <asp:Label ID="lbl_PaperTitleRm4" runat="server" Style="float: left; vertical-align: top"
                                                                                                            Width="140" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="vertical-align: bottom">
                                                                                        <div class="paperOptionBox" id="divPaperOptionBox4" runat="server">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td colspan="3">
                                                                                                        <div class="paperOptionsLabelDiv">
                                                                                                            <div style="float: right; margin-right: 5px">
                                                                                                                2<sup>nd</sup></div>
                                                                                                            <div style="float: right; margin-right: 13px">
                                                                                                                1<sup>st</sup></div>
                                                                                                        </div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="width: 90px; text-align: right; padding-right: 5px">
                                                                                                        Select:
                                                                                                    </td>
                                                                                                    <td style="width: 30px">
                                                                                                        <asp:CheckBox ID="cb_FirstChoicePaperRm4" CssClass="floaterL" AutoPostBack="true"
                                                                                                            OnCheckedChanged="cb_PaperChoiceCheckedChanged" runat="server" Checked="false"
                                                                                                            Text="" />
                                                                                                    </td>
                                                                                                    <td style="width: 30px">
                                                                                                        <asp:CheckBox ID="cb_SecondChoicePaperRm4" CssClass="floaterL" AutoPostBack="true"
                                                                                                            OnCheckedChanged="cb_PaperChoiceCheckedChanged" runat="server" Checked="false"
                                                                                                            Text="" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Panel ID="pnl_ModOrMember4" Visible="false" runat="server">
                                                                                            <img src="Images/star.png" class="floaterL" alt="Assigned" style="border: none; margin-right: 3px" />
                                                                                            <asp:Label ID="lbl_ModOrMember4" CssClass="floaterL" Text="You're presenting!" runat="server" />
                                                                                        </asp:Panel>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td class="paperItem" runat="server" id="td_paper5">
                                                                            <table style="width: 100%">
                                                                                <tr class="paperCat">
                                                                                    <td>
                                                                                        <asp:Label ID="lbl_PaperCatRm5" runat="server" Font-Bold="true" Style="float: left;
                                                                                            vertical-align: top" Width="140" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="vertical-align: top; height: 150px">
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td style="vertical-align: top; padding-top: 4px;">
                                                                                                    <a id="hl_PaperDetail_5" runat="server" class="PaperItemLabel" title="More About This Paper">
                                                                                                        <img src="Images/info.gif" alt="detail" style="border: 0px solid silver; float: left" />
                                                                                                    </a>
                                                                                                </td>
                                                                                                <td style="vertical-align: top; padding-left: 5px;">
                                                                                                    <div style="height: 100px; width: 132px; float: left">
                                                                                                        <asp:Label ID="lbl_PaperTitleRm5" runat="server" Style="float: left; vertical-align: top"
                                                                                                            Width="140" />
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="vertical-align: bottom">
                                                                                        <div class="paperOptionBox" id="divPaperOptionBox5" runat="server">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td colspan="3">
                                                                                                        <div class="paperOptionsLabelDiv">
                                                                                                            <div style="float: right; margin-right: 5px">
                                                                                                                2<sup>nd</sup></div>
                                                                                                            <div style="float: right; margin-right: 13px">
                                                                                                                1<sup>st</sup></div>
                                                                                                        </div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td style="width: 90px; text-align: right; padding-right: 5px">
                                                                                                        Select:
                                                                                                    </td>
                                                                                                    <td style="width: 30px">
                                                                                                        <asp:CheckBox ID="cb_FirstChoicePaperRm5" CssClass="floaterL" AutoPostBack="true"
                                                                                                            OnCheckedChanged="cb_PaperChoiceCheckedChanged" runat="server" Checked="false"
                                                                                                            Text="" />
                                                                                                    </td>
                                                                                                    <td style="width: 30px">
                                                                                                        <asp:CheckBox ID="cb_SecondChoicePaperRm5" CssClass="floaterL" AutoPostBack="true"
                                                                                                            OnCheckedChanged="cb_PaperChoiceCheckedChanged" runat="server" Checked="false"
                                                                                                            Text="" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </td>
                                                                                </tr>
                                                                                <td>
                                                                                    <asp:Panel ID="pnl_ModOrMember5" Visible="false" runat="server">
                                                                                        <img src="Images/star.png" class="floaterL" alt="Assigned" style="border: none; margin-right: 3px" />
                                                                                        <asp:Label ID="lbl_ModOrMember5" CssClass="floaterL" Text="You're presenting!" runat="server" />
                                                                                    </asp:Panel>
                                                                                </td>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                        <SeparatorTemplate>
                                            <div style="height: 10px">
                                                &nbsp;</div>
                                        </SeparatorTemplate>
                                    </asp:DataList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-right: 35px; vertical-align: top">
                            <asp:Button ID="bn_Dummy" Height="35" Width="100" Style="display: none" CausesValidation="false"
                                runat="server" />
                            <div class="bnFormButtons">
                                <asp:Button ID="bn_Cancel" Height="35" Width="100" CssClass="aspHoverButton" CausesValidation="false"
                                    Text="Cancel" runat="server" OnClick="bn_Cancel_Click" />
                            </div>
                            <div class="bnFormButtons">
                                <asp:Button ID="bn_SubmitRSVP" Height="35" Width="100" CssClass="aspHoverButton"
                                    CausesValidation="false" Text="Submit RSVP" runat="server" OnClick="bn_SubmitRSVP_Click" />
                            </div>
                            <div style="float: right; margin-right: 10px">
                                <asp:UpdateProgress ID="progress_Submit" runat="server" DisplayAfter="100">
                                    <ProgressTemplate>
                                        <div align="left">
                                            Working....<br />
                                            <img src="Images/searching.gif" alt="" />
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                            <asp:UpdatePanel ID="up_ExecAdmin" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Panel ID="pnl_ExecAdmin" runat="server" Width="375" CssClass="hidden">
                                        <table>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <div style="width: 125px; float: left; text-align: center">
                                                        First Name</div>
                                                    <div style="width: 125px; float: left; margin-left: 3px; text-align: center">
                                                        Last Name</div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div style="width: 100px">
                                                        Admin Name:</div>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tb_ExecAdminFirstName" runat="server" Text="" CssClass="input" Width="123"
                                                        Style="float: left" />
                                                    <asp:TextBox ID="tb_ExecAdminLastName" runat="server" Text="" CssClass="input" Width="124"
                                                        Style="margin-left: 3px; float: left" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Admin Email:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tb_ExecAdminEmail" runat="server" Text="" CssClass="input" Width="253" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Admin Phone:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="tb_ExecAdminPhone" runat="server" Text="" CssClass="input" Width="253" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="bn_ShowUtilityModal" runat="server" Style="display: none" />
    <asp:ModalPopupExtender ID="mdl_UtilityModal" runat="server" TargetControlID="bn_ShowUtilityModal"
        PopupControlID="pnl_UtilityModal" BackgroundCssClass="modalBackground" />
    <asp:Panel ID="pnl_UtilityModal" runat="server" Width="500px" BackColor="White" BorderColor="Navy"
        BorderStyle="Solid" BorderWidth="2" Style="display: none; padding: 10px 10px 10px 10px">
        <asp:UpdatePanel ID="up_UtilityModal" runat="server">
            <ContentTemplate>
                <div id="container" style="display: table">
                    <h2>
                        <asp:Label ID="lbl_UtilityModalHeader" runat="server" Text=""></asp:Label></h2>
                    <div style="margin-top: 10px; margin-bottom: 5px; width: 95%">
                        <asp:Label ID="lbl_UtilityModalMessage" runat="server" Text=""></asp:Label>
                    </div>
                    <br />
                    <asp:TextBox ID="tb_UtilityModalEntry" CssClass="input" Style="width: 90%;" runat="server"></asp:TextBox><br />
                    <asp:Label ID="lbl_Error" CssClass="error" Visible="false" runat="server" />
                    <asp:CustomValidator ID="val_TextEntry" ControlToValidate="tb_UtilityModalEntry"
                        Display="Dynamic" runat="server"></asp:CustomValidator>
                    <asp:HiddenField ID="hdn_UtilityModalPurpose" Value="" runat="server" />
                    <asp:UpdateProgress ID="progress_Modal" runat="server" DisplayAfter="100">
                        <ProgressTemplate>
                            <div class="searching" align="center">
                                <asp:Label ID="lbl_ModalProgress" runat="server" Text="Working...."></asp:Label>
                                <br />
                                <br />
                                <img src="Images/searching.gif" alt="" />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <div align="right" style="width: 95%; margin-top: 15px; text-align:left;">
                    <asp:Button ID="bn_UtilityModaClose" runat="server" Text="Cancel" Width="75px" OnClick="bn_UtilityModalCancel_Click"
                        BackColor="White" BorderColor="Silver" BorderWidth="1" CssClass="aspHoverButton"
                        CausesValidation="false" />
                    <asp:Button ID="bn_UtilityModalSave" runat="server" Text="Save" Width="75px" OnClick="bn_UtilityModalSave_Click"
                        CssClass="aspHoverButton" CausesValidation="false" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <%--        </ContentTemplate>
    </asp:UpdatePanel>
    --%>
</asp:Content>
