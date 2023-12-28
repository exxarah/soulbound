using System;
using System.Collections.Generic;

namespace Core
{

    /// <summary>
    /// The CommandRouter stores commands from anywhere, and sends them to the correct ICommandDispatcher, who will then deliver
    /// them to the correct entity. Basically this is the mail processing center, ICommandDispatchers are the postmen for specific
    /// areas (move to, attack, etc), and ICommands are the letters
    ///
    /// While this looks very similar to EventQueue, they're distinct in the way they're intended to respond to and process
    /// their messages. CommandRouter refers to future events/actions, and should be processed near immediately.
    /// </summary>
    public class CommandRouter : Singleton<CommandRouter>
    {
        private readonly Dictionary<Type, LinkedList<ICommandDispatcher>> m_dispatchers;

        private CommandRouter()
        {
            m_dispatchers = new Dictionary<Type, LinkedList<ICommandDispatcher>>();
        }

#region ICommandDispatcher Management

        public void RegisterDispatcher<T>(ICommandDispatcher dispatcher) where T : ICommand
        {
            Type type = typeof(T);
            if (!m_dispatchers.ContainsKey(type))
            {
                m_dispatchers.Add(type, new LinkedList<ICommandDispatcher>());
                m_dispatchers[type].AddLast(dispatcher);
            }
            else
            {
                m_dispatchers[type].AddLast(dispatcher);
            }
        }

        public void UnRegisterDispatcher<T>(ICommandDispatcher dispatcher) where T : ICommand
        {
            if (m_dispatchers.TryGetValue(typeof(T), out LinkedList<ICommandDispatcher> dispatchers))
            {
                dispatchers.Remove(dispatcher);
            }
        }

#endregion

        public void Send<T>(T command) where T : ICommand
        {
            Type type = typeof(T);
            Send(command, type);
        }

        public void Send(ICommand command, Type type)
        {
            if (m_dispatchers.TryGetValue(type, out LinkedList<ICommandDispatcher> dispatchers))
            {
                // Push command to all registered dispatchers
                for (LinkedListNode<ICommandDispatcher> node = dispatchers.First; node != null; node = node.Next)
                {
                    node.Value.Receive(command);
                }
            }
        }
    }

    public interface ICommand
    {
    }

    public interface ICommandDispatcher
    {
        void Receive<T>(T command) where T : ICommand;
    }
}