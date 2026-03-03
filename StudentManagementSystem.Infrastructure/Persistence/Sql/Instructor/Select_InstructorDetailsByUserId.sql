SELECT 
       [Id]
      ,[UserId]
      ,[ExperienceYears]
      ,[PreferredSalary]
      ,[Status]
      ,[IsActive]
      ,[CreatedDateTime]
      ,[LastModifiedDateTime]
  FROM [dbo].[Instructor]
  WHERE [UserId] = @UserId;