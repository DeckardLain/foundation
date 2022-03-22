using Saved.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Saved.Code.Common;
using static Saved.Code.EntityHelper;

namespace Saved
{
    public partial class BlockHistory : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private string GetTd(DataRow dr, string colname, string sAnchor)
        {
            string val = dr[colname].ToString();
            string td = "<td>" + sAnchor + val + "</a></td>";
            return td;
        }

        public string GetReport()
        {
            return _report;
        }

        private string _report = "";
        protected void btnRunBlockHistory_Click(object sender, EventArgs e)
        {
            string bbpaddress = BMS.PurifySQL(txtAddress.Text, 100);
            string sql;

            // Miner address
            string html = "<h3>" + bbpaddress + "</h3>";

            // Current round share
            html += "<span title='Delayed up to 2 minutes'><b>Current Round Share: </b></span>";
            sql = "SELECT RewardPercent FROM Leaderboard WHERE bbpaddress='" + bbpaddress + "'";
            double rewardPercent = gData.GetScalarDouble(sql, "RewardPercent");
            html += rewardPercent + "%<br>";

            // Approximate reward
            sql = "SELECT Subsidy FROM Share WHERE Subsidy > 0 ORDER BY updated DESC";
            double lastSubsidy = gData.GetScalarDouble(sql, "Subsidy");
            html += "<span title='Based on last block subsidy won of " + lastSubsidy.ToString() + ".'><b>Approximate Reward Per Block: </b></span>";
            html += Math.Round(lastSubsidy * (1 - GetDouble(GetBMSConfigurationKeyValue("PoolFee"))) * rewardPercent / 100, 2) + " BBP<br>";

            // Pending rewards
            html += "<span title='Rewards become eligible for payout approximately 15 hours after block is solved.'><b>Pending Rewards: </b></span>";
            sql = "SELECT SUM(Reward) AS Reward FROM Share WHERE bbpaddress = '"+bbpaddress+"' AND Paid IS NULL";
            html += gData.GetScalarDouble(sql, "Reward");
            html += " BBP<br>";

            // Total payouts last 30 days
            html += "<b>Total Payouts (last 30 days): </b>";
            sql = "SELECT SUM(Reward) AS Reward FROM Share WHERE bbpaddress = '" + bbpaddress + "' AND Paid IS NOT NULL";
            html += gData.GetScalarDouble(sql, "Reward");
            html += " BBP<br><hr>";

            // Block Rewards
            html += "<h4>Block Rewards (Last 100)</h4>";
            sql = "Select TOP 100 Height, percentage, reward, subsidy, txid "
                + " FROM Share where subsidy > 1 and reward > 0 "
                +" and bbpaddress='" + bbpaddress + "' order by height desc";

            DataTable dt = gData.GetDataTable(sql);
            html += "<table class=saved><tr><th>Height</th><th>Percentage<th>Reward<th>Block Subsidy<th>TXID</tr>";

            string txid;
            for (int y = 0; y < dt.Rows.Count; y++)
            {
                txid = dt.Rows[y]["TXID"].ToString();

                string div = "<tr><td><a target=\"_blank\" href=\"https://chainz.cryptoid.info/bbp/block.dws?" + dt.Rows[y]["height"].ToString()
                    + ".htm\">" + dt.Rows[y]["height"].ToString() + "</a>"
                    + "<td>" + Math.Round(GetDouble(dt.Rows[y]["percentage"].ToString()) * 100, 4) + "%"
                    + "<td>" + dt.Rows[y]["reward"].ToString()
                    + "<td>" + dt.Rows[y]["subsidy"].ToString();
                if (txid == "")
                    div += "<td><small><nobr>Pending</nobr></small></tr>";
                else
                    div += "<td><small><nobr><a target=\"_blank\" href=\"https://chainz.cryptoid.info/bbp/tx.dws?" + txid + ".htm\">"+txid+"</a></nobr></small></tr>";

                html += div + "\r\n";

            }
            html += "</table>";

            // Payouts
            html += "<hr><h4>Payouts (Last 30)</h4>";
            sql = "SELECT TOP 30 TXID, Paid, SUM(Reward) AS Reward FROM Share WHERE bbpaddress='" + bbpaddress 
                + "' AND Paid IS NOT NULL GROUP BY TXID, Paid ORDER BY Paid DESC";
            dt = gData.GetDataTable(sql);
            html += "<table class=saved><tr><th>Timestamp</th><th>Amount<th>TXID</tr>";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string row = "<tr><td>" + dt.Rows[i]["Paid"].ToString()
                    + "<td>" + Math.Round(GetDouble(dt.Rows[i]["Reward"].ToString()), 2).ToString()
                    + "<td><small><nobr><a target=\"_blank\" href=\"https://chainz.cryptoid.info/bbp/tx.dws?" + dt.Rows[i]["TXID"].ToString()
                    + ".htm\">" + dt.Rows[i]["TXID"].ToString() + "</a></nobr></small></tr>";
                html += row + "\r\n";
            }

            html += "</table><hr>";

            // Blocks Found
            html += "<h4>Blocks Found (Last 10)</h4>";
            sql = "SELECT TOP 10 height, worker, timestamp FROM Blocks WHERE bbpaddress ='" + bbpaddress + "' ORDER BY height DESC";
            dt = gData.GetDataTable(sql);
            html += "<table class=saved><tr><th>Height</th><th>Worker</th><th>Timestamp (UTC-10)</th></tr>";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string row = "<tr><td><a target=\"_blank\" href=\"https://chainz.cryptoid.info/bbp/block.dws?" + dt.Rows[i]["height"].ToString()
                    + ".htm\">" + dt.Rows[i]["height"].ToString() + "</a>"
                    + "<td>" + dt.Rows[i]["worker"].ToString()
                    + "<td>" + dt.Rows[i]["timestamp"].ToString() + "</tr>\r\n";
                html += row;
            }
            html += "</table><hr>";

            _report = html;
        }
    }
}