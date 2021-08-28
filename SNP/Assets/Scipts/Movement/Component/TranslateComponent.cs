using System;
using EntityComponentSystem.Model;
using UnityEngine;
using Utils;

namespace Movement.Component {
    public struct TranslateComponent : IComponent {
        [SerializeField] private readonly CharacterController controller;

        public TranslateComponent(CharacterController controller) {
            this.controller = controller;
        }

        public void Move(Vector3 direction) {
            controller.Move(direction);
        }

        public void Rotate(Vector3 direction) {
            if (direction == Vector3.zero) return;
            
            Quaternion inputRotation = Quaternion.LookRotation(direction, Vector3.up);
            controller.transform.rotation = Quaternion.Lerp(controller.transform.rotation, inputRotation, 0.3f);
        }
    }
}