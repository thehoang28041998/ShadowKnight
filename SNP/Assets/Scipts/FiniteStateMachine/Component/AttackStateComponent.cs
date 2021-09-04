using Leopotam.EcsLite;
using Scipts.EntityComponentSystem;
using Scipts.FiniteStateMachine.Model;
using Scipts.UnityAnimation.Component;
using Scipts.UserInput.Component;
using UnityEngine;

namespace Scipts.FiniteStateMachine.Component {
    public struct AttackStateComponent : IComponent{
        private const float MAX_COMBO = 3;
        private const float SCALE = 0.75f;

        private EntityManager entityManager;
        private EcsPool<InputComponent> pool1;
        private EcsPool<AttackStateComponent> pool2;
        
        private int combo;
        private float duration;
        private bool saveInputAttack;
        private float elapsed;
        private bool isRunning;
        
        public AttackStateComponent(EntityManager entityManager) {
            this.elapsed = 0.0f;
            this.isRunning = false;
            this.combo = 1;
            this.duration = 0.0f;
            this.saveInputAttack = false;
            
            this.entityManager = entityManager;
            this.pool1 = entityManager.World.GetPool<InputComponent>();;
            this.pool2 = entityManager.World.GetPool<AttackStateComponent>();;
        }

        public void Enter(int entity, StateName previous, bool isContinue) {
            if (previous == StateName.ATTACK) {
                combo++;
                if (combo > MAX_COMBO) combo = 1;
            }
            else {
                combo = 1;
            }

            this.duration = GetAttackDuration(combo) / SCALE;
            this.isRunning = true;
            this.saveInputAttack = false;
            this.elapsed = 0.0f;
            this.entityManager.GetComponent<AnimationComponent>(entity).PlayAttack(combo, SCALE);
        }

        public void Run(ref StateMachineComponent stateMachine, int entity, float delta) {
            if (!isRunning) return;

            elapsed += delta;
            var input = pool1.Get(entity);
            var attack = pool2.Get(entity);

            if (elapsed >= duration * 0.8f && !attack.saveInputAttack && input.isAttack) {
                saveInputAttack = true;
            }

            if (elapsed >= duration) {
                var method = attack.saveInputAttack ? ChangeStateMethod.Replace : ChangeStateMethod.GoBack;
                stateMachine.ListenChangeState(StateName.ATTACK, method);
            }
        }

        public void Exit() {
            isRunning = false;
        }
        
        private float GetAttackDuration(int idx) {
            switch (idx) {
                case 1: return 0.43f;
                case 2: return 0.4f;
                case 3: return 0.66f;
            }

            return 0.0f;
        }
    }
}