using UnityEngine;

namespace VisionCones {
    
    /// <summary>
    /// Vision cone class which just holds data and registers itself to the vision cone subsystem for processing.
    /// </summary>
    public class VisionCone : MonoBehaviour {
        /// <summary>
        /// The radius of the vision cone in meters.
        /// </summary>
        public float radius = 3f;
        /// <summary>
        /// The angle of the vision cones vision range in degrees
        /// </summary>
        [Tooltip("Full width of the vision cone in degrees.")]
        public float angle  = 70f;
        /// <summary>
        /// The vertical range of the vision cone
        /// </summary>
        public float height = 1f;

#region Subsystem registration
        private void OnEnable() {
            VisionConeSystem.RegisterVisionCone(this);
        }

        private void OnDisable() {
            VisionConeSystem.UnregisterVisionCone(this);
        }
#endregion

#if UNITY_EDITOR
        private void OnValidate() {
            radius = Mathf.Max(0.01f, radius);
            height = Mathf.Max(0.01f, height);
            angle = Mathf.Clamp(angle, 1, 180);
        }
#endif
    }
}