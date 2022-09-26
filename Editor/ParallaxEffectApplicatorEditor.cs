using UnityEditor;
using UnityEngine;

namespace Sxm.ParallaxEffect
{
    [CustomEditor(typeof(ParallaxEffectApplicator))]
    public class ParallaxEffectApplicatorEditor : Editor
    {
        private ParallaxEffectApplicator _selfTarget;
    
        private void OnEnable()
        {
            _selfTarget = target as ParallaxEffectApplicator;
        }
    
        public override void OnInspectorGUI()
        {
            DrawScriptField();
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty(ParallaxEffectApplicator.TargetCameraName));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(ParallaxEffectApplicator.MotionSourceName));
    
            if (_selfTarget.SelectedMotionSource == ParallaxEffectApplicator.MotionSource.Self)
            {
                DrawSelfMotionSourceProperties();
            }
    
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty(ParallaxEffectApplicator.ForceRootLookAtCameraName));
            EditorGUILayout.PropertyField(serializedObject.FindProperty(ParallaxEffectApplicator.LayersRootName));
            
            EditorGUILayout.PropertyField(serializedObject.FindProperty(ParallaxEffectApplicator.ParallaxLayersName), true);
    
            serializedObject.ApplyModifiedProperties();
        }
    
        private void DrawScriptField()
        {
            GUI.enabled = false;
            {
                SerializedProperty property = serializedObject.FindProperty("m_Script");
                EditorGUILayout.PropertyField(property, true);
            }
            GUI.enabled = true;
    
            EditorGUILayout.Space();
        }
    
        private void DrawSelfMotionSourceProperties()
        {
            EditorGUI.indentLevel++;
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty(ParallaxEffectApplicator.SelfSpeedName));
                EditorGUILayout.PropertyField(serializedObject.FindProperty(ParallaxEffectApplicator.SelfDirectionName));
            }
            EditorGUI.indentLevel--;
        }
    }
}
