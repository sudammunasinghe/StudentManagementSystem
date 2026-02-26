INSERT INTO [dbo].[Instructor](
    [UserId],
    [ExperienceYears],
    [PreferredSalary]

) 
VALUES 
(
    @UserId,
    @ExperienceYears,
    @PreferredSalary
);
SELECT CAST(SCOPE_IDENTITY() AS INT);