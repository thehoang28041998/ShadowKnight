using UnityEngine;

namespace Scipts.Movement.Component {
    public struct DashComponent : IComponent {
        private float dashDistance;
        private float dashDuration;
        private Vector3 direction;
        private float previousDistance;
        private float elapsed;
        private bool abort;

        public DashComponent(float dashDistance, float dashDuration, Vector3 direction) {
            this.dashDistance = dashDistance;
            this.dashDuration = dashDuration;
            this.direction = direction;
            this.abort = false;
            this.elapsed = 0.0f;
            this.previousDistance = 0.0f;
        }

        public Vector3 GetDirection(float delta) {
            elapsed += delta;
            
            float progress = elapsed / dashDuration;
            progress = Mathf.Min(progress, 1.0f);
            
            float traveledDistance = calculate(0, dashDistance, progress);
            float deltaDistance = traveledDistance - previousDistance;
            previousDistance = traveledDistance;

            return direction * deltaDistance;
        }

        public bool IsFinish {
            get => abort || elapsed >= dashDuration;
        }

        public void Abort() {
            abort = true;
        }
        
        private float calculate(float s, float e, float t) {
            return Mathf.Lerp(s, e, t);
        }
    }
}