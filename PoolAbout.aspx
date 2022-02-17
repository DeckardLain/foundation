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
    <table>
        <tr><td width="80%">
            <img src="Images/workers.png" width="95%"/>
            </td>
        </tr>
        <tr>
            <td>
            <img src="Images/hashrate.png" width="95%"/>
            </td>
        </tr>
        <tr>
            <td>
            <img src="Images/blockssolved.png" width="95%"/>
            </td>
        </tr>


    </table>




</asp:Content>
