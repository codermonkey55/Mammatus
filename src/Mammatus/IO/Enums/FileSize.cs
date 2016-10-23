namespace Mammatus.IO.Enums
{
    public enum FileSize : long
    {
        Scale = 1024,
        KiloByte = 1 * Scale,
        MegaByte = KiloByte * Scale,
        GigaByte = MegaByte * Scale,
        TeraByte = GigaByte * Scale
    }
}
