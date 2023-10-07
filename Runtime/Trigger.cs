using System.Collections.Generic;

using UnityEngine;

namespace VisionCones {
    /// <summary>
    /// A component used for visibility checks in vision cones. Use this component to make certain objects visible
    /// to the vision cones.
    /// </summary>
    public class Trigger : MonoBehaviour {

        /// <summary>
        /// Amount of vision cones that this trigger can be seen from.
        /// </summary>
        public bool IsVisible => triggeringVisionCones.Count > 0;
        
        /// <summary>
        /// The list of vision cones from which this trigger can be seen.
        /// </summary>
        public List<VisionCone> triggeringVisionCones = new List<VisionCone>();

#region Subsystem registration
        private void OnEnable() {
            VisionConeSystem.RegisterTrigger(this);
        }

        private void OnDisable() {
            VisionConeSystem.UnregisterTrigger(this);
        }
#endregion
    }
}