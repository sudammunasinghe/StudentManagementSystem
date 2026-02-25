UPDATE [dbo].[InstructorExperience]
    SET
        [IsActive] = 0,
        [LastModifiedDateTime] = GETDATE()
    WHERE [Id] IN @Ids;
                        