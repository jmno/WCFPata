
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 11/28/2014 09:33:46
-- Generated from EDMX file: \\vmware-host\Shared Folders\Documents\Visual Studio 2012\Projects\ProjetoIS\WCFPata\WCFPata\Modelo.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [db35768e000b9f4e7a85c6a3f1009b11c7];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Entity1Set'
CREATE TABLE [dbo].[Entity1Set] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'Entity2Set'
CREATE TABLE [dbo].[Entity2Set] (
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Entity1Set'
ALTER TABLE [dbo].[Entity1Set]
ADD CONSTRAINT [PK_Entity1Set]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Entity2Set'
ALTER TABLE [dbo].[Entity2Set]
ADD CONSTRAINT [PK_Entity2Set]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------