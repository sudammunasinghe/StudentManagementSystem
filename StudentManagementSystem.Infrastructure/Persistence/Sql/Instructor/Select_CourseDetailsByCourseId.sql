SELECT
    [Id],
    [Title],
    [Credits],
    [InstructorId]
FROM [dbo].[Course]
WHERE [IsActive] = 1 AND [Id] = @CourseId;