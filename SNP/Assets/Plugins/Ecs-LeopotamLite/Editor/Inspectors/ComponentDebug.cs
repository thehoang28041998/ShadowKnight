using System;
using UnityEditor;
using UnityEngine;

namespace Leopotam.EcsLite.UnityEditor.Inspectors {
    public class ComponentDebug : IEcsComponentInspector{
        public Type GetFieldType() {
            return typeof(IComponentDebug);
        }

        public void OnGUI (string label, object value, EcsWorld world, int entityId) {
            GUIStyle gs = new GUIStyle(EditorStyles.popup);
            gs.fontSize = 17;
			
            EditorGUILayout.LabelField(label, gs);
            EditorGUILayout.BeginVertical("window");

            EditorGUI.indentLevel += 2;
            ((IComponentDebug) value).OnGUI();
            EditorGUI.indentLevel -= 2;
			
            EditorGUILayout.EndHorizontal();
        }
    }
}