using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets
{
    class Dispatcher:IDispatcher
    {
        public List<Action> pendingActions = new List<Action>();
        private static Dispatcher instance;

        // Singleton
        private Dispatcher()
        {

        }

        public static Dispatcher getInstance()
        {
            if (instance == null)
            {
                instance = new Dispatcher();
            }

            return instance;

        }

        public void invoke(Action function)
        {
            lock (pendingActions)
            {
                pendingActions.Add(function);
            }
        }

        public void invokePending()
        {
            lock (pendingActions)
            {
                foreach (var action in pendingActions)
                {
                    action();
                }

                pendingActions.Clear();
            }
        }

        
    }
}
