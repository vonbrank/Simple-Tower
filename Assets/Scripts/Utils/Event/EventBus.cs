using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Utils.Event
{
    public class EventBus<T> where T : IEvent
    {
        private static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();

        public static void Register(EventBinding<T> binding) => bindings.Add(binding);
        public static void Deregister(EventBinding<T> binding) => bindings.Add(binding);

        public static void Raise(T @event)
        {
            foreach (var binding in bindings)
            {
                binding.OnEvent?.Invoke(@event);
                binding.OnEventNoArgs?.Invoke();
            }
        }
    }
}