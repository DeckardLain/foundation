# foundation
BiblePay Foundation WebSite and Pool

Modified by Scott Yoshimura - Hanalani Schools

Required changes to the database are included as .sql files in \SeedData\

# List of Changes
- Change from Proportional to [PPLNS](https://www.reddit.com/r/dogemining/comments/1xc356/guide_pps_rbpps_prop_pplns_whats_the_difference/) (1-hour share window) reward system. (See [pool hopping](https://bitcoin.stackexchange.com/questions/5072/what-is-pool-hopping).)
- Diff-weighted shares (Original pool code was awarding all shares equally--Rob has already applied his own implementation of this to Foundation pool.)
- Fixed hashrate calculation (for miners and pool total).
- Various database changes and performance improvements.
- Changed Leaderboard to Mining Status, updated every minute (vs. 2 minutes) and at every block mined by the pool:
	- Last 10 blocks mined by the pool and who mined them
	- List of pool miners by BBP address with the following stats:
		- 1-hour average hashrate.
		- Total diff-1 shares in the PPLNS share log.
		- Approximate reward percentage of the next block found.
		- Last share timestamp.
- Miner Info - search by your BBP mining address
	- Approximate percent of next block subsidy to be awarded and estimated BBP based on last block subsidy.
	- Total pending BBP (awarded, but not yet paid).
	- Total BBP paid in the last 30 days.
	- List of block rewards awarded (up to last 100 blocks) with height, percent of block subsidy, BBP amount, and transaction ID, if already paid.
	- List of recent payouts (up to 30), with timestamp, amount, and transaction ID.
	- List of recent blocks found (up to 10) with height, worker name, and timestamp.
	- Block heights and transaction IDs can be clicked to open in Chainz.
- About page
	- XMR fees are controlled by the upstream pool (minexmr.com) and subject to change, so the number will no longer be displayed here.
	- Next block target difficulty is included with height.
	- Total Blocks found is linked to the Chainz Extraction Statistics page for the pool mining address.
- Getting Started
	- You must agree to the terms and conditions before the pool address/port and instructions are displayed.  You must agree to the terms and conditions before mining at the pool.
	- Pool XMR donation address is displayed on this page if you have no interest in the XMR rewards.
  - Sample Windows batch file is provided, with failover pools (Foundation and miningpool.fun) configured.
