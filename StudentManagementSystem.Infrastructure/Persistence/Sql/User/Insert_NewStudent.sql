INSERT INTO [dbo].[Student](
    [UserId],
    [GPA]
) 
VALUES
(
    @UserId,
    @GPA
); 
SELECT CAST(SCOPE_IDENTITY() AS INT);