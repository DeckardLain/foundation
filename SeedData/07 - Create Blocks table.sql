USE [Saved]
GO

/****** Object:  Table [dbo].[Blocks]    Script Date: 3/15/2022 7:29:32 AM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Blocks]') AND type in (N'U'))
DROP TABLE [dbo].[Blocks]
GO

/****** Object:  Table [dbo].[Blocks]    Script Date: 3/15/2022 7:29:32 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Blocks](
	[height] [int] NOT NULL,
	[bbpaddress] [varchar](90) NOT NULL,
	[worker] [varchar](50) NULL,
	[ShareRatio] [float] NULL,
	[timestamp] [datetime] NULL,
 CONSTRAINT [PK_Blocks] PRIMARY KEY CLUSTERED 
(
	[height] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


