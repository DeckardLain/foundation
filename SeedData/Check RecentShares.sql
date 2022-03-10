/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [id]
      ,[bbpaddress]
      ,[shares]
      ,[received]
  FROM [Saved].[dbo].[RecentShares] WITH (SNAPSHOT) ORDER BY received DESC