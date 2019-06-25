using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityIoC;


/// <summary>
/// Process the context on this behaviour
/// </summary>
[IgnoreProcessing]
public class ContextProcess : MonoBehaviour
{
    [SerializeField] private bool ProcessOnChildren;
    [SerializeField] private bool ProcessOnDescends;
    // Start is called before the first frame update
    void Awake()
    {
        if (Context.Initialized)
        {
            Context.DefaultInstance.ProcessInjectAttribute(gameObject);
        }
    }
}
