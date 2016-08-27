namespace Mammatus.Library.Generic
{
    public static class TypeSwitch
    {
        public static Switch<TSource> On<TSource>(TSource value)
        {
            return Switch<TSource>.Create(value);
        }
    }
}
