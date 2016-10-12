<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="up_Body" UpdateMode="Conditional" runat="server" ChildrenAsTriggers="false">
        <ContentTemplate>

            <asp:Panel runat="server" ID="rsvp_form" Style="position: relative; margin-top: 10px;
                border: 1px solid silver;">
                <table>
                <tr>
                <td style="text-align:center">
                <table style="width:100%">
                <tr>
                <td style="text-align:right; padding-right:15px">
                <div style="border:0px solid silver;float:right; margin-right:15px; width:175px">
                  <asp:Button ID="bn_masterRegister" runat="server" Width="175" Height="40" CssClass="aspHoverButton" OnClick="bn_Register_Click"
                   Text="Register Me!" />
                   </div>
                </td>
                <td>
                <div style="border:0px solid red; width:175px">
                  <asp:Button ID="bn_declineRegister" runat="server" Width="175" Height="40" CssClass="aspHoverRedButton" OnClick="bn_DeclineRegister_Click"
                   Text="Sorry, I must decline." />
                   </div>
                </td>
                
                </tr>
                </table>
                </td>
                </tr>
                    <tr>
                        <td style="width: 650px">
                            <asp:Panel ID="pnl_Agenda" runat="server" Width="640">
                                <asp:DataList ID="dl_DayOfConference" OnItemDataBound="dl_DayOfConference_ItemDataBound"
                                    runat="server">
                                    <HeaderTemplate>
                                        <div class="floaterL" style="margin-left: 10px; margin-bottom: 10px">
                                            <h2>
                                                Conference Agenda</h2>
                                            <i>(Note: Subject to change)</i></div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div class="floaterL" style="margin-left: 10px; margin-top: 10px">
                                            <h3>
                                                <asp:Label ID="lbl_DayOfConference" Font-Bold="true" runat="server"></asp:Label>
                                            </h3>
                                        </div>
                                        <br />
                                        <div class="floaterL" style="padding-left: 10px; margin-top: 5px">
                                            <asp:DataList ID="dl_DailyAgenda" OnItemDataBound="dl_DailyAgenda_ItemDataBound"
                                                runat="server">
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td style="width: 125px; vertical-align: top; text-align: center" height="1">
                                                                <asp:Label ID="lbl_Time" runat="server" />
                                                            </td>
                                                            <td style="vertical-align: top;">
                                                                <div class="floaterL" style="margin-left: 10px;">
                                                                    <asp:Label ID="lbl_EventText" CssClass="floaterL" runat="server" /></div>
                                                                <div class="floaterL" style="width: 100%; margin-left: 15px;">
                                                                    <asp:Label ID="lbl_EventSpeaker" Font-Italic="true" CssClass="hidden" runat="server" /></div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <div style="padding-left: 5px; padding-right: 5px;">
                                                        <div style="width: 610px; border-bottom: 1px solid silver" />
                                                    </div>
                                                </FooterTemplate>
                                            </asp:DataList></div>
                                    </ItemTemplate>
                                </asp:DataList>
                            </asp:Panel>
                        </td>
                        <td valign="top" style="width: 350px; padding-top:95px; padding-left:85px">
                            <table>
                            
                                <tr>
                                    <td colspan="2" style="vertical-align: top; padding-top: 15px">
                                        <h2>
                                            Admin Only</h2>
                                    </td>
                                </tr>
                                 <tr>
                                 <td class="iconCol">
                                        <img src="Images/email.gif" alt="Survey Email" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lb_SendInvitations" runat="server" CausesValidation="false"
                                            OnClick="lb_SendInvitations_Click" Text="Send Invitations" />
                                    </td>
                                
                                </tr>
                                                                 <tr>
                                 <td class="iconCol">
                                        <img src="Images/email.gif" alt="Survey Email" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lb_BatchConfirmations" runat="server" CausesValidation="false"
                                            OnClick="lb_SendBatchConfirmations_Click" Text="Send Confirmation Batch" />
                                    </td>
                                
                                </tr>
                                <tr>
                                 <td class="iconCol">
                                        <img src="Images/email.gif" alt="Survey Email" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lb_SurveyEmail" runat="server" CausesValidation="false"
                                            OnClick="bn_SurveyEmail_Click" Text="Send Survey Emails" />
                                    </td>
                                
                                </tr>



                               <tr>
                                 <td class="iconCol">
                                        <img src="Images/email.gif" alt="Video Waivers" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lb_iConnectEmail" runat="server" CausesValidation="false"
                                            OnClick="bn_iConnectEmail_Click" Text="Send iConnect Lunch & Learn Emails" />
                                    </td>
                                
                                </tr>



                                 <tr style="display:none">
                                 <td class="iconCol">
                                        <img src="Images/email.gif" alt="Video Waivers" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="bn_VideoWaiversEmail" runat="server" CausesValidation="false"
                                            OnClick="bn_VideoWaiversEmail_Click" Text="Send Video WaiverEmails" />
                                    </td>
                                
                                </tr>


                                 <tr>
                                 <td class="iconCol">
                                        <img src="Images/event.png" alt="Allocate Panels" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="HyperLink1" NavigateUrl="~/EngSpeakers.aspx" runat="server">Engineering Speakers</asp:HyperLink>
                                    </td>
                                
                                </tr>
                              <tr style="display:none">
                                 <td class="iconCol">
                                        <img src="Images/event.png" alt="Allocate Panels" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lb_PostConferenceEmail" runat="server" CausesValidation="false"
                                            OnClick="lb_PostConferenceEmail_Click" Text="Send Post Conf Emails" />
                                    </td>
                                
                                </tr>
                             <tr>
                                    <td class="iconCol">
                                        <img src="Images/email.gif" alt="Edit registration" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lb_SendReminder" runat="server" CausesValidation="false"
                                            OnClick="bn_SendReminder_Click" Text="Send Reminder!" />
                                    </td>
                                </tr>
                                 <tr style="display:none">
                                 <td class="iconCol">
                                        <img src="Images/event.png" alt="Allocate Panels" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lb_AllocateTechPanels" runat="server" CausesValidation="false"
                                            OnClick="lb_AllocateTechPanels_Click" Text="Allocate Tech Panels" />
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
                                        <img src="Images/crowdCompass.png" alt="Send Mobile App Email" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lb_MobileAppEmail" runat="server" CausesValidation="false"
                                            OnClick="bn_MobileAppEmail_Click" Text="Send Mobile App Email" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="iconCol">
                                        <img src="Images/email.gif" alt="Send Test Email" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lb_TestEmail" runat="server" CausesValidation="false"
                                            OnClick="bn_TestEmail_Click" Text="Send Test Email" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="iconCol">
                                        <img src="Images/edit_icon.gif" alt="Edit registration" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lb_EditRegistration" runat="server" CausesValidation="false"
                                            OnClick="bn_Register_Click" Text="Register Me!" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="iconCol">
                                        <img src="Images/view.png" alt="Edit registration" />
                                    </td>
                                    <td>
                                        <asp:HyperLink ID="hl_ViewMyRegistration" runat="server" Text="View My Registration"
                                            NavigateUrl="Confirmation.aspx" />
                                    </td>
                                </tr>
                                                            <tr>
                                <td class="iconCol">
                                    <img src="Images/new_registration.png" alt="Edit registration" />
                                </td>
                                <td>
                                    <asp:LinkButton ID="lb_RegisterAnother" runat="server" Text="Register Another Participant"
                                        OnClick="bn_RegOther_Click" />
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
                            </table>
                        </td>
                    </tr>
                </table>
                <%--                <asp:Panel runat="server" ID="pnl_Register" Style="position: absolute; top: 150px;left:825px">
                    <table>
                        <tr>
                            <td>
                                <div class="bnFormButtons">
                                    <asp:Button ID="bn_Register" BorderStyle="None" Height="35" Width="125" BackColor="Transparent"
                                        CausesValidation="false" Text="Register Me!" runat="server" OnClick="bn_Register_Click" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="bnFormButtons">
                                    <asp:Button ID="bn_RegOther" BorderStyle="None" Height="35" Width="125" BackColor="Transparent"
                                        CausesValidation="false" Text="Register Another" runat="server" OnClick="bn_RegOther_Click" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>--%>
            </asp:Panel>
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
            </ContentTemplate>
        </asp:UpdatePanel>
        <div align="right" style="width: 95%; margin-top: 15px">
        
            <asp:Button ID="bn_UtilityModalSave" runat="server" Text="OK" Width="75px" OnClick="bn_UtilityModalSave_Click" CssClass="aspHoverButton"
                CausesValidation="false" />
            <asp:Button ID="bn_UtilityModaClose" runat="server" Text="Cancel" Width="75px" OnClick="bn_UtilityModalCancel_Click" CssClass="aspHoverButton"
                CausesValidation="false" />
        </div>
    </asp:Panel>
</asp:Content>
