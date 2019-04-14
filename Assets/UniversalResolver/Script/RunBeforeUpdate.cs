using UnityEngine;

namespace UnityIoC
{
    [IgnoreProcessing]
    public class RunBeforeUpdate : MonoBehaviour
    {
        private bool called;

        private void Update()
        {
            if (!called)
            {
                called = true;
                if (Context.Initialized)
                {
                    Context.GetDefaultInstance().ProcessInjectAttribute(gameObject);
                }
                SendMessage("RunBeforeUpdate", SendMessageOptions.RequireReceiver);
                Destroy(this);
            }
        }
    }
}