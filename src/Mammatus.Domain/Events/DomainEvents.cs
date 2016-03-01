using System.Collections.Concurrent;
using System.Collections.Generic;
using Mammatus.Core.IoC;
using Mammatus.Core.State;
using Mammatus.IoC;
using Mammatus.IoC.Containers;

namespace Mammatus.Domain
{
    using System;


    public static class DomainEvents
    {
        private const string DomainEventsKey = "Mammatus.Domain.Events";

        [ThreadStatic]
        private static List<Delegate> actions;

        private static ConcurrentQueue<Action> Events
        {
            get
            {
                IInternalContainer containerAdapter = InternalContainer.Current;

                IState currentContextState = containerAdapter.GetInstance<IState>();

                if (currentContextState.Get<ConcurrentQueue<Action>>(DomainEvents.DomainEventsKey) == null)
                {
                    currentContextState.Store(DomainEvents.DomainEventsKey, new ConcurrentQueue<Action>());
                }

                return currentContextState.Get<ConcurrentQueue<Action>>(DomainEvents.DomainEventsKey);
            }
        }

        public static void ClearCallbacks()
        {
            DomainEvents.actions = null;
        }

        public static void Dispatch<T>(T @event)
        where T : class
        {
            IInternalContainer containerAdapter = InternalContainer.Current;

            DomainEvents.Events.Enqueue(() =>
                {
                    foreach (var consumer in containerAdapter.GetAllInstances<IConsumeEvent<T>>())
                    {
                        consumer.Consume(@event);
                    }

                    if (DomainEvents.actions != null)
                    {
                        foreach (Delegate action in DomainEvents.actions)
                        {
                            Action<T> typedAction = action as Action<T>;
                            if (typedAction != null)
                            {
                                typedAction(@event);
                            }
                        }
                    }
                });
        }

        public static void Register<T>(Action<T> callback)
        where T : class
        {
            if (DomainEvents.actions == null)
            {
                DomainEvents.actions = new List<Delegate>();
            }

            DomainEvents.actions.Add(callback);
        }

        public static void Raise()
        {
            Action dispatch;
            while (DomainEvents.Events.TryDequeue(out dispatch))
            {
                dispatch();
            }
        }
    }
}