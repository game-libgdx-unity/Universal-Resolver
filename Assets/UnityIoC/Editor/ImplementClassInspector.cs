using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace UnityIoC.Editor
{
    [CustomEditor(typeof(ImplementClass))]
    public class ImplementClassInspector : BaseInspector
    {
        public override void OnInspectorGUI()
        {
            DrawList("Scene Setting", "Scene", ((ImplementClass) target).defaultSetting,
                data => { DrawObject("Abstract", ref data.AbstractType); }
            );
        }
    }
}