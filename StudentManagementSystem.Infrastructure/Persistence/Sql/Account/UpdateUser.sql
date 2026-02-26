UPDATE [dbo].[User]
SET
	[FirstName] = @FirstName,
	[LastName] = @LastName,
	[Address] = @Address,
	[ContactNumber] = @ContactNumber,
	[Email] = @Email,
	[NIC] = @NIC,
	[LastModifiedDateTime] = GETDATE()
WHERE [Id] = @Id;