-- http://snippets.dzone.com/posts/show/2035
-- http://databases.aspfaq.com/database/how-do-i-find-a-stored-procedure-containing-text.html

declare @column varchar(200)

set @column = 'MyObjectName'

SELECT name FROM sysobjects WHERE id IN ( SELECT id FROM syscolumns WHERE name = @column )

SELECT Name 
    FROM sys.procedures 
    WHERE OBJECT_DEFINITION(object_id) like '%' + @column + '%'