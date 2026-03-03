UPDATE [dbo].[CourseContent]
SET
	[IsActive] = 0,
	[LastModifiedDateTime] = GETDATE()
WHERE [Id] = @ContentId;