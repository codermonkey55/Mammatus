namespace Mammatus.Delegates
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <param name="item1"></param>
    /// <param name="item2"></param>
    /// <returns></returns>
    public delegate bool Predicate<T1, T2>(T1 item1, T2 item2);
}
