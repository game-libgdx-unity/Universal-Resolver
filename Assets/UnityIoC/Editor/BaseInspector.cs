using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace UnityIoC.Editor
{
    [CanEditMultipleObjects]
    public class BaseInspector : UnityEditor.Editor
    {
        protected Dictionary<GUIStyle, GUIStyle> customStyles = new Dictionary<GUIStyle, GUIStyle>();
        protected Dictionary<string, bool> foldOutFlags = new Dictionary<string, bool>();

        public override void OnInspectorGUI()
        {
            DrawFoldOut("Show Default Inspector", () => DrawDefaultInspector(), false);
            Revert();
        }
        
        protected virtual GUIStyle CustomizeStyle(GUIStyle editorStyle)
        {
            GUIStyle custom = new GUIStyle(editorStyle);
            customStyles.Add(editorStyle, custom);
            return custom;
        }
        protected virtual void Revert()
        {
            foreach (var style in customStyles.Keys)
            {
                RevertStyle(style, customStyles[style]);
            }
        }
        protected virtual void RevertStyle(GUIStyle target,GUIStyle backup)
        {
            //todo: add more properties as long as you need to change it in your custom inspector
            target.font = backup.font;
            target.fontSize = backup.fontSize;
            target.fontStyle = backup.fontStyle;
            target.fixedHeight = backup.fixedHeight;
            target.fixedWidth = backup.fixedWidth;
            target.margin = backup.margin;
            target.padding = backup.padding;
            target.alignment = backup.alignment;
            target.contentOffset = backup.contentOffset;
        }
        
    

        protected T DrawEnumPopup<T>(string label, ref T value, params GUILayoutOption[] options)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enumerated type");
            }

            var enumValue = (Enum) Enum.ToObject(typeof(T), value);
            value = (T) (object) EditorGUILayout.EnumPopup(label, enumValue, options);
            return value;
        }

        protected void DrawListObject<T>(string label, string elementName, List<T> objects) where T : UnityEngine.Object
        {
            int objCount = objects.Count;
            label = "No. " + label;
            //label = CheckInFoldout(label);
            DrawInt(label, ref objCount);

            if (objCount != objects.Count)
            {
                while (objects.Count < objCount) objects.Add(null);
                while (objects.Count > objCount) objects.RemoveAt(objects.Count - 1);
            }

            EditorGUILayout.BeginHorizontal();
            DrawButton("Clone", () =>
            {
                if (objCount > 0) objects.Add(objects[objects.Count - 1]);
                else objects.Add(null);
            });
            DrawButton("Remove First", () =>
            {
                if (objCount > 0) objects.RemoveAt(0);
            });
            DrawButton("Remove Last", () =>
            {
                if (objCount > 0) objects.RemoveAt(objects.Count - 1);
            });
            EndHorizontal();

            if (objects.Count > 0)
                DrawFoldOut(elementName, () =>
                {
                    for (int i = 0; i < objects.Count; i++)
                    {
                        objects[i] = (T) EditorGUILayout.ObjectField(elementName + i, objects[i], typeof(T), true);
                    }
                });
        }

        protected List<T> DrawList<T>(string label, string elementName, List<T> objects, Func<T, T> DrawElement,
            bool useFoldout = true)
            where T : new()
        {
            T defaultElement = new T();
            int objCount = objects.Count;

            BeginHorizontal();

            //label = CheckInFoldout(label);
            DrawInt(label, ref objCount);

            if (objCount != objects.Count)
            {
                while (objects.Count < objCount) objects.Add(defaultElement);
                while (objects.Count > objCount) objects.RemoveAt(objects.Count - 1);
            }


            var list = objects;
            DrawButton("Create", () =>
            {
                list.Add(defaultElement);
                foldOutFlags[label] = true;
            });

//            if (objCount > 0)
//            {
//                DrawButton("Clear", () =>
//                {
//                    if (objCount > 0) list.Clear();
//                    foldOutFlags[label] = true;
//                });
////                DrawButton("Remove last", () =>
////                {
////                    if (objCount > 0) list.RemoveAt(list.Count - 1);
////                    showFoldOutFlags[label] = true;
////                });
//            }

            EndHorizontal();
            try
            {
                if (objects.Count > 0)
                {
                    if (useFoldout)
                        DrawFoldOut(label, () =>
                        {
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (list[i] == null)
                                {
                                    list[i] = new T();
                                }

                                var item = DrawElement(list[i]);
                                if (item != null)
                                {
                                    list[i] = item;
                                }
                            }

                            objects = list;
                        }, false);
                    else
                    {
                        for (int i = 0; i < objects.Count; i++)
                        {
                            if (objects[i] == null)
                            {
                                objects[i] = new T();
                            }

                            objects[i] = 
                                DrawElement(objects[i]);
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Debug.Log(ex.Message);
            }

            return objects;
        }

        protected void DrawLabel(string content, params GUILayoutOption[] options)
        {
            EditorGUILayout.LabelField(content, options);
        }

        protected void DrawFoldOut(string label, Action draw, bool foldout = true)
        {
            if (!foldOutFlags.ContainsKey(label))
            {
                foldOutFlags.Add(label, foldout);
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(10));

            //default is fold out.
            if (!foldOutFlags.ContainsKey(label))
            {
                foldOutFlags[label] = true;
            }
            
            foldOutFlags[label] = EditorGUILayout.Foldout(foldOutFlags[label], label, true);
            EditorGUILayout.EndHorizontal();

            if (foldOutFlags[label]) draw();
        }


        protected void DrawDictionary<K, V>(string label, string elementName, Dictionary<K, V> objects,
            Action<K> DrawKeyElement, Action<V> DrawValueElement)
            where K : ICloneable<K>
        {
            K defaultElement = default(K);
            int objCount = objects.Count;
            //label = CheckInFoldout(label);
            DrawInt(label, ref objCount);

//            if (objCount != objects.Count)
//            {
//                while (objects.Count < objCount) objects.Add(defaultElement);
//                while (objects.Count > objCount) objects.RemoveAt(objects.Count - 1);
//            }
//
//            EditorGUILayout.BeginHorizontal();
//
//            DrawButton("Create", () => { objects.Add(defaultElement); });
//
//            if (objCount > 0)
//            {
//                DrawButton("Clone", () => { objects.Add(objects[objects.Count - 1].Clone()); });
//            }
//
//            DrawButton("Remove First", () =>
//            {
//                if (objCount > 0) objects.RemoveAt(0);
//            });
//            DrawButton("Remove Last", () =>
//            {
//                if (objCount > 0) objects.RemoveAt(objects.Count - 1);
//            });
//            EndHorizontal();
//
//            if (objects.Count > 0)
//                DrawFoldOut(label, () =>
//                {
//                    for (int i = 0; i < objects.Count; i++)
//                    {
//                        DrawElement(objects[i]);
//                    }
//                });
        }

        protected Transform DrawTransform(string label, UnityEngine.Transform transform)
        {
            label = CheckInFoldout(label);

            transform = (Transform) EditorGUILayout.ObjectField(label: label, obj: transform,
                objType: typeof(Transform), allowSceneObjects: true);
            return transform;
        }

        protected GameObject DrawGameObject(string label, UnityEngine.GameObject gameObject,
            params GUILayoutOption[] options)
        {
            label = CheckInFoldout(label);
            gameObject = (GameObject) EditorGUILayout.ObjectField(label: label, obj: gameObject,
                objType: typeof(GameObject), allowSceneObjects: true, options: options);
            return gameObject;
        }

        protected T DrawObject<T>(string label, ref T obj, params GUILayoutOption[] options)
            where T : UnityEngine.Object
        {
            label = CheckInFoldout(label);
            obj = (T) EditorGUILayout.ObjectField(label, obj, typeof(T), true, options);
            return obj;
        }

        protected T DrawGameObject<T>(string label, ref T gameObject, params GUILayoutOption[] options)
            where T : Component
        {
            label = CheckInFoldout(label);
            gameObject = (T) EditorGUILayout.ObjectField(label: label, obj: gameObject, objType: typeof(T),
                allowSceneObjects: true, options: options);
            return gameObject;
        }

        protected void DrawGO(string label, ref UnityEngine.Object gameObject, params GUILayoutOption[] options)
        {
            label = CheckInFoldout(label);
            gameObject = (UnityEngine.Object) EditorGUILayout.ObjectField(label: label, obj: gameObject,
                objType: typeof(UnityEngine.Object), allowSceneObjects: true, options: options);
        }


        protected void DrawButton(string label, System.Action onClicked, params GUILayoutOption[] options)
        {
            if (GUILayout.Button(label, options: options))
            {
                onClicked();
            }
        }

        protected void DrawTextfield(string label, ref string unitName, params GUILayoutOption[] options)
        {
            label = CheckInFoldout(label);
            unitName = EditorGUILayout.TextField(label, unitName, options: options);
        }

        static List<string> layers;
        static string[] layerNames;

        protected void DrawMask(string label, ref LayerMask mask, params GUILayoutOption[] options)
        {
            label = CheckInFoldout(label);
            if (layers == null)
            {
                layers = new List<string>();
                layerNames = new string[4];
            }
            else
            {
                layers.Clear();
            }

            int emptyLayers = 0;
            for (int i = 0; i < 32; i++)
            {
                string layerName = LayerMask.LayerToName(i);

                if (layerName != "")
                {
                    for (; emptyLayers > 0; emptyLayers--) layers.Add("Layer " + (i - emptyLayers));
                    layers.Add(layerName);
                }
                else
                {
                    emptyLayers++;
                }
            }

            if (layerNames.Length != layers.Count)
            {
                layerNames = new string[layers.Count];
            }

            for (int i = 0; i < layerNames.Length; i++) layerNames[i] = layers[i];
            mask.value = EditorGUILayout.MaskField(label, mask.value, layerNames, options: options);
        }

        protected void EndHorizontal()
        {
            EditorGUILayout.EndHorizontal();
        }

        protected LayerMask DrawLayer(string label, ref LayerMask customLayer)
        {
            label = CheckInFoldout(label);
            customLayer.value = EditorGUILayout.LayerField(label, customLayer.value);
            return customLayer;
        }

        private string CheckInFoldout(string label)
        {
            return label;
        }


        protected void BeginVertical()
        {
            EditorGUILayout.BeginVertical();
        }

        protected void EndVertical()
        {
            EditorGUILayout.EndVertical();
        }

        protected void BeginHorizontal()
        {
            EditorGUILayout.BeginHorizontal();
        }

        protected void Separator()
        {
            EditorGUILayout.Separator();
        }

        protected void DrawLayer2(string label, ref LayerMask customMask, params GUILayoutOption[] options)
        {
            label = CheckInFoldout(label);
            customMask = EditorGUILayout.LayerField(label, customMask.value, options);
        }

        protected int DrawLayer(string label, int customMask, params GUILayoutOption[] options)
        {
            label = CheckInFoldout(label);
            customMask = EditorGUILayout.LayerField(label, customMask, options);
            return customMask;
        }

        protected bool DrawButtonToggle(string label, ref bool value, params GUILayoutOption[] options)
        {
            label = CheckInFoldout(label);
            value = EditorGUILayout.Toggle(label, value, options);
            return value;
        }

        protected void DrawFloat(string label, ref float value, params GUILayoutOption[] options)
        {
            label = CheckInFoldout(label);
            value = EditorGUILayout.FloatField(label, value, options);
        }

        protected int DrawInt(string label, ref int value, params GUILayoutOption[] options)
        {
            label = CheckInFoldout(label);
            value = EditorGUILayout.IntField(label, value, options);
            return value;
        }

        protected void DrawSlider(string label, ref float value, float min, float max, params GUILayoutOption[] options)
        {
            label = CheckInFoldout(label);
            value = EditorGUILayout.Slider(label, value, min, max, options);
        }
    }
}