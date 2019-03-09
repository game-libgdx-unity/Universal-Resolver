using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityIoC.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(InjectIntoBindingSetting))]
    public class InjectIntoBindingSettingInspector : BaseInspector
    {
        private void OnDisable()
        {
            //validate the input
            for (var i = 0; i < ((InjectIntoBindingSetting) target).defaultSettings.Count; i++)
            {
                var setting = ((InjectIntoBindingSetting) target).defaultSettings[i];
                if (setting.AbstractTypeHolder == null)
                {
                    if (EditorUtility.DisplayDialog("Failed validation", "Bind fields must not be null", "Go back",
                        "Delete them"))
                    {
                        //go back to editor to correct it
                        Undo.PerformUndo();
                        return;
                    }

                    //delete it
                    ((InjectIntoBindingSetting) target).defaultSettings.RemoveAt(i--);
                    continue;
                }

                if (setting.ImplementedTypeHolder == null)
                {
                    if (EditorUtility.DisplayDialog("Failed validation", "From fields must not be null", "Go back",
                        "Delete them"))
                    {
                        //go back to editor to correct it
                        Undo.PerformUndo();
                        return;
                    }

                    //delete it
                    ((InjectIntoBindingSetting) target).defaultSettings.RemoveAt(i--);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            //default binding setting
            DrawList("Default Setting", "", ((InjectIntoBindingSetting) target).defaultSettings,
                data =>
                {
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    EditorGUILayout.BeginHorizontal();

                    var maxWidth = 30.0f;
                    var maxButtonWidth = 40.0f;

                    DrawLabel("From", GUILayout.MaxWidth(35));

                    data.ImplementedTypeHolder = EditorGUILayout.ObjectField("", data.ImplementedTypeHolder,
                        typeof(MonoScript),
                        false,
                        GUILayout.MaxWidth(80),
                        GUILayout.ExpandWidth(true));

                    if (data.ImplementedTypeHolder)
                    {
                        data.ImplementedType = ((MonoScript) data.ImplementedTypeHolder).GetClass();
                        serializedObject.ApplyModifiedProperties();
                    }

                    DrawLabel("Bind", GUILayout.MaxWidth(maxWidth));

                    data.AbstractTypeHolder = EditorGUILayout.ObjectField("",
                        data.AbstractTypeHolder, typeof(MonoScript), false,
                        GUILayout.MaxWidth(60),
                        GUILayout.ExpandWidth(true));

                    if (data.AbstractTypeHolder)
                    {
                        data.AbstractType = ((MonoScript) data.AbstractTypeHolder).GetClass();
                    }

                    DrawLabel(" As ", GUILayout.MaxWidth(20f));

                    data.LifeCycle = DrawEnumPopup("", ref data.LifeCycle,
                        GUILayout.MaxWidth(60),
                        GUILayout.ExpandWidth(false));


                    DrawButton("X", () => { ((InjectIntoBindingSetting) target).defaultSettings.Remove(data); },
                        GUILayout.MaxWidth(17),
                        GUILayout.ExpandWidth(false));

                    DrawButton("C", () => { ((InjectIntoBindingSetting) target).defaultSettings.Add(data.JsonClone()); },
                        GUILayout.MaxWidth(20),
                        GUILayout.ExpandWidth(false));

                    DrawButtonToggle("", ref data.EnableInjectInto, GUILayout.MaxWidth(10));
                    DrawLabel("Inject", GUILayout.MaxWidth(50f));

                    EditorGUILayout.EndHorizontal();
                    
//                    For non-array injectInto inspector
//                    if (data.EnableInjectInto)
//                    {
//                        EditorGUILayout.BeginHorizontal("Box");
//                        
//                        DrawLabel("When Inject into", GUILayout.MaxWidth(150));
//
//                        data.InjectIntoHolder = EditorGUILayout.ObjectField("",
//                            data.InjectIntoHolder, typeof(MonoScript), false,
//                            GUILayout.MaxWidth(60),
//                            GUILayout.ExpandWidth(true));
//
//                        if (data.InjectIntoHolder)
//                        {
//                            data.InjectInto = ((MonoScript) data.InjectIntoHolder).GetClass(); 
//                        }
//
//                        EditorGUILayout.EndHorizontal();
//                    }

                    if (data.EnableInjectInto)
                    {
                        EditorGUILayout.BeginVertical("Box");

                        //auto add 1 element if the array data is empty 
                        if (data.InjectIntoHolder.Count == 0)
                        {
                            data.InjectIntoHolder.Add(null);
                        }

                        DrawList("Inject into list", "Component",
                            data.InjectIntoHolder,
                            obj =>
                            {
                                EditorGUILayout.BeginHorizontal();

                                obj = EditorGUILayout.ObjectField("Component",
                                    obj, typeof(MonoScript), false);


                                DrawButton("X", () =>
                                    {
                                        data.InjectIntoHolder.Remove(obj);
                                        //update enable injectInto setting
                                        if (data.InjectIntoHolder.Count == 0)
                                        {
                                            data.EnableInjectInto = false;
                                        }
                                    },
                                    GUILayout.MaxWidth(17),
                                    GUILayout.ExpandWidth(false));

                                EditorGUILayout.EndHorizontal();

                                return obj;
                            }, false);

                        EditorGUILayout.EndVertical();
                    }

                    return data;
                }
                , false
            );

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }

            //make sure styles are default after finishing your custom inspector
            Revert();
        }
    }
}