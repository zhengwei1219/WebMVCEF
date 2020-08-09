
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/27/2020 21:11:15
-- Generated from EDMX file: E:\MyProject\webMVCEF\ZhengWeil.ExtractOil.Model\ExtractOil.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ExtractOil];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_UserInfoRoleInfo_UserInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserInfoRoleInfo] DROP CONSTRAINT [FK_UserInfoRoleInfo_UserInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_UserInfoRoleInfo_RoleInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserInfoRoleInfo] DROP CONSTRAINT [FK_UserInfoRoleInfo_RoleInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_R_UserInfo_ActionInfoUserInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[R_UserInfo_ActionInfo] DROP CONSTRAINT [FK_R_UserInfo_ActionInfoUserInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_R_UserInfo_ActionInfoActionInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[R_UserInfo_ActionInfo] DROP CONSTRAINT [FK_R_UserInfo_ActionInfoActionInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_ActionInfoRoleInfo_ActionInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ActionInfoRoleInfo] DROP CONSTRAINT [FK_ActionInfoRoleInfo_ActionInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_ActionInfoRoleInfo_RoleInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ActionInfoRoleInfo] DROP CONSTRAINT [FK_ActionInfoRoleInfo_RoleInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentUserInfo_Department]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentUserInfo] DROP CONSTRAINT [FK_DepartmentUserInfo_Department];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentUserInfo_UserInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentUserInfo] DROP CONSTRAINT [FK_DepartmentUserInfo_UserInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentActionInfo_Department]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentActionInfo] DROP CONSTRAINT [FK_DepartmentActionInfo_Department];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentActionInfo_ActionInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentActionInfo] DROP CONSTRAINT [FK_DepartmentActionInfo_ActionInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_Replys_Topics]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Replys] DROP CONSTRAINT [FK_Replys_Topics];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserInfo];
GO
IF OBJECT_ID(N'[dbo].[RoleInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleInfo];
GO
IF OBJECT_ID(N'[dbo].[Department]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Department];
GO
IF OBJECT_ID(N'[dbo].[R_UserInfo_ActionInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[R_UserInfo_ActionInfo];
GO
IF OBJECT_ID(N'[dbo].[ActionInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ActionInfo];
GO
IF OBJECT_ID(N'[dbo].[Replys]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Replys];
GO
IF OBJECT_ID(N'[dbo].[Topics]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Topics];
GO
IF OBJECT_ID(N'[dbo].[DocumentInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentInfo];
GO
IF OBJECT_ID(N'[dbo].[UserInfoRoleInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserInfoRoleInfo];
GO
IF OBJECT_ID(N'[dbo].[ActionInfoRoleInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ActionInfoRoleInfo];
GO
IF OBJECT_ID(N'[dbo].[DepartmentUserInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartmentUserInfo];
GO
IF OBJECT_ID(N'[dbo].[DepartmentActionInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartmentActionInfo];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserInfo'
CREATE TABLE [dbo].[UserInfo] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Pwd] nvarchar(50)  NOT NULL,
    [SubTime] nvarchar(50)  NULL,
    [DelFlag] bit  NOT NULL,
    [ModifiedOn] nvarchar(50)  NULL,
    [Remark] nvarchar(50)  NULL,
    [Sort] nvarchar(50)  NULL
);
GO

-- Creating table 'RoleInfo'
CREATE TABLE [dbo].[RoleInfo] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RoleName] nvarchar(max)  NOT NULL,
    [DelFlag] nvarchar(max)  NOT NULL,
    [SubTime] datetime  NOT NULL,
    [Remark] nvarchar(max)  NOT NULL,
    [ModifiedOn] datetime  NOT NULL,
    [Sort] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Department'
CREATE TABLE [dbo].[Department] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DepName] nvarchar(max)  NOT NULL,
    [ParentId] nvarchar(max)  NOT NULL,
    [TreePath] nvarchar(max)  NOT NULL,
    [Level] nvarchar(max)  NOT NULL,
    [IsLeaf] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'R_UserInfo_ActionInfo'
CREATE TABLE [dbo].[R_UserInfo_ActionInfo] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [UserInfoID] nvarchar(max)  NOT NULL,
    [ActionInfoID] nvarchar(max)  NOT NULL,
    [IsPass] nvarchar(max)  NOT NULL,
    [UserInfo_ID] int  NOT NULL,
    [ActionInfo_ID] int  NOT NULL
);
GO

-- Creating table 'ActionInfo'
CREATE TABLE [dbo].[ActionInfo] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SubTime] nvarchar(max)  NOT NULL,
    [DelFlag] nvarchar(max)  NOT NULL,
    [ModifiedOn] nvarchar(max)  NOT NULL,
    [Remark] nvarchar(max)  NOT NULL,
    [Url] nvarchar(max)  NOT NULL,
    [HttpMethod] nvarchar(max)  NOT NULL,
    [ActionMethod] nvarchar(max)  NOT NULL,
    [ControllerName] nvarchar(max)  NOT NULL,
    [ActionInfoName] nvarchar(max)  NOT NULL,
    [MenuIcon] nvarchar(max)  NOT NULL,
    [IconWidth] nvarchar(max)  NOT NULL,
    [IconHeight] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Replys'
CREATE TABLE [dbo].[Replys] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TopicId] int  NOT NULL,
    [ReplyDate] datetime  NULL,
    [Content] nvarchar(1000)  NULL,
    [ReplyName] nvarchar(50)  NULL,
    [UserHostAddress] nvarchar(50)  NULL
);
GO

-- Creating table 'Topics'
CREATE TABLE [dbo].[Topics] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(100)  NOT NULL,
    [Author] nvarchar(20)  NULL,
    [AddDate] datetime  NULL,
    [Content] nvarchar(1000)  NULL,
    [UserName] nvarchar(50)  NULL,
    [UserHostAddress] nvarchar(50)  NULL,
    [LastModifyDate] datetime  NULL
);
GO

-- Creating table 'DocumentInfo'
CREATE TABLE [dbo].[DocumentInfo] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FileName] nvarchar(1000)  NULL,
    [FilePath] nvarchar(2000)  NOT NULL,
    [CreateDate] datetime  NULL
);
GO

