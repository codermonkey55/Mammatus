
namespace Mammatus.Data.Database
{
    public interface IDatabaseManager
    {
        void CreateDatabase();

        bool DatabaseExists();

        void DeleteDatabase();

        void ValidateDatabaseSchema();
    }
}