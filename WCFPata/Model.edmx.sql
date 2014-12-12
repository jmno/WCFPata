
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 12/12/2014 10:40:20
-- Generated from EDMX file: C:\Users\nelson\Desktop\IS_3o_Ano_1S\IS\ProjetoInterS\WCFPata\WCFPata\Model.edmx
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

-- Creating table 'ContaSet'
CREATE TABLE [dbo].[ContaSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [username] nvarchar(max)  NOT NULL,
    [password] nvarchar(max)  NOT NULL,
    [isAdmin] bit  NOT NULL
);
GO

-- Creating table 'TerapeutaSet'
CREATE TABLE [dbo].[TerapeutaSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [nome] nvarchar(max)  NOT NULL,
    [cc] int  NOT NULL,
    [telefone] nvarchar(max)  NOT NULL,
    [dataNasc] datetime  NOT NULL,
    [Conta_Id] int  NOT NULL
);
GO

-- Creating table 'PacienteSet'
CREATE TABLE [dbo].[PacienteSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [nome] nvarchar(max)  NOT NULL,
    [dataNasc] datetime  NOT NULL,
    [morada] nvarchar(max)  NOT NULL,
    [cc] nvarchar(max)  NOT NULL,
    [telefone] nvarchar(max)  NOT NULL,
    [Terapeuta_Id] int  NOT NULL
);
GO

-- Creating table 'EpisodioClinicoSet'
CREATE TABLE [dbo].[EpisodioClinicoSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PacienteId] int  NOT NULL,
    [data] datetime  NOT NULL,
    [diagnostico] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SintomaSet'
CREATE TABLE [dbo].[SintomaSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [nome] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'EpisodioClinicoSintoma'
CREATE TABLE [dbo].[EpisodioClinicoSintoma] (
    [EpisodioClinico_Id] int  NOT NULL,
    [Sintoma_Id] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ContaSet'
ALTER TABLE [dbo].[ContaSet]
ADD CONSTRAINT [PK_ContaSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TerapeutaSet'
ALTER TABLE [dbo].[TerapeutaSet]
ADD CONSTRAINT [PK_TerapeutaSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PacienteSet'
ALTER TABLE [dbo].[PacienteSet]
ADD CONSTRAINT [PK_PacienteSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EpisodioClinicoSet'
ALTER TABLE [dbo].[EpisodioClinicoSet]
ADD CONSTRAINT [PK_EpisodioClinicoSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SintomaSet'
ALTER TABLE [dbo].[SintomaSet]
ADD CONSTRAINT [PK_SintomaSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [EpisodioClinico_Id], [Sintoma_Id] in table 'EpisodioClinicoSintoma'
ALTER TABLE [dbo].[EpisodioClinicoSintoma]
ADD CONSTRAINT [PK_EpisodioClinicoSintoma]
    PRIMARY KEY CLUSTERED ([EpisodioClinico_Id], [Sintoma_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Conta_Id] in table 'TerapeutaSet'
ALTER TABLE [dbo].[TerapeutaSet]
ADD CONSTRAINT [FK_TerapeutaConta]
    FOREIGN KEY ([Conta_Id])
    REFERENCES [dbo].[ContaSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TerapeutaConta'
CREATE INDEX [IX_FK_TerapeutaConta]
ON [dbo].[TerapeutaSet]
    ([Conta_Id]);
GO

-- Creating foreign key on [Terapeuta_Id] in table 'PacienteSet'
ALTER TABLE [dbo].[PacienteSet]
ADD CONSTRAINT [FK_TerapeutaPaciente]
    FOREIGN KEY ([Terapeuta_Id])
    REFERENCES [dbo].[TerapeutaSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TerapeutaPaciente'
CREATE INDEX [IX_FK_TerapeutaPaciente]
ON [dbo].[PacienteSet]
    ([Terapeuta_Id]);
GO

-- Creating foreign key on [PacienteId] in table 'EpisodioClinicoSet'
ALTER TABLE [dbo].[EpisodioClinicoSet]
ADD CONSTRAINT [FK_PacienteEpisodioClinico]
    FOREIGN KEY ([PacienteId])
    REFERENCES [dbo].[PacienteSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PacienteEpisodioClinico'
CREATE INDEX [IX_FK_PacienteEpisodioClinico]
ON [dbo].[EpisodioClinicoSet]
    ([PacienteId]);
GO

-- Creating foreign key on [EpisodioClinico_Id] in table 'EpisodioClinicoSintoma'
ALTER TABLE [dbo].[EpisodioClinicoSintoma]
ADD CONSTRAINT [FK_EpisodioClinicoSintoma_EpisodioClinico]
    FOREIGN KEY ([EpisodioClinico_Id])
    REFERENCES [dbo].[EpisodioClinicoSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Sintoma_Id] in table 'EpisodioClinicoSintoma'
ALTER TABLE [dbo].[EpisodioClinicoSintoma]
ADD CONSTRAINT [FK_EpisodioClinicoSintoma_Sintoma]
    FOREIGN KEY ([Sintoma_Id])
    REFERENCES [dbo].[SintomaSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EpisodioClinicoSintoma_Sintoma'
CREATE INDEX [IX_FK_EpisodioClinicoSintoma_Sintoma]
ON [dbo].[EpisodioClinicoSintoma]
    ([Sintoma_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------