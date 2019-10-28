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
        Context.AddConstraint<IAbstract>(o =>
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
        var count = Context.GetObjects<IAbstract>().Count;
        Debug.Log("C: " + count); //1
        deleteOne.DoSomething();

        Context.Bind<IAbstract, ImplClass2>();

        var cantDelete = Context.Resolve<IAbstract>();
        cantDelete.DoSomething();

        count = Context.GetObjects<IAbstract>().Count;
        Debug.Log("C: " + count); //2

        Context.Resolve<IAbstract>();

        count = Context.GetObjects<IAbstract>().Count;
        Debug.Log("C: " + count); //2
        
        Context.Resolve<IAbstract>(); //count should be 3

        count = Context.GetObjects<IAbstract>().Count;
        Debug.Log("C: " + count); //true count is still 2
        
        var success = Context.RemoveConstraint<IAbstract>(When.BeforeResolve);
        Debug.Log("Remove constraint: " + success);
        Context.Resolve<IAbstract>(); //now count is true 3
        
        //Test for new constraint
        var minMsg = "Minimum count of IAstract reached";
        Context.AddConstraint<IAbstract>(o =>
            {
                var countAbstract = Context.GetObjects<IAbstract>().Count;
                if (countAbstract <= 2)
                {
                    return false;
                }

                return true;
            }
            , minMsg, When.BeforeDelete);

        count = Context.GetObjects<IAbstract>().Count;
        Debug.Log("C: " + count); //3

        Context.Delete(deleteOne);
        count = Context.GetObjects<IAbstract>().Count;
        Debug.Log("C: " + count); //2

        Context.Delete(cantDelete);
        count = Context.GetObjects<IAbstract>().Count;
        Debug.Log("C: " + count); //2

        success = Context.RemoveConstraint<IAbstract>(When.BeforeDelete);
        Debug.Log("Remove constraint: " + success);
        
        Context.Delete(cantDelete);
        count = Context.GetObjects<IAbstract>().Count;
        Debug.Log("C: " + count); //1    
    }
}