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

        var maxMsg = "Maximun count of IAstract reached";
        Context.AddConstraint(typeof(IAbstract), (ref object o) =>
            {
                var countIAbstract = Context.GetObjects<IAbstract>().Count;
                if (countIAbstract >= 2)
                {
                    return false;
                }

                return true;
            }
            , maxMsg, When.BeforeResolve);

        Context.OnResolved<IAbstract>().SubscribeToConsole("IAbstract");

        Context.OnResolved<ImplClass>().SubscribeToConsole("ImplCLass");

        Context.OnResolved<ImplClass2>().SubscribeToConsole("ImplCLass 2");

        Context.OnEventRaised<InvalidDataException>().SubscribeToConsole("Exception");

        Context.Bind<IAbstract, ImplClass>();

        var deleteOne = Context.Resolve<IAbstract>();
        deleteOne.DoSomething();

        Context.Unbind<IAbstract>();

        Context.Bind<IAbstract, ImplClass2>();

        var cantDelete = Context.Resolve<IAbstract>();
        cantDelete.DoSomething();

        var count = Context.GetObjects<IAbstract>().Count;
        Debug.Log("C: " + count); //2

        Context.Resolve<IAbstract>();

        count = Context.GetObjects<IAbstract>().Count;
        Debug.Log("C: " + count); //2

        var success = Context.RemoveConstraint(typeof(IAbstract), maxMsg);
        Debug.Log("Remove constraint: " + success);

        var minMsg = "Minimum count of IAstract reached";
        Context.AddConstraint(typeof(IAbstract), (ref object o) =>
            {
                var countAbstract = Context.GetObjects<IAbstract>().Count;
                if (countAbstract <= 2)
                {
                    return false;
                }

                return true;
            }
            , minMsg, When.BeforeDelete);

        var newOne = Context.Resolve<IAbstract>();
        Debug.Log(newOne == null);

        count = Context.GetObjects<IAbstract>().Count;
        Debug.Log("C: " + count); //3

        Context.Delete(deleteOne);
        count = Context.GetObjects<IAbstract>().Count;
        Debug.Log("C: " + count);

        Context.Delete(cantDelete);
        count = Context.GetObjects<IAbstract>().Count;
        Debug.Log("C: " + count);
    }
}