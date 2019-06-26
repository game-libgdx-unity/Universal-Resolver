using System.Collections;
using System.Collections.Generic;
using Unity.Linq;
using UnityEngine;
using UnityIoC;

#if UNITY_EDITOR
//using UnityEditor;
//using UTJ;
//
//[CustomEditor(typeof(ContextProcess))]
//public class ContextProcessInspector : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        //hide the AutoLoadSetting if customBindingSetting is null
//        ContextProcess cb = (ContextProcess) serializedObject.targetObject;
//
//        EditorGUI.BeginChangeCheck();
//
//        List<string> propertyToExclude = new List<string>();
//
//
//        if (!cb.ProcessOnChildren)
//        {
//            propertyToExclude.Add(nameof(cb.ProcessOnDescends));
//        }
//
//        if (!cb.ProcessOnGameObject)
//        {
//            propertyToExclude.Add(nameof(cb.ProcessOnChildren));
//        }
//
//        if (propertyToExclude.Count > 0)
//        {
//            DrawPropertiesExcluding(serializedObject, propertyToExclude.ToArray());
//        }
//        else
//        {
//            DrawDefaultInspector();
//        }
//
//        if (EditorGUI.EndChangeCheck())
//        {
//            serializedObject.ApplyModifiedProperties();
//        }
//    }
//}
#endif


/// <summary>
/// Process the context on this behaviour
/// </summary>
[IgnoreProcessing]
public class ContextProcess : MonoBehaviour
{
    [SerializeField] public bool ProcessOnGameObject = true;
    [SerializeField] public bool ProcessOnChildren;
    [SerializeField] public bool ProcessOnDescendants;

    // Start is called before the first frame update
    void Awake()
    {
        if (Context.Initialized)
        {
            if (ProcessOnGameObject)
            {
                Context.DefaultInstance.ProcessInjectAttribute(gameObject);
            }

            if (ProcessOnChildren)
            {
                foreach (Transform tf in transform)
                {
                    Context.DefaultInstance.ProcessInjectAttribute(tf.gameObject);
                }
            }

            if (ProcessOnDescendants)
            {
                foreach (Transform tf in transform)
                {
                    foreach (var child in tf.gameObject.Descendants())
                    {
                        Context.DefaultInstance.ProcessInjectAttribute(child);
                    }
                }
            }
        }
    }
}