UPDATE [dbo].[Instructor]
SET
	[Status] = @Status,
	[RejectedReason] = @RejectedReason,
	[ApprovedAt] = GETDATE(),
	[LastModifiedDateTime] = GETDATE()
WHERE [Id] = @InstructorId;