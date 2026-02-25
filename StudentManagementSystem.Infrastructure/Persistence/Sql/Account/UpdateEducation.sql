UPDATE [dbo].[Education]
SET
    [IsActive] = 0,
    [LastModifiedDateTime] = GETDATE()
WHERE [Id] IN @Ids;