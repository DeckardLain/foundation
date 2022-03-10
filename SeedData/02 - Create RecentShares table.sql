--======================================================
-- Add MEMORY_OPTIMIZED_DATA Filegroup and Create Memory Optimized Table Template
-- Use the Specify Values for Template Parameters command (Ctrl-Shift-M) to fill in the parameter values below.
-- This template adds a MEMORY_OPTIMIZED_DATA filegroup to the database, and creates a memory optimized table and indexes on the memory optimized table.
-- The database must have a MEMORY_OPTIMIZED_DATA filegroup before the memory optimized table can be created.
--======================================================

USE Saved
GO

--Drop table if it already exists.
IF OBJECT_ID('dbo.RecentShares','U') IS NOT NULL
    DROP TABLE dbo.RecentShares
GO

--Create memory optimized table and indexes on the memory optimized table.
CREATE TABLE dbo.RecentShares
(
	id INT NOT NULL IDENTITY PRIMARY KEY NONCLUSTERED,
	bbpaddress varchar(90) NOT NULL, 
	shares float NOT NULL,
	received datetime NOT NULL
) WITH (MEMORY_OPTIMIZED = ON, DURABILITY = SCHEMA_ONLY)
GO