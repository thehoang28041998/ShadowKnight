using System;
using Movement.Model;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Movement.Component {
    public struct DashComponent : IComponent {
        public readonly float dashDistance;
        public readonly float dashDuration;
        public readonly Vector3 direction;

        /// <summary>
        /// only use for job
        /// </summary>
        public float previousDistance;
       
        /// <summary>
        /// only use for job
        /// </summary>
        public float elapsed;
        [SerializeField] private bool abort;

        public DashComponent(float dashDistance, float dashDuration, Vector3 direction) {
            this.dashDistance = dashDistance;
            this.dashDuration = dashDuration;
            this.direction = direction;
            this.abort = false;
            this.elapsed = 0.0f;
            this.previousDistance = 0.0f;
        }

        public bool IsFinish {
            get => abort || elapsed >= dashDuration;
        }
        
        public void Abort() {
            abort = true;
        }
    }
}