using EntityComponentSystem.Model;
using UnityEngine;

namespace Movement.Component {
    public struct VelocityComponent : IComponent{
        public Vector2 velocity;
    }
}