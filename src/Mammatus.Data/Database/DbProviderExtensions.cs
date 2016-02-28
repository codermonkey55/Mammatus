using System.Data.Common;

namespace Mammatus.Data.Database
{
    public static class DbProviderExtensions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static void ExecuteNonQuery(this DbProviderFactory provider, string connectionString, string command)
        {
            using (DbConnection conn = provider.CreateConnection())
            {
                conn.ConnectionString = connectionString;
                conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = command;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public static object ExecuteScalar(this DbProviderFactory provider, string connectionString, string command)
        {
            using (DbConnection conn = provider.CreateConnection())
            {
                conn.ConnectionString = connectionString;
                conn.Open();
                using (DbCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = command;
                    object ret = cmd.ExecuteScalar();
                    return ret;
                }
            }
        }
    }
}