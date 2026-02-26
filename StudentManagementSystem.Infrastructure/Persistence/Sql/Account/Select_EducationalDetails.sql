SELECT
    EDC.[Id],
    EDC.[StudentId],
    EDC.[Institute],
    EDC.[Degree],
    EDC.[Major],
    EDC.[StartingDate],
    EDC.[EndingDate],
    EDC.[IsStudying],
    EDC.[Description],
    EDC.[IsActive],
    EDC.[CreatedDateTime],
    EDC.[LastModifiedDateTime]
FROM [dbo].[User] US
   	INNER JOIN [dbo].[Student] STD ON US.[Id] = STD.[UserId] AND STD.[IsActive] = 1
   	INNER JOIN [dbo].[Education] EDC ON STD.[Id] = EDC.[StudentId] AND EDC.[IsActive] = 1
WHERE US.[Id] = @UserId;