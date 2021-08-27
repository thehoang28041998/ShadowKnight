using EntityComponentSystem.Model;
using UnityEngine;
using UserInput.Model;

namespace UserInput.Component {
    public struct InputComponent : IComponent {
        public Vector3 direction;
        public bool isRunning;
        public bool isDash;
        public InputFrom inputFrom;

        public InputComponent(InputFrom inputFrom) {
            this.inputFrom = inputFrom;
            this.direction = Vector3.zero;
            this.isRunning = false;
            this.isDash = false;
        }

        public void Reset() {
            direction = Vector3.zero;
            isDash = false;
            isRunning = false;
        }
    }
}