using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnityIoC
{
    [Flags]
    public enum When
    {
        Resolve = 1,
        Update = 2,
        Delete = 3,
        All = Resolve | Update | Delete
    }

    public class ValidState
    {
        public delegate bool Predicator(ref object obj);

        public Predicator predicator;
        public string message;
        public When action;
    }
}