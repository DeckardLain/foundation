<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="PoolAbout.aspx.cs" Inherits="Saved.PoolAbout" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>About - BiblePay Pool</h3>

    <p>            </p>
    <p>&nbsp;</p>



    <br />
    <p>
        <span><font color="red">
            <br /><%=Saved.Code.PoolCommon.PoolBonusNarrative() %>

              </font></span>
    </p>
    
    <hr />
    <%=GetPoolAboutMetrics() %>
    <br />


</asp:Content>
