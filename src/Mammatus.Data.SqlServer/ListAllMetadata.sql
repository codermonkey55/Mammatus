
--- <summary>
--- Returns full compliment of metadata
--- </summary>
--- <returns>lists of tables, columns, procedures, parameters</returns>
--- <remarks>
--- History:
---     8.18.2009 - Created by tdill
--- </remarks>
CREATE PROCEDURE [dbo].[ListAllMetadata]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    EXECUTE [ListAllTableNames] 
	EXECUTE [ListAllTableMetaData] 
	EXECUTE [ListAllColumnMetaData] 
	EXECUTE [ListTableRelationships] 
	EXECUTE [ListAllProcNames] 
	EXECUTE [ListAllProcParams] 
END
