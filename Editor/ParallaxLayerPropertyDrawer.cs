using UnityEditor;
using UnityEngine;

namespace Sxm.ParallaxEffect
{
    [CustomPropertyDrawer(typeof(ParallaxLayer))]
    public class ParallaxLayerPropertyDrawer : PropertyDrawer
    {
        private const int CountProperties = 2;
        private const float Padding = 5f;
        private const float Offset = 2.5f;
    
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return CountProperties * EditorGUIUtility.singleLineHeight * (EditorGUIUtility.wideMode ? 1 : 2) +
                   2f * Padding + Offset;
        }
    
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            {
                float propertyHeight = rect.size.y / CountProperties - Padding;
            
                Rect firstPropertyRect = new Rect(new Vector2(
                    x: rect.position.x,
                    y: rect.position.y + Padding
                ), new Vector2(rect.size.x, propertyHeight));
                Rect secondPropertyRect = new Rect(new Vector2(
                    x: rect.position.x,
                    y: rect.position.y + Padding + propertyHeight + Offset
                ), new Vector2(rect.size.x, propertyHeight));
            
                EditorGUI.ObjectField(firstPropertyRect, property.FindPropertyRelative(ParallaxLayer.RectTransformName));
                EditorGUI.PropertyField(secondPropertyRect, property.FindPropertyRelative(ParallaxLayer.ParallaxWeight));
            }
            EditorGUI.EndProperty();
        }
    }
}
