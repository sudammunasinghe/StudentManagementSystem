    INSERT  INTO [dbo].[User]
    (
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
    )
    VALUES(
        @FirstName,
        @LastName,
        @ContactNumber,
        @Address,
        @NIC,
        @DateOfBirth,
        @Gender,
    	@Email,
        @PasswordHash,
        @RoleId
    );
    SELECT CAST(SCOPE_IDENTITY() AS INT);