--- <summary>
--- Returns procedure names
--- </summary>
--- <returns>list of procedure names in the database</returns>
--- <remarks>
--- History:
---     8.18.2009 - Created by tdill
--- </remarks>
CREATE PROCEDURE [dbo].[ListAllProcNames]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT obj.Name as SPName
	FROM sys.sql_modules modu 
		INNER JOIN sys.objects obj 
		ON modu.object_id = obj.object_id 
	WHERE obj.type = 'P'
END
