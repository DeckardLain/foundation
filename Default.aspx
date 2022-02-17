<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Saved._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div>
          Welcome to <%=Saved.Code.Common.GetLongSiteName(this) %>
      
    </div>
    <br />

    <div id="AboutBiblePay1" style="width:70vw;">
        <b>
            Launched in July 2017 with no premine and no ICO, BiblePay describes itself as a decentralized autonomous cryptocurrency that tithes 10% to orphan-charity (with Sanctuary governance).
              The project is passionate about spreading the gospel of Jesus, having the entire KJV bible compiled in its wallet utilizing RandomX.
            <br> BiblePay (BBP) is deflationary, decreasing its emissions by 19.5% per year. <br>
            <br>The project views itself as a utility that provides an alternative method for giving to charity.
             With Generic Smart Contracts, the project seeks to become the go-to wallet for Christians.In the future, the team intends to lease file space on its Sanctuaries and release corporate integration features, such as c# access to the blockchain. 
               The BiblePay platform is a derivative of Dash-Evolution. BiblePay is an international decentralized autonomous organization.  The Team seeks to help orphans globally. <br>
     </div>

    <br />
    <hr />

    <div>
        <a target="_blank" href="https://www.biblepay.org/">Biblepay Homepage</a>
        <br />
        <a target="_blank" href="https://www.hanalani.org/">Hanalani Schools Homepage</a>
    </div>
    
</asp:Content>
