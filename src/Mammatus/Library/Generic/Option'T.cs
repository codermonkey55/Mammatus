using System;

namespace Mammatus.Library.Generic
{
    public abstract class Option<T>
    {
        public T Val { get; private set; }

        protected Option(T val)
        {
            Val = val;
        }

        public virtual Option<T> IfSome(Action<T> action)
        {
            return this;
        }

        public Option<T> IfSome(Action action)
        {
            return IfSome(x => action());
        }

        public virtual Option<T> IfNone(Action<T> action)
        {
            return this;
        }

        public Option<T> IfNone(Action action)
        {
            return IfNone(x => action());
        }

        public static Option<T> Create(T val)
        {
            if (val == null)
            {
                return new None<T>(val);
            }

            return new Some<T>(val);
        }
    }

}
