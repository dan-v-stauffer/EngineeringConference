﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Invitation.aspx.cs" Inherits="EmailTemplates_Invitation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Engineering Conference 2015</title>
    <style type="text/css">
/* DEFAULTS
----------------------------------------------------------*/


body   
{
/*    background: #b6b7bc; */
    font-size: .80em;
    font-family: Century Gothic, "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
    margin: 0px;
    padding: 0px;
    color: #696969;
}

a:link, a:visited
{
    color: #034af3;
}

a:hover
{
    color: #1d60ff;
    text-decoration: none;
}

a:active
{
    color: #034af3;
}

p
{
    margin-bottom: 10px;
    line-height: 1.6em;
}


/* HEADINGS   
----------------------------------------------------------*/

h1, h2, h3, h4, h5, h6
{
    font-size: 1.5em;
    color: #666666;
    font-variant: small-caps;
    text-transform: none;
    font-weight: 200;
    margin-bottom: 0px;
    font-family: Century Gothic, "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;

}

h1
{
    font-size: 2.0em;
    padding-bottom: 0px;
    margin-bottom: 0px;
    font-family: Century Gothic, "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
    font-weight:bold;
}

h2
{
    font-size: 1.5em;
    font-weight: 600;
    font-family: Century Gothic, "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
}

h3
{
    font-size: 1.2em;
}

h4
{
    font-size: 1.1em;
}

h5, h6
{
    font-size: 1em;
}

/* this rule styles <h1> and <h2> tags that are the 
first child of the left and right table columns */
.rightColumn > h1, .rightColumn > h2, .leftColumn > h1, .leftColumn > h2
{
    margin-top: 0px;
}


/* PRIMARY LAYOUT ELEMENTS   
----------------------------------------------------------*/

.page
{
    background-color: #fff;
    margin: 20px auto 0px auto;
}

.header
{
    position: relative;
    margin: 0px;
    padding: 0px;
    background-image: url(../Images/RSVP_Header.png);
    
    width: 100%;
    height:244px;
}

.header h1
{
    font-weight: 700;
    margin-top: 180px;
    padding: 0px 0px 0px 20px;
    color: #f9f9f9;
    border: none;
    line-height: 2em;
    font-size: 2em;
}

.main
{
    padding: 0px 12px;
    margin: 12px 8px 8px 8px;
    min-height: 420px;
    font-size:12pt;
}

.leftCol
{
    padding: 6px 0px;
    margin: 12px 8px 8px 8px;
    width: 200px;
    min-height: 200px;
}

.footer
{
    color: #4e5766;
    padding: 8px 0px 0px 0px;
    margin: 0px auto;
    text-align: center;
    line-height: normal;
}


/* TAB MENU   
----------------------------------------------------------*/

div.hideSkiplink
{
    background-color:#3a4f63;
    width:100%;
}

div.menu
{
    padding: 4px 0px 4px 8px;
}

div.menu ul
{
    list-style: none;
    margin: 0px;
    padding: 0px;
    width: auto;
}

div.menu ul li a, div.menu ul li a:visited
{
    background-color: #465c71;
    border: 1px #4e667d solid;
    color: #dde4ec;
    display: block;
    line-height: 1.35em;
    padding: 4px 20px;
    text-decoration: none;
    white-space: nowrap;
}

div.menu ul li a:hover
{
    background-color: #bfcbd6;
    color: #465c71;
    text-decoration: none;
}

div.menu ul li a:active
{
    background-color: #465c71;
    color: #cfdbe6;
    text-decoration: none;
}

/* FORM ELEMENTS   
----------------------------------------------------------*/

fieldset
{
    margin: 1em 0px;
    padding: 1em;
    border: 1px solid #ccc;
}

fieldset p 
{
    margin: 2px 12px 10px 10px;
}

fieldset.login label, fieldset.register label, fieldset.changePassword label
{
    display: block;
}

fieldset label.inline 
{
    display: inline;
}

