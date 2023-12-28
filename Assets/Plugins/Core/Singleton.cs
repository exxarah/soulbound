using System;
using System.Reflection;

namespace Core
{

    public abstract class Singleton<T> where T : Singleton<T>
    {
        private static T s_instance;
        private static object s_initLock = new object();

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                    CreateInstance();

                return s_instance;
            }
        }

        private static void CreateInstance()
        {
            lock (s_initLock)
            {
                if (s_instance == null)
                {
                    Type t = typeof(T);

                    // Ensure there are no public constructors...
                    ConstructorInfo[] ctors = t.GetConstructors();
                    if (ctors.Length > 0)
                    {
                        throw new InvalidOperationException(
                                                            $"{t.Name} has at least one accessible ctor making it" +
                                                            "impossible to enforce singleton behaviour");
                    }

                    // Create an instance via the private constructor
                    s_instance = (T)Activator.CreateInstance(t, true);
                }
            }
        }
    }
}