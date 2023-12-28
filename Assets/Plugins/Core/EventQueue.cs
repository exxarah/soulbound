using System;
using System.Collections.Generic;

namespace Core
{
    /// <summary>
    /// The EventQueue mediates communication about the past between different IEventListeners.
    /// Eg, "player died" is an event, "kill player" is not
    /// </summary>
    public class EventQueue : Singleton<EventQueue>
    {
        private readonly Dictionary<Type, LinkedList<IEventListener>> m_listeners;

        private EventQueue()
        {
            m_listeners = new Dictionary<Type, LinkedList<IEventListener>>();
        }

#region IEventListener Management

        public void RegisterListener(Type eventType, IEventListener listener)
        {
            if (!m_listeners.ContainsKey(eventType))
            {
                m_listeners.Add(eventType, new LinkedList<IEventListener>());
                m_listeners[eventType].AddLast(listener);
            }
            else
            {
                m_listeners[eventType].AddLast(listener);
            }
        }

        public void RegisterListener<T>(IEventListener listener) where T : IEvent
        {
            RegisterListener(typeof(T), listener);
        }

        public void UnRegisterListener(Type eventType, IEventListener listener)
        {
            if (m_listeners.TryGetValue(eventType, out LinkedList<IEventListener> listeners))
            {
                listeners.Remove(listener);
            }
        }

        public void UnRegisterListener<T>(IEventListener listener) where T : IEvent
        {
            UnRegisterListener(typeof(T), listener);
        }

#endregion

        public void Publish<T>(T @event) where T : IEvent
        {
            Type type = @event.GetType();
            if (m_listeners.TryGetValue(type, out LinkedList<IEventListener> listeners))
            {
                // Push event to all registered listeners
                for (LinkedListNode<IEventListener> node = listeners.First; node != null; node = node.Next)
                {
                    node.Value.Receive(@event);
                }
            }
        }
    }

    public interface IEvent
    {
    }

    public interface IEventListener
    {
        void Receive<T>(T @event) where T : IEvent;
    }
}