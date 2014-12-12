
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 12/12/2014 11:26:03
-- Generated from EDMX file: \\vmware-host\Shared Folders\Documents\Visual Studio 2012\Projects\ProjetoIS\WCFPata\WCFPata\Model.edmx
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

IF OBJECT_ID(N'[dbo].[FK_TerapeutaConta]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TerapeutaSet] DROP CONSTRAINT [FK_TerapeutaConta];
GO
IF OBJECT_ID(N'[dbo].[FK_TerapeutaPaciente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PacienteSet] DROP CONSTRAINT [FK_TerapeutaPaciente];
GO
IF OBJECT_ID(N'[dbo].[FK_PacienteEpisodioClinico]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EpisodioClinicoSet] DROP CONSTRAINT [FK_PacienteEpisodioClinico];
GO
IF OBJECT_ID(N'[dbo].[FK_EpisodioClinicoSintoma_EpisodioClinico]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EpisodioClinicoSintoma] DROP CONSTRAINT [FK_EpisodioClinicoSintoma_EpisodioClinico];
GO
IF OBJECT_ID(N'[dbo].[FK_EpisodioClinicoSintoma_Sintoma]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EpisodioClinicoSintoma] DROP CONSTRAINT [FK_EpisodioClinicoSintoma_Sintoma];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ContaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContaSet];
GO
IF OBJECT_ID(N'[dbo].[TerapeutaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TerapeutaSet];
GO
IF OBJECT_ID(N'[dbo].[PacienteSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PacienteSet];
GO
IF OBJECT_ID(N'[dbo].[EpisodioClinicoSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EpisodioClinicoSet];
GO
IF OBJECT_ID(N'[dbo].[SintomaSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SintomaSet];
GO
IF OBJECT_ID(N'[dbo].[EpisodioClinicoSintoma]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EpisodioClinicoSintoma];
GO

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
    [data] datetime  NOT NULL,
    [diagnostico] nvarchar(max)  NOT NULL,
    [Paciente_Id] int  NOT NULL
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
    PRIMARY KEY NONCLUSTERED ([EpisodioClinico_Id], [Sintoma_Id] ASC);
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

-- Creating foreign key on [Paciente_Id] in table 'EpisodioClinicoSet'
ALTER TABLE [dbo].[EpisodioClinicoSet]
ADD CONSTRAINT [FK_EpisodioClinicoPaciente]
    FOREIGN KEY ([Paciente_Id])
    REFERENCES [dbo].[PacienteSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EpisodioClinicoPaciente'
CREATE INDEX [IX_FK_EpisodioClinicoPaciente]
ON [dbo].[EpisodioClinicoSet]
    ([Paciente_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------