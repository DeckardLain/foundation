using Saved.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Saved.Code.Common;
using static Saved.Code.PoolCommon;

namespace Saved
{
    public partial class MiningCalculator : Page
    {
        double GetAvgHashRate()
        {
            string sql = "select avg(hashRate) hr from hashrate where added > getdate()-1";
            double nHash = gData.GetScalarDouble(sql, "hr");
            return nHash;
        }

        double GetAvgBlocksFound()
        {
            string sql = "select avg(solvedCount) ct from hashrate where added > getdate()-1";
            double nCt = gData.GetScalarDouble(sql, "ct");
            return nCt;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {

            }
            else
            {
                txtHPS.Text = "10000";

                // These are pulled from the pool
                string sql = "SELECT Subsidy FROM Share WHERE Subsidy > 0 ORDER BY updated DESC";
                double nLastSubsidy = gData.GetScalarDouble(sql, "Subsidy");

                double nBonus = GetDouble(GetBMSConfigurationKeyValue("PoolBlockBonus"));
                double nBBPReward = nLastSubsidy + nBonus;
                txtBlockReward.Text = nBBPReward.ToString();

                UInt64 iTarget = UInt64.Parse(Common._pool._template.target.Substring(0, 12), System.Globalization.NumberStyles.HexNumber);
                double dDiff = 655350.0 / iTarget;
                txtBBPDifficulty.Text = Math.Round(dDiff, 4).ToString();
            }
            btnCalculate_Click(this, null);

        }

        string PrintDouble(double n)
        {
            return String.Format("{0:n10}", Math.Round(n, 10));
        }
        protected void btnCalculate_Click (object sender, EventArgs e)
        {
            double nHPS = GetDouble(txtHPS.Text);
            double nDiff = GetDouble(txtBBPDifficulty.Text);
            double xmrDiff = 429503283 * nDiff;
            double blockReward = GetDouble(txtBlockReward.Text);

            txtBBPDaily.Text = PrintDouble(nHPS * 86400 / xmrDiff * blockReward);
            txtBBPMonthly.Text = PrintDouble(nHPS * 2592000 / xmrDiff * blockReward);
        }
    }
}