-- Creating table 'UserInfoRoleInfo'
CREATE TABLE [dbo].[UserInfoRoleInfo] (
    [UserInfo_ID] int  NOT NULL,
    [RoleInfo_ID] int  NOT NULL
);
GO

-- Creating table 'ActionInfoRoleInfo'
CREATE TABLE [dbo].[ActionInfoRoleInfo] (
    [ActionInfo_ID] int  NOT NULL,
    [RoleInfo_ID] int  NOT NULL
);
GO

-- Creating table 'DepartmentUserInfo'
CREATE TABLE [dbo].[DepartmentUserInfo] (
    [Department_ID] int  NOT NULL,
    [UserInfo_ID] int  NOT NULL
);
GO

-- Creating table 'DepartmentActionInfo'
CREATE TABLE [dbo].[DepartmentActionInfo] (
    [Department_ID] int  NOT NULL,
    [ActionInfo_ID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'UserInfo'
ALTER TABLE [dbo].[UserInfo]
ADD CONSTRAINT [PK_UserInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RoleInfo'
ALTER TABLE [dbo].[RoleInfo]
ADD CONSTRAINT [PK_RoleInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Department'
ALTER TABLE [dbo].[Department]
ADD CONSTRAINT [PK_Department]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'R_UserInfo_ActionInfo'
ALTER TABLE [dbo].[R_UserInfo_ActionInfo]
ADD CONSTRAINT [PK_R_UserInfo_ActionInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ActionInfo'
ALTER TABLE [dbo].[ActionInfo]
ADD CONSTRAINT [PK_ActionInfo]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id] in table 'Replys'
ALTER TABLE [dbo].[Replys]
ADD CONSTRAINT [PK_Replys]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Topics'
ALTER TABLE [dbo].[Topics]
ADD CONSTRAINT [PK_Topics]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'DocumentInfo'
ALTER TABLE [dbo].[DocumentInfo]
ADD CONSTRAINT [PK_DocumentInfo]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UserInfo_ID], [RoleInfo_ID] in table 'UserInfoRoleInfo'
ALTER TABLE [dbo].[UserInfoRoleInfo]
ADD CONSTRAINT [PK_UserInfoRoleInfo]
    PRIMARY KEY CLUSTERED ([UserInfo_ID], [RoleInfo_ID] ASC);
GO

-- Creating primary key on [ActionInfo_ID], [RoleInfo_ID] in table 'ActionInfoRoleInfo'
ALTER TABLE [dbo].[ActionInfoRoleInfo]
ADD CONSTRAINT [PK_ActionInfoRoleInfo]
    PRIMARY KEY CLUSTERED ([ActionInfo_ID], [RoleInfo_ID] ASC);
GO

-- Creating primary key on [Department_ID], [UserInfo_ID] in table 'DepartmentUserInfo'
ALTER TABLE [dbo].[DepartmentUserInfo]
ADD CONSTRAINT [PK_DepartmentUserInfo]
    PRIMARY KEY CLUSTERED ([Department_ID], [UserInfo_ID] ASC);
GO

-- Creating primary key on [Department_ID], [ActionInfo_ID] in table 'DepartmentActionInfo'
ALTER TABLE [dbo].[DepartmentActionInfo]
ADD CONSTRAINT [PK_DepartmentActionInfo]
    PRIMARY KEY CLUSTERED ([Department_ID], [ActionInfo_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserInfo_ID] in table 'UserInfoRoleInfo'
ALTER TABLE [dbo].[UserInfoRoleInfo]
ADD CONSTRAINT [FK_UserInfoRoleInfo_UserInfo]
    FOREIGN KEY ([UserInfo_ID])
    REFERENCES [dbo].[UserInfo]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RoleInfo_ID] in table 'UserInfoRoleInfo'
ALTER TABLE [dbo].[UserInfoRoleInfo]
ADD CONSTRAINT [FK_UserInfoRoleInfo_RoleInfo]
    FOREIGN KEY ([RoleInfo_ID])
    REFERENCES [dbo].[RoleInfo]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserInfoRoleInfo_RoleInfo'
CREATE INDEX [IX_FK_UserInfoRoleInfo_RoleInfo]
ON [dbo].[UserInfoRoleInfo]
    ([RoleInfo_ID]);
GO

-- Creating foreign key on [UserInfo_ID] in table 'R_UserInfo_ActionInfo'
ALTER TABLE [dbo].[R_UserInfo_ActionInfo]
ADD CONSTRAINT [FK_R_UserInfo_ActionInfoUserInfo]
    FOREIGN KEY ([UserInfo_ID])
    REFERENCES [dbo].[UserInfo]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_R_UserInfo_ActionInfoUserInfo'
CREATE INDEX [IX_FK_R_UserInfo_ActionInfoUserInfo]
ON [dbo].[R_UserInfo_ActionInfo]
    ([UserInfo_ID]);
GO

-- Creating foreign key on [ActionInfo_ID] in table 'R_UserInfo_ActionInfo'
ALTER TABLE [dbo].[R_UserInfo_ActionInfo]
ADD CONSTRAINT [FK_R_UserInfo_ActionInfoActionInfo]
    FOREIGN KEY ([ActionInfo_ID])
    REFERENCES [dbo].[ActionInfo]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_R_UserInfo_ActionInfoActionInfo'
CREATE INDEX [IX_FK_R_UserInfo_ActionInfoActionInfo]
ON [dbo].[R_UserInfo_ActionInfo]
    ([ActionInfo_ID]);
GO

-- Creating foreign key on [ActionInfo_ID] in table 'ActionInfoRoleInfo'
ALTER TABLE [dbo].[ActionInfoRoleInfo]
ADD CONSTRAINT [FK_ActionInfoRoleInfo_ActionInfo]
    FOREIGN KEY ([ActionInfo_ID])
    REFERENCES [dbo].[ActionInfo]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RoleInfo_ID] in table 'ActionInfoRoleInfo'
ALTER TABLE [dbo].[ActionInfoRoleInfo]
ADD CONSTRAINT [FK_ActionInfoRoleInfo_RoleInfo]
    FOREIGN KEY ([RoleInfo_ID])
    REFERENCES [dbo].[RoleInfo]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ActionInfoRoleInfo_RoleInfo'
CREATE INDEX [IX_FK_ActionInfoRoleInfo_RoleInfo]
ON [dbo].[ActionInfoRoleInfo]
    ([RoleInfo_ID]);
GO

-- Creating foreign key on [Department_ID] in table 'DepartmentUserInfo'
ALTER TABLE [dbo].[DepartmentUserInfo]
ADD CONSTRAINT [FK_DepartmentUserInfo_Department]
    FOREIGN KEY ([Department_ID])
    REFERENCES [dbo].[Department]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserInfo_ID] in table 'DepartmentUserInfo'
ALTER TABLE [dbo].[DepartmentUserInfo]
ADD CONSTRAINT [FK_DepartmentUserInfo_UserInfo]
    FOREIGN KEY ([UserInfo_ID])
    REFERENCES [dbo].[UserInfo]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentUserInfo_UserInfo'
CREATE INDEX [IX_FK_DepartmentUserInfo_UserInfo]
ON [dbo].[DepartmentUserInfo]
    ([UserInfo_ID]);
GO

-- Creating foreign key on [Department_ID] in table 'DepartmentActionInfo'
ALTER TABLE [dbo].[DepartmentActionInfo]
ADD CONSTRAINT [FK_DepartmentActionInfo_Department]
    FOREIGN KEY ([Department_ID])
    REFERENCES [dbo].[Department]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ActionInfo_ID] in table 'DepartmentActionInfo'
ALTER TABLE [dbo].[DepartmentActionInfo]
ADD CONSTRAINT [FK_DepartmentActionInfo_ActionInfo]
    FOREIGN KEY ([ActionInfo_ID])
    REFERENCES [dbo].[ActionInfo]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentActionInfo_ActionInfo'
CREATE INDEX [IX_FK_DepartmentActionInfo_ActionInfo]
ON [dbo].[DepartmentActionInfo]
    ([ActionInfo_ID]);
GO

-- Creating foreign key on [TopicId] in table 'Replys'
ALTER TABLE [dbo].[Replys]
ADD CONSTRAINT [FK_Replys_Topics]
    FOREIGN KEY ([TopicId])
    REFERENCES [dbo].[Topics]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Replys_Topics'
CREATE INDEX [IX_FK_Replys_Topics]
ON [dbo].[Replys]
    ([TopicId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------