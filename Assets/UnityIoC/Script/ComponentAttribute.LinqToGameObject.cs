
#if USE_LINQ_TO_GAMEOBJECT

using System;
using Object = UnityEngine.Object;
using Unity.Linq;
using UnityEngine;


public partial class ComponentAttribute
{
    
    public class SiblingAttribute : InjectAttribute, IInjectComponent
    {
        public Component GetComponent(MonoBehaviour behaviour, Type type)
        {
            var parent = behaviour.gameObject.Parent();
            var componentInChildren = parent.GetComponentInChildren(type);
            if (componentInChildren != null)
            {
                return componentInChildren;
            }

            Debug.LogFormat("Can't find type of {0}, create a new one on gameObject {1}", type, behaviour.gameObject.name);
            return behaviour.gameObject.AddComponent(type);
        }
    }

    public class AncestorsAttribute : InjectAttribute, IInjectComponent
    {
        public Component GetComponent(MonoBehaviour behaviour, Type type)
        {
            var ancestors = behaviour.gameObject.Ancestors();

            //search on ancestors for needed component
            foreach (var gameObject in ancestors)
            {
                var component = gameObject.GetComponent(type);
                if (component != null)
                {
                    return component;
                }
            }

            Debug.LogFormat("Can't find type of {0}, create a new one on gameObject {1}", type, behaviour.gameObject.name);
            return behaviour.gameObject.AddComponent(type);
        }
    }

    public class DescendantsAttribute : InjectAttribute, IInjectComponent
    {
        public Component GetComponent(MonoBehaviour behaviour, Type type)
        {
            var descendants = behaviour.gameObject.Descendants();

            //search on descendants for needed component
            foreach (var gameObject in descendants)
            {
                var component = gameObject.GetComponent(type);
                if (component != null)
                {
                    return component;
                }
            }

            Debug.LogFormat("Can't find type of {0}, create a new one on gameObject {1}", type, behaviour.gameObject.name);
            return behaviour.gameObject.AddComponent(type);
        }
    }

}

#endif
