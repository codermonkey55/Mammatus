CREATE PROCEDURE [dbo].[DeleteAllProcedures]

AS
	declare @procName varchar(500)
	declare cur cursor 
		for select [name] from sys.objects where type = 'p'
	open cur
	fetch next from cur into @procName
	while @@fetch_status = 0
	begin
		if @procName <> 'DeleteAllProcedures'
			  exec('drop procedure ' + @procName)
			  fetch next from cur into @procName
	end
	close cur
	deallocate cur

RETURN 0
