-- =============================================
-- Script Template
-- =============================================

declare @handle uniqueidentifier

declare conv cursor for select conversation_handle from sys.conversation_endpoints where far_service = '[myInitiatorService]' or far_service = '[myTargetService]'

open conv

fetch NEXT FROM conv into @handle

while @@FETCH_STATUS = 0

Begin

END Conversation @handle with cleanup

fetch NEXT FROM conv into @handle

End

close conv

deallocate conv
