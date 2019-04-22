//using UnityEditor;
//
//namespace UnityIoC.Editor
//{
//    [CustomEditor(typeof(InjectIntoBindingData))]
//    public class BindingDataInspector : BaseInspector
//    {
//        private SerializedProperty defaultSettingProp;
//        private SerializedProperty sceneSettingProp;
//
////        private void OnEnable()
////        {
////            defaultSettingProp = serializedObject.FindProperty("defaultSettings");
////            sceneSettingProp = serializedObject.FindProperty("sceneSettings");
////        }
//
////        public override void OnInspectorGUI()
////        {
////            EditorGUI.BeginChangeCheck();
////
////            DrawObject("Abstract", ref ((BindingData) target).AbstractTypeHolder);
////            DrawObject("Implement", ref ((BindingData) target).ImplementedTypeHolder);
////            DrawEnumPopup("Life cycle", ref ((BindingData) target).LifeCycle);
////            
////            if (EditorGUI.EndChangeCheck())
////            {
////                EditorUtility.SetDirty(target);
////                serializedObject.ApplyModifiedProperties();
////            }
////        }
//    }
//}