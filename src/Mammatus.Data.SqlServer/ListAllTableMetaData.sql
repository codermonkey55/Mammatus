--- <summary>
--- Returns table column names
--- </summary>
--- <returns>list of table names, column names, and column metadata in the database</returns>
--- <remarks>
--- History:
---     8.18.2009 - Created by tdill
--- </remarks>
CREATE PROCEDURE [dbo].[ListAllTableMetaData]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT  tab_les.TABLE_SCHEMA,
        tab_les.TABLE_NAME,
        STUFF(( SELECT  ',' + COLUMN_NAME
                FROM    INFORMATION_SCHEMA.COLUMNS AS cols
                WHERE   cols.TABLE_SCHEMA = t.TABLE_SCHEMA
                        AND cols.TABLE_NAME = t.TABLE_NAME
                ORDER BY cols.COLUMN_NAME
              FOR
                XML PATH('')
              ), 1, 1, '') AS Columns
FROM    INFORMATION_SCHEMA.TABLES AS tab_les
END
