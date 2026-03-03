UPDATE [dbo].[Course]
SET
	[IsActive] = 0,
	[LastModifiedDateTime] = GETDATE()
WHERE [Id] = @CourseId;