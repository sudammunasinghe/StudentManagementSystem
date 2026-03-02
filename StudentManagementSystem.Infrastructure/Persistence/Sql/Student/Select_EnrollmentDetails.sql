SELECT 
	[StudentId],
    [CourseId],
    [Status],
    [RequestedAt],
    [ApprovedAt],
    [RejectedReason],
    [IsActive],
    [LastModifiedDateTime]
FROM [dbo].[Enrollment]
WHERE [IsActive] = 1 AND
	[StudentId] = @StudentId AND
	[CourseId] = @CourseId;