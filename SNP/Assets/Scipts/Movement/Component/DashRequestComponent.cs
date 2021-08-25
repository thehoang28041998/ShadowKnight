using System;
using Movement.Model;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Movement.Component {
    public struct DashRequestComponent : IRequest, IComponent, IComponentDebug {
        private float dashDistance;
        private float dashDuration;

        private float previousDistance;
        private float elapsed;
        private bool abort;

        public void SetDash(float dashDistance, float dashDuration) {
            this.dashDistance = dashDistance;
            this.dashDuration = dashDuration;
            this.abort = false;
            this.elapsed = 0.0f;
            this.previousDistance = 0.0f;
        }

        public Vector3 GetVelocity(float dt) {
            float progress = elapsed / dashDuration;
            progress = Math.Min(progress, 1f);

            float traveledDistance = calculate(0, dashDistance, progress);
            float deltaDistance = traveledDistance - previousDistance;
            previousDistance = traveledDistance;

            return Vector3.forward * deltaDistance;
            
        }

        private float calculate(float s, float e, float t) {
            return Mathf.Lerp(s, e, t);
        }

        public Reason Reason {
            get {
                if (abort) return Reason.Abort;
                return Reason.EndOfLifeCycle;
            }
        }

        public bool IsFinish {
            get => abort || elapsed >= dashDuration;
        }

        public void Update(float dt) {
            elapsed += dt;
        }

        public void Abort() {
            abort = true;
        }

#if UNITY_EDITOR
        public void OnGUI() {
            EditorGUILayout.LabelField($"{dashDuration}, {dashDuration}, {elapsed}");
        }
#endif
    }
}