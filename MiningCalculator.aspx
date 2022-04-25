<%@ Page Title="MiningCalculator" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MiningCalculator.aspx.cs" Inherits="Saved.MiningCalculator" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h3>Mining Calculator</h3>
    <br />

    <table>
    <tr><td> Your Miner HPS:&nbsp;<td>    <asp:TextBox ID="txtHPS" width="500px" runat="server" ></asp:TextBox></tr>
   
    <tr><td> BBP Difficulty:&nbsp;<td>    <asp:TextBox ID="txtBBPDifficulty" width="500px" runat="server" ></asp:TextBox></tr>
    <tr><td> Block Reward (BBP):&nbsp;<td>    <asp:TextBox ID="txtBlockReward" width="500px" runat="server" ></asp:TextBox></tr>

    <tr><td> BBP Daily Revenue:&nbsp;<td>    <asp:TextBox ID="txtBBPDaily" width="500px" runat="server" readonly ></asp:TextBox></tr>   
    <tr><td> BBP Monthly Revenue:&nbsp;<td>    <asp:TextBox ID="txtBBPMonthly" width="500px" runat="server" readonly ></asp:TextBox></tr>

   
    </table>
    <asp:Button ID="btnCalculate" runat="server" Text="Calculate" OnClick="btnCalculate_Click" />


</asp:Content>
