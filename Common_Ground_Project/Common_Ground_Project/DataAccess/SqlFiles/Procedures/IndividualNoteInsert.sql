﻿USE MASTER
GO

CREATE PROCEDURE dbo.IndividualNoteInsert
(
	@IndividualID INT, 
	@Note VARCHAR(MAX), 
	@NewID INT OUTPUT
)
AS 
	SET NOCOUNT ON;

	INSERT INTO Master.dbo.IndividualNote
	(
		IndividualID, 
		Note, 
		DateTime
	)
	VALUES
	(
		@IndividualID, 
		@Note, 
		GETDATE()
	)

	SET @NewID = SCOPE_IDENTITY()

GO	