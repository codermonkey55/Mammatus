using System;

namespace Mammatus.Library.Generic
{
    public class Some<T> : Option<T>
    {
        public Some(T val) : base(val) { }

        public override Option<T> IfSome(Action<T> action)
        {
            action(Val);

            return this;
        }
    }

}