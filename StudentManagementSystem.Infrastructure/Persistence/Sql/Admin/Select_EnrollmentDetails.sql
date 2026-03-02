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
WHERE [StudentId] = @StudentId AND [CourseId] = @CourseId;



