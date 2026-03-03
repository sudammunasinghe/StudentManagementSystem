SELECT
	US.[RegistrationNumber],
	'Enrollment Approval' [ApprovalType],
	ENR.[StudentId],
	ENR.[CourseId] [EnrolledCourseId],
	NULL [InstructorId],
	US.[FirstName],
	US.[LastName],
	US.[RoleId],
	VW_APS.[Description] [ApprovalStatus],
	ENR.[RejectedReason],
	ENR.[RequestedAt] [RequestedDateTime],
	ENR.[ApprovedAt] [ApprovedDateTime]
FROM [dbo].[Enrollment] ENR
	INNER JOIN [dbo].[Student] STD ON ENR.[StudentId] = STD.[Id] AND STD.[IsActive] = 1
	INNER JOIN [dbo].[User] US ON STD.[UserId] = US.[Id] AND US.[IsActive] = 1
	INNER JOIN [dbo].[VW_EnrollmentApprovalStatus] VW_APS ON ENR.[Status] = VW_APS.[Id]
WHERE ENR.[IsActive] = 1 AND (ENR.[Status] = @StatusCode OR @StatusCode IS NULL)

UNION

SELECT
	US.[RegistrationNumber],
	'Instructor Registration Approval' [ApprovalType],
	NULL [StudentId],
	NULL [EnrolledCourseId],
	INS.[Id] [InstructorId],
	US.[FirstName],
	US.[LastName],
	US.[RoleId],
	VW_APS.[Description] [ApprovalStatus],
	INS.[RejectedReason],
	INS.[CreatedDateTime] [RequestedDateTime],
	INS.[ApprovedAt] [ApprovedDateTime]
FROM [dbo].[Instructor] INS
	INNER JOIN [dbo].[User] US ON INS.[UserId] = US.[Id] AND US.[IsActive] = 1
	INNER JOIN [dbo].[VW_EnrollmentApprovalStatus] VW_APS ON INS.[Status] = VW_APS.[Id]
WHERE INS.[IsActive] = 1 AND (INS.[Status] = @StatusCode OR @StatusCode IS NULL)