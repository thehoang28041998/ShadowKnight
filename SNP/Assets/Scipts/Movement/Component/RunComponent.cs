using UnityEngine;
#if UNITY_EDITOR

#endif

namespace Scipts.Movement.Component {
    public struct RunComponent : IComponent{
        private Vector3 direction;
        private bool finish;
        private bool abort;

        public RunComponent(Vector3 direction) {
            this.finish = false;
            this.abort = false;
            this.direction = direction;
        }

        public Vector3 GetDirection(float delta) {
            this.finish = true;
            return delta * 7.5f * direction;
        }

        public bool IsFinish {
            get => finish || abort;
        }

        public void Abort() {
            abort = true;
        }
    }
}