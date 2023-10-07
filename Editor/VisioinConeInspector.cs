using UnityEngine;
using UnityEditor;

namespace VisionCones.Editor {
    
    [CustomEditor(typeof(VisionCone))]    
    public class VisioinConeInspector : UnityEditor.Editor {

        [DrawGizmo(GizmoType.Selected | GizmoType.Active | GizmoType.NonSelected)]
        static void DrawGizmoForMyScript(VisionCone scr, GizmoType gizmoType) {
            DrawVisionConeGizmo(scr);
        }
        
        private void OnSceneGUI() {
            VisionCone visionCone = (VisionCone)target;

            DrawVisionConeGizmo(visionCone);
        }

        private static void DrawVisionConeGizmo(VisionCone visionCone) {
            Handles.matrix = visionCone.transform.localToWorldMatrix;
            float halfHeight = 0.5f * visionCone.height;
            float halfAngle  = 0.5f * visionCone.angle;
            
            Vector3 leftAngle  = Quaternion.AngleAxis(halfAngle, Vector3.up) * Vector3.forward;
            Vector3 rightAngle = Quaternion.AngleAxis(-halfAngle, Vector3.up) * Vector3.forward;

            leftAngle  *= visionCone.radius;
            rightAngle *= visionCone.radius;

            // NOTE: Draw vertical lines
            Handles.DrawLine(Vector3.up * -halfHeight, Vector3.up * halfHeight);
            Handles.DrawLine(Vector3.up * -halfHeight + Vector3.forward * visionCone.radius,
                Vector3.up * halfHeight + Vector3.forward * visionCone.radius);
            Handles.DrawLine(Vector3.up * -halfHeight + leftAngle, Vector3.up * halfHeight + leftAngle);
            Handles.DrawLine(Vector3.up * -halfHeight + rightAngle, Vector3.up * halfHeight + rightAngle);

            // NOTE: Draw angle handles
            Handles.DrawLine(Vector3.up * -halfHeight, Vector3.up * -halfHeight + leftAngle);
            Handles.DrawLine(Vector3.up * halfHeight, Vector3.up * halfHeight + leftAngle);

            Handles.DrawLine(Vector3.up * -halfHeight, Vector3.up * -halfHeight + rightAngle);
            Handles.DrawLine(Vector3.up * halfHeight, Vector3.up * halfHeight + rightAngle);

            // NOTE: Draw arcs
            Handles.DrawWireArc(-Vector3.up * halfHeight, Vector3.up, rightAngle, visionCone.angle, visionCone.radius);
            Handles.DrawWireArc(Vector3.up * halfHeight, Vector3.up, rightAngle, visionCone.angle, visionCone.radius);
        }
    }
}