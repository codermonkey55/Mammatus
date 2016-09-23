DECLARE @db_id int;
SET @db_id = DB_ID();

--- <summary>
--- list of recommended index rebuild / reorganize statements for the database
--- http://weblogs.asp.net/okloeten/archive/2009/01/05/6819737.aspx
--- </summary>
--- <returns>List of indexes to be refactored.</returns>
--- <remarks>
--- History:
---     07.15.2009 - Created by tdill
--- </remarks>

SELECT 'ALTER INDEX [' + ix.name + '] ON [' + t.name + '] ' +
      CASE WHEN ps.avg_fragmentation_in_percent > 40 THEN 'REBUILD' ELSE 'REORGANIZE' END +
      CASE WHEN pc.partition_count > 1 THEN ' PARTITION = ' + cast(ps.partition_number as nvarchar(max)) ELSE '' END
FROM   sys.indexes AS ix INNER JOIN sys.tables t
          ON t.object_id = ix.object_id
      INNER JOIN (SELECT object_id, index_id, avg_fragmentation_in_percent, partition_number
                  FROM sys.dm_db_index_physical_stats(@db_id, NULL, NULL, NULL, NULL)) ps
          ON t.object_id = ps.object_id AND ix.index_id = ps.index_id
      INNER JOIN (SELECT object_id, index_id, COUNT(DISTINCT partition_number) AS partition_count
                  FROM sys.partitions
                  GROUP BY object_id, index_id) pc
          ON t.object_id = pc.object_id AND ix.index_id = pc.index_id
WHERE  ps.avg_fragmentation_in_percent > 10