<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommentaryUserControl.ascx.cs" Inherits="EWSProject_Commentary.Commentary.CommentaryUserControl" %>

<p>
<strong>Your interested proposals</strong>
<asp:Table ID="interestedProposalsTable" runat="server" GridLines="Both" 
    style="height: 23px">
</asp:Table>
</p>
<br />
<p>
<strong>Current proposals</strong>
<asp:Table ID="proposalsTable" runat="server" GridLines="Both" 
    style="height: 23px">
</asp:Table>
</p>

<br />

<asp:Label ID="selectAProposalLabel" runat="server" Text="Select a Proposal: " 
    Font-Bold="True"></asp:Label><br />
<asp:DropDownList ID="proposalsList" runat="server" AutoPostBack="True" onselectedindexchanged="proposalsList_SelectedIndexChanged1">
</asp:DropDownList>

<br /><br />
    
<p>
<asp:Label ID="proposalInfoLabel" runat="server"></asp:Label>

<asp:Label ID="likeLabel" runat="server" Visible="False">Likes:&nbsp;</asp:Label>
<asp:Label ID="noOfCommentLikesLabel" runat="server" Visible="False" 
        ForeColor="#009933"></asp:Label>&nbsp;&nbsp;
<asp:Label ID="dislikeLabel" runat="server" Visible="False">Dislikes:&nbsp;</asp:Label>
<asp:Label ID="noOfCommentDislikesLabel" runat="server" 
    Visible="False" ForeColor="#FF3300"></asp:Label><br /><br />

<asp:LinkButton ID="likeCommentButton" runat="server" 
    onclick="likeCommentButton_Click" Visible="False" ForeColor="#009933">Like</asp:LinkButton>&nbsp;
<asp:LinkButton ID="dislikeCommentButton" runat="server" 
    onclick="dislikeCommentButton_Click" Visible="False" ForeColor="#FF3300">Dislike</asp:LinkButton>&nbsp;
<asp:LinkButton ID="indicateFlagLabelInterested" runat="server" Visible="False" 
    onclick="indicateFlagLabelInterested_Click" >I'm Interested</asp:LinkButton>
<asp:LinkButton ID="indicateFlagLabelInterestedNotInterested" runat="server" 
    Visible="False" onclick="indicateFlagLabelInterestedNotInterested_Click" >I'm Not Interested</asp:LinkButton>
</p>
<br />
<p>
<asp:Label ID="commentsLabel" runat="server" Text="Comments" Visible="False" 
        Font-Bold="True"></asp:Label>
<asp:Table ID="commentsTable" runat="server" GridLines="Both"></asp:Table>
</p>

<p>
<asp:TextBox ID="commentTextBox" runat="server" Visible="False"></asp:TextBox>
    <asp:Button ID="addCommentButton" runat="server" Text="Add" 
        onclick="addCommentButton_Click" Visible="False" />
</p>


<asp:HiddenField ID="currentPoposalID" runat="server" Value="-1" />



