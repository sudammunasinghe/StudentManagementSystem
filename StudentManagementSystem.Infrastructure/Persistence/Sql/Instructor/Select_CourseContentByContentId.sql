SELECT 
	  [Id],
      [CourseId],
      [InstructorId],
      [Title],
      [Description],
      [ContentType],
      [FileUrl],
      [FileSize],
      [IsActive],
      [CreatedDateTime],
      [LastModifiedDateTime]
FROM [dbo].[CourseContent]
WHERE [IsActive] = 1 AND [Id] = @ContentId;