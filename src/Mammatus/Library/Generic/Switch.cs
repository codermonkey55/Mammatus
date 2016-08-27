using System;

namespace Mammatus.Library.Generic
{
    public sealed class Switch<TSource>
    {
        private readonly TSource _sourceTypeInstance;
        private bool _matchingCaseFound;

        internal Switch(TSource source)
        {
            this._sourceTypeInstance = source;
            this._matchingCaseFound = false;
        }

        internal static Switch<TSource> Create(TSource source)
        {
            return new Switch<TSource>(source);
        }

        public Switch<TSource> Case<TTarget>(Action<TTarget> action)
        {
            if (_matchingCaseFound == false)
            {
                var sourceType = _sourceTypeInstance.GetType();

                var targetType = typeof(TTarget);

                if (targetType.IsAssignableFrom(sourceType))
                {
                    Action<Object> caseAction = (_) => action((TTarget)_);

                    try
                    {
                        caseAction.Invoke(_sourceTypeInstance);
                    }
                    catch (Exception)
                    {
                        _matchingCaseFound = false;
                    }

                    _matchingCaseFound = true;
                }
            }

            return this;
        }

        public void Default(Func<TSource, Object> func)
        {
            if (_matchingCaseFound == false) func(_sourceTypeInstance);
        }
    }
}