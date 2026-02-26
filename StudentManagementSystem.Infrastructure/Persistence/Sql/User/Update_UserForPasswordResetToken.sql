UPDATE [dbo].[User]
    SET
        [PasswrodResetToken] = @Token,
        [PasswrodResetTokenExpiry] = @Expiry,
        [LastModifiedDateTime] = GETDATE()
WHERE [Id] = @UserId;