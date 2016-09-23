/****** Object:  UserDefinedFunction [dbo].[GetWorkingDays]    Script Date: 05/26/2010 23:46:57 ******/
/*http://classicasp.aspfaq.com/date-time-routines-manipulation/how-do-i-count-the-number-of-business-days-between-two-dates.html*/

CREATE FUNCTION [dbo].[FindWorkingDays]  
(  
    @startDate SMALLDATETIME,  
    @endDate SMALLDATETIME  
) 
RETURNS INT  
AS  
BEGIN 
    DECLARE @range INT; 
 
    SET @range = DATEDIFF(DAY, @startDate, @endDate)+1; 
 
    RETURN  
    ( 
        SELECT  
            @range / 7 * 5 + @range % 7 -  
            ( 
                SELECT COUNT(*)  
            FROM 
                ( 
                    SELECT 1 AS d 
                    UNION ALL SELECT 2  
                    UNION ALL SELECT 3  
                    UNION ALL SELECT 4  
                    UNION ALL SELECT 5  
                    UNION ALL SELECT 6  
                    UNION ALL SELECT 7 
                ) weekdays 
                WHERE d <= @range % 7  
                AND DATENAME(WEEKDAY, @endDate - d + 1)  
                IN 
                ( 
                    'Saturday', 
                    'Sunday' 
                )	 
            ) 
    ); 
END  
 