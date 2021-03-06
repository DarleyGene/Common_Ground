﻿USE MASTER
GO

CREATE PROCEDURE dbo.ActivityGetByID
(
	@ActivityID INT
)
AS 
	SET NOCOUNT ON;

	SELECT
		a.ActivityID, 
		a.ActivityTypeID,
		a.TripLeaderID, 
		a.Location, 
		a.StartDate, 
		a.EndDate, 
		a.PickUpTime, 
		a.DropOffTime, 
		a.StaffArrivalTime, 
		a.Cost, 

		at.Name [ActivityTypeName], 
		at.[Description] [ActivityTypeDescription]
	FROM
		Master.dbo.Activity a
		JOIN Master.dbo.ActivityType at ON at.ActivityTypeID = a.ActivityTypeID
	WHERE
		ActivityID = @ActivityID

GO	