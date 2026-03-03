UPDATE [dbo].[InstructorExperience]
SET
	[CompanyName] = @CompanyName,
	[JobTitle] = @JobTitle,
	[EmployementType] = @EmployementType,
	[Location] = @Location,
	[StartDate] = @StartDate,
	[EndDate] = @EndDate,
	[IsCurrentlyWorking] = @IsCurrentlyWorking,
	[Description] = @Description,
    [LastModifiedDateTime] = GETDATE()
WHERE [Id] = @Id;