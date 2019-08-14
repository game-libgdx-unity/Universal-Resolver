using System.Collections;
using System.Collections.Generic;
using System.IO;
using SceneTest;
using UnityEngine;
using UnityIoC;

public class TestUnbind : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        Context.AddConstraint(typeof(IAbstract), (ref object o) =>
            {
                var countAbstract = Pool<IAbstract>.List.Count;
                if (countAbstract >= 2)
                {
                    return false;
                }
                return true;
            }
            , "Maximun count of IAstract reached");

        Context.OnExceptionRaised<InvalidDataException>().SubscribeToConsole("Exception");
        
        Context.Bind<IAbstract, ImplClass>();

        Context.Resolve<IAbstract>().DoSomething();

        Context.Unbind<IAbstract>();

        Context.Bind<IAbstract, ImplClass2>();

        Context.Resolve<IAbstract>().DoSomething();
        
        var count = Pool<IAbstract>.List.Count;
        Debug.Log("C: "+count);
        
        Context.Resolve<IAbstract>();
        
        count = Pool<IAbstract>.List.Count;
        Debug.Log("C: "+count);
    }
}