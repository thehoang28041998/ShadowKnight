using EntityComponentSystem.Model;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Movement.Component {
    public struct VelocityComponent : IComponent {
        public Vector3 velocity { get; set; }
        public Vector3 saveVelocity;
    }
}