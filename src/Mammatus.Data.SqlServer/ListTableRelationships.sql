--- <summary>
--- Returns table relationships
--- </summary>
--- <returns>list of table relationships in the database</returns>
--- <remarks>
--- History:
---     8.18.2009 - Created by tdill
--- </remarks>
CREATE PROCEDURE [dbo].[ListTableRelationships]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT  const_col_use.TABLE_SCHEMA + '.' + const_col_use.TABLE_NAME + '.' + 
    const_col_use.COLUMN_NAME AS ForeignKeyColumn,
        const_col_use2.TABLE_SCHEMA + '.' + const_col_use2.TABLE_NAME + '.' + 
    const_col_use2.COLUMN_NAME AS PrimaryKeyColumn
		FROM    INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS ref_const
		JOIN    INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS const_col_use
				ON ref_const.CONSTRAINT_CATALOG = const_col_use.CONSTRAINT_CATALOG
				AND ref_const.CONSTRAINT_SCHEMA = const_col_use.CONSTRAINT_SCHEMA
				AND ref_const.CONSTRAINT_NAME = const_col_use.CONSTRAINT_NAME
		JOIN    INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS const_col_use2
				ON ref_const.UNIQUE_CONSTRAINT_CATALOG = const_col_use2.CONSTRAINT_CATALOG
				AND ref_const.UNIQUE_CONSTRAINT_SCHEMA = const_col_use2.CONSTRAINT_SCHEMA
				AND ref_const.UNIQUE_CONSTRAINT_NAME = const_col_use2.CONSTRAINT_NAME
END
