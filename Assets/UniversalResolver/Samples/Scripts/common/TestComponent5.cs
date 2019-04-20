using UnityEngine;

namespace SceneTest
{
    [ProcessingOrder(3)]
    public class TestComponent5 : MonoBehaviour
    {
        [Component] public IAbstract getFromGameObject;

        // Use this for initialization
        void Start()
        {
            @getFromGameObject.DoSomething();
        }
    }
}