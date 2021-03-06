﻿USE MASTER
GO

CREATE PROCEDURE dbo.EmergencyContactUpdate
(
	@EmergencyContactID INT,
	@IndividualID INT, 
	@Name VARCHAR(50), 
	@Phone VARCHAR(50), 
	@Email VARCHAR(50), 
	@NewID INT OUTPUT
)
AS 
	SET NOCOUNT ON;

	UPDATE Master.dbo.EmergencyContact
	SET 
		IndividualID = @IndividualID, 
		Name = @Name, 
		Phone = @Phone, 
		Email = @Email
	WHERE
		EmergencyContactID = @EmergencyContactID

GO	