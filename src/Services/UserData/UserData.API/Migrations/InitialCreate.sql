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

CREATE TABLE [EnrolledCourses] (
    [EnrolledCourseId] int NOT NULL IDENTITY,
    [CompletedDate] datetime2 NULL,
    [CourseId] int NOT NULL,
    [StudierUserId] int NOT NULL,
    CONSTRAINT [PK_EnrolledCourses] PRIMARY KEY ([EnrolledCourseId])
);
GO

CREATE TABLE [FollowedCourses] (
    [FollowedCourseId] int NOT NULL IDENTITY,
    [CourseId] int NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_FollowedCourses] PRIMARY KEY ([FollowedCourseId])
);
GO

CREATE TABLE [FollowedTopics] (
    [FollowedTopicId] int NOT NULL IDENTITY,
    [TopicId] int NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_FollowedTopics] PRIMARY KEY ([FollowedTopicId])
);
GO

CREATE TABLE [Ratings] (
    [RatingId] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [Stars] int NOT NULL,
    [Created] datetime2 NOT NULL,
    [CourseId] int NOT NULL,
    [UserId] int NOT NULL,
    CONSTRAINT [PK_Ratings] PRIMARY KEY ([RatingId])
);
GO

CREATE TABLE [Submissions] (
    [SubmissionId] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [LessonName] nvarchar(max) NOT NULL,
    [CourseName] nvarchar(max) NOT NULL,
    [Created] datetime2 NOT NULL,
    [Graded] datetime2 NULL,
    [Elapsed] time NOT NULL,
    [TimeLimit] time NOT NULL,
    [OwnerUserId] int NOT NULL,
    CONSTRAINT [PK_Submissions] PRIMARY KEY ([SubmissionId])
);
GO

CREATE TABLE [CompletedUnits] (
    [CompletedUnitId] int NOT NULL IDENTITY,
    [UnitId] int NOT NULL,
    [EnrolledCourseId] int NOT NULL,
    CONSTRAINT [PK_CompletedUnits] PRIMARY KEY ([CompletedUnitId]),
    CONSTRAINT [FK_CompletedUnits_EnrolledCourses_EnrolledCourseId] FOREIGN KEY ([EnrolledCourseId]) REFERENCES [EnrolledCourses] ([EnrolledCourseId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Comments] (
    [CommentId] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [OwnerUserId] int NOT NULL,
    [SubmissionId] int NOT NULL,
    CONSTRAINT [PK_Comments] PRIMARY KEY ([CommentId]),
    CONSTRAINT [FK_Comments_Submissions_SubmissionId] FOREIGN KEY ([SubmissionId]) REFERENCES [Submissions] ([SubmissionId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Criteria] (
    [CriterionId] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [SubmissionId] int NOT NULL,
    CONSTRAINT [PK_Criteria] PRIMARY KEY ([CriterionId]),
    CONSTRAINT [FK_Criteria_Submissions_SubmissionId] FOREIGN KEY ([SubmissionId]) REFERENCES [Submissions] ([SubmissionId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Questions] (
    [QuestionId] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [Point] int NOT NULL,
    [SubmissionId] int NOT NULL,
    CONSTRAINT [PK_Questions] PRIMARY KEY ([QuestionId]),
    CONSTRAINT [FK_Questions_Submissions_SubmissionId] FOREIGN KEY ([SubmissionId]) REFERENCES [Submissions] ([SubmissionId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Checkpoints] (
    [CheckpointId] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [Point] int NOT NULL,
    [IsChecked] bit NOT NULL,
    [CriterionId] int NOT NULL,
    CONSTRAINT [PK_Checkpoints] PRIMARY KEY ([CheckpointId]),
    CONSTRAINT [FK_Checkpoints_Criteria_CriterionId] FOREIGN KEY ([CriterionId]) REFERENCES [Criteria] ([CriterionId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Answers] (
    [AnswerId] int NOT NULL IDENTITY,
    [Content] nvarchar(max) NOT NULL,
    [IsCorrect] bit NULL,
    [IsChosen] bit NULL,
    [QuestionId] int NOT NULL,
    CONSTRAINT [PK_Answers] PRIMARY KEY ([AnswerId]),
    CONSTRAINT [FK_Answers_Questions_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [Questions] ([QuestionId]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_Answers_QuestionId] ON [Answers] ([QuestionId]);
GO

CREATE INDEX [IX_Checkpoints_CriterionId] ON [Checkpoints] ([CriterionId]);
GO

CREATE INDEX [IX_Comments_SubmissionId] ON [Comments] ([SubmissionId]);
GO

CREATE INDEX [IX_CompletedUnits_EnrolledCourseId] ON [CompletedUnits] ([EnrolledCourseId]);
GO

CREATE INDEX [IX_Criteria_SubmissionId] ON [Criteria] ([SubmissionId]);
GO

CREATE INDEX [IX_Questions_SubmissionId] ON [Questions] ([SubmissionId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20221202114658_InitialCreate', N'7.0.0');
GO

COMMIT;
GO
