
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/05/2014 10:46:35
-- Generated from EDMX file: C:\Users\nelson\Desktop\IS_3o_Ano_1S\IS\ProjetoInterS\WCFPata\WCFPata\Modelo.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
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

-- Creating table 'UtilizadorSet'
CREATE TABLE [dbo].[UtilizadorSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [username] nvarchar(max)  NOT NULL,
    [password] nvarchar(max)  NOT NULL,
    [nome] nvarchar(max)  NOT NULL,
    [token] nvarchar(max)  NOT NULL,
    [cc] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PacienteSet'
CREATE TABLE [dbo].[PacienteSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TerapeutaId] int  NOT NULL
);
GO

-- Creating table 'UtilizadorSet_Terapeuta'
CREATE TABLE [dbo].[UtilizadorSet_Terapeuta] (
    [Id] int  NOT NULL
);
GO

-- Creating table 'UtilizadorSet_Administrador'
CREATE TABLE [dbo].[UtilizadorSet_Administrador] (
    [Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'UtilizadorSet'
ALTER TABLE [dbo].[UtilizadorSet]
ADD CONSTRAINT [PK_UtilizadorSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PacienteSet'
ALTER TABLE [dbo].[PacienteSet]
ADD CONSTRAINT [PK_PacienteSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UtilizadorSet_Terapeuta'
ALTER TABLE [dbo].[UtilizadorSet_Terapeuta]
ADD CONSTRAINT [PK_UtilizadorSet_Terapeuta]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UtilizadorSet_Administrador'
ALTER TABLE [dbo].[UtilizadorSet_Administrador]
ADD CONSTRAINT [PK_UtilizadorSet_Administrador]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [TerapeutaId] in table 'PacienteSet'
ALTER TABLE [dbo].[PacienteSet]
ADD CONSTRAINT [FK_PacienteTerapeuta]
    FOREIGN KEY ([TerapeutaId])
    REFERENCES [dbo].[UtilizadorSet_Terapeuta]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PacienteTerapeuta'
CREATE INDEX [IX_FK_PacienteTerapeuta]
ON [dbo].[PacienteSet]
    ([TerapeutaId]);
GO

-- Creating foreign key on [Id] in table 'UtilizadorSet_Terapeuta'
ALTER TABLE [dbo].[UtilizadorSet_Terapeuta]
ADD CONSTRAINT [FK_Terapeuta_inherits_Utilizador]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[UtilizadorSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'UtilizadorSet_Administrador'
ALTER TABLE [dbo].[UtilizadorSet_Administrador]
ADD CONSTRAINT [FK_Administrador_inherits_Utilizador]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[UtilizadorSet]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------