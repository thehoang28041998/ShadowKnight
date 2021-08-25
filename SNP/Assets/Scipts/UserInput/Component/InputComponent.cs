using EntityComponentSystem.Model;
using UnityEngine;
using UserInput.Model;

namespace UserInput.Component {
    public struct InputComponent : IComponent {
        public Vector3 direction;
        public bool isRunning;
        public InputFrom inputFrom;
    }
}