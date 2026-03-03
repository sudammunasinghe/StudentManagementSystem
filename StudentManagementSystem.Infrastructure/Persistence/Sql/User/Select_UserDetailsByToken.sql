    SELECT
        [Id],
        [FirstName], 
	    [LastName], 
        [ContactNumber],
	    [Address], 
	    [NIC], 
	    [DateOfBirth],
	    [Gender],
	    [Email],
	    [PasswordHash],
        [RoleId],
        [PasswrodResetToken],
        [PasswrodResetTokenExpiry]
    FROM [dbo].[User]
    WHERE [IsActive] = 1 AND [PasswrodResetToken] = @Token;