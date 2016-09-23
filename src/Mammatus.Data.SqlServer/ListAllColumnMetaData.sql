--- <summary>
--- Returns table column names
--- </summary>
--- <returns>list of table names and column names in the database</returns>
--- <remarks>
--- History:
---     8.18.2009 - Created by tdill
--- </remarks>
CREATE PROCEDURE [dbo].[ListAllColumnMetaData]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT OBJECT_SCHEMA_NAME(tab_les.[object_id],DB_ID()) AS [Schema],   
        tab_les.[name] AS [table_name], cols.[name] AS [column_name],   
        ty_pes.[name] AS system_data_type, cols.[max_length],  
        cols.[precision], cols.[scale], cols.[is_nullable], cols.[is_ansi_padded]  
		FROM sys.[tables] AS tab_les   
		  INNER JOIN sys.[all_columns] cols ON tab_les.[object_id] = cols.[object_id]  
		 INNER JOIN sys.[types] ty_pes ON cols.[system_type_id] = ty_pes.[system_type_id] AND cols.[user_type_id] = ty_pes.[user_type_id]   
		WHERE tab_les.[is_ms_shipped] = 0  
		ORDER BY tab_les.[name], cols.[column_id]
END
