	SELECT
        [Id],
        [RegistrationNumber],
        [FirstName], 
	    [LastName], 
        [ContactNumber],
	    [Address], 
	    [NIC], 
	    [DateOfBirth],
	    [Gender],
	    [Email],
	    [PasswordHash],
        [RoleId]
    FROM [dbo].[User]
    WHERE [IsActive] = 1 AND [Email] = @Email;