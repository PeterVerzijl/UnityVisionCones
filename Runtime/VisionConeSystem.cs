using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.LowLevel;

namespace VisionCones {
    
    /// <summary>
    /// Vision cone system updates all registered vision cones and checks if any trigger can be seen from them.
    /// </summary>
    public class VisionConeSystem {

        private static List<VisionCone> visionCones = new List<VisionCone>();
        private static List<Trigger> triggers = new List<Trigger>();

#region Registration functions
        /// <summary>
        /// Registers a trigger to be processed.
        /// NOTE: This is usually done in the trigger's OnEnable function.
        /// </summary>
        /// <param name="visionCone"></param>
        public static void RegisterTrigger(Trigger trigger) {
            if (!triggers.Contains(trigger)) {
                triggers.Add(trigger);
            }
        }
        
        /// <summary>
        /// Unregisters a trigger to be processed.
        /// NOTE: This is usually done in the trigger's OnDisable function.
        /// </summary>
        /// <param name="visionCone"></param>
        public static void UnregisterTrigger(Trigger trigger) {
            triggers.Remove(trigger);
        }
        
        /// <summary>
        /// Registers a vision cone to be processed.
        /// NOTE: This is usually done in the vision cone OnEnable function.
        /// </summary>
        /// <param name="visionCone"></param>
        public static void RegisterVisionCone(VisionCone visionCone) {
            if (!visionCones.Contains(visionCone)) {
                visionCones.Add(visionCone);
            }
        }
        
        /// <summary>
        /// Unregisters a vision cone from the processing list.
        /// NOTE: THis is usually done in the vision cone OnDisable function.
        /// </summary>
        /// <param name="visionCone"></param>
        public static void UnregisterVisionCone(VisionCone visionCone) {
            visionCones.Remove(visionCone);
        }
#endregion
        
        /// <summary>
        /// Functions is called once when the game starts, sets up registration of the VisionCone subsystem so this
        /// class's update function is called every frame without us having to create an invisible gameObject which
        /// isn't saved to the scene or the system breaking when we change scenes.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void OnStart() {
            PlayerLoopSystem playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            
            PlayerLoopSystem[] subSystemList = playerLoop.subSystemList;
            
            // Create subsystem for the vision cone and register it's Update function.
            PlayerLoopSystem visionConeSystem = new PlayerLoopSystem();
            visionConeSystem.updateDelegate = Update;
            visionConeSystem.type = typeof(VisionConeSystem);
            
            PlayerLoopSystem[] newSubSystemList = new PlayerLoopSystem[subSystemList.Length + 1];
            Array.Copy(subSystemList, newSubSystemList, subSystemList.Length);
            newSubSystemList[subSystemList.Length] = visionConeSystem;

            playerLoop.subSystemList = newSubSystemList;
            
            PlayerLoop.SetPlayerLoop(playerLoop);
        }

        /// <summary>
        /// Function called by the Unity player loop subsystem every engine update.
        /// Here we loop over vision cones and triggers and check if any trigger is visible from any vision cone.
        /// </summary>
        private static void Update() {
            for (int visionConeIndex = 0; visionConeIndex < visionCones.Count; ++visionConeIndex) {
                VisionCone visionCone = visionCones[visionConeIndex];
                for (int triggerIndex = 0; triggerIndex < triggers.Count; ++triggerIndex) {
                    Trigger trigger = triggers[triggerIndex];

                    Vector3 triggerRelativePosition = visionCone.transform.InverseTransformPoint(trigger.transform.position);
                    Vector3 triggerRelativePositionXZ =
                        new Vector3(triggerRelativePosition.x, 0, triggerRelativePosition.z);
                    
                    float halfHeight = visionCone.height * 0.5f;
                    float radiusSqrt = visionCone.radius * visionCone.radius;
                    float halfAngle  = visionCone.angle * 0.5f;

                    bool isVisible = triggerRelativePosition.sqrMagnitude < radiusSqrt &&
                                     Vector3.Angle(Vector3.forward, triggerRelativePositionXZ) <= halfAngle &&
                                     Mathf.Abs(triggerRelativePosition.y) <= halfHeight;

                    if (isVisible) {
                        if (!trigger.triggeringVisionCones.Contains(visionCone)) {
                            trigger.triggeringVisionCones.Add(visionCone);
                        }
                    } else {
                        trigger.triggeringVisionCones.Remove(visionCone);
                    }
                }
            }
        }
    }
}