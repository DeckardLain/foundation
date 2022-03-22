<%@ Page Title="Leaderboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Leaderboard.aspx.cs" Inherits="Saved.Leaderboard" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Mining Status</h2>

    <div style="font-size:smaller;">
        <i>Stats for the last hour only, updated every minute.</i><br />
        <b>Hash Rate (1hr)</b> = Average hashrate based on shares within the last hour.<br />
        <b>Shares</b> = Weighted (to XMRig diff 1) shares accepted in the last hour.<br />
        <b>Reward Percent</b> = Approximate percentage of the subsidy to be awarded for the next block found by the pool.<br />
        <b>Last Share</b> = Last share submitted to the pool (UTC-10).
    </div>
    <hr />
    <%=GetLeaderboard() %>





</asp:Content>
