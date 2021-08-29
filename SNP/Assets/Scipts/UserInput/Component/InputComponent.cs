using Scipts.UserInput.Model;
using UnityEngine;

namespace Scipts.UserInput.Component {
    public struct InputComponent : IComponent {
        public Vector3 direction;
        public bool isRunning;
        public bool isDash;
        public bool isAttack;
        public InputFrom inputFrom;

        public InputComponent(InputFrom inputFrom) {
            this.inputFrom = inputFrom;
            this.direction = Vector3.zero;
            this.isRunning = false;
            this.isDash = false;
            this.isAttack = false;
        }

        public void Reset() {
            direction = Vector3.zero;
            isDash = false;
            isRunning = false;
            isAttack = false;
        }
    }
}