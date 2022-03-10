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
CREATE PROCEDURE CleanUp 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM Job WHERE added < GETDATE()-.5
	DELETE FROM Job WHERE added < GETDATE()-.05 AND SolvedTime IS NULL
	DELETE FROM Share WHERE updated < GETDATE()-30
	DELETE FROM Share WHERE bbpaddress = ''
	DELETE FROM HashRate WHERE Added < GETDATE()-7
	DELETE FROM Worker WHERE added < GETDATE()-2
	DELETE FROM bandetails WHERE added < GETDATE()-1
END
GO
