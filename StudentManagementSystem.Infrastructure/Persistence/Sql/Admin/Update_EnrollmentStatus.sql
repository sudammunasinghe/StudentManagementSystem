UPDATE [dbo].[Enrollment]
SET 
	[Status] = @Status,
	[RejectedReason] = @RejectedReason,
	[ApprovedAt] = GETDATE(),
	[LastModifiedDateTime] = GETDATE()
WHERE [StudentId] = @StudentId  AND [CourseId] = @CourseId;