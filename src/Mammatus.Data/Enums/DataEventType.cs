namespace Mammatus.Data.Enums
{
    public enum DataEventType
    {
        ColoumnChanged, // a spacific column value has changed
        RowDeleted, // a specific row has been "logically" deleted - physically still on disk
        RowChanged, // part or all of a specific row has changed
        RowAdded, // a new row has been added to a specific table
        StoredProc, // a specific stored procedure was called
        Access, // executed a query of specific data
        Filter // a filter object/expression has different data output
    }

}