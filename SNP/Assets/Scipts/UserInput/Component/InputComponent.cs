using EntityComponentSystem.Model;
using UnityEngine;
using UserInput.Model;

namespace UserInput.Component {
    public struct InputComponent : IComponent{
        public Vector2 direction;
        public InputFrom inputFrom;
    }
}