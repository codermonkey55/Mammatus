--- <summary>
--- Returns procedure params
--- </summary>
--- <returns>list of procedure names and params in the database</returns>
--- <remarks>
--- History:
---     8.18.2009 - Created by tdill
--- </remarks>
CREATE PROCEDURE [dbo].[ListAllProcParams]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT SCHEMA_NAME(SCHEMA_ID) AS [Schema], 
		sys_objects.name AS [ObjectName],
		sys_objects.Type_Desc AS [ObjectType (UDF/SP)],
		para_meter.parameter_id AS [ParameterID],
		para_meter.name AS [ParameterName],
		TYPE_NAME(para_meter.user_type_id) AS [ParameterDataType],
		para_meter.max_length AS [ParameterMaxBytes],
		para_meter.is_output AS [IsOutPutParameter]
		FROM sys.objects AS sys_objects
		INNER JOIN sys.parameters AS para_meter 
		ON sys_objects.OBJECT_ID = para_meter.OBJECT_ID
		WHERE sys_objects.OBJECT_ID IN ( SELECT OBJECT_ID 
		FROM sys.objects
		WHERE TYPE IN ('P','FN'))
		ORDER BY [Schema], sys_objects.name, para_meter.parameter_id
END
