namespace Mammatus.Core.State
{
    public interface IState
    {
        T Get<T>();

        T Get<T>(object key);

        T Get<T>(string key);

        bool TryGet<T>(object key, out T value);

        void Put<T>(T instance);

        void Put<T>(object key, T instance);

        void Store(string key, object value);

        void Remove<T>();

        void Remove<T>(object key);

        void Clear();

    }
}
