SELECT 
	[Id],
    [UserId],
    [GPA],
    [IsActive],
    [CreatedDateTime],
    [LastModifiedDateTime]
FROM [dbo].[Student]
WHERE [IsActive] = 1 AND [UserId] = @UserId;