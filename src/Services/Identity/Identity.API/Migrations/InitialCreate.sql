IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Images] (
    [ImageId] int NOT NULL IDENTITY,
    [MediaType] nvarchar(max) NOT NULL,
    [Data] varbinary(max) NOT NULL,
    CONSTRAINT [PK_Images] PRIMARY KEY ([ImageId])
);
GO

CREATE TABLE [Users] (
    [UserId] int NOT NULL IDENTITY,
    [Email] nvarchar(max) NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [AboutMe] nvarchar(max) NOT NULL,
    [Gender] int NOT NULL,
    [DateOfBirth] datetime2 NOT NULL,
    [Phonenumber] nvarchar(max) NOT NULL,
    [Location] nvarchar(max) NOT NULL,
    [ImageId] int NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId]),
    CONSTRAINT [FK_Users_Images_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [Images] ([ImageId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Experience] (
    [ExperienceId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Title] nvarchar(max) NOT NULL,
    [Started] datetime2 NOT NULL,
    [Ended] datetime2 NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Experience] PRIMARY KEY ([ExperienceId]),
    CONSTRAINT [FK_Experience_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);
GO

CREATE TABLE [InterestedTopics] (
    [InterestedTopicId] int NOT NULL IDENTITY,
    [TopicId] int NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_InterestedTopics] PRIMARY KEY ([InterestedTopicId]),
    CONSTRAINT [FK_InterestedTopics_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([UserId]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Experience_UserId] ON [Experience] ([UserId]);
GO

CREATE INDEX [IX_InterestedTopics_UserId] ON [InterestedTopics] ([UserId]);
GO

CREATE INDEX [IX_Users_ImageId] ON [Users] ([ImageId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221202113108_InitialCreate', N'7.0.0');
GO

COMMIT;
GO