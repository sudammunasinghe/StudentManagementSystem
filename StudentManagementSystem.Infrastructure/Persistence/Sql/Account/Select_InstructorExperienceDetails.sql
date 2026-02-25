SELECT
 IE.[Id],
 IE.[InstructorId],
 IE.[CompanyName],
 IE.[JobTitle],
 IE.[EmployementType],
 IE.[Location],
 IE.[StartDate],
 IE.[EndDate],
 IE.[IsCurrentlyWorking],
 IE.[Description],
 IE.[IsActive],
 IE.[CreatedDateTime],
 IE.[LastModifiedDateTime]
FROM [dbo].[User] US
	INNER JOIN [dbo].[Instructor] INS ON US.[Id] = INS.[UserId] AND INS.[IsActive] = 1
	INNER JOIN [dbo].[InstructorExperience] IE ON INS.[Id] = IE.[InstructorId] AND IE.[IsActive] = 1
WHERE US.[Id] = @UserId;