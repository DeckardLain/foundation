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
    public partial class PoolAbout : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }


        public string GetTR(string key, string value)
        {
            string tr = "<TR><TD width='55%'>" + key + ":</TD><TD>" + value + "</TD></TR>\r\n";
            return tr;
        }
        public string GetImgSource()
        {
            try
            {
                string sql = "Select * from Bio";
                DataTable dt = gData.GetDataTable(sql, false);
                int nHour = (DateTime.Now.Hour+DateTime.Now.DayOfYear) % dt.Rows.Count;
                string url = dt.Rows[nHour]["URL"].ToString();
                return url;
            }
            catch(Exception)
            {
                return "https://i.ibb.co/W691XWC/Screen-Shot-2019-12-12-at-16-01-29.png";
            }
        }

        public string GetPoolAboutMetrics()
        {
            string html = "<table>";
            string sql = "Select sum(Hashrate) hr From Leaderboard";
            double dHR = gData.GetScalarDouble(sql, "hr");
            sql = "Select count(bbpaddress) ct from Leaderboard";
            double dCt = gData.GetScalarDouble(sql, "ct");
            html += GetTR("Miners", dCt.ToString());
            html += GetTR("Speed", UICommon.GetHPSLabel(dHR));

            html += GetTR("Contact E-Mail", "<a href=\"mailto:"+GetBMSConfigurationKeyValue("OperatorEmailAddress")+"\">"+ GetBMSConfigurationKeyValue("OperatorEmailAddress")+"</a>");
            html += GetTR("Pool Fees XMR", "See <a target=\"_blank\" href=\"https://minexmr.com/\">minexmr.com</a>");
            html += GetTR("Pool Fees BBP", Math.Round(GetDouble(GetBMSConfigurationKeyValue("PoolFee")) * 100, 2) + "%");
            html += GetTR("Reward System", "PPLNS with 1 hour share window");
            string nextPayout = UICommon.GetDurationString(PoolCommon.nLastPaid + 28800 - UnixTimeStamp());
            html += GetTR("Payouts", "Minimum 10 BBP, every 8 hours<br>(next payout in approx. "+nextPayout+")");
            html += GetTR("Block Bonus", Math.Round(GetDouble(GetBMSConfigurationKeyValue("PoolBlockBonus")), 0) + " BBP Per Block");
            
            html += GetTR("Build Version", PoolCommon.pool_version.ToString() + " Hanalani Revision 1.2.0");
            html += GetTR("Startup Time", PoolCommon.start_date.ToString());

            UInt64 iTarget = UInt64.Parse(Common._pool._template.target.Substring(0,12), System.Globalization.NumberStyles.HexNumber);
            double dDiff = 655350.0 / iTarget;
            string sHeight = String.Format("{0:n0}", PoolCommon.nGlobalHeight) + "<br>(BBP Diff: " + Math.Round(dDiff, 4) + " = XMR Diff: "+ String.Format("{0:n0}", Common._pool._template.expectedShares)+")";

            html += GetTR("Next Height", sHeight);

            sql = "SELECT TOP 1 timestamp FROM Blocks ORDER BY height DESC";
            DataTable dtLastBlock = gData.GetDataTable(sql);

            html += GetTR("Current Round Duration", UICommon.GetElapsedString((DateTime) dtLastBlock.Rows[0]["timestamp"]));
            if (dHR == 0)
                html += GetTR("Expected Block Time", "Calculating...");
            else
                html += GetTR("Expected Block Time", UICommon.GetDurationString(Common._pool._template.expectedShares / dHR));

            if (PoolCommon.roundLuck == 0)
                html += GetTR("Current Round Luck", "Calculating...");
            else
                html += GetTR("Current Round Luck", String.Format("{0:n0}", Math.Round(1.0/PoolCommon.roundLuck*100, 2))+"%");

            sql = "SELECT COUNT(height) AS ct FROM Blocks WHERE ShareRatio IS NOT NULL";
            double luckAvailableCount = gData.GetScalarDouble(sql, "ct");
            if (luckAvailableCount >= 5)
            {
                // 1-day luck
                sql = "SELECT COUNT(height) as count FROM Blocks WHERE ShareRatio IS NOT NULL AND timestamp > getdate()-1";
                double count = gData.GetScalarDouble(sql, "count");
                sql = "SELECT SUM(ShareRatio) AS ratio FROM Blocks WHERE ShareRatio IS NOT NULL AND timestamp > getdate()-1";
                double ratio = gData.GetScalarDouble(sql, "ratio");
                double luck;

                if (count > 0 && ratio > 0)
                {
                    luck = 1 / (ratio / count);
                    html += GetTR("1-Day Luck", Math.Round(luck * 100, 2).ToString() + "%");
                } else
                    html += GetTR("1-Day Luck", "Calculating...");

                // 30-day luck
                sql = "SELECT COUNT(height) as count FROM Blocks WHERE ShareRatio IS NOT NULL AND timestamp > getdate()-30";
                count = gData.GetScalarDouble(sql, "count");
                sql = "SELECT SUM(ShareRatio) AS ratio FROM Blocks WHERE ShareRatio IS NOT NULL AND timestamp > getdate()-30";
                ratio = gData.GetScalarDouble(sql, "ratio");

                if (count > 0 && ratio > 0)
                {
                    luck = 1 / (ratio / count);
                    html += GetTR("30-Day Luck", Math.Round(luck * 100, 2).ToString() + "%");
                }
                else
                    html += GetTR("30-Day Luck", "Calculating...");
            } else
            {
                html += GetTR("1-Day Luck", "Calculating...");
                html += GetTR("30-Day Luck", "Calculating...");
            }

            html += GetTR("Job Count", PoolCommon.dictJobs.Count().ToString());
            html += GetTR("Worker Count", PoolCommon.dictWorker.Count().ToString());

            sql = "Select sum(shares) suc, sum(fails) fail from Share (nolock) where updated > getdate()-1";
            double ts24 = gData.GetScalarDouble(sql, "suc");
            double tis24 = gData.GetScalarDouble(sql, "fail");
            html += GetTR("Total Shares (24 hours)", ts24.ToString());
            //html += GetTR("Total Invalid Shares (24 hours)", tis24.ToString());

            sql = "Select count(distinct height) h from Share (nolock) where updated > getdate()-1 and subsidy > 0 and reward > .05";
            double tbf24 = gData.GetScalarDouble(sql, "h");
            html += GetTR("Total <a target=\"_blank\" href=\"https://chainz.cryptoid.info/bbp/extraction.dws?1795771.htm\">Blocks Found</a> (24 hours)", tbf24.ToString());

            html += "</table>";
            return html;
        }
        
    }
}