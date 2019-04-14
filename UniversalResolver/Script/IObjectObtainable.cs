using System;

namespace UnityIoC
{
    public interface IObjectObtainable
    {
        object TryObtain(Type type);
    }
}