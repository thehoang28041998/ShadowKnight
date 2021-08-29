using UnityEngine;

namespace Scipts.Movement.Component {
    public struct VelocityComponent : IComponent {
        public Vector3 velocity { get; set; }
        public Vector3 saveVelocity; // must init data

        public void Init() {
            saveVelocity = Vector3.forward;
        }
    }
}