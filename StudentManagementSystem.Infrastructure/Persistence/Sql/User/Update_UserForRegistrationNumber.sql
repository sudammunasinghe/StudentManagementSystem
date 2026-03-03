UPDATE [dbo].[User]
SET
    [RegistrationNumber] = @RegistrationNumber,
    [LastModifiedDateTime] = GETDATE()
WHERE [Id] = @UserId;