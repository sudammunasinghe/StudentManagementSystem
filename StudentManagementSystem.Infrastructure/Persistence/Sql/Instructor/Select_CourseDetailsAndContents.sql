SELECT
	[Id],
	[InstructorId],
	[Title],
	[Description],
	[CategoryEnum],
	[Credits],
	[DurationHours],
	[EntrollmentLimit]
FROM [dbo].[Course]
WHERE [IsActive] = 1 AND [InstructorId] = @InstructorId;

SELECT
	[Id],
	[CourseId],
	[InstructorId],
	[Title],
	[Description],
	[ContentType],
	[FileUrl],
	[FileSize]
FROM [dbo].[CourseContent]
WHERE [IsActive] = 1 AND [InstructorId] = @InstructorId;