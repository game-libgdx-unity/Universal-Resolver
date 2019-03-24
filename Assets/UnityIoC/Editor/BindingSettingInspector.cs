using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityIoC.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(BindingSetting))]
    public class BindingSettingInspector : BaseInspector
    {
        private void OnDisable()
        {
            //validate the input
            for (var i = 0; i < ((BindingSetting) target).defaultSettings.Length; i++)
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

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            //default binding setting
            var defaultSettingList = ((BindingSetting) target).defaultSettings.ToList();
            ((BindingSetting) target).defaultSettings = DrawList("Default Setting", "", defaultSettingList,
                data =>
                {
                    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                    EditorGUILayout.BeginHorizontal();

                    var maxWidth = 30.0f;
                    var maxButtonWidth = 40.0f;

                    DrawLabel("From", GUILayout.MaxWidth(35));

                    data.ImplementedTypeHolder = EditorGUILayout.ObjectField("", data.ImplementedTypeHolder,
                        typeof(Object),
                        true,
                        GUILayout.MaxWidth(80),
                        GUILayout.ExpandWidth(true));

                    if (data.ImplementedTypeHolder is MonoScript)
                    {
                        data.ImplementedType = ((MonoScript) data.ImplementedTypeHolder).GetClass();
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


                    DrawButton("X", () => { defaultSettingList.Remove(data); },
                        GUILayout.MaxWidth(17),
                        GUILayout.ExpandWidth(false));

                    DrawButton("C", () => { defaultSettingList.Add(data.Clone()); },
                        GUILayout.MaxWidth(20),
                        GUILayout.ExpandWidth(false));

                    EditorGUILayout.EndHorizontal();

                    return data;
                }
                , false
            ).ToArray();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }

            //make sure styles are default after finishing your custom inspector
            Revert();
        }

    }
}