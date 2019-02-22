using System;
using UnityEditor;
using UnityEngine;

namespace UnityIoC.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(BindingSetting))]
    public class ImplementClassInspector : BaseInspector
    {
        private void OnDisable()
        {
            //validate the input
            for (var i = 0; i < ((BindingSetting) target).defaultSettings.Count; i++)
            {
                var setting = ((BindingSetting) target).defaultSettings[i];
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
                    ((BindingSetting) target).defaultSettings.RemoveAt(i--);
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
                    ((BindingSetting) target).defaultSettings.RemoveAt(i--);
                }    
            }
        }

        private void OnEnable()
        {
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            //default binding setting
            DrawList("Default Setting", "", ((BindingSetting) target).defaultSettings,
                data =>
                {
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    EditorGUILayout.BeginHorizontal();

                    var maxWidth = 30.0f;
                    var objFieldMaxWidth = 60.0f;
                    var maxButtonWidth = 40.0f;

                    DrawLabel("From", GUILayout.MaxWidth(35));

                    data.ImplementedTypeHolder = EditorGUILayout.ObjectField("", data.ImplementedTypeHolder,
                        typeof(MonoScript),
                        false,
                        GUILayout.MaxWidth(objFieldMaxWidth),
                        GUILayout.ExpandWidth(false));

                    if (data.ImplementedTypeHolder)
                    {
                        data.ImplementedType = ((MonoScript) data.ImplementedTypeHolder).GetClass();
                        serializedObject.ApplyModifiedProperties();
                    }

                    DrawLabel("Bind", GUILayout.MaxWidth(maxWidth));

                    data.AbstractTypeHolder = EditorGUILayout.ObjectField("",
                        data.AbstractTypeHolder, typeof(MonoScript), false,
                        GUILayout.MaxWidth(objFieldMaxWidth),
                        GUILayout.ExpandWidth(false));

                    if (data.AbstractTypeHolder)
                    {
                        data.AbstractType = ((MonoScript) data.AbstractTypeHolder).GetClass();
                    }

                    DrawLabel(" As ", GUILayout.MaxWidth(20f));

                    data.LifeCycle = DrawEnumPopup("", ref data.LifeCycle,
                        GUILayout.MaxWidth(objFieldMaxWidth),
                        GUILayout.ExpandWidth(false));


                    DrawButton("X", () => { ((BindingSetting) target).defaultSettings.Remove(data); },
                        GUILayout.MaxWidth(17),
                        GUILayout.ExpandWidth(false));

                    DrawButton("C", () => { ((BindingSetting) target).defaultSettings.Add(data.Clone()); },
                        GUILayout.MaxWidth(20),
                        GUILayout.ExpandWidth(false));

                    DrawButtonToggle("", ref data.EnableInjectInto, GUILayout.MaxWidth(10));
                    DrawLabel("Inject", GUILayout.MaxWidth(50f));

                    EditorGUILayout.EndHorizontal();

                    if (data.EnableInjectInto)
                    {
                        EditorGUILayout.BeginVertical("Box");
                        
                        DrawList("Inject into list", "Component", data.InjectIntoHolder, obj =>
                        {
                            EditorGUILayout.BeginHorizontal();

                            obj = EditorGUILayout.ObjectField("Component",
                                obj, typeof(MonoScript), false);


                            DrawButton("X", () =>
                                {
                                    if (data.InjectIntoHolder.Count == 1)
                                    {
                                        data.InjectIntoHolder.Clear();
                                        return;
                                    }
                                    
                                    data.InjectIntoHolder.Remove(obj);
                                },
                                GUILayout.MaxWidth(17),
                                GUILayout.ExpandWidth(false));

                            EditorGUILayout.EndHorizontal();

                            return obj;
                        }, true);

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

        void OnLostFocus()
        {
            Debug.Log("Choose go back");
        }
    }
}