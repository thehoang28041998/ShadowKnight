using System;
using EntityComponentSystem.Model;
using UnityEngine;

namespace Movement.Component {
    public struct TranslateComponent : IComponent {
        [SerializeField] private CharacterController controller;

        public TranslateComponent(CharacterController controller) {
            this.controller = controller;
        }

        public void Move(Vector3 direction) {
            controller.Move(direction);
        }

        public void Rotate(Vector3 direction) {
            if (Math.Abs(direction.x) <= 0 && Math.Abs(direction.z) <= 0) return;
            
            Quaternion inputRotation = Quaternion.LookRotation(direction, Vector3.up);
            controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, inputRotation, 500.0f);
        }
    }
}