UPDATE [dbo].[CourseContent]
SET
	[IsActive] = 0,
	[LastModifiedDateTime] = GETDATE()
WHERE [CourseId] = @CourseId;