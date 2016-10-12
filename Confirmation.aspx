<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Confirmation.aspx.cs" Inherits="Confirmation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up_Body" UpdateMode="Conditional" ChildrenAsTriggers="true"
        runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnl_Header" runat="server" CssClass="successHeader">
                <table>
                    <tr>
                        <td style="vertical-align: middle; padding-left: 40px">
                            <asp:Panel ID="pnl_Icon" runat="server" CssClass="successIcon">
                            </asp:Panel>
                        </td>
                        <td style="width: 825px; text-align: center; vertical-align: middle;">
                            <asp:Panel ID="pnl_HdrMessage" runat="server" CssClass="h2Confirmation" Style="width: 825px">
                                <div class="h2Confirmation">
                                    <asp:Label ID="lbl_HeaderMsgTitle" Text="Your registration is complete." runat="server" /></div>
                                <asp:Label ID="lbl_HeaderMsg" Font-Bold="true" Text="An email confirmation has been sent."
                                    runat="server" />
                                <br />
                                <asp:Panel ID="pnl_ConfirmationCode" Visible="true" HorizontalAlign="Center" runat="server">
                                    
                                        <asp:Label ID="lbl_ConfirmationCode" runat="server" />
                                    
                                </asp:Panel>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <table>
                <tr>
                    <td style="width: 650px" valign="top">
                        <asp:Panel runat="server" ID="rsvp_form" Style="margin-top: 10px; border: 1px solid silver;">
                            <table style="width: 100%;">
                                <tr>
                                    <td colspan="2" style="padding-right: 5px; padding-left: 5px;">
                                        <h2>
                                            <b>Personal Information</b></h2>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rsvpForm_LeftColumn" style="padding-top: 5px">
                                        Name</span>:
                                    </td>
                                    <td class="rsvpForm_RightColumn">
                                        <asp:Label ID="lbl_FullName" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rsvpForm_LeftColumn" style="padding-top: 5px">
                                        Division</span>:
                                    </td>
                                    <td class="rsvpForm_RightColumn">
                                        <asp:Label ID="lbl_Division" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rsvpForm_LeftColumn">
                                        Work Location:
                                    </td>
                                    <td class="rsvpForm_RightColumn">
                                        <asp:Label ID="lbl_WorkLocation" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rsvpForm_LeftColumn" style="padding-top: 5px">
                                        Job Role</span>:
                                    </td>
                                    <td class="rsvpForm_RightColumn">
                                        <asp:Label ID="lbl_JobRole" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rsvpForm_LeftColumn" style="padding-top: 5px">
                                        Mobile Phone:
                                    </td>
                                    <td class="rsvpForm_RightColumn">
                                        <asp:Label ID="lbl_MobilePhone" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rsvpForm_LeftColumn" style="padding-top: 5px">
                                        Shirt Size:
                                    </td>
                                    <td class="rsvpForm_RightColumn">
                                        <asp:Label ID="lbl_ShirtSize" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-right: 5px; padding-left: 5px;">
                                        <div style="width: 630px; border-top: 1px solid silver; margin-bottom: 5px;" />
                                        <h2>
                                            <b>Hospitality and Catering</b></h2>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rsvpForm_LeftColumn">
                                        Attending Sunday<br />
                                        Evening Reception?
                                    </td>
                                    <td class="rsvpForm_RightColumn">
                                        <asp:Panel ID="pnl_SundayReception" runat="server" CssClass="checkBoxClear" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rsvpForm_LeftColumn" style="padding-top: 10px">
                                        Interested in Sunday Golf?
                                    </td>
                                    <td class="rsvpForm_RightColumn">
                                        <asp:Panel ID="pnl_Golfing" runat="server" CssClass="checkBoxClear" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rsvpForm_LeftColumn" style="padding-top: 10px">
                                        Hotel Reservations:
                                    </td>
                                    <td class="rsvpForm_RightColumn">
                                        <table>
                                            <tr>
                                                <td style="width: 100px">
                                                    Checking In:
                                                </td>
                                                <td style="width: 540px">
                                                    <asp:Label ID="lbl_HotelCheckin" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px">
                                                    Checking Out:
                                                </td>
                                                <td style="width: 540px">
                                                    <asp:Label ID="lbl_HotelCheckout" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px">
                                                    Total Nights:
                                                </td>
                                                <td style="width: 540px">
                                                    <asp:Label ID="lbl_HotelDuration" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <div style="float: left; margin-top: 7px;">
                                                        <span>NOTE: Conference administrators will make your reservations for the nights selected.
                                                            <u><i>Please do not contact the hotel directly.</i></u></span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rsvpForm_LeftColumn" style="padding-top: 12px">
                                        Transportation:
                                    </td>
                                    <td class="rsvpForm_RightColumn">
                                        <table>
                                            <tr>
                                                <td style="width: 250px">
                                                    <div style="width: 250px; float: left; margin: 5px 5px 5px 5px">
                                                        KT Milpitas to Hyatt Regency Monterey:
                                                    </div>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_FromKT" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 250px">
                                                    <div style="width: 250px; float: left; margin: 5px 5px 5px 5px">
                                                    Hyatt Regency Monterey to KT Milpitas:
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_ToKT" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rsvpForm_LeftColumn" style="padding-top: 6px">
                                        Meal Choices:
                                    </td>
                                    <td class="rsvpForm_RightColumn">
                                        <asp:DataList ID="dl_MealDates" runat="server" RepeatDirection="Vertical" OnItemDataBound="dl_MealDates_ItemDataBound">
                                            <ItemTemplate>
                                                <div>
                                                    <div style="margin-bottom: 5px">
                                                        <asp:Label ID="lbl_MealDate" runat="server" Font-Bold="true" /><br />
                                                    </div>
                                                    <asp:DataList ID="dl_MealsOnDate" runat="server" RepeatDirection="Vertical" OnItemDataBound="dl_MealsOnDate_ItemDataBound">
                                                        <ItemTemplate>
                                                            <table style="margin-bottom: 5px">
                                                                <tr>
                                                                    <td style="width: 70px; text-align: left; margin-right: 5px">
                                                                        <asp:Label ID="lbl_MealType" runat="server" />
                                                                    </td>
                                                                    <td style="width: 100px; margin-left: 5px;">
                                                                        <asp:Panel ID="pnl_MealChoice" runat="server">
                                                                            <asp:Label ID="lbl_MealChoice" runat="server" /></asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:DataList>
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
                                        <asp:Label ID="lbl_FoodRestrictions" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rsvpForm_LeftColumn">
                                        Special Requirements or Needs:
                                    </td>
                                    <td class="rsvpForm_RightColumn">
                                        <asp:Label ID="lbl_SpecialNeeds" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="padding-right: 5px; padding-left: 5px;">
                                        <div style="width: 630px; border-top: 1px solid silver; margin-bottom: 5px;" />
                                        <h2>
                                            <b>Conference Participation Requests</b></h2>
                                    </td>
                                </tr>
                                <tr>
                                 <td class="rsvpForm_LeftColumn">
                                        Sunday Deep Learning Tutorial:
                                    </td>
                                     <td class="rsvpForm_RightColumn">
                                        <asp:Panel ID="pnl_SundayDeeopLearning" runat="server" CssClass="checkBoxClear" />
                                        <asp:HyperLink ID="hl_DeepLearningCalendar" runat="server" 
                                        NavigateUrl="http://collaboration.kla-tencor.com/EngineeringConference/SiteAssets/SundayDeepLearningTutorial.ics" Text="Add to your calendar"></asp:HyperLink>
                                     </td>
                                </tr>
                                <tr>
                                    <td class="rsvpForm_LeftColumn">
                                        Technical Panels:
                                    </td>
                                    <td class="rsvpForm_RightColumn">
                                        <asp:DataList ID="dl_TechPanelsList" OnItemDataBound="dl_TechPanelsList_ItemDataBound"
                                            RepeatDirection="Vertical" runat="server">
                                            <HeaderTemplate>
                                                <div style="width: 510px; margin-left: 6px; margin-bottom: 5px">
                                                    <i>Please Note: We'll strive to get you into your top choice, but seats will be allocated
                                                        on a 'first-come, first-served' basis.).<br />
                                                        You'll recieve confirmation of your allotted seat about two weeks prior to the conference.</i></div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div runat="server" id="div_TechPanelItem" style="margin-left: 6px">
                                                    <table style="border-collapse: collapse">
                                                        <tr>
                                                            <td valign="top" style="width: 30px; border: 1px solid silver; padding: 5px">
                                                                <asp:Label ID="lbl_TechPanelChoice" runat="server" />
                                                            </td>
                                                            <td valign="top" style="width: 455px; border: 1px solid silver; padding: 5px">
                                                                <div>
                                                                    <asp:Label ID="lbl_TechPanelTitle" CssClass="floaterL" runat="server" /></div>
                                                                <div>
                                                                    <asp:Label ID="lbl_TechPanelDetail" runat="server" CssClass="TechPanelItem" Style="margin-left: 5px;
                                                                        font-size: smaller" Text="(Detail)" />
                                                                </div>
                                                                <div>
                                                                    <i>
                                                                        <asp:Label ID="lbl_Moderator" runat="server" /></i></div>
                                                                <asp:Panel ID="pnl_ModOrMember" Visible="false" runat="server">
                                                                    <img src="Images/star.png" class="floaterL" alt="Assigned" style="border: none; margin-right: 3px" />
                                                                    <asp:Label ID="lbl_ModOrMember" CssClass="floaterL" Text="" runat="server" />
                                                                    <i>
                                                                        <asp:Label ID="lbl_TechPanelTopic" CssClass="hidden" runat="server" />
                                                                    </i>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rsvpForm_LeftColumn" style="padding-top: 10px">
                                        Paper Symposia</span>:
                                    </td>
                                    <td class="rsvpForm_RightColumn">
                                        <asp:DataList ID="dl_PaperSymposiaList" Layout="Flow" OnItemDataBound="dl_PaperSymposiaList_ItemDataBound"
                                            CellSpacing="5" RepeatDirection="Vertical" runat="server">
                                            <HeaderTemplate>
                                                <div style="float: left; width: 510px">
                                                    <i>Please Note: We'll strive to get you into your top choices, but seats will be allocated
                                                        on a 'first-come, first-served' basis.<br />
                                                        You'll recieve confirmation of your allotted seats about two weeks prior to the
                                                        conference. </i>
                                                </div>
                                            </HeaderTemplate>
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
                                                        <td style="vertical-align: top;">
                                                            <asp:DataList ID="dl_PaperTimeSlots" runat="server" OnItemDataBound="dl_PaperTimeSlots_ItemDataBound">
                                                                <ItemTemplate>
                                                                    <table style="border-collapse: collapse; padding: 5px">
                                                                        <tr>
                                                                            <td valign="top" style="width: 65px; padding: 5px; padding-top: 8px; border: 1px solid silver">
                                                                                <asp:Label ID="lbl_PaperTimeStart" CssClass="paperTimeSlot" Width="65" runat="server" />
                                                                            </td>
                                                                            <td style="border: 1px solid silver; padding: 5px; width: 465px">
                                                                                <asp:DataList ID="dl_PaperTimeSlotChoices" runat="server" OnItemDataBound="dl_PaperTimeSlotChoices_ItemDataBound">
                                                                                    <ItemTemplate>
                                                                                        <table>
                                                                                            <td valign="top" style="width: 30px">
                                                                                                <asp:Label ID="lbl_TimeSlotChoice" runat="server" />
                                                                                            </td>
                                                                                            <td valign="top">
                                                                                                <asp:HyperLink ID="hl_PaperTitle" runat="server" />
                                                                                                <asp:Panel ID="pnl_Presenter" Visible="false" runat="server">
                                                                                                    <img src="Images/star.png" class="floaterL" alt="Assigned" style="border: none; margin-right: 3px" />
                                                                                                    <asp:Label ID="lbl_Presenter" CssClass="floaterL" Text="You're presenting!" runat="server" />
                                                                                                </asp:Panel>
                                                                                            </td>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                </asp:DataList>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:DataList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="rsvpForm_LeftColumn">
                                        <asp:Panel ID="pnl_ExecAdminLabel" runat="server" Width="375" CssClass="hidden">
                                            Attendee Admin Assistant:</asp:Panel>
                                    </td>
                                    <td class="rsvpForm_RightColumn">
                                        <asp:Panel ID="pnl_ExecAdmin" runat="server" Width="375" CssClass="hidden">
                                            <table>
                                                <tr>
                                                    <td>
                                                        Name:
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_AdminName" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Admin Email:
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lbl_AdminEmail" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Admin Phone:
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblAdminPhone" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="cancel_form" CssClass="hidden" runat="server">
                            <asp:Label ID="lbl_Cancel" runat="server" />
                        </asp:Panel>
                    </td>
                    <td style="width: 350px; vertical-align: top; padding-left: 75px">
                        <table>
                            <tr>
                                <td colspan="2" style="vertical-align: top; padding-top: 10px">
                                    <asp:Panel runat="server" ID="pnl_POC" Style="width: 225px; z-index: 1">
                                        <asp:DataList ID="dl_ContactInfo" runat="server" DataSourceID="sqlds_POCs">
                                            <HeaderTemplate>
                                                <h2>
                                                    Contact Information</h2>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="margin-top: 4px">
                                                    <asp:Label runat="server" ID="lbl_POCName" Font-Bold="true" Text='<%# Eval("name") %>' /><br />
                                                    <asp:Label runat="server" ID="lbl_Phone" Text='<%# Eval("userWorkPhone") %>' /><br />
                                                    <asp:HyperLink ID="bl_EmailPOC" Text='<%# "Email " + Eval("userFirstName") %>' NavigateUrl='<%# "mailto:" + Eval("userEmail") %>'
                                                        runat="server"></asp:HyperLink></div>
                                            </ItemTemplate>
                                            <SeparatorTemplate>
                                                <div style="height: 4px">
                                                </div>
                                            </SeparatorTemplate>
                                        </asp:DataList>
                                        <asp:SqlDataSource ID="sqlds_POCs" runat="server" ConnectionString="<%$ ConnectionStrings:EngConferenceDB %>"
                                            SelectCommand="sp_GetConferencePOCs" SelectCommandType="StoredProcedure">
                                            <SelectParameters>
                                                <asp:Parameter DefaultValue="1" Name="conferenceID" Type="Int32" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="vertical-align: top; padding-top: 15px">
                                    <h2>
                                        Actions</h2>
                                </td>
                            </tr>
                            <tr>
                                <td class="iconCol">
                                    <img src="Images/edit_icon.gif" alt="Edit registration" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lb_EditRegistration" runat="server" CausesValidation="false"
                                        OnClick="lb_EditRegistration_Click" Text="Update My Registration" />
                                </td>
                            </tr>
                            <tr>
                                <td class="iconCol">
                                    <img src="Images/new_registration.png" alt="Edit registration" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lb_RegisterAnother" runat="server" Text="Register Another Participant"
                                        OnClick="lb_RegisterAnother_Click" />
                                </td>
                            </tr>
                            <tr id="tr_adminView" runat="server" class="hidden">
                                <td class="iconCol">
                                    <img src="Images/view.png" alt="Edit registration" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lb_ViewAdmin" runat="server" Text="View My Registrations" OnClick="lb_ViewAdmin_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td class="iconCol">
                                    <img src="Images/email.gif" alt="Edit registration" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lb_EmailDetails" runat="server" Text="Email Registration" OnClick="lb_EmailDetails_Click" />
                                </td>
                            </tr>
                            <tr class="hidden">
                                <td class="iconCol">
                                    <img src="Images/calendar.gif" alt="Edit registration" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lb_Calendar" runat="server" Text="Add to My Calendar" OnClick="lb_Calendar_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td class="iconCol">
                                    <img src="Images/deleteRSVP.png" alt="Edit registration" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lb_DeleteRegistration" runat="server" Text="Delete My Registration"
                                        OnClick="lb_DeleteRegistration_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="vertical-align: top; padding-top: 15px">
                                    <h2>
                                        Documents and Links</h2>
                                </td>
                            </tr>
                            <tr>
                                <td class="iconCol">
                                    <img src="Images/pdf.png" alt="Current Agenda" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lb_CurrentAgenda" runat="server" Text="Current Agenda" OnClick="lb_CurrentAgenda_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td class="iconCol">
                                    <img src="Images/pdf.png" alt="Conference FAQs" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="hl_ConferenceFAQs" runat="server" Text="Conference FAQs" NavigateUrl="~/Files/EngConf2014FAQs.pdf"
                                        Target="_blank" />
                                </td>
                            </tr>
                            <tr>
                                <td class="iconCol">
                                    <img src="Images/map.png" alt="Map and Directions" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="hl_Map" runat="server" Text="Map and Directions" NavigateUrl="http://www.google.com"
                                        Target="_blank" />
                                </td>
                            </tr>
                            <tr>
                                <td class="iconCol">
                                    <img src="Images/legal.png" alt="Multimedia Release Form" />
                                </td>
                                <td>
                                    <asp:HyperLink ID="hl_MediaRelease" runat="server" Text="Multimedia Release Waiver"
                                        NavigateUrl="~/Files/KT_photo_release_form.pdf" Target="_blank" />
                                </td>
                            </tr>
                            <tr>
                                <td class="iconCol">
                                    <img src="Images/exit.png" alt="Exit" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lb_Exit" runat="server" Text="Exit" OnClick="lb_Exit_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="up_Timer" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div>
                <asp:Timer ID="timer_Redirect" OnTick="timer_Redirect_Tick" runat="server">
                </asp:Timer>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="bn_ShowUtilityModal" runat="server" Style="display: none" />
    <asp:ModalPopupExtender ID="mdl_UtilityModal" runat="server" TargetControlID="bn_ShowUtilityModal"
        PopupControlID="pnl_UtilityModal" BackgroundCssClass="modalBackground" />
    <asp:Panel ID="pnl_UtilityModal" runat="server" Width="500px" BackColor="White" BorderColor="Navy"
        BorderStyle="Solid" BorderWidth="2" Style="display: none; padding: 10px 10px 10px 10px">
        <asp:UpdatePanel ID="up_UtilityModal" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="container" style="display: table">
                    <h2>
                        <asp:Label ID="lbl_UtilityModalHeader" runat="server" Text=""></asp:Label></h2>
                    <div style="margin-top: 10px; margin-bottom: 5px; width: 95%">
                        <asp:Label ID="lbl_UtilityModalMessage" runat="server" Text=""></asp:Label>
                    </div>
                    <br />
                    <br />
                    <asp:Panel ID="pnl_CboContainer" runat="server" CssClass="hidden">
                        <asp:ComboBox ID="cbo_AdminRsvps" Width="250" ForeColor="#696969" AutoPostBack="true"
                            OnSelectedIndexChanged="cbo_AdminRsvps_SelectedIndexChanged" ItemInsertLocation="OrdinalText"
                            CssClass="WindowsStyle" AutoCompleteMode="SuggestAppend" DropDownStyle="DropDownList"
                            runat="server">
                        </asp:ComboBox>
                    </asp:Panel>
                    <asp:TextBox ID="tb_UtilityModalEntry" runat="server" Style="width: 90%;" CssClass="input" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
        <div align="right" style="width: 95%; margin-top: 15px">
            <asp:Button ID="bn_UtilityModaClose" runat="server" Text="Cancel" Width="75px" OnClick="bn_UtilityModalCancel_Click"
                CssClass="aspHoverButton" CausesValidation="false" />            
                <asp:Button ID="bn_UtilityModalSave" runat="server" Text="OK" Width="75px" OnClick="bn_UtilityModalSave_Click"
                CssClass="aspHoverButton" CausesValidation="false" />

        </div>
    </asp:Panel>
</asp:Content>
