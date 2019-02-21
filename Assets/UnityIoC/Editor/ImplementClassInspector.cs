using UnityEditor;
using UnityEngine;

namespace UnityIoC.Editor
{
    [CustomEditor(typeof(ImplementClass))]
    public class ImplementClassInspector : BaseInspector
    {
        private SerializedProperty defaultSettingProp;
        private SerializedProperty sceneSettingProp;

        private void OnEnable()
        {
            defaultSettingProp = serializedObject.FindProperty("defaultSettings");
            sceneSettingProp = serializedObject.FindProperty("sceneSettings");
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            //default binding setting
            DrawList("Default Setting", "", ((ImplementClass) target).defaultSettings,
                data =>
                {
                    EditorGUILayout.BeginHorizontal();

                    var maxWidth = 30.0f;
                    var objFieldMaxWidth = 60.0f;
                    var maxButtonWidth = 40.0f;

                    DrawLabel("From ", GUILayout.MaxWidth(35));

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

                    DrawLabel(" Bind ", GUILayout.MaxWidth(maxWidth));

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


                    DrawButton("X", () => { ((ImplementClass) target).defaultSettings.Remove(data); },
                        GUILayout.MaxWidth(17),
                        GUILayout.ExpandWidth(false));

                    DrawButton("C", () => { ((ImplementClass) target).defaultSettings.Add(data.Clone());},
                        GUILayout.MaxWidth(20),
                        GUILayout.ExpandWidth(false));
                    
                    DrawButtonToggle("", ref data.EnableInjectInto, GUILayout.MaxWidth(5));
                    DrawLabel("Inject", GUILayout.MaxWidth(50f));
                    
                    EditorGUILayout.EndHorizontal();

                    if (data.EnableInjectInto)
                    {
                        data.InjectIntoHolder = EditorGUILayout.ObjectField("Inject into component",
                            data.InjectIntoHolder, typeof(MonoScript), false);

                        if (data.InjectIntoHolder)
                        {
                            data.InjectInto = ((MonoScript) data.InjectIntoHolder).GetClass();
                        }
                    }


                    return data;
                }
                , true
            );

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}