using Movement.Model;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;    
#endif

namespace Movement.Component {
    public struct RunRequestComponent : IComponent, IRequest, IComponentDebug {
        private bool finish;
        private bool abort;
        private Vector3 direction;

        public void SetDirection(Vector3 direction) {
            this.finish = false;
            this.abort = false;
            this.direction = direction;
        }

        public Vector3 GetVelocity(float dt) {
            return direction * 0.05f;
        }

        public Reason Reason {
            get {
                if (abort) return Reason.Abort;
                return Reason.EndOfLifeCycle;
            }
        }

        public bool IsFinish {
            get => finish || abort;
        }

        public void Update(float dt) {
            finish = true;
        }

        public void Abort() {
            abort = true;
        }

#if UNITY_EDITOR
        public void OnGUI() {
            EditorGUILayout.LabelField($"{direction}, {IsFinish}");
        }
#endif
    }
}