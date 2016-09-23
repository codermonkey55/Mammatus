select 'Grant Execute on ' + name +  ' SQLServerUSERName'
from sysobjects where xtype in ('P') 