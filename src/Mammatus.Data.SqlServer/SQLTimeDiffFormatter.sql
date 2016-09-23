--- <summary>
--- function to compute the difference between two datestimes and return it as 00:00:00
--- </summary>
--- <param name="@startDate">The beginning time.</param>
--- <param name="@endDate">The ending time.</param>
--- <returns>string formatted as 00:00:00</returns>
--- <remarks>
--- Requires fn_Format
--- History:
---     07.16.2009 - Created by tdill
--- </remarks>
CREATE  FUNCTION [dbo].[FormatTimeDiff] (@startDate DateTime, @endDate DateTime )
RETURNS varchar(20)
AS
BEGIN
	declare @formattedDiff varchar(20)
	
	BEGIN
		set @formattedDiff = 
			dbo.fn_Format(DATEDIFF("s",@startDate, @endDate)/3600, '00') 
			+ ':' 
			+ dbo.fn_Format((DATEDIFF("s",@startDate, @endDate)/60) - ((DATEDIFF("s",@startDate, @endDate)/3600) * 60), '00')  
			+ ':' 
			+ dbo.fn_Format(DATEDIFF("s",@startDate, @endDate) - ((DATEDIFF("s",@startDate, @endDate)/60) * 60),'00')
		
	END

	RETURN @formattedDiff

END
