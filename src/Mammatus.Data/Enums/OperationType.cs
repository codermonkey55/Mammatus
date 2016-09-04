namespace Mammatus.Data.Enums
{

    /// <summary>
    /// Categorise data operations into operations that
    /// inspect/read (QUERY) the data and operations that
    /// change/write (UPDATE) the data.
    /// </summary>
    public enum OperationType
    {
        Query, // query the data store for specific data (query)
        Update // change the data store contents (update/delete/insert)
    }

}