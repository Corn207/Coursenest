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

CREATE TABLE [Credentials] (
    [CredentialId] int NOT NULL IDENTITY,
    [Username] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Credentials] PRIMARY KEY ([CredentialId])
);
GO

CREATE TABLE [Roles] (
    [RoleId] int NOT NULL IDENTITY,
    [Type] int NOT NULL,
    [Expiry] datetime2 NOT NULL,
    [CredentialId] int NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([RoleId]),
    CONSTRAINT [FK_Roles_Credentials_CredentialId] FOREIGN KEY ([CredentialId]) REFERENCES [Credentials] ([CredentialId])
);
GO

CREATE INDEX [IX_Roles_CredentialId] ON [Roles] ([CredentialId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221202104533_InitialCreate', N'7.0.0');
GO

COMMIT;
GO
