use test_db;

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Test](
	[Uid] [uniqueidentifier] NOT NULL,
	[Name] [varchar](150) NOT NULL,
	[Email] [varchar](300) NOT NULL,
	CONSTRAINT [PK_Test] PRIMARY KEY CLUSTERED 
(
	[Uid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Test] ADD  CONSTRAINT [DF_Test_Uid]  DEFAULT (newid()) FOR [Uid]
GO

CREATE TABLE [dbo].[Text](
	[Uid] [uniqueidentifier] NOT NULL,
	[Name] [varchar](150) NOT NULL,
	[Description] [varchar](300) NOT NULL,
	[UrlImage] [varchar](5000) NOT NULL,
	CONSTRAINT [PK_Text] PRIMARY KEY CLUSTERED 
(
	[Uid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Text] ADD  CONSTRAINT [DF_Text_Uid]  DEFAULT (newid()) FOR [Uid]
GO

CREATE TABLE [dbo].[Product](
	[Uid] [uniqueidentifier] NOT NULL,
	[Name] [varchar](150) NOT NULL,
	[Description] [varchar](300) NOT NULL,
	[UrlImage] [varchar](5000) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Uid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_Uid]  DEFAULT (newid()) FOR [Uid]
GO

CREATE TABLE [dbo].[Post](
	[Uid] [uniqueidentifier] NOT NULL,
	[Name] [varchar](150) NOT NULL,
	[Description] [varchar](300) NOT NULL,
	[UrlImage] [varchar](5000) NOT NULL,
	CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED 
(
	[Uid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Post] ADD  CONSTRAINT [DF_Post_Uid]  DEFAULT (newid()) FOR [Uid]
GO