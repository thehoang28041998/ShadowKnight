using Scipts.EntityComponentSystem.Model;
using Scipts.FiniteStateMachine.Model;
using Scipts.UnityAnimation.Component;
using UnityEngine;

namespace Scipts.FiniteStateMachine.Component {
    public struct AttackStateComponent : IComponent{
        private const float MAX_COMBO = 3;
        private const float SCALE = 0.75f;
        private readonly int entity;

        public int combo { get; private set; }
        public float duration { get; private set; }
        public bool saveInputAttack;
        public float elapsed;
        public bool isRunning;

        public AttackStateComponent(int entity) {
            this.entity = entity;
            this.combo = 1;
            this.elapsed = 0.0f;
            this.duration = 0.0f;
            this.isRunning = false;
            this.saveInputAttack = false;
        }

        public void Enter(StateName @from, bool isContinue, EntityManager entityManager) {
            if (from == StateName.ATTACK) {
                this.combo++;
                if (combo > MAX_COMBO) {
                    this.combo = 1;
                }
            }
            else {
                this.combo = 1;
            }
            
            entityManager.GetComponent<AnimationComponent>(entity).PlayAttack(combo, SCALE);

            this.duration = GetDuration(combo) / SCALE;
            this.elapsed = 0.0f;
            this.saveInputAttack = false;
            this.isRunning = true;
        }

        public void Exit() {
            this.isRunning = false;
        }
        
        private float GetDuration(int idx) {
            switch (idx) {
                case 1: return 0.43f;
                case 2: return 0.4f;
                case 3: return 0.66f;
            }

            return 0.0f;
        }
    }
}