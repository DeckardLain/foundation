/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [height]
      ,[bbpaddress]
      ,[worker]
      ,[ShareRatio]
      ,[timestamp]
  FROM [Saved].[dbo].[Blocks] ORDER BY height DESC