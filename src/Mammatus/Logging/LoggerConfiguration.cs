namespace Mammatus.Logging
{
    public class LoggerConfiguration
    {
        public LoggerConfiguration()
        {
            Enabled = true;
            EnabledDebug = true;
            EnabledError = true;
            EnabledException = true;
            EnabledInfo = true;
        }

        public bool Enabled { get; set; }

        public bool EnabledDebug { get; set; }

        public bool EnabledError { get; set; }

        public bool EnabledException { get; set; }

        public bool EnabledInfo { get; set; }
    }
}
