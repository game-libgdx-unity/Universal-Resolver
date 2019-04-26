using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityIoC
{
    public sealed class UIThreadInvoker : SingletonBehaviour<UIThreadInvoker>
    {
        private Queue<Action> _actions = new Queue<Action>();

        void FixedUpdate()
        {
            lock (_actions)
            {
                while (_actions.Count > 0)
                {
                    Action a = _actions.Dequeue();
                    a();
                }
            }
        }

        public void Invoke(Action action)
        {
            lock (_actions)
            {
                _actions.Enqueue(action);
            }
        }
    }
}