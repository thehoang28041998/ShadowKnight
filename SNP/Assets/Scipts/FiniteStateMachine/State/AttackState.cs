using Scipts.EntityComponentSystem.Model;
using Scipts.FiniteStateMachine.Component;
using Scipts.FiniteStateMachine.Model;
using Scipts.UnityAnimation.Component;
using Scipts.UserInput.Component;

namespace Scipts.FiniteStateMachine.State {
    public struct AttackState : IState {
        private const float MAX_COMBO = 3;
        private const float SCALE = 0.75f;

        private readonly EntityManager entityManager;
        private readonly int entity;
        private int combo;
        private float elapsed;
        private float duration;
        private bool saveInputAttack;

        public AttackState(EntityManager entityManager, int entity) {
            this.entityManager = entityManager;
            this.entity = entity;
            this.combo = 1;
            this.elapsed = 0.0f;
            this.duration = 0.0f;
            this.saveInputAttack = false;
        }

        public StateName StateName {
            get => StateName.ATTACK;
        }

        public void Enter(StateName @from, bool isContinue) {
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
            // todo: start attack
            // todo: continue attack if from = Attack
        }

        public void Update(ref StateMachineComponent component, float dt) {
            elapsed += dt;

            if (elapsed >= duration * 0.8f && !saveInputAttack && entityManager.GetComponent<InputComponent>(entity).isAttack) {
                this.saveInputAttack = true;
            }

            if (elapsed >= duration) {
                ChangeStateMethod method = saveInputAttack ? ChangeStateMethod.Replace : ChangeStateMethod.GoBack;
                component.ListenChangeState(StateName.ATTACK, method);
            }
        }

        public void Exit() {
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