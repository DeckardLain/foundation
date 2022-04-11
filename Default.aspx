<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Saved._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


    <div>
          <b>Welcome to <%=Saved.Code.Common.GetLongSiteName(this) %></b>
      
    </div>
    <br />

    <div id="AboutBiblePay1" style="width:70vw;">
            Launched in July 2017 with no premine and no ICO, BiblePay describes itself as a decentralized autonomous cryptocurrency that tithes 10% to orphan-charity (with Sanctuary governance).
              The project is passionate about spreading the gospel of Jesus, having the entire KJV bible compiled in its wallet utilizing RandomX.
            <br> BiblePay (BBP) is deflationary, decreasing its emissions by 19.5% per year. <br>
            <br>The project views itself as a utility that provides an alternative method for giving to charity.
             With Generic Smart Contracts, the project seeks to become the go-to wallet for Christians.In the future, the team intends to lease file space on its Sanctuaries and release corporate integration features, such as c# access to the blockchain. 
               The BiblePay platform is a derivative of Dash-Evolution. BiblePay is an international decentralized autonomous organization.  The Team seeks to help orphans globally. <br>
     </div>

    <hr />
    <div style="width:70vw;">
    Located in Hawaii, the pool is based on the same code as Foundation pool, with a number of changes/improvements (listed below).<br /><br />
    <ul>
        <li>Change from Proportional to <a target="_blank" href="https://www.reddit.com/r/dogemining/comments/1xc356/guide_pps_rbpps_prop_pplns_whats_the_difference/"><b>PPLNS</b></a> (1-hour share window) reward system. (See <a target="_blank" href="https://bitcoin.stackexchange.com/questions/5072/what-is-pool-hopping">pool hopping</a>)</li>
        <li><b>Diff-weighted</b> shares (Original pool code was awarding all shares equally--Rob has already applied his own implementation of this to Foundation pool.)</li>
        <li>Fixed <b>hashrate</b> calculation (for miners and pool total).</li>
        <li>Various database changes and performance improvements.
            <ul>
                <li>Redid some networking code to eliminate occasional 4-8 second share submission times. This may also slightly <b>reduce stale (expired)</b> shares on the XMR side (I've observed a drop of ~3.5% to &lt;1.0% after the change).</li>
                <li>Check chain height every 3 seconds (vs. 60) to make sure we're working on the latest block</li>
            </ul>
        </li>

        <li>Changed Leaderboard to Mining Status, updated every minute (vs. 2 minutes) and at every block mined by the pool:
            <ul>
            <li>Last 10 blocks mined by the pool and who mined them</li>
            <li>List of pool miners by BBP address with the following stats:
                <ul>
                <li>1-hour average hashrate.</li>
                <li>Total diff-1 shares in the PPLNS share log.</li>
                <li>Approximate reward percentage of the next block found.</li>
                <li>Last share timestamp.</li>
                </ul>
            </li>
            </ul>
        </li>
        <li>Miner Info - search by your BBP mining address
            <ul>
            <li>Approximate percent of next block subsidy to be awarded and estimated BBP based on last block subsidy.</li>
            <li>Total pending BBP (awarded, but not yet paid).</li>
            <li>Total BBP paid in the last 30 days.</li>
            <li>List of block rewards awarded (up to last 100 blocks, updated every 20 minutes for blocks with at least 7 confirmations) with height, percent of block subsidy, BBP amount, and transaction ID, if already paid.</li>
            <li>List of recent payouts (up to 30), with timestamp, amount, and transaction ID.</li>
            <li>List of recent blocks found (up to 10) with height, worker name, and timestamp.</li>
            <li>Block heights and transaction IDs can be clicked to open in Chainz.</li>
            </ul>
        </li>
        <li>About Page
            <ul>
            <li>XMR fees are controlled by the upstream pool (minexmr.com) and subject to change, so the number will no longer be displayed here.</li>
            <li>Next block target difficulty is included with height.</li>
            <li>Total Blocks found is linked to the Chainz Extraction Statistics page for the pool mining address.</li>
            </ul>
        </li>
        <li>Getting Started
            <ul>
            <li>You must agree to the terms and conditions before the pool address/port and instructions are displayed.  You must agree to the terms and conditions before mining at the pool.</li>
            <li>Pool XMR donation address is displayed on this page if you have no interest in the XMR rewards.</li>
            <li>Sample Windows batch file is provided, with failover pools (Foundation and miningpool.fun) configured.</li>
            </ul>
        </li>
    </ul>
    </div>

    <hr />

    <div>
        <a target="_blank" href="https://www.biblepay.org/">Biblepay Homepage</a>
        <br />
        <a target="_blank" href="https://www.hanalani.org/">Hanalani Schools Homepage</a>
    </div>
    
</asp:Content>
