SELECT
	CS.[Id],
	CS.[InstructorId],
	CS.[Title],
	CS.[Description],
	CS.[CategoryEnum],
	CS.[Credits],
	CS.[DurationHours],
	CS.[EntrollmentLimit]
FROM [dbo].[User] US
	INNER JOIN [dbo].[Student] STD ON US.[Id] = STD.[UserId] AND STD.[IsActive] = 1
	INNER JOIN [dbo].[Enrollment] ENR ON STD.[Id] = ENR.[StudentId] AND ENR.[IsActive] = 1
	INNER JOIN [dbo].[Course] CS ON ENR.[CourseId] = CS.[Id] AND CS.[IsActive] = 1
WHERE US.[IsActive] = 1 AND US.[Id] = @UserId AND
	ENR.[Status] IN(2,3,5);

SELECT
	CC.[Id],
	CC.[CourseId],
	CC.[InstructorId],
	CC.[Title],
	CC.[Description],
	CC.[ContentType],
	CC.[FileUrl],
	CC.[FileSize]
FROM [dbo].[User] US
	INNER JOIN [dbo].[Student] STD ON US.[Id] = STD.[UserId] AND STD.[IsActive] = 1
	INNER JOIN [dbo].[Enrollment] ENR ON STD.[Id] = ENR.[StudentId] AND ENR.[IsActive] = 1
	INNER JOIN [dbo].[Course] CS ON ENR.[CourseId] = CS.[Id] AND CS.[IsActive] = 1
	INNER JOIN [dbo].[CourseContent] CC ON CS.[Id] = CC.[CourseId] AND CC.[IsActive] = 1
WHERE US.[IsActive] = 1 AND US.[Id] = @UserId AND
	ENR.[Status] IN(2,3,5);