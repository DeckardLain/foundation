/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [height]
      ,[bbpaddress]
      ,[worker]
      ,[bonus]
      ,[timestamp]
  FROM [Saved].[dbo].[Blocks] ORDER BY height DESC