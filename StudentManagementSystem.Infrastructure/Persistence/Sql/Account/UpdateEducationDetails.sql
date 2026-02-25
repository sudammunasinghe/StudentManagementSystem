UPDATE [dbo].[Education]
SET
	[Institute] = @Institute,
    [Degree] = @Degree,
    [Major] = @Major,
    [StartingDate] = @StartingDate,
    [EndingDate] = @EndingDate,
    [IsStudying] = @IsStudying,
    [Description] = @Description,
	[LastModifiedDateTime] = GETDATE()
WHERE [Id] = @Id;