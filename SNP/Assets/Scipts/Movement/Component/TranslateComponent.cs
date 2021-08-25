using EntityComponentSystem.Model;
using UnityEngine;

namespace Movement.Component {
    public struct TranslateComponent : IComponent {
        public CharacterController Controller;
    }
}