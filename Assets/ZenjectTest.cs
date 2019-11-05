using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ZenjectTest : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<string>().FromInstance("Hello World!");
        Container.Bind<Greeter>().AsSingle().WhenInjectedInto(typeof(string));

//        var g = Container.Resolve<Greeter>();
    }
    
    public class Greeter
    {
        public Greeter(string message)
        {
            Debug.Log(message);
        }
    }
}
