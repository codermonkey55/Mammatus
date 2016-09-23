﻿--- <summary>
--- function to split a comma delimited list
--- </summary>
--- <param name="@String">The string to split.</param>
--- <param name="@Delimiter">The delimiter on which to split.</param>
--- <returns>Table containing split values.</returns>
--- <remarks>
--- History:
---     07.16.2009 - Created by tdill
---		http://www.logiclabz.com/sql-server/split-function-in-sql-server-to-break-comma-separated-strings-into-table.aspx
--- </remarks>
CREATE FUNCTION dbo.Split(@String varchar(8000), @Delimiter char(1))        
returns @temptable TABLE (items varchar(8000))        
as        
begin        
    declare @idx int        
    declare @slice varchar(8000)        
       
    select @idx = 1        
        if len(@String)<1 or @String is null  return        
       
    while @idx!= 0        
    begin        
        set @idx = charindex(@Delimiter,@String)        
        if @idx!=0        
            set @slice = left(@String,@idx - 1)        
        else        
            set @slice = @String        
           
        if(len(@slice)>0)   
            insert into @temptable(Items) values(@slice)        
  
        set @String = right(@String,len(@String) - @idx)        
        if len(@String) = 0 break        
    end    
return        
end
