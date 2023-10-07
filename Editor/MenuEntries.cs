using UnityEditor;
using UnityEngine;

namespace VisionCones.Editor
{
    /// <summary>
    /// Functions for creating the rightclick menu's in the hierarchy window
    /// </summary>
    public class MenuEntries
    {
        [MenuItem("GameObject/Vision Cones/Create Vision Cone")]
        private static void CreateVisionCone() {
            GameObject gameObject = new GameObject("Vision Cone", typeof(VisionCone));
        }
        
        [MenuItem("GameObject/Vision Cones/Create Vision Cone Trigger")]
        private static void CreateVisionTrigger() {
            GameObject gameObject = new GameObject("Vision Cone Trigger", typeof(Trigger));
        }
    }
}
