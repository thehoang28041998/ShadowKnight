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
        private Vector3 direction;

        private float previousDistance;
        private float elapsed;
        private bool abort;

        public DashRequestComponent(float dashDistance, float dashDuration, Vector3 direction) {
            this.dashDistance = dashDistance;
            this.dashDuration = dashDuration;
            this.direction = direction.normalized;
            this.abort = false;
            this.elapsed = 0.0f;
            this.previousDistance = 0.0f;
        }

        public RequestType RequestType {
            get => RequestType.Dash;
        }

        public Vector3 GetVelocity(float dt) {
            float progress = elapsed / dashDuration;
            progress = Math.Min(progress, 1f);

            float traveledDistance = calculate(0, dashDistance, progress);
            float deltaDistance = traveledDistance - previousDistance;
            previousDistance = traveledDistance;

            return direction * deltaDistance;
            
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

        public float DashDuration {
            get => dashDuration;
        }

        public float DashDistance {
            get => dashDistance;
        }

        public Vector3 DashDirection {
            get => direction;
        }

#if UNITY_EDITOR
        public void OnGUI() {
            EditorGUILayout.LabelField($"{dashDuration}, {dashDuration}, {elapsed}");
        }
#endif
    }
}