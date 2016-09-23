--- <summary>
--- Returns table names
--- </summary>
--- <returns>list of table names in the database</returns>
--- <remarks>
--- History:
---     8.18.2009 - Created by tdill
--- </remarks>
CREATE PROCEDURE [dbo].[ListAllTableNames]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   SELECT '['+SCHEMA_NAME(schema_id)+'].['+name+']'
	AS SchemaTable
	FROM sys.tables
END
