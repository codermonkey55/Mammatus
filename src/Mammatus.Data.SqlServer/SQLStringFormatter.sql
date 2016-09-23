--- <summary>
--- function to format a string
--- </summary>
--- <param name="@Expression">The expression to format.</param>
--- <param name="@Format">The format to give it.</param>
--- <returns>string formatted </returns>
--- <remarks>
--- History:
---     08.10.2009 - Created by tdill
--- http://www.vbforums.com/showthread.php?t=402188
--- </remarks>
CREATE FUNCTION FormatString (@Expression varchar(8000),@Format varchar(8000))  
RETURNS varchar(8000) AS  
BEGIN 
declare @ret varchar(8000), @temp_expr varchar(8000)

if @Expression is null or @Format is null
	begin
		set @ret=NULL
	end
else
	begin
	if len(@Expression)>=len(@Format)
		begin
			set @ret=@Expression
		end
	else
		begin
			set @ret=substring(@Format,1,len(@Format)-len(@Expression))+@Expression
		end
	end

return @ret

end 
