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
        Context.Setting.EnableLogging = false;
        
        var msg = "Maximun count of IAstract reached";
        Context.AddConstraint(typeof(IAbstract), (ref object o) =>
            {
                var countAbstract = Pool<IAbstract>.List.Count;
                if (countAbstract >= 2)
                {
                    return false;
                }
                return true;
            }
            , msg);

        Context.OnResolved<IAbstract>().SubscribeToConsole("IAbstract");
        
        Context.OnResolved<ImplClass>().SubscribeToConsole("ImplCLass");
        
        Context.OnResolved<ImplClass2>().SubscribeToConsole("ImplCLass 2");

        Context.OnEventRaised<InvalidDataException>().SubscribeToConsole("Exception");
        
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

        var success = Context.RemoveConstraint(typeof(IAbstract), msg);
        Debug.Log("Remove constraint: "+success);
        
        Context.Resolve<IAbstract>();
        count = Pool<IAbstract>.List.Count;
        Debug.Log("C: "+count);

    }
}