legend 
{
    font-size: 1.1em;
    font-weight: 600;
    padding: 2px 4px 8px 4px;
}

input.textEntry 
{
    width: 320px;
    border: 1px solid #ccc;
}

input.passwordEntry 
{
    width: 320px;
    border: 1px solid #ccc;
}

div.accountInfo
{
    width: 42%;
}


/* MISC  
----------------------------------------------------------*/

.clear
{
    clear: both;
}

.title
{
    display: block;
    float: left;
    text-align: left;
    width: auto;
}

.loginDisplay
{
    font-size: 1.1em;
    display: block;
    text-align: right;
    padding: 10px;
    color: White;
}

.loginDisplay a:link
{
    color: white;
}

.loginDisplay a:visited
{
    color: white;
}

.loginDisplay a:hover
{
    color: white;
}

.failureNotification
{
    font-size: 1.2em;
    color: Red;
}

.bold
{
    font-weight: bold;
}

.submitButton
{
    text-align: right;
    padding-right: 10px;
}

.dropdown
{
    border:1px solid silver;
    height:30px;
    font-family: Century Gothic, "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
    color: #696969;
}

.sortButtonDiv
{
    float:right; 
    width:12px; 
    margin-left:5px; 
    margin-top:2px;
    margin-top:9px; 
    margin-bottom:9px;
}
.linkButtonDiv
{
    float:left; 
    margin-top:7px; 
    margin-bottom:7px;
}

.itemData
{
     border:1px solid silver;
}

.sortasc
{
 display:block; padding:0 4px 0 15px; 
  background:url(../Images/asc.gif) no-repeat;  
 }

 .sortdesc
 {
     display:block; padding:0 4px 0 15px; 
    background:url(../Images/desc.gif) no-repeat;
}

.sortasc-header A { background:url(../Images/asc.gif) right center no-repeat; }
.sortdesc-header A { background:url(../Images/desc.gif) right center no-repeat; }

.rounded-corners {
     -moz-border-radius: 8px;
    -webkit-border-radius: 8px;
    -khtml-border-radius: 8px;
    border-radius: 8px;
}

.hidden
{
    display:none;
}

.invisible
{
    visibility:hidden;
}

.headerTable
{
    width:600px;
}

.headerTable .leftColumn
{
    text-align: right; 
    padding-right: 15px; 
    padding-bottom: 8px; 
    vertical-align: top;
    width:140px;
}
.headerTable .rightColumn
{
    text-align: left; 
    padding-left: 15px; 
    vertical-align: top;
    width:600px;
}

.headerTable .mergedColumn
{
}

.headerTable .mergedRow
{
     padding-left: 5px;
}

.rsvpForm_LeftColumn
{
    text-align:right;
    padding-right:15px;
    padding-bottom:15px;
    padding-top:5px;    
    vertical-align:top;
    width:150px;
}

.rsvpForm_RightColumn
{
    text-align:left;
    padding-left:5px;
    padding-bottom:15px;
    padding-top:5px;    
    vertical-align:middle;
    width:950px;
}

.floaterL
{
    float:left;
}
.floaterLWide
{
    float:left;
    margin-left:15px;
    vertical-align:top;
}
.floaterR
{
    float:right;
}
.hyphen
{
    float:left;
    margin-left:3px;
    margin-right:3px;
}

.WindowsStyle .ajax__combobox_itemlist
{
    color: #696969;
    font-family: Century Gothic, "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
    position:inherit !important; 
}
    
.WindowsStyle .ajax__combobox_inputcontainer .ajax__combobox_textboxcontainer input
{
    margin: 0;
    border: solid 1px #7F9DB9;
    border-right: 0px none;
    padding: 1px 0px 0px 5px;
    font-size: 13px;
    height: 18px;
    position: relative;       
    font-family: Century Gothic, "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;

}
    
.WindowsStyle .ajax__combobox_inputcontainer .ajax__combobox_buttoncontainer button
{
    margin: 0;
    padding: 0;
    background-image: url(../Images/asc.gif);
    background-position: top left;
    border: 0px none;
    height: 21px;
    width: 21px;
    border: solid 1px #7F9DB9;
    font-family: Century Gothic, "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;

}
    
