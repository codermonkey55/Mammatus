declare @TableName varchar(50)
set @TableName  = 'MyTableName'

SET NOCOUNT ON 
DECLARE @name varchar(50)
DECLARE @sql varchar(2000)
DECLARE @DBName varchar(50)

SELECT @sql = ''
SELECT @DBName = ''

CREATE Table #temp
(
DBName varchar(50),
TableName varchar(50)
)

DECLARE DB_Cursor SCROLL CURSOR FOR 

SELECT [Name]
FROM Master.dbo.sysdatabases
WHERE [name] NOT IN ('master', 'model', 'msdb', 'tempdb')

OPEN DB_Cursor
FETCH FIRST FROM DB_Cursor
INTO @Name

WHILE ( @@Fetch_Status = 0)
BEGIN
SELECT @DBName = @Name

SELECT @sql =N' INSERT INTO #temp '
SELECT @sql = @sql + ' SELECT '''+@DBName+''', [Name] FROM '+@DBName+'.dbo.sysobjects WHERE type = ''u'' '

BEGIN TRY
    EXECUTE (@sql)
END TRY
BEGIN CATCH
    SELECT 
        ERROR_NUMBER() AS ErrorNumber
        ,ERROR_MESSAGE() AS ErrorMessage;
END CATCH


FETCH NEXT FROM DB_Cursor
INTO @Name
END

CLOSE DB_Cursor
DEALLOCATE DB_Cursor

IF @TableName IS NOT NULL
SELECT * FROM #temp WHERE TableName = @TableName

IF @TableName IS NULL
SELECT * FROM #temp ORDER BY DBNAME, TableName

drop table #temp
