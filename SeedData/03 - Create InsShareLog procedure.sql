--======================================================
-- Create Natively Compiled Stored Procedure Template
--======================================================

USE Saved
GO

-- Drop stored procedure if it already exists
IF OBJECT_ID('dbo.InsShareLog','P') IS NOT NULL
   DROP PROCEDURE dbo.InsShareLog
GO

CREATE PROCEDURE dbo.InsShareLog
	-- Add the parameters for the stored procedure here
	@miner varchar(90), 
	@shares float = 0
WITH NATIVE_COMPILATION, SCHEMABINDING
AS BEGIN ATOMIC WITH
(
 TRANSACTION ISOLATION LEVEL = SNAPSHOT, LANGUAGE = N'us_english'
)
   --Insert statements for the stored procedure here
 INSERT INTO dbo.RecentShares (bbpaddress, shares, received)
 VALUES (@miner, @shares, GETDATE())
END
GO
