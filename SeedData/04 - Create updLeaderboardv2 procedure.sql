-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Scott Yoshimura
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE updLeaderboardv2 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM RecentShares WHERE received < DATEADD(hour, -1, GETDATE())
	DECLARE @TotalShares as FLOAT
	SELECT @TotalShares = sum(shares) FROM RecentShares WITH (SNAPSHOT)

	BEGIN TRANSACTION
		DROP TABLE Leaderboard

		SELECT bbpaddress, SUM(shares) AS Shares, max(received) AS Updated,
		ROUND(SUM(shares)/3600,1) AS Hashrate, ROUND(SUM(shares)/@TotalShares*100,2) AS RewardPercent
		INTO Leaderboard
		FROM RecentShares WITH (SNAPSHOT)
		GROUP BY bbpaddress
	COMMIT TRANSACTION

END
GO
