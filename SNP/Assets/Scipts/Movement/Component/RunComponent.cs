using System;
using Movement.Model;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;    
#endif

namespace Movement.Component {
    public struct RunComponent : IComponent{
        public readonly Vector3 direction;
        
        /// <summary>
        /// only use for job
        /// </summary>
        public bool finish;
        
        [SerializeField] private bool abort;

        public RunComponent(Vector3 direction) {
            this.finish = false;
            this.abort = false;
            this.direction = direction;
        }

        public bool IsFinish {
            get => finish || abort;
        }

        public void Abort() {
            abort = true;
        }
    }
}