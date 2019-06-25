using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App
{
    public class EventData
    {
        public int EventId;
        public EventType EventType;
    }

    public enum EventType
    {
        Default,
    }
}
