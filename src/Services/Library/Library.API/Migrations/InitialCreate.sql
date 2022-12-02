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

CREATE TABLE [Categories] (
    [CategoryId] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([CategoryId])
);
GO

CREATE TABLE [Images] (
    [Id] int NOT NULL IDENTITY,
    [MediaType] nvarchar(max) NOT NULL,
    [Data] varbinary(max) NOT NULL,
    CONSTRAINT [PK_Images] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Subcategories] (
    [SubcategoryId] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [CategoryId] int NOT NULL,
    CONSTRAINT [PK_Subcategories] PRIMARY KEY ([SubcategoryId]),
    CONSTRAINT [FK_Subcategories_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([CategoryId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Topics] (
    [TopicId] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [SubcategoryId] int NOT NULL,
    CONSTRAINT [PK_Topics] PRIMARY KEY ([TopicId]),
    CONSTRAINT [FK_Topics_Subcategories_SubcategoryId] FOREIGN KEY ([SubcategoryId]) REFERENCES [Subcategories] ([SubcategoryId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Courses] (
    [CourseId] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [About] nvarchar(max) NOT NULL,
    [LastUpdated] datetime2 NOT NULL,
    [Tier] int NOT NULL,
    [IsApproved] bit NOT NULL,
    [TopicId] int NOT NULL,
    [PublisherUserId] int NOT NULL,
    [ImageId] int NOT NULL,
    CONSTRAINT [PK_Courses] PRIMARY KEY ([CourseId]),
    CONSTRAINT [FK_Courses_Images_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [Images] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Courses_Topics_TopicId] FOREIGN KEY ([TopicId]) REFERENCES [Topics] ([TopicId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Lessons] (
    [LessonId] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [CourseId] int NOT NULL,
    CONSTRAINT [PK_Lessons] PRIMARY KEY ([LessonId]),
    CONSTRAINT [FK_Lessons_Courses_CourseId] FOREIGN KEY ([CourseId]) REFERENCES [Courses] ([CourseId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Units] (
    [UnitId] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [OrderIndex] int NOT NULL,
    [LessonId] int NOT NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [TimeLimit] time NULL,
    [Content] nvarchar(max) NULL,
    [ApproximateTime] time NULL,
    CONSTRAINT [PK_Units] PRIMARY KEY ([UnitId]),
    CONSTRAINT [FK_Units_Lessons_LessonId] FOREIGN KEY ([LessonId]) REFERENCES [Lessons] ([LessonId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Questions] (
    [QuestionId] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [Point] int NOT NULL,
    [ExamId] int NOT NULL,
    CONSTRAINT [PK_Questions] PRIMARY KEY ([QuestionId]),
    CONSTRAINT [FK_Questions_Units_ExamId] FOREIGN KEY ([ExamId]) REFERENCES [Units] ([UnitId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Choices] (
    [ChoiceId] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [IsCorrect] bit NOT NULL,
    [QuestionId] int NOT NULL,
    CONSTRAINT [PK_Choices] PRIMARY KEY ([ChoiceId]),
    CONSTRAINT [FK_Choices_Questions_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [Questions] ([QuestionId]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Choices_QuestionId] ON [Choices] ([QuestionId]);
GO

CREATE INDEX [IX_Courses_ImageId] ON [Courses] ([ImageId]);
GO

CREATE INDEX [IX_Courses_TopicId] ON [Courses] ([TopicId]);
GO

CREATE INDEX [IX_Lessons_CourseId] ON [Lessons] ([CourseId]);
GO

CREATE INDEX [IX_Questions_ExamId] ON [Questions] ([ExamId]);
GO

CREATE INDEX [IX_Subcategories_CategoryId] ON [Subcategories] ([CategoryId]);
GO

CREATE INDEX [IX_Topics_SubcategoryId] ON [Topics] ([SubcategoryId]);
GO

CREATE INDEX [IX_Units_LessonId] ON [Units] ([LessonId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221202114323_InitialCreate', N'7.0.0');
GO

COMMIT;
GO
