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
CREATE PROCEDURE InsSharev2 
	-- Add the parameters for the stored procedure here
	@height float
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO Share (bbpaddress, shares, fails, SucXMR, FailXMR, SucXMRC, FailXMRC, BXMR, BXMRC, height, updated, Solved)
	SELECT bbpaddress, SUM(shares), 0, 0, 0, 0, 0, SUM(shares), 0, @height, GETDATE(), 1
	FROM Leaderboard GROUP BY bbpaddress
END
GO
