namespace Mammatus.Library.Constants
{
    public class RegularExpressions
    {
        public const string Number = "^[0-9]+$";
        public const string NumberSign = "^[+-]?[0-9]+$";
        public const string Decimal = "^[0-9]+[.]?[0-9]+$";
        public const string DecimalSign = "^[+-]?[0-9]+[.]?[0-9]+$";
        public const string EmailRestricted = "^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$";
        public const string Email = "(['\"]{1,}.+['\"]{1,}\\s+)?<?[\\w\\.\\-]+@[^\\.][\\w\\.\\-]+\\.[a-z]{2,}>?";
        public const string Chzn = "[\u4e00-\u9fa5]";

    }
}