.dropDownList
{    
    border: 1px solid #7F9DB9;
    color: #696969;  
    background-image: url(../Images/asc.gif) !important; 
    background-position:top right;  
    background-repeat:no-repeat;  
    font-family: Century Gothic, "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
}    

.input
{
    border: 1px solid #7F9DB9;
    color: #696969;  
    font-family: Century Gothic, "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
    margin-left:5px;
}

.checkBox label
{
    margin-left:5px;
}

.mealDatesItem
{
    border: 1px solid #7F9DB9;
    margin: 5px 5px 5px 5px;
    padding: 3px 3px 3px 3px;
    vertical-align:top;
    height:150px;
    width: 350px;
    font-family: Century Gothic, "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
    float:left;
}

.cboContainer
{
    margin-left:5px;
}

.customSelectDiv
{
border: 1px solid #7F9DB9;
}
   
.customSelect
{
margin-left:5px;
font-size:small;
height:20px;
//border: 0px solid white;
border-radius: 0;
-webkit-appearance: none;
font-family: Century Gothic, "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;

}

.selectContainerStyled {
  position: relative;
  border: 1px solid #b5b5b5;
  border-radius: 4px;
  background-clip: padding-box;
  background-color: #f1f1f1;
  display:inline-block;
  box-shadow: 0 1px 3px rgba(0,0,0,.13), inset 0 1px 0 #fff;
  color: 0 1px 3px rgba(0,0,0,.13), inset 0 1px 0 #fff;
  background-image: linear-gradient(bottom, #dcdcdc 0%, #dcdcdc 2%, #f3f3f3 100%);
}
.selectContainerStyled select {
  float: left;
  position: relative;
  z-index: 2;
  height: 30px;
  display: block;
  line-height: 14px;
  padding: 5px 25px 4px 5px;
  margin: 0;
  -moz-appearance: window;
  -moz-padding-end: 19px;
  background: transparent;
  background-color: transparent;
  border: none;
  -webkit-appearance: none;
  appearance: none;
}

.itemWithDetail:hover {
    text-decoration: underline;
    color:Blue;
    cursor:default;
}

.horizontalRadioButtonList input
{
    margin-right:15px;
    margin-left:15px;
}

.techPanelChoiceLabels
{
    margin-left:11px;
}


.bnFormButtons
{
    float:right;
    margin: 5px 5px 5px 5px; 
    border:1px solid #7F9DB9;
}

.bnFormButtons: hover
{
   background-color:#00FF99;
}

 .modalBackground {
            background-color:Gray;
            filter:alpha(opacity=70);
            opacity:0.7;
        }
        
.paperItem
{
    width: 145px; 
    height: 150px; 
    border: 1px solid silver; 
    padding: 5px; 
    vertical-align: top;
}
        
.paperOptionBox
{
float: right; 
width: 145px; 
height:50px;
display: block; 
vertical-align:bottom !important;
}

.paperOptionsLabelDiv
{
    float: right; 
    width: 145px; 
    text-align: right;
    background-color
}

.paperRoomHeaderCell
{
    vertical-align: middle; 
    width: 145px; 
    text-align: center
}

.paperRooomHeaderText
{
    width:163px;
}

.paperSymposiumDiv
{
    border:1px solid silver;
    padding-top:5px;
    padding-bottom:5px;
}

.firstChoiceKey
{
    float:left; 
    width:10px;
    height:10px; 
    background-color: #98FB98; 
    border: 1px solid silver; 
    margin-right:5px;
    margin-top:5px;
    margin-left:5px;
}

.secondChoiceKey
{
    float:left; 
    width:10px;
    height:10px; 
    background-color: #D5D8F5; 
    border: 1px solid silver; 
    margin-right:8px;
    margin-top:5px;
    margin-left:5px;
}

.thirdChoiceKey
{
    float:left; 
    width:10px;
    height:10px; 
    background-color: #FFE4B5; 
    border: 1px solid silver; 
    margin-right:8px;
    margin-top:5px;
    margin-left:5px;
}

.error
{
    color: Red;
    font-style:italic;
}

    .searching
    {
        font-family: Century Gothic, "Lucida Grande", "Segoe UI", Arial, Helvetica, Verdana, sans-serif;
        border:1px solid silver;
        width: 175px;
        height: 75px;
        background-color: White;
        z-index: 999;
        margin: auto;
        position:absolute; 
        top:50%; 
        margin-top:-38px;
        left:50%;
        margin-left:-90px;
    }
    
.validationMsg
{
    margin-left:10px; 
    color:Red; 
    font-style:italic
}

.TechPanelItem:hover
{
    text-decoration:underline;
}

.execAdmin
{
    float: left; 
    margin-left:123px; 
    width:350px;
    margin-right: 15px; border: 1px solid silver;
}

.reqdNote
{
    color:Red;
}

.CheckBoxChecked
{
    width:21px;
    height:21px;
    background-image:url(../Images/CheckBox_Checked.png);
}
.CheckBoxClear
{
    width:21px;
    height:21px;
    background-image:url(../Images/CheckBox_Clear.png);
}
.successIcon
{
        width:75px;
    height:75px;
    background-image:url(../Images/green_checkmark.png);
}
.successMsg
{
    color: #173B0B;
}
.successHeader
{
    width: 100%; 
    background-color: #9FF781;
    border:1px solid #173B0B;
    padding-top:10px;
    padding-bottom:10px;
}
.errorIcon
{
        width:75px;
    height:75px;
    background-image:url(../Images/error.png);
}
.errorMsg
{
    color: #610B0B;
}
.errorHeader
{
    width: 100%; 
    background-color:#F5A9A9;
        border:1px solid #610B0B;
            padding-top:10px;
    padding-bottom:10px;


}

.h2Confirmation
{   font-size: 1.5em;
    font-variant: small-caps;
    text-transform: none;
    font-weight:  bold;
    margin-bottom: 0px;
}

.iconCol
{
    width:30px;
    text-align:center;
    vertical-align:middle;
}

.comboBoxInsideModalPopup
{
    position: relative;
}
.comboBoxInsideModalPopup ul
{
    position: absolute !important;
    left: 2px !important;
    top: 22px !important;
}    
    </style>
</head>
<body>
    <form id="form1" runat="server">
                {imgheader}
                <div class="main">
                    Dear
                    <asp:Label ID="lbl_userName" runat="server"></asp:Label>
                    ,<p>
                        We are pleased to invite you to the 2015 KLA-Tencor Engineering Conference in 
                        Monterey California! The Engineering Conference is a fantastic opportunity to 
                        connect, collaborate, and innovate with your peers and Engineering Leadership at KLA-Tencor.  
                    </p>
                    <p>
                       Highlights of this year's conference include six guest speakers from industry and academia, 
                       KT Executive speakers, 30 paper presentations by KT engineers, 26 technical forums/tutorials, 
                       and about 75 topical and product posters on display. </p>
                    <p>
                        As demand is very high for this confernece, <b>you are requested to register no later
                            than
                            <asp:Label ID="lbl_RegistrationEndDate" runat="server" Text="Label"></asp:Label>
                            . </b>After that date, registration will be closed and remaining seats may be reassigned. 
                            You can register at the <a href="http://chief-engineer/EngineeringConference/Registration">
                            Engineering Conference Registration Site.</a></p>
                    <p>
                        We look forward to seeing you there!</p>
                    <p>
                        Connect, Collaborate, Innovate,
                        <br />
                        Zain Saidin, EVP and Chief Engineer</p>
                </div>
                    <div style="width:100%; margin-bottom:5px">
                {imgsecuritymarking}
                </div>
                <div class="page">
                    <div class="clear">
                    </div>
                </div>
                {imgfooter}

    </form>
</body>
</html>
