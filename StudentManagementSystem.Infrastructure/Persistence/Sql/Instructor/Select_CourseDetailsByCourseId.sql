

SELECT 
	[Id],
    [InstructorId],
    [Title],
    [Description],
    [CategoryEnum],
    [Credits],
    [DurationHours],
    [EntrollmentLimit],
    [IsActive],
    [CreatedDateTime],
    [LastModifiedDateTime]
FROM [dbo].[Course]
WHERE [IsActive] = 1 AND [Id] = @CourseId;



