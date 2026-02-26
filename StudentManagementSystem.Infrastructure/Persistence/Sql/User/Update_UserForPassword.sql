UPDATE [dbo].[User]
    SET
        [PasswordHash] = @NewPasswordHash,
        [PasswrodResetToken] = NULL,
        [PasswrodResetTokenExpiry] = NULL,
        [LastModifiedDateTime] = GETDATE()
WHERE [Id] = @UserId