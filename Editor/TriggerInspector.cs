using System;
using System.Reflection;

using UnityEngine;
using UnityEngine.UIElements;

using UnityEditor;
using UnityEditor.UIElements;

namespace VisionCones.Editor {
    
    /// <summary>
    /// Draws the custom inspector for the tiggers
    /// </summary>
    [CustomEditor(typeof(Trigger))]
    public class TriggerInspector : UnityEditor.Editor {
        
        private SerializedProperty triggeringVisionConesProperty;
        
        private Toggle isTriggerSeenToggle;

        private static Texture2D triggerOffIcon;
        private static Texture2D triggerOnIcon;
        
        private void OnEnable() {
            // Load textures for on/off trigger state to be set as the gameObject icons
            triggerOffIcon = (Texture2D)EditorGUIUtility.Load("Packages/com.peterverzijl.visioncones/Icons/trigger-off.png");
            triggerOnIcon = (Texture2D)EditorGUIUtility.Load("Packages/com.peterverzijl.visioncones/Icons/trigger-on.png");
        }

        public override VisualElement CreateInspectorGUI() {
            // Create a new VisualElement to be the root of our inspector UI
            VisualElement myInspector = new VisualElement();

            triggeringVisionConesProperty = serializedObject.FindProperty("triggeringVisionCones");
            
            // Add UI elements 
            isTriggerSeenToggle = new Toggle("Is Visible");
            isTriggerSeenToggle.SetEnabled(false);
            myInspector.Add(isTriggerSeenToggle);
            
            myInspector.Add(new PropertyField(triggeringVisionConesProperty));

            // Return the finished inspector UI
            return myInspector;
        }

        private void OnSceneGUI() {
            // Update 'Is Visible' toggle
            bool isTriggerVisisble = triggeringVisionConesProperty.arraySize > 0;
            isTriggerSeenToggle.SetValueWithoutNotify(isTriggerVisisble);
        }

        [DrawGizmo(GizmoType.Selected | GizmoType.Active | GizmoType.NonSelected)]
        static void DrawTriggerGizmo(Trigger trigger, GizmoType gizmoType) {
            // Update gameObject icon based on the trigger state.
            EditorGUIUtility.SetIconForObject(trigger.gameObject, trigger.IsVisible ? triggerOnIcon : triggerOffIcon);
        }
    }
}