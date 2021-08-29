using Scipts.EntityComponentSystem.Model;
using Scipts.FiniteStateMachine.Component;
using Scipts.FiniteStateMachine.Model;
using Scipts.UnityAnimation.Component;
using Scipts.UserInput.Component;

namespace Scipts.FiniteStateMachine.State {
    public struct IdleState : IState {
        private const float TIME_REQUIREMENT_CAN_CHANGE_RUN_STATE = 0.1f;
        private EntityManager entityManager;
        private int entity;
        private float elaped;

        public IdleState(EntityManager entityManager, int entity) {
            this.entityManager = entityManager;
            this.entity = entity;
            this.elaped = 0.0f;
        }

        public StateName StateName {
            get => StateName.IDLE;
        }

        public void Enter(StateName @from, bool isContinue) {
            // todo: play idle animation in here
            this.elaped = 0.0f;
            entityManager.GetComponent<AnimationComponent>(entity).PlayIdle();
        }

        public void Update(ref StateMachineComponent component, float dt) {
            elaped += dt;
            ref var inputComponent = ref entityManager.GetComponent<InputComponent>(entity);

            if (inputComponent.isAttack) {
                component.ListenChangeState(StateName.ATTACK, ChangeStateMethod.Backup);
                return;
            }

            if (inputComponent.isDash) {
                // change state to the dash state
                component.ListenChangeState(StateName.DASH, ChangeStateMethod.Backup);
                return;
            }

            if (inputComponent.isRunning && elaped >= TIME_REQUIREMENT_CAN_CHANGE_RUN_STATE) {
                // change state to the run state
                component.ListenChangeState(StateName.RUN, ChangeStateMethod.Backup);
                return;
            }
        }

        public void Exit() {
            // nothing
        }
    }
}