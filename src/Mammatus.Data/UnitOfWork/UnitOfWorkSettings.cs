using System.Transactions;

namespace Mammatus.Data.UnitOfWork
{
    public static class UnitOfWorkSettings
    {
        public static IsolationLevel DefaultIsolation { get; set; }

        public static bool AutoCompleteScope { get; set; }

        static UnitOfWorkSettings()
        {
            AutoCompleteScope = true;

            DefaultIsolation = IsolationLevel.ReadCommitted;
        }
    }
}