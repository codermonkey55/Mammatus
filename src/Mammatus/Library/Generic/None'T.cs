using System;

namespace Mammatus.Library.Generic
{
    public class None<T> : Option<T>
    {
        public None(T val) : base(val) { }

        public override Option<T> IfNone(Action<T> action)
        {
            action(Val);

            return this;
        }
    }